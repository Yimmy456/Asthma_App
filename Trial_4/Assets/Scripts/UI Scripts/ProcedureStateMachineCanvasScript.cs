using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcedureStateMachineCanvasScript : CanvasScript
{
    [SerializeField]
    Button _infoButton;

    [SerializeField]
    Button _nextButton;

    [SerializeField]
    Button _restartButton;

    [SerializeField]
    Text _percentageText;

    //[SerializeField]
    //ProcedureStateMachineScript _stateMachine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //CheckMachineStatusUpdates();
    }

    public Button GetInfoButton()
    {
        return _infoButton;
    }

    public Button GetNextButton()
    {
        return _nextButton;
    }

    public Button GetRestartButton()
    {
        return _restartButton;
    }

    public Text GetPercentageText()
    {
        return _percentageText;
    }
}
