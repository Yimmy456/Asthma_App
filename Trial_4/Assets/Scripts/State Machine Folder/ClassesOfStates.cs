using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;


public abstract class BaseState
{
    protected bool _isRootState = false;

    protected StateMachineScript _stateMachine;

    protected StateFactoryClass _factory;

    protected BaseState _currentSuperState;

    protected BaseState _currentSubState;

    public BaseState(StateMachineScript _stateMachineInput, StateFactoryClass _factoryInput)
    {
        _stateMachine = _stateMachineInput;

        _factory = _factoryInput;
    }

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract void UpdateState();

    public abstract void CheckSwitchState();

    public abstract void InitializeSubState();

    public bool GetIsRootState()
    {
        return _isRootState;
    }

    public void UpdateStates()
    {
        UpdateState();

        if(_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }

    public void ExitStates()
    {
        ExitState();

        if(_currentSubState != null)
        {
            _currentSubState.ExitStates();
        }
    }

    protected void SwitchState(BaseState _input)
    {
        if(_input == null)
        {
            return;
        }

        ExitStates();

        _input.EnterState();

        if (_isRootState)
        {
            _stateMachine.SetCurrentState(_input);
        }
        else if(_currentSuperState != null)
        {
            _currentSuperState.SetSubState(_input);
        }
    }

    protected void SetSuperState(BaseState _input)
    {
        _currentSuperState = _input;
    }

    protected void SetSubState(BaseState _input)
    {
        _currentSubState = _input;

        _currentSubState.SetSuperState(this);
    }
}

public class SequenceState : BaseState
{

    public SequenceState(StateMachineScript _machineInput, StateFactoryClass _factoryInput) : base (_machineInput, _factoryInput)
    {

    }

    public override void EnterState()
    {
        //throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        //throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        //throw new System.NotImplementedException();
    }

    public override void CheckSwitchState()
    {
        //throw new System.NotImplementedException();
    }

    public override void InitializeSubState()
    {

    }
}

public class IntroductionState : SequenceState
{
    public IntroductionState(StateMachineScript _machineInput, StateFactoryClass _factoryInput) : base(_machineInput, _factoryInput)
    {
        
    }

    public override void EnterState()
    {
        //base.EnterState();
        Debug.Log("We are entering the introduction state.");
    }

    public override void UpdateState()
    {
        //base.UpdateState();

        Debug.Log("We are updating the introduction state.");

        CheckSwitchState();
        //CheckSwitchState();
    }

    public override void ExitState()
    {
        //base.ExitState();

        Debug.Log("We are leaving the introduction state.");
    }

    public override void CheckSwitchState()
    {
        if(!_stateMachine.GetGoToNextState())
        {
            return;
        }

        _stateMachine.SetCurrentState(_factory.GetStage1State());

        _stateMachine.SetGoToNextState(false);
    }
}

public class MainStageState : SequenceState
{
    protected int _stageNumber = -1;

    protected int _partNumber = -1;

    public MainStageState(StateMachineScript _machineInput, StateFactoryClass _factoryInput, int _stageNumberInput) : base (_machineInput, _factoryInput)
    {
        InitializeSubState();

        _isRootState = true;

        _stageNumber = _stageNumberInput;

        _partNumber = 0;
    }

    public int GetStageNumber()
    {
        return _stageNumber;
    }

    public int GetPartNumber()
    {
        return _partNumber;
    }

    public override void CheckSwitchState()
    {
        if(!_stateMachine.GetGoToNextState())
        {
            return;
        }



        if (_partNumber == 2 || ((_currentSubState as BadgeState) != null))
        {
            _partNumber = 0;

            switch (_stageNumber)
            {
                case 1:
                    _stateMachine.SetCurrentState(_factory.GetStage2State());
                    break;
                case 2:
                    _stateMachine.SetCurrentState(_factory.GetStage3State());
                    break;
                case 3:
                    _stateMachine.SetCurrentState(_factory.GetStage4State());
                    break;
                case 4:
                    _stateMachine.SetCurrentState(_factory.GetStage5State());
                    break;
                default:
                    _stateMachine.SetCurrentState(_factory.GetConclusionState());
                    break;
            }
        }

        _stateMachine.SetGoToNextState(false);
    }

    public override void InitializeSubState()
    {
        SetSubState(_factory.GetLectureState());
    }

    public override void EnterState()
    {
        //base.EnterState();

        Debug.Log("We are entering stage " + _stageNumber.ToString() + " state.");
    }

    public override void UpdateState()
    {
        //base.UpdateState();

        Debug.Log("We are updating stage " + _stageNumber.ToString() + " state.");
    }

    public override void ExitState()
    {
        Debug.Log("We are exiting stage " + _stageNumber.ToString() + " state.");
    }
}

public class LectureState : SequenceState
{
    float _seconds = 0.0f;

    public LectureState(StateMachineScript _machineInput, StateFactoryClass _factoryInput, float _secondsInput) : base (_machineInput, _factoryInput)
    {
        _seconds = _secondsInput;
    }

    public float GetSeconds()
    {
        return _seconds;
    }

    public override void UpdateState()
    {
        //base.UpdateState();

        string _debugText = "We are updating the lecture state";

        if (_currentSuperState != null)
        {
            if ((_currentSuperState as MainStageState) != null)
            {
                MainStageState _mss = _currentSuperState as MainStageState;

                _debugText = _debugText + " of stage " + _mss.GetStageNumber().ToString();
            }
        }

        _debugText = _debugText + ".";

        Debug.Log(_debugText);


        CheckSwitchState();
    }

    public override void CheckSwitchState()
    {
        //base.CheckSwitchState();
        if(!_stateMachine.GetGoToNextState())
        {
            return;
        }

        SwitchState(_factory.GetGameState());

        _stateMachine.SetGoToNextState(false);
    }

    public override void EnterState()
    {
        string _debugText = "We are entering the lecture state";

        if(_currentSuperState != null)
        {
            if((_currentSuperState as MainStageState) != null)
            {
                MainStageState _mss = _currentSuperState as MainStageState;

                _debugText = _debugText + " of stage " + _mss.GetStageNumber().ToString();
            }
        }

        _debugText = _debugText + ".";

        _stateMachine.GetProcedureCanvas().GetNextButton().gameObject.SetActive(false);

        _stateMachine.StartLecture(this);

        Debug.Log(_debugText);
    }

    public override void ExitState()
    {
        string _debugText = "We are exiting the lecture state";

        if (_currentSuperState != null)
        {
            if ((_currentSuperState as MainStageState) != null)
            {
                MainStageState _mss = _currentSuperState as MainStageState;

                _debugText = _debugText + " of stage " + _mss.GetStageNumber().ToString();
            }
        }

        _debugText = _debugText + ".";

        _stateMachine.GetProcedureCompletionMeter().AddToValue(1);

        Debug.Log(_debugText);
    }
}


public class GameState : SequenceState
{
    GameMBScript _game;

    public GameState(StateMachineScript _machineInput, StateFactoryClass _factory) : base(_machineInput, _factory) { }

    public override void UpdateState()
    {
        //base.UpdateState();
        string _debugText = "We are updating the game state";

        if (_currentSuperState != null)
        {
            if ((_currentSuperState as MainStageState) != null)
            {
                MainStageState _mss = _currentSuperState as MainStageState;

                _debugText = _debugText + " of stage " + _mss.GetStageNumber().ToString();
            }
        }

        _debugText = _debugText + ".";

        Debug.Log(_debugText);

        CheckSwitchState();
    }

    public override void CheckSwitchState()
    {
        if(!_stateMachine.GetGoToNextState())
        {
            return;
        }

        SwitchState(_factory.GetBadgeState());

        _stateMachine.SetGoToNextState(false);
    }

    public GameMBScript GetGame()
    {
        return _game;
    }

    public override void EnterState()
    {
        string _debugText = "We are entering the game state";

        if (_currentSuperState != null)
        {
            if ((_currentSuperState as MainStageState) != null)
            {
                MainStageState _mss = _currentSuperState as MainStageState;

                _debugText = _debugText + " of stage " + _mss.GetStageNumber().ToString();
            }
        }

        _debugText = _debugText + ".";

        Debug.Log(_debugText);
    }

    public override void ExitState()
    {
        string _debugText = "We are exiting the game state";

        if (_currentSuperState != null)
        {
            if ((_currentSuperState as MainStageState) != null)
            {
                MainStageState _mss = _currentSuperState as MainStageState;

                _debugText = _debugText + " of stage " + _mss.GetStageNumber().ToString();
            }
        }

        _debugText = _debugText + ".";

        Debug.Log(_debugText);
    }
}

public class BadgeState : SequenceState
{
    public BadgeState(StateMachineScript _machineInput, StateFactoryClass _factoryInput) : base(_machineInput, _factoryInput) { }

    public override void UpdateState()
    {
        string _debugText = "We are updating the badge state";

        if (_currentSuperState != null)
        {
            if ((_currentSuperState as MainStageState) != null)
            {
                MainStageState _mss = _currentSuperState as MainStageState;

                _debugText = _debugText + " of stage " + _mss.GetStageNumber().ToString();
            }
        }

        _debugText = _debugText + ".";

        Debug.Log(_debugText);

        CheckSwitchState();
    }

    public override void EnterState()
    {
        string _debugText = "We are entering the badge state";

        if (_currentSuperState != null)
        {
            if ((_currentSuperState as MainStageState) != null)
            {
                MainStageState _mss = _currentSuperState as MainStageState;

                _debugText = _debugText + " of stage " + _mss.GetStageNumber().ToString();
            }
        }

        _debugText = _debugText + ".";

        Debug.Log(_debugText);
    }

    public override void ExitState()
    {
        string _debugText = "We are exiting the badge state";

        if (_currentSuperState != null)
        {
            if ((_currentSuperState as MainStageState) != null)
            {
                MainStageState _mss = _currentSuperState as MainStageState;

                _debugText = _debugText + " of stage " + _mss.GetStageNumber().ToString();
            }
        }

        _debugText = _debugText + ".";

        Debug.Log(_debugText);
    }

    public override void CheckSwitchState()
    {
        if(!_stateMachine.GetGoToNextState())
        {
            return;
        }

        if(_currentSuperState != null)
        {
            _currentSuperState.CheckSwitchState();
        }

        _stateMachine.SetGoToNextState(false);
    }
}

public class ConclusionState : SequenceState
{
    public ConclusionState(StateMachineScript _machineInput, StateFactoryClass _factoryInput) : base(_machineInput, _factoryInput)
    {
        _currentSubState = null;
    }

    public override void EnterState()
    {
        Debug.Log("We are entering the conclusion state.");
    }

    public override void UpdateState()
    {
        Debug.Log("We are updating the conclusion state.");

        CheckSwitchState();
    }

    public override void ExitState()
    {
        _stateMachine.EndMachine();

        Debug.Log("We are exiting the conclusion state and ending the machine.");
    }

    public override void CheckSwitchState()
    {
        if(!_stateMachine.GetGoToNextState())
        {
            return;
        }

        ExitState();
    }
}