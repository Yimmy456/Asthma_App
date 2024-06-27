using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GamePropertiesClass<T>
{
    [SerializeField]
    MeterClass _meter;

    [SerializeField]
    MainCanvasesClass _mainCanvases;

    [SerializeField]
    Canvas _gameCanvas;

    [SerializeField]
    Canvas _gameIndicatorCanvas;

    [SerializeField]
    Canvas _gameMenuCanvas;

    [SerializeField]
    YesOrNoCanvasScript _yesOrNoCanvas;

    [SerializeField]
    Camera _camera;

    bool _updateUIBoolean = false;

    [SerializeField]
    Text _countText;

    [SerializeField]
    Text _percentageText;

    bool _gameInSession = false;

    [SerializeField]
    List<T> _listOfObjects;

    [SerializeField]
    List<GameObject> _listOfObjectsAsGO;

    [SerializeField]
    Text _responseText;

    [SerializeField]
    Canvas _newBadgeCanvas;

    [SerializeField]
    InformationCanvasScript _informationCanvas;

    [SerializeField]
    BadgeScript _badge;

    public MeterClass GetMeter() { return _meter; }

    public MainCanvasesClass GetMainCanvases() { return _mainCanvases; }

    public Canvas GetGameCanvas() { return _gameCanvas; }

    public Canvas GetGameIndicatorCanvas() { return _gameIndicatorCanvas; }

    public YesOrNoCanvasScript GetYesOrNoCanvas() { return _yesOrNoCanvas; }

    public Camera GetCamera() { return _camera; }

    public Text GetCountText() { return _countText; }

    public Text GetPercentageText() { return _percentageText; }

    public bool GetGameInSession() { return _gameInSession; }

    public List<T> GetListOfObjects() { return _listOfObjects; }

    public Text GetResponseText() { return _responseText; }

    public InformationCanvasScript GetInformationCanvas() { return _informationCanvas; }

    public BadgeScript GetBadge() { return _badge; }

    public UIIndicatorCanvasScript GetGameIndicatorProperties()
    {
        if(_gameIndicatorCanvas == null)
        {
            return null;
        }

        return _gameIndicatorCanvas.gameObject.GetComponent<UIIndicatorCanvasScript>();
    }

    public void SetGameInSession(bool _input)
    {
        _gameInSession = _input;
    }

    public void SetListOfObjects(List<T> _input)
    {
        _listOfObjects = _input;
    }

    public void SetListOfObjectsAsGO(List<GameObject> _input)
    {
        _listOfObjectsAsGO = _input;
    }

    public void SignalToUpdateUI()
    {
        _updateUIBoolean = true;
    }

    public void UpdateUI()
    {
        if(!_updateUIBoolean)
        {
            return;
        }
        else
        {
            _updateUIBoolean = false;
        }

        if(_countText == null)
        {
            return;
        }

        _countText.text = _meter.GetValue().ToString() + "/" + _meter.GetMaxValue().ToString();

        if(_percentageText == null)
        {
            return;
        }

        _percentageText.text = _meter.GetPercentage().ToString("0.00") + "%";

        _meter.SetTextColor(_percentageText);
    }

    public void ClearGame()
    {
        _meter.SetMaxValue(1);

        _meter.SetValue(0);

        _gameInSession = false;

        foreach(GameObject _go in _listOfObjectsAsGO)
        {
            Object.Destroy(_go);
        }

        _listOfObjects.Clear();

        _listOfObjectsAsGO.Clear();

        SignalToUpdateUI();

        UpdateUI();

        if(_gameCanvas != null)
        {
            _gameCanvas.gameObject.SetActive(false);
        }

        if(_yesOrNoCanvas != null)
        {
            _yesOrNoCanvas.GetNoButton().onClick.RemoveAllListeners();

            _yesOrNoCanvas.GetYesButton().onClick.RemoveAllListeners();

            _yesOrNoCanvas.gameObject.SetActive(false);
        }
    }

    public void ReturnToPlayerCanvas()
    {
        _mainCanvases.SetCanvasesOn(true);
    }

    public void StartGame(int _valueInput, int _maxValueInput)
    {
        _gameInSession = true;

        _mainCanvases.SetCanvasesOn(false);

        if (_gameMenuCanvas != null)
        {
            _gameMenuCanvas.gameObject.SetActive(false);
        }

        if (_gameCanvas != null)
        {
            _gameCanvas.gameObject.SetActive(true);
        }

        _meter.SetMaxValue(_maxValueInput);

        _meter.SetValue(_valueInput);

        UpdateUI();
    }

    public void StartGame()
    {
        StartGame(0, 1);
    }

    public void ActionsOfNoButton()
    {
        if (_yesOrNoCanvas == null)
        {
            return;
        }

        if (_yesOrNoCanvas.GetNoButton() == null)
        {
            return;
        }

        if (_mainCanvases.GetMainPlayerCanvas() != null)
        {
            _yesOrNoCanvas.GetNoButton().onClick.AddListener(delegate { _mainCanvases.GetMainPlayerCanvas().gameObject.SetActive(true); });
        }

        _yesOrNoCanvas.GetNoButton().onClick.AddListener(delegate { _yesOrNoCanvas.gameObject.SetActive(false); });

        _yesOrNoCanvas.GetNoButton().onClick.AddListener(delegate { _yesOrNoCanvas.GetNoButton().onClick.RemoveAllListeners(); });
    }

    public void ActionsOfYesButton()
    {
        ClearGame();
    }

    public void AddObjectToList(T _input)
    {
        _listOfObjects.Add(_input);
    }

    public void SetResponseText(string _textInput, Color _textColorInput, Color _textOutlineColorInput, Vector2 _textOutlineDistanceInput)
    {
        if(_responseText == null)
        {
            return;
        }

        _responseText.text = _textInput;

        _responseText.color = _textColorInput;

        if(_responseText.gameObject.GetComponent<Outline>() == null)
        {
            return;
        }

        Outline _outline = _responseText.gameObject.GetComponent<Outline>();

        _outline.effectColor = _textOutlineColorInput;

        _outline.effectDistance = _textOutlineDistanceInput;
    }

    public void ClearResponseText()
    {
        if(_responseText == null)
        {
            return;
        }

        _responseText.text = "";
    }

    public Canvas GetNewBadgeCanvas()
    {
        return _newBadgeCanvas;
    }

    public List<GameObject> GetListOfObjectsAsGO()
    {
        return _listOfObjectsAsGO;
    }

    public void AddObjectsAsGO(GameObject _input)
    {
        _listOfObjectsAsGO.Add(_input);
    }

    public void CallInformationCanvas(string _textInput, Color _textColorInput, Color _outlineColorInput, Vector2 _outlineDistanceInput)
    {
        if(_informationCanvas == null)
        {
            return;
        }

        _informationCanvas.gameObject.SetActive(true);

        _informationCanvas.SetText(_textInput, _textColorInput, _outlineColorInput, _outlineDistanceInput);
    }

    public void SetBadge(BadgeScript _input)
    {
        _badge = _input;
    }

    public void SetBadge(string _textInput, bool _caseInsensitive = false)
    {
        if(BadgesManagerScript.GetInstance() == null)
        {
            return;
        }

        BadgeScript _b = BadgesManagerScript.GetInstance().GetBadgeByName(_textInput, _caseInsensitive);

        _badge = _b;
    }

    public void SetBadge(int _input)
    {
        if(BadgesManagerScript.GetInstance() == null)
        {
            return;
        }

        BadgeScript _b = BadgesManagerScript.GetInstance().GetBadges()[_input];

        _badge = _b;
    }

    public void ClearObjectLists()
    {
        foreach(GameObject _obj in _listOfObjectsAsGO)
        {
            GameObject.Destroy(_obj);
        }

        _listOfObjectsAsGO.Clear();

        _listOfObjects.Clear();
    }

    public void SetShowBadge(BadgeScript _badgeInput, bool _collectedStatusInput)
    {
        if(_newBadgeCanvas == null)
        {
            return;
        }

        RectTransform _rt = _newBadgeCanvas.gameObject.GetComponent<RectTransform>();

        Image _im = _rt.Find("Badge Image").GetComponent<Image>();

        Text _tx = _rt.Find("Congratulations Text").GetComponent<Text>();

        if(_im == null || _tx == null)
        {
            return;
        }

        _im.sprite = _badgeInput.GetBadgeSprite();

        string _displayText = "Well done! You have already earned the \\(" + _badgeInput.GetBadgeName() + ")\\!";

        if(!_collectedStatusInput)
        {
            _displayText = "Congratulations! You won a new badge! it is the \\(" + _badgeInput.GetBadgeName() + ")\\ badge!"; 
        }
    }
}
