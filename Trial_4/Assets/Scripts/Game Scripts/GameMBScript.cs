using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMBScript : MonoBehaviour
{ 
    [Header("1. Variables of all games")]
    [SerializeField]
    protected string _gameName;

    [SerializeField]
    protected Transform _mainContainer;

    [SerializeField]
    protected GameObject _gameSpace;

    protected bool _currentGameInSession;

    protected static GameMBScript _currentGame = null;

    protected bool _gameDone = false;

    public string GetGameName()
    {
        return _gameName;
    }

    public bool GetCurrentGameInSession()
    {
        return _currentGameInSession;
    }

    public static GameMBScript GetCurrentGame()
    {
        return _currentGame;
    }

    protected static bool _gamePaused = false;

    public static bool GetGameInSession()
    {
        return (_currentGame != null);
    }

    /*public virtual void StartGameFunction()
    {
        if(GetGameInSession())
        {
            return;
        }

        _currentGameInSession = true;
    }

    public virtual void StartGameFunction(int _input)
    {
        StartGameFunction();
    }

    public virtual void ClearGameFunction()
    {
        _currentGame = null;

        _currentGameInSession = false;
    }

    public virtual void FinishGame()
    {
        _gameDone = true;

        ClearGameFunction();
    }

    protected virtual bool CanRewardBadge()
    { 
        if(BadgesManagerScript.GetInstance() == null)
        {
            return false;
        }

        return true;
    }*/

    public bool GetGameDone()
    {
        return _gameDone;
    }

    protected virtual string SetInfoText()
    {
        return "";
    }

    protected virtual void SetBadge()
    {

    }

    protected virtual void SetBadge(string _textInput, bool _caseSensitiveInput = false)
    {

    }



    //Game Status

    public virtual void StartGame()
    {
        if(_currentGame != null)
        {
            return;
        }

        _gameDone = false;

        _currentGame = this;

        _currentGameInSession = true;
    }

    public virtual void StartGame(int _input) { }

    public virtual void StartGame(string _input) { }

    public virtual void RestartGame()
    {

    }

    public virtual void RestartGameWithAnotherInstance()
    {

    }

    public virtual void RestartGameWithAnotherInstance(int _input) { }

    public virtual void RestartGameWithAnotherInstance(string _input) { }

    public virtual void StopGame()
    {
        //_currentGame = null;

        //_gameDone = true;

        if(_currentGame == this)
        {
            _currentGame = null;
        }

        if (_currentGameInSession)
        {
            _currentGameInSession = false;
        }

        if(_gamePaused)
        {
            _gamePaused = false;
        }
    }

    protected virtual void WinGame()
    {
        
    }

    public static bool GamePaused()
    {
        return _gamePaused;
    }

    public virtual void QuitGame()
    {
        //StopGame();
    }
    /*public virtual void FinishGame()
    {

    }*/
}
