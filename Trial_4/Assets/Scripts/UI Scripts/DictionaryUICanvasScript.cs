using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DictionaryUICanvasScript : CollectionUICanvasScript
{
    [Header("2. Variables for the Dictionary UI Canvas.")]
    [SerializeField]
    Color _generalColor;

    [SerializeField]
    Color _symptomColor;

    [SerializeField]
    Color _triggerColor;

    [SerializeField]
    Color _treatmentColor;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    float _alpha = (80.0f / 255.0f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ShowItems()
    {
        if(BookScript.GetInstance() == null || _rowObjectTemplate == null || _contentArea == null)
        {
            return;
        }

        ClearList();

        List<DefinitionClass> _defs = new List<DefinitionClass>();

        _defs.AddRange(BookScript.GetInstance().GetDefinitions());

        Vector2 _pos = _initialPosition;

        for(int _i = 0; _i < _defs.Count; _i++)
        {
            _currentRowObject = Instantiate(_rowObjectTemplate);

            _currentRowObject.SetActive(true);

            DefinitionClass _currentWord = _defs[_i];

            RectTransform _rectT = _currentRowObject.GetComponent<RectTransform>();

            _rectT.parent = _contentArea.GetComponent<RectTransform>();

            int _disIndex = _i + 1;

            Text _numberText = _rectT.Find("Index Text").GetComponent<Text>();

            _numberText.text = _disIndex.ToString() + ".";

            Text _wordText = _rectT.Find("Word Text").GetComponent<Text>();

            _wordText.text = _currentWord.GetInformationName();

            Image _im = _currentRowObject.GetComponent<Image>();

            _im.color = GetColorByWordCategory(_currentWord);

            AssignImage(_currentWord, _currentRowObject);

            _rectT.anchoredPosition3D = _pos;

            _rectT.localRotation = Quaternion.identity;

            _rectT.localScale = Vector3.one;

            _pos.y = _pos.y - _distance.y;

            SetButton(_currentRowObject, _currentWord, _i);

            _rowObjects.Add(_currentRowObject);
        }

        ContentAreaHeightFunction();
    }

    Color GetColorByWordCategory(DefinitionClass _input)
    {
        WordCategoryEnum _wc = _input.GetWordCategory();

        Color _c;

        switch(_wc) {
            case WordCategoryEnum.Trigger:
                _c = _triggerColor;
                break;
            case WordCategoryEnum.Symptom:
                _c = _symptomColor;
                break;
            case WordCategoryEnum.Treatment:
                _c = _treatmentColor;
                break;
            default:
                _c = _generalColor;
                break;
        }

        _c.a = _alpha;

        return _c;
    }

    public override void ClearList()
    {
        foreach (GameObject _obj in _rowObjects)
        {
            Destroy(_obj);
        }

        _rowObjects.Clear();
    }

    void AssignImage(DefinitionClass _wordInput, GameObject _rowInput)
    {
        if(_wordInput.GetSprite() == null)
        {
            return;
        }

        Sprite _sp = _wordInput.GetSprite();

        Image _image = _rowInput.GetComponent<RectTransform>().Find("Word Card Image").gameObject.GetComponent<Image>();

        _image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        _image.sprite = _sp;
    }

    void MakeDefinitionCanvas(DefinitionClass _wordInput, int _indexInput)
    {
        if(_selectedItemCanvas == null)
        {
            return;
        }

        //Declaring Variables
        RectTransform _canvasRectT = _selectedItemCanvas.gameObject.GetComponent<RectTransform>();

        Text _wordText = _canvasRectT.Find("Word Text").gameObject.GetComponent<Text>();

        Text _indexText = _canvasRectT.Find("Index Text").gameObject.GetComponent<Text>();

        Text _typeText = _canvasRectT.Find("Type Text").gameObject.GetComponent<Text>();

        Text _categoryText = _canvasRectT.Find("Category Text").gameObject.GetComponent<Text>();

        Text _definitionText = _canvasRectT.Find("Definition Text").gameObject.GetComponent<Text>();

        Image _cardImage = _canvasRectT.Find("Card Image").gameObject.GetComponent<Image>();

        //Initializing Variables
        _wordText.text = _wordInput.GetInformationName();

        _indexText.text = _indexInput.ToString() + ".";

        _typeText.text = "Type: " + GetWordTypeAsText(_wordInput);

        _categoryText.text = "Category: " + GetWordCategoryAsText(_wordInput);

        _definitionText.text = _wordInput.GetInformationDescription();

        if(_wordInput.GetSprite() != null)
        {
            _cardImage.sprite = _wordInput.GetSprite();
        }
    }

    string GetWordTypeAsText(DefinitionClass _input)
    {
        WordTypeEnum _tp = _input.GetWordType();

        string _tx;

        switch(_tp)
        {
            case WordTypeEnum.Verb:
                _tx = "Verb";
                break;
            default:
                _tx = "Noun";
                break;
        }

        return _tx;
    }

    string GetWordCategoryAsText(DefinitionClass _input)
    {
        WordCategoryEnum _category = _input.GetWordCategory();

        string _st;

        switch (_category)
        {
            case WordCategoryEnum.Trigger:
                _st = "Trigger";
                break;
            case WordCategoryEnum.Symptom:
                _st = "Symptom";
                break;
            case WordCategoryEnum.Treatment:
                _st = "Treatment";
                break;
            default:
                _st = "General";
                break;
        }

        return _st;
    }

    void SetButton(GameObject _rowInput, DefinitionClass _wordInput, int _indexInput)
    {
        Button _b = _rowInput.GetComponent<Button>();

        if(_b == null)
        {
            return;
        }

        _b.onClick.AddListener(delegate { MakeDefinitionCanvas(_wordInput, (_indexInput + 1)); });

        _b.onClick.AddListener(delegate { _selectedItemCanvas.gameObject.SetActive(true); });

        _b.onClick.AddListener(delegate { gameObject.SetActive(false); });
    }
}
