using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InhalerMatchingObjectHoleScript : MatchingGameHoleScript
{
    [Header("2. Variables for Inhaler Matching Block Holes Only")]

    [SerializeField]
    Canvas _textCanvas;

    [SerializeField]
    Text _nameText;

    [SerializeField]
    Outline _textOutline;

    [SerializeField]
    string _holeName;

    InhalerMatchingObjectScript _matchingObject;

    public InhalerMatchingObjectHoleScript():base()
    {
        _draggableTypeNeeded = DraggableTypeEnum.Inhaler_Object;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(_playerController == null)
        {
            _playerController = new PlayerController();

            _playerController.Enable();
        }

        //_currentSizeValue = _defaultSizeValue;
    }

    private void OnEnable()
    {
        if(_playerController == null)
        {
            _playerController = new PlayerController();

            _playerController.Enable();
        }
    }

    // Update is called once per frame
    void Update()
    {
        SeeObject();

        if(!_objectPlaced)
        {
            EvaluateHole();
        }
    }

    public Canvas GetTextCanvas()
    {
        return _textCanvas;
    }

    public Text GetNameText()
    {
        return _nameText;
    }

    public Outline GetTextOutline()
    {
        return _textOutline;
    }

    public InhalerMatchingObjectScript GetMatchingObject()
    {
        return _matchingObject;
    }

    public string GetHoleName()
    {
        return _holeName;
    }

    public void SetHoleName(string _input)
    {
        _holeName = _input;
    }

    protected override void SeeObject()
    {
        if(!CheckBasicNeeds())
        {
            ResetValues();

            return;
        }

        if(_errorIdentifier.DisplayHere())
        {
            _errorIdentifier.DisplayTextByIndex(-1);
        }

        CheckHoveringStatus();
    }

    protected override bool CheckMatch()
    {
        _matchingObject = _currentObject as InhalerMatchingObjectScript;

        if(_matchingObject == null)
        {
            return false;
        }

        bool _bool = string.Compare(_holeName, _matchingObject.GetObjectName(), true) == 0;

        if(_errorIdentifier.DisplayHere())
        {
            if(_bool)
            {
                _errorIdentifier._textField.color = Color.green;

                _errorIdentifier._textField.text = "It is a match!";
            }
            else
            {
                _errorIdentifier._textField.color = Color.red;
            }
        }

        return _bool;
    }

    protected override void EvaluateHole()
    {
        if (!_canEvaluate || _objectPlaced || _currentObject == null)
        {
            if(_errorIdentifier.DisplayHere())
            {
               _errorIdentifier.DisplayTextByIndex(3);
            }

            return;
        }

        if (_currentObject.GetDraggableProperties().GetTouchPhase() != TouchPhase.Ended)
        {
            if(_errorIdentifier.DisplayHere())
            {
              _errorIdentifier.DisplayTextByIndex(4);
            }
          return;
        }

        //if(_errorIdentifier.DisplayHere())
        //{
        //   _errorIdentifier.DisplayTextByIndex(-1);
        // }

        bool _matchingBool = CheckMatch();

        if(_matchingBool)
        {
            ConfirmMatch();
        }
        else if(_audioSource != null && _holeCanvas.GetIncorrectAudioClip() != null)
        {
            _audioSource.clip = _holeCanvas.GetIncorrectAudioClip();
            
            _audioSource.Play();
            
            Debug.Log("We are playing the audio for the incorrect reponse for hole " + @"""" + gameObject.name + @"""" + ".");         
        }

        ResetValues();
    }

    protected override void ConfirmMatch()
    {
        if(_currentObject.GetComponent<RotationScript>() != null)
        {
            _currentObject.GetComponent<RotationScript>().SetDoAction(true);
        }

        base.ConfirmMatch();

        if(_textCanvas != null)
        {
            _textCanvas.gameObject.SetActive(false);
        }
    }

    IEnumerator DisplayResponse()
    {
        _holeCanvas.SetResponseText("That's not a match. Please try again. I know you can do it.", Color.red, new Vector2(1.0f, -1.0f), new Color(0.5f, 0.0f, 0.0f, 0.5f));

        yield return new WaitForSeconds(5.0f);

        _holeCanvas.ClearResponseText();
    }
}
