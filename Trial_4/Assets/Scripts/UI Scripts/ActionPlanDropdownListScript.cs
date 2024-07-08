using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPlanDropdownListScript : CanvasScript
{
    [SerializeField]
    Dropdown _dropdownList;

    [SerializeField]
    ActionPlanUIScript _actionPlanUI;

    [SerializeField]
    Sprite _downSprite;

    [SerializeField]
    Sprite _upSprite;

    [SerializeField]
    Image _dropdownImage;

    [SerializeField]
    Image _optionIconImage;

    [SerializeField]
    float _iconSize = 0.2f;

    bool _listDropped = false;

    bool _styleChanged = false;

    bool _mainLabelChanged = false;

    // Start is called before the first frame update
    void Start()
    {
        _mainLabelChanged = true;

        ChangeMainLabelFunction();
    }

    // Update is called once per frame
    void Update()
    {
        CheckListDropped();
    }

    private void OnEnable()
    {
        _mainLabelChanged = true;

        ChangeMainLabelFunction();
    }

    void CheckListDropped()
    {

        if (_dropdownList == null || ActionPlanManagerScript.GetInstance() == null)
        {
            _listDropped = false;

            _styleChanged = false;

            DefaultArrow();

            return;
        }

        if (ActionPlanManagerScript.GetInstance().GetCurrentQuestion() == null)
        {
            _listDropped = false;

            _styleChanged = false;

            DefaultArrow();

            return;
        }

        ActionPlanQuestionScript _currentQuestion = ActionPlanManagerScript.GetInstance().GetCurrentQuestion();

        System.Type _type = _currentQuestion.GetType();

        ChangeMainLabelFunction();

        _listDropped = (_dropdownList.gameObject.GetComponent<RectTransform>().Find("Dropdown List") != null);

        if (_listDropped && _styleChanged)
        {
            return;
        }
        else if(!_listDropped)
        {
            DefaultArrow();

            _styleChanged = false;

            return;
        }

        if (_type == typeof(ActionPlanQuestionEnum_InhalerColor))
        {
            var _styles = ActionPlanManagerScript.GetInstance().GetStylesOfInhalerColorLabels().GetStyles();

            ChangeLabelStyles(_styles);
        }

        if(_type == typeof(ActionPlanQuestionEnum_Gender))
        {
            var _styles = ActionPlanManagerScript.GetInstance().GetStylesOfGenderLabels().GetStyles();

            ChangeLabelStyles(_styles);
        }

        if(_dropdownImage != null && _upSprite != null)
        {
            _dropdownImage.sprite = _upSprite;

            _dropdownImage.color = Color.red;
        }

        _styleChanged = true;
    }

    void ChangeLabelStyles<T>(List<EnumDropdownLabelStyleClass<T>> _input) where T : System.Enum
    {
        RectTransform _rt = _dropdownList.gameObject.GetComponent<RectTransform>().Find("Dropdown List").gameObject.GetComponent<RectTransform>();

        if (_rt == null)
        {
            Debug.LogError("There is no child called 'Dropdown List'.");

            return;
        }

        _rt = _rt.gameObject.GetComponent<RectTransform>().Find("Viewport").gameObject.GetComponent<RectTransform>();

        if (_rt == null)
        {
            Debug.LogError("There is no child called 'Viewport'.");

            return;
        }

        _rt = _rt.gameObject.GetComponent<RectTransform>().Find("Content").gameObject.GetComponent<RectTransform>();

        if (_rt == null)
        {
            Debug.LogError("There is no child called 'Content'.");

            return;
        }

        RectTransform _rt2, _rt3;

        Image _img;

        //int _size;

        Text _text;

        EnumDropdownLabelStyleClass<T> _selectedStyle = null;

        EnumDropdownLabelStyleClass<T> _currentStyleInLoop;

        Outline _outline;

        bool _stringComp;

        for (int _i = 0; _i < _input.Count; _i++)
        {
            _rt2 = _rt.GetChild(_i + 1).gameObject.GetComponent<RectTransform>();

            if(_rt2 == null)
            {
                Debug.LogError("Child No. " + (_i + 1) + " does not exist.");

                continue;
            }

            //_currentOption = _dropdownList.value == _i;

            _rt3 = _rt2.gameObject.GetComponent<RectTransform>().Find("Item Background").gameObject.GetComponent<RectTransform>();

            _img = _rt3.gameObject.GetComponent<Image>();

            _rt3 = _rt2.gameObject.GetComponent<RectTransform>().Find("Item Label").gameObject.GetComponent<RectTransform>();

            _text = _rt3.gameObject.GetComponent<Text>();

            if(_img == null || _text == null)
            {
                Debug.LogError("Something is missing in label No. " + (_i + 1) + ".");

                continue;
            }

            _outline = _text.gameObject.GetComponent<Outline>();

            _selectedStyle = null;

            for(int _j = 0; _j < _input.Count && _selectedStyle == null; _j++)
            {
                _currentStyleInLoop = _input[_j];

                _stringComp = string.Compare(_currentStyleInLoop.GetAlternativeText(), _text.text, true) == 0;

                if(_stringComp)
                {
                    _selectedStyle = _currentStyleInLoop;
                }
            }

            if(_selectedStyle == null)
            {
                continue;
            }

            _img.color = _selectedStyle.GetLabelColor();

            _text.color = _selectedStyle.GetTextColor();

            _text.fontSize = _selectedStyle.GetTextSize();

            if(_outline != null)
            {
                _outline.effectColor = _selectedStyle.GetOutlineColor();

                _outline.effectDistance = _selectedStyle.GetOutlineEffect();
            }

            _rt3 = _rt2.gameObject.GetComponent<RectTransform>().Find("Item Icon Area").gameObject.GetComponent<RectTransform>();

            if(_rt3 == null)
            {
                continue;
            }

            _rt3 = _rt3.gameObject.GetComponent<RectTransform>().Find("Image").gameObject.GetComponent<RectTransform>();

            if(_rt3 == null)
            {
                continue;
            }

            Image _labelImage = _rt3.gameObject.GetComponent<Image>();

            if (_labelImage == null)
            {
                continue;
            }

            if(_selectedStyle.GetLabelSprite() == null)
            {
                Color _c = _labelImage.color;

                _c.a = 0.0f;

                _labelImage.color = _c;

                continue;
            }
            else
            {
                _labelImage.color = _selectedStyle.GetLabelSpriteColor();
            }

            Vector2 _v2 = _selectedStyle.GetLabelSprite().textureRect.size;

            _labelImage.sprite = _selectedStyle.GetLabelSprite();

            _labelImage.GetComponent<RectTransform>().sizeDelta = _v2;

            _labelImage.GetComponent<RectTransform>().localScale = (Vector3.one * _iconSize);
        }
    }


    void ChangeMainLabelFunction()
    {

        if(ActionPlanManagerScript.GetInstance() == null)
        {
            return;
        }

        if(ActionPlanManagerScript.GetInstance().GetCurrentQuestion() == null)
        {
            return;
        }

        ActionPlanQuestionScript _currentQuestion = ActionPlanManagerScript.GetInstance().GetCurrentQuestion();

        System.Type _type = _currentQuestion.GetType();

        if(_type == typeof(ActionPlanQuestionEnum_InhalerColor))
        {
            var _styles = ActionPlanManagerScript.GetInstance().GetStylesOfInhalerColorLabels().GetStyles();

            ColorMainLabel(_styles);
        }

        if(_type == typeof(ActionPlanQuestionEnum_Gender))
        {
            var _styles = ActionPlanManagerScript.GetInstance().GetStylesOfGenderLabels().GetStyles();

            ColorMainLabel(_styles);
        }
    }

    void ColorMainLabel<T>(List<EnumDropdownLabelStyleClass<T>> _input) where T : System.Enum 
    {
        int _lIndex = _dropdownList.value;

        string _st = _dropdownList.options[_lIndex].text;

        EnumDropdownLabelStyleClass<T> _selectedStyle = null;

        EnumDropdownLabelStyleClass<T> _currentStyle;

        bool _comp;

        for(int _i = 0; _i < _input.Count && _selectedStyle == null; _i++)
        {
            _currentStyle = _input[_i];

            _comp = string.Compare(_currentStyle.GetAlternativeText(), _st, true) == 0;

            if(_comp)
            {
                _selectedStyle = _currentStyle;
            }
        }

        if(_selectedStyle == null)
        {
            return;
        }

        _dropdownList.image.color = _selectedStyle.GetLabelColor();

        RectTransform _rt = _dropdownList.gameObject.GetComponent<RectTransform>();

        _rt =_rt.Find("Label").gameObject.GetComponent<RectTransform>();

        if(_rt == null)
        {
            return;
        }

        Text _labelText = _rt.gameObject.GetComponent<Text>();

        if(_labelText == null)
        {
            return;
        }

        _labelText.color = _selectedStyle.GetTextColor();

        _labelText.gameObject.GetComponent<Outline>().effectColor = _selectedStyle.GetOutlineColor();

        _labelText.fontSize = _selectedStyle.GetTextSizeOnMainLabel();

        if(_optionIconImage == null)
        {
            return;
        }
        
        
        if(_selectedStyle.GetLabelSprite() == null)
        {
            Color _c = _optionIconImage.color;

            _c.a = 0.0f;

            _optionIconImage.color = _c;

            return;
        }
        else
        {
            _optionIconImage.color = _selectedStyle.GetLabelSpriteColor();
        }

        Vector2 _originalDim = _selectedStyle.GetLabelSprite().textureRect.size;

        _optionIconImage.GetComponent<RectTransform>().sizeDelta = _originalDim;

        _optionIconImage.GetComponent<RectTransform>().localScale = (Vector3.one * _iconSize);

        _optionIconImage.sprite = _selectedStyle.GetLabelSprite();
    }

    void DefaultArrow()
    {
        if(_dropdownImage == null)
        {
            return;
        }

        if(_dropdownImage.color != Color.green)
        {
            _dropdownImage.color = Color.green;
        }

        if(_downSprite == null)
        {
            return;
        }

        if(_dropdownImage.sprite != _downSprite)
        {
            _dropdownImage.sprite = _downSprite;
        }
    }

    public void MainLabelHasChanged()
    {
        _mainLabelChanged = true;
    }
}
