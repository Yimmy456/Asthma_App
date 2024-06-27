using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterHoleScript : MatchingGameHoleScript
{
    [SerializeField]
    char _letter;

    LetterBlockScript _matchingObject;

    /*
     Getters
    */

    public LetterHoleScript():base()
    {
        _draggableTypeNeeded = DraggableTypeEnum.Letter;
    }

    public char GetLetter()
    {
        return _letter;
    }

    /*
     Setters
    */

    public void SetLetter(char _input)
    {
        _letter = _input;
    }

    /*
     Other Functions     
    */

    private void Start()
    {
        _originalSize = gameObject.transform.localScale;

        if(_playerController == null)
        {
            _playerController = new PlayerController();

            _playerController.Enable();
        }

        _currentSizeValue = _defaultSizeValue;

        _animIncreaseOrDecrease = IncreasOrDecreaseEnum.None;
    }

    private void OnEnable()
    {
        _originalSize = gameObject.transform.localScale;

        if (_playerController == null)
        {
            _playerController = new PlayerController();

            _playerController.Enable();
        }

        _currentSizeValue = _defaultSizeValue;

        _animIncreaseOrDecrease = IncreasOrDecreaseEnum.None;
    }

    private void Update()
    {
        SeeObject();

        if(!_objectPlaced)
        {
            EvaluateHole();
        }
    }

    protected override void SeeObject()
    {
        if(!CheckBasicNeeds())
        {
            return;
        }

        CheckHoveringStatus();
    }

    protected override bool CheckMatch()
    {
        _matchingObject = _currentObject as LetterBlockScript;

        if(_matchingObject == null)
        {
            return false;
        }

        return ToolsStruct.CompareLetters(_matchingObject.GetLetter(), _letter, true);
    }

    protected override void EvaluateHole()
    {
        if(_currentObject == null || _objectPlaced || !_canEvaluate)
        {
            return;
        }

        if(_currentObject.GetDraggableProperties().GetTouchPhase() != TouchPhase.Ended)
        {
            return;
        }

        bool _matchingBool = CheckMatch();

        if(_matchingBool)
        {
            ConfirmMatch();
        }

        ResetValues();
    }

    protected override void ConfirmMatch()
    {
        _currentObject.GetRotationProperties().SetDoAction(true);

        base.ConfirmMatch();
    }
}
