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

    [SerializeField]
    Transform _landingTerrain;

    [SerializeField]
    Button _menuButton;

    [SerializeField]
    DialoguesScript _fastyDialogues;

    //[SerializeField]
    //Animator _sceneAnimator;

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
        /*if (_animator != null)
        {
            //_animator.StopPlayback();

            _animator.SetFloat("Animation Speed", 1.0f);

            _animator.SetTrigger("Restart Animation");

            _animator.SetBool("Landed", false);

            SetLandedInAnimator(false);            
        }

        _placeIndicator.RestartLanding();

        _placeIndicator.ResetInhalerPosition();

        //_restartButton.gameObject.SetActive(false);

        _startingCanvas.gameObject.SetActive(true);

        //_playerCanvas.gameObject.SetActive(true);

        _menuButton.gameObject.SetActive(false);

        //_indicatorCanvas.gameObject.SetActive(false);

        _animator.enabled = true;

        _yesOrNoCanvas.GetNoButton().onClick.RemoveAllListeners();

        _yesOrNoCanvas.GetYesButton().onClick.RemoveAllListeners();

        if(_rocket != null)
        {
            _rocket.SetActive(false);
        }

        //if(_flag != null)
        //{
        //    _flag.SetActive(false);
        //}

        //if(_playerCanvas.gameObject.GetComponent<MainPlayerCanvasScript>() != null)
        //{
        //    _playerCanvas.gameObject.GetComponent<MainPlayerCanvasScript>().HideButtonsImmediately();
        //}

        if(_rocketParticles != null)
        {
            _rocketParticles.Play();
        }

        if(_landingTerrain != null)
        {
            _landingTerrain.gameObject.SetActive(false);
        }

        if(_rocket != null)
        {
            _rocket.SetActive(false);
        }

        _dialogueCanvas.gameObject.SetActive(true);

        _yesOrNoCanvas.gameObject.SetActive(false);*/

        if(DataPersistenceManager.GetInstance() != null)
        {
            DataPersistenceManager.GetInstance().SaveGame();
        }

        string _currentSceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(_currentSceneName);
    }

    public void ActivateCorrectButton()
    {
        if(_playerCanvas == null)
        {
            return;
        }

        if(_playerCanvas.GetComponent<MainPlayerCanvasScript>() == null)
        {
            return;
        }

        _playerCanvas.GetComponent<MainPlayerCanvasScript>().ShowCorrectButton();
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

    public void ShowLandingTerrain()
    {
        if(_landingTerrain == null || _placeIndicator == null)
        {
            return;
        }

        _landingTerrain.gameObject.SetActive(true);

        StartCoroutine(ExpandLandingTerrain());
    }

    IEnumerator ExpandLandingTerrain()
    {
        float _finalSize = _placeIndicator.GetConstantSizeOfMainScene();

        float _expandSpeed = _placeIndicator.GetExpandingAnimationSpeed();

        float _distance = _placeIndicator.GetPlaneDistance();

        float _c = (_expandSpeed * _finalSize) / _distance;

        Vector3 _size = Vector3.one;

        for(float _t = 0.0f; _t < (_finalSize * _distance); _t += (Time.deltaTime * _c))
        {
            _size = _size * _t;

            _landingTerrain.localScale = _size;

            yield return null;
        }

        _size = Vector3.one * _finalSize;

        _landingTerrain.localScale = _size;
    }

    public void SwitchOffAnimator()
    {
        _animator.enabled = false;
    }

    public void PlayFastyDialogue(string _input)
    {
        if(_fastyDialogues == null)
        {
            return;
        }

        _fastyDialogues.PlayClip(_input);
    }
}
