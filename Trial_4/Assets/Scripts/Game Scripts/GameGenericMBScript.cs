using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGenericMBScript<T> : GameMBScript, YesOrNoInterface
{
    [SerializeField]
    protected GamePropertiesClass<T> _gameProperties;

    [SerializeField]
    protected Color _indicatorColor;

    public GamePropertiesClass<T> GetGameProperties()
    {
        return _gameProperties;
    }

    protected string GetBadgeStatement()
    {
        BadgeScript _selectedBadge = _gameProperties.GetBadge();

        if(_selectedBadge == null)
        {
            return "";
        }

        string _badgeStatement;

        if(!_selectedBadge.GetBadgeCollected())
        {
            _badgeStatement = "Congratuations! You have won a new badge! it is the " + @"""" + _selectedBadge.GetBadgeName() + @"""" + " badge!";
        }
        else
        {
            _badgeStatement = "Well done! Although, you have already earned the " + @"""" + _selectedBadge.GetBadgeName() + @"""" + " badge.";
        }

        return _badgeStatement;
    }

    public override void StartGame()
    {
        if(_currentGame != null)
        {
            return;
        }

        Debug.Log("We are preparing your selected game...");

        _gameProperties.GetMeter().SetValue(0);

        if(_gameProperties.GetGameIndicatorProperties() != null)
        {
            _gameProperties.GetGameIndicatorProperties().SetIndicatorColor(_indicatorColor);

            _gameProperties.GetGameIndicatorProperties().SetTargetObject(_gameSpace);
        }

        if(_gameProperties.GetGameIndicatorCanvas() != null)
        {
            //Indicator

            _gameProperties.GetGameIndicatorCanvas().gameObject.SetActive(PrepareGameIndicator());
        }

        base.StartGame();
    }

    public virtual void SetActionForYesButtonToQuit()
    {
        if(_gameProperties.GetYesOrNoCanvas() == null)
        {
            return;
        }

        if(_gameProperties.GetYesOrNoCanvas().GetYesButton() == null)
        {
            return;
        }

        Button _yesB = _gameProperties.GetYesOrNoCanvas().GetYesButton();

        _yesB.onClick.AddListener(delegate { StopGame(); });
    }

    public virtual void SetActionForNoButton()
    {
        if(_gameProperties.GetYesOrNoCanvas() == null)
        {
            return;
        }

        if(_gameProperties.GetYesOrNoCanvas().GetNoButton() == null)
        {
            return;
        }

        Button _noButton = _gameProperties.GetYesOrNoCanvas().GetNoButton();

        _noButton.onClick.AddListener(delegate { _gameProperties.GetGameCanvas().gameObject.SetActive(true); });

        _noButton.onClick.AddListener(delegate { _gameProperties.GetYesOrNoCanvas().gameObject.SetActive(false); });

        if (_gameProperties.GetMainCanvases().GetDoctorCanvas() != null)
        {
            _noButton.onClick.AddListener(delegate { _gameProperties.GetMainCanvases().GetDoctorCanvas().gameObject.SetActive(true); });
        }

        _noButton.onClick.AddListener(delegate { _noButton.onClick.RemoveAllListeners(); });
    }

    public override void StopGame()
    {
        _gameProperties.GetMeter().SetValue(0);

        _gameProperties.GetMeter().SetMaxValue(1);

        _gameProperties.ClearObjectLists();

        _gameProperties.SignalToUpdateUI();

        if (_gameProperties.GetMainCanvases().GetDoctorCanvas() != null)
        {
            _gameProperties.GetMainCanvases().GetDoctorCanvas().gameObject.SetActive(true);
        }

        base.StopGame();

        if (_gameProperties.GetYesOrNoCanvas().gameObject.activeSelf)
        {
            _gameProperties.GetYesOrNoCanvas().gameObject.SetActive(false);
        }

        _gameProperties.GetYesOrNoCanvas().GetNoButton().onClick.RemoveAllListeners();

        _gameProperties.GetYesOrNoCanvas().GetYesButton().onClick.RemoveAllListeners();
    }

    protected virtual void ShuffleFunction()
    {

    }

    protected override void WinGame()
    {
        base.WinGame();

        if (!_gameDone)
        {
            StopGame();

            _gameDone = true;
        }

        _gameProperties.GetNewBadgeCanvas().gameObject.SetActive(true);

        Text _newBadgeText = _gameProperties.GetNewBadgeCanvas().gameObject.GetComponent<RectTransform>().Find("Congratulations Text").gameObject.GetComponent<Text>();

        if(_newBadgeText != null)
        {
            _newBadgeText.text = GetBadgeStatement();
        }

        Image _badgeImage = _gameProperties.GetNewBadgeCanvas().gameObject.GetComponent<RectTransform>().Find("Badge Image").gameObject.GetComponent<Image>();

        if(_badgeImage != null)
        {
            _badgeImage.sprite = _gameProperties.GetBadge().GetBadgeSprite();
        }

        BadgesManagerScript.GetInstance().SetBadgeEarned(_gameProperties.GetBadge());

        Button _b = _gameProperties.GetNewBadgeCanvas().gameObject.GetComponent<RectTransform>().Find("Next Button").gameObject.GetComponent<Button>();

        _gameProperties.ClearObjectLists();

        if(_b != null)
        {
            _b.onClick.AddListener(delegate
            {
                _gameDone = false;

                if(_currentGame == this)
                {
                    _currentGame = null;
                }

                if(_currentGameInSession)
                {
                    _currentGameInSession = false;
                }

                _gameProperties.GetMeter().SetValue(0);

                _gameProperties.GetMeter().SetMaxValue(1);

                _gameProperties.GetMainCanvases().SetCanvasesOn(true);

                _gameProperties.GetNewBadgeCanvas().gameObject.SetActive(false);

                _b.onClick.RemoveAllListeners();
            });
        }

        _gameProperties.GetGameCanvas().gameObject.SetActive(false);
    }

    public override void QuitGame()
    {
        Button _noButton = _gameProperties.GetYesOrNoCanvas().GetNoButton();

        Button _yesButton = _gameProperties.GetYesOrNoCanvas().GetYesButton();

        _gamePaused = true;

        if(_noButton != null)
        {
            _noButton.onClick.AddListener(delegate { ISetActionsOfNoButton(); });
        }

        if(_yesButton != null)
        {
            _yesButton.onClick.AddListener(delegate { ISetActionsOfYesButtonToQuit(); });
        }
    }

    public virtual void ISetActionsOfNoButton()
    {
        _gameProperties.GetGameCanvas().gameObject.SetActive(true);

        _gameProperties.GetYesOrNoCanvas().gameObject.SetActive(false);

        if(_gameProperties.GetGameIndicatorCanvas() != null)
        {
            _gameProperties.GetGameIndicatorCanvas().gameObject.SetActive(true);
        }

        _gamePaused = false;

        _gameProperties.GetYesOrNoCanvas().GetYesButton().onClick.RemoveAllListeners();

        _gameProperties.GetYesOrNoCanvas().GetNoButton().onClick.RemoveAllListeners();
    }

    public virtual void ISetActionsOfYesButtonToQuit() {

        _gameProperties.GetMainCanvases().SetCanvasesOn(true);

        if(_gameProperties.GetGameCanvas().gameObject.activeSelf)
        {
            _gameProperties.GetGameCanvas().gameObject.SetActive(false);
        }

        if(_gameProperties.GetGameIndicatorCanvas() != null)
        {
            if(_gameProperties.GetGameIndicatorCanvas().gameObject.activeSelf)
            {
                _gameProperties.GetGameIndicatorCanvas().gameObject.SetActive(false);
            }
        }

        StopGame();
    }

    public virtual void ISetActionsOfYesButtonToRestart()
    {

    }

    bool PrepareGameIndicator()
    {

        if(SettingsManager.GetInstance() == null)
        {
            return true;
        }

        BooleanSettingsClass _boolS =  SettingsManager.GetInstance().GetBooleanSettingByName("Indicator On");

        if(_boolS == null)
        {
            return true;
        }

        return _boolS.GetBooleanValue();
    }
}
