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

public interface GameCorrectOrWrongInterface
{
    public abstract void IGameCorrect();

    public abstract void IGameCorrect(int _indexInput);

    public abstract void IGameIncorrect();

    public abstract void IGameIncorrect(int _indexInput);

    public abstract void IGameCorrect(string _dialogueNameInput);

    public abstract void IGameCorrect(string _dialogueNameInput, int _indexInput);

    public abstract void IGameIncorrect(string _dialogueNameInput);

    public abstract void IGameIncorrect(string _dialogueNameInput, int _indexInput);
}

public interface DialogueDelayInterface
{
    public abstract IEnumerator PlayDialogueAfterDelay(string _dialogueNameInput, float _delayInput = 0.0f);

    public abstract IEnumerator PlayDialogueAfterDelay(string _dialogueName1Input, string _dialogueName2Input, float _delayInput = 0.0f);

    public abstract IEnumerator PlayDialogueAfterDelay(string _dialogueName1Input, string _dialogueName2Input, string _dialogueName3Input, float _delayInput = 0.0f);

    public abstract IEnumerator PlayDialogueAfterDelay(List<string> _dialogueNamesInput, float _delayInput = 0.0f);
}

public interface StateInterface
{    
    public abstract void IOnStateEnter();

    public abstract void IOnStateStay();

    public abstract void IOnStateExit();
}