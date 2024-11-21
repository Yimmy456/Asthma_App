using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class MatchingGameCanvasScript : GameGenericMBScript<MatchingGameBlockScript>
{
    [Header("2. Variables for All Matching Games")]

    [SerializeReference]
    protected List<MatchingBlockAndHoleClass> _presetBlocksAndHoles;

    [SerializeField]
    protected CurrentBlocksAndHolesDefaultClass _currentBlocksAndHoles;

    [SerializeField]
    protected GameObject _floor;

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
    protected MinMaxV3ValuesClass _minMaxV3Values;

    [SerializeField]
    protected bool _reverseDirectionForHoles;

    [SerializeField]
    protected Transform _currentRotation;

    [SerializeField]
    protected float _draggingZOffset = -3.8f;

    [SerializeField]
    Vector3 _additionalLookingAngles;

    [SerializeField]
    Camera _spaceCamera;

    protected Vector3 _initialPositionForHoles;

    protected Vector3 _currentlySelectedPositionForHoles;

    protected int _addedSpace = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //_completionMeter = _gameProperties.GetMeter();

        _minMaxV3Values.MaintainValues();
    }

    public CurrentBlocksAndHolesDefaultClass GetCurrentBlocksAndHoles()
    {
        return _currentBlocksAndHoles;
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

    protected virtual void FindSpawningSpot(ref List<Vector3> _posListInput, ref GameObject _gameObjectInput)
    {
        bool _spotFound = false;

        bool _rejectSpot = false;

        Vector3 _currentPos = Vector3.zero;

        float _dist;

        while (!_spotFound)
        {
            _currentPos = _minMaxV3Values.GetRandomVector3();

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

    protected void AlignHolePositions()
    {
        float _pos = (_currentBlocksAndHoles.GetHoleGOs().Count + _addedSpace) * _addedDistanceForHoles;

        _pos = _pos / -2.0f;

        float _a = _addedDistanceForHoles / 2.0f;

        _pos = _pos + _a;

        for(int _i = 0; _i < _currentBlocksAndHoles.GetHoleGOs().Count; _i++)
        {
            Transform _t = _currentBlocksAndHoles.GetHoleGOs()[_i].transform;

            Vector3 _v3 = _t.localPosition;
            
            _v3.z = _v3.z + _pos;

            if(_reverseDirectionForHoles)
            {
                _v3.z = -_v3.z;
            }

            _t.localPosition = _v3;
        }
    }

    protected void LookIntoCamera()
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

    protected void RotateSpaceFunction()
    {
        if(_gameSpace == null || _camera == null || _currentGame != this || _spaceCamera == null)
        {
            return;
        }

        Vector3 _pos = _spaceCamera.WorldToScreenPoint(_camera.gameObject.transform.position);

        bool _withinSight = _pos.z > 0.0f && _pos.x > 0.0f && _pos.x < Screen.width && _pos.y > 0.0f && _pos.y < Screen.height;

        float _distance = Vector3.Distance(_spaceCamera.gameObject.transform.position, _camera.gameObject.transform.position);

        if(_withinSight && _distance <= 5.0f)
        {
            return;
        }

        var _lookPosCam = _camera.gameObject.transform.position - _gameSpace.transform.position;

        _lookPosCam.y = 0.0f;

        var _rot = Quaternion.LookRotation(_lookPosCam);

        _rot = _rot * Quaternion.Euler(_additionalLookingAngles);

        _gameSpace.transform.rotation = Quaternion.Slerp(_gameSpace.transform.rotation, _rot, Time.deltaTime);
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

        foreach(GameObject _hole in _currentBlocksAndHoles.GetHoleGOs())
        {
            if(_hole == null)
            {
                continue;
            }

            _hole.transform.rotation = _currentRotation.rotation;
        }
    }
}
