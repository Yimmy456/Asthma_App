using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InhalerMatchingGameScript : MatchingGameCanvasScript
{
    [ContextMenu("Add To Preset List...")]
    protected override void AddToBnHList()
    {
        _presetBlocksAndHoles.Add(new InhalerMatchingBlockAndHoleClass());
    }


    //public Text _errorText;

    // Start is called before the first frame update
    void Start()
    {
        //InitializeCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        if(_currentGame == this)
        {
            IUpdateExperience();
        }
    }

    private void OnEnable()
    {

    }

    void InitializeCanvas()
    {
        if(InhalerManagerScript.GetInstance() != null)
        {
            int _count = InhalerManagerScript.GetInstance().GetBlocks().Count;

            for(int _i = 0; _i < _count; _i++)
            {
                InhalerInformationClass _currentInfo = InhalerManagerScript.GetInstance().GetInhalerInfoList()[_i];

                if(!_currentInfo.GetObjectIncludedInMatchingGame())
                {
                    Debug.Log("The object " + @"""" + _currentInfo.GetObjectName() + @"""" + " is not included for the game.");

                    continue;
                }

                if(_currentInfo.GetInhalerMatchingObjectScript() == null || _currentInfo.GetHole() == null)
                {
                    Debug.LogError("There is not block or hole to include for " + @"""" + _currentInfo.GetObjectName() + @"""" + ".");

                    continue;
                }

                InhalerMatchingBlockAndHoleClass _newClass = new InhalerMatchingBlockAndHoleClass(_currentInfo.GetInhalerMatchingObjectScript(), _currentInfo.GetHole(), _currentInfo.GetObjectName());

                _presetBlocksAndHoles.Add(_newClass);
            }
        }

        gameObject.SetActive(false);
    }

    public override void TryToStartGame()
    {
        try
        {
            IStartExperience();
        }
        catch(Exception e)
        {
            Debug.LogError("There is an error in the inhaler game!");
        }
    }

    public override void IStartExperience()
    {
        if(_gameSpace == null || InhalerManagerScript.GetInstance() == null || _currentGame != null || _floor == null || _spawningArea == null || _camera == null || _spawningSizeForBlocks <= 0.0f || _spawningSizeForHoles <= 0.0f)
        {
            Debug.LogError("Game cannot be loaded.");

            return;
        }

        if(_presetBlocksAndHoles.Count != InhalerManagerScript.GetInstance().GetInhalerInfoList().Count)
        {
            Debug.LogError("There is a mismatch.");

            return;
        }

        base.IStartExperience();

        _mainCanvases.SetCanvasesOn(false);

        _floor.SetActive(true);

        _walls.SetActive(true);

        List<Vector3> _selectedPos = new List<Vector3>();

        _currentlySelectedPositionForHoles = Vector3.zero;

        string _currentDialogue;

        for(int _i = 0; _i < _presetBlocksAndHoles.Count; _i++)
        {
            //1. Instantiating Block

            InhalerMatchingBlockAndHoleClass _currentPreset = _presetBlocksAndHoles[_i] as InhalerMatchingBlockAndHoleClass;

            if(_currentPreset == null)
            {
                Debug.LogError("This is not a valid type of preset.");

                continue;
            }

            if (_currentPreset.GetGameBlock() == null || _currentPreset.GetGameBlockHole() == null)
            {
                Debug.LogError("No block or hole to instantiate.");

                continue;
            }

            string _givenName = InhalerManagerScript.GetInstance().GetInhalerInfoList()[_i].GetObjectName();

            GameObject _newBlock = Instantiate(_currentPreset.GetGameBlock().gameObject);

            _newBlock.transform.parent = _spawningArea.transform;

            base.FindSpawningSpot(ref _selectedPos, ref _newBlock);

            _newBlock.transform.localScale = (Vector3.one * _spawningSizeForBlocks);

            _newBlock.GetComponent<InhalerMatchingObjectScript>().SetMatchingGameCanvas(this);

            _newBlock.GetComponent<InhalerMatchingObjectScript>().SetObjectName(_givenName);

            _newBlock.GetComponent<InhalerMatchingObjectScript>().GetDraggableProperties().SetOffset_Z(_draggingZOffset);

            _gameProperties.AddObjectToList(_newBlock.GetComponent<InhalerMatchingObjectScript>());

            _gameProperties.AddObjectsAsGO(_newBlock);

            _newBlock.GetComponent<DraggableClass>().SetCamera(_camera);

            _newBlock.GetComponent<DraggableClass>().GetBody().velocity = Vector3.zero;

            _newBlock.transform.parent = _mainContainer;

            GiveRotationProperties(_newBlock);

            //2. Instantiating Hole

            GameObject _newHole = Instantiate(_currentPreset.GetGameBlockHole().gameObject);

            _currentDialogue = _currentPreset.GetDialogueOfCurrentIndex();

            Debug.Log("The current dialogue is " + @"""" + _currentDialogue + @"""" + ".");

            _newHole.transform.parent = _gameSpace.transform;

            _newHole.transform.localPosition = _currentlySelectedPositionForHoles;

            _newHole.transform.localScale = (Vector3.one * _spawningSizeForHoles);

            _currentHoleProperties.AddObjectsAsGO(_newHole);

            _currentlySelectedPositionForHoles.z = _currentlySelectedPositionForHoles.z + _addedDistanceForHoles;

            _newHole.transform.localRotation = Quaternion.Euler(_facingDirection);

            InhalerMatchingObjectHoleScript _holeInfo = _newHole.GetComponent<InhalerMatchingObjectHoleScript>();

            try
            {
                if (_holeInfo != null)
                {
                    InhalerInformationClass _currentInfo;

                    _holeInfo.SetHoleGameCanvas(this);

                    _currentInfo = InhalerManagerScript.GetInstance().GetInhalerInfoList()[_i];

                    _holeInfo.SetCamera(_camera);

                    _holeInfo.SetHoleName(_givenName);

                    _currentHoleProperties.AddObjectToList(_holeInfo);

                    _holeInfo.SetCorrectMatchDialogue(_currentDialogue);

                    Debug.Log("The name of the correct dialogue at index " + (_i + 1).ToString() + " is " + @"""" + _holeInfo.GetCorrectMatchDialogue() + @"""" + ".");

                    if (_dialogues != null)
                    {
                        _holeInfo.SetDialogues(_dialogues);
                    }

                    //if (_currentPreset.GetGameBlockHole().GetCorrectMatchDialogue() != "")
                    //{
                        //_holeInfo.SetCorrectMatchDialogue(_currentPreset.GetGameBlockHole().GetCorrectMatchDialogue());
                    //}

                    if (_holeInfo.GetNameText() != null && _currentInfo != null)
                    {
                        _holeInfo.GetNameText().text = _currentInfo.GetObjectName();

                        Color _c = _currentInfo.GetColorWitrhDifferentShadeOrAlpha(2.0f, 0.5f);

                        _holeInfo.GetTextOutline().effectColor = _c;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("The error occurs at index '" + _i + "'.");
            }
        }

        AlignHolePositions();

        _completionMeter.SetMaxValue(_gameProperties.GetListOfObjects().Count);
        
        _completionMeter.SignalToUpdateUI();

        SetBadge("Inhaler Matching Badge");

        if(_currentRotation != null)
        {
            _currentRotation.GetComponent<RotationScript>().SetDoAction(true);
        }

        _indicatorCoroutine = StartCoroutine(SetNewTargetForIndicator2());
    }

    public override void IUpdateExperience()
    {
        _completionMeter.UpdateUI();

        _spawningRangesForBlocks.MaintainValues();

        base.LookIntoCamera();

        RotateHoles();

        if (_completionMeter.GetPercentage() == 100.0f && !_waitToCompleteSignal)
        {
            StartCoroutine(IWaitUntilCompletion());

            _waitToCompleteSignal = true;
        }
        else if (_completionMeter.GetPercentage() < 100.0f)
        {
            UpdateTargetForIndicator2();
        }
    }

    public  override void ICompleteExperience()
    {
        base.ICompleteExperience();

        _gameSpace.transform.localRotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
    }

    public override void IChooseToQuitExperience()
    {
        _yesOrNoCanvas.GetYesButton().onClick.AddListener(delegate { IStopExperience(); });

        _yesOrNoCanvas.GetNoButton().onClick.AddListener(delegate { base.IResumeExperience(); });

        base.IChooseToQuitExperience();
    }

    public override void IStopExperience()
    {
        base.IStopExperience();

        _addedSpace = 0;

        _floor.SetActive(false);

        _walls.SetActive(false);

        _gameProperties.ClearObjectLists();

        _currentHoleProperties.ClearObjectLists();

        if(_currentGame == this)
        {
            if (_currentRotation != null)
            {
                _currentRotation.GetComponent<RotationScript>().SetDoAction(false);
            }

            _currentGame = null;
        }
    }

    void GiveRotationProperties(GameObject _input)
    {
        if(_input == null)
        {
            return;
        }

        RotationScript _rotationProperties;

        if(_input.GetComponent<RotationScript>() == null)
        {
            _rotationProperties = _input.AddComponent<RotationScript>();
        }
        else
        {
            _rotationProperties = _input.GetComponent<RotationScript>();
        }

        _rotationProperties.SetDoAction(false);

        _rotationProperties.SetDeltaTime(true);

        _rotationProperties.SetAxis(Vector3.up);

        _rotationProperties.SetConstant(100.0f);
    }

    public override IEnumerator IWaitUntilCompletion()
    {
        if (_dialogues == null)
        {
            Debug.LogError("There is no dialogue script assigned.");

            yield break;
        }

        if (_dialogues.GetAudioSource() == null)
        {
            Debug.LogError("There is no audio source assigned.");

            yield break;
        }

        AudioClip _c = _dialogues.GetAudioSource().clip;

        if (_c == null)
        {
            Debug.LogError("There is no audio clip being played.");

            yield break;
        }

        float _seconds = _c.length;

        Debug.Log("We are waiting for the dialogue to be complete before the conclusion...");

        yield return new WaitForSeconds(_seconds);

        _dialogues.GetAudioSource().Stop();

        yield return new WaitForSeconds(2.0f);

        Debug.Log("YAY! We are now concluding the game!");

        ICompleteExperience();
    }

    public override void IGameCorrect()
    {
        base.IGameCorrect();
    }

    public override void IGameIncorrect()
    {
        base.IGameIncorrect();

        //_fastyDelayCoroutine = StartCoroutine("Matching Game Incorrect", _fastyDialogueDelay);
        _dialogues.PlayClip("Matching Game Incorrect");
    }

    protected override IEnumerator SetNewTargetForIndicator2()
    {
        if (_gameIndicatorCanvas == null)
        {
            yield break;
        }

        if (!PrepareGameIndicator())
        {
            yield break;
        }

        if (_completionMeter.GetPercentage() == 100.0f)
        {
            _gameIndicatorCanvasCountdownStarted = false;

            _gameIndicatorCanvas.gameObject.SetActive(false);

            yield break;
        }

        _gameIndicatorCanvas.gameObject.SetActive(false);

        _gameIndicatorCanvasCountdownStarted = true;

        yield return new WaitForSeconds(10.0f);

        if (_completionMeter.GetPercentage() == 100.0f)
        {
            _gameIndicatorCanvasCountdownStarted = false;

            _gameIndicatorCanvas.gameObject.SetActive(false);

            yield break;
        }

        _gameIndicatorCanvasCountdownStarted = false;

        if (_currentBlock != null)
        {
            List<MatchingGameHoleScript> _holes = new List<MatchingGameHoleScript>();

            MatchingGameHoleScript _currentHole = null;

            InhalerMatchingObjectHoleScript _currentInhalerHole = null;

            InhalerMatchingObjectScript _currentInhalerBlock = (InhalerMatchingObjectScript)(_currentBlock);

            if (_currentInhalerBlock == null)
            {
                yield break;
            }

            string _value = _currentInhalerBlock.GetObjectName();

            bool _found = false;

            for (int _i = 0; _i < _currentHoleProperties.GetListOfObjects().Count && !_found; _i++)
            {
                _currentHole = _currentHoleProperties.GetListOfObjects()[_i];

                if (_currentHole == null)
                {
                    continue;
                }

                if (_currentHole.GetObjectPlaced())
                {
                    continue;
                }

                _currentInhalerHole = (InhalerMatchingObjectHoleScript)(_currentHole);

                if (_currentInhalerHole == null)
                {
                    continue;
                }

                if (_value == _currentInhalerHole.GetHoleName())
                {
                    _gameIndicatorCanvas.SetTargetObject(_currentInhalerHole.gameObject);

                    _gameIndicatorCanvas.gameObject.SetActive(true);

                    _found = true;
                }
            }
        }
        else
        {
            MatchingGameBlockScript _selectedBlock = null;

            InhalerMatchingObjectScript _selectedInhalerBlock = null;

            int _index = -1;

            while (_selectedBlock == null)
            {
                _index = UnityEngine.Random.Range(0, _gameProperties.GetListOfObjects().Count);

                if (_gameProperties.GetListOfObjects()[_index] != null)
                {
                    if (!_gameProperties.GetListOfObjects()[_index].GetBlockPlaced())
                    {
                        _selectedBlock = _gameProperties.GetListOfObjects()[_index];
                    }
                }
            }

            if ((InhalerMatchingObjectScript)(_selectedBlock) != null)
            {
                _selectedInhalerBlock = (InhalerMatchingObjectScript)(_selectedBlock);

                _gameIndicatorCanvas.SetTargetObject(_selectedInhalerBlock.gameObject);

                _gameIndicatorCanvas.gameObject.SetActive(true);
            }
        }
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

}
