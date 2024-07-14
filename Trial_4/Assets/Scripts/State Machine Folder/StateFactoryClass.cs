using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFactoryClass
{
    StateMachineScript _stateMachine;

    public StateFactoryClass(StateMachineScript _stateMachineInput)
    {
        _stateMachine = _stateMachineInput;
    }

    public SequenceState GetIntroductionState()
    {
        return new IntroductionState(_stateMachine, this);
    }

    public SequenceState GetStage1State()
    {
        return new MainStageState(_stateMachine, this, 1);
    }

    public SequenceState GetStage2State()
    {
        return new MainStageState(_stateMachine, this, 2);
    }

    public SequenceState GetStage3State()
    {
        return new MainStageState(_stateMachine, this, 3);
    }

    public SequenceState GetStage4State()
    {
        return new MainStageState(_stateMachine, this, 4);
    }

    public SequenceState GetStage5State()
    {
        return new MainStageState(_stateMachine, this, 5);
    }

    public SequenceState GetLectureState()
    {
        return new LectureState(_stateMachine, this, 5.0f);
    }

    public SequenceState GetGameState()
    {
        return new GameState(_stateMachine, this);
    }

    public SequenceState GetBadgeState()
    {
        return new BadgeState(_stateMachine, this);
    }

    public SequenceState GetConclusionState()
    {
        return new ConclusionState(_stateMachine, this);
    }

}
