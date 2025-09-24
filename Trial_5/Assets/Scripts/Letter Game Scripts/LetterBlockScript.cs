using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LetterBlockScript : MatchingGameBlockScript
{
    [SerializeField]
    protected char _letter;

    private void Update()
    {
        UpdateBlockWhenDragged();

        MaintainVelocity();

        StayInXnZRange();
    }

    public char GetLetter()
    {
        return _letter;
    }

    public bool MatchLetters(char _input)
    {
        return (string.Compare(_input.ToString(), _letter.ToString(), true) == 0);
    }
}
