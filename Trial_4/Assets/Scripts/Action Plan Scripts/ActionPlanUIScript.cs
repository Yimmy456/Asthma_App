using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class ActionPlanUIScript : MonoBehaviour
{
    [SerializeField]
    Text _questionNumberText;

    [SerializeField]
    Text _questionText;

    [SerializeField]
    InputField _integerNumberField;

    [SerializeField]
    InputField _decimalNumberField;

    [SerializeField]
    InputField _textField;

    [SerializeField]
    Dropdown _dropdown;

    [SerializeField]
    DatePanelScript _datePanel;

    [SerializeField]
    ContactBarsScript _contactBars;

    [SerializeField]
    Button _nextButton;

    [SerializeField]
    Button _previousButton;

    [SerializeField]
    Button _quitButton;

    [SerializeField]
    Button _submitButton;

    [SerializeField]
    Slider _questionsSlider;

    int _totalNumberOfQuestions = 12;

    // Start is called before the first frame update
    void Start()
    {
        AssignToSelf();

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Getters
    public Text GetQuestionNumberText()
    {
        return _questionNumberText;
    }

    public Text GetQuestionText()
    {
        return _questionText;
    }

    public InputField GetIntegerNumberField()
    {
        return _integerNumberField;
    }

    public InputField GetDecimalNumberField()
    {
        return _decimalNumberField;
    }

    public InputField GetTextField()
    {
        return _textField;
    }

    public Dropdown GetDropdown()
    {
        return _dropdown;
    }

    public DatePanelScript GetDatePanel()
    {
        return _datePanel;
    }

    public ContactBarsScript GetContactBars()
    {
        return _contactBars;
    }

    public Button GetNextButton()
    {
        return _nextButton;
    }

    public Button GetPreviousButton()
    {
        return _previousButton;
    }

    public Button GetQuitButton()
    {
        return _quitButton;
    }

    public Button GetSubmitButton()
    {
        return _submitButton;
    }

    public Slider GetQuestionsSlider()
    {
        return _questionsSlider;
    }

    public int GetTotalNumberOfQuestions()
    {
        return _totalNumberOfQuestions;
    }

    //Setters
    public void SetTotalNumberOfQuestions(int _input)
    {
        _totalNumberOfQuestions = _input;
    }

    //Other Functions

    public void SetQuestionNumber(int _input)
    {
        if(!(_input >= 0 && _input < _totalNumberOfQuestions) || _questionNumberText == null)
        {
            return;
        }

        int _v = _input + 1;

        string _st = "Question " + _v.ToString() + "/" + _totalNumberOfQuestions.ToString();

        _questionNumberText.text = _st;
    }

    public void SetQuestionText(string _input)
    {
        if(_questionText == null)
        {
            return;
        }

        _questionText.text = _input;
    }

    public void SetCorrectInputUI(ActionPlanQuestionScript _input)
    {
        VariableTypeForAPEnum _enum = _input.GetVariableType();

        bool _enumTrue;

        if(_integerNumberField != null)
        {
            _enumTrue = _enum == VariableTypeForAPEnum.Integer;

            _integerNumberField.gameObject.SetActive(_enumTrue);

            if(_enumTrue)
            {
                _integerNumberField.text = _input.GetIntegerAnswer().ToString();
            }
        }

        if(_decimalNumberField != null)
        {
            _enumTrue = _enum == VariableTypeForAPEnum.Decimal;

            _decimalNumberField.gameObject.SetActive(_enumTrue);

            if(_enumTrue)
            {
                _decimalNumberField.text = _input.GetDecimalAnswer().ToString();
            }
        }

        if(_textField != null)
        {
            _enumTrue = _enum == VariableTypeForAPEnum.Text;

            _textField.gameObject.SetActive(_enumTrue);

            if(_enumTrue)
            {
                _textField.text = _input.GetTextAnswer();

                //_textField.placeholder.GetComponent<Text>().text = _input.GetIn

                string _plH = ((ActionPlanQuestionText)_input).GetInputFieldHolderText();

                _textField.placeholder.GetComponent<Text>().text = _plH;
            }
        }

        if(_dropdown != null)
        {
            _enumTrue = _enum == VariableTypeForAPEnum.Enum;

            _dropdown.gameObject.SetActive(_enumTrue);

            if(_enumTrue)
            {
                SetEnumValues2(_input);

                //SetEnumValues2<InhalerColorEnum>(_input);

                //SetEnumValues2<GenderEnum>(_input);
            }
        }

        if(_datePanel != null)
        {
            _enumTrue = _enum == VariableTypeForAPEnum.Date;

            _datePanel.gameObject.SetActive(_enumTrue);
        }

        if(_contactBars != null)
        {
            _enumTrue = _enum == VariableTypeForAPEnum.Contact;

            _contactBars.gameObject.SetActive(_enumTrue);
        }
    }

    public void RegisterAnswer(ActionPlanQuestionScript _input)
    {
        if(_input == null)
        {
            return;
        }

        VariableTypeForAPEnum _enum = _input.GetVariableType();

        if(_enum == VariableTypeForAPEnum.Integer)
        {
            int.TryParse(_integerNumberField.text, out int _int);

            _input.SetIntegerAnswer(_int);
        }

        if(_enum == VariableTypeForAPEnum.Decimal)
        {
            float.TryParse(_decimalNumberField.text, out float _flt);

            _input.SetDecimalAnswer(_flt);
        }

        if(_enum == VariableTypeForAPEnum.Text)
        {
            _input.SetTextAnswer(_textField.text);

            _input.SetQuestionAnswered(_textField.text != "");
        }

        if(_enum == VariableTypeForAPEnum.Enum)
        {
            _input.SetIntegerAnswer(_dropdown.value);
        }

        if(_enum == VariableTypeForAPEnum.Date)
        {
            int.TryParse(_datePanel.GetYearPanel().text, out int _y);

            int _d = _datePanel.GetDayDropdown().value + 1;

            int _m = _datePanel.GetMonthDropdown().value + 1;

            _input.SetDateAnswer(_d, _m, _y);

            bool _finalBool = _d != 0 && _d != -1 && _m != 0 && _m != -1 && _y != 0 && _y != -1;

            _input.SetQuestionAnswered(_finalBool);

            _datePanel.ConfirmDate();
        }

        if(_enum == VariableTypeForAPEnum.Contact)
        {
            ActionPlanQuestionContact _c = (ActionPlanQuestionContact)(_input);

            if(_c != null)
            {
                bool _b;

                _c.GetAnswer().SetVariable1(_contactBars.GetEMailInputField().text);

                _b = _contactBars.GetEMailInputField().text != "";

                _c.GetAnswer().SetVariable2(_contactBars.GetPhoneNumberInputField().text);

                _b = _b && _contactBars.GetPhoneNumberInputField().text != "";

                _input.SetQuestionAnswered(_b);
            }
        }
    }

    public void LoadAnswer(ActionPlanQuestionScript _input)
    {
        if(_input == null)
        {
            return;
        }

        VariableTypeForAPEnum _enum = _input.GetVariableType();

        if(_enum == VariableTypeForAPEnum.Integer)
        {
            _integerNumberField.text = _input.GetIntegerAnswer().ToString();

            _input.SetQuestionAnswered(true);
        }

        if(_enum == VariableTypeForAPEnum.Decimal)
        {
            _decimalNumberField.text = _input.GetDecimalAnswer().ToString();

            _input.SetQuestionAnswered(true);
        }

        if(_enum == VariableTypeForAPEnum.Text)
        {
            _textField.text = _input.GetTextAnswer();

            _input.SetQuestionAnswered(_textField.text != "");
        }

        if(_enum == VariableTypeForAPEnum.Enum)
        {
            _dropdown.value = _input.GetIntegerAnswer();

            _input.SetQuestionAnswered(true);
        }

        if(_enum == VariableTypeForAPEnum.Date && _input.GetDateAnswer().GetDateSet())
        {
            _datePanel.SetDayText(_input.GetDateAnswer().GetDay());

            _datePanel.SetMonthText(_input.GetDateAnswer().GetMonth());

            _datePanel.SetYearText(_input.GetDateAnswer().GetYear());

            bool _bool2 = _input.GetDateAnswer().GetDay() != 0 && _input.GetDateAnswer().GetDay() != -1;

            _bool2 = _bool2 && _input.GetDateAnswer().GetMonth() != 0 && _input.GetDateAnswer().GetMonth() != -1;

            _bool2 = _bool2 && _input.GetDateAnswer().GetYear() != 0 && _input.GetDateAnswer().GetYear() != -1;

            _input.SetQuestionAnswered(_bool2);
        }

        if(_enum == VariableTypeForAPEnum.Contact)
        {
            ActionPlanQuestionContact _c2 = (ActionPlanQuestionContact)(_input);

            if(_c2 != null)
            {
                bool _bool3;

                _contactBars.SetEmailText(_c2.GetAnswer().GetVariable1());

                _bool3 = _c2.GetAnswer().GetVariable1() != "";

                _contactBars.SetPhoneNumberText(_c2.GetAnswer().GetVariable2());

                _bool3 = _c2.GetAnswer().GetVariable2() != "";

                _input.SetQuestionAnswered(_bool3);
            }
        }
    }

    void SetEnumValues(ActionPlanQuestionScript _input)
    {
        Enum _e = _input.GetEnumAnswer();

        string[] _valueNames = _e.GetType().GetEnumNames();

        int _currentValue = _input.GetIntegerAnswer();

        _dropdown.options.Clear();

        ActionPlanQuestionEnum_Gender _genderEnumCheck = _input as ActionPlanQuestionEnum_Gender;

        List<string> _l = new List<string>();

        if (_genderEnumCheck != null)
        {

            object _genderEnumObject;

            GenderEnum _genderEnum;

            for(int _i = 0; _i < Enum.GetValues(typeof(GenderEnum)).Length; _i++)
            {
                _genderEnumObject = Enum.GetValues(typeof(GenderEnum)).GetValue(_i);

                _genderEnum = (GenderEnum)_genderEnumObject;

                if(_genderEnum.GetType() != typeof(GenderEnum))
                {
                    continue;
                }

                if(_genderEnum == GenderEnum.Unknown)
                {
                    _l.Add("I do not wish to specify");
                }
                else
                {
                    _l.Add(_genderEnum.ToString());
                }
            }

            _dropdown.AddOptions(_l);
        }
        else
        {
            _dropdown.AddOptions(_valueNames.ToList<string>());
        }

        _dropdown.SetValueWithoutNotify(_currentValue);
    }

    void SetEnumValues2(ActionPlanQuestionScript _input)
    {
        List<string> _valueNames = new List<string>();

        Type _type = _input.GetType();

        int _currentValue = _input.GetIntegerAnswer();

        if(_type == typeof(ActionPlanQuestionEnum_InhalerColor))
        {
            var _v = ActionPlanManagerScript.GetInstance().GetStylesOfInhalerColorLabels().GetStyles();

            for(int _i = 0; _i < _v.Count; _i++)
            {
                _valueNames.Add(_v[_i].GetAlternativeText());
            }
        }

        if(_type == typeof(ActionPlanQuestionEnum_Gender))
        {
            var _v = ActionPlanManagerScript.GetInstance().GetStylesOfGenderLabels().GetStyles();

            for(int _i = 0; _i < _v.Count; _i++)
            {
                _valueNames.Add(_v[_i].GetAlternativeText());
            }
        }

        _dropdown.options.Clear();

        _dropdown.AddOptions(_valueNames);

        _dropdown.SetValueWithoutNotify(_currentValue);
    }

    void AssignToSelf()
    {
        if(ActionPlanManagerScript.GetInstance() == null)
        {
            return;
        }

        ActionPlanManagerScript.GetInstance().SetActionPlanUI(this);

        _nextButton.onClick.AddListener(delegate { ActionPlanManagerScript.GetInstance().GoToNextQuestion(); });

        _previousButton.onClick.AddListener(delegate { ActionPlanManagerScript.GetInstance().GoToPreviousQuestion(); });

        if(_questionsSlider != null)
        {
            _questionsSlider.maxValue = ActionPlanManagerScript.GetInstance().GetQuestionList().Count;

            _questionsSlider.minValue = 1;

            _questionsSlider.value = 1;
        }
    }

    public void FinishPlan()
    {
        if(ActionPlanManagerScript.GetInstance() == null)
        {
            return;
        }

        ActionPlanManagerScript.GetInstance().FinishPlan();
    }

    public void SaveResults()
    {
        if(ActionPlanManagerScript.GetInstance() == null)
        {
            return;
        }

        ActionPlanQuestionScript _q = ActionPlanManagerScript.GetInstance().GetCurrentQuestion();

        RegisterAnswer(_q);

        ActionPlanManagerScript.GetInstance().SaveResults();
    }

    ActionPlanQuestionEnum<T> GetClass<T>(ActionPlanQuestionScript<T> _input) where T : Enum
    {
        if(_input.GetVariableType() == VariableTypeForAPEnum.Enum)
        {
            ActionPlanQuestionEnum<T> _v = _input as ActionPlanQuestionEnum<T>;
        }

        return null;
    }

    RectTransform GetLabelRT(Dropdown _input, int _indexInput)
    {
        if(_input == null)
        {
            return null;
        }

        RectTransform _rt = _dropdown.gameObject.GetComponent<RectTransform>().Find("Dropdown List").gameObject.GetComponent<RectTransform>();

        if(_rt == null)
        {
            return null;
        }

        _rt = _rt.gameObject.GetComponent<RectTransform>().Find("Viewport").gameObject.GetComponent<RectTransform>();

        if (_rt == null)
        {
            return null;
        }

        _rt = _rt.gameObject.GetComponent<RectTransform>().gameObject.GetComponent<RectTransform>();

        if(_rt == null)
        {
            return null;
        }

        RectTransform _rt2 = _rt.GetChild(_indexInput + 1).gameObject.GetComponent<RectTransform>();

        return _rt2;
    }
}
