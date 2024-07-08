using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ProcedurePhaseClass
{
    [SerializeField]
    string _phaseName;

    [SerializeField]
    MeterClass _phaseCompletionMeter;

    [SerializeField]
    List<ProcedurePhaseChapterClass> _chapters;

    ProcedurePhaseChapterClass _currentChapter;

    [ContextMenu("Add New Chapter/Game Chapter")]
    public void AddGameChapter()
    {
        _chapters.Add(new ProcedurePhaseChapterClass_Game());
    }

    [ContextMenu("Add New Chapter/Lesson Chapter")]
    public void AddLessonChapter()
    {
        Debug.Log("We are adding a lesson chapter.");
    }

    public string GetPhaseName()
    {
        return _phaseName;
    }

    public MeterClass GetPhaseCompletionMeter()
    {
        return _phaseCompletionMeter;
    }

    public List<ProcedurePhaseChapterClass> GetChapters()
    {
        return _chapters;
    }

    public ProcedurePhaseChapterClass GetCurrentChapter()
    {
        return _currentChapter;
    }
}
