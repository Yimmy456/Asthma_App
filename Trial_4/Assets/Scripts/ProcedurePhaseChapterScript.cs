using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ProcedurePhaseChapterClass
{
    [SerializeField]
    protected string _chapterName;

    [SerializeField]
    protected MeterClass _chapterCompletionPhase;

    public string GetChapterName()
    {
        return _chapterName;
    }

    public MeterClass GetChapterCompletionPhase()
    {
        return _chapterCompletionPhase;
    }
}

public class ProcedurePhaseChapterClass_Game : ProcedurePhaseChapterClass
{
    [SerializeField]
    string _input;
}
