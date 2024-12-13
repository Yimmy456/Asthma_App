using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
//using TMPro;
using Unity.VisualScripting;
//using UnityEditor.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsCanvasScript : MonoBehaviour
{
    [SerializeField]
    ScrollRect _scrollRect;

    [SerializeField]
    RectTransform _togglePanel;

    [SerializeField]
    RectTransform _countryEnumPanel;

    [SerializeField]
    RectTransform _decimalValuePanel;

    [SerializeField]
    RectTransform _contentTransform;

    [SerializeField]
    List<GameObject> _panels = new List<GameObject>();

    [SerializeField]
    SerializableDictionaryScript<string, string> _keysAndValues;

    [SerializeField]
    List<InputField> _rangedInputFields;

    [SerializeField]
    List<Vector2> _rangedValues;

    [SerializeField]
    Scrollbar _verticalScrollBar;

    LanguageEnum _selectedLanguage;

    // Start is called before the first frame update
    void Start()
    {
        if(SettingsManager.GetInstance() != null)
        {
            if (SettingsManager.GetInstance().GetCanvas() == null)
            {
                SettingsManager.GetInstance().SetCanvas(this);
            }
        }

        if(_keysAndValues == null)
        {
            _keysAndValues = new SerializableDictionaryScript<string, string>();
        }
    }

    private void OnEnable()
    {
        if(_keysAndValues == null)
        {
            _keysAndValues = new SerializableDictionaryScript<string, string>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        MaintainRanges();
    }

    public void LoadSettings()
    {
        if(!IsSettingsManagerReady())
        {
            return;
        }
    }

    bool IsSettingsManagerReady()
    {
        if(SettingsManager.GetInstance() == null)
        {
            return false;
        }

        return true;
    }

    public void PreparePanels()
    {
        if(SettingsManager.GetInstance() == null || _contentTransform == null)
        {
            return;
        }

        for(int _i = 0; _i < SettingsManager.GetInstance().GetSettings().Count; _i++)
        {
            SettingsScript _currentSetting = SettingsManager.GetInstance().GetSettings()[_i];

            if(_currentSetting.GetSettingType() == SettingsTypeEnum.Boolean)
            {
                LoadIfBoolean(ref _currentSetting, (_i + 1));
            }

            if(_currentSetting.GetSettingType() == SettingsTypeEnum.Decimal)
            {
                LoadIfDecimal(ref _currentSetting, (_i + 1));
            }

            if(_currentSetting.GetSettingType() == SettingsTypeEnum.Enum)
            {
                LoadIfEnum(ref _currentSetting, (_i + 1));
            }

            _keysAndValues.Add(_currentSetting.GetSettingID(), _currentSetting.GetValueInStringForm());
        }

        AdditionalRules();

        if(_verticalScrollBar != null)
        {
            _verticalScrollBar.value = 1.0f;
        }
    }

    public void EndSettingsSession()
    {
        foreach(GameObject _go in _panels)
        {
            Destroy(_go);
        }

        _panels.Clear();

        _keysAndValues.Clear();
    }

    void MaintainRanges()
    {
        if(_rangedInputFields.Count != _rangedValues.Count)
        {
            return;
        }

        for(int _i = 0; _i < _rangedInputFields.Count; _i++)
        {
            if (_rangedInputFields[_i] == null)
            {
                continue;
            }

            if (_rangedInputFields[_i].isFocused)
            {
                continue;
            }

            float.TryParse(_rangedInputFields[_i].text, out float _v);

            if(_v >= _rangedValues[_i].x && _v <= _rangedValues[_i].y)
            {
                continue;
            }

            if(_v < _rangedValues[_i].x)
            {
                _rangedInputFields[_i].text = _rangedValues[_i].x.ToString("0.00");
            }

            if(_v > _rangedValues[_i].y)
            {
                _rangedInputFields[_i].text = _rangedValues[_i].y.ToString("0.00");
            }
        }
    }

    void LoadIfBoolean(ref SettingsScript _settingInput, int _indexInput)
    {
        if(_settingInput == null || _togglePanel == null)
        {
            return;
        }

        BooleanSettingsClass _boolS = (BooleanSettingsClass)_settingInput;

        if (_boolS == null)
        {
            return;
        }

        GameObject _panelI = Instantiate(_togglePanel.gameObject);

        RectTransform _rectT = _panelI.GetComponent<RectTransform>();

        string _stValue = DataPersistenceManager.GetInstance().GetGameData()._settingsValues[_settingInput.GetSettingID()];

        if(_rectT == null)
        {
            Destroy(_panelI.gameObject);

            return;
        }

        Text _indexText = _rectT.Find("Index Text").gameObject.GetComponent<Text>();

        Text _text = _rectT.Find("Text").gameObject.GetComponent<Text>();

        ToggleScript _toggle = _rectT.Find("Toggle").gameObject.GetComponent<ToggleScript>();

        if(_indexText == null || _text == null || _toggle == null)
        {
            Destroy(_panelI.gameObject);

            return;
        }

        _rectT.SetParent(_contentTransform);

        _panelI.SetActive(true);

        _rectT.anchoredPosition3D = Vector3.zero;

        _rectT.localRotation = Quaternion.identity;

        _rectT.localScale = Vector3.one;

        _panels.Add(_panelI);

        _indexText.text = _indexInput.ToString() + ".";

        _text.text = _settingInput.GetSettingName();

        _boolS.SetValueFromStringForm(_stValue);

        _toggle.GetToggle().isOn = _boolS.GetValue();

        string _id = _settingInput.GetSettingID();

        _toggle.GetToggle().onValueChanged.AddListener(delegate {
        
            int _v = _toggle.GetToggle().isOn ? 1 : 0;

            _keysAndValues[_id] = _v.ToString();
        });

        _panelI.name = _settingInput.GetSettingName() + " Panel";
    }

    void LoadIfDecimal(ref SettingsScript _settingInput, int _indexInput)
    {
        if(_settingInput == null || _decimalValuePanel == null)
        {
            return;
        }

        DecimalSettingsClass _decimalS = (DecimalSettingsClass)_settingInput;

        if(_decimalS == null)
        {
            return;
        }

        GameObject _panelI = Instantiate(_decimalValuePanel.gameObject);

        RectTransform _rectT = _panelI.GetComponent<RectTransform>();

        string _st = DataPersistenceManager.GetInstance().GetGameData()._settingsValues[_settingInput.GetSettingID()];

        if(_rectT == null)
        {
            Destroy(_panelI);

            return;
        }

        _rectT.SetParent(_contentTransform);

        _panelI.SetActive(true);

        _rectT.anchoredPosition3D = Vector3.zero;

        _rectT.localRotation = Quaternion.identity;

        _rectT.localScale = Vector3.one;

        Text _text = _rectT.Find("Text").gameObject.GetComponent<Text>();

        Text _indexText = _rectT.Find("Index Text").gameObject.GetComponent<Text>();

        InputField _inputField = _rectT.Find("Input Field").gameObject.GetComponent<InputField>();

        Slider _slider = _rectT.Find("Slider").gameObject.GetComponent<Slider>();

        string _id = _settingInput.GetSettingID();

        if (_text == null || _indexText == null || _inputField == null)
        {
            Destroy(_panelI);

            return;
        }

        _indexText.text = _indexInput.ToString() + ".";

        _text.text = _settingInput.GetSettingName();

        _decimalS.SetValueFromStringForm(_st);

        _inputField.text = _decimalS.GetValue().ToString("0.00");

        _panelI.name = _settingInput.GetSettingName() + " Panel";

        _inputField.onValueChanged.AddListener(delegate
        {
            float.TryParse(_inputField.text, out float _flV);

            _keysAndValues[_id] = _flV.ToString("0.00");
        });

        _panels.Add(_panelI);

        string _placeHolderText = "Insert value here...";

        if (_slider == null)
        {
            if (_inputField.placeholder.GetComponent<Text>() != null)
            {
                _inputField.placeholder.GetComponent<Text>().text = _placeHolderText;
            }

            return;
        }

        if(!_decimalS.GetIsRanged() || _slider.gameObject.GetComponent<InteractedScript>() == null)
        {
            _slider.gameObject.SetActive(false);

            if(_inputField.placeholder.GetComponent<Text>() != null)
            {
                _inputField.placeholder.GetComponent<Text>().text = _placeHolderText;
            }

            return;
        }

        InteractedScript _interaction = _slider.gameObject.GetComponent<InteractedScript>();

        _slider.maxValue = _decimalS.GetMaxValue();

        _slider.minValue = _decimalS.GetMinValue();

        _slider.value = _decimalS.GetValue();

        _inputField.onValueChanged.AddListener(delegate
        {
            float _flV = 0.0f;

            if (_inputField.isFocused)
            {
                float.TryParse(_inputField.text, out _flV);

                _keysAndValues[_id] = _flV.ToString("0.00");
                
                _slider.value = _flV;
            }
        });

        _slider.onValueChanged.AddListener(delegate
        {
            if (!_inputField.isFocused)
            {
                float _vl = _slider.value;

                _keysAndValues[_id] = _vl.ToString("0.00");

                if (_interaction)
                {
                    _inputField.text = _vl.ToString("0.00");
                }
            }
        });

        _rangedInputFields.Add(_inputField);

        _rangedValues.Add(new Vector2(_decimalS.GetMinValue(), _decimalS.GetMaxValue()));

        _placeHolderText = "Insert value here or use slider...";

        if(_inputField.placeholder.GetComponent<Text>() != null)
        {
            _inputField.placeholder.GetComponent<Text>().text = _placeHolderText;
        }
    }

    void LoadIfEnum(ref SettingsScript _settingInput, int _indexInput)
    {
        if(_settingInput == null || _countryEnumPanel == null)
        {
            return;
        }

        LanguageSettingClass2 _lang = (LanguageSettingClass2)(_settingInput);

        if(_lang == null)
        {
            return;
        }

        GameObject _obj = Instantiate(_countryEnumPanel.gameObject);

        RectTransform _rectT = _obj.gameObject.GetComponent<RectTransform>();

        string _st = DataPersistenceManager.GetInstance().GetGameData()._settingsValues[_settingInput.GetSettingID()];

        if(_rectT == null)
        {
            Destroy(_obj);

            return;
        }

        _obj.SetActive(true);

        _rectT.SetParent(_contentTransform);

        _rectT.anchoredPosition3D = Vector3.zero;

        _rectT.localRotation = Quaternion.identity;

        _rectT.localScale = Vector3.one;

        Text _indexText = _rectT.Find("Index Text").gameObject.GetComponent<Text>();

        Text _text = _rectT.Find("Text").gameObject.GetComponent<Text>();

        Image _flagImage = _rectT.Find("Language Flag").gameObject.GetComponent<Image>();

        if(_indexText == null || _text == null || _flagImage == null)
        {
            Destroy(_obj);

            return;
        }

        _indexText.text = _indexInput.ToString() + ".";

        _text.text = "Language";

        _lang.SetValueFromStringForm(_st);

        LanguageClass _currentLang = SettingsManager.GetInstance().GetMatchingLanguage(_lang.GetValue());

        _selectedLanguage = _currentLang.GetLanguage();

        _flagImage.sprite = _currentLang.GetFlagSprite();

        _panels.Add(_obj);

        _obj.name = _settingInput.GetSettingName() + " Panel";
    }

    public void SaveResults()
    {
        if(SettingsManager.GetInstance() == null || DataPersistenceManager.GetInstance() == null || _keysAndValues == null)
        {
            return;
        }

        SettingsManager.GetInstance().SaveResults(_keysAndValues);

        EndSession();
    }

    void EndSession()
    {
        foreach(GameObject _obj in _panels)
        {
            Destroy(_obj);
        }

        _keysAndValues.Clear();

        _panels.Clear();

        _rangedInputFields.Clear();

        _rangedValues.Clear();
    }

    void AdditionalRules()
    {
        //1. Settings about the indicator

        GameObject _panel1 = GetPanelByName("Indicator On");

        GameObject _panel2 = GetPanelByName("Indicator Threshold");

        if(_panel1 != null && _panel2 != null)
        {
            ToggleScript _tog = _panel1.GetComponent<RectTransform>().Find("Toggle").gameObject.GetComponent<ToggleScript>();

            CanvasGroup _cg = _panel2.gameObject.GetComponent<CanvasGroup>();

            if(_tog != null && _cg != null)
            {
                _tog.GetToggle().onValueChanged.AddListener(delegate
                {
                    _cg.interactable = _tog.GetToggle().isOn;

                    float _alpha = _tog.GetToggle().isOn ? 1.0f : 0.5f;

                    if (_panel2.GetComponent<RectTransform>().Find("Text") != null)
                    {
                        Text _t = _panel2.GetComponent<RectTransform>().Find("Text").gameObject.GetComponent<Text>();

                        Color _color = _t.color;

                        _color.a = _alpha;

                        _t.color = _color;
                    }

                    if(_panel2.GetComponent<RectTransform>().Find("Input Field") != null)
                    {
                        InputField _inputF = _panel2.GetComponent<RectTransform>().Find("Input Field").gameObject.GetComponent<InputField>();

                        Color _color2 = _inputF.textComponent.color;

                        _color2.a = _alpha;

                        _inputF.textComponent.color = _color2;
                    }

                    if(_panel2.GetComponent<RectTransform>().Find("Slider") != null)
                    {
                        Slider _slider = _panel2.GetComponent<RectTransform>().Find("Slider").gameObject.GetComponent<Slider>();

                        Color _color3 = _slider.fillRect.gameObject.GetComponent<Image>().color;

                        _color3.a = _alpha;

                        _slider.fillRect.gameObject.GetComponent<Image>().color = _color3;

                        _color3 = _slider.gameObject.GetComponent<RectTransform>().Find("Background").gameObject.GetComponent<Image>().color;

                        _color3.a = _alpha;

                        _slider.gameObject.GetComponent<RectTransform>().Find("Background").gameObject.GetComponent<Image>().color= _color3;
                    }

                    if(SettingsManager.GetInstance().GetSettingByName("Indicator Threshold") != null)
                    {
                        SettingsScript _st = SettingsManager.GetInstance().GetSettingByName("Indicator Threshold");

                        _st.SetSaveAfterConfirming(_tog.GetToggle().isOn);
                    }
                });

                _cg.interactable = _tog.GetToggle().isOn;

                float _alpha = _tog.GetToggle().isOn ? 1.0f : 0.5f;

                if (_panel2.GetComponent<RectTransform>().Find("Text") != null)
                {
                    Text _t = _panel2.GetComponent<RectTransform>().Find("Text").gameObject.GetComponent<Text>();

                    Color _color = _t.color;

                    _color.a = _alpha;

                    _t.color = _color;
                }

                if (_panel2.GetComponent<RectTransform>().Find("Input Field") != null)
                {
                    InputField _inputF = _panel2.GetComponent<RectTransform>().Find("Input Field").gameObject.GetComponent<InputField>();

                    Color _color2 = _inputF.textComponent.color;

                    _color2.a = _alpha;

                    _inputF.textComponent.color = _color2;
                }

                if (_panel2.GetComponent<RectTransform>().Find("Slider") != null)
                {
                    Slider _slider = _panel2.GetComponent<RectTransform>().Find("Slider").gameObject.GetComponent<Slider>();

                    Color _color3 = _slider.fillRect.gameObject.GetComponent<Image>().color;

                    _color3.a = _alpha;

                    _slider.fillRect.gameObject.GetComponent<Image>().color = _color3;

                    _color3 = _slider.gameObject.GetComponent<RectTransform>().Find("Background").gameObject.GetComponent<Image>().color;

                    _color3.a = _alpha;

                    _slider.gameObject.GetComponent<RectTransform>().Find("Background").gameObject.GetComponent<Image>().color = _color3;
                }
            }
        }

        //2. Settings about the languages
        _panel1 = GetPanelByName("Language");

        if(_panel1 != null)
        {
            Image _flagImage = _panel1.GetComponent<RectTransform>().Find("Language Flag").gameObject.GetComponent<Image>();

            if(_flagImage != null)
            {
                Button _flagButton = _flagImage.GetComponent<Button>();

                if(_flagButton != null)
                {
                    _flagButton.onClick.AddListener(delegate
                    {
                        int _index = (int)_selectedLanguage;

                        _index++;

                        if(_index >= Enum.GetNames(typeof(LanguageEnum)).Length)
                        {
                            _index = 0;
                        }

                        _selectedLanguage = (LanguageEnum)_index;

                        _flagImage.sprite = SettingsManager.GetInstance().GetLanguages()[_index].GetFlagSprite();

                        string _id = SettingsManager.GetInstance().GetSettingByName("Language").GetSettingID();

                        _keysAndValues[_id] = _index.ToString();
                    });
                }

                LanguageSettingClass2 _lC = (LanguageSettingClass2)SettingsManager.GetInstance().GetSettingByName("Language");

                if(_lC != null)
                {
                    _selectedLanguage = _lC.GetValue();

                    _flagImage.sprite = SettingsManager.GetInstance().GetMatchingLanguage(_selectedLanguage).GetFlagSprite();
                }
            }
        }
    }


    public GameObject GetPanelByName(string _nameInput, bool _capsInput = true)
    {
        for(int _i = 0; _i < _panels.Count; _i++)
        {
            GameObject _currentPanel = _panels[_i];

            if (_currentPanel == null) { continue; }

            if (_currentPanel.GetComponent<RectTransform>() == null) { continue; }

            if (_currentPanel.GetComponent<RectTransform>().Find("Text") == null) { continue; }

            if (_currentPanel.GetComponent<RectTransform>().Find("Text").gameObject.GetComponent<Text>() == null) { continue; }

            string _name = _currentPanel.GetComponent<RectTransform>().Find("Text").gameObject.GetComponent<Text>().text;

            if(string.Compare(_nameInput, _name, _capsInput) == 0)
            {
                return _currentPanel;
            }
        }

        return null;
    }
}
