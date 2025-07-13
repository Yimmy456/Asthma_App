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

    int _currentIndex = -1;


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

        _currentIndex++;

        if(_currentIndex == _tutorialSubjectTargets2.Count)
        {
            ICompleteExperience();
        }

        _canvas.SetArrowPositionBasedOnObject(_tutorialSubjectTargets2[_currentIndex]);
    }

    public void GoToNextStepUnderIndexMatchCondition(int _input)
    {
        if (!_tutorialMode)
        {
            return;
        }

        if (_currentIndex == _input)
        {
            _currentIndex++;
        }

        if (_currentIndex == _tutorialSubjectTargets2.Count)
        {
            ICompleteExperience();
        }

        _canvas.SetArrowPositionBasedOnObject(_tutorialSubjectTargets2[_currentIndex]);
    }

    public void WaitToReappear(float _input = 10.0f)
    {
        StartCoroutine(WaitToReappearIEnumerator(_input));
    }

    IEnumerator WaitToReappearIEnumerator(float _seconds = 10)
    {
        if(_canvas == null || !_tutorialMode)
        {
            yield break;
        }

        _canvas.gameObject.SetActive(false);

        yield return new WaitForSeconds(_seconds);

        _canvas.gameObject.SetActive(true);
    }

    public void IStartExperience()
    {
        _tutorialMode = true;

        _currentIndex = 0;

        if (_canvas != null)
        {
            _canvas.gameObject.SetActive(true);

            _canvas.SetArrowPositionBasedOnObject(_tutorialSubjectTargets2[0]);
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
    }

    public void IResumeExperience()
    {

    }

    public IEnumerator IWaitUntilCompletion()
    {
        yield break;
    }
}

[System.Serializable]
public class TutorialLessonClass
{
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

    public GameObject GetGameObject()
    {
        return _object;
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
}
