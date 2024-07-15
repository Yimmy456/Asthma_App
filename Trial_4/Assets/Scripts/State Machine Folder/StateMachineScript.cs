using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineScript : MonoBehaviour
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
    ProcedureStateMachineCanvasScript _procedureCanvas;

    [SerializeField]
    MeterClass _procedureCompletionMeter;

    Coroutine _lectureCoroutine;

    BaseState _currentState;

    StateFactoryClass _states;

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

        //_currentState.EnterState();

        //_machineOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_machineOn)
        {
            _currentState.UpdateStates();
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

        _machineOn = true;
    }

    public void EndMachine()
    {
        _stageNumber = -1;

        _machineOn = false;

        _goToNextState = false;

        _mainCanvases.SetCanvasesOn(true);

        _procedureCanvas.gameObject.SetActive(false);
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
}
