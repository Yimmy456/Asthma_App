using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPlanManagerScript : MonoBehaviour, IDataPersistenceInterface
{
    static ActionPlanManagerScript _instance;

    [SerializeField]
    ActionPlanUIScript _actionPlanCanvas;

    [SerializeReference] List<ActionPlanQuestionScript> _questionList = new List<ActionPlanQuestionScript>();

    [SerializeField]
    EnumActionPlanLabelStylesClass<InhalerColorEnum> _stylesOfInhalerColorLabels;

    [SerializeField]
    EnumActionPlanLabelStylesClass<GenderEnum> _stylesOfGenderLabels;

    ActionPlanQuestionScript _currentQuestion = null;

    int _currentQuestionIndex = -1;

    [ContextMenu("Give IDs to Questions")]
    void GiveIDs()
    {
        foreach (var question in _questionList)
        {
            if (question.GetQuestionID().Equals(""))
            {
                string _text = System.Guid.NewGuid().ToString();

                question.SetQuestionID(_text);
            }
        }
    }

    [ContextMenu("Add Integer Question")]
    void AddIntegerQuestion()
    {
        _questionList.Add(new ActionPlanQuestionInteger());
    }

    [ContextMenu("Add Decimal Question")]
    void AddDecimalQuestion()
    {
        _questionList.Add(new ActionPlanQuestionDecimal());
    }

    [ContextMenu("Add Text Question")]
    void AddTextQuestion()
    {
        _questionList.Add(new ActionPlanQuestionText());
    }
    [ContextMenu("Add Enum Question/Inhaler Color")]
    void AddInhalerColorQuestion()
    {
        _questionList.Add(new ActionPlanQuestionEnum_InhalerColor());
    }

    [ContextMenu("Add Enum Question/Gender")]
    void AddGenderQuestion()
    {
        _questionList.Add(new ActionPlanQuestionEnum_Gender());
    }

    [ContextMenu("Add Date Question")]
    void AddDateQuestion()
    {
        //_questionList.Add(new ActionPlanQuestionDate());

        _questionList.Add(new ActionPlanQuestionDate());
    }

    [ContextMenu("Add Contact Question")]
    void AddContactQuestion()
    {
        _questionList.Add(new ActionPlanQuestionContact());
    }


    // Start is called before the first frame update
    void Start()
    {
        //PrepareVariableTypes();

        //LoadData();

        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        foreach (ActionPlanQuestionScript _q in _questionList)
        {
            StartCoroutine(_q.CheckPersistence());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static ActionPlanManagerScript GetInstance()
    {
        return _instance;
    }

    public List<ActionPlanQuestionScript> GetQuestionList()
    {
        return _questionList;
    }

    public ActionPlanUIScript GetActionPlanUI()
    {
        return _actionPlanCanvas;
    }

    public int GetCurrentQuestionIndex()
    {
        return _currentQuestionIndex;
    }

    public ActionPlanQuestionScript GetCurrentQuestion()
    {
        return _currentQuestion;
    }

    public bool GetAllQuestionsAnswered()
    {
        for(int _i = 0; _i < _questionList.Count; _i++)
        {
            if (!_questionList[_i].GetQuestionAnswered())
            {
                return false;
            }
        }
        return true;
    }

    public EnumActionPlanLabelStylesClass<InhalerColorEnum> GetStylesOfInhalerColorLabels()
    {
        return _stylesOfInhalerColorLabels;
    }

    public EnumActionPlanLabelStylesClass<GenderEnum> GetStylesOfGenderLabels()
    {
        return _stylesOfGenderLabels;
    }

    public void SetActionPlanUI(ActionPlanUIScript _input)
    {
        _actionPlanCanvas = _input;
    }

    public void FinishPlan()
    {
        _currentQuestionIndex = -1;

        _actionPlanCanvas.gameObject.SetActive(false);
    }

    public void SaveResults()
    {
        if(DataPersistenceManager.GetInstance() == null)
        {
            FinishPlan();
            return;
        }

        if(DataPersistenceManager.GetInstance().GetGameData() == null)
        {
            FinishPlan();
            return;
        }

        GameDataScript _d = DataPersistenceManager.GetInstance().GetGameData();

        foreach(ActionPlanQuestionScript _q in _questionList)
        {
            _q.SaveData(ref _d);
        }

        DataPersistenceManager.GetInstance().SaveGame();

        FinishPlan();
    }

    public void StartPlan()
    {
        if(_actionPlanCanvas == null || _questionList.Count == 0)
        {
            return;
        }

        _currentQuestionIndex = 0;

        _actionPlanCanvas.SetTotalNumberOfQuestions(_questionList.Count);

        _actionPlanCanvas.GetPreviousButton().gameObject.SetActive(false);

        _actionPlanCanvas.GetQuitButton().GetComponent<RectTransform>().anchoredPosition = new Vector2(100.0f, 100.0f);
        
        _actionPlanCanvas.GetNextButton().gameObject.SetActive(_questionList.Count > 1);

        _actionPlanCanvas.GetSubmitButton().gameObject.SetActive(_questionList.Count == 1);

        SetUI();
    }

    void SetUI()
    {
        _currentQuestion = _questionList[_currentQuestionIndex];

        _actionPlanCanvas.SetQuestionNumber(_currentQuestionIndex);

        _actionPlanCanvas.SetQuestionText(_currentQuestion.GetQuestionText());

        _actionPlanCanvas.SetCorrectInputUI(_currentQuestion);

        _actionPlanCanvas.LoadAnswer(_currentQuestion);

        if(_actionPlanCanvas.GetQuestionsSlider() != null)
        {
            _actionPlanCanvas.GetQuestionsSlider().value = _currentQuestionIndex + 1;
        }
    }

    public void GoToPreviousQuestion()
    {
        if(_actionPlanCanvas == null || _currentQuestionIndex == 0)
        {
            return;
        }

        _actionPlanCanvas.RegisterAnswer(_currentQuestion);

        _currentQuestionIndex--;

        _actionPlanCanvas.GetPreviousButton().gameObject.SetActive(_currentQuestionIndex > 0);

        float _posY = (_currentQuestionIndex == 0) ? 100.0f : 350.0f;

        _actionPlanCanvas.GetQuitButton().GetComponent<RectTransform>().anchoredPosition = new Vector2(100.0f, _posY);

        _actionPlanCanvas.GetNextButton().gameObject.SetActive(_currentQuestionIndex < (_questionList.Count - 1));

        _actionPlanCanvas.GetSubmitButton().gameObject.SetActive(_currentQuestionIndex == (_questionList.Count - 1));

        SetUI();
    }

    public void GoToNextQuestion()
    {
        if (_actionPlanCanvas == null || _currentQuestionIndex == (_questionList.Count - 1))
        {
            return;
        }

        _actionPlanCanvas.RegisterAnswer(_currentQuestion);

        _currentQuestionIndex++;

        _actionPlanCanvas.GetPreviousButton().gameObject.SetActive(_currentQuestionIndex > 0);

        _actionPlanCanvas.GetQuitButton().GetComponent<RectTransform>().anchoredPosition = new Vector2(100.0f, 350.0f);

        _actionPlanCanvas.GetNextButton().gameObject.SetActive(_currentQuestionIndex < (_questionList.Count - 1));

        _actionPlanCanvas.GetSubmitButton().gameObject.SetActive(_currentQuestionIndex == (_questionList.Count - 1));

        SetUI();
    }

    public void LoadData(GameDataScript _input)
    {
        foreach(ActionPlanQuestionScript _q in _questionList)
        {
            string _id2 = _q.GetQuestionID();

            string _value2;

            _input._actionPlanAnswers.TryGetValue(_id2, out _value2);

            SetAnswerInCorrectFormForLoading(_q, _value2);
        }
    }

    public void SaveData(ref GameDataScript _input)
    {
        foreach(ActionPlanQuestionScript _q in _questionList)
        {
            string _id = _q.GetQuestionID();

            if(!_input._actionPlanAnswers.ContainsKey(_id))
            {
                _input._actionPlanAnswers.Remove(_id);
            }

            string _value = GetAnswerInStringFormForSaving(_q);

            _input._actionPlanAnswers.Add(_id, _value);
        }
    }

    string GetAnswerInStringFormForSaving(ActionPlanQuestionScript _input)
    {
        string _finalAnswer = "";

        if(_input.GetVariableType() == VariableTypeForAPEnum.Integer || _input.GetVariableType() == VariableTypeForAPEnum.Enum)
        {
            _finalAnswer = _input.GetIntegerAnswer().ToString();
        }

        if(_input.GetVariableType() == VariableTypeForAPEnum.Decimal)
        {
            _finalAnswer = _input.GetDecimalAnswer().ToString("0.00");
        }
        if(_input.GetVariableType() == VariableTypeForAPEnum.Text)
        {
            _finalAnswer = _input.GetTextAnswer();
        }

        return _finalAnswer;
    }

    void SetAnswerInCorrectFormForLoading(ActionPlanQuestionScript _questionInput, string _answerTextInput)
    {
        if(_questionInput.GetVariableType() == VariableTypeForAPEnum.Integer || _questionInput.GetVariableType() == VariableTypeForAPEnum.Enum)
        {
            int _v = int.Parse(_answerTextInput);

            _questionInput.SetIntegerAnswer(_v);
        }

        if(_questionInput.GetVariableType() == VariableTypeForAPEnum.Decimal)
        {
            float _v2 = float.Parse(_answerTextInput);

            _questionInput.SetDecimalAnswer(_v2);
        }

        if(_questionInput.GetVariableType() == VariableTypeForAPEnum.Text)
        {
            _questionInput.SetTextAnswer(_answerTextInput);
        }
    }
}
