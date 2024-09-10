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
    Vector3 _additionalLookingAngles;

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
        _completionMeter = _gameProperties.GetMeter();

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
        if(_gameSpace == null || _gameProperties.GetCamera() == null || _currentGame == null)
        {
            return;
        }

        if(_currentGame != this)
        {
            return;
        }

        var _lookPosCam = _gameProperties.GetCamera().gameObject.transform.position - _gameSpace.transform.position;

        _lookPosCam.y = 0.0f;

        //_lookPosCam = _lookPosCam;

        //var _addQ = Quaternion.LookRotation(_additionalLookingAngles);

        var _rotCam = Quaternion.LookRotation(-_lookPosCam) * Quaternion.Euler(_additionalLookingAngles);

        _gameSpace.transform.localRotation = Quaternion.Slerp(_gameSpace.transform.localRotation, _rotCam, Time.deltaTime);
    }

    protected virtual void AdjustContainer()
    {

    }
}
