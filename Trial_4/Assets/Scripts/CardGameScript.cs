using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardGameScript : GameGenericMBScript<CardScript>, YesOrNoInterface
{
    [Header("2. Properties of this particular Game.")]

    [SerializeField]
    CardScript _cardSample;

    [SerializeField]
    float _cardDistanceConstant = 1.0f;

    [SerializeField]
    int _cardsInARow = 4;

    [SerializeField]
    Vector2 _cardDistance;

    [SerializeField]
    float _cardFlipAnimationSpeed = 5.0f;

    [SerializeField]
    TextPropertiesClass _matchP;

    [SerializeField]
    TextPropertiesClass _noMatchP;

    [SerializeField]
    TextPropertiesClass _infoP;

    int _numberOfCards = 12;

    List<DefinitionClass> _selectedWords;

    CardScript _selectedCard1;

    CardScript _selectedCard2;

    EvaluateCardsEnum _evaluateCards;

    Coroutine _evalCoroutine;

    Coroutine _newTextC;

    bool _evalInProcess = false;

    public EvaluateCardsEnum CardEvaluationStatus
    {
        get
        {
            return _evaluateCards;
        }
    }

    public bool EvalInProcess
    {
        get
        {
            return _evalInProcess;
        }
    }

    InfoCategoryEnum _selectedCategory = InfoCategoryEnum.None;

    public InfoCategoryEnum SelectedCategory
    {
        get
        {
            return _selectedCategory;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        OnEnabledFunction();
    }

    private void OnEnable()
    {
        OnEnabledFunction();
    }

    // Update is called once per frame
    void Update()
    {
        if(_evaluateCards != EvaluateCardsEnum.None && !_evalInProcess)
        {
            _evalCoroutine = StartCoroutine(MatchCardsFunction());

            _evalInProcess = true;
        }

        _completionMeter = _gameProperties.GetMeter();

        _gameProperties.UpdateUI();

        if(_gameProperties.GetMeter().GetPercentage() == 100.0f && !_gameDone && _gameProperties.GetResponseText().text == "")
        {
            WinGame();
        }
    }

    public int GetNumberOfCards()
    {
        return _numberOfCards;
    }

    public List <DefinitionClass> GetSelectedWords()
    {
        return _selectedWords;
    }

    public void SetNumebrOfCards(int _input)
    {
        _numberOfCards = _input;
    }

    public override void StartGame()
    {
        StartGame(0);
    }

    public override void StartGame(int _indexInput = 0)
    {
        if(BookScript.GetInstance() == null || _cardSample == null || GetGameInSession() || _gameSpace == null)
        {
            return;
        }

        Debug.Log("Card game is starting...");

        base.StartGame();

        //base.StartGameFunction();

        _evalInProcess = false;

        int _infoCardsNeeded = 1;

        if(_numberOfCards % 2 == 0)
        {
            _infoCardsNeeded = 2;
        }

        int _selectionDone = 0;

        int _inf = 0;

        bool _wordSelectedInLoop = false;

        Vector3 _pos = Vector3.zero;

        List<int> _infoIndexList = new List<int>();

        int _i = 0;

        if(_evalCoroutine != null)
        {
            StopCoroutine(_evalCoroutine);
        }

        if(_newTextC != null)
        {
            StopCoroutine(_newTextC);
        }

        if(_gameProperties.GetResponseText() != null)
        {
            _gameProperties.GetResponseText().text = "";
        }

        GenerateRandomIndexForList(ref _infoIndexList, 2);

        SetSelectedCategory(_indexInput);

        List<DefinitionClass> _defList = (_selectedCategory == InfoCategoryEnum.None) ? BookScript.GetInstance().GetDefinitions() : BookScript.GetInstance().GetDefinitionsByCategory(_selectedCategory);

        //List<InformationClass> _infoList = (_selectedCategory == InfoCategoryEnum.None) ? BookScript.GetInstance().GetGeneralInformation() : BookScript.GetInstance().GetGeneralInformationByCategory(_selectedCategory);

        _evaluateCards = EvaluateCardsEnum.None;

        while(_selectionDone < _numberOfCards)
        {
            Debug.Log("We are in the process of loop 1.");

            if(_inf < _infoCardsNeeded)
            {
                GameObject _cgo = Instantiate(_cardSample.gameObject);

                InstantiateInfoCardFunction(ref _cgo, ref _pos, ref _infoIndexList);

                _inf++;

                _selectionDone++;

                Debug.Log("The info condition is complete.");

                continue;
            }

            while(!_wordSelectedInLoop)
            {
                int _s = UnityEngine.Random.Range(0, _defList.Count);

                DefinitionClass _currentWord = _defList[_s];

                if (!_selectedWords.Contains(_currentWord) && _currentWord.GetSprite() != null)
                {
                    if (_selectedCategory == InfoCategoryEnum.None || _selectedCategory != InfoCategoryEnum.None && _currentWord.GetInfoCategory() == _selectedCategory)
                    {
                        InstantiateNormalCardsFunction(ref _currentWord, ref _pos, ref _i);

                        _i++;

                        _selectionDone = _selectionDone + 2;

                        _wordSelectedInLoop = true;
                    }
                }
            }

            Debug.Log("We are finding a word.");

            _wordSelectedInLoop = false;
        }

        ShuffleFunction();

        ReorganizeObjects();

        _gameProperties.StartGame(0, _numberOfCards);

        _gameProperties.SignalToUpdateUI();

        _gameProperties.SetBadge("Cards' Badge");
    }

    void ReorganizeObjects()
    {
        List<GameObject> _currentObjects = new List<GameObject>(_gameProperties.GetListOfObjectsAsGO());

        //List<GameObject> _newList = new List<GameObject>();

        GameObject _currentObj;

        Vector3 _posAdd = Vector3.zero;

        Vector3 _pos = Vector3.zero;

        bool _takePos = false;

        for(int _i = (_currentObjects.Count - 1); _i >= 0; _i--)
        {
            _currentObj = _currentObjects[_i];

            _currentObj.transform.SetSiblingIndex(0);

            _currentObj.name = "Card No. " + (_i + 1);

            if(!_takePos)
            {
                _posAdd = _currentObj.transform.localPosition / -2.0f;

                _posAdd.y = 0.0f;

                _takePos = true;
            }

            _pos = _currentObj.transform.localPosition;

            _pos = _pos + _posAdd;

            _currentObj.transform.localPosition = _pos;
        }
    }

    public List<CardScript> GetDoneCards()
    {
        List<CardScript> _list = new List<CardScript>();

        foreach(CardScript _card in _gameProperties.GetListOfObjects())
        {
            if(_card.GetCardDone())
            {
                _list.Add(_card);
            }
        }

        return _list;
    }

    public CardScript GetSelectedCard1()
    { return _selectedCard1; }

    public CardScript SelectedCard2()
    { return _selectedCard2; }

    public Text GetResponseText()
    {
        return _gameProperties.GetResponseText();
    }

    public void SetSelectedCard(CardScript _input)
    {
        if(_input == null)
        {
            return;
        }

        if(_input.GetIsInfoCard())
        {
            _evaluateCards = EvaluateCardsEnum.ShowInfoCard;
        }

        if(_selectedCard1 == null)
        {
            _selectedCard1 = _input;

            _selectedCard1.SetCardFlipped(true);

            StartCoroutine(FlipUpCardAnimation(_input));
        }
        else if(_selectedCard1 == _input && !_selectedCard1.GetIsInfoCard())
        {
            _selectedCard1.SetCardFlipped(false);

            _selectedCard1 = null;

            StartCoroutine(FlipDownCardAnimation(_input));
        }
        else if(_selectedCard2 == null)
        {
            _selectedCard2 = _input;

            _selectedCard2.SetCardFlipped(true);

            if (_evaluateCards != EvaluateCardsEnum.ShowInfoCard)
            {
                _evaluateCards = EvaluateCardsEnum.MatchCards;
            }

            StartCoroutine(FlipUpCardAnimation(_input));
        }
    }

    IEnumerator MatchCardsFunction()
    {
        if((_selectedCard1 == null || _selectedCard2 == null) && _evaluateCards != EvaluateCardsEnum.ShowInfoCard)
        {
            yield return new WaitForSeconds(0.1f);

            _evaluateCards = EvaluateCardsEnum.None;

            _evalInProcess = false;
             
            yield break;
        }

        yield return new WaitForSeconds(2.0f);

        if(_evaluateCards == EvaluateCardsEnum.ShowInfoCard)
        {
            string _infoText = "";

            if(_selectedCard1.GetIsInfoCard())
            {
                _infoText = _selectedCard1.GetCardDescription();

                _selectedCard1.SetCardDone(true);
            }
            else if(_selectedCard2.GetIsInfoCard())
            {
                _infoText = _selectedCard2.GetCardDescription();

                _selectedCard2.SetCardDone(true);

                _selectedCard1.SetCardFlipped(false);

                StartCoroutine(FlipDownCardAnimation(_selectedCard1));
            }

            _gameProperties.GetMeter().AddToValue(1);

            _gameProperties.SignalToUpdateUI();

            if(_procedureStateMachine != null)
            {
                if(_procedureStateMachine.GetMachineOn() && _procedureStateMachine.GetProcedureCanvas() != null && _procedureStateMachine.GetCardGame() == this)
                {
                    if(_procedureStateMachine.GetProcedureCanvas().gameObject.activeSelf)
                    {
                        _procedureStateMachine.GetProcedureCompletionMeter().AddToMaxValue(1);

                        _procedureStateMachine.SignalToUpdateMeter();
                    }
                }
            }

            bool _finalCard = !_gameDone && _gameProperties.GetMeter().GetPercentage() == 100.0f;

            string _textToDisplay = _infoP.GetText() + "Fun Fact:\n\n" + _infoText;

            SetResponseText(_textToDisplay, _infoP.GetTextColor(), _infoP.GetTextTimeToDisplay(), _finalCard);

            if(_finalCard)
            {
                _gameProperties.GetGameCanvas().gameObject.SetActive(false);
            }
        }
        else if(CardsMatch() && _evaluateCards == EvaluateCardsEnum.MatchCards)
        {
            _selectedCard1.SetCardDone(true);

            _selectedCard2.SetCardDone(true);

            _gameProperties.GetMeter().AddToValue(2);

            _gameProperties.SignalToUpdateUI();

            if (_gameProperties.GetMeter().GetPercentage() < 100.0f)
            {
                SetResponseText(_matchP.GetText(), _matchP.GetTextColor(), _matchP.GetTextTimeToDisplay());
            }
        }
        else if(_evaluateCards == EvaluateCardsEnum.MatchCards)
        {
            _selectedCard1.SetCardFlipped(false);

            _selectedCard2.SetCardFlipped(false);

            SetResponseText(_noMatchP.GetText(), _noMatchP.GetTextColor(), _noMatchP.GetTextTimeToDisplay());

            StartCoroutine(FlipDownCardAnimation(_selectedCard1));

            StartCoroutine(FlipDownCardAnimation(_selectedCard2));
        }

        _selectedCard1 = null;

        _selectedCard2 = null;

        _evaluateCards = EvaluateCardsEnum.None;

        _evalInProcess = false;
    }

    protected override void ShuffleFunction()
    {
        List<CardScript> _shuffledGroup = new List<CardScript>();

        List<GameObject> _goList = new List<GameObject>();

        List<int> _indexes = new List<int>();

        List<Vector3> _positions = new List<Vector3>();

        for(int _i = 0; _i < _gameProperties.GetListOfObjects().Count; _i++)
        {
            _indexes.Add(_i);

            _positions.Add(_gameProperties.GetListOfObjects()[_i].GetOriginalPosition());

            Debug.Log("We are preparing to suffle the cards.");
        }

        for(int _j = 0; _j < _gameProperties.GetListOfObjects().Count; _j++)
        {
            int _r = UnityEngine.Random.Range(0, _indexes.Count);

            int _selectedIndex = _indexes[_r];

            CardScript _selectedCard = _gameProperties.GetListOfObjects()[_selectedIndex];

            _shuffledGroup.Add(_selectedCard);

            _goList.Add(_selectedCard.gameObject);

            _selectedCard.SetOriginalPosition(_positions[_j]);

            Debug.Log("We are shuffling the cards.");

            _indexes.RemoveAt(_r);
        }

        foreach(CardScript _c in _shuffledGroup)
        {
            _c.gameObject.transform.localPosition = _c.GetOriginalPosition();
        }

        _gameProperties.SetListOfObjects(_shuffledGroup);

        _gameProperties.SetListOfObjectsAsGO(_goList);
    }

    void GenerateRandomIndexForList(ref List<int> _listInput, int _counterInput)
    {
        if(BookScript.GetInstance() == null)
        {
            return;
        }

        int _i = 0;

        while(_i < _counterInput)
        {
            int _j = UnityEngine.Random.Range(0, BookScript.GetInstance().GetGeneralInformation().Count);

            if(!_listInput.Contains(_j))
            {
                _listInput.Add(_j);

                _i++;
            }
        }
    }

    void SetCardMaterial(CardScript _cardInput, Sprite _spriteInput)
    {
        if(_cardInput.GetRenderer() == null || _spriteInput == null)
        {
            return;
        }

        if (_cardInput.GetRenderer().materials[0] == null)
        {
            return;
        }

        _cardInput.GetRenderer().materials[0].SetTexture("_Front_Side", _spriteInput.texture);
    }

    void InstantiateInfoCardFunction(ref GameObject _cardGOInput, ref Vector3 _posInput, ref List<int> _listInput)
    {
        CardScript _infoCard = _cardGOInput.GetComponent<CardScript>();

        SetCardPositionAndProperty(ref _cardGOInput, ref _posInput);

        if(_gameProperties.GetResponseText() != null)
        {
            _infoCard._text = _gameProperties.GetResponseText();
        }

        _infoCard.SetIsInfoCard(true);

        InformationClass _info = BookScript.GetInstance().GetGeneralInformation()[0];

        _infoCard.SetCardName(_info.GetInformationName());

        _infoCard.SetCardDescription(_info.GetInformationDescription());

        _listInput.RemoveAt(0);

        SetCardMaterial(_infoCard, BookScript.GetInstance().GetInfoCardFrontMaterial());
    }

    void InstantiateNormalCardsFunction(ref DefinitionClass _wordInput, ref Vector3 _posInput, ref int _numberInput)
    {
        _selectedWords.Add(_wordInput);

        GameObject _cgo1 = Instantiate(_cardSample.gameObject);

        CardScript _c1 = _cgo1.GetComponent<CardScript>();

        SetCardPositionAndProperty(ref _cgo1, ref _posInput);

        GameObject _cgo2 = Instantiate(_cardSample.gameObject);

        CardScript _c2 = _cgo2.GetComponent<CardScript>();

        SetCardPositionAndProperty(ref _cgo2, ref _posInput);

        _c1.SetCardName(_wordInput.GetInformationName());

        _c2.SetCardName(_wordInput.GetInformationName());

        _c1.SetCardDescription(_wordInput.GetInformationDescription());

        _c2.SetCardDescription(_wordInput.GetInformationDescription());

        _c1.SetCardNumber(_numberInput);

        _c2.SetCardNumber(_numberInput);

        if (_gameProperties.GetResponseText() != null) {

            _c1._text = _gameProperties.GetResponseText();

            _c2._text = _gameProperties.GetResponseText();
        }

        SetCardMaterial(_c1, _wordInput.GetSprite());

        SetCardMaterial(_c2, _wordInput.GetSprite());
    }

    void SetResponseText(string _textInput, Color _colorInput, float _secondsToDisplayInput = 5.0f, bool _endGameInput = false)
    {
        if (_gameProperties.GetResponseText() == null)
        {
            return;            
        }

        if(_newTextC != null)
        {
            StopCoroutine(_newTextC);

            _gameProperties.GetResponseText().text = "";
        }

        _newTextC = StartCoroutine(SetResponseTextC(_textInput, _colorInput, _secondsToDisplayInput, _endGameInput));
    }

    IEnumerator SetResponseTextC(string _textInput, Color _colorInput, float _secondsToDisplayInput = 5.0f, bool _endGameInput = false)
    {
        _gameProperties.GetResponseText().text = _textInput;

        _gameProperties.GetResponseText().color = _colorInput;

        _gameProperties.GetResponseText().gameObject.GetComponent<Outline>().effectColor = ToolsStruct.ChangeColorValue(_colorInput, 0.5f, 0.5f);

        yield return new WaitForSeconds(_secondsToDisplayInput);

        if(_endGameInput)
        {
            WinGame();
        }

        _gameProperties.GetResponseText().text = "";
    }

    protected void OnEnabledFunction()
    {
        if(_selectedWords == null)
        {
            _selectedWords = new List<DefinitionClass>();
        }
    }

    bool CardsMatch()
    {
        if(_selectedCard1 == null || _selectedCard2 == null)
        {
            return false;
        }

        bool _finalBool = _selectedCard1.GetCardNumber() == _selectedCard2.GetCardNumber();

        return _finalBool;
    }

    public void FinishGame()
    {

        var _instance = BadgesManagerScript.GetInstance();

        _selectedCategory = InfoCategoryEnum.None;

        if(_instance.GetBadgeByName("Cards' Badge") == null)
        {
            return;
        }

        var _badge = _instance.GetBadgeByName("Cards' Badge");
    }

    public void SetSelectedCategory(InfoCategoryEnum _input)
    {
        _selectedCategory = _input;
    }

    public void SetSelectedCategory(int _input)
    {
        InfoCategoryEnum _var;

        int _l = Enum.GetValues(typeof(InfoCategoryEnum)).Length;

        int _i = _input == -1 ? UnityEngine.Random.Range(0, (_l - 1)) : _input;

        switch(_i)
        {
            case 1:
                _var = InfoCategoryEnum.Trigger;
                break;
            case 2:
                _var = InfoCategoryEnum.Symptom;
                break;
            case 3:
                _var = InfoCategoryEnum.Treatment;
                break;
            default:
                _var = InfoCategoryEnum.None;
                break;
        }

        SetSelectedCategory(_var);
    }

    public override void StopGame()
    {
        base.StopGame();

        _gameProperties.GetMainCanvases().SetCanvasesOn(true);

        _selectedWords.Clear();


        if (_gameProperties.GetGameCanvas() != null)
        {
            _gameProperties.GetGameCanvas().gameObject.SetActive(false);
        }

        if (_gameProperties.GetGameIndicatorCanvas() != null)
        {
            _gameProperties.GetGameIndicatorCanvas().gameObject.SetActive(false);
        }
    }

    protected override void WinGame()
    {
        base.WinGame();

        _selectedWords.Clear();
    }

    void SetCardPositionAndProperty(ref GameObject _cardObjectInput, ref Vector3 _posInput)
    {
        if(_cardObjectInput.GetComponent<CardScript>() == null)
        {
            return;
        }

        CardScript _cardProperty = _cardObjectInput.GetComponent<CardScript>();

        _cardObjectInput.transform.parent = _gameSpace.transform;

        _cardObjectInput.transform.localPosition = _posInput;

        _cardObjectInput.transform.localRotation = Quaternion.Euler(-90.0f, 90.0f, 0.0f);

        _cardProperty.SetGroup(this);

        _cardProperty.SetOriginalPosition(_posInput);

        _cardProperty.SetCamera(_gameProperties.GetCamera());

        _posInput.z = _posInput.z + (_cardDistance.x * _cardDistanceConstant);

        if(_posInput.z >= (_cardDistance.x * _cardDistanceConstant * _cardsInARow))
        {
            _posInput.x = _posInput.x + (_cardDistance.y * _cardDistanceConstant);

            _posInput.z = 0.0f;
        }

        _gameProperties.AddObjectsAsGO(_cardObjectInput);

        _gameProperties.AddObjectToList(_cardProperty);
    }

    public override void ISetActionsOfNoButton()
    {
        if(_gameProperties.GetYesOrNoCanvas() == null)
        {
            return;
        }

        if(_gameProperties.GetYesOrNoCanvas().GetNoButton() == null)
        {
            return;
        }

        Button _bt = _gameProperties.GetYesOrNoCanvas().GetNoButton();

        _bt.onClick.AddListener(delegate { base.ISetActionsOfNoButton(); });
    }

    public override void ISetActionsOfYesButtonToQuit()
    {
        if(_gameProperties.GetYesOrNoCanvas() == null)
        {
            return;
        }

        if(_gameProperties.GetYesOrNoCanvas().GetYesButton() == null)
        {
            return;
        }

        Button _bt = _gameProperties.GetYesOrNoCanvas().GetYesButton();

        _bt.onClick.AddListener(delegate { StopGame(); });
    }

    IEnumerator FlipUpCardAnimation(CardScript _input)
    {
        float _a = _input.gameObject.transform.localEulerAngles.x;

        float _currentRot = _a;

        _input.SetBlackOpacity(0.0f);

        for (float _s = _a; _s > 90.0f; _s -= (Time.deltaTime * _cardFlipAnimationSpeed))
        {
            _currentRot = _s;

            if (_currentRot <= 90.0f)
            {
                _currentRot = 90.0f;
            }

            _input.gameObject.transform.localRotation = Quaternion.Euler(_currentRot, 0.0f, 90.0f);

            yield return null;
        }

        _input.gameObject.transform.localRotation = Quaternion.Euler(90.0f, 0.0f, 90.0f);
    }

    IEnumerator FlipDownCardAnimation(CardScript _input)
    {
        float _a = _input.gameObject.transform.localEulerAngles.x;

        float _currentRot = _a;

        for (float _s = _a; _s < 270.0f; _s += (Time.deltaTime * _cardFlipAnimationSpeed))
        {
            _currentRot = _s;

            if (_currentRot >= 270.0f)
            {
                _currentRot = 270.0f;
            }

            _input.gameObject.transform.localRotation = Quaternion.Euler(_currentRot, 0.0f, 90.0f);

            yield return null;
        }

        _input.SetBlackOpacity(1.0f);

        _input.gameObject.transform.localRotation = Quaternion.Euler(270.0f, 0.0f, 90.0f);
    }

    public override void QuitGame()
    {
        _gameProperties.GetYesOrNoCanvas().GetYesButton().onClick.AddListener(delegate { StopGame(); });

        base.QuitGame();
    }
}
