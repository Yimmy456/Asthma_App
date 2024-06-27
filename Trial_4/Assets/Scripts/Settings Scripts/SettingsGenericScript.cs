using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsGenericClass <T>: SettingsScript
{
    [SerializeField]
    protected T _value;

    public T GetValue()
    {
        return _value;
    }

    public virtual void SetValue(T _input)
    {
        _value = _input;
    }
}

public class BooleanSettingsClass : SettingsGenericClass<bool>
{
    public BooleanSettingsClass()
    {
        _value = false;

        _settingsType = SettingsTypeEnum.Boolean;
    }

    public BooleanSettingsClass(bool _input)
    {
        _value = _input;

        _settingsType = SettingsTypeEnum.Boolean;
    }

    public override string GetValueInStringForm()
    {
        int _intValue = _value ? 1 : 0;

        return _intValue.ToString();
    }

    public override void SetValueFromStringForm(string _input)
    {
        _value = (string.Compare(_input, "1", true) == 0) ? true : false;
    }

    public override bool GetBooleanValue()
    {
        return _value;
    }

    public override int GetIntegerValue()
    {
        if(_value)
        {
            return 1;
        }

        return 0;
    }

    public override void SetValueByInteger(int _input)
    {
        _value = _input == 1 ? true : false;
    }
}

public class DecimalSettingsClass : SettingsGenericClass<float>
{
    [SerializeField]
    float _minimumValue = 0.0f;

    [SerializeField]
    float _maximumValue = 10.0f;

    public DecimalSettingsClass()
    {
        _value = 0.0f;

        _settingsType = SettingsTypeEnum.Decimal;
    }

    public DecimalSettingsClass(float _input)
    {
        _value = _input;

        _settingsType = SettingsTypeEnum.Decimal;
    }

    public override float GetDecimalValue()
    {
        return _value;
    }

    public override void SetValue(float _input)
    {
        base.SetValue(_input);

        MaintainValues();
    }

    void MaintainValues()
    {
        if(_maximumValue == 0.0f && _minimumValue == 0.0f)
        {
            return;
        }

        if(_value < _minimumValue)
        {
            _value = _minimumValue;
        }

        if(_value > _maximumValue)
        {
            _value = _maximumValue;
        }
    }

    public override string GetValueInStringForm()
    {
        return _value.ToString("0.00");
    }

    public override void SetValueFromStringForm(string _input)
    {
        float.TryParse(_input, out float _v);

        _value = _v;
    }

    public bool GetIsRanged()
    {
        return (!(_minimumValue == 0.0f && _maximumValue == 0.0f));
    }

    public float GetMinValue()
    {
        return _minimumValue;
    }

    public float GetMaxValue()
    {
        return _maximumValue;
    }
}

public class LanguageSettingClass : SettingsGenericClass<LanguageClass>
{
    public LanguageSettingClass()
    {
        _settingsType = SettingsTypeEnum.Enum;
    }

    public override string GetValueInStringForm()
    {
        return _value.GetLanguage().ToString();
    }

    public override void SetValueByInteger(int _input)
    {
        if(SettingsManager.GetInstance() == null)
        {
            return;
        }

        _value = SettingsManager.GetInstance().GetLanguages()[_input];
    }

    public override void SetValueFromStringForm(string _input)
    {
        if(SettingsManager.GetInstance() == null)
        {
            return;
        }

        bool _languageFound = false;

        List<LanguageClass> _lang = SettingsManager.GetInstance().GetLanguages();

        for(int _i = 0; _i < _lang.Count && !_languageFound; _i++)
        {
            if (string.Compare(_lang[_i].GetLanguage().ToString(), _input, true) == 0)
            {
                _value = _lang[_i];

                _languageFound = true;
            }
        }
    }
}

public class LanguageSettingClass2 : SettingsGenericClass<LanguageEnum>
{
    public LanguageSettingClass2()
    {
        _settingsType = SettingsTypeEnum.Enum;
    }

    public override string GetValueInStringForm()
    {
        int _v = (int)_value;

        return _v.ToString();
    }

    public override float GetDecimalValue()
    {
        float _n = (int)_value;

        return _n;
    }

    public override int GetIntegerValue()
    {
        return (int)_value;
    }

    public void SetToNextValue()
    {
        int _n = (int)_value;

        _n++;

        if(_n >= Enum.GetValues(typeof(LanguageEnum)).Length)
        {
            _n = 0;
        }

        _value = (LanguageEnum)(_n);
    }

    public void SetToPreviousValue()
    {
        int _n = (int)_value;

        _n--;

        if(_n < 0)
        {
            _n = (Enum.GetValues(typeof(LanguageEnum)).Length - 1);
        }

        _value = (LanguageEnum)(_n);
    }

    public override void SetValueFromStringForm(string _input)
    {
        int.TryParse(_input, out int _output);

        _value = (LanguageEnum)(_output);
    }

    public LanguageClass GetMatchingClass()
    {
        if(SettingsManager.GetInstance() == null)
        {
            return null;
        }

        List<LanguageClass> _list = SettingsManager.GetInstance().GetLanguages();

        foreach(LanguageClass _language in _list)
        {
            if(_language.GetLanguage() == _value)
            {
                return _language;
            }
        }

        return null;
    }
}
