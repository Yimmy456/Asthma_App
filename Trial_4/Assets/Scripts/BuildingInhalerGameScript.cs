using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BuildingInhalerGameScript : MatchingGameCanvasScript
{

    [ContextMenu("Add Components.")]
    protected override void AddToBnHList()
    {
        _presetBlocksAndHoles.Add(new InhalerMatchingBlockAndHoleClass());
    }

    [SerializeField]
    GameObject _cap;

    int _currentPhaseNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentGame == this)
        {
            UpdateStatus();
        }
    }

    public int GetCurrentPhaseNumber()
    {
        return _currentPhaseNumber;
    }

    public override void StartGame()
    {
        if(_currentGame != null || _gameSpace == null || _spawningArea == null || _floor == null || _mainContainer == null)
        {
            return;
        }

        base.StartGame();

        _currentPhaseNumber = 0;

        Quaternion _currentHoleRotation;

        Vector3 _currentHolePosition;

        List<Vector3> _blockPositions = new List<Vector3>();

        for(int _i = 0; _i < _presetBlocksAndHoles.Count; _i++)
        {
            InhalerMatchingBlockAndHoleClass _currentPreset = _presetBlocksAndHoles[_i] as InhalerMatchingBlockAndHoleClass;

            if (_currentPreset == null)
            {
                Debug.LogError("This is not a valid type of preset.");

                continue;
            }

            if (_currentPreset.GetGameBlock() == null || _currentPreset.GetGameBlockHole() == null)
            {
                Debug.LogError("No block or hole to instantiate.");

                continue;
            }

            //string _givenName = _currentPreset.GetMatchingAttribute();

            //1. Instantiating Block

            GameObject _newBlock = Instantiate(_currentPreset.GetGameBlock().gameObject);

            if(_currentPreset.GetMatchingAttribute() == "Inhaler Neck")
            {
                GetCap(_newBlock);
            }

            base.FindSpawningSpot(ref _blockPositions, ref _newBlock);

            _newBlock.transform.parent = _spawningArea.transform;

            _newBlock.transform.localScale = (Vector3.one * _spawningSizeForBlocks);

            _newBlock.GetComponent<InhalerMatchingObjectScript>().SetMatchingGameCanvas(this);

            //_newBlock.GetComponent<InhalerMatchingObjectScript>().SetObjectName(_givenName);

            _gameProperties.AddObjectToList(_newBlock.GetComponent<InhalerMatchingObjectScript>());

            _gameProperties.AddObjectsAsGO(_newBlock);

            _currentBlocksAndHoles.AddBlock(_newBlock.GetComponent<InhalerMatchingObjectScript>());

            _newBlock.GetComponent<DraggableClass>().SetCamera(_gameProperties.GetCamera());

            _newBlock.GetComponent<DraggableClass>().GetBody().velocity = Vector3.zero;

            _newBlock.transform.parent = _mainContainer;

            //2. Instantiating Hole

            GameObject _newHole = Instantiate(_currentPreset.GetGameBlockHole().gameObject);

            _currentHolePosition = _currentPreset.GetGameBlockHole().gameObject.transform.position;

            _currentHoleRotation = _currentPreset.GetGameBlockHole().gameObject.transform.rotation;

            _newHole.transform.position = _currentHolePosition;

            _newHole.transform.rotation = _currentHoleRotation;

            if (_currentPreset.GetMatchingAttribute() == "Medicine")
            {
                _newHole.transform.localScale = (Vector3.one * _spawningSizeForHoles * 0.01f);
            }
            else
            {
                _newHole.transform.localScale = (Vector3.one * _spawningSizeForHoles);
            }

            _newHole.transform.parent = _gameSpace.transform;

            InhalerMatchingObjectHoleScript _holeInfo = _newHole.GetComponent<InhalerMatchingObjectHoleScript>();

            if(_i != 0)
            {
                _newHole.SetActive(false);
            }

            try
            {
                if (_holeInfo != null)
                {
                    InhalerInformationClass _currentInfo;

                    _holeInfo.SetHoleGameCanvas(this);

                    _currentInfo = InhalerManagerScript.GetInstance().GetInhalerInfoList()[_i];

                    _holeInfo.SetCamera(_gameProperties.GetCamera());

                    //_holeInfo.SetHoleName(_givenName);

                    _currentBlocksAndHoles.AddHole(_holeInfo);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("The error occurs at index '" + _i + "'.");
            }
        }
    }

    void GetCap(GameObject _input)
    {
        if(_input == null)
        {
            return;
        }

        Transform _tr = _input.transform;

        _tr = _tr.Find("Cap");

        if(_tr != null)
        {
            _cap = _tr.gameObject;
        }
    }

    void UpdateStatus()
    {
        MatchingGameHoleScript _currentHole;

        if(_currentPhaseNumber == 0 && _currentBlocksAndHoles.GetHoles()[0].GetObjectPlaced())
        {
            _currentHole = _currentBlocksAndHoles.GetHoles()[0];

            _currentHole.GetRenderer().materials[0].SetFloat("_Alpha", 0.0f);

            _currentHole = _currentBlocksAndHoles.GetHoles()[1];

            _currentHole.gameObject.SetActive(true);

            _currentPhaseNumber = 1;
        }
    }
}
