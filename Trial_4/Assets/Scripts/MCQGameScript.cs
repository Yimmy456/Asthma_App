using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class MCQGameScript : GameGenericMBScript<QuestionClass>
{
    [Header("2. Properties of this Particular Game.")]

    [SerializeField]
    MCQCanvasScript _mcqCanvas;

    [SerializeField]
    QuestionClass _currentQuestion;

    [SerializeField]
    int _numberOfQuestions = 5;

    [SerializeField]
    int _currentQuestionIndex = -1;

    Coroutine _coroutine;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _gameProperties.UpdateUI();

        _completionMeter = _gameProperties.GetMeter();

        if(_gameDone)
        {
            WinGame();
        }
    }

    void OnEnable()
    {

    }

    public QuestionClass GetCurrentQuestion()
    {
        return _currentQuestion;
    }

    /*
    void CorrectAnswer()
    {
        if(_nextButton != null)
        {
            _nextButton.gameObject.SetActive(true);
        }

        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);

            _gameProperties.ClearResponseText();
        }

        _gameProperties.SetResponseText("That is correct! Well Done! Press 'Next' to continue.", Color.green, new Color(0.0f, 0.5f, 0.0f, 0.5f), new Vector2(1.0f, -1.0f));

        //_gameProperties.AddObjectToList(_currentQuestion);

        _currentQuestion.SetAnsweredCorrectly(true);

        for(int _i = 0; _i < 4; _i++)
        {
            if (_buttons[_i] != null)
            {
                SetButtonTextAlpha(_buttons[_i], 0.5f);

                _buttons[_i].interactable = false;
            }
        }

        _gameProperties.GetMeter().AddToValue(1);

        _gameProperties.SignalToUpdateUI();

        if(_nextButton != null)
        {
            if(_gameProperties.GetMeter().GetPercentage() == 100.0f && !_gameDone)
            {
                _nextButton.onClick.AddListener(delegate { WinGame(); });
            }
            else
            {
                _nextButton.onClick.AddListener(delegate { PrepareQuestion(); });
            }

            _nextButton.onClick.AddListener(delegate { _nextButton.onClick.RemoveAllListeners(); });
        }
    }

    void IncorrectAnswer()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);

            _gameProperties.ClearResponseText();
        }

        _gameProperties.SetResponseText("That is not quite correct. Please, try again. I know you can do it!", Color.red, new Color(0.5f, 0.0f, 0.0f, 0.5f), new Vector2(1.0f, -1.0f));

        _coroutine = StartCoroutine(ClearResponseTextFunction());
    }

    void PrepareQuestion()
    {
        for(int _i = 0; _i < 4; _i++)
        {
            _buttons[_i].onClick.RemoveAllListeners();
        }

        _currentQuestionIndex++;

        if(_currentQuestionIndex >= MCQManagerScript.GetInstance().GetSelectedQuestions().Count)
        {
            if(_nextButton != null)
            {
                _nextButton.onClick.RemoveAllListeners();
            }

            return;
        }

        QuestionClass _crq = MCQManagerScript.GetInstance().GetSelectedQuestions()[_currentQuestionIndex];

        _currentQuestion = _crq;

        SetQuestionNumber(_currentQuestionIndex + 1);

        SetQuestionText(_crq.GetQuestionText());

        _gameProperties.ClearResponseText();

        for(int _i = 0; _i < 4; _i++)
        {
            if (_buttons[_i] == null)
            {
                continue;
            }

            Text _t = _buttons[_i].GetComponent<RectTransform>().Find("Text (Legacy)").gameObject.GetComponent<Text>();

            _t.text = _crq.GetAnswers()[_i];

            _buttons[_i].gameObject.SetActive(true);

            _buttons[_i].interactable = true;

            SetButtonTextAlpha(_buttons[_i], 1.0f);

            if(_i == _crq.GetCorrectAnswerIndex())
            {
                _buttons[_i].onClick.AddListener(CorrectAnswer);
            }
            else
            {
                _buttons[_i].onClick.AddListener(IncorrectAnswer);
            }
        }

        if (_nextButton != null)
        {
            _nextButton.onClick.RemoveAllListeners();

            _nextButton.gameObject.SetActive(false);
        }
    }
    */
    void SetButtonTextAlpha(Button _buttonInput, float _alphaInput)
    {
        if(_buttonInput == null || !(_alphaInput >= 0.0f && _alphaInput <= 1.0f))
        {
            return;
        }

        Text _text = _buttonInput.gameObject.GetComponent<RectTransform>().GetComponentInChildren<Text>();

        if(_text == null)
        {
            return;
        }

        Color _c  =_text.color;

        _c.a = _alphaInput;

        _text.color = _c;
    }

    IEnumerator ClearResponseTextFunction()
    {
        yield return new WaitForSeconds(5.0f);

        _gameProperties.ClearResponseText();
    }

    public void SetActionsOfYesButton()
    {
        if(_gameProperties.GetYesOrNoCanvas() == null)
        {
            return;
        }

        Button _bYes = _gameProperties.GetYesOrNoCanvas().GetYesButton();

        _bYes.onClick.AddListener(_gameProperties.ReturnToPlayerCanvas);

        _bYes.onClick.AddListener(_gameProperties.ActionsOfYesButton);
    }

    public void SetActionsOfNoButton()
    {
        if(_gameProperties.GetYesOrNoCanvas() == null)
        {
            return;
        }

        Button _bNo = _gameProperties.GetYesOrNoCanvas().GetNoButton();

        _bNo.onClick.AddListener(delegate { if (_coroutine != null) { StopCoroutine(_coroutine); _gameProperties.ClearResponseText(); } });

        _bNo.onClick.AddListener(_gameProperties.ActionsOfNoButton);
    }

    /*public override void StartGame()
    {
        if(MCQManagerScript.GetInstance() == null || GetGameInSession())
        {
            return;
        }

        base.StartGame();

        _currentGame = this;

        int _counter1 = 0;

        int _counter2 = 0;

        int _selectedIndex = -1;

        while(_counter1 < _numberOfQuestions)
        {
            _selectedIndex = Random.Range(0, (MCQManagerScript.GetInstance().GetQuestions().Count));

            QuestionClass _currentQ = MCQManagerScript.GetInstance().GetQuestions()[_selectedIndex];

            while(_counter2 < _counter1 && _counter2 != -1)
            {
                string _currentID = _gameProperties.GetListOfObjects()[_counter2].GetQuestionID();

                if(string.Compare(_currentQ.GetQuestionID(), _currentID, false) == 0)
                {
                    _counter2 = -1;

                    continue;
                }

                _counter2++;
            }

            if(_counter2 == -1)
            {
                _counter2 = 0;

                continue;
            }

            _gameProperties.AddObjectToList(_currentQ);

            _counter2 = 0;

            _counter1++;
        }

        ShuffleFunction();

        MCQManagerScript.GetInstance().SetSelectedQuestions(_gameProperties.GetListOfObjects());

        _gameProperties.GetMeter().SetMaxValue(_numberOfQuestions);

        //PrepareQuestion();
    }*/

    protected override void ShuffleFunction()
    {
        List<QuestionClass> _tempList = _gameProperties.GetListOfObjects();

        List<QuestionClass> _newList = new List<QuestionClass>();

        QuestionClass _currentQ2;

        int _selectedIndex2 = -1;

        while(_tempList.Count > 0)
        {
            _selectedIndex2 = Random.Range(0, _tempList.Count);

            _currentQ2 = _tempList[_selectedIndex2];

            _currentQ2.RandomizeAnswerPositions();

            _newList.Add(_currentQ2);

            _tempList.RemoveAt(_selectedIndex2);
        }

        _gameProperties.SetListOfObjects(_newList);
    }

    protected override void WinGame()
    {
        //_gameProperties.ClearObjectLists();

        MCQManagerScript.GetInstance().GetSelectedQuestions().Clear();

        _currentQuestionIndex = -1;

        base.WinGame();
    }

    public override void QuitGame()
    {
        //_gameProperties.ClearObjectLists();

        //MCQManagerScript.GetInstance().GetSelectedQuestions().Clear();

        base.QuitGame();

        //ISetActionsOfYesButtonToQuit

        //_currentQuestionIndex = -1;

        //MCQManagerScript.GetInstance().GetSelectedQuestions().Clear();

        //if(_gameProperties.GetYesOrNoCanvas() != null)
        //{
        //    if(_gameProperties.GetYesOrNoCanvas().GetYesButton() != null)
        //    {

        //    }
        //}
    }

    public override void StopGame()
    {
        base.StopGame();

        _currentQuestion = null;

        _currentQuestionIndex = -1;
    }

    public override void StartGame()
    {
        if(GetGameInSession() || MCQManagerScript.GetInstance() == null || _mcqCanvas == null)
        {
            return;
        }

        base.StartGame();

        _currentGame = this;

        int _counter = 0;

        int _selectedIndex = -1;

        QuestionClass _currentQuestion;

        int _noOfQuestionsInList = MCQManagerScript.GetInstance().GetQuestions().Count;

        bool _questionInList = false;

        bool _comp = false;

        while(_counter < _numberOfQuestions)
        {
            _selectedIndex = Random.Range(0, _noOfQuestionsInList);

            _currentQuestion = MCQManagerScript.GetInstance().GetQuestions()[_selectedIndex];

            for(int _i = 0; _i < _gameProperties.GetListOfObjects().Count && !_questionInList; _i++)
            {
                _comp = string.Compare(_currentQuestion.GetQuestionID(), _gameProperties.GetListOfObjects()[_i].GetQuestionID(), false) == 0;

                if(_comp)
                {
                    _questionInList = true;
                }
            }

            if(_questionInList)
            {
                _questionInList = false;

                continue;
            }

            _gameProperties.AddObjectToList(_currentQuestion);

            _counter++;
        }

        ShuffleFunction();

        MCQManagerScript.GetInstance().SetSelectedQuestions(_gameProperties.GetListOfObjects());

        _gameProperties.GetMeter().SetMaxValue(_gameProperties.GetListOfObjects().Count);

        _gameProperties.GetMeter().SetValue(0);

        _gameProperties.SignalToUpdateUI();

        PrepareQuestion();
    }

    public void PrepareQuestion()
    {
        if(_mcqCanvas == null || MCQManagerScript.GetInstance() == null)
        {
            return;
        }

        _currentQuestionIndex++;

        if(_currentQuestionIndex >= _numberOfQuestions)
        {
            _gameDone = true;

            //_currentQuestionIndex = -1;

            return;
        }

        _currentQuestion = MCQManagerScript.GetInstance().GetSelectedQuestions()[_currentQuestionIndex];

        _mcqCanvas.SetQuestion(_currentQuestion);

        _mcqCanvas.SetQuestionNumber(_currentQuestionIndex + 1);

        _mcqCanvas.PrepareQuestion(_currentQuestion);
    }

    public override void ISetActionsOfYesButtonToQuit()
    {
        _currentQuestionIndex = -1;

        MCQManagerScript.GetInstance().GetSelectedQuestions().Clear();

        _currentQuestion = null;

        Debug.Log("We will quit the MCQ game.");

        base.ISetActionsOfYesButtonToQuit();
    }
}
