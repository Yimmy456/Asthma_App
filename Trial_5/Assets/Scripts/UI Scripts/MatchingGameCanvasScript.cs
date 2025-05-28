using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class MatchingGameCanvasScript : GameGenericMBScript<MatchingGameBlockScript>
{
    [Header("2. Variables for All Matching Games")]

    [SerializeField]
    protected GamePropertiesClass<MatchingGameHoleScript> _currentHoleProperties;

    [SerializeReference]
    protected List<MatchingBlockAndHoleClass> _presetBlocksAndHoles;

    //[SerializeField]
    //protected CurrentBlocksAndHolesDefaultClass _currentBlocksAndHoles;

    [SerializeField]
    protected GameObject _floor;

    [SerializeField]
    protected GameObject _walls;

    [SerializeField]
    protected Transform _spawningArea;

    [SerializeField]
    protected float _addedDistanceForHoles;

    [SerializeField]
    protected Vector3 _facingDirection;

    [SerializeField]
    protected float _spawningSizeForBlocks = 1.0f;

    [SerializeField]
    protected float _spawningSizeForHoles = 1.0f;

    [SerializeField]
    protected float _spawningDistanceForBlocks = 5.0f;

    [SerializeField]
    protected MinMaxV3ValuesClass _spawningRangesForBlocks;

    [SerializeField]
    protected bool _reverseDirectionForHoles;

    [SerializeField]
    protected Transform _currentRotation;

    [SerializeField]
    protected float _draggingZOffset = -3.8f;

    [SerializeField]
    protected Vector3 _additionalLookingAngles;

    protected Vector3 _initialPositionForHoles;

    protected Vector3 _currentlySelectedPositionForHoles;

    protected int _addedSpace = 0;

    [SerializeField]
    protected MatchingGameBlockScript _currentBlock;

    [SerializeField]
    protected MatchingGameHoleScript _currentHole;

    protected bool _holeIndicatorCountdownStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //_completionMeter = _gameProperties.GetMeter();

        _spawningRangesForBlocks.MaintainValues();
    }

    public GamePropertiesClass<MatchingGameHoleScript> GetCurrentHoleProperties()
    {
        return _currentHoleProperties;
    }

    public List<MatchingBlockAndHoleClass> GetPresetBlockAndHoles()
    {
        return _presetBlocksAndHoles;
    }

    public Transform GetCurrentRotation()
    {
        return _currentRotation;
    }

    public float GetDraggingZOffset()
    {
        return _draggingZOffset;
    }

    public MatchingGameBlockScript GetCurrentBlock()
    {
        return _currentBlock;
    }

    public MatchingGameHoleScript GetCurrentHole()
    {
        return _currentHole;
    }

    public void SetSpawningSizeForBlocks(float _input)
    {
        _spawningSizeForBlocks = _input;
    }

    public void SetSpawningSizeForHoles(float _input)
    {
        _spawningSizeForHoles = _input;
    }

    public void SetPresetBlocksAndHoles(List<MatchingBlockAndHoleClass> _input)
    {
        _presetBlocksAndHoles = _input;
    }

    public void SetCurrentBlock(MatchingGameBlockScript _input)
    {
        _currentBlock = _input;
    }

    public void SetCurrentHole(MatchingGameHoleScript _input)
    {
        _currentHole = _input;
    }

    protected virtual void FindSpawningSpot(ref List<Vector3> _posListInput, ref GameObject _gameObjectInput)
    {
        bool _spotFound = false;

        bool _rejectSpot = false;

        Vector3 _currentPos = Vector3.zero;

        float _dist;

        while (!_spotFound)
        {
            _currentPos = _spawningRangesForBlocks.GetRandomVector3();

            for (int _i = 0; _i < _posListInput.Count && !_rejectSpot; _i++)
            {
                _dist = Vector3.Distance(_currentPos, _posListInput[_i]);

                if(_dist <= _spawningDistanceForBlocks)
                {
                    _rejectSpot = true;
                }
            }

            if(!_rejectSpot)
            {
                _spotFound = true;
            }
            else
            {
                _rejectSpot = false;
            }
        }

        _gameObjectInput.transform.localPosition = _currentPos;

        _posListInput.Add(_currentPos);
    }

    protected virtual void AddToBnHList()
    {

    }

    public override void IStopExperience()
    {
        foreach(MatchingBlockAndHoleClass _cl in _presetBlocksAndHoles)
        {
            _cl.ResetIndex();
        }

        base.IStopExperience();
    }



    protected void AlignHolePositions()
    {
        float _pos = (_currentHoleProperties.GetListOfObjectsAsGO().Count + _addedSpace) * _addedDistanceForHoles;

        _pos = _pos / -2.0f;

        float _a = _addedDistanceForHoles / 2.0f;

        _pos = _pos + _a;

        for(int _i = 0; _i < _currentHoleProperties.GetListOfObjectsAsGO().Count; _i++)
        {
            Transform _t = _currentHoleProperties.GetListOfObjectsAsGO()[_i].transform;

            Vector3 _v3 = _t.localPosition;
            
            _v3.z = _v3.z + _pos;

            if(_reverseDirectionForHoles)
            {
                _v3.z = -_v3.z;
            }

            _t.localPosition = _v3;
        }
    }

    protected virtual void LookIntoCamera()
    {
        if(_gameSpace == null || _camera == null || _currentGame == null)
        {
            return;
        }

        if(_currentGame != this)
        {
            return;
        }

        var _lookPosCam = _camera.gameObject.transform.position - _gameSpace.transform.position;

        _lookPosCam.y = 0.0f;

        var _rot = Quaternion.LookRotation(_lookPosCam);

        _rot = _rot * Quaternion.Euler(_additionalLookingAngles);

        bool _notHolding = true;

        if(DraggableManagerClass.GetInstance() != null)
        {
            _notHolding = !DraggableManagerClass.GetInstance().GetDraggingSomething();
        }

        if (_notHolding)
        {
            _gameSpace.transform.rotation = Quaternion.Slerp(_gameSpace.transform.rotation, _rot, Time.deltaTime);
        }
    }

    protected void RotateHoles()
    {
        if(_currentRotation == null || _currentGame == null)
        {
            return;
        }

        if(_currentGame != this)
        {
            return;
        }

        foreach(GameObject _hole in _currentHoleProperties.GetListOfObjectsAsGO())
        {
            if(_hole == null)
            {
                continue;
            }

            _hole.transform.rotation = _currentRotation.rotation;
        }
    }

    public override void IGameCorrect(int _indexInput)
    {
        base.IGameCorrect(_indexInput);
    }

    public override void IGameCorrect()
    {
        base.IGameCorrect();
    }

    public override void IGameCorrect(string _dialogueNameInput)
    {
        base.IGameCorrect(_dialogueNameInput);
    }

    public override void IGameCorrect(string _dialogueNameInput, int _indexInput)
    {
        base.IGameCorrect(_dialogueNameInput, _indexInput);
    }

    public override void IGameIncorrect()
    {
        base.IGameIncorrect();
    }

    public override void IGameIncorrect(int _indexInput)
    {
        base.IGameIncorrect(_indexInput);
    }

    public override void IGameIncorrect(string _dialogueNameInput)
    {
        base.IGameIncorrect(_dialogueNameInput);
    }

    public override void IGameIncorrect(string _dialogueNameInput, int _indexInput)
    {
        base.IGameIncorrect(_dialogueNameInput, _indexInput);
    }

    protected virtual IEnumerator SetNewTargetForIndicator2()
    {
        yield break;
    }

    protected override void UpdateTargetForIndicator2()
    {
        if (_gameIndicatorCanvas == null)
        {
            return;
        }

        if (!PrepareGameIndicator())
        {
            return;
        }

        if (_gameIndicatorCanvasCountdownStarted)
        {
            return;
        }

        if (_completionMeter.GetPercentage() == 100.0f)
        {
            _gameIndicatorCanvas.gameObject.SetActive(false);

            TriggerStopNewTarget();

            _gameIndicatorCanvas.SetTargetObject(null);

            return;
        }

        if (_currentBlock != null)
        {
            if (_gameIndicatorCanvas.GetTargetObject() != null)
            {
                if (_gameIndicatorCanvas.GetTargetObject().GetComponent<MatchingGameHoleScript>())
                {
                    return;
                }
                else if (_gameIndicatorCanvas.GetTargetObject().GetComponent<MatchingGameBlockScript>() != null)
                {
                    _gameIndicatorCanvas.gameObject.SetActive(false);

                    _gameIndicatorCanvas.SetTargetObject(null);
                }
            }

            TriggerStartNewTarget();
        }
        else
        {
            if (_gameIndicatorCanvas.GetTargetObject() != null)
            {
                if (_gameIndicatorCanvas.GetTargetObject().GetComponent<MatchingGameBlockScript>())
                {
                    return;
                }
                else if (_gameIndicatorCanvas.GetTargetObject().GetComponent<MatchingGameHoleScript>() != null)
                {
                    _gameIndicatorCanvas.gameObject.SetActive(false);

                    _gameIndicatorCanvas.SetTargetObject(null);
                }
            }

            TriggerStartNewTarget();
        }
    }

    public void TriggerStartNewTarget()
    {
        if (_indicatorCoroutine != null)
        {
            StopCoroutine(_indicatorCoroutine);
        }

        _indicatorCoroutine = StartCoroutine(SetNewTargetForIndicator2());
    }

    public void TriggerStopNewTarget()
    {
        if (_indicatorCoroutine != null)
        {
            StopCoroutine(_indicatorCoroutine);
        }

        if (_gameIndicatorCanvasCountdownStarted)
        {
            _gameIndicatorCanvasCountdownStarted = false;
        }
    }
}
