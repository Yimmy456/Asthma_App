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

    Coroutine _responseTextCoroutine;

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

        for(int _i = 0; _i < 4; _i++)
        {
            _answerButtons[_i].interactable = true;

            _answerButtons[_i].gameObject.GetComponent<RectTransform>().Find("Text").gameObject.GetComponent<Text>().text = _input.GetAnswers()[_i];

            if(_input.GetCorrectAnswerIndex() == _i)
            {
                _correctButton = _answerButtons[_i];
            }
        }

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
    }

    void IncorrectAnswer()
    {
        if(_game == null)
        {
            return;
        }

        if(_game.GetGameProperties().GetResponseText() == null)
        {
            return;
        }

        if(_responseTextCoroutine != null)
        {
            StopCoroutine(_responseTextCoroutine);

            _game.GetGameProperties().GetResponseText().text = "";
        }

        _responseTextCoroutine = StartCoroutine(DisplayResponse("This is not the correct answer. Please, try again. I know you can do it!", 5.0f, new Color(0.5f, 0.5f, 0.5f, 1.0f), new Color(0.25f, 0.25f, 0.25f, 0.5f)));

    }

    void CorrectAnswer()
    {
        if(_game == null)
        {
            return;
        }

        if(_game.GetGameProperties().GetResponseText() == null)
        {
            return;
        }

        if(_responseTextCoroutine != null)
        {
            StopCoroutine(_responseTextCoroutine);

            _game.GetGameProperties().GetResponseText().text = "";
        }

        for(int _i = 0; _i < 4; _i++)
        {
            _answerButtons[_i].interactable = false;
        }

        _nextButton.interactable = true;

        _nextButton.gameObject.SetActive(true);
    }

    IEnumerator DisplayResponse(string _textInput, float _timeInput, Color _textColorInput, Color _outlineColorInput)
    {
        _game.GetGameProperties().GetResponseText().text = _textInput;

        _game.GetGameProperties().GetResponseText().color = _textColorInput;

        _game.GetGameProperties().GetResponseText().gameObject.GetComponent<Outline>().effectColor = _outlineColorInput;

        yield return new WaitForSeconds(_timeInput);

        _game.GetGameProperties().GetResponseText().text = "";
    }
}
