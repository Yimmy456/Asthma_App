using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

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

    //[SerializeField]
    //Gradient _textBorderGradient;

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

        _c = ToolsStruct.ChangeColorValue(_c, 0.5f, 0.5f);

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

    [SerializeField]
    protected InfoCategoryEnum _infoCategory;

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

    public InfoCategoryEnum GetInfoCategory()
    {
        return _infoCategory;
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

    public DefinitionClass()
    {
        _informationType = InformationTypeEnum.Definition;
    }

    public Sprite GetSprite()
    {
        return _sprite;
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
public class CircleClass
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
            return _enumValue.ToString();
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
    protected int _textSizeOnMainLabel = 140;

    [SerializeField]
    protected string _alternativeText;

    [SerializeField]
    protected Sprite _labelSprite;

    [SerializeField]
    protected Color _outlineColor;

    [SerializeField]
    protected Color _labelSpriteColor = Color.white;

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

    public int GetTextSizeOnMainLabel()
    {
        return _textSizeOnMainLabel;
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

    public Color GetLabelSpriteColor()
    {
        return _labelSpriteColor;
    }
}

[System.Serializable]
public class TimedTextPropertiesClass : UITextPropertiesClass
{
    [SerializeField]
    float _textTimeToDisplay = 5.0f;

    public float GetTextTimeToDisplay()
    {
        return _textTimeToDisplay;
    }
}

[System.Serializable]
public class HighlightingAnimationClass : BasicAnimationClass
{
    [SerializeField]
    Material _highlightingMaterial1;

    [SerializeField]
    Material _highlightingMaterial2;

    [SerializeField]
    Color _highlightingMaterialColor;

    [SerializeField]
    float _additionalThicknessForMat2 = 0.5f;

    [SerializeField]
    float _colorValueForMaterial2 = 0.5f;

    [SerializeField]
    TwoVariablesClass<float, float> _range;

    [SerializeField]
    TwoVariablesClass<float, float> _alphas = new TwoVariablesClass<float, float>(1.0f, 1.0f);

    public Material GetHighlightingMaterial1()
    {
        return _highlightingMaterial1;
    }

    public Material GetHighlightingMaterial2()
    {
        return _highlightingMaterial2;
    }

    public Color GetHighlightingMaterialColor()
    {
        return _highlightingMaterialColor;
    }

    public float GetAdditionalThicknessForMaterial2()
    {
        return _additionalThicknessForMat2;
    }

    public bool GetAnimate()
    {
        return _animateBoolean;
    }

    public TwoVariablesClass<float, float> GetRange()
    {
        return _range;
    }

    public float GetColorValueForMaterial2()
    {
        return _colorValueForMaterial2;
    }

    public TwoVariablesClass<float, float> GetAlphas()
    {
        return _alphas;
    }

    public void SetHighlightingMaterialColor(Color _input)
    {
        _highlightingMaterialColor = _input;
    }

    public void SetAdditionalThicknessForMaterial2(float _input)
    {
        _additionalThicknessForMat2 = _input;
    }

    public void SetColorValueForMaterial2(float _input)
    {
        _colorValueForMaterial2 = _input;
    }


    public override IEnumerator Animate()
    {
        if(_highlightingMaterial1 == null)
        {
            yield break;
        }

        _animateBoolean = true;

        //CheckValues();

        float _currentSize = _range.GetVariable1();

        float _nextSize = _range.GetVariable2();

        float _lerp;

        float _difference = Math.Abs(_nextSize - _currentSize);

        _t = 0.0f;

        Color _c2 = ToolsStruct.ChangeColorValue(_highlightingMaterialColor, _colorValueForMaterial2, 1.0f, true);

        _highlightingMaterial1.SetColor("_Base_Color", _highlightingMaterialColor);

        _highlightingMaterial1.SetFloat("_Alpha", _alphas.GetVariable1());

        //_highlightingMaterial1.SetFloat("_Outline_Thickness", _currentValue);

        if (_highlightingMaterial2 != null)
        {
            _highlightingMaterial2.SetColor("_Base_Color", _c2);

            _highlightingMaterial2.SetFloat("_Alpha", _alphas.GetVariable2());

            //_highlightingMaterial1.SetFloat("_Outline_Thickness", _currentValue2);
        }

        while(_animateBoolean)
        {
            if(_pauseAnimation)
            {
                yield return null;

                continue;
            }

            _delta = _animationSpeed;

            if(_withDeltaTime)
            {
                _delta = _delta * Time.deltaTime;
            }

            if(_discardDifference && _difference > 0.0f)
            {
                _delta = _delta / _difference;
            }

            _t = _t + _delta;

            if(_t >= 1.0f)
            {
                _t = 1.0f;

                _switch = true;
            }

            _lerp = ((_nextSize * _t) - (_currentSize * _t)) + _currentSize;

            _highlightingMaterial1.SetFloat("_Outline_Thickness", _lerp);

            if (_highlightingMaterial2 != null && _additionalThicknessForMat2 > 0.0f)
            {
                _highlightingMaterial2.SetFloat("_Outline_Thickness", (_lerp + _additionalThicknessForMat2));
            }

            if(_switch)
            {
                float _sw = _nextSize;

                _nextSize = _currentSize;

                _currentSize = _sw;

                _t = 0.0f;

                _switch = false;
            }

            yield return null;
        }

        _highlightingMaterial1.SetFloat("_Outline_Thickness", _range.GetVariable1());

        _highlightingMaterial1.SetFloat("_Alpha", 0.0f);

        _t = 0.0f;

        if (_highlightingMaterial2 != null)
        {
            _highlightingMaterial1.SetFloat("_Outline_Thickness", (_range.GetVariable1() + _additionalThicknessForMat2));

            _highlightingMaterial2.SetFloat("_Alpha", 0.0f);
        }

        _animateBoolean = false;
    }

    void CheckValues()
    {
        if(_range.GetVariable1() > _range.GetVariable2())
        {
            float _v  = _range.GetVariable2();

            _range.SetVariable2(_range.GetVariable1());

            _range.SetVariable1(_v);
        }
    }
}

[System.Serializable]
public class ArrowAnimationClass : BasicAnimationClass
{
    [SerializeField]
    protected GameObject _arrowContainer;

    [SerializeField]
    protected GameObject _arrowObject;

    [SerializeField]
    protected Vector3 _pointA;

    [SerializeField]
    protected Vector3 _pointB;

    [SerializeField]
    protected Space _spaceType;

    [SerializeField]
    protected GameObject _number3DObject;


    //Getters
    public GameObject GetArrowContainer() { return _arrowContainer; }

    public GameObject GetArrowObject() { return _arrowObject; }

    public Vector3 GetPointA() { return _pointA; }

    public Vector3 GetPointB() { return _pointB; }

    public Space GetSpaceType() { return _spaceType; }

    public GameObject GetNumber3DObject() { return _number3DObject; }


    //Setters
    public void SetArrowContainer(GameObject _input)
    {
        _arrowContainer = _input;
    }

    public void SetArrowObject(GameObject _input)
    {
        _arrowObject = _input;
    }

    public void SetPointA(Vector3 _input) { _pointA = _input; }

    public void SetPointB(Vector3 _input) { _pointB = _input; }

    public void SetSpaceType(Space _input) { _spaceType = _input; }

    public void SetNumber3DObject(GameObject _input) { _number3DObject = _input; }

    public override IEnumerator Animate()
    {
        if(_arrowContainer == null || _arrowObject == null)
        {
            yield break;
        }

        _animateBoolean = true;

        Vector3 _currentLocation = _pointA;

        Vector3 _nextTarget = _pointB;

        _t = 0.0f;

        _switch = false;

        if(_spaceType == Space.World)
        {
            _arrowObject.transform.position = _currentLocation;
        }
        else
        {
            _arrowObject.transform.localPosition = _currentLocation;
        }

        Vector3 _positionLerp = Vector3.zero;

        float _distance = Vector3.Distance(_pointA, _pointB);

        if(_distance <= 0.0f)
        {
            _animateBoolean = false;

            yield break;
        }

        while(_animateBoolean)
        {
            if(_pauseAnimation)
            {
                //continue;

                yield return null;

                continue;
            }

            _delta = _animationSpeed;

            if(_withDeltaTime)
            {
                _delta = _delta * Time.deltaTime;
            }

            if(_discardDifference && _distance > 0.0f)
            {
                _delta = _delta / _distance;
            }

            _t = _t + _delta;

            if (_t >= 1.0f)
            {
                _t = 1.0f;

                _switch = true;
            }

            _positionLerp = ((_t * _nextTarget) - (_t * _currentLocation)) + _currentLocation;

            if(_spaceType == Space.World)
            {
                _arrowObject.transform.position = _positionLerp;
            }
            else
            {
                _arrowObject.transform.localPosition = _positionLerp;
            }

            if(_switch)
            {
                Vector3 _swV3 = _nextTarget;

                _nextTarget = _currentLocation;

                _currentLocation = _swV3;

                _t = 0.0f;

                _switch = false;
            }

            yield return null;
        }

        if(_spaceType == Space.World)
        {
            _arrowObject.transform.position = _pointA;
        }
        else
        {
            _arrowObject.transform.localPosition = _pointA;
        }

        _animateBoolean = false;
    }

}

[System.Serializable]
public class BasicAnimationClass
{
    [SerializeField]
    protected float _animationSpeed = 1.0f;

    [SerializeField]
    protected bool _withDeltaTime = true;

    [SerializeField]
    protected bool _discardDifference = true;

    protected bool _animateBoolean;

    protected bool _pauseAnimation;

    protected float _t = 0.0f;

    protected bool _switch;

    protected float _delta = 0.0f;
    
    public float GetAnimationSpeed()
    {
        return _animationSpeed;
    }

    public bool GetWithDeltaTime()
    {
        return _withDeltaTime;
    }

    public bool GetDiscardDifference()
    {
        return _discardDifference;
    }

    public bool GetAnimateBoolean()
    {
        return _animateBoolean;
    }

    public bool GetPauseAnimation()
    {
        return _pauseAnimation;
    }

    public float GetT()
    {
        return _t;
    }

    public float GetDelta()
    {
        return _delta;
    }

    public void SetAnimationSpeed(float _input)
    {
        _animationSpeed = _input;
    }

    public void SetWithDeltaTime(bool _input)
    {
        _withDeltaTime = _input;
    }

    public void SetDiscardDifference(bool _input)
    {
        _discardDifference = _input;
    }

    public void SetAnimateBoolean(bool _input)
    {
        _animateBoolean = _input;
    }

    public void SetPauseAnimation(bool _input)
    {
        _pauseAnimation = _input;
    }

    public void SwitchAnimateBoolean()
    {
        _animateBoolean = !_animateBoolean;
    }

    public void SwitchPauseAnimation()
    {
        _pauseAnimation = !_pauseAnimation;
    }

    public virtual IEnumerator Animate()
    {
        while (_animateBoolean)
        {
            yield return null;
        }
    }
}

[System.Serializable]
public class InhalerPropertiesClass
{
    [SerializeField]
    GameObject _inhalerGO;

    [SerializeField]
    Vector3 _inhalerInitialPosition;

    [SerializeField]
    Button _inhalerShowingButton;

    [SerializeField]
    Sprite _showInhalerButtonSprite;

    [SerializeField]
    Sprite _hideInhalerButtonSprite;

    [SerializeField]
    UITextPropertiesClass _textPropertiesToShowInhaler;

    [SerializeField]
    UITextPropertiesClass _textPropertiesToHideInhaler;

    bool _showingInhalerBoolean = false;

    public GameObject GetInhalerGameObject()
    {
        return _inhalerGO;
    }

    public Vector3 GetInhalerInitialPosition()
    {
        return _inhalerInitialPosition;
    }

    public Button GetInhalerShowingButton()
    {
        return _inhalerShowingButton;
    }

    public Sprite GetHideInhalerButtonSprite()
    {
        return _hideInhalerButtonSprite;
    }

    public Sprite GetShowInhalerButtonSprite()
    {
        return _showInhalerButtonSprite;
    }

    public bool GetShowingInhalerBoolean()
    {
        return _showingInhalerBoolean;
    }

    public void SetShowingInhalerBoolean(bool _input)
    {
        _showingInhalerBoolean = _input;

        UpdateInhalerByBoolean();
    }

    public void SwitchShowingInhalerBoolean()
    {
        _showingInhalerBoolean = !_showingInhalerBoolean;

        UpdateInhalerByBoolean();
    }

    void UpdateInhalerByBoolean()
    {
        if(_inhalerGO == null)
        {
            return;
        }

        //1. Setting the activeness of the GO.
        _inhalerGO.SetActive(_showingInhalerBoolean);

        //2.Setting the sprite of the button.
        if (_inhalerShowingButton != null)
        {
            Sprite _selectedSprite = _showingInhalerBoolean ? _hideInhalerButtonSprite : _showInhalerButtonSprite;

            if (_selectedSprite != null)
            {
                _inhalerShowingButton.image.sprite = _selectedSprite;
            }
        }

        //3. Setting the text of the button.
        UITextPropertiesClass _selectedUItextProperties = _showingInhalerBoolean ? _textPropertiesToHideInhaler : _textPropertiesToShowInhaler;

        _selectedUItextProperties.SetTextProperties("", false);

        //4. Returning the GO to the initial position.
        if(!_showingInhalerBoolean)
        {
            _inhalerGO.transform.localPosition = _inhalerInitialPosition;
        }
    }
}


[System.Serializable]
public class UITextPropertiesClass
{
    [SerializeField]
    protected Text _uiText;

    [SerializeField]
    protected string _uiTextString;

    [SerializeField]
    protected Color _uiTextColor;

    [SerializeField]
    protected Outline _uiTextOutline;

    [SerializeField]
    protected Vector2 _uiTextOutlineDistance = new Vector2(1.0f, -1.0f);

    [SerializeField]
    protected float _uiTextOutlineAlpha = 0.5f;

    [SerializeField]
    protected float _uiTextOutlineColorValue = 0.5f;

    public Text GetUIText()
    {
        return _uiText;
    }

    public string GetUITextString()
    {
        return _uiTextString;
    }

    public Color GetUTextColor()
    {
        return _uiTextColor;
    }

    public Outline GetUITextOutline()
    {
        return _uiTextOutline;
    }

    public Vector2 GetUITextOutlineDistance()
    {
        return _uiTextOutlineDistance;
    }

    public float GetUITextOutlineAlpha()
    {
        return _uiTextOutlineAlpha;
    }

    public float GetUITextOutlineColorValue()
    {
        return _uiTextOutlineColorValue;
    }

    public void SetTextString(string _input)
    {
        if(_uiText == null)
        {
            return;
        }

        _uiTextString = _input;

        _uiText.text = _uiTextString;
    }

    public void SetUITextColor(Color _input)
    {
        _uiTextColor = _input;
    }

    public void SetUITextOutlineDistance(Vector2 _input)
    {
        _uiTextOutlineDistance = _input;
    }

    public void SetUITextOutlineDistance(float _inputX, float _inputY)
    {
        SetUITextOutlineDistance(new Vector2(_inputX, _inputY));
    }

    public void SetUITextOutlineAlpha(float _input)
    {
        _uiTextOutlineAlpha = _input;
    }

    public void SetUITextOutlineColorValue(float _input)
    {
        _uiTextOutlineColorValue = _input;
    }

    public void SetTextProperties(string _input, bool _appendBoolean = false)
    {
        if(_uiText == null)
        {
            return;
        }

        if(_input != "")
        {
            if (_appendBoolean)
            {
                _uiTextString = _uiTextString + _input;
            }
            else
            {
                _uiTextString = _input;
            }
        }

        _uiText.text = _uiTextString;

        _uiText.color = _uiTextColor;

        if(_uiTextOutline == null)
        {
            return;
        }

        _uiTextOutline.effectDistance = _uiTextOutlineDistance;

        Color _outlineColor = ToolsStruct.ChangeColorValue(_uiTextColor, _uiTextOutlineColorValue, _uiTextOutlineAlpha, true);

        _uiTextOutline.effectColor = _outlineColor;
    }

    public void AppendText(string _input)
    {
        if(_uiText == null)
        {
            return;
        }

        _uiTextString = _uiTextString + _input;

        _uiText.text = _uiTextString;
    }
}

[System.Serializable]
public class ShrinkAndExpandAnimationClass : BasicAnimationClass
{
    [SerializeField]
    GameObject _gameObject;

    [SerializeField]
    Vector3 _maxSize = Vector3.one;

    [SerializeField]
    float _maxSizeConstant = 1.0f;

    [SerializeField]
    IncreasOrDecreaseEnum _increaseOrDecrease;

    public GameObject GetGameObject()
    {
        return _gameObject;
    }

    public Vector3 GetMaxSize() { return _maxSize; }

    public float GetMaxSizeConstant() { return _maxSizeConstant; }

    public IncreasOrDecreaseEnum GetIncreaseOrDecrease()
    {
        return _increaseOrDecrease;
    }

    public override IEnumerator Animate()
    {
        if(_gameObject == null)
        {
            _animateBoolean = false;

            yield break;
        }

        _animateBoolean = true;

        _increaseOrDecrease = IncreasOrDecreaseEnum.Increase;

        Vector3 _finalSize = _maxSize * _maxSizeConstant;

        _gameObject.SetActive(true);

        while(_t < 1.0f)
        {
            if(_pauseAnimation)
            {
                yield return null;

                continue;
            }

            _delta = _animationSpeed;

            if(_withDeltaTime)
            {
                _delta = _delta * Time.deltaTime;
            }

            _t = _t + _delta;

            if(_t >= 1.0f)
            {
                _t = 1.0f;
            }

            _gameObject.transform.localScale = (_finalSize * _t);

            yield return null;
        }

        _increaseOrDecrease = IncreasOrDecreaseEnum.None;

        _animateBoolean = false;
    }

    public IEnumerator ReverseAnimate()
    {
        if (_gameObject == null)
        {
            _animateBoolean = false;

            yield break;
        }

        _animateBoolean = true;

        _increaseOrDecrease = IncreasOrDecreaseEnum.Decrease;

        Vector3 _finalSize = _maxSize * _maxSizeConstant;

        while (_t > 0.0f)
        {
            if (_pauseAnimation)
            {
                yield return null;

                continue;
            }

            _delta = _animationSpeed;

            if (_withDeltaTime)
            {
                _delta = _delta * Time.deltaTime;
            }

            _t = _t - _delta;

            if (_t <= 0.0f)
            {
                _t = 0.0f;
            }

            _gameObject.transform.localScale = (_finalSize * _t);

            yield return null;
        }

        _gameObject.SetActive(false);

        _increaseOrDecrease = IncreasOrDecreaseEnum.None;

        _animateBoolean = false;
    }
}

[System.Serializable]
public class ArrowAnimationClass2: ArrowAnimationClass
{
    [SerializeField]
    Vector3 _pointC;

    [SerializeField]
    float _updateAnimationSpeed = 5.0f;

    int _stage = 0;

    public Vector3 GetPointC()
    { return _pointC; }

    public float GetUpdateAnimationSpeed()
    {
        return _updateAnimationSpeed;
    }

    public int GetStage()
    {
        return _stage;
    }

    public void EndAnimation()
    {
        if(!_animateBoolean || _stage == 1 || _stage == 2)
        {
            _stage = 3;
        }
    }

    public void AbortAnimation()
    {
        _stage = 0;

        _arrowObject.SetActive(false);

        if(_spaceType == Space.World)
        {
            _arrowObject.transform.position = _pointA;
        }
        else
        {
            _arrowObject.transform.localPosition = _pointA;
        }

        _t = 0.0f;

        _switch = false;

        _pauseAnimation = false;

        _animateBoolean = false;
    }

    public override IEnumerator Animate()
    {
        if(_arrowObject == null)
        {
            _animateBoolean = false;

            yield break;
        }

        _stage = 1;

        _animateBoolean = true;

        _delta = _animationSpeed;

        _t = 0.0f;

        Vector3 _lerp = _pointA;

        _arrowObject.SetActive(true);

        if(_withDeltaTime)
        {
            _delta = _delta * Time.deltaTime;
        }

        float _distance = Vector3.Distance(_pointA, _pointC);

        if(_discardDifference && _distance > 0.0f)
        {
            _delta = _delta / _distance;
        }

        while(_stage == 1)
        {
            if(_pauseAnimation)
            {
                yield return null;

                continue;
            }

            _t = _t + _delta;

            if(_t >= 1.0f)
            {
                _t = 1.0f;

                _stage = 2;
            }

            _lerp = Vector3.Lerp(_pointA, _pointC, _t);

            if(_spaceType == Space.World)
            {
                _arrowObject.transform.position = _lerp;
            }
            else
            {
                _arrowObject.transform.localPosition = _lerp;
            }

            yield return null;
        }

        Vector3 _currentPos = _pointC;

        Vector3 _nextPos = _pointB;

        _t = 0.0f;

        _delta = _updateAnimationSpeed;

        if(_withDeltaTime)
        {
            _delta = _delta * Time.deltaTime;
        }

        _distance = Vector3.Distance(_pointB, _pointC);

        if(_discardDifference && _distance > 0.0f)
        {
            _delta = _delta / _distance;
        }

        _switch = false;

        while(_stage == 2)
        {
            if(_pauseAnimation)
            {
                yield return null;

                continue;
            }

            _t = _t + _delta;

            if(_t >= 1.0f)
            {
                _t = 1.0f;

                _switch = true;
            }

            _lerp = Vector3.Lerp(_currentPos, _nextPos, _t);

            if(_spaceType == Space.World)
            {
                _arrowObject.transform.position = _lerp;
            }
            else
            {
                _arrowObject.transform.localPosition = _lerp;
            }

            if(_switch)
            {
                Vector3 _v3 = _nextPos;

                _nextPos = _currentPos;

                _currentPos = _v3;

                _switch = false;

                _t = 0.0f;
            }

            yield return null;
        }

        _distance = Vector3.Distance(_pointA, _pointC);

        if(_spaceType == Space.World && _distance > 0.0f)
        {
            _t = Vector3.Distance(_arrowObject.transform.position, _pointC) / _distance;
        }
        else if(_distance > 0.0f)
        {
            _t = Vector3.Distance(_arrowObject.transform.localPosition, _pointC) / _distance;
        }

        _delta = _animationSpeed;

        if(_withDeltaTime)
        {
            _delta = _delta * Time.deltaTime;
        }

        if(_discardDifference && _distance > 0.0f)
        {
            _delta = _delta / _distance;
        }

        while(_stage == 3)
        {
            if (_pauseAnimation)
            {
                yield return null;

                continue;
            }

            _t = _t + _delta;

            if (_t >= 1.0f)
            {
                _t = 1.0f;

                _stage = 0;
            }

            _lerp = Vector3.Lerp(_pointC, _pointA, _t);

            if (_spaceType == Space.World)
            {
                _arrowObject.transform.position = _lerp;
            }
            else
            {
                _arrowObject.transform.localPosition = _lerp;
            }

            yield return null;
        }

        _arrowObject.SetActive(false);

        _animateBoolean = false;
    }
}

[System.Serializable]
public class DoctorSalemDialoguesClass
{
    [SerializeField]
    protected Animator _doctorAnimator;

    [SerializeField]
    protected AudioSource _doctorAudioSource;

    [SerializeField]
    protected AudioClip _doctorDefaultAudioClip;

    protected static bool _isDoctorTalking = false;

    protected Coroutine _talkingCoroutine;

    public Animator GetDoctorAnimator()
    {
        return _doctorAnimator;
    }

    public AudioSource GetDoctorAudioSource()
    {
        return _doctorAudioSource;
    }

    public AudioClip GetAudioClip()
    {
        return _doctorDefaultAudioClip;
    }

    public static bool GetIsDoctorTalking()
    {
        return _isDoctorTalking;
    }

    public Coroutine GetTalkingCoroutine()
    {
        return _talkingCoroutine;
    }

    public void SetDoctorAnimator(Animator _input)
    {
        _doctorAnimator = _input;
    }

    public void SetDoctorAudioSource(AudioSource _input)
    {
        _doctorAudioSource = _input;
    }

    public void SetDoctorDefaultAudioClip(AudioClip _input)
    {
        _doctorDefaultAudioClip = _input;
    }

    public static void SetIsDoctorTalking(bool _input)
    {
        _isDoctorTalking = _input;
    }

    public void SetTalkingCoroutine(Coroutine _input)
    {
        _talkingCoroutine = _input;
    }

    public IEnumerator TalkCoroutine(AudioClip _clipInput = null, bool _setDefaultValueInput = true)
    {
        if(_clipInput == null && _setDefaultValueInput)
        {
            _clipInput = _doctorDefaultAudioClip;
        }

        if(_doctorAudioSource == null || _clipInput == null || _doctorAnimator == null)
        {
            yield break;
        }

        _doctorAudioSource.clip = _clipInput;

        _isDoctorTalking = true;

        float _seconds = _clipInput.length;

        _doctorAnimator.SetBool("Talking", true);

        _doctorAudioSource.Play();

        yield return new WaitForSeconds(_seconds);

        _isDoctorTalking = false;

        _doctorAnimator.SetBool("Talking", false);
    }
}