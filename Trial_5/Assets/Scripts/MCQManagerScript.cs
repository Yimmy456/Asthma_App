using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class MCQManagerScript : MonoBehaviour
{    
    static MCQManagerScript _instance;

    [SerializeField]
    List<QuestionClass> _questions;

    [SerializeField]
    List<QuestionClass> _selectedQuestions;

    [SerializeField]
    List<string> _questionsToAvoid;

    [ContextMenu("Give IDs to Questions")]
    void GiveIDs()
    {
        QuestionClass _currentQuestion;

        for(int _i = 0; _i < _questions.Count; _i++)
        {
            _currentQuestion = _questions[_i];

            if(_currentQuestion.GetQuestionID().Length != 32)
            {
                string _newID = System.Guid.NewGuid().ToString();

                _questions[_i].SetQuestionID(_newID);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static MCQManagerScript GetInstance()
    {
        return _instance;
    }

    public List<QuestionClass> GetQuestions()
    {
        return _questions;
    }

    public List<QuestionClass> GetQuestionsAnsweredCorrectly()
    {
        List<QuestionClass> _q = new List<QuestionClass>();

        foreach(QuestionClass _qu in _questions) { 
            if(_qu.GetQuestionAnsweredCorrectly())
            {
                _q.Add(_qu);
            }
        }

        return _q;
    }

    public int GetPercentageOfQuestionsAnsweredCorrectly()
    {
        int _total = _questions.Count;

        if(_total == 0)
        {
            return -1;
        }

        int _count = GetQuestionsAnsweredCorrectly().Count;

        float _ratio = _count / _total;

        float _percentage = _ratio * 100.0f;

        return (int)_percentage;
    }

    public List<QuestionClass> GetSelectedQuestions()
    {
        return _selectedQuestions;
    }

    public List<string> GetQuestionsToAvoid()
    {
        return _questionsToAvoid;
    }

    public void SetSelectedQuestions(List<QuestionClass> _input)
    {
        _selectedQuestions = _input;
    }

    public void ClearSelectedQuestions()
    {
        _selectedQuestions.Clear();
    }
}

[System.Serializable]
public class QuestionClass : IDataPersistenceInterface
{
    public void LoadData(GameDataScript _input)
    {

    }

    public void SaveData(ref GameDataScript _input)
    {

    }

    public QuestionClass()
    {

    }

    public QuestionClass(string _questionIDInput, string _questionTextInput, bool _answeredCorrectlyInput, string[] _answersInput, int _correctAnswerIndexInput)
    {
        _questionID = _questionIDInput;

        _questionText = _questionTextInput;

        _answeredCorrectly = _answeredCorrectlyInput;

        _answers = _answersInput;

        _correctAnswerIndex = _correctAnswerIndexInput;
    }

    public QuestionClass(QuestionClass _input)
    {
        _questionID = _input._questionID;

        _questionText = _input._questionText;

        _answeredCorrectly = _input._answeredCorrectly;

        _answers = _input._answers;

        _correctAnswerIndex = _input._correctAnswerIndex;
    }

    [SerializeField]
    string _questionID;

    [SerializeField]
    string _questionText;

    bool _answeredCorrectly = false;

    [SerializeField]
    string[] _answers = new string[4];

    [SerializeField]
    int _correctAnswerIndex = 0;

    [SerializeField]
    string _correctAnswerResponse;

    [SerializeField]
    string _dialogueClipName;

    QuestionGroupClass _belongingGroup;

    public string GetQuestionID()
    {
        return _questionID;
    }

    public string GetQuestionText() { return _questionText; }

    public bool GetQuestionAnsweredCorrectly() { return _answeredCorrectly; }

    public string [] GetAnswers()
    {
        return _answers;
    }

    public string GetCorrectAnswer()
    {
        return _answers[_correctAnswerIndex];
    }

    public int GetCorrectAnswerIndex() { return _correctAnswerIndex; }

    public string GetDialogueClipName()
    {
        return _dialogueClipName;
    }

    public QuestionGroupClass GetBelongingGroup()
    {
        return _belongingGroup;
    }

    public string GetCorrectAnswerResponse()
    {
        return _correctAnswerResponse;
    }

    public void SetAnsweredCorrectly(bool _input)
    {
        _answeredCorrectly = _input;
    }

    public void SetBelongingGroup(QuestionGroupClass _input)
    {
        _belongingGroup = _input;
    }

    public void SetQuestionID(string _input)
    {
        _questionID = _input;
    }

    public void RandomizeAnswerPositions()
    {
        List<string> _a = _answers.ToList();

        List<string> _b = new List<string>();

        List<int> _c = new List<int>();

        for(int _k = 0; _k < 4; _k++)
        {
            _c.Add(_k);
        }

        bool _correctAnswerFound = false;

        for(int _i = 0; _i < 4; _i++)
        {
            int _j = UnityEngine.Random.Range(0, _a.Count);

            _b.Add(_a[_j]);

            if(_c[_j] == _correctAnswerIndex && !_correctAnswerFound)
            {
                _correctAnswerIndex = _i;

                _correctAnswerFound = true;
            }

            _a.RemoveAt(_j);

            _c.RemoveAt(_j);
        }

        _answers = _b.ToArray();
    }
}

[System.Serializable]
public class QuestionGroupClass
{
    [SerializeField]
    string _groupName;

    [SerializeField]
    QuestionClass[] _questions = new QuestionClass[4];

    public string GetGroupName()
    {
        return _groupName;
    }

    public List<QuestionClass> GetQuestions()
    {
        return _questions.ToList();
    }

    public QuestionClass GetRandomQuestion()
    {
        int _range = _questions.Length;

        int _i = UnityEngine.Random.Range(0, _range);

        QuestionClass _selectedQuestion = _questions[_i];

        return _selectedQuestion;
    }

    public void SetBelongingGroupToQuestions()
    {
        for(int _i = 0; _i < _questions.Count(); _i++)
        {
            _questions[_i].SetBelongingGroup(this);
        }
    }
}
