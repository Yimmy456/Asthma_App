using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{

    static SettingsManager _instance;

    [SerializeReference]
    List<SettingsScript> _settings = new List<SettingsScript>();

    [SerializeField]
    SettingsCanvasScript _canvas;

    [SerializeField]
    List<LanguageClass> _languages;

    [ContextMenu("Give IDs to settings.")]
    void GiveIDs()
    {
        for(int _i = 0; _i < _settings.Count; _i++)
        {
            _settings[_i].SetSettingID();
        }
    }

    [ContextMenu("Add Boolean Setting")]
    void AddBooleanSetting()
    {
        _settings.Add(new BooleanSettingsClass());
    }

    [ContextMenu("Add Decimal Setting")]
    void AddDecimalSetting()
    {
        _settings.Add(new DecimalSettingsClass());
    }

    [ContextMenu("Add Enum Setting/Language")]
    void AddLanguageSetting()
    {
        _settings.Add(new LanguageSettingClass2());
    }

    void Start()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        foreach(SettingsScript _s in _settings)
        {
            StartCoroutine(_s.LoadSetting());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public static SettingsManager GetInstance()
    {
        return _instance;
    }

    public SettingsCanvasScript GetCanvas()
    {
        return _canvas;
    }

    public void SetCanvas(SettingsCanvasScript _input)
    {
        _canvas = _input;
    }

    public List<SettingsScript> GetSettings()
    {
        return _settings;
    }

    public List<LanguageClass> GetLanguages()
    {
        return _languages;
    }

    public LanguageClass GetMatchingLanguage(LanguageEnum _input)
    {
        foreach(LanguageClass _l in _languages)
        {
            if(_l.GetLanguage() == _input)
            {
                return _l;
            }
        }

        return null;
    }

    public LanguageClass GetMatchingLanguage(int _input)
    {
        if(!(_input >= 0 && _input < Enum.GetValues(typeof(LanguageEnum)).Length))
        {
            return null;
        }

        LanguageEnum _i = (LanguageEnum)(_input);

        return GetMatchingLanguage(_i);
    }

    public void SaveResults(SerializableDictionaryScript<string, string> _input)
    {
        if(DataPersistenceManager.GetInstance() == null)
        {
            return;
        }

        if(DataPersistenceManager.GetInstance().GetGameData() == null)
        {
            return;
        }

        GameDataScript _d = DataPersistenceManager.GetInstance().GetGameData();

        bool _idMatches = false;

        string _currentId = "";

        for(int _i = 0; _i < _settings.Count; _i++)
        {
            _currentId = _input.ElementAt(_i).Key;

            for(int _j = 0; _j < _settings.Count && !_idMatches; _j++)
            {
                if (string.Compare(_settings[_j].GetSettingID(), _currentId, false) == 0)
                {
                    if (_settings[_j].GetSaveAfterConfirming())
                    {
                        _settings[_j].SetValueFromStringForm(_input.ElementAt(_i).Value);

                        _settings[_j].SaveData(ref _d);
                    }
                    _idMatches = true;
                }
            }

            _idMatches = false;
        }

        DataPersistenceManager.GetInstance().SaveGame();
    }

    public SettingsScript GetSettingByName(string _nameInput, bool _capsInput = true)
    {
        for(int _i = 0; _i < _settings.Count; _i++)
        {
            if (string.Compare(_settings[_i].GetSettingName(), _nameInput, _capsInput) == 0)
            {
                return _settings[_i];
            }
        }

        return null;
    }

    public BooleanSettingsClass GetBooleanSettingByName(string _nameInput, bool _capsInput = true)
    {
        SettingsScript _currentSetting;

        for(int _i = 0; _i < _settings.Count; _i++)
        {
            _currentSetting = _settings[_i];

            if(string.Compare(_currentSetting.GetSettingName(), _nameInput, _capsInput) == 0)
            {
                if(_currentSetting.GetSettingType() == SettingsTypeEnum.Boolean)
                {
                    return _currentSetting as BooleanSettingsClass;
                }
            }
        }
        return null;
    }

    public DecimalSettingsClass GetDecimalSettingByName(string _nameInput, bool _capsInput = true)
    {
        SettingsScript _currentSettings2;

        for(int _i = 0; _i < _settings.Count; _i++)
        {
            _currentSettings2 = _settings[_i];

            if(string.Compare(_currentSettings2.GetSettingName(), _nameInput, _capsInput) == 0)
            {
                if(_currentSettings2.GetSettingType() == SettingsTypeEnum.Decimal)
                {
                    return _currentSettings2 as DecimalSettingsClass;
                }
            }
        }

        return null;
    }
}
