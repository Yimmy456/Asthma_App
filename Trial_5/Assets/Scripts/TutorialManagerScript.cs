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
    MainPlayerCanvasScript _mainPlayerCanvas;

    int _currentIndex = -1;

    GameObject _currentObject;

    GameObject _currentObjectDuplicate;

    Coroutine _watingCoroutine;

    [SerializeField]
    Button _exitButton;

    [SerializeField]
    Button _resizingButton;

    [SerializeField]
    Button _menuButton;

    [SerializeField]
    Button _inhalerButton;

    [SerializeField]
    Button _startButton;

    [SerializeField]
    PlaceIndicatorScript _placeIndicator;

    Coroutine _arrowAnimationCoroutine;
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

                /*if (_tutorialMode)
                {
                    IStartExperience();
                }
                else
                {
                    _canvas.GetUIIndicator().gameObject.SetActive(false);

                    _canvas.gameObject.SetActive(false);

                    gameObject.SetActive(false);
                }*/

                IStartExperience();
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
        for(int _i = 0; _i < _tutorialSubjectTargets.Count; _i++)
        {
            if (_tutorialSubjectTargets[_i].GetName() == _input)
            {
                return _tutorialSubjectTargets[_i];
            }
        }

        return null;
    }

    public int GetLessonIndex(string _nameInput)
    {
        for(int _i = 0; _i < _tutorialSubjectTargets.Count; _i++)
        {
            if (_tutorialSubjectTargets[_i].GetName() == _nameInput)
            {
                return _i;
            }
        }
        return -1;
    }

    public int GetLessonIndex(TutorialLessonClass _input)
    {
        for (int _i = 0; _i < _tutorialSubjectTargets.Count; _i++)
        {
            if (_tutorialSubjectTargets[_i] == _input)
            {
                return _i;
            }
        }
        return -1;
    }

    public TutorialLessonClass GetCurrentLesson()
    {
        if(_currentIndex == -1 || !_tutorialMode)
        {
            return null;
        }

        return _tutorialSubjectTargets[_currentIndex];
    }

    public PlaceIndicatorScript GetPlaceIndicator()
    {
        return _placeIndicator;
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

        if (_tutorialSubjectTargets[_currentIndex].GetName() != _input)
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

            _canvas.SetArrowPositionBasedOnObject(_tutorialSubjectTargets[0]);

            _tutorialSubjectTargets[0].IOnStateEnter();

            if(_canvas.GetArrowAnimation() != null)
            {
                _arrowAnimationCoroutine = StartCoroutine(_canvas.GetArrowAnimation().Animate());
            }
        }

        ShowOrHideButton(_exitButton);

        ShowOrHideButton(_resizingButton);

        ShowOrHideButton(_menuButton);

        ShowOrHideButton(_inhalerButton);
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

        ShowOrHideButton(_exitButton, 2);

        ShowOrHideButton(_resizingButton, 2);

        ShowOrHideButton(_menuButton, 2);

        ShowOrHideButton(_inhalerButton, 2);
    }

    public void ICompleteExperience()
    {
        DataPersistenceManager.GetInstance().GetGameData()._tutorialComplete = true;

        IStopExperience();
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

        if (_canvas != null)
        {
            if(_canvas.GetUIIndicator() != null)
            {
                if(_canvas.GetUIIndicator().gameObject.activeSelf && _canvas.GetUIIndicator().GetTargetObject() != null)
                {
                    if (_tutorialSubjectTargets[_currentIndex].GetShowNextButton())
                    {
                        _canvas.GetNextButton().gameObject.SetActive(_canvas.GetUIIndicator().GetTargetInSight());
                    }
                }
            }
        }

        if (_currentIndex >= 0 && _currentIndex < _tutorialSubjectTargets.Count)
        {
            _tutorialSubjectTargets[_currentIndex].IOnStateStay();
        }
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

        TutorialLessonClass _currentSubject = _tutorialSubjectTargets[_currentIndex];

        if (_currentSubject.GetName() == "Menu Button")
        {
            if (_currentObjectDuplicate != null)
            {
                _currentObjectDuplicate.SetActive(true);

                ShowOrHideButton(_currentObjectDuplicate.GetComponent<Button>(), 2);
            }
        }

        if (_currentSubject.GetName() == "Inhaler Button")
        {
            if (_currentObjectDuplicate != null)
            {
                _currentObjectDuplicate.SetActive(true);

                ShowOrHideButton(_currentObjectDuplicate.GetComponent<Button>(), 2);

                _currentObjectDuplicate.GetComponent<Button>().onClick.AddListener(delegate { GoToNextStepUnderNameMatchCondition("Inhaler Button"); Destroy(_currentObjectDuplicate); });
            }
        }

        if (_currentSubject.GetName() == "Inhaler")
        {
            if (_mainPlayerCanvas != null && _canvas != null)
            {
                if (_canvas.GetNextButton() != null)
                {
                    _inhalerButton.gameObject.SetActive(true);

                    ShowOrHideButton(_inhalerButton, 1);

                    _canvas.GetNextButton().onClick.AddListener(delegate { _mainPlayerCanvas.GetShakingInhalerProperties().SetShowingInhalerBoolean(false); });
                }
            }
        }

        if (_currentSubject.GetName() == "Resizing Button")
        {
            if (_currentObjectDuplicate != null)
            {
                _currentObjectDuplicate.SetActive(true);

                ShowOrHideButton(_currentObjectDuplicate.GetComponent<Button>(), 1);
            }
        }

        if (_currentSubject.GetName() == "Exit Button")
        {
            if (_currentObjectDuplicate != null)
            {
                _currentObjectDuplicate.SetActive(true);

                ShowOrHideButton(_currentObjectDuplicate.GetComponent<Button>(), 1);
            }
        }

        if(_currentSubject.GetName() == "Start Button")
        {
            if(_currentObjectDuplicate != null)
            {
                _currentObjectDuplicate.SetActive(true);

                ShowOrHideButton(_currentObjectDuplicate.GetComponent<Button>(), 2);

                _currentObjectDuplicate.GetComponent<Button>().onClick.AddListener(delegate { GoToNextStepUnderNameMatchCondition("Start Button"); Destroy(_currentObjectDuplicate); });
            }
        }

        if (_currentSubject.GetName() == "End")
        {
            _canvas.GetNextButton().onClick.AddListener(delegate { ICompleteExperience(); });
        }
    }

    void ProcessOfGoingToNextStep()
    {
        if (_canvas != null)
        {
            if (_canvas.GetArrowAnimation() != null)
            {
                _canvas.GetArrowAnimation().SetAnimateBoolean(false);
            }
        }

        if (_arrowAnimationCoroutine != null)
        {
            StopCoroutine(_arrowAnimationCoroutine);
        }


        if (_currentIndex == _tutorialSubjectTargets.Count)
        {
            ICompleteExperience();

            return;
        }

        TutorialLessonClass _currentSubject = _tutorialSubjectTargets[_currentIndex];

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

                _currentObjectDuplicate.SetActive(true);

                RectTransform _rt1 = _currentObject.GetComponent<RectTransform>();

                RectTransform _rt2 = _currentObjectDuplicate.GetComponent<RectTransform>();

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

                _canvas.GetArrowAnimation().SetAnimateBoolean(true);

                _arrowAnimationCoroutine = StartCoroutine(_canvas.GetArrowAnimation().Animate());
            }
        }

        if (!(_currentIndex >= 0 && _currentIndex < _tutorialSubjectTargets.Count))
        {
            return;
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
        CheckCurrentLessonBeforeLeaving();

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

        if (_currentIndex >= 0 && _currentIndex < _tutorialSubjectTargets.Count)
        {
            _tutorialSubjectTargets[_currentIndex].IOnStateExit();
        }
    }

    void CheckCurrentLessonBeforeLeaving()
    {
        if(!_tutorialMode)
        {
            return;
        }

        TutorialLessonClass _currentLesson = _tutorialSubjectTargets[_currentIndex];

        if(_currentLesson.GetName() == "Menu Button")
        {
            _currentObject.gameObject.SetActive(true);

            _currentObject.GetComponent<Button>().interactable = true;

            ShowOrHideButton(_currentObject.GetComponent<Button>(), 2);

            Destroy(_currentObjectDuplicate);
        }

        if(_currentLesson.GetName() == "Procedure Button")
        {
            _mainPlayerCanvas.HideButtonsImmediately2();
        }

        if (_currentLesson.GetName() == "Inhaler Button")
        {
            _currentObject.gameObject.SetActive(true);

            _currentObject.GetComponent<Button>().interactable = true;

            ShowOrHideButton(_currentObject.GetComponent<Button>(), 2);
        }

        if(_currentLesson.GetName() == "Resizing Button")
        {
            _currentObject.gameObject.SetActive(true);

            ShowOrHideButton(_currentObject.GetComponent<Button>(), 2);

            Destroy(_currentObjectDuplicate);
        }
    }

    void ShowOrHideButton(Button _buttonInput, int _actionInput = 0)
    {
        if(_buttonInput == null)
        {
            Debug.LogError("There is no button.");

            return;
        }

        Color _c;

        switch (_actionInput)
        {
            case 1:
                _c = _buttonInput.image.color;
                _c.a = 1.0f;
                _buttonInput.image.color = _c;
                _buttonInput.interactable = false;
                break;
            case 2:
                _c = _buttonInput.image.color;
                _c.a = 1.0f;
                _buttonInput.image.color = _c;
                _buttonInput.interactable = true;
                break;
            default:
                _c = _buttonInput.image.color;
                _c.a = 0.0f;
                _buttonInput.image.color = _c;
                _buttonInput.interactable = false;
                break;
        }

    }

}

[System.Serializable]
public class TutorialLessonClass : StateInterface
{
    [SerializeField]
    string _name;

    [SerializeField]
    TransformTypeEnum _transformType;

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

    [SerializeField]
    TutorialLessonTextPropoertiesClass _textPropoerties;

    public GameObject GetGameObject()
    {
        return _object;
    }

    public string GetName()
    {
        return _name;
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

    public TransformTypeEnum GetTransformType()
    {
        return _transformType;
    }

    public TutorialLessonTextPropoertiesClass GetTextProperties()
    {
        return _textPropoerties;
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

[System.Serializable]
public class TutorialLessonTextPropoertiesClass
{
    [Header("Lesson Text Prpoerties")]

    [SerializeField]
    string _text;

    [SerializeField]
    string _alternativeText;

    [SerializeField]
    Vector2 _anchorMin = new Vector2(0.5f, 0.5f);

    [SerializeField]
    Vector2 _anchorMax = new Vector2(0.5f, 0.5f);

    [SerializeField]
    bool _inRelationToArrow;

    [SerializeField]
    Vector2 _anchoredPosition;

    [SerializeField]
    Vector2 _size = new Vector2(100.0f, 100.0f);

    [SerializeField]
    float _sizeConstant = 1.0f;

    public string GetText()
    {
        return _text;
    }

    public string GetAlternativeText()
    {
        return _alternativeText;
    }

    public Vector2 GetAnchorMin()
    {
        return _anchorMin;
    }

    public Vector2 GetAnchorMax()
    {
        return _anchorMax;
    }

    public bool GetInRelationToArrow()
    {
        return _inRelationToArrow;
    }

    public Vector2 GetAnchoredPosition()
    {
        return _anchoredPosition;
    }

    public Vector2 GetSize()
    {
        return _size;
    }

    public float GetSizeConstant()
    {
        return _sizeConstant;
    }

    public Vector2 GetFinalSize()
    {
        return (_size * _sizeConstant);
    }
}
