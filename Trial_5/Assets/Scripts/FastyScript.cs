using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastyScript : MonoBehaviour
{
    [SerializeField]
    GameObject _fastyContainer;

    [SerializeField]
    GameObject _fastyDefaultModel;

    [SerializeField]
    GameObject _fastyInhalerModel;

    [SerializeField]
    Transform _lookingModel;

    [SerializeField]
    bool _rotate;

    [SerializeField]
    bool _rotateToLookAtTarget;

    [SerializeField]
    Camera _camera;

    [SerializeField]
    float _rotationSpeed = 100.0f;

    [SerializeField]
    Vector3 _additionalRotationWhenLooking;

    [SerializeField]
    Animator _defaultAnimator;

    [SerializeField]
    Animator _inhalerAnimator;

    [SerializeField]
    MainPlayerCanvasScript _mainPlayerCanvasClass;

    [SerializeField]
    AnimationClip _animationClip;

    [SerializeField]
    DoctorTalkingScript _doctorTalking;

    [SerializeField]
    Text _countingText;

    [SerializeField]
    Outline _countingTextOutline;

    [SerializeField]
    Gradient _countingGradient;

    [SerializeField]
    DialoguesScript _dialogues;

    [SerializeField]
    float _animationDurationConstant = 1.0f;

    bool _startCounting = false;

    Coroutine _inhalerCoroutine;

    Coroutine _countingCoroutine;

    bool _playingInhaler = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        RotateFasty();

        if(_fastyDefaultModel != null)
        {
            _fastyDefaultModel.transform.localPosition = Vector3.zero;

            //_fastyDefaultModel.transform.localRotation = Quaternion.identity;
        }

        if(_fastyInhalerModel != null)
        {
            _fastyInhalerModel.transform.localPosition = Vector3.zero;

            //_fastyInhalerModel.transform.localRotation = Quaternion.identity;
        }
    }

    void RotateFasty()
    {
        if(_fastyContainer == null)
        {
            return;
        }

        /*if (_fastyDefaultModel != null)
        {
            _fastyDefaultModel.transform.localRotation = Quaternion.identity;
        }

        if (_fastyInhalerModel != null)
        {
            _fastyInhalerModel.transform.localRotation = Quaternion.identity;
        }*/

        if (_rotateToLookAtTarget)
        {
            var _lookPosCam = _camera.gameObject.transform.position - _fastyContainer.transform.position;

            _lookPosCam.y = 0.0f;

            var _rot = Quaternion.LookRotation(_lookPosCam);

            _rot = _rot * Quaternion.Euler(_additionalRotationWhenLooking);

            _fastyContainer.transform.rotation = Quaternion.Slerp(_fastyContainer.transform.rotation, _rot, Time.deltaTime);
        }
    }

    public void SetRotate(bool _input)
    {
        _rotate = _input;
    }

    public void PlayInhalerAnimation()
    {
        if(_fastyDefaultModel == null || _fastyInhalerModel == null || _playingInhaler || _defaultAnimator == null || _inhalerAnimator == null || _animationClip == null)
        {
            return;
        }

        _fastyInhalerModel.SetActive(true);

        _fastyDefaultModel.SetActive(false);

        _playingInhaler = true;

        _mainPlayerCanvasClass.HideButtonsImmediately2();

        _mainPlayerCanvasClass.GetMenuButton().gameObject.SetActive(false);

        _inhalerCoroutine = StartCoroutine(PlayAnimation());
    }

    void AbortAnimaton()
    {
        if(_inhalerCoroutine != null)
        {
            StopCoroutine(_inhalerCoroutine);

            _inhalerCoroutine = null;
        }

        if(_countingCoroutine != null)
        {
            StopCoroutine(_countingCoroutine);

            _countingCoroutine = null;

            _countingText.text = "";
        }

        _fastyDefaultModel.SetActive(true);

        _fastyInhalerModel.SetActive(false);

        _mainPlayerCanvasClass.GetMenuButton().gameObject.SetActive(true);

        if(_countingText != null)
        {
            _countingText.text = "";
        }

        _playingInhaler = false;
    }

    IEnumerator PlayAnimation()
    {
        //float _s = 5.0f;

        float _seconds = _animationClip.length;

        if(_doctorTalking != null)
        {
            _doctorTalking.StartTalking(_seconds);
        }

        /*yield return new WaitForSeconds(_s);

        _inhalerAnimator.speed = 0;

        yield return new WaitForSeconds(_seconds - _s);*/

        yield return new WaitForSeconds(_seconds * _animationDurationConstant);

        AbortAnimaton();
    }

    public void CountToTenFunction()
    {
        if(_countingText == null)
        {
            _startCounting = false;

            return;
        }

        if(!_startCounting)
        {
            return;
        }

        _countingCoroutine = StartCoroutine(CountingCoroutine());

        _startCounting = false;
    }

    IEnumerator CountingCoroutine()
    {
        int _t = 0;

        Color _textC;

        Color _outlineC;

        float _ratio;

        _inhalerAnimator.speed = 1;

        for(float _f = 0.0f; _f < 10.0f; _f += Time.deltaTime)
        {
            _t = (int)_f + 1;

            _countingText.text = _t.ToString();

            _ratio = (_t - 1) / 9.0f;

            _textC = _countingGradient.Evaluate(_ratio);

            _outlineC = _textC;

            _outlineC = ToolsStruct.ChangeColorValue(_textC, 0.5f, 0.5f, false);

            _countingText.color = _textC;

            _countingTextOutline.effectColor = _outlineC;

            yield return null;
        }

        _countingText.text = "";
    }

    public DialoguesScript GetDialogues()
    {
        return _dialogues;
    }
}
