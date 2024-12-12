using System.Collections;
using System.Collections.Generic;
//using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class GameDataScript
{
    public SerializableDictionaryScript<string, bool> _badgesCollected;

    public SerializableDictionaryScript<string, string> _actionPlanAnswers;

    public SerializableDictionaryScript<string, string> _settingsValues;

    public bool _userTypeSelected;

    public UserTypeEnum _userType;

    public GameDataScript()
    {
        //_badges = new SerializableDictionaryScript<string, bool>();

        _userType = UserTypeEnum.Random_User;

        _userTypeSelected = false;

        _badgesCollected = new SerializableDictionaryScript<string, bool>();

        _actionPlanAnswers = new SerializableDictionaryScript<string, string>();

        _settingsValues = new SerializableDictionaryScript<string, string>();
    }

    public void ConfirmUserTypeStatus(UserTypeEnum _input)
    {
        _userType = _input;

        _userTypeSelected = true;
    }

    public void AddBadge(BadgeScript _input)
    {
        if(!_badgesCollected.ContainsKey(_input.GetBadgeID()))
        {
            _badgesCollected.Add(_input.GetBadgeID(), _input.GetBadgeCollected());
        }
    }

    public void AddQuestionAnswer(ActionPlanQuestionScript _input)
    {
        string _answerValue = "";

        if(_input.GetVariableType() == VariableTypeForAPEnum.Integer || _input.GetVariableType() == VariableTypeForAPEnum.Enum)
        {
            _answerValue = _input.GetIntegerAnswer().ToString();
        }

        if(_input.GetVariableType() == VariableTypeForAPEnum.Decimal)
        {
            _answerValue = _input.GetDecimalAnswer().ToString("0.00");
        }

        if(_input.GetVariableType() == VariableTypeForAPEnum.Text)
        {
            _answerValue = _input.GetTextAnswer();
        }

        if(!_actionPlanAnswers.ContainsKey(_input.GetQuestionID()))
        {
            _actionPlanAnswers.Add(_input.GetQuestionID(), _answerValue);
        }
    }

    public void LoadSetting(SettingsScript _input)
    {
        string _settingValue = "";

        if(_input.GetSettingType() == SettingsTypeEnum.Boolean)
        {
            _settingValue = _input.GetBooleanValue().ToString();

            _input.SetValueFromStringForm(_settingValue);
        }

        if(_input.GetSettingType() == SettingsTypeEnum.Decimal)
        {
            _settingValue = _input.GetDecimalValue().ToString("0.00");

            _input.SetValueFromStringForm(_settingValue);
        }
    }

    public void AddSetting(SettingsScript _input)
    {
        if (!_settingsValues.ContainsKey(_input.GetSettingID()))
        {
            _settingsValues.Add(_input.GetSettingID(), _input.GetValueInStringForm());
        }
    }

    public string GetDataInStringForm()
    {
        string _st1 = _badgesCollected.ToString();

        string _st2 = _actionPlanAnswers.ToString();

        return (_st1 + _st2);
    }
}
