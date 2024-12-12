using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MCQCanvasScript : GameCanvasScript
{
    [SerializeField]
    Button[] _answerButtons = new Button[4];

    [SerializeField]
    Canvas _gameCanvas;

    [SerializeField]
    Button _nextButton;

    [SerializeField]
    Text _questionTextNumberField;

    [SerializeField]
    Text _questionTextField;

    [SerializeField]
    MCQGameScript _game;

    [SerializeField]
    Slider _progressionSlider;

    Coroutine _responseTextCoroutine;

    string _correctAnswerResponse;

    //[SerializeField]
    Button _correctButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Slider GetProgressionSlider()
    {
        return _progressionSlider;
    }

    public void SetQuestionNumber(int _input)
    {
        if(_questionTextNumberField == null)
        {
            return;
        }

        _questionTextNumberField.text = "Question " + _input.ToString();
    }

    public void SetQuestion(QuestionClass _input)
    {
        if(_questionTextField == null)
        {
            return;
        }

        _questionTextField.text = _input.GetQuestionText();
    }

    public void PrepareQuestion(QuestionClass _input)
    {
        for(int _i = 0; _i < 4; _i++)
        {
            _answerButtons[_i].onClick.RemoveAllListeners();
        }

        _correctButton = null;

        StopDisplayResponseCoroutine();

        for(int _i = 0; _i < 4; _i++)
        {
            _answerButtons[_i].interactable = true;

            _answerButtons[_i].gameObject.GetComponent<RectTransform>().Find("Text").gameObject.GetComponent<Text>().text = _input.GetAnswers()[_i];

            if(_input.GetCorrectAnswerIndex() == _i)
            {
                _correctButton = _answerButtons[_i];
            }
        }

        _correctAnswerResponse = _input.GetCorrectAnswerResponse();

        for(int _i = 0; _i < 4; _i++)
        {
            if(_correctButton == _answerButtons[_i])
            {
                _answerButtons[_i].onClick.AddListener(delegate { CorrectAnswer(); });
            }
            else
            {
                _answerButtons[_i].onClick.AddListener(delegate { IncorrectAnswer(); });
            }
        }

        _nextButton.gameObject.SetActive(false);
    }

    void IncorrectAnswer()
    {
        if(_game == null)
        {
            return;
        }

        if(_game.GetResponseText() == null)
        {
            return;
        }

        StopDisplayResponseCoroutine();

        _responseTextCoroutine = StartCoroutine(DisplayResponse("This is not the correct answer. Please, try again. I know you can do it!", 5.0f, new Color(0.5f, 0.5f, 0.5f, 1.0f)));

        if (_game.GetAudioSource() != null && _game.GetIncorrectAudioClip() != null)
        {
            _game.GetAudioSource().clip = _game.GetIncorrectAudioClip();

            _game.GetAudioSource().Play();
        }

        if(_game.GetDialogues() != null)
        {
            string _dialogueS = _game.SelectRandomDialogue(false);
            
            _game.GetDialogues().PlayClip(_dialogueS);
        }
    }

    void CorrectAnswer()
    {
        if(_game == null)
        {
            return;
        }

        if(_game.GetResponseText() == null)
        {
            return;
        }

        if(_game.GetAudioSource() != null && _game.GetCorrectAudioClip() != null)
        {
            _game.GetAudioSource().clip = _game.GetCorrectAudioClip();

            _game.GetAudioSource().Play();
        }

        if(_game.GetDialogues() != null)
        {
            _game.GetDialogues().PlayClip(_game.GetCurrentQuestion().GetDialogueClipName());
        }

        StopDisplayResponseCoroutine();

        for(int _i = 0; _i < 4; _i++)
        {
            _answerButtons[_i].interactable = false;
        }

        if (_game.GetDialogues() != null)
        {
            string _dialogueS = _game.SelectRandomDialogue();

            //List<string> _dialoguesStrings = new List<string>() { _dialogueS, _game.GetCurrentQuestion().GetDialogueClipName(), "Press Next"};

            //_game.GetDialogues().PlayClips(_dialoguesStrings);

            _game.GetDialogues().PlayClip(_dialogueS, _game.GetCurrentQuestion().GetDialogueClipName(), "Press next");
        }

        string _response = "That's correct! Well done! ";

        _response = _response + _correctAnswerResponse + "Press on " + @"""" + "Next" + @"""" + " to continue.";

        _responseTextCoroutine = StartCoroutine(DisplayResponse(_response, 5.0f, new Color(0.0f, 1.0f, 0.0f, 1.0f)));

        _nextButton.interactable = true;

        _game.GetCompletionMeter().AddToValue(1);

        _game.GetCompletionMeter().SignalToUpdateUI();

        _nextButton.gameObject.SetActive(true);
    }

    IEnumerator DisplayResponse(string _textInput, float _timeInput, Color _textColorInput)
    {
        //_game.GetResponseText().text = _textInput;

        //_game.GetResponseText().color = _textColorInput;

        //_game.GetResponseText().gameObject.GetComponent<Outline>().effectColor = ToolsStruct.ChangeColorValue(_textColorInput, 0.5f, 0.5f);

        //_game.SetResponseText(_textInput, _textColorInput, new Vector2(1.0f, -1.0f));

        yield return new WaitForSeconds(_timeInput);

        _game.GetResponseText().text = "";
    }

    public void StopDisplayResponseCoroutine()
    {
        if(_responseTextCoroutine == null)
        {
            return;
        }

        StopCoroutine(_responseTextCoroutine);

        if(_game == null)
        {
            return;
        }

        if(_game.GetResponseText() == null)
        {
            return;
        }

        _game.GetResponseText().text = "";
    }

    Text GetButtonText(int _input)
    {
        if (!(_input >= 0 && _input < _answerButtons.Length))
        {
            return null;
        }

        Text _tx = _answerButtons[_input].gameObject.GetComponent<RectTransform>().Find("Text").gameObject.GetComponent<Text>();

        return _tx;
    }
}
