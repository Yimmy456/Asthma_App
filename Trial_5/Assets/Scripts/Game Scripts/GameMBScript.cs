using System.Collections;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMBScript : MonoBehaviour, ExperienceInterface, YesOrNoInterface, RewardingBadgeInterface, GameCorrectOrWrongInterface
{ 
    [Header("1. Variables of all games")]
    [SerializeField]
    protected string _gameName;

    [SerializeField]
    protected Transform _mainContainer;

    [SerializeField]
    protected Transform _sceneContainer;

    [SerializeField]
    protected GameObject _gameSpace;

    [SerializeField]
    protected AudioClip _correctAudioClip;

    [SerializeField]
    protected AudioClip _incorrectAudioClip;

    [SerializeField]
    protected DialoguesScript _dialogues;

    [SerializeField]
    protected MeterClass _completionMeter;

    [SerializeField]
    protected YesOrNoCanvasScript _yesOrNoCanvas;

    [SerializeField]
    protected Color _gameColor;

    [SerializeField]
    protected BadgeScript _badge;

    [SerializeField]
    protected Canvas _newBadgeCanvas;

    [SerializeField]
    protected GameCanvasScript _gameCanvas;

    [SerializeField]
    protected Camera _camera;

    [SerializeField]
    protected MainCanvasesClass _mainCanvases;

    [SerializeField]
    protected Canvas _gameMenuCanvas;

    [SerializeField]
    protected UIIndicatorCanvasScript _gameIndicatorCanvas;

    [SerializeField]
    protected Text _responseText;

    [SerializeField]
    protected InformationCanvasScript _informationCanvas;

    [SerializeField]
    protected string _gameIntroDialogueName;

    [SerializeField]
    protected DialoguesScript _fastyDialogues;

    protected bool _currentGameInSession;

    protected static GameMBScript _currentGame = null;

    protected bool _gameDone = false;

    protected bool _progressUpdated = false;

    protected bool _progressUpdatedSwitchOn = false;

    protected Coroutine _progressUpdateCoroutine;

    protected bool _waitToCompleteSignal = false;

    protected bool _introductionDialogueComplete = false;

    Coroutine _responseTextCoroutine;

    public string GetGameName()
    {
        return _gameName;
    }

    public bool GetCurrentGameInSession()
    {
        return _currentGameInSession;
    }

    public static GameMBScript GetCurrentGame()
    {
        return _currentGame;
    }

    protected static bool _gamePaused = false;

    public static bool GetGameInSession()
    {
        return (_currentGame != null);
    }

    public bool GetProgressUpdated()
    {
        return _progressUpdated;
    }

    public MeterClass GetCompletionMeter()
    {
        return _completionMeter;
    }

    public AudioClip GetCorrectAudioClip()
    {
        return _correctAudioClip;
    }

    public AudioClip GetIncorrectAudioClip()
    {
        return _incorrectAudioClip;
    }

    public DialoguesScript GetDialogues()
    {
        return _dialogues;
    }

    public InformationCanvasScript GetInformationCanvas()
    {
        return _informationCanvas;
    }

    /*public virtual void StartGameFunction()
    {
        if(GetGameInSession())
        {
            return;
        }

        _currentGameInSession = true;
    }

    public virtual void StartGameFunction(int _input)
    {
        StartGameFunction();
    }

    public virtual void ClearGameFunction()
    {
        _currentGame = null;

        _currentGameInSession = false;
    }

    public virtual void FinishGame()
    {
        _gameDone = true;

        ClearGameFunction();
    }

    protected virtual bool CanRewardBadge()
    { 
        if(BadgesManagerScript.GetInstance() == null)
        {
            return false;
        }

        return true;
    }*/

    public bool GetGameDone()
    {
        return _gameDone;
    }

    public bool GetWaitToCompleteSignal()
    {
        return _waitToCompleteSignal;
    }

    protected virtual string SetInfoText()
    {
        return "";
    }

    public BadgeScript GetBadge()
    {
        return _badge;
    }

    public GameCanvasScript GetGameCanvas()
    {
        return _gameCanvas;
    }

    public Text GetResponseText()
    {
        return _responseText;
    }

    public string GetGameIntroDialogueName()
    {
        return _gameIntroDialogueName;
    }

    public Coroutine GetResponseTextCoroutine()
    {
        return _responseTextCoroutine;
    }

    protected virtual void SetBadge()
    {

    }

    public void SetBadge(BadgeScript _input)
    {
        _badge = _input;
    }

    public void SetBadgeByName(string _badgeNameInput, bool _ignoreCapsBooleanInput = true)
    {
        if(BadgesManagerScript.GetInstance() == null)
        {
            return;
        }        

        BadgeScript _b = BadgesManagerScript.GetInstance().GetBadgeByName(_badgeNameInput, _ignoreCapsBooleanInput);

        if(_b != null)
        {
            _badge = _b;
        }
    }

    protected void SetBadge(string _textInput, bool _caseInsensitiveInput = false)
    {
        if (BadgesManagerScript.GetInstance() == null)
        {
            return;
        }

        BadgeScript _b = BadgesManagerScript.GetInstance().GetBadgeByName(_textInput, _caseInsensitiveInput);

        _badge = _b;
    }

    public static bool GamePaused()
    {
        return _gamePaused;
    }

    protected IEnumerator ProgressUpdateCoroutineFunction()
    {
        _progressUpdated = true;

        yield return null;

        _progressUpdated = false;

        _progressUpdatedSwitchOn = false;

        _progressUpdateCoroutine = null;
    }

    public void SetProgressUpdated(bool _input)
    {
        _progressUpdated = _input;
    }

    public virtual void TryToStartGame()
    {

    }

    //Functions from "ExperienceInterface"

    public virtual void IUpdateExperience()
    {

    }

    public virtual void ICompleteExperience()
    {
        if (!_gameDone)
        {
            IStopExperience();

            _gameDone = true;
        }

        _mainCanvases.SetCanvasesOn(false);

        _mainCanvases.GetDoctorCanvas().gameObject.SetActive(true);

        _newBadgeCanvas.gameObject.SetActive(true);

        Text _newBadgeText = _newBadgeCanvas.gameObject.GetComponent<RectTransform>().Find("Congratulations Text").gameObject.GetComponent<Text>();

        if (_newBadgeText != null)
        {
            _newBadgeText.text = GetBadgeStatement();
        }

        Image _badgeImage = _newBadgeCanvas.gameObject.GetComponent<RectTransform>().Find("Badge Image").gameObject.GetComponent<Image>();

        if (_badgeImage != null)
        {
            _badgeImage.sprite = _badge.GetBadgeSprite();
        }

        if (_dialogues != null)
        {
            if (!_badge.GetBadgeCollected())
            {
                _dialogues.PlayClip("New Badge Earned");
            }
            else
            {
                _dialogues.PlayClip("Badge Already Earned");
            }
        }

        BadgesManagerScript.GetInstance().SetBadgeEarned(_badge);

        Button _b = _newBadgeCanvas.gameObject.GetComponent<RectTransform>().Find("Next Button").gameObject.GetComponent<Button>();

        Button _backB = _newBadgeCanvas.gameObject.GetComponent<RectTransform>().Find("Back Button").gameObject.GetComponent<Button>();

        ClearResponseText();

        if (_gameIndicatorCanvas != null)
        {
            if (_gameIndicatorCanvas.gameObject.activeSelf)
            {
                _gameIndicatorCanvas.gameObject.SetActive(false);
            }
        }

        if (_b != null)
        {
            _b.onClick.AddListener(delegate
            {
                _gameDone = false;

                if (_currentGame == this)
                {
                    _currentGame = null;
                }

                if (_currentGameInSession)
                {
                    _currentGameInSession = false;
                }

                _completionMeter.SetValue(0);

                _completionMeter.SetMaxValue(1);

                _mainCanvases.SetCanvasesOn(true);

                _newBadgeCanvas.gameObject.SetActive(false);

                _b.onClick.RemoveAllListeners();
            });
        }

        if (_backB != null)
        {
            _backB.gameObject.SetActive(false);
        }

        if (_gameIndicatorCanvas != null && _camera != null)
        {
            _gameIndicatorCanvas.SetIndicatorColor(_gameColor);

            _gameIndicatorCanvas.SetTargetObject(_gameSpace);
        }

        if (_gameIndicatorCanvas != null)
        {
            _gameIndicatorCanvas.gameObject.SetActive(false);
        }


        _gameCanvas.gameObject.SetActive(false);
    }

    public virtual void IStartExperience()
    {
        if (_currentGame != null)
        {
            return;
        }

        _currentGame = this;

        _currentGameInSession = true;

        _gameDone = false;

        _gamePaused = false;

        _waitToCompleteSignal = false;

        _mainCanvases.SetCanvasesOn(false);

        _mainCanvases.GetDoctorCanvas().gameObject.SetActive(true);

        _gameMenuCanvas.gameObject.SetActive(false);

        _gameCanvas.gameObject.SetActive(true);

        _mainCanvases.SetDoctorImageCanvasPropertiesAccordingly();

        if(_gameIndicatorCanvas != null && _camera != null)
        {
            bool _choice = PrepareGameIndicator();

            _gameIndicatorCanvas.gameObject.SetActive(_choice);

            if(_choice)
            {
                _gameIndicatorCanvas.SetIndicatorColor(_gameColor);
            }
        }

        if(_gameIntroDialogueName != "" && _dialogues != null)
        {
            _dialogues.PlayClip(_gameIntroDialogueName);
        }

        GetAndSetRestartAndQuitButtons();
    }

    public virtual void IStartExperience(int _input) { if (_currentGame != null) { return; } IStartExperience(); }

    public virtual void IStartExperience(string _input) { if (_currentGame != null) { return; } IStartExperience(); }

    public virtual void IChooseToRestartExperience()
    {
        if (_yesOrNoCanvas == null)
        {
            return;
        }

        Button _noButton = _yesOrNoCanvas.GetNoButton();

        Button _yesButton = _yesOrNoCanvas.GetYesButton();

        _gamePaused = true;

        if (_dialogues != null)
        {
            _dialogues.StopCurrentDialogue();
        }

        if (_gameIndicatorCanvas != null)
        {
            _gameIndicatorCanvas.gameObject.SetActive(false);
        }

        _mainCanvases.SetCanvasesOn(false);

        _gameCanvas.gameObject.SetActive(false);

        _gameIndicatorCanvas.gameObject.SetActive(false);

        _mainCanvases.GetDoctorCanvas().gameObject.SetActive(false);

        _yesOrNoCanvas.gameObject.SetActive(true);

        _yesOrNoCanvas.GetText().text = "Are you sure you want to restart the game?";

        if (_noButton != null)
        {
            _noButton.onClick.AddListener(delegate { ISetActionsOfNoButton(); });
        }

        if (_yesButton != null)
        {
            _yesButton.onClick.AddListener(delegate { ISetActionsOfYesButtonToRestart(); });
        }
    }

    public virtual void RestartGameWithAnotherInstance()
    {

    }

    public virtual void RestartGameWithAnotherInstance(int _input) { }

    public virtual void RestartGameWithAnotherInstance(string _input) { }

    public virtual void IStopExperience()
    {
        if (_currentGame == this)
        {
            _currentGame = null;
        }

        if (_currentGameInSession)
        {
            _currentGameInSession = false;
        }

        if (_gamePaused)
        {
            _gamePaused = false;
        }

        if (_gameCanvas != null)
        {
            _gameCanvas.gameObject.SetActive(false);
        }

        _mainCanvases.SetCanvasesOn(true);

        _completionMeter.SetValue(0);

        _completionMeter.SetMaxValue(1);

        _completionMeter.SignalToUpdateUI();

        _completionMeter.UpdateUI();

        _waitToCompleteSignal = false;

        _introductionDialogueComplete = false;

        //SetResponseTextProperties();

        ClearResponseText();

        _mainCanvases.SetDoctorImageCanvasPropertiesBackToDefault();

        if (_gameCanvas.GetQuitButton() != null)
        {
            _gameCanvas.GetQuitButton().onClick.RemoveAllListeners();
        }

        if (_mainCanvases.GetDoctorCanvas() != null)
        {
            _mainCanvases.GetDoctorCanvas().gameObject.SetActive(true);
        }

        if (_yesOrNoCanvas.gameObject.activeSelf)
        {
            _yesOrNoCanvas.gameObject.SetActive(false);
        }

        if (_gameCanvas.gameObject.activeSelf)
        {
            _gameCanvas.gameObject.SetActive(false);
        }

        if (_gameIndicatorCanvas != null)
        {
            if (_gameIndicatorCanvas.gameObject.activeSelf)
            {
                _gameIndicatorCanvas.gameObject.SetActive(false);
            }
        }

        _mainCanvases.SetCanvasesOn(true);

        if (_yesOrNoCanvas != null)
        {
            if (_yesOrNoCanvas.GetNoButton() != null)
            {
                _yesOrNoCanvas.GetNoButton().onClick.RemoveAllListeners();
            }

            if (_yesOrNoCanvas.GetYesButton() != null)
            {
                _yesOrNoCanvas.GetYesButton().onClick.RemoveAllListeners();
            }

            _yesOrNoCanvas.gameObject.SetActive(false);
        }
    }

    public virtual void IChooseToQuitExperience()
    {
        if(_yesOrNoCanvas == null)
        {
            return;
        }

        Button _noButton = _yesOrNoCanvas.GetNoButton();

        Button _yesButton = _yesOrNoCanvas.GetYesButton();

        _gamePaused = true;

        if(_dialogues != null)
        {
            _dialogues.StopCurrentDialogue();
        }

        if(_gameIndicatorCanvas != null)
        {
            _gameIndicatorCanvas.gameObject.SetActive(false);
        }

        _mainCanvases.SetCanvasesOn(false);

        if (_gameCanvas != null)
        {
            _gameCanvas.gameObject.SetActive(false);
        }

        if (_gameIndicatorCanvas != null)
        {
            _gameIndicatorCanvas.gameObject.SetActive(false);
        }

        _mainCanvases.GetDoctorCanvas().gameObject.SetActive(false);

        _yesOrNoCanvas.gameObject.SetActive(true);

        _yesOrNoCanvas.GetText().text = "Are you sure you want to quit the game?";

        if(_noButton != null)
        {
            _noButton.onClick.AddListener(delegate { ISetActionsOfNoButton(); });
        }

        if (_yesButton != null)
        {
            _yesButton.onClick.AddListener(delegate { ISetActionsOfYesButtonToQuit(); });
        }
    }

    public virtual void IResumeExperience()
    {
        _gameCanvas.gameObject.SetActive(true);

        _yesOrNoCanvas.gameObject.SetActive(false);

        _mainCanvases.GetDoctorCanvas().gameObject.SetActive(true);

        if (_gameIndicatorCanvas != null && _camera != null)
        {
            _gameIndicatorCanvas.gameObject.SetActive(PrepareGameIndicator());
        }

        _gamePaused = false;

        _yesOrNoCanvas.GetYesButton().onClick.RemoveAllListeners();

        _yesOrNoCanvas.GetNoButton().onClick.RemoveAllListeners();
    }

    public virtual IEnumerator IWaitUntilCompletion()
    {
        yield return null;
    }

    //Functions from "YesOrNoInterface"
    public virtual void ISetActionsOfNoButton()
    {
        IResumeExperience();
    }

    public virtual void ISetActionsOfYesButtonToQuit()
    {
        IStopExperience();
    }

    public virtual void ISetActionsOfYesButtonToRestart()
    {

    }

    public virtual void IRewardBadge()
    {

    }

    public void SetBadge(int _input)
    {
        if (BadgesManagerScript.GetInstance() == null)
        {
            return;
        }

        BadgeScript _b = BadgesManagerScript.GetInstance().GetBadges()[_input];

        _badge = _b;
    }

    public MainCanvasesClass GetMainCanvases()
    {
        return _mainCanvases;
    }

    public void SetResponseText(string _textInput, Color _textColorInput, Vector2 _textOutlineDistanceInput, Color _textOutlineColorInput)
    {
        if (_responseText == null)
        {
            return;
        }

        if (SettingsManager.GetInstance() != null)
        {
            BooleanSettingsClass _b = SettingsManager.GetInstance().GetBooleanSettingByName("Text Dialogue");

            if (_b != null)
            {
                if (_b.GetBooleanValue() == false)
                {
                    _responseText.text = "";

                    return;
                }
            }
        }

        _responseText.text = _textInput;

        _responseText.color = _textColorInput;

        if (_responseText.gameObject.GetComponent<Outline>() == null)
        {
            return;
        }

        Outline _outline = _responseText.gameObject.GetComponent<Outline>();

        _outline.effectColor = _textOutlineColorInput;

        _outline.effectDistance = _textOutlineDistanceInput;
    }

    public void SetResponseText(string _textInput, Color _textColorInput, Vector2 _textOutlineDistanceInput, float _outlineColorValueInput = 0.5f, float _outlineAlphaInput = 1.0f)
    {
        if (_responseText == null)
        {
            return;
        }

        if (SettingsManager.GetInstance() != null)
        {
            BooleanSettingsClass _b = SettingsManager.GetInstance().GetBooleanSettingByName("Text Dialogue");

            if (_b != null)
            {
                if (_b.GetBooleanValue() == false)
                {
                    _responseText.text = "";

                    return;
                }
            }
        }

        _responseText.text = _textInput;

        _responseText.color = _textColorInput;

        if (_responseText.gameObject.GetComponent<Outline>() == null)
        {
            return;
        }

        Outline _outline = _responseText.gameObject.GetComponent<Outline>();

        _outline.effectColor = ToolsStruct.ChangeColorValue(_textColorInput, _outlineColorValueInput, _outlineAlphaInput, true);

        _outline.effectDistance = _textOutlineDistanceInput;
    }


    public void ClearResponseText()
    {
        if (_responseText == null)
        {
            return;
        }

        _responseText.text = "";
    }

    public void SetShowBadge(BadgeScript _badgeInput, bool _collectedStatusInput)
    {
        if (_newBadgeCanvas == null)
        {
            return;
        }

        RectTransform _rt = _newBadgeCanvas.gameObject.GetComponent<RectTransform>();

        Image _im = _rt.Find("Badge Image").GetComponent<Image>();

        Text _tx = _rt.Find("Congratulations Text").GetComponent<Text>();

        if (_im == null || _tx == null)
        {
            return;
        }

        _im.sprite = _badgeInput.GetBadgeSprite();

        string _displayText = "Well done! You have already earned the \\(" + _badgeInput.GetBadgeName() + ")\\!";

        if (!_collectedStatusInput)
        {
            _displayText = "Congratulations! You won a new badge! it is the \\(" + _badgeInput.GetBadgeName() + ")\\ badge!";
        }
    }

    public void CallInformationCanvas(string _textInput, Color _textColorInput, Color _outlineColorInput, Vector2 _outlineDistanceInput)
    {
        if (_informationCanvas == null)
        {
            return;
        }

        _informationCanvas.gameObject.SetActive(true);

        _informationCanvas.SetText(_textInput, _textColorInput, _outlineColorInput, _outlineDistanceInput);
    }

    protected string GetBadgeStatement()
    {
        BadgeScript _selectedBadge = _badge;

        if (_selectedBadge == null)
        {
            return "";
        }

        string _badgeStatement;

        if (!_selectedBadge.GetBadgeCollected())
        {
            _badgeStatement = "Congratuations! You have won a new badge! it is the " + @"""" + _selectedBadge.GetBadgeName() + @"""" + " badge!";
        }
        else
        {
            _badgeStatement = "Well done! Although, you have already earned the " + @"""" + _selectedBadge.GetBadgeName() + @"""" + " badge.";
        }

        return _badgeStatement;
    }

    public DoctorTalkingScript GetDoctorTalkingProperties()
    {
        if (_mainCanvases.GetDoctorCanvas() == null)
        {
            return null;
        }

        DialogueCanvasScript _dialogueS = _mainCanvases.GetDoctorCanvas().gameObject.GetComponent<DialogueCanvasScript>();

        if (_dialogueS == null)
        {
            return null;
        }

        return _dialogueS.GetDoctorTalkingProperties();
    }

    protected virtual void ShuffleFunction()
    {

    }

    protected void SetResponseTextProperties(float _positionYInput = -150.0f, float _heightInput = 250.0f)
    {
        if (GetResponseText() == null)
        {
            //Debug.LogError("There is no response text to modify.");

            return;
        }

        RectTransform _rt = GetResponseText().gameObject.GetComponent<RectTransform>();

        Vector2 _ancP = _rt.anchoredPosition;

        _ancP.y = _positionYInput;

        _rt.anchoredPosition = _ancP;

        Vector2 _dS = _rt.sizeDelta;

        _dS.y = _heightInput;

        _rt.sizeDelta = _dS;
    }

    bool PrepareGameIndicator()
    {
        if (SettingsManager.GetInstance() == null)
        {
            return true;
        }

        BooleanSettingsClass _boolS = SettingsManager.GetInstance().GetBooleanSettingByName("Indicator On");

        if (_boolS == null)
        {
            return true;
        }

        return _boolS.GetBooleanValue();
    }

    void GetAndSetRestartAndQuitButtons()
    {
        if(_gameCanvas == null)
        {
            return;
        }

        if(_gameCanvas.GetQuitButton() != null)
        {
            _gameCanvas.GetQuitButton().onClick.AddListener(delegate { IChooseToQuitExperience(); });
        }
    }


    public virtual void IGameCorrect()
    {
        if (_fastyDialogues != null)
        {
            _fastyDialogues.PlayRandomDialogue(new List<string>() { "Yes!", "Hooray" });
        }
    }

    public virtual void IGameIncorrect()
    {
        if(_fastyDialogues != null)
        {
            _fastyDialogues.PlayRandomDialogue(new List<string>() { "Hmm Uh uh", "Nope" });
        }
    }

    public virtual void IGameCorrect(int _indexInput)
    {
        IGameCorrect();
    }

    public virtual void IGameIncorrect(int _indexInput)
    {
        IGameIncorrect();
    }

    public virtual void IGameCorrect(string _dialogueNameInput)
    {
        IGameCorrect();
    }

    public virtual void IGameIncorrect(string _dialogueNameInput)
    {
        IGameIncorrect();
    }

    public virtual void IGameCorrect(string _dialogueNameInput, int _indexInput)
    {
        IGameCorrect();
    }

    public virtual void IGameIncorrect(string _dialogueNameInput, int _indexInput)
    {
        IGameIncorrect();
    }

    IEnumerator SetResponseTextCoroutine(string _textInput, Color _textColorInput, float _timeInput, int _fontSizeInput)
    {
        _responseText.text = _textInput;

        Outline _outline = _responseText.gameObject.GetComponent<Outline>();

        _responseText.fontSize = _fontSizeInput;

        if(_outline != null)
        {
            Color _outlineColor = ToolsStruct.ChangeColorValue(_textColorInput, 0.5f, 0.5f, true);

            _outline.effectColor = _outlineColor;
        }

        yield return new WaitForSeconds(_timeInput);

        _responseText.text = "";
    }

    protected void SetResponseText(string _textInput, Color _textColorInput, float _timeInput = 5.0f, int _fontSizeInput = 40)
    {
        if(_responseText == null)
        {
            return;
        }

        if(_responseTextCoroutine != null)
        {
            StopCoroutine(_responseTextCoroutine);
        }

        _responseTextCoroutine = StartCoroutine(SetResponseTextCoroutine(_textInput, _textColorInput, _timeInput, _fontSizeInput));
    }

    protected void StopResponseTextCoroutine()
    {
        if (_responseText == null)
        {
            return;
        }

        if (_responseTextCoroutine != null)
        {
            StopCoroutine(_responseTextCoroutine);

            _responseTextCoroutine = null;
        }

        _responseText.text = "";
    }
}
