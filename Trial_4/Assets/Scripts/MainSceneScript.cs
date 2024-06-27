using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;

public class MainSceneScript : MonoBehaviour, YesOrNoInterface
{

    [Header("Buttons")]
    [SerializeField]
    Button _exitButton;

    [SerializeField]
    Button _restartButton;

    [SerializeField]
    GameObject _menuButtons;

    [Header("Canvases")]
    [SerializeField]
    YesOrNoCanvasScript _yesOrNoCanvas;

    [SerializeField]
    Canvas _indicatorCanvas;

    [SerializeField]
    Canvas _playerCanvas;

    [SerializeField]
    Canvas _dialogueCanvas;

    [SerializeField]
    Canvas _startingCanvas;

    [Header("Other Variables")]
    [SerializeField]
    Animator _animator;

    [SerializeField]
    PlaceIndicatorScript _placeIndicator;

    [SerializeField]
    GameObject _rocket;

    [SerializeField]
    GameObject _flag;

    [SerializeField]
    ParticleSystem _rocketParticles;

    [SerializeField]
    Animator _fastyAnimator;

    public bool _started;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_animator != null)
        {
            _animator.SetBool("Landed", _started);
        }
    }

    public Button GetExitButton()
    {
        return _exitButton;
    }

    public YesOrNoCanvasScript GetYesOrNoCanvas()
    {
        return _yesOrNoCanvas;
    }

    public void ISetActionsOfNoButton()
    {
        _playerCanvas.gameObject.SetActive(true);

        _indicatorCanvas.gameObject.SetActive(true);

        _dialogueCanvas.gameObject.SetActive(true);

        _animator.SetFloat("Animation Speed", 1.0f);

        if (_rocketParticles != null)
        {
            _rocketParticles.Play();
        }

        //if(_menuButtons != null)
        //{
        //    _menuButtons.SetActive(false);
        //}

        _yesOrNoCanvas.gameObject.SetActive(false);

        _yesOrNoCanvas.GetYesButton().onClick.RemoveAllListeners();

        _yesOrNoCanvas.GetNoButton().onClick.RemoveAllListeners();
    }

    public void ISetActionsOfYesButtonToQuit()
    {
        if(DataPersistenceManager.GetInstance() != null)
        {
            DataPersistenceManager.GetInstance().SaveGame();
        }

        SceneManager.LoadScene("Menu Scene");
    }

    public void ISetActionsOfYesButtonToRestart()
    {
        if (_animator != null)
        {
            //_animator.StopPlayback();

            _animator.SetFloat("Animation Speed", 1.0f);

            _animator.SetTrigger("Restart Animation");

            _animator.SetBool("Landed", false);

            SetLandedInAnimator(false);            
        }

        _placeIndicator.RestartLanding();

        _placeIndicator.ResetInhalerPosition();

        _restartButton.gameObject.SetActive(false);

        _startingCanvas.gameObject.SetActive(true);

        _playerCanvas.gameObject.SetActive(true);

        _yesOrNoCanvas.GetNoButton().onClick.RemoveAllListeners();

        _yesOrNoCanvas.GetYesButton().onClick.RemoveAllListeners();

        if(_rocket != null)
        {
            _rocket.SetActive(false);
        }

        if(_flag != null)
        {
            _flag.SetActive(false);
        }

        if(_playerCanvas.gameObject.GetComponent<MainPlayerCanvasScript>() != null)
        {
            _playerCanvas.gameObject.GetComponent<MainPlayerCanvasScript>().HideButtonsImmediately();
        }

        if(_rocketParticles != null)
        {
            _rocketParticles.Play();
        }

        _dialogueCanvas.gameObject.SetActive(true);

        _yesOrNoCanvas.gameObject.SetActive(false);
    }

    public void PrepareToLeave()
    {
        if(_playerCanvas == null || _indicatorCanvas == null || _yesOrNoCanvas == null || _exitButton == null || _placeIndicator == null || _dialogueCanvas == null)
        {
            return;
        }

        if(_yesOrNoCanvas.GetNoButton() == null || _yesOrNoCanvas.GetYesButton() == null)
        {
            return;
        }

        _yesOrNoCanvas.SetText("Are you sure that you want to leave the game?");

        _animator.SetFloat("Animation Speed", 0.0f);

        if(_rocketParticles != null)
        {
            _rocketParticles.Pause();
        }

        _yesOrNoCanvas.GetNoButton().onClick.AddListener(delegate { ISetActionsOfNoButton(); });

        _yesOrNoCanvas.GetYesButton().onClick.AddListener(delegate { ISetActionsOfYesButtonToQuit(); });
    }

    public void PrepareToRestart()
    {
        if(_playerCanvas == null || _indicatorCanvas == null || _yesOrNoCanvas == null || _restartButton == null || _dialogueCanvas == null || _startingCanvas == null)
        {
            return;
        }

        if(_yesOrNoCanvas.GetNoButton() == null || _yesOrNoCanvas.GetYesButton() == null)
        {
            return;
        }

        _yesOrNoCanvas.SetText("Are you sure you want to reset your AR position?");

        _animator.SetFloat("Animation Speed", 0.0f);

        if(_rocketParticles != null)
        {
            _rocketParticles.Pause();
        }

        _yesOrNoCanvas.GetNoButton().onClick.AddListener(delegate { ISetActionsOfNoButton(); });

        _yesOrNoCanvas.GetYesButton().onClick.AddListener(delegate { ISetActionsOfYesButtonToRestart(); });
    }

    public void SetLandedInAnimator(bool _input)
    {
        if(_animator == null)
        {
            return;
        }

        bool _paramterValid = ToolsStruct.CheckParameterExistance(_animator, "Landed");

        if(!_paramterValid)
        {
            Debug.LogError("There is no parameter called " + @"""" + "Landed" + @"""" + ". Sorry.");

            return;
        }

        _animator.SetBool("Landed", _input);
    }

    public void TriggerGettingOffAnimation()
    {
        if(_fastyAnimator == null)
        {
            return;
        }

        _fastyAnimator.SetTrigger("Begin Intro");
    }

    public void TriggerInhalerAnimation()
    {
        if(_fastyAnimator == null)
        {
            return;
        }

        _fastyAnimator.SetTrigger("Shake Inhaler");
    }
}
