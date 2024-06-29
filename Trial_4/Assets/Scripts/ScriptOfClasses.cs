using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public class MeterClass
{
    int _value;

    int _reverseValue;

    int _maxValue;

    float _percentage;

    float _reversePercentage;

    [SerializeField]
    Gradient _textGradient;

    [SerializeField]
    Gradient _textBorderGradient;

    public MeterClass()
    {
        _value = 0;

        _reverseValue = 1;

        _maxValue = 1;

        _percentage = 0.0f;

        _reversePercentage = 100.0f;
    }

    public MeterClass(int _input)
    {
        int _i = _input <= 0 ? 1 : _input;

        _value = _i;

        _reverseValue = 0;

        _maxValue = _i;

        _percentage = 100.0f;

        _reversePercentage = 0.0f;
    }

    public MeterClass(int _input, int _inputMax)
    {
        _value = _input;

        _maxValue = _inputMax;

        AdjustValues();
    }

    public int GetValue()
    {
        return _value;
    }

    public int GetReverseValue()
    {
        return _reverseValue;
    }

    public int GetMaxValue()
    {
        return _maxValue;
    }

    public float GetPercentage()
    {
        return _percentage;
    }

    public float GetReversePercentage()
    {
        return _reversePercentage;
    }

    public Gradient GetTextGradient()
    {
        return _textGradient;
    }

    public Gradient GetTextBorderGradient()
    {
        return _textBorderGradient;
    }

    public void SetValue(int _input)
    {
        _value = _input;

        AdjustValues();
    }

    public void SetNewValues(int _vInput, int _mvInput)
    {
        _value = _vInput;

        _maxValue = _mvInput;

        AdjustValues();
    }

    public void SetMaxValue(int _input)
    {
        _maxValue = _input;

        AdjustValues();
    }

    public void AddToValue(int _input)
    {
        _value = _value + _input;

        AdjustValues();
    }

    public void AddToMaxValue(int _input)
    {
        _maxValue = _maxValue + _input;

        AdjustValues();
    }

    public void RefillMeter()
    {
        _value = _maxValue;

        _reverseValue = 0;

        _percentage = 100.0f;

        _reversePercentage = 0.0f;
    }

    public void SetTextColor(Text _input)
    {
        if(_input == null)
        {
            return;
        }

        float _ratio = _percentage / 100.0f;

        Color _c = _textGradient.Evaluate(_ratio);

        _input.color = _c;

        if (_input.GetComponent<Outline>() == null)
        {
            return;
        }

        _c = _textBorderGradient.Evaluate(_ratio);

        _input.GetComponent<Outline>().effectColor = _c;
    }

    void AdjustValues()
    {
        if(_maxValue <= 0)
        {
            _maxValue = 1;
        }

        if(_value < 0)
        {
            _value = 0;
        }
        if(_value > _maxValue)
        {
            _value = _maxValue;
        }

        _reverseValue = _maxValue - _value;

        _percentage = (_maxValue > 0) ? (((float)_value / (float)_maxValue) * 100.0f) : -1.0f;

        _reversePercentage = (_maxValue > 0) ? (((float)_reverseValue / (float)_maxValue) * 100.0f) : -1.0f;
    }
}

[System.Serializable]
public class InformationClass
{

    [Header("1. Properties of any information.")]
    [SerializeField]
    protected string _informationName;

    [SerializeField]
    protected string _informationDesc;

    [SerializeField]
    protected List<string> _informationExamples;

    [SerializeField]
    protected bool _informationLearned;

    protected InformationTypeEnum _informationType = InformationTypeEnum.General;

    [SerializeField]
    protected string _badgeName;

    public InformationClass()
    {
        _informationType = InformationTypeEnum.General;
    }

    public string GetInformationName()
    {
        return _informationName;
    }

    public string GetInformationDescription()
    {
        return _informationDesc;
    }

    public List<string> GetInformationExamples()
    {
        return _informationExamples;
    }

    public bool GetInformationLearned()
    {
        return _informationLearned;
    }

    public InformationTypeEnum GetInformationType()
    {
        return _informationType;
    }

    public string GetBadgeName()
    {
        return _badgeName;
    }

    public void SetInformationLearned(bool _input)
    {
        _informationLearned = _input;
    }

    public void UpdateInformationLearnedToBook(bool _input = true)
    {
        if(BookScript.GetInstance() == null)
        {
            return;
        }

        BookScript.GetInstance().UpdateInformationLearned(this, _input);
    }

    public void AwardBadge()
    {
        if(BadgesManagerScript.GetInstance() == null)
        {
            return;
        }

        if(BadgesManagerScript.GetInstance().GetBadgeByName(_badgeName) == null)
        {
            return;
        }

        BadgeScript _badge = BadgesManagerScript.GetInstance().GetBadgeByName(_badgeName);

        BadgesManagerScript.GetInstance().UpdateBadgesCollectedMeter();
    }
}

[System.Serializable]
public class DefinitionClass : InformationClass
{
    [Header("2. Properties of a definition.")]
    [SerializeField]
    protected Sprite _sprite;

    [SerializeField]
    protected WordTypeEnum _wordType;

    [SerializeField]
    protected WordCategoryEnum _wordCategory;

    public DefinitionClass()
    {
        _informationType = InformationTypeEnum.Definition;
    }

    public Sprite GetSprite()
    {
        return _sprite;
    }

    public WordCategoryEnum GetWordCategory()
    {
        return _wordCategory;
    }

    public WordTypeEnum GetWordType()
    {
        return _wordType;
    }
}

[System.Serializable]
public class MenuSceneCameraClass
{
    [SerializeField]
    string _name;

    [SerializeField]
    Camera _camera;

    [SerializeField]
    Transform _transform;

    public string GetName()
    {
        return _name;
    }

    public Camera GetCamera()
    {
        return _camera;
    }

    public Transform GetTransform()
    {
        return _transform;
    }
}

[System.Serializable]
public class DateClass
{
    [SerializeField]
    int _year = 2020;

    [SerializeField]
    int _month = 1;

    [SerializeField]
    int _day = 1;

    string _dayOfWeek;

    public DateClass()
    {
        _year = 2020;
        _month = 1;
        _day = 1;
    }

    public DateClass(bool _setDateInput = true)
    {
        _year = _setDateInput ? 2020 : -1;
        _month = _setDateInput ? 1 : -1;
        _day = _setDateInput ? 1 : -1;
    }

    public DateClass(int _yearInput, int _monthInput, int _dayInput)
    {
        _year = _yearInput;
        _month = _monthInput;
        _day = _dayInput;

        ReviseDate();
    }

    void ReviseDate()
    {
        if(_month > 12 || _month < 1)
        {
            _month = 1;
        }

        int _maxDay = GetMaxDays();

        if(_day < 1 || _day > _maxDay)
        {
            _day = 1;
        }

        GetDayOfWeekInDate();
    }

    public string GetMonthName()
    {
        string _name = "";

        switch(_month)
        {
            case 1:
                _name = "January";
                break;
            case 2:
                _name = "February";
                break;
            case 3:
                _name = "March";
                break;
            case 4:
                _name = "April";
                break;
            case 5:
                _name = "May";
                break;
            case 6:
                _name = "June";
                break;
            case 7:
                _name = "July";
                break;
            case 8:
                _name = "August";
                break;
            case 9:
                _name = "September";
                break;
            case 10:
                _name = "October";
                break;
            case 11:
                _name = "November";
                break;
            case 12:
                _name = "December";
                break;
        }

        return _name;
    }

    public string GetMonthAbbreviation()
    {
        string _abb = "";

        switch (_month)
        {
            case 1:
                _abb = "Jan";
                break;
            case 2:
                _abb = "Feb";
                break;
            case 3:
                _abb = "Mar";
                break;
            case 4:
                _abb = "Apr";
                break;
            case 5:
                _abb = "May";
                break;
            case 6:
                _abb = "Jun";
                break;
            case 7:
                _abb = "Jul";
                break;
            case 8:
                _abb = "Aug";
                break;
            case 9:
                _abb = "Sept";
                break;
            case 10:
                _abb = "Oct";
                break;
            case 11:
                _abb = "Nov";
                break;
            case 12:
                _abb = "Dec";
                break;
        }

        return _abb;
    }

    public int GetDay()
    {
        return _day;
    }

    public int GetMonth()
    {
        return _month;
    }

    public int GetYear()
    {
        return _year;
    }

    public string GetDayOfWeek()
    {
        return _dayOfWeek;
    }

    public string GetDayOfWeekAbb()
    {
        string _text = "";

        switch (_dayOfWeek)
        {
            case "Monday":
                _text = "Mon";
                break;
            case "Tuesday":
                _text = "Tues";
                break;
            case "Wednesday":
                _text = "Wed";
                break;
            case "Thursday":
                _text = "Thurs";
                break;
            case "Friday":
                _text = "Fri";
                break;
            case "Saturday":
                _text = "Sat";
                break;
            default:
                _text = "Sun";
                break;
        }

        return _text;
    }

    public int GetDayOfWeekNumber()
    {
        int _value;

        switch(_dayOfWeek)
        {
            case "Monday":
                _value = 1;
                break;
            case "Tuesday":
                _value = 2;
                break;
            case "Wednesday":
                _value = 3;
                break;
            case "Thursday":
                _value = 4;
                break;
            case "Friday":
                _value = 5;
                break;
            case "Saturday":
                _value = 6;
                break;
            default:
                _value = 0;
                break;
        }

        return _value;
    }

    public void SetDay(int _input)
    {
        _day = _input;

        ReviseDate();
    }

    public void SetMonth(int _input)
    {
        _month = _input;

        ReviseDate();
    }

    public void SetYear(int _input)
    {
        _year = _input;

        ReviseDate();
    }

    public int GetMaxDays()
    {
        int _maxDays = 31;

        if(_month == 2)
        {
            _maxDays = 28;

            if((_year % 4) == 0)
            {
                _maxDays += 1;
            }
        }
        else if(_month == 4 || _maxDays == 6 || _maxDays == 9 || _maxDays == 11)
        {
            _maxDays = 30;
        }

        return _maxDays;
    }

    public string GetDateText(MonthDisplayEnum _monthDisplayInput = MonthDisplayEnum.Number)
    {
        string _finalText = "";

        string _dayText = _day.ToString("00");

        string _monthText;

        switch(_monthDisplayInput)
        {
            case MonthDisplayEnum.Name:
                _monthText = GetMonthName();
                break;
            case MonthDisplayEnum.Abbreviation:
                _monthText = GetMonthAbbreviation();
                break;
            default:
                _monthText = _month.ToString("00");
                break;
        }

        string _yearText = _year.ToString("0000");

        _finalText = _dayText + "/" + _monthText + "/" + _yearText;

        return _finalText;
    }

    public bool GetDateSet()
    {
        return (_year != -1 && _year != 0 && _month != -1 && _month != 0 && _day != -1 && _day != 0);
    }

    public void SetDate(int _dayInput, int _monthInput, int _yearInput)
    {
        _year = _yearInput;
        _month = _monthInput;
        _day = _dayInput;

        ReviseDate();
    }

    public void SetDateFromString(string _input)
    {
        int _stage = 0;

        string _dayText = "";
        string _monthText = "";
        string _yearText = "";

        for(int _i = 0; _i < _input.Length; _i++)
        {
            if (_input[_i] == '/' || _input[_i] == '-')
            {
                _stage++;
            }
            else
            {
                if(_stage == 0)
                {
                    _dayText += _input[_i];
                }
                else if(_stage == 1 && _input[_i] != '.')
                {
                    _monthText += _input[_i];
                }
                else if(_stage == 2)
                {
                    _yearText += _input[_i];
                }
            }
        }

        int.TryParse(_dayText, out _day);

        SetMonthFromString(_monthText);

        int.TryParse(_yearText, out _year);
    }

    void SetMonthFromString(string _input)
    {
        if (_input.Equals("Jan") || _input.Equals("January") || _input.Equals("1") || _input.Equals("01"))
        {
            _month = 1;
        }
        else if (_input.Equals("Feb") || _input.Equals("February") || _input.Equals("2") || _input.Equals("02"))
        {
            _month = 2;
        }
        else if (_input.Equals("Mar") || _input.Equals("March") || _input.Equals("3") || _input.Equals("03"))
        {
            _month = 3;
        }
        else if (_input.Equals("Apr") || _input.Equals("April") || _input.Equals("4") || _input.Equals("04"))
        {
            _month = 4;
        }
        else if (_input.Equals("May") || _input.Equals("5") || _input.Equals("05"))
        {
            _month = 5;
        }
        else if (_input.Equals("Jun") || _input.Equals("June") || _input.Equals("6") || _input.Equals("06"))
        {
            _month = 6;
        }
        else if (_input.Equals("Jul") || _input.Equals("July") || _input.Equals("7") || _input.Equals("07"))
        {
            _month = 7;
        }
        else if (_input.Equals("Aug") || _input.Equals("August") || _input.Equals("8") || _input.Equals("08"))
        {
            _month = 8;
        }
        else if(_input.Equals("Sept") || _input.Equals("September") || _input.Equals("9") || _input.Equals("09"))
        {
            _month = 9;
        }
        else if(_input.Equals("Oct") || _input.Equals("October") || _input.Equals("10"))
        {
            _month = 10;
        }
        else if(_input.Equals("Nov") || _input.Equals("November") || _input.Equals("11"))
        {
            _month = 11;
        }
        else if(_input.Equals("Dec") || _input.Equals("December") || _input.Equals("12"))
        {
            _month = 12;
        }
    }

    void GetDayOfWeekInDate()
    {
        if (GetDateSet())
        {
            Debug.Log("The date is Year: " + _year.ToString() + ", Month: " + _month.ToString() + ", and Day: " + _day.ToString() + ".");

            DateTime _dt = new DateTime(_year, _month, _day);

            _dayOfWeek = _dt.DayOfWeek.ToString();
        }
    }
}

[System.Serializable]
public class TwoVariablesClass<T, U>
{
    [SerializeField]
    protected T _variable1;

    [SerializeField]
    protected U _variable2;

    protected bool _valuesChanged = false;

    public TwoVariablesClass()
    {

    }

    public TwoVariablesClass(T _input1, U _input2)
    {
        _variable1 = _input1;

        _variable2 = _input2;

        _valuesChanged = true;
    }

    public T GetVariable1()
    {
        return _variable1;
    }

    public U GetVariable2()
    {
        return _variable2;
    }

    public bool GetValuesChanged()
    {
        return _valuesChanged;
    }

    public void SetVariable1(T _input)
    {
        _variable1 = _input;

        _valuesChanged = true;
    }

    public void SetVariable2(U _input)
    {
        _variable2 = _input;

        _valuesChanged = true;
    }

    public void SetVariables(T _input1, U _input2)
    {
        _variable1 = _input1;
        _variable2 = _input2;

        _valuesChanged = true;
    }

    public void ValuesChecked()
    {
        _valuesChanged = false;
    }
}

[System.Serializable]
public class LanguageClass
{
    [SerializeField]
    protected LanguageEnum _language;

    [SerializeField]
    protected Sprite _flagSprite;

    public LanguageEnum GetLanguage()
    {
        return _language;
    }

    public Sprite GetFlagSprite()
    {
        return _flagSprite;
    }
}


[System.Serializable]
public class MainCanvasesClass
{
    [SerializeField]
    Canvas _mainPlayerCanvas;

    [SerializeField]
    Canvas _indicatorCanvas;

    [SerializeField]
    Canvas _doctorCanvas;

    bool _canvasesOn = true;

    public Canvas GetMainPlayerCanvas()
    {
        return _mainPlayerCanvas;
    }

    public Canvas GetIndicatorCanvas()
    {
        return _indicatorCanvas;
    }

    public Canvas GetDoctorCanvas()
    {
        return _doctorCanvas;
    }

    public bool GetCanvasesOn()
    {
        return _canvasesOn;
    }

    public void SetCanvasesOn(bool _input)
    {
        _canvasesOn = _input;

        if(_mainPlayerCanvas != null)
        {
            _mainPlayerCanvas.gameObject.SetActive(_canvasesOn);
        }

        if(_indicatorCanvas != null)
        {
            ProcessForIndicatorCanvas();
        }
    }

    public void SwitchCanvasesOn()
    {
        bool _switch = !_canvasesOn;

        SetCanvasesOn(_switch);
    }

    void ProcessForIndicatorCanvas()
    {
        if(SettingsManager.GetInstance() == null)
        {
            _indicatorCanvas.gameObject.SetActive(_canvasesOn);

            return;
        }

        BooleanSettingsClass _setting = SettingsManager.GetInstance().GetBooleanSettingByName("Indicator On");

        if(_setting == null)
        {
            _indicatorCanvas.gameObject.SetActive(_canvasesOn);

            return;
        }

        bool _secondValue = _setting.GetValue();

        _indicatorCanvas.gameObject.SetActive(_canvasesOn && _secondValue);
    }
}

[System.Serializable]
public class MinMaxV3ValuesClass
{
    [SerializeField]
    TwoVariablesClass<float, float> _xRange;

    [SerializeField]
    TwoVariablesClass<float, float> _yRange;

    [SerializeField]
    TwoVariablesClass<float, float> _zRange;

    bool _valueChanged;

    public TwoVariablesClass<float, float> GetXRange()
    {
        return _xRange;
    }

    public TwoVariablesClass <float, float> GetYRange()
    {
        return _yRange;
    }

    public TwoVariablesClass<float, float> GetZRange()
    {
        return _zRange;
    }

    public float GetRandomValueForX()
    {
        float _v = UnityEngine.Random.Range(_xRange.GetVariable1(), _xRange.GetVariable2());

        return _v;
    }

    public float GetRandomValueForY()
    {
        float _v = UnityEngine.Random.Range(_yRange.GetVariable1(), _yRange.GetVariable2());

        return _v;
    }

    public float GetRandomValueForZ()
    {
        float _v = UnityEngine.Random.Range(_zRange.GetVariable1(), _zRange.GetVariable2());

        return _v;
    }

    public Vector3 GetRandomVector3()
    {
        Vector3 _v = new Vector3(GetRandomValueForX(), GetRandomValueForY(), GetRandomValueForZ());

        return _v;
    }

    public void MaintainValues()
    {
        _valueChanged = _xRange.GetValuesChanged() || _yRange.GetValuesChanged() || _zRange.GetValuesChanged();

        if(!_valueChanged)
        {
            return;
        }

        if(_xRange.GetVariable2() < _xRange.GetVariable1())
        {
            float _s1 = _xRange.GetVariable1();

            float _s2 = _xRange.GetVariable2();

            _xRange.SetVariables(_s2, _s1);

            _xRange.ValuesChecked();
        }

        if(_yRange.GetVariable2() < _yRange.GetVariable1())
        {
            float _s1 = _yRange.GetVariable1();

            float _s2 = _yRange.GetVariable2();

            _yRange.SetVariables(_s2, _s1);

            _yRange.ValuesChecked();
        }

        if(_zRange.GetVariable2() < _zRange.GetVariable1())
        {
            float _s1 = _zRange.GetVariable1();

            float _s2 = _zRange.GetVariable2();

            _zRange.SetVariables(_s2, _s1);

            _zRange.ValuesChecked();
        }
    }
}

[Serializable]
public class MatchingBlockAndHoleClass
{

}

[Serializable]
public class MatchingBlockAndHoleClass<T, U, V>: MatchingBlockAndHoleClass where T : MatchingGameBlockScript where U : MatchingGameHoleScript
{
    [SerializeField]
    protected T _gameBlock;

    [SerializeField]
    protected U _gameBlockHole;

    [SerializeField]
    protected V _matchingAttribute;

    public MatchingBlockAndHoleClass()
    {

    }

    public MatchingBlockAndHoleClass(T _blockInput, U _holeInput, V _matchingAttributeInput)
    {
        _gameBlock = _blockInput;

        _gameBlockHole = _holeInput;

        _matchingAttribute = _matchingAttributeInput;
    }

    public T GetGameBlock()
    {
        return _gameBlock;
    }

    public U GetGameBlockHole()
    {
        return _gameBlockHole;
    }

    public V GetMatchingAttribute()
    {
        return _matchingAttribute;
    }
}

public class InhalerMatchingBlockAndHoleClass:MatchingBlockAndHoleClass<InhalerMatchingObjectScript, InhalerMatchingObjectHoleScript, string>
{
    public InhalerMatchingBlockAndHoleClass():base()
    {

    }

    public InhalerMatchingBlockAndHoleClass(InhalerMatchingObjectScript _blockInput, InhalerMatchingObjectHoleScript _holeInput,  string _matchingAttributeInput): base(_blockInput, _holeInput, _matchingAttributeInput)
    {

    }
}

[Serializable]
public class WordMatchingBlockAndHoleClass : MatchingBlockAndHoleClass<LetterBlockScript, LetterHoleScript, char>
{

}


[Serializable]
public class CurrentBlocksAndHolesClass
{
    [SerializeField]
    protected List<GameObject> _blockGOs;

    [SerializeField]
    protected List<GameObject> _holeGOs;

    [SerializeField]
    protected bool _reverseLists;

    protected GameObject _selectedBlockGO = null;

    protected GameObject _selectedHoleGO = null;

    protected int _totalBlocksAndHoles = 0;

    public List<GameObject> GetBlockGOs()
    {
        return _blockGOs;
    }

    public List<GameObject> GetHoleGOs()
    {
        return _holeGOs;
    }

    public GameObject GetSelectedBlockGameObject()
    {
        return _selectedBlockGO;
    }

    public GameObject GetSelectedHoleGameObject()
    {
        return _selectedHoleGO;
    }

    public bool GetBlockSelected()
    {
        return (_selectedBlockGO != null);
    }

    public bool GetHoleSelected()
    {
        return (_selectedHoleGO != null);
    }

    public int GetTotalBlocksAndHoles()
    {
        return _totalBlocksAndHoles;
    }

    public bool GetReverseListsBoolean()
    {
        return _reverseLists;
    }

    public void IncreaseTotalBlocksAndHoles()
    {
        _totalBlocksAndHoles++;
    }

    public virtual void ClearLists()
    {
        foreach(GameObject _go in _blockGOs)
        {
            GameObject.Destroy(_go);
        }

        foreach(GameObject _go in _holeGOs)
        {
            GameObject.Destroy(_go);
        }

        _blockGOs.Clear();

        _holeGOs.Clear();

        _selectedBlockGO = null;

        _selectedHoleGO = null;

        _totalBlocksAndHoles = 0;
    }
}


[Serializable]
public class CurrentBlocksAndHolesClass<T, U> : CurrentBlocksAndHolesClass where T : MatchingGameBlockScript where U : MatchingGameHoleScript
{
    [SerializeField]
    protected List<T> _blocks;

    [SerializeField]
    protected List<U> _holes;

    [SerializeField]
    protected T _selectedBlock;

    [SerializeField]
    protected U _selectedHole;

    public List<T> GetBlocks()
    {
        return _blocks;
    }

    public List<U> GetHoles()
    {
        return _holes;
    }

    public T GetSelectedBlock()
    {
        return _selectedBlock;
    }

    public U GetSelectedHole()
    {
        return _selectedHole;
    }

    public void AddBlock(T _input)
    {
        _blocks.Add(_input);
        
        _blockGOs.Add(_input.gameObject);
    }

    public void AddHole(U _input)
    {
        _holes.Add(_input);
        
        _holeGOs.Add(_input.gameObject);
    }

    public void SetSelectedBlock(T _input)
    {
        _selectedBlock = _input;

        _selectedBlockGO = _input == null ? null : _input.gameObject;
    }

    public void SetSelectedHole(U _input)
    {
        _selectedHole = _input;

        _selectedHoleGO = _input == null ? null : _input.gameObject;
    }

    public override void ClearLists()
    {
        base.ClearLists();

        _blocks.Clear();

        _holes.Clear();
    }

    public void ReverseLists()
    {
        List<T> _list1 = new List<T>();

        for(int _i = (_blocks.Count); _i > 0; _i--)
        {
            _list1.Add(_blocks[_i - 1]);
        }

        _blocks = _list1;

        List<U> _list2 = new List<U>();

        for(int _i = (_holes.Count); _i > 0; _i--)
        {
            _list2.Add(_holes[_i - 1]);
        }

        _holes = _list2;

        List<GameObject> _list3 = new List<GameObject>();

        for(int _i = (_blockGOs.Count); _i > 0; _i--)
        {
            _list3.Add(_blockGOs[_i - 1]);
        }

        _blockGOs = _list3;

        _list3.Clear();

        for(int _i = (_holeGOs.Count); _i > 0; _i--)
        {
            _list3.Add(_holeGOs[_i - 1]);
        }

        _holeGOs = _list3;
    }
}

[Serializable]
public class CurrentBlocksAndHolesForInhalerClass : CurrentBlocksAndHolesClass<InhalerMatchingObjectScript, InhalerMatchingObjectHoleScript>
{

}

[Serializable]
public class CurrentBlocksAndHolesDefaultClass : CurrentBlocksAndHolesClass<MatchingGameBlockScript, MatchingGameHoleScript>
{

}

[Serializable]
public class CurrentBlocksAndHolesForLetterClass : CurrentBlocksAndHolesClass<LetterBlockScript, LetterHoleScript>
{

}

[System.Serializable]
public class UIButtonCircleClass
{
    [SerializeField]
    float _spreadingDegrees = 180.0f;

    [SerializeField]
    float _additionalDegrees;

    [SerializeField]
    float _radius = 100.0f;

    [SerializeField]
    bool _counterClockwise = true;

    [SerializeField]
    bool _switchAxes;

    [SerializeField]
    float _defaultAngle = 90.0f;

    public float GetSpreadingDegrees()
    {
        return _spreadingDegrees;
    }

    public float GetAdditionalDegrees()
    {
        return _additionalDegrees;
    }

    public float GetRadius()
    {
        return _radius;
    }

    public bool GetIsCounterClockwise()
    {
        return _counterClockwise;
    }

    public bool GetSwitchAxes()
    {
        return _switchAxes;
    }

    public float GetDefaultAngle()
    {
        return _defaultAngle;
    }
}

[System.Serializable]
public class ErrorIdentifierClass
{
    [SerializeField]
    public bool _debugIdentifierBool;

    [SerializeField]
    public Text _textField;

    [SerializeField]
    public List<string> _textsToDisplay;

    public string _name;

    public bool DisplayHere()
    {
        return (_debugIdentifierBool && _textField != null);
    }

    public void DisplayTextByIndex(int _input)
    {
        if(!DisplayHere())
        {
            return;
        }

        if(_input == -1)
        {
            _textField.text = "";
        }
        else if(_input >= 0 && _input < _textsToDisplay.Count)
        {
            _textField.text = _textsToDisplay[_input];
        }
    }
}

[System.Serializable]
public class ActionPlanEnumLabelProperties<T> where T : Enum
{
    [SerializeField]
    protected T _enum;

    [SerializeField]
    protected List<ActionPlanEnumValueLabelProperties<T>> _labelProperties;

    public ActionPlanEnumLabelProperties()
    {
        //InitializeAtConstructor();
    }


    public T GetEnum()
    {
        return _enum;
    }

    public List<ActionPlanEnumValueLabelProperties<T>> GetValueLabelProperties()
    {
        return _labelProperties;
    }

    public void SetEnum(T _input)
    {
        _enum = _input;
    }

    /*void InitializeAtConstructor()
    {
        int _count = Enum.GetValues(typeof(T)).Length;

        ActionPlanEnumValueLabelProperties<T> _newValue;

        T _currentValue;

        for (int _i = 0; _i < _count; _i++)
        {
            _newValue = new ActionPlanEnumValueLabelProperties<T>();
        }
    }*/
}

[System.Serializable]
public class ActionPlanEnumValueLabelProperties<T> where T : Enum
{
    [SerializeField]
    protected T _enumValue;

    [SerializeField]
    protected Color _color;

    [SerializeField]
    protected string _alternativeText;

    [SerializeField]
    protected Color _textColor;

    [SerializeField]
    protected float _textSize = 130.0f;

    [SerializeField]
    protected Sprite _labelSprite;

    public T GetEnumValue()
    {
        return _enumValue;
    }

    public Color GetColor()
    {
        return _color;
    }

    public string GetAlternativeText()
    {
        if(_alternativeText == "")
        {
            return _enumValue.DisplayName();
        }

        return _alternativeText;
    }

    public Color GetTextColor()
    {
        return _textColor;
    }

    public float GetTextSize()
    {
        return _textSize;
    }

    public void SetEnumValue(T _input)
    {
        _enumValue = _input;
    }

    public Sprite GetLabelSprite()
    {
        return _labelSprite;
    }
}

[System.Serializable]
public class EnumActionPlanLabelStylesClass <T> where T : Enum
{
    [SerializeField]
    protected List<EnumDropdownLabelStyleClass<T>> _styles;

    APEnumTypeEnum _type;

    public List<EnumDropdownLabelStyleClass<T>> GetStyles()
    {
        return _styles;
    }

    public EnumActionPlanLabelStylesClass()
    {
        if(typeof(T) == typeof(InhalerColorEnum))
        {
            _type = APEnumTypeEnum.Inhaler_Color;
        }

        if(typeof(T) == typeof(GenderEnum))
        {
            _type = APEnumTypeEnum.Gender;
        }
    }

    public APEnumTypeEnum GetEnumType()
    {
        return _type;
    }
}

[System.Serializable]
public class EnumDropdownLabelStyleClass<T> where T : Enum
{
    [SerializeField]
    protected T _value;

    [SerializeField]
    protected Color _labelColor;

    [SerializeField]
    protected Color _textColor;

    [SerializeField]
    protected int _textSize = 130;

    [SerializeField]
    protected string _alternativeText;

    [SerializeField]
    protected Sprite _labelSprite;

    [SerializeField]
    protected Color _outlineColor;

    [SerializeField]
    protected Vector2 _outlineEffect = new Vector2(1.0f, -1.0f);

    public T GetValue()
    {
        return _value;
    }

    public Color GetLabelColor()
    {
        return _labelColor;
    }

    public Color GetTextColor()
    {
        return _textColor;
    }

    public int GetTextSize()
    {
        return _textSize;
    }

    public string GetAlternativeText()
    {
        if(_alternativeText == "")
        {
            return _value.ToString();
        }

        return _alternativeText;
    }

    public Sprite GetLabelSprite()
    {
        return _labelSprite;
    }

    public Color GetOutlineColor()
    {
        return _outlineColor;
    }

    public Vector2 GetOutlineEffect()
    {
        return _outlineEffect;
    }
}