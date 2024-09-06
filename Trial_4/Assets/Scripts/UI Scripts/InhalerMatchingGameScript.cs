using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InhalerMatchingGameScript : MatchingGameCanvasScript
{
    [ContextMenu("Add To Preset List...")]
    protected override void AddToBnHList()
    {
        _presetBlocksAndHoles.Add(new InhalerMatchingBlockAndHoleClass());
    }


    public Text _errorText;

    // Start is called before the first frame update
    void Start()
    {
        //InitializeCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        _gameProperties.UpdateUI();

        _minMaxV3Values.MaintainValues();

        _completionMeter = _gameProperties.GetMeter();

        if(_gameProperties.GetMeter().GetPercentage() == 100.0f && !_gameDone)
        {
            WinGame();
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
            StartGame();
        }
        catch(Exception e)
        {
            Debug.LogError("There is an error in the inhaler game!");
        }
    }

    public override void StartGame()
    {
        if(_gameSpace == null || InhalerManagerScript.GetInstance() == null || _currentGame != null || _floor == null || _spawningArea == null || _gameProperties.GetCamera() == null || _spawningSizeForBlocks <= 0.0f || _spawningSizeForHoles <= 0.0f)
        {
            Debug.LogError("Game cannot be loaded.");

            return;
        }

        if(_presetBlocksAndHoles.Count != InhalerManagerScript.GetInstance().GetInhalerInfoList().Count)
        {
            Debug.LogError("There is a mismatch.");

            return;
        }

        base.StartGame();

        _gameProperties.GetMainCanvases().SetCanvasesOn(false);

        _floor.SetActive(true);

        List<Vector3> _selectedPos = new List<Vector3>();

        _currentlySelectedPositionForHoles = Vector3.zero;

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

            _gameProperties.AddObjectToList(_newBlock.GetComponent<InhalerMatchingObjectScript>());

            _gameProperties.AddObjectsAsGO(_newBlock);

            _currentBlocksAndHoles.AddBlock(_newBlock.GetComponent<InhalerMatchingObjectScript>());

            _newBlock.GetComponent<DraggableClass>().SetCamera(_gameProperties.GetCamera());

            _newBlock.GetComponent<DraggableClass>().GetBody().velocity = Vector3.zero;

            _newBlock.transform.parent = _mainContainer;

            //2. Instantiating Hole

            GameObject _newHole = Instantiate(_currentPreset.GetGameBlockHole().gameObject);

            _newHole.transform.parent = _gameSpace.transform;

            _newHole.transform.localPosition = _currentlySelectedPositionForHoles;

            _newHole.transform.localScale = (Vector3.one * _spawningSizeForHoles);

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

                    _holeInfo.SetCamera(_gameProperties.GetCamera());

                    _holeInfo.SetHoleName(_givenName);

                    if (_holeInfo.GetNameText() != null && _currentInfo != null)
                    {
                        _holeInfo.GetNameText().text = _currentInfo.GetObjectName();

                        Color _c = _currentInfo.GetColorWitrhDifferentShadeOrAlpha(2.0f, 0.5f);

                        _holeInfo.GetTextOutline().effectColor = _c;
                    }

                    _currentBlocksAndHoles.AddHole(_holeInfo);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("The error occurs at index '" + _i + "'.");
            }
        }

        AlignHolePositions();

        if(_reverseDirectionForHoles)
        {
            _currentBlocksAndHoles.ReverseLists();
        }

        _gameProperties.GetMeter().SetMaxValue(_gameProperties.GetListOfObjects().Count);

        _gameProperties.SignalToUpdateUI();
    }

    protected override void WinGame()
    {
        base.WinGame();
    }

    public override void QuitGame()
    {
        //base.QuitGame();

        //_gameProperties.GetYesOrNoCanvas().gameObject.SetActive(true);

        //gameObject.SetActive(false);

        _gameProperties.GetYesOrNoCanvas().GetYesButton().onClick.AddListener(delegate { StopGame(); });

        //_gameProperties.GetYesOrNoCanvas().GetText().text = "Are you sure you want to quit the game?";

        base.QuitGame();
    }

    public override void StopGame()
    {
        base.StopGame();

        _addedSpace = 0;

        _floor.SetActive(false);

        _currentBlocksAndHoles.ClearLists();

        if(_currentGame == this)
        {
            _currentGame = null;
        }
    }
}
