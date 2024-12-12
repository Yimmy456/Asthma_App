using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateMachineScript : MonoBehaviour, YesOrNoInterface
{
    [SerializeField]
    int _stageNumber = -1;

    [SerializeField]
    MCQGameScript _mcqGame;

    [SerializeField]
    CardGameScript _cardGame;

    [SerializeField]
    LetterGameScript _letterGame;

    [SerializeField]
    bool _machineOn;

    [SerializeField]
    bool _goToNextState;

    [SerializeField]
    MainCanvasesClass _mainCanvases;

    [SerializeField]
    YesOrNoCanvasScript _yesOrNoCanvas;

    [SerializeField]
    ProcedureStateMachineCanvasScript _procedureCanvas;

    [SerializeField]
    MeterClass _procedureCompletionMeter;

    [SerializeField]
    int _meterMaxValue = 20;

    [SerializeField]
    InformationCanvasScript _informationCanvas;

    [SerializeField]
    Canvas _newBadgeCanvas;

    [SerializeField]
    List<string> _badgeNames;

    Coroutine _lectureCoroutine;

    BaseState _currentState;

    StateFactoryClass _states;

    bool _updateMeterTextBoolean = false;

    //GameMBScript _currentGame = null;

    private void Awake()
    {
        _states = new StateFactoryClass(this);

        _currentState = _states.GetIntroductionState();

        //_currentState.EnterState();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(_states == null)
        {
            _states = new StateFactoryClass(this);
        }

        _currentState = _states.GetIntroductionState();

        _procedureCompletionMeter.SetMaxValue(_meterMaxValue);

        //_currentState.EnterState();

        //_machineOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_machineOn)
        {
            _currentState.UpdateStates();

            UpdateMeter();
        }
    }

    public int GetStageNumber()
    {
        return _stageNumber;
    }

    public void StartMachine()
    {
        _stageNumber = 0;

        if(_states == null)
        {
            _states = new StateFactoryClass(this);
        }

        _currentState = _states.GetIntroductionState();

        _currentState.EnterState();

        _procedureCanvas.gameObject.SetActive(true);

        _mainCanvases.SetCanvasesOn(false);

        _procedureCompletionMeter.SetValue(0);

        _updateMeterTextBoolean = true;

        _machineOn = true;
    }

    public void EndMachine()
    {
        _stageNumber = -1;

        _machineOn = false;

        _goToNextState = false;

        if (_currentState != null)
        {
            _currentState.ExitStates();

            _currentState = null;
        }

        _mainCanvases.SetCanvasesOn(true);

        _procedureCanvas.gameObject.SetActive(false);

        //_cardGame.GetGameProperties().GetMeter().SetValue(0);

        _cardGame.GetCompletionMeter().SetValue(0);

        _cardGame.GetCompletionMeter().SignalToUpdateUI();

        _letterGame.GetCompletionMeter().SetValue(0);

        _letterGame.GetCompletionMeter().SignalToUpdateUI();

        _mcqGame.GetCompletionMeter().SetValue(0);

        _mcqGame.GetCompletionMeter().SignalToUpdateUI();
    }

    public BaseState GetCurrentState()
    {
        return _currentState;
    }

    public bool GetGoToNextState()
    {
        return _goToNextState;
    }

    public bool GetMachineOn()
    {
        return _machineOn;
    }

    public MeterClass GetProcedureCompletionMeter()
    {
        return _procedureCompletionMeter;
    }

    public Coroutine GetLectureCoroutine()
    {
        return _lectureCoroutine;
    }

    public ProcedureStateMachineCanvasScript GetProcedureCanvas()
    {
        return _procedureCanvas;
    }

    public LetterGameScript GetLetterGame()
    {
        return _letterGame;
    }

    public CardGameScript GetCardGame()
    {
        return _cardGame;
    }

    public MCQGameScript GetMCQGame()
    {
        return _mcqGame;
    }

    public InformationCanvasScript GetInformationCanvas()
    {
        return _informationCanvas;
    }

    public Canvas GetNewBadgeCanvas()
    {
        return _newBadgeCanvas;
    }

    public List<string> GetBadgeNames()
    {
        return _badgeNames;
    }

    public void SetCurrentState(BaseState _input)
    {
        _currentState = _input;
    }

    public void SetGoToNextState(bool _input)
    {
        _goToNextState = _input;
    }

    public void StartLecture(LectureState _input)
    {
        if(_lectureCoroutine != null)
        {
            return;
        }

        //_lectureCoroutine.

        float _durationTime = _input.GetSeconds();

        _lectureCoroutine = StartCoroutine(StartLectureCoroutine(_durationTime));
    }

    IEnumerator StartLectureCoroutine(float _secondsInput)
    {
        _procedureCanvas.GetRestartButton().gameObject.SetActive(false);

        Debug.Log("The lecture coroutine begins.");

        yield return new WaitForSeconds(_secondsInput);

        Debug.Log("The lecture coroutine ends.");

        _procedureCanvas.GetRestartButton().gameObject.SetActive(true);

        _procedureCanvas.GetNextButton().gameObject.SetActive(true);

        _lectureCoroutine = null;
    }

    public void EndLectureImmediately()
    {
        if(_lectureCoroutine != null)
        {
            StopCoroutine(_lectureCoroutine);

            _lectureCoroutine = null;
        }
    }

    void UpdateMeter()
    {
        if(!_updateMeterTextBoolean)
        {
            return;
        }
        else
        {
            _updateMeterTextBoolean = false;
        }

        Text _tx = _procedureCanvas.GetPercentageText();

        if(_tx == null)
        {
            //_updateMeterTextBoolean = false;

            return;
        }

        float _percentage = _procedureCompletionMeter.GetPercentage();

        _tx.text = _percentage.ToString("0.00") + "%";

        _percentage = _percentage / 100.0f;

        Color _c = _procedureCompletionMeter.GetTextGradient().Evaluate(_percentage);

        _c.a = 1.0f;

        _tx.color = _c;

        if(_tx.gameObject.GetComponent<Outline>() == null)
        {
            //_updateMeterTextBoolean = fals
            return;
        }

        Outline _outline = _tx.gameObject.GetComponent<Outline>();

        _c = ToolsStruct.ChangeColorValue(_c, 0.5f, 0.5f);

        _outline.effectColor = _c;
    }

    public void SignalToUpdateMeter()
    {
        _updateMeterTextBoolean = true;
    }

    public void ISetActionsOfNoButton()
    {
        _yesOrNoCanvas.GetNoButton().onClick.AddListener(delegate { _procedureCanvas.gameObject.SetActive(true); });

        _yesOrNoCanvas.GetNoButton().onClick.AddListener(delegate { _yesOrNoCanvas.gameObject.SetActive(false); });

        _yesOrNoCanvas.GetNoButton().onClick.AddListener(delegate { _yesOrNoCanvas.GetNoButton().onClick.RemoveAllListeners(); });
    }

    public void ISetActionsOfYesButtonToQuit()
    {
        Button _yesButton = _yesOrNoCanvas.GetYesButton();

        _yesButton.onClick.AddListener(delegate { _mainCanvases.SetCanvasesOn(true); });

        _yesButton.onClick.AddListener(delegate { EndMachine(); });

        _yesButton.onClick.AddListener(delegate { _yesOrNoCanvas.gameObject.SetActive(false); });

        _yesButton.onClick.AddListener(delegate { _yesButton.onClick.RemoveAllListeners(); });
    }

    public void ISetActionsOfYesButtonToRestart()
    {

    }
}
