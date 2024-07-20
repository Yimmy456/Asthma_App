using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LetterGameScript : MatchingGameCanvasScript, YesOrNoInterface
{
    [ContextMenu("Add to Preset List...")]
    protected override void AddToBnHList()
    {
        _presetBlocksAndHoles.Add(new WordMatchingBlockAndHoleClass());
    }

    [Header("2. Variables for the Current Word.")]
    [SerializeField]
    DefinitionClass _currentWord;

    [SerializeField]
    Transform _currentRotation;

    LetterHoleScript _currentHole;

    private void Update()
    {
        if(_progressUpdated && !_progressUpdatedSwitchOn)
        {
            _progressUpdateCoroutine = StartCoroutine(ProgressUpdateCoroutineFunction());

            Debug.Log("Switch is triggered as on.");

            _progressUpdatedSwitchOn = true;
        }

        _completionMeter = _gameProperties.GetMeter();

        if(_gameProperties.GetMeter().GetPercentage() == 100.0f && !_gameDone)
        {
            WinGame();
        }

        _gameProperties.UpdateUI();
    }

    public LetterHoleScript GetCurrentHole()
    {
        return _currentHole;
    }

    public Transform GetCurrentRotation()
    {
        return _currentRotation;
    }
    //Calling the start of the game
    public override void StartGame()
    {
        if(GetGameInSession() || BookScript.GetInstance() == null || _spawningArea == null || _gameSpace == null || _mainContainer == null || _gameProperties.GetCamera() == null)
        {
            AbortGame();

            return;
        }

        base.StartGame();

        DefinitionClass _word = BookScript.GetInstance().GetRandomDefinition();

        _floor.SetActive(true);

        CreateWord(_word);

        _gameProperties.GetMeter().SetMaxValue(_currentBlocksAndHoles.GetBlockGOs().Count);

        _gameProperties.GetMeter().SetValue(0);

        _gameProperties.SignalToUpdateUI();
    }

    public override void StartGame(int _input)
    {
        if(GetGameInSession() || BookScript.GetInstance() == null || _spawningArea == null || _gameSpace == null || _mainContainer == null || _gameProperties.GetCamera() == null)
        {
            AbortGame();

            return;
        }

        DefinitionClass _word2;

        if(_input == -1)
        {
            _word2 = BookScript.GetInstance().GetRandomDefinition();
        }
        else if(_input >= 0 && _input < BookScript.GetInstance().GetDefinitions().Count)
        {
            _word2 = BookScript.GetInstance().GetDefinitions()[_input];
        }
        else
        {
            AbortGame();

            return;
        }

        if (_word2 != null)
        {
            base.StartGame();

            _floor.SetActive(true);

            CreateWord(_word2);

            _gameProperties.GetMeter().SetMaxValue(_currentBlocksAndHoles.GetBlockGOs().Count);

            _gameProperties.GetMeter().SetValue(0);

            _gameProperties.SignalToUpdateUI();
        }
        else
        {
            AbortGame();
        }
    }

    public void SetCurrentHole(LetterHoleScript _input)
    {
        _currentHole = _input;
    }
    
    protected override string SetInfoText()
    {
        string _textS = "Well Done! You spelled a new word! It is '" + _currentWord.GetInformationName() + "'! " + _currentWord.GetInformationDescription() + ".";

        return _textS;
    }

    protected override void SetBadge(string _textInput, bool _caseSensitiveInput = false)
    {
        string _badgeName = "Spelling Badge (" + _textInput + ")";

        _gameProperties.SetBadge(_badgeName, _caseSensitiveInput);
    }

    public override void StopGame()
    {
        base.StopGame();

        _floor.SetActive(false);

        _currentBlocksAndHoles.ClearLists();

        _addedSpace = 0;

        if (_currentGame == this)
        {
            _currentWord = null;
        }
    }

    public override void QuitGame()
    {
        _gameProperties.GetYesOrNoCanvas().GetYesButton().onClick.AddListener(delegate { StopGame(); });

        base.QuitGame();
    }

    protected override void WinGame()
    {
        if (!_gameDone)
        {
            //StopGame();

            _gameDone = true;
        }

        _floor.SetActive(false);

        _gameProperties.GetInformationCanvas().gameObject.SetActive(true);

        _gameProperties.ClearObjectLists();

        _gameProperties.GetInformationCanvas().SetText(SetInfoText());

        Button _nextB = _gameProperties.GetInformationCanvas().GetNextButton();

        _nextB.onClick.AddListener(delegate { base.WinGame(); _gameProperties.GetInformationCanvas().gameObject.SetActive(false); _nextB.onClick.RemoveAllListeners(); });

        _gameProperties.GetGameCanvas().gameObject.SetActive(false);
    }

    void CreateWord(DefinitionClass _input)
    {
        _addedSpace = 0;

        bool _letterFound = false;

        string _word = _input.GetInformationName();

        int _count = _word.Length;

        char _currentLetter;

        WordMatchingBlockAndHoleClass _blockAndHole = null;

        LetterBlockScript _newLetterBlock;

        LetterHoleScript _newLetterHole;

        GameObject _newLetterBlockGO;

        GameObject _newLetterHoleGO;

        _currentlySelectedPositionForHoles.z = 0.0f;

        List<Vector3> _positions = new List<Vector3>();

        for(int _i = 0; _i < _count; _i++)
        {
            _currentLetter = _word.ElementAt(_i);

            if(_currentLetter == ' ')
            {
                _addedSpace++;

                _currentlySelectedPositionForHoles.z += _addedDistanceForHoles;

                continue;
            }

            //1. Acquiring the Correct Letter

            for(int _j = 0; _j < _presetBlocksAndHoles.Count && !_letterFound; _j++)
            {
                _blockAndHole = _presetBlocksAndHoles[_j] as WordMatchingBlockAndHoleClass;

                if(_blockAndHole == null)
                {
                    continue;
                }

                char _ma = _blockAndHole.GetMatchingAttribute();

                bool _match = ToolsStruct.CompareLetters(_currentLetter, _ma);

                if(_match)
                {
                    _letterFound = true;
                }
            }

            if(_blockAndHole == null)
            {
                continue;
            }

            //2. Instantiating Block

            _newLetterBlockGO = Instantiate(_blockAndHole.GetGameBlock().gameObject);

            _newLetterBlock = _newLetterBlockGO.GetComponent<LetterBlockScript>();

            _newLetterBlock.SetMatchingGameCanvas(this);

            _newLetterBlockGO.transform.localScale = (Vector3.one * _spawningSizeForBlocks);

            _newLetterBlockGO.transform.parent = _spawningArea;

            _currentBlocksAndHoles.AddBlock(_newLetterBlock);

            base.FindSpawningSpot(ref _positions, ref _newLetterBlockGO);

            _gameProperties.AddObjectsAsGO(_newLetterBlockGO);

            _gameProperties.AddObjectToList(_newLetterBlock);

            _newLetterBlock.GetDraggableProperties().SetCamera(_gameProperties.GetCamera());

            _newLetterBlock.GetDraggableProperties().GetBody().velocity = Vector3.zero;

            _newLetterBlockGO.transform.parent = _mainContainer;

            //3. Instaitiating Hole

            _newLetterHoleGO = Instantiate(_blockAndHole.GetGameBlockHole().gameObject);

            _newLetterHole = _newLetterHoleGO.GetComponent<LetterHoleScript>();

            _newLetterHole.SetHoleGameCanvas(this);

            _newLetterHole.SetCamera(_gameProperties.GetCamera());

            _newLetterHoleGO.transform.localScale = (Vector3.one * _spawningSizeForHoles);

            _currentBlocksAndHoles.AddHole(_newLetterHole);

            _newLetterHoleGO.transform.parent = _gameSpace.transform;

            _newLetterHoleGO.transform.localPosition = _currentlySelectedPositionForHoles;

            _currentlySelectedPositionForHoles.z = _currentlySelectedPositionForHoles.z + _addedDistanceForHoles;

            _newLetterHoleGO.transform.localEulerAngles = _facingDirection;

            _currentBlocksAndHoles.IncreaseTotalBlocksAndHoles();

            _letterFound = false;
        }

        AlignHolePositions();

        if(_currentBlocksAndHoles.GetReverseListsBoolean())
        {
            _currentBlocksAndHoles.ReverseLists();
        }
    }

    void AbortGame()
    {
        if(_currentGame == this)
        {
            _currentGame = null;
        }

        if(_currentGameInSession)
        {
            _currentGameInSession = false;
        }

        _gameProperties.GetMainCanvases().SetCanvasesOn(true);

        gameObject.SetActive(false);
    }
}
