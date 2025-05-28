using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LetterGameScript : MatchingGameCanvasScript
{
    [ContextMenu("Add to Preset List...")]
    protected override void AddToBnHList()
    {
        _presetBlocksAndHoles.Add(new WordMatchingBlockAndHoleClass());
    }

    [Header("3. Variables for the Current Word.")]
    [SerializeField]
    DefinitionClass _currentWord;

    private void Update()
    {
        if (_currentGame == this)
        {
            IUpdateExperience();
        }
    }


    //Calling the start of the game
    public override void IStartExperience()
    {
        if(GetGameInSession() || BookScript.GetInstance() == null || _spawningArea == null || _gameSpace == null || _mainContainer == null || _camera == null)
        {
            AbortGame();

            return;
        }

        base.IStartExperience();

        DefinitionClass _word = BookScript.GetInstance().GetRandomDefinition();

        _floor.SetActive(true);

        _walls.SetActive(true);

        CreateWord(_word);

        _completionMeter.SetMaxValue(_gameProperties.GetListOfObjects().Count);

        _completionMeter.SetValue(0);

        _completionMeter.SignalToUpdateUI();

        SetBadge(2);

        if(_indicatorCoroutine != null)
        {
            StopCoroutine(_indicatorCoroutine);

            _gameIndicatorCanvasCountdownStarted = false;
        }

        //_indicatorCoroutine = StartCoroutine(SetNewTargetForIndicator());
    }

    public override void IStartExperience(int _input)
    {
        if(GetGameInSession() || BookScript.GetInstance() == null || _spawningArea == null || _gameSpace == null || _mainContainer == null || _camera == null)
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
            base.IStartExperience();

            _floor.SetActive(true);

            _walls.SetActive(true);

            CreateWord(_word2);

            _completionMeter.SetMaxValue(_gameProperties.GetListOfObjects().Count);

            _completionMeter.SetValue(0);

            _completionMeter.SignalToUpdateUI();

            string _badgeName = "Spelling Badge (" + _word2.GetInformationName() + ")";

            if(_indicatorCoroutine != null)
            {
                StopCoroutine(_indicatorCoroutine);
            }

            TriggerStartNewTarget();

            //_indicatorCoroutine = StartCoroutine(SetNewTargetForIndicator());

            //string _badgeName = "Spelling Badge (" + _textInput + ")";

            //base.SetBadge(_badgeName, _caseSensitiveInput);

            SetBadge(_badgeName);
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

    public override void IStopExperience()
    {
        base.IStopExperience();

        _floor.SetActive(false);

        _walls.SetActive(false);

        _gameProperties.ClearObjectLists();

        _currentHoleProperties.ClearObjectLists();

        _addedSpace = 0;

        if (_currentGame == this)
        {
            _currentWord = null;
        }
    }

    public override void IChooseToQuitExperience()
    {
        _yesOrNoCanvas.GetYesButton().onClick.AddListener(delegate { IStopExperience(); });

        _yesOrNoCanvas.GetNoButton().onClick.AddListener(delegate { base.IResumeExperience(); });

        base.IChooseToQuitExperience();
    }

    public  override void ICompleteExperience()
    {
        //StartCoroutine(IWaitUntilCompletion());
        _floor.SetActive(false);

        _walls.SetActive(false);

        _informationCanvas.gameObject.SetActive(true);

        _gameProperties.ClearObjectLists();

        _currentHoleProperties.ClearObjectLists();

        _informationCanvas.SetText(SetInfoText());

        Button _nextB = _informationCanvas.GetNextButton();

        _gameSpace.transform.localRotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);

        _nextB.onClick.AddListener(delegate { base.ICompleteExperience(); _informationCanvas.gameObject.SetActive(false); _nextB.onClick.RemoveAllListeners(); });

        _dialogues.PlayClip("Dr. Salem Letter Matching Game Word 'Asthma' Winning");

        _gameCanvas.gameObject.SetActive(false);
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

        string _currentDialogue;

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

            _newLetterBlockGO.transform.parent = _spawningArea;

            _newLetterBlockGO.transform.localScale = ((Vector3.one * _spawningSizeForBlocks));            

            base.FindSpawningSpot(ref _positions, ref _newLetterBlockGO);

            _gameProperties.AddObjectsAsGO(_newLetterBlockGO);

            _gameProperties.AddObjectToList(_newLetterBlock);

            _newLetterBlock.GetDraggableProperties().SetCamera(_camera);

            _newLetterBlock.GetDraggableProperties().SetOffset_Z(_draggingZOffset);

            _newLetterBlock.GetDraggableProperties().GetBody().velocity = Vector3.zero;

            _newLetterBlockGO.transform.parent = _spawningArea.transform.parent;

            //3. Instaitiating Hole

            _newLetterHoleGO = Instantiate(_blockAndHole.GetGameBlockHole().gameObject);

            _currentDialogue = _blockAndHole.GetDialogueOfCurrentIndex();

            _newLetterHole = _newLetterHoleGO.GetComponent<LetterHoleScript>();

            _newLetterHole.SetCorrectMatchDialogue(_currentDialogue);

            _blockAndHole.IncrementIndex();

            _newLetterHole.SetHoleGameCanvas(this);

            _newLetterHole.SetDialogues(_dialogues);

            _newLetterHole.SetCamera(_camera);

            _newLetterHoleGO.transform.parent = _gameSpace.transform;

            _newLetterHoleGO.transform.localScale = (Vector3.one * _spawningSizeForHoles);

            //UseAlternateDialogue(_i, _word, _newLetterHole);

            _newLetterHoleGO.transform.localPosition = _currentlySelectedPositionForHoles;

            _currentlySelectedPositionForHoles.z = _currentlySelectedPositionForHoles.z + _addedDistanceForHoles;

            _newLetterHoleGO.transform.localEulerAngles = _facingDirection;

            _currentHoleProperties.AddObjectToList(_newLetterHole);

            _currentHoleProperties.AddObjectsAsGO(_newLetterHoleGO);

            _letterFound = false;
        }

        AlignHolePositions();
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

        _mainCanvases.SetCanvasesOn(true);

        gameObject.SetActive(false);
    }

    void UseAlternateDialogue(int _indexInput, string _wordInput, LetterHoleScript _holeInput)
    {
        if(_holeInput == null)
        {
            return;
        }

        if(string.Compare(_wordInput, "Asthma", true) == 0)
        {
            if(_indexInput == 5)
            {
                _holeInput.SetCorrectMatchDialogue("Matching Letter Game 'A' 2");
            }
        }
    }

    public override void IUpdateExperience()
    {
        if (_progressUpdated && !_progressUpdatedSwitchOn)
        {
            _progressUpdateCoroutine = StartCoroutine(ProgressUpdateCoroutineFunction());

            Debug.Log("Switch is triggered as on.");

            _progressUpdatedSwitchOn = true;
        }

        //_completionMeter = _gameProperties.GetMeter();

        base.LookIntoCamera();

        //RotateSpaceFunction();

        _completionMeter.UpdateUI();

        /*if (_completionMeter.GetPercentage() == 100.0f && !_gameDone)
        {
            _gameDone = true;

            ICompleteExperience();
        }*/

        //UpdateGameIndicator();

        UpdateTargetForIndicator2();

        if(_completionMeter.GetPercentage() == 100.0f && !_waitToCompleteSignal)
        {
            StartCoroutine(IWaitUntilCompletion());

            _gameCanvas.GetQuitButton().gameObject.SetActive(false);

            _waitToCompleteSignal = true;
        }
    }

    public override IEnumerator IWaitUntilCompletion()
    {
        if(_dialogues == null)
        {
            Debug.LogError("There is no dialogue script assigned.");

            yield break;
        }

        if(_dialogues.GetAudioSource() == null)
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

        if(_indicatorCoroutine != null)
        {
            StopCoroutine(_indicatorCoroutine);

            _gameIndicatorCanvasCountdownStarted = false;
        }

        //_indicatorCoroutine = StartCoroutine(SetNewTargetForIndicator());
    }

    public override void IGameIncorrect()
    {
        base.IGameIncorrect();

        //_dialogues.PlayClip("Matching Game Incorrect");

        _fastyDelayCoroutine = StartCoroutine(PlayDialogueAfterDelay("Matching Game Incorrect", (_fastyDialogueDelay)));

        if (_indicatorCoroutine != null)
        {
            StopCoroutine(_indicatorCoroutine);

            _gameIndicatorCanvasCountdownStarted = false;
        }

        //_indicatorCoroutine = StartCoroutine(SetNewTargetForIndicator());
    }

    protected override IEnumerator SetNewTargetForIndicator2()
    {
        if(_gameIndicatorCanvas == null)
        {
            yield break;
        }

        if(!PrepareGameIndicator())
        {
            yield break;
        }

        if(_completionMeter.GetPercentage() == 100.0f)
        {
            _gameIndicatorCanvasCountdownStarted = false;

            _gameIndicatorCanvas.gameObject.SetActive(false);

            yield break;
        }

        _gameIndicatorCanvas.gameObject.SetActive(false);

        _gameIndicatorCanvasCountdownStarted = true;

        yield return new WaitForSeconds(10.0f);

        _gameIndicatorCanvasCountdownStarted = false;

        if(_currentBlock != null)
        {
            List<MatchingGameHoleScript> _holes = new List<MatchingGameHoleScript>();

            MatchingGameHoleScript _currentHole = null;

            LetterHoleScript _currentLetterHole = null;

            LetterBlockScript _letterBlock = (LetterBlockScript)(_currentBlock);

            if(_letterBlock == null)
            {
                yield break;
            }

            char _value = _letterBlock.GetLetter();

            for(int _i = 0; _i < _currentHoleProperties.GetListOfObjects().Count; _i++)
            {
                _currentHole = _currentHoleProperties.GetListOfObjects()[_i];

                if(_currentHole == null)
                {
                    continue;
                }

                if(_currentHole.GetObjectPlaced())
                {
                    continue;
                }

                _currentLetterHole = (LetterHoleScript)(_currentHole);

                if(_currentLetterHole == null)
                {
                    continue;
                }

                if(_value == _currentLetterHole.GetLetter())
                {
                    _holes.Add(_currentLetterHole);
                }
            }

            if(_holes.Count == 0)
            {
                yield break;
            }

            int _rand = Random.Range(0, _holes.Count);

            _gameIndicatorCanvas.SetTargetObject(_holes[_rand].gameObject);

            _gameIndicatorCanvas.gameObject.SetActive(true);
        }
        else
        {
            MatchingGameBlockScript _selectedBlock = null;

            LetterBlockScript _selectedLetterBlock = null;

            int _index = -1;

            while(_selectedBlock == null)
            {
                _index = Random.Range(0, _gameProperties.GetListOfObjects().Count);

                if (_gameProperties.GetListOfObjects()[_index] != null)
                {
                    if (!_gameProperties.GetListOfObjects()[_index].GetBlockPlaced())
                    {
                        _selectedBlock = _gameProperties.GetListOfObjects()[_index];
                    }
                }
            }

            if((LetterBlockScript)(_selectedBlock) != null)
            {
                _selectedLetterBlock = (LetterBlockScript)(_selectedBlock);

                _gameIndicatorCanvas.SetTargetObject(_selectedLetterBlock.gameObject);

                _gameIndicatorCanvas.gameObject.SetActive(true);
            }
        }
    }
}
