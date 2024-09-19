using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BuildingInhalerGameScript : MatchingGameCanvasScript
{
    [SerializeField]
    HighlightingAnimationClass _highlightingAnimationProperties;

    [SerializeField]
    List<GameObject> _arrowLocations;

    List<GameObject> _spawnedArrowLocations = new List<GameObject>();

    [ContextMenu("Add Components.")]
    protected override void AddToBnHList()
    {
        _presetBlocksAndHoles.Add(new InhalerMatchingBlockAndHoleClass());
    }

    [SerializeField]
    GameObject _cap;

    int _currentPhaseNumber = 0;

    Coroutine _animationCoroutine;

    Vector3 _capLocalPosition;

    // Start is called before the first frame update
    void Start()
    {
        /*if(_completionMeter == null)
        {
            _completionMeter = new MeterClass();
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentGame == this)
        {
            UpdateStatus();

            _gameProperties.UpdateUI();
        }
    }

    public HighlightingAnimationClass GetHighlightingAnimationProperties()
    {
        return _highlightingAnimationProperties;
    }

    public int GetCurrentPhaseNumber()
    {
        return _currentPhaseNumber;
    }

    public List<GameObject> GetArrowLocations()
    {
        return _arrowLocations;
    }

    public List<GameObject> GetSpawnedArrowLocations()
    {
        return _spawnedArrowLocations;
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

        _floor.SetActive(true);

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

            _newBlock.GetComponent<InhalerMatchingObjectScript>().GetDraggableProperties().SetMaintainZDistance(false);

            _newBlock.GetComponent<InhalerMatchingObjectScript>().GetDraggableProperties().SetApplyGravity(true);

            _newBlock.GetComponent<InhalerMatchingObjectScript>().SetObjectName(_currentPreset.GetMatchingAttribute());

            //_newBlock.GetComponent<InhalerMatchingObjectScript>().SetObjectName(_givenName);

            _gameProperties.AddObjectToList(_newBlock.GetComponent<InhalerMatchingObjectScript>());

            _gameProperties.AddObjectsAsGO(_newBlock);

            _currentBlocksAndHoles.AddBlock(_newBlock.GetComponent<InhalerMatchingObjectScript>());

            _newBlock.GetComponent<DraggableClass>().SetCamera(_gameProperties.GetCamera());

            _newBlock.GetComponent<DraggableClass>().GetBody().velocity = Vector3.zero;

            _newBlock.transform.parent = _mainContainer;

            _newBlock.SetActive(true);

            //2. Instantiating Hole

            GameObject _newHole = Instantiate(_currentPreset.GetGameBlockHole().gameObject);

            _currentHolePosition = _currentPreset.GetGameBlockHole().gameObject.transform.position;

            _currentHoleRotation = _currentPreset.GetGameBlockHole().gameObject.transform.rotation;

            _newHole.transform.position = _currentHolePosition;

            _newHole.transform.rotation = _currentHoleRotation;

            _newHole.transform.parent = _gameSpace.transform;

            _newHole.transform.localScale = Vector3.one;

            InhalerMatchingObjectHoleScript _holeInfo = _newHole.GetComponent<InhalerMatchingObjectHoleScript>();

            if(_i == 0)
            {
                _newHole.SetActive(true);
            }
            else
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

                    _holeInfo.SetHoleName(_currentPreset.GetMatchingAttribute());

                    //_holeInfo.SetHoleName(_givenName);

                    _currentBlocksAndHoles.AddHole(_holeInfo);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("The error occurs at index '" + _i + "'.");
            }
        }

        GameObject _go;

        for(int _i = 0;  _i < _arrowLocations.Count; _i++)
        {
            _go = Instantiate(_arrowLocations[_i]);

            _go.transform.parent = _gameSpace.transform;

            _go.transform.position = _arrowLocations[_i].transform.position;

            _go.transform.rotation = _arrowLocations[_i].transform.rotation;

            _go.transform.localScale = Vector3.one;

            _spawnedArrowLocations.Add(_go);

            _go.SetActive(_i == 0);
        }

        _gameProperties.GetMeter().SetMaxValue(4);

        _gameProperties.GetMeter().SetValue(0);

        _gameProperties.SignalToUpdateUI();
    }

    void GetCap(GameObject _input)
    {
        if(_input == null)
        {
            return;
        }

        Transform _tr = _input.transform;

        _tr = _tr.Find("Cap 2");

        if(_tr == null)
        {
            return;
        }

        _cap = _tr.gameObject;

        if(_cap.GetComponent<MeshCollider>() != null)
        {
            _cap.GetComponent<MeshCollider>().enabled = false;
        }

        if(_cap.GetComponent<DraggableClass>() != null)
        {
            _cap.GetComponent<DraggableClass>().SetDraggableOn(false);

            _cap.GetComponent<DraggableClass>().SetCamera(_gameProperties.GetCamera());
        }

        _capLocalPosition = _cap.transform.localPosition;
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

            _spawnedArrowLocations[0].SetActive(false);

            _currentPhaseNumber = 1;

            _spawnedArrowLocations[1].SetActive(true);

            //_gameProperties.GetMeter().AddToValue(1);

            _gameProperties.SignalToUpdateUI();
        }

        if(_currentPhaseNumber == 1 && _currentBlocksAndHoles.GetHoles()[1].GetObjectPlaced())
        {
            _currentHole = _currentBlocksAndHoles.GetHoles()[1];

            _currentHole.GetRenderer().materials[0].SetFloat("_Alpha", 0.0f);

            TurnOnCapComponent();

            _currentPhaseNumber = 2;

            _spawnedArrowLocations[1].SetActive(false);

            _spawnedArrowLocations[2].SetActive(true);

            //_gameProperties.GetMeter().AddToValue(1);

            _gameProperties.SignalToUpdateUI();
        }

        if(_currentPhaseNumber == 2 && _cap != null)
        {
            if (_cap.GetComponent<DraggableClass>() != null)
            {
                if (_cap.GetComponent<DraggableClass>().GetTouchPhase() == TouchPhase.Ended && _cap.transform.localPosition.z >= 0.035f)
                {
                    _currentHole = _currentBlocksAndHoles.GetHoles()[2];

                    _currentHole.GetRenderer().materials[0].SetFloat("_Alpha", 0.7f);

                    _currentHole.gameObject.SetActive(true);

                    _currentPhaseNumber = 3;

                    _highlightingAnimationProperties.SetAnimateBool(false);

                    if(_animationCoroutine != null)
                    {
                        StopCoroutine(_animationCoroutine);
                    }

                    _cap.layer = 0;

                    Destroy(_cap);

                    _gameProperties.GetMeter().AddToValue(1);

                    _spawnedArrowLocations[2].SetActive(false);

                    _spawnedArrowLocations[3].SetActive(true);

                    _gameProperties.SignalToUpdateUI();
                }
                else if(_cap.GetComponent<DraggableClass>().GetTouchPhase() == TouchPhase.Ended)
                {
                    _cap.transform.localPosition = _capLocalPosition;
                }
            }
        }
    }

    public override void StopGame()
    {
        base.StopGame();

        _currentPhaseNumber = 0;

        _floor.SetActive(false);

        foreach(GameObject _go in  _spawnedArrowLocations)
        {
            Destroy(_go);
        }

        _spawnedArrowLocations.Clear();

        _currentBlocksAndHoles.ClearLists();

        if (_currentGame == this)
        {
            _currentGame = null;
        }
    }

    public override void QuitGame()
    {
        _gameProperties.GetYesOrNoCanvas().GetYesButton().onClick.AddListener(delegate { StopGame(); });

        base.QuitGame();
    }

    void TurnOnCapComponent()
    {
        if(_cap == null)
        {
            return;
        }

        if (_cap.GetComponent<MeshCollider>() != null)
        {
            _cap.GetComponent<MeshCollider>().enabled = true;
        }

        if (_cap.GetComponent<DraggableClass>() != null)
        {
            _cap.GetComponent<DraggableClass>().SetDraggableOn(true);
        }

        _cap.layer = 7;

        _animationCoroutine = StartCoroutine(_highlightingAnimationProperties.Animate());
    }
}
