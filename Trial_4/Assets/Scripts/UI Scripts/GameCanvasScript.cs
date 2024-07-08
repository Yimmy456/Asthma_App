using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvasScript : CanvasScript, YesOrNoInterface
{
    [SerializeField]
    protected Button _infoPanelButton;

    [SerializeField]
    protected Text _countText;

    [SerializeField]
    protected Text _percentageText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ISetActionsOfNoButton()
    {

    }

    public void ISetActionsOfYesButtonToQuit()
    {

    }

    public void ISetActionsOfYesButtonToRestart()
    {

    }
}
