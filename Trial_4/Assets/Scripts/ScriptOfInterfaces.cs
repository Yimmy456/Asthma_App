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
