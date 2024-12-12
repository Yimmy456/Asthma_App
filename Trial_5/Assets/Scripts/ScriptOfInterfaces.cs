using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface YesOrNoInterface
{
    public void ISetActionsOfNoButton();

    public void ISetActionsOfYesButtonToQuit();

    public void ISetActionsOfYesButtonToRestart();

    //public void ActionsOfNoButton();

    //public void ActionsOfYesButton();
}

public interface PlayDialogueInterface
{
    //public int _coroutine;

    public void IPlayDialogue();
}

public interface RewardingBadgeInterface
{
    public abstract void IRewardBadge();
}


public interface ExperienceInterface
{
    public abstract void IStartExperience();
    public abstract void IStartExperience(int _input);
    public abstract void IStartExperience(string _input);
    public abstract void IStopExperience();
    public abstract void ICompleteExperience();
    public abstract void IUpdateExperience();
    public abstract void IChooseToQuitExperience();
    public abstract void IChooseToRestartExperience();
    public abstract void IResumeExperience();
    public abstract IEnumerator IWaitUntilCompletion();
}