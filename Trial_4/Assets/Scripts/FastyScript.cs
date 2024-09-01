using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Vector3 _defaultRotation;

    [SerializeField]
    Animator _defaultAnimator;

    [SerializeField]
    Animator _inhalerAnimator;

    [SerializeField]
    Canvas _mainPlayerCanvas;

    [SerializeField]
    MainPlayerCanvasScript _mainPlayerCanvasClass;

    [SerializeField]
    AnimationClip _animationClip;

    Coroutine _inhalerCoroutine;

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

            _fastyDefaultModel.transform.localRotation = Quaternion.identity;
        }

        if(_fastyInhalerModel != null)
        {
            _fastyInhalerModel.transform.localPosition = Vector3.zero;

            _fastyInhalerModel.transform.localRotation = Quaternion.identity;
        }
    }

    void RotateFasty()
    {
        if(_fastyContainer == null)
        {
            return;
        }

        if(!_rotate)
        {
            //_rotateToLookAtTarget = false;

            return;
        }

        if(!_rotateToLookAtTarget)
        {
            var _lookPos = _defaultRotation - _fastyContainer.transform.position;

            _lookPos.y = 0.0f;

            var _rot = Quaternion.LookRotation(_lookPos);

            _fastyContainer.transform.localRotation = Quaternion.Slerp(_fastyContainer.transform.localRotation, _rot, (_rotationSpeed * Time.deltaTime));

            return;
        }

        if (_camera != null)
        {
            var _lookPosCam = _camera.transform.position - _fastyContainer.transform.position;

            _lookPosCam.y = 0.0f;

            var _rotCam = Quaternion.LookRotation(-_lookPosCam);

            _fastyContainer.transform.localRotation = Quaternion.Slerp(_fastyContainer.transform.localRotation, _rotCam, (_rotationSpeed * Time.deltaTime));
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

        _inhalerCoroutine = StartCoroutine(PlayAnimation());
    }

    void AbortAnimaton()
    {
        if(_inhalerCoroutine != null)
        {
            StopCoroutine(_inhalerCoroutine);

            _inhalerCoroutine = null;
        }

        _fastyDefaultModel.SetActive(true);

        _fastyInhalerModel.SetActive(false);

        _playingInhaler = false;
    }

    IEnumerator PlayAnimation()
    {
        float _seconds = _animationClip.length;

        yield return new WaitForSeconds(_seconds);

        AbortAnimaton();
    }
}
