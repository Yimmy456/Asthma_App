using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BookScript : MonoBehaviour
{
    static BookScript _instance;

    [SerializeField]
    List<InformationClass> _generalInformation;

    [SerializeField]
    List<DefinitionClass> _definitions;

    [SerializeField]
    Sprite _infoCardFrontMaterial;

    [SerializeField]
    MeterClass _informationLearnedMeter;

    List<InformationClass> _allInformation;

    // Start is called before the first frame update
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

        SetAllInformation();
    }

    public static BookScript GetInstance()
    {
        return _instance;
    }

    public List<InformationClass> GetGeneralInformation()
    {
        return _generalInformation;
    }

    public List<DefinitionClass> GetDefinitions()
    {
        return _definitions;
    }

    public List <InformationClass> GetAllInformation()
    {
        return _allInformation;
    }

    public Sprite GetInfoCardFrontMaterial()
    {
        return _infoCardFrontMaterial;
    }

    public InformationClass GetGeneralInformationByName(string _input)
    {
        foreach(InformationClass _info in _generalInformation)
        {
            bool _boolean = string.Compare(_input, _info.GetInformationName(), true) == 0.0f;

            if(_boolean)
            {
                return _info;
            }
        }
        return null;
    }

    public DefinitionClass GetDefinitionByName(string _input)
    {
        foreach(DefinitionClass _def in _definitions)
        {
            bool _boolean = string.Compare(_input, _def.GetInformationName(), true) == 0.0f;

            if(_boolean)
            {
                return _def;
            }
        }
        return null;
    }

    public List<InformationClass> GetGeneralInformationByCategory(InfoCategoryEnum _input)
    {
        List<InformationClass> _list = new List<InformationClass>();

        for(int _i = 0; _i < _generalInformation.Count; _i++)
        {
            InformationClass _info = _generalInformation[_i];

            if(_info.GetInfoCategory() == _input)
            {
                _list.Add(_info);
            }
        }
        return _list;
    }

    public List<DefinitionClass> GetWordsByType(WordTypeEnum _input)
    {
        List<DefinitionClass> _words = new List<DefinitionClass>();

        foreach(DefinitionClass _word in _definitions)
        {
            if(_word.GetWordType() == _input)
            {
                _words.Add(_word);
            }
        }

        return _words;
    }

    public List<DefinitionClass> GetDefinitionsByCategory(InfoCategoryEnum _input)
    {
        List<DefinitionClass> _defs = new List<DefinitionClass>();

        foreach(DefinitionClass _def in _definitions)
        {
            if(_input == _def.GetInfoCategory())
            {
                _defs.Add(_def);
            }
        }

        return _defs;
    }

    public List<DefinitionClass> GetDefinitionsByCategory(int _input)
    {
        InfoCategoryEnum _var;

        int _valueCount = Enum.GetValues(typeof(InfoCategoryEnum)).Length;

        int _i = _input;

        if(_input == -1)
        {
            _i = UnityEngine.Random.Range(0, _valueCount);
        }
        else if(_input == -2)
        {
            _i = UnityEngine.Random.Range(1, _valueCount);
        }

        switch(_i)
        {
            case 1:
                _var = InfoCategoryEnum.Trigger;
                break;
            case 2:
                _var = InfoCategoryEnum.Symptom;
                break;
            case 3:
                _var = InfoCategoryEnum.Treatment;
                break;
            default:
                _var = InfoCategoryEnum.None;
                break;
        }

        return GetDefinitionsByCategory(_var);
    }

    public List<DefinitionClass> GetDefinitionsByCategory(string _input)
    {   
        InfoCategoryEnum _var = InfoCategoryEnum.None;

        if(string.Compare(_input, "Random") == 0.0f)
        {
            return GetDefinitionsByCategory(-2);
        }

        if(string.Compare(_input, "Trigger") == 0.0f || string.Compare(_input, "Triggers") == 0.0f)
        {
            _var = InfoCategoryEnum.Trigger;
        }
        else if(string.Compare(_input, "Symptom") == 0.0f || string.Compare(_input, "Symptoms") == 0.0f)
        {
            _var = InfoCategoryEnum.Symptom;
        }
        else if (string.Compare(_input, "Treatment") == 0.0f || string.Compare(_input, "Treatments") == 0.0f || string.Compare(_input, "TTT") == 0.0f)
        {
            _var = InfoCategoryEnum.Treatment;
        }

        if(_var == InfoCategoryEnum.None)
        {   
            return new List<DefinitionClass>();
        }

        return GetDefinitionsByCategory(_var);
    }

    public InformationClass GetInformationByName(string _input)
    {
        foreach(InformationClass _info in _allInformation)
        {
            bool _boolean = string.Compare(_input, _info.GetInformationName(), true) == 0.0f;

            if(_boolean)
            {
                return _info;
            }
        }
        return null;
    }

    public List<InformationClass> GetInformationExamples(InformationClass _input)
    {
        List<InformationClass> _list = new List<InformationClass>();

        for(int _i = 0; _i < _allInformation.Count; _i++)
        {
            InformationClass _currentInfo = _allInformation[_i];

            if(_currentInfo == _input)
            {
                continue;
            }

            for(int _j = 0; _j < _input.GetInformationExamples().Count; _j++)
            {
                string _currentExample = _input.GetInformationExamples()[_j];

                if (string.Compare(_currentInfo.GetInformationName(), _currentExample, true) == 0.0f && !_list.Contains(_currentInfo))
                {
                    _list.Add(_currentInfo);
                }
            }
        }
        return _list;
    }

    void SetAllInformation()
    {
        _allInformation = _generalInformation;

        _allInformation.AddRange(_definitions);

        _informationLearnedMeter.SetMaxValue(_allInformation.Count);

        UpdateMeter();
    }

    public DefinitionClass GetRandomDefinition()
    {
        int _l = _definitions.Count;

        int _rand = UnityEngine.Random.Range(0, _l);

        return _definitions[_rand];
    }

    public List<InformationClass> GetInformationLearned(bool _input = true)
    {
        List<InformationClass> _list = new List<InformationClass>();

        foreach(InformationClass _info in _allInformation)
        {
            if(_info.GetInformationLearned() == _input)
            {
                _list.Add(_info);
            }
        }
        return _list;
    }

    public List<InformationClass> GetInformationLearned(InformationTypeEnum _typeInput = InformationTypeEnum.General, bool _learnedInput = true)
    {
        List<InformationClass> _list = new List<InformationClass>();

        foreach(InformationClass _info in _allInformation)
        {
            if(_info.GetInformationType() == _typeInput && _info.GetInformationLearned() == _learnedInput)
            {
                _list.Add(_info);
            }
        }

        return _list;
    }

    public void UpdateInformationLearned(InformationClass _infoInput, bool _learnedInput = true)
    {
        if(_infoInput == null)
        {
            return;
        }

        _infoInput.SetInformationLearned(_learnedInput);

        UpdateMeter();
    }

    void UpdateMeter()
    {
        _informationLearnedMeter.SetValue(0);

        foreach(InformationClass _info in _allInformation)
        {
            if(_info.GetInformationLearned())
            {
                _informationLearnedMeter.AddToValue(1);
            }
        }
    }
}
