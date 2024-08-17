using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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

        if (_isRootState)
        {
            _stateMachine.SetCurrentState(_input);
        }
        else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(_input);
        }

        _input.EnterState();
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
        Debug.Log("We are entering the introduction state.");
    }

    public override void UpdateState()
    {
        Debug.Log("We are updating the introduction state.");

        CheckSwitchState();
    } 

    public override void ExitState()
    {
        Debug.Log("We are leaving the introduction state.");
    }

    public override void CheckSwitchState()
    {
        if(!_stateMachine.GetGoToNextState())
        {
            return;
        }

        ExitStates();

        _stateMachine.SetCurrentState(_factory.GetStage1State());

        _stateMachine.GetCurrentState().EnterState();

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

        if(_currentSubState != null)
        {
            _currentSubState.EnterState();
        }
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
                case 1:;
                    SwitchState(_factory.GetStage2State());
                    break;
                case 2:
                    SwitchState(_factory.GetStage3State());
                    break;
                case 3:
                    SwitchState(_factory.GetStage4State());
                    break;
                case 4:
                    SwitchState(_factory.GetStage5State());
                    break;
                default:
                    SwitchState(_factory.GetConclusionState());
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
        Debug.Log("We are entering stage " + _stageNumber.ToString() + " state.");
    }

    public override void UpdateState()
    {
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

        _stateMachine.GetProcedureCanvas().GetRestartButton().onClick.AddListener(delegate { _stateMachine.StartLecture(this); });

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

        _stateMachine.SignalToUpdateMeter();

        _stateMachine.GetProcedureCanvas().GetRestartButton().onClick.RemoveAllListeners();

        _stateMachine.EndLectureImmediately();

        _stateMachine.GetProcedureCanvas().GetRestartButton().gameObject.SetActive(false);

        Debug.Log(_debugText);
    }
}


public class GameState : SequenceState
{
    GameMBScript _game;

    bool _gameDone = false;

    public GameState(StateMachineScript _machineInput, StateFactoryClass _factory) : base(_machineInput, _factory)
    {
        //SelectGame();

        _gameDone = false;
    }

    public override void UpdateState()
    {
        string _debugText = "We are updating the game state";

        if (_currentSuperState != null)
        {
            if ((_currentSuperState as MainStageState) != null)
            {
                MainStageState _mss = _currentSuperState as MainStageState;

                _debugText = _debugText + " of stage " + _mss.GetStageNumber().ToString();
            }
        }

        if (_game != null)
        {
            if (_game.GetProgressUpdated())
            {
                _stateMachine.SignalToUpdateMeter();
            }

            if (_game.GetCompletionMeter() != null)
            {
                Debug.Log("The completion percentage of this game is " + _game.GetCompletionMeter().GetPercentage().ToString("0.00") + "%.");

                if (_game.GetCompletionMeter().GetPercentage() == 100.0f && !_gameDone)
                {
                    _stateMachine.GetProcedureCanvas().GetNextButton().gameObject.SetActive(true);

                    if (_stateMachine.GetInformationCanvas() != null)
                    {
                        if (_stateMachine.GetInformationCanvas().gameObject.activeSelf)
                        {
                            InformationCanvasScript _infoC = _stateMachine.GetInformationCanvas();

                            _infoC.GetNextButton().gameObject.SetActive(false);

                            UnityAction _call = delegate
                            {
                                _stateMachine.SetGoToNextState(true);

                                _infoC.GetNextButton().gameObject.SetActive(true);

                                _infoC.gameObject.SetActive(false);
                            };

                            _stateMachine.GetProcedureCanvas().GetNextButton().onClick.AddListener(_call);

                            _stateMachine.GetProcedureCanvas().GetNextButton().onClick.AddListener(delegate { _stateMachine.GetProcedureCanvas().GetNextButton().onClick.RemoveListener(_call); });
                        }
                        else
                        {
                            _stateMachine.SetGoToNextState(true);
                        }
                    }
                    _gameDone = true;
                }
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

        _gameDone = false;

        if (_currentSuperState != null)
        {
            if ((_currentSuperState as MainStageState) != null)
            {
                MainStageState _mss = _currentSuperState as MainStageState;

                _debugText = _debugText + " of stage " + _mss.GetStageNumber().ToString();
            }
        }

        _debugText = _debugText + ".";

        if(_stateMachine.GetProcedureCanvas() != null)
        {
            _stateMachine.GetProcedureCanvas().GetNextButton().gameObject.SetActive(false);
        }

        Debug.Log(_debugText);

        SelectGame();
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

        if (_game != null)
        {
            _game.StopGame();
        }

        Debug.Log(_debugText);
    }

    void SelectGame()
    {
        if(_currentSuperState == null)
        {
            return;
        }

        if((_currentSuperState as MainStageState) == null)
        {
            return;
        }

        Debug.Log("We are starting the game.");

        MainStageState _mss = _currentSuperState as MainStageState;

        int _stageNo = _mss.GetStageNumber();

        if(_stageNo == 1)
        {
            _game = _stateMachine.GetLetterGame();

            LetterGameScript _letterGame = _game as LetterGameScript;

            _letterGame.StartGame(0);

            _letterGame.GetGameProperties().GetGameCanvas().gameObject.SetActive(false);
        }

        if(_stageNo == 2)
        {
            _game = _stateMachine.GetCardGame();

            CardGameScript _cardGame = _game as CardGameScript;

            _cardGame.StartGame(0);

            _cardGame.GetGameProperties().GetGameCanvas().gameObject.SetActive(false);
        }
    }
}

public class BadgeState : SequenceState
{
    BadgeScript _badge;

    public BadgeState(StateMachineScript _machineInput, StateFactoryClass _factoryInput) : base(_machineInput, _factoryInput)
    {
        //GetSelectedBadge();    
    }

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

        GetSelectedBadge();

        if(_stateMachine.GetProcedureCanvas() != null)
        {
            _stateMachine.GetProcedureCanvas().GetNextButton().gameObject.SetActive(true);
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

    void GetSelectedBadge()
    {
        if (BadgesManagerScript.GetInstance() == null || _currentSuperState == null || _stateMachine.GetNewBadgeCanvas() == null)
        {
            return;
        }

        if ((_currentSuperState as MainStageState) == null)
        {
            return;
        }

        MainStageState _mss = _currentSuperState as MainStageState;

        int _nameIndex = _mss.GetStageNumber() - 1;

        string _name = _stateMachine.GetBadgeNames()[_nameIndex];

        _badge = BadgesManagerScript.GetInstance().GetBadgeByName(_name);

        if (_badge != null)
        {
            Debug.Log("We have acquired the requested badge!");
        }
        else
        {
            Debug.LogError("Sorry, but no badge with the name " + @"""" + _name + @"""" + " was found.");

            return;
        }

        BadgesManagerScript.GetInstance().SetBadgeEarned(_badge);

        Canvas _newBadgeCanvas = _stateMachine.GetNewBadgeCanvas();

        Transform _tImage = _newBadgeCanvas.gameObject.GetComponent<RectTransform>().Find("Badge Image");

        Transform _tText = _newBadgeCanvas.gameObject.GetComponent<RectTransform>().Find("Congratulations Text");

        if (_tImage == null || _tText == null)
        {
            return;
        }

        Image _img = _tImage.gameObject.GetComponent<Image>();

        Text _txt = _tText.gameObject.GetComponent<Text>();

        if (_img == null || _txt == null)
        {
            return;
        }

        _newBadgeCanvas.gameObject.SetActive(true);

        _newBadgeCanvas.gameObject.GetComponent<RectTransform>().Find("Next Button").gameObject.SetActive(false);

        _img.sprite = _badge.GetBadgeSprite();

        _txt.text = "Congratulations! You have earned the " + @"""" + _badge.GetBadgeName() + @"""" + " badge!";

        UnityAction _action = delegate
        {
            _newBadgeCanvas.gameObject.GetComponent<RectTransform>().Find("Next Button").gameObject.SetActive(true);

            _newBadgeCanvas.gameObject.SetActive(false);

            _stateMachine.GetProcedureCanvas().GetNextButton().onClick.RemoveAllListeners();

            _stateMachine.GetProcedureCanvas().GetNextButton().onClick.AddListener(delegate { _stateMachine.SetGoToNextState(true); });
        };

        _stateMachine.GetProcedureCanvas().GetNextButton().onClick.AddListener(_action);
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