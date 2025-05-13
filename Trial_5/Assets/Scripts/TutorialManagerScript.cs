using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManagerScript : MonoBehaviour
{
    bool _tutorialMode = true;

    [SerializeField]
    TutorialCanvasScript _canvas;

    [SerializeField]
    List<TutorialSubjectTargetClass> _tutorialSubjectTargets;

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
            if(DataPersistenceManager.GetInstance().GetGameData() != null)
            {
                _tutorialMode = !DataPersistenceManager.GetInstance().GetGameData()._tutorialComplete;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetTutorialMode()
    {
        return _tutorialMode;
    }

    public List<TutorialSubjectTargetClass> GetTutorialSubjectTargets()
    {
        return _tutorialSubjectTargets;
    }

    public int GetCurrentIndex()
    {
        return _currentIndex;
    }

    public TutorialSubjectTargetClass GetCurrentStep()
    {
        if(!(_currentIndex >= 0 && _currentIndex < _tutorialSubjectTargets.Count) || !_tutorialMode)
        {
            return null;
        }

        return _tutorialSubjectTargets[_currentIndex];
    }

    public void SetTutorialMode(bool _input)
    {
        _tutorialMode = _input;

        SetIndexOnStart();
    }

    public void ToggleTutorialMode()
    {
        _tutorialMode = !_tutorialMode;

        SetIndexOnStart();
    }

    public void SaveTutorialComplete()
    {
        if(DataPersistenceManager.GetInstance() == null) { return; }

        if(DataPersistenceManager.GetInstance().GetGameData() == null) { return; }

        DataPersistenceManager.GetInstance().GetGameData()._tutorialComplete = true;

        _tutorialMode = false;

        _currentIndex = -1;
    }

    void SetIndexOnStart()
    { 
        _currentIndex = _tutorialMode ? 0 : -1;
    }
}
