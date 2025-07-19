using Org.BouncyCastle.Asn1.Mozilla;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManagerScript : MonoBehaviour, ExperienceInterface
{
    bool _tutorialMode = true;

    [SerializeField]
    TutorialCanvasScript _canvas;

    [SerializeField]
    List<TutorialLessonClass> _tutorialSubjectTargets;

    [SerializeField]
    List<TutorialLessonClass> _tutorialSubjectTargets2;

    [SerializeField]
    MainPlayerCanvasScript _mainPlayerCanvas;

    int _currentIndex = -1;

    GameObject _currentObject;

    GameObject _currentObjectDuplicate;

    Coroutine _watingCoroutine;

    public TutorialCanvasScript Canvas
    {
        get
        {
            return _canvas;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(DataPersistenceManager.GetInstance() != null)
        {
            if (DataPersistenceManager.GetInstance().GetGameData() != null)
            {
                _tutorialMode = !DataPersistenceManager.GetInstance().GetGameData()._tutorialComplete;

                if (_tutorialMode)
                {
                    IStartExperience();
                }
                else
                {
                    _canvas.GetUIIndicator().gameObject.SetActive(false);

                    _canvas.gameObject.SetActive(false);
                }

                //IStartExperience();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        IUpdateExperience();
    }

    public bool GetTutorialMode()
    {
        return _tutorialMode;
    }

    public int GetCurrentIndex()
    {
        return _currentIndex;
    }

    public TutorialLessonClass GetTutorialSubjectByName(string _input)
    {
        for(int _i = 0; _i < _tutorialSubjectTargets2.Count; _i++)
        {
            if (_tutorialSubjectTargets2[_i].GetName() == _input)
            {
                return _tutorialSubjectTargets2[_i];
            }
        }

        return null;
    }

    public int GetLessonIndex(string _nameInput)
    {
        for(int _i = 0; _i < _tutorialSubjectTargets2.Count; _i++)
        {
            if (_tutorialSubjectTargets2[_i].GetName() == _nameInput)
            {
                return _i;
            }
        }
        return -1;
    }

    public int GetLessonIndex(TutorialLessonClass _input)
    {
        for (int _i = 0; _i < _tutorialSubjectTargets2.Count; _i++)
        {
            if (_tutorialSubjectTargets2[_i] == _input)
            {
                return _i;
            }
        }
        return -1;
    }

    public void SetTutorialMode(bool _input)
    {
        _tutorialMode = _input;
    }

    public void SwitchTutorialMode()
    {
        _tutorialMode = !_tutorialMode;
    }

    public void GoToNextStep()
    {
        if(!_tutorialMode)
        {
            return;
        }

        ProcessOfLeavingPreviousStep();

        _currentIndex++;

        ProcessOfGoingToNextStep();
    }

    public void GoToNextStepUnderIndexMatchCondition(int _input)
    {
        if (!_tutorialMode)
        {
            return;
        }

        if(_currentIndex != _input)
        {
            return;
        }

        ProcessOfLeavingPreviousStep();

        _currentIndex++;       

        ProcessOfGoingToNextStep();
    }

    public void GoToNextStepUnderNameMatchCondition(string _input)
    {
        if (!_tutorialMode)
        {
            return;
        }

        if (_tutorialSubjectTargets2[_currentIndex].GetName() != _input)
        {
            return;
        }

        ProcessOfLeavingPreviousStep();

        _currentIndex++;

        ProcessOfGoingToNextStep();
    }

    public void SetTimingState(float _secondsInput = 10.0f)
    {
        if(!_tutorialMode)
        {
            return;
        }

        if(_watingCoroutine != null)
        {
            StopCoroutine(_watingCoroutine);
        }

        _watingCoroutine = StartCoroutine(TimingStateIEnumerator(_secondsInput));
    }

    IEnumerator TimingStateIEnumerator(float _secondsInput = 10.0f)
    {
        yield return new WaitForSeconds(_secondsInput);

        GoToNextStep();
    }

    public void IStartExperience()
    {
        _tutorialMode = true;

        _currentIndex = 0;

        if (_canvas != null)
        {
            _canvas.gameObject.SetActive(true);

            _canvas.SetArrowPositionBasedOnObject(_tutorialSubjectTargets2[0]);

            _tutorialSubjectTargets2[0].IOnStateEnter();
        }
    }

    public void IStartExperience(int _input)
    {

    }

    public void IStartExperience(string _input)
    {

    }

    public void IStopExperience()
    {
        _tutorialMode = false;

        _currentIndex = -1;

        if (_canvas != null)
        {
            _canvas.gameObject.SetActive(false);

            _canvas.ResetCanvas();
        }
    }

    public void ICompleteExperience()
    {
        DataPersistenceManager.GetInstance().GetGameData()._tutorialComplete = true;
    }

    public void IChooseToQuitExperience()
    {

    }

    public void IChooseToRestartExperience()
    {

    }

    public void IUpdateExperience()
    {
        if(!_tutorialMode)
        {
            return;
        }

        if(_canvas != null)
        {
            if(_canvas.GetUIIndicator() != null)
            {
                if(_canvas.GetUIIndicator().gameObject.activeSelf && _canvas.GetUIIndicator().GetTargetObject() != null)
                {
                    if (_tutorialSubjectTargets2[_currentIndex].GetShowNextButton())
                    {
                        _canvas.GetNextButton().gameObject.SetActive(_canvas.GetUIIndicator().GetTargetInSight());
                    }
                }
            }
        }

        _tutorialSubjectTargets2[_currentIndex].IOnStateStay();
    }

    public void IResumeExperience()
    {

    }

    public IEnumerator IWaitUntilCompletion()
    {
        yield break;
    }

    public void SetCurrentObject(GameObject _input, bool _duplicateInput = false)
    {
        _currentObject = _input;

        if(_duplicateInput && _currentObject.GetComponent<RectTransform>() != null)
        {
            _currentObjectDuplicate = Instantiate(_currentObject);

            _currentObjectDuplicate.GetComponent<RectTransform>().parent = _canvas.GetComponent<RectTransform>();

            _currentObjectDuplicate.SetActive(true);

            _currentObject.SetActive(false);
        }
    }

    void CheckCurrentLesson()
    {
        if(!_tutorialMode)
        {
            return;
        }

        TutorialLessonClass _currentSubject = _tutorialSubjectTargets2[_currentIndex];

        if (_currentSubject.GetName() == "Menu Button")
        {
            if(_canvas != null && _currentObjectDuplicate != null)
            {
                if(_currentObjectDuplicate.GetComponent<Button>() != null)
                {
                    //Button _bt = _currentObjectDuplicate.GetComponent<Button>();

                    //_bt.onClick.RemoveAllListeners();

                    //_bt.onClick.AddListener(delegate { _mainPlayerCanvas.ShowButtonsImmediately(); Destroy(_bt.gameObject); });
                }
            }
        }

        if(_currentSubject.GetName() == "Inhaler Button")
        {
            if (_canvas != null && _currentObjectDuplicate != null)
            {
                if (_currentObjectDuplicate.GetComponent<Button>() != null)
                {
                    Button _bt = _currentObjectDuplicate.GetComponent<Button>();

                    _bt.onClick.RemoveAllListeners();

                    _bt.onClick.AddListener(delegate { GoToNextStepUnderNameMatchCondition("Inhaler Button"); });

                    _bt.onClick.AddListener(delegate { Destroy(_bt.gameObject); });
                }
            }
        }

        if (_currentSubject.GetName() == "Inhaler")
        {
            if (_mainPlayerCanvas != null && _canvas != null)
            {
                if (_canvas.GetNextButton() != null)
                {
                    _canvas.GetNextButton().onClick.AddListener(delegate { _mainPlayerCanvas.GetShakingInhalerProperties().SetShowingInhalerBoolean(false); });
                }
            }
        }

        /*if(_currentSubject.GetName() == "Dictionary Button")
        {
            if (_canvas != null && _currentObjectDuplicate != null)
            {
                if (_currentObjectDuplicate.GetComponent<Button>() != null)
                {
                    Button _bt = _currentObjectDuplicate.GetComponent<Button>();

                    _bt.onClick.RemoveAllListeners();

                    _bt.interactable = false;

                    Color _c = _bt.image.color;

                    _c.a = 1.0f;

                    _bt.image.color = _c;

                    RectTransform _rt1 = _currentObject.GetComponent<RectTransform>();

                    RectTransform _rt2 = _currentObjectDuplicate.GetComponent<RectTransform>();

                    _rt2.parent = _rt1;

                    //_rt2.anchorMin = new Vector2(0.5f, 0.5f);

                    //_rt2.anchorMax = new Vector2(0.5f, 0.5f);

                    _rt2.localScale = Vector3.one;

                    _rt2.parent = _canvas.GetComponent<RectTransform>();

                    Vector3 _ap = _rt2.anchoredPosition;

                    _ap.x = -236.0f;

                    _rt2.anchoredPosition = _ap;
                }
            }
        }*/
    }

    void ProcessOfGoingToNextStep()
    {
        if (_currentIndex == _tutorialSubjectTargets2.Count)
        {
            ICompleteExperience();

            return;
        }

        TutorialLessonClass _currentSubject = _tutorialSubjectTargets2[_currentIndex];

        if (_canvas != null)
        {
            if (_canvas.GetNextButton() != null)
            {
                _canvas.GetNextButton().gameObject.SetActive(false);

                _canvas.GetNextButton().onClick.RemoveAllListeners();
            }

            _canvas.SetArrowPositionBasedOnObject(_currentSubject);
        }

        _currentObject = _currentSubject.GetGameObject();

        if (_currentObject != null)
        {
            if (_currentObject.GetComponent<RectTransform>() != null)
            {
                _currentObjectDuplicate = Instantiate(_currentObject);

                //_currentObjectDuplicate.GetComponent<RectTransform>().parent = _canvas.GetComponent<RectTransform>();

                //_currentObject.SetActive(false);

                _currentObjectDuplicate.SetActive(true);

                RectTransform _rt1 = _currentObject.GetComponent<RectTransform>();

                RectTransform _rt2 = _currentObjectDuplicate.GetComponent<RectTransform>();

                /*_rt2.anchorMin = _rt1.anchorMin;

                _rt2.anchorMax = _rt1.anchorMax;

                _rt2.anchoredPosition = _rt1.anchoredPosition;

                _rt2.localScale = _rt1.localScale;*/

                _rt2.parent = _rt1;

                _rt2.anchorMin = new Vector2(0.5f, 0.5f);

                _rt2.anchorMax = new Vector2(0.5f, 0.5f);

                _rt2.localScale = Vector3.one;

                _rt2.localRotation = Quaternion.identity;

                _rt2.anchoredPosition = Vector2.zero;

                _rt2.parent = _canvas.GetComponent<RectTransform>();

                _currentObject.gameObject.SetActive(false);

                if(_currentSubject.GetShowNextButton())
                {
                    if(_currentObjectDuplicate.GetComponent<Button>() != null)
                    {
                        _currentObjectDuplicate.GetComponent<Button>().interactable = false;
                    }
                }
            }
        }

        _currentSubject.IOnStateEnter();

        float _waitingSeconds = _currentSubject.GetWaitingSeconds();

        if (_waitingSeconds >= 0.0f)
        {
            SetTimingState(_waitingSeconds);

            Debug.Log("Counting begins now...");
        }
        else
        {
            if (_canvas != null)
            {
                if (_canvas.GetNextButton() != null && _currentSubject.GetShowNextButton())
                {
                    _canvas.GetNextButton().gameObject.SetActive(true);

                    _canvas.GetNextButton().onClick.AddListener(delegate { GoToNextStep(); });

                    if (_currentObjectDuplicate != null)
                    {
                        if (_currentObjectDuplicate.GetComponent<Button>() != null)
                        {
                            _currentObjectDuplicate.GetComponent<Button>().interactable = false;
                        }
                    }
                }
            }
        }

        CheckCurrentLesson();
    }

    void ProcessOfLeavingPreviousStep()
    {
        if (_currentObjectDuplicate != null)
        {
            Destroy(_currentObjectDuplicate);
        }

        if (_currentObject != null)
        {
            if (_currentObject.GetComponent<RectTransform>() != null)
            {
                _currentObject.SetActive(true);
            }

            _currentObject = null;
        }

        _tutorialSubjectTargets2[_currentIndex].IOnStateExit();
    }

}

[System.Serializable]
public class TutorialLessonClass : StateInterface
{
    [SerializeField]
    string _name;

    [SerializeField]
    string _text;

    [SerializeField]
    GameObject _object;

    [SerializeField]
    Vector2 _arrowPosition;

    [SerializeField]
    float _zRotation;

    [SerializeField]
    float _scale = 1.0f;

    [SerializeField]
    Image _mask;

    [SerializeField]
    bool _showNextButton;

    [SerializeField]
    float _waitingSeconds = -1.0f;

    public GameObject GetGameObject()
    {
        return _object;
    }

    public string GetName()
    {
        return _name;
    }

    public string GetText()
    {
        return _text;
    }

    public Vector2 GetArrowPosition()
    {
        return _arrowPosition;
    }

    public float GetZRotation()
    {
        return _zRotation;
    }

    public float GetScale()
    {
        return _scale;
    }

    public Image GetMask()
    {
        return _mask;
    }

    public bool GetShowNextButton()
    {
        return _showNextButton;
    }

    public float GetWaitingSeconds()
    {
        return _waitingSeconds;
    }

    public void IOnStateEnter()
    {
        Debug.Log("Starting tutorial name " + @"""" + _name + @"""" + ".");
    }

    public void IOnStateStay()
    {
        Debug.Log("Updating tutorial name " + @"""" + _name + @"""" + ".");
    }

    public void IOnStateExit()
    {
        Debug.Log("Ending tutorial name " + @"""" + _name + @"""" + ".");
    }
}
