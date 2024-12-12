using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameGenericMBScript<T> : GameMBScript
{
    [SerializeField]
    protected GamePropertiesClass<T> _gameProperties;

    public GamePropertiesClass<T> GetGameProperties()
    {
        return _gameProperties;
    }

    public override void IStartExperience()
    {
        if(_currentGame != null)
        {
            return;
        }

        //Debug.Log("We are preparing your selected game...");

        //GameCanvasScript _gameCanvas = _gameCanvas.gameObject.GetComponent<GameCanvasScript>();

        //_gameCanvas.gameObject.SetActive(fal);

        //InitializeQuitButton();

        base.IStartExperience();
    }

    public virtual void SetActionForYesButtonToQuit()
    {
        if(_yesOrNoCanvas == null)
        {
            return;
        }

        if(_yesOrNoCanvas.GetYesButton() == null)
        {
            return;
        }

        Button _yesB = _yesOrNoCanvas.GetYesButton();

        _yesB.onClick.AddListener(delegate { IStopExperience(); });
    }

    public virtual void SetActionForNoButton()
    {
        if(_yesOrNoCanvas == null)
        {
            return;
        }

        if(_yesOrNoCanvas.GetNoButton() == null)
        {
            return;
        }

        Button _noButton = _yesOrNoCanvas.GetNoButton();

        _noButton.onClick.AddListener(delegate { _gameCanvas.gameObject.SetActive(true); });

        _noButton.onClick.AddListener(delegate { _yesOrNoCanvas.gameObject.SetActive(false); });

        if (_mainCanvases.GetDoctorCanvas() != null)
        {
            _noButton.onClick.AddListener(delegate { _mainCanvases.GetDoctorCanvas().gameObject.SetActive(true); });
        }

        _noButton.onClick.AddListener(delegate { _noButton.onClick.RemoveAllListeners(); });
    }

    public override void IStopExperience()
    {
        _gameProperties.ClearObjectLists();

        base.IStopExperience();
    }

    public  override void ICompleteExperience()
    {
        _gameProperties.ClearObjectLists();

        base.ICompleteExperience();
    }

    public override void IChooseToQuitExperience()
    {
        base.IChooseToQuitExperience();
    }

    public override void ISetActionsOfNoButton()
    {

    }

    public override void ISetActionsOfYesButtonToQuit() {

        base.ISetActionsOfYesButtonToQuit();
    }

    public override void ISetActionsOfYesButtonToRestart()
    {

    }
}
