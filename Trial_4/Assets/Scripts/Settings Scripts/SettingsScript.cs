using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SettingsScript: IDataPersistenceScript
{
    [SerializeField]
    protected string _settingName;

    [SerializeField]
    protected string _settingID;

    protected SettingsTypeEnum _settingsType;

    [SerializeField]
    protected bool _saveAfterConfirming = true;

    public void LoadData(GameDataScript _input)
    {
        _input._settingsValues.TryGetValue(_settingID, out var _value);

        SetValueFromStringForm(_value);

        _input.AddSetting(this);
    }

    public void SaveData(ref GameDataScript _input)
    {
        if(_input._settingsValues.ContainsKey(_settingID))
        {
            _input._settingsValues.Remove(_settingID);
        }

        string _st = "";

        if(_settingsType == SettingsTypeEnum.Boolean || _settingsType == SettingsTypeEnum.Enum)
        {
            int _int = GetIntegerValue();

            _st = _int.ToString();
        }

        if(_settingsType == SettingsTypeEnum.Decimal)
        {
            _st = GetDecimalValue().ToString("0.00");
        }

        _input._settingsValues.Add(_settingID, _st);
    }

    public string GetSettingName()
    {
        return _settingName;
    }

    public string GetSettingID()
    {
        return _settingID;
    }

    public virtual string GetValueInStringForm()
    {
        return "";
    }

    public SettingsTypeEnum GetSettingType()
    {
        return _settingsType;
    }

    public bool GetSaveAfterConfirming()
    {
        return _saveAfterConfirming;
    }

    public virtual void SetValueFromStringForm(string _input)
    {

    }

    public void SetSettingID()
    {
        if (_settingID == "")
        {
            string _st = System.Guid.NewGuid().ToString();

            _settingID = _st;
        }
    }

    public virtual bool GetBooleanValue()
    {
        return true;
    }

    public virtual float GetDecimalValue()
    {
        return 0.0f;
    }

    public virtual int GetIntegerValue()
    {
        return 0;
    }

    public virtual void SetValueByInteger(int _input)
    {

    }

    public void SetSaveAfterConfirming(bool _input)
    {
        _saveAfterConfirming = _input;
    }

    public IEnumerator LoadSetting()
    {
        yield return new WaitForSeconds(1.0f);

        if(DataPersistenceManager.GetInstance() == null)
        {
            yield break;
        }

        if(!DataPersistenceManager.GetInstance().GetGameData()._settingsValues.ContainsKey(_settingID))
        {
            DataPersistenceManager.GetInstance().GetGameData()._settingsValues.Add(_settingID, GetValueInStringForm());
        }

        string _value = DataPersistenceManager.GetInstance().GetGameData()._settingsValues[_settingID];

        SetValueFromStringForm(_value);
    }
}
