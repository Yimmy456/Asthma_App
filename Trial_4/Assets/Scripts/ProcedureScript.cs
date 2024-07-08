using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProcedureScript : MonoBehaviour
{
    [SerializeField]
    string _procedureName;

    [SerializeField]
    MeterClass _procedureCompletionMeter;

    [SerializeField]
    List<ProcedurePhaseClass> _phases;

    ProcedurePhaseClass _currentPhase;

    int _currentPhaseIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetProcedureName()
    {
        return _procedureName;
    }

    public MeterClass GetProcedureCompletionMeter()
    {
        return _procedureCompletionMeter;
    }

    public List<ProcedurePhaseClass> GetPhases()
    {
        return _phases;
    }

    public ProcedurePhaseClass GetCurrentPhase()
    {
        return _currentPhase;
    }

    public void SetCurrentPhase(ProcedurePhaseClass _input)
    {
        if (_phases.Contains(_input))
        {
            _currentPhase = _input;
        }
    }

    public void SetCurrentPhase(int _input)
    {
        if(!(_input >= 0 && _input < _phases.Count))
        {
            return;
        }

        _currentPhase = _phases[_input];
    }

    public int GetCurrentPhaseIndex()
    {
        return _currentPhaseIndex;
    }

    public void StartProcedure()
    {
        _currentPhase = _phases[0];

        _currentPhaseIndex = 0;

        _procedureCompletionMeter.SetValue(0);
    }

    public void EndProcedure()
    {
        _currentPhase = null;

        _currentPhaseIndex = -1;

        _procedureCompletionMeter.SetValue(0);
    }
}
