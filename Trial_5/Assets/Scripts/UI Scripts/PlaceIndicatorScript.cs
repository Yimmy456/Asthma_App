using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Threading;

public class PlaceIndicatorScript : MonoBehaviour
{
    private ARRaycastManager _raycastManager;

    [SerializeField]
    private GameObject _indicator;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    [SerializeField]
    float _speed = 100.0f;

    [SerializeField]
    Button _startButton;

    [SerializeField]
    Button _noLandButton;

    [SerializeField]
    GameObject _prefab;

    bool _started = false;

    [SerializeField]
    float _scale = 1.0f;

    [SerializeField]
    GameObject _plane;

    [SerializeField]
    CanvasGroup _canvasGroup;

    UnityAction _action;

    [SerializeField]
    GameObject _inhaler;

    [SerializeField]
    Animator _startingAnimator;

    [SerializeField]
    Vector3 _inhalerLocalPosition;

    [SerializeField]
    Camera _camera;

    [SerializeField]
    Text _distanceText;

    [SerializeField]
    Transform _mainSceneTransform;

    [SerializeField]
    float _constantSizeOfMainScene = 0.1f;

    [SerializeField]
    float _expandingAnimationSpeed = 0.5f;

    [SerializeField]
    Transform _front;

    [SerializeField]
    float _frontDistance = 5.0f;

    [SerializeField]
    ShrinkAndExpandAnimationClass _shinkAndExpandAnimationForLandingTerrain;

    [SerializeField]
    GameObject _pointingArrowGameObject;

    [SerializeField]
    ArrowAnimationClass2 _arrowAnimationClass;

    float _planeDistance = -1.0f;

    bool _lookForTerrainBool = false;

    bool _terrainFound = false;

    float _showingT = 0.0f;

    Coroutine _animationCoroutine;

    Coroutine _startingAnimationCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        _raycastManager = FindObjectOfType<ARRaycastManager>();
        //_indicator = transform.GetChild(0).gameObject;
        //_indicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            LookForTerrain();
        }
    }

    void LookForTerrain()
    {
        if(!_lookForTerrainBool)
        {
            if (_indicator != null)
            {
                _indicator.SetActive(false);
            }

            if(_mainSceneTransform != null)
            {
                _mainSceneTransform.gameObject.SetActive(false);
            }

            if (_noLandButton != null)
            {
                _noLandButton.gameObject.SetActive(false);
            }

            if (_startButton != null)
            {
                _startButton.gameObject.SetActive(false);
            }

            return;
        }
        else
        {
            if (_indicator != null)
            {
                _indicator.SetActive(true);
            }

            if(_mainSceneTransform != null)
            {
                _mainSceneTransform.gameObject.SetActive(true);
            }
        }

        var _ray = new Vector2(Screen.width / 2, Screen.height / 2);

        if (_action != null)
        {
            _startButton.onClick.RemoveListener(_action);
        }

        if (_indicator != null)
        {
            Material _m = _indicator.GetComponent<MeshRenderer>().materials[0];
        }

        if (_raycastManager.Raycast(_ray, _hits, TrackableType.Planes) && !_started)
        {
            int _hitIndex = _hits.Count - 1;

            Pose _hitPose = _hits[_hitIndex].pose;

            _planeDistance = Vector3.Distance(_hitPose.position, _camera.gameObject.transform.position);

            transform.position = _hitPose.position;
            transform.rotation = _hitPose.rotation;

            if (_indicator != null)
            {
                _indicator.SetActive(true);

                _indicator.transform.localScale = Vector3.one * _planeDistance;
            }

            if(_shinkAndExpandAnimationForLandingTerrain.GetT() < 1.0f && _shinkAndExpandAnimationForLandingTerrain.GetIncreaseOrDecrease() != IncreasOrDecreaseEnum.Increase)
            {
                //_landingTerrain.SetActive(true);

                if(_animationCoroutine != null)
                {
                    StopCoroutine(_animationCoroutine);
                }

                _animationCoroutine = StartCoroutine(_shinkAndExpandAnimationForLandingTerrain.Animate());                
            }

            //StartCoroutine(_arrowAnimationClass.Animate());

            SetSceneSize();

            _terrainFound = true;

            _distanceText.text = "Distance: " + _planeDistance.ToString("0.00");

            //RotateIndicator();

            if (_startButton != null)
            {
                _startButton.gameObject.SetActive(true);

                _action = delegate { _started = true; CallToStartAnimation(); };

                _startButton.onClick.AddListener(_action);
            }

            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 1.0f;

                _canvasGroup.interactable = true;
            }

            if (_noLandButton != null)
            {
                _noLandButton.gameObject.SetActive(false);
            }

            //_m.SetColor("_Color", Color.green);

            //_raycastManager.
        }
        else if (!_started)
        {
            //_indicator.SetActive(false);

            if (_inhaler != null)
            {
                _inhaler.SetActive(false);
            }

            //_arrowAnimationClass.EndAnimation();

            //_m.SetColor("_Color", Color.red);

            //if(_landingTerrain != null)
            //{
            //    _landingTerrain.SetActive(false);
            //}

            _terrainFound = false;

            if (_shinkAndExpandAnimationForLandingTerrain.GetT() > 0.0f && _shinkAndExpandAnimationForLandingTerrain.GetIncreaseOrDecrease() != IncreasOrDecreaseEnum.Decrease)
            {
                //_landingTerrain.SetActive(true);

                if(_animationCoroutine != null)
                {
                    StopCoroutine(_animationCoroutine);
                }

                _animationCoroutine = StartCoroutine(_shinkAndExpandAnimationForLandingTerrain.ReverseAnimate());
            }


            _distanceText.text = "";

            if (_startButton != null)
            {
                _startButton.gameObject.SetActive(false);
            }

            if (_noLandButton != null)
            {
                _noLandButton.gameObject.SetActive(true);
            }

            _planeDistance = -1.0f;
        }

        //RotateIndicator();
    }

    void RotateIndicator()
    {
        float _finalSpeed = _speed * Time.deltaTime;

        Vector3 _rotV3 = _indicator.transform.localEulerAngles;

        _rotV3.y = _rotV3.y + _finalSpeed;

        Quaternion _rot = Quaternion.Euler(_rotV3);

        //_indicator.transform.Rotate((Vector3.up * _finalSpeed));

        _indicator.transform.localRotation = _rot;
    }

    public void RestartLanding()
    {
        _started = false;

        if(_startingAnimator != null)
        {
            _startingAnimator.Play("Rocket Landing Animation 2", -1, 0);
        }
    }

    public void ResetInhalerPosition()
    {
        _inhaler.transform.localPosition = _inhalerLocalPosition;
    }

    public float GetPlaneDistance()
    {
        return _planeDistance;
    }

    public float GetConstantSizeOfMainScene()
    {
        return _constantSizeOfMainScene;
    }

    public float GetExpandingAnimationSpeed()
    {
        return _expandingAnimationSpeed;
    }

    public GameObject GetIndicator()
    {
        return _indicator;
    }

    public ShrinkAndExpandAnimationClass GetShinkAndExpandAnimationForLandingTerrain()
    {
        return _shinkAndExpandAnimationForLandingTerrain;
    }

    public GameObject GetPointingArrowGameObject()
    {
        return _pointingArrowGameObject;
    }

    public void SetIndicatorOn(bool _input = true)
    {
        _indicator.gameObject.SetActive(_input);
    }

    public void SetLookForTerrainBool(bool _input)
    {
        _lookForTerrainBool = _input;
    }

    public void SetSceneSize()
    {
        if(_mainSceneTransform == null)
        {
            return;
        }

        Vector3 _size = Vector3.one * _constantSizeOfMainScene;

        _mainSceneTransform.localScale = _size;

        if(_front == null)
        {
            return;
        }

        Vector3 _frontDistanceV3 = Vector3.zero;

        _frontDistanceV3.z = _frontDistance;

        _front.localPosition = _frontDistanceV3;
    }

    void CallToStartAnimation()
    {
        if(_mainSceneTransform == null)
        {
            Debug.LogError("There is nothing to expand.");

            return;
        }

        _startingAnimationCoroutine = StartCoroutine(StartingGameCoroutine());
    }

    IEnumerator StartingGameCoroutine()
    {
        Vector3 _v31 = Vector3.one * _constantSizeOfMainScene;

        Vector3 _v32 = Vector3.one * _constantSizeOfMainScene * _planeDistance;

        Vector3 _v3C = _v31;

        for(float _t = 0.0f; _t < 1.0f; _t += (Time.deltaTime * _expandingAnimationSpeed))
        {
            if(_t >= 1.0f)
            {
                _t = 1.0f;

                Debug.Log("'_t' is now '1', and the expansion is complete.");
            }

            _v3C = Vector3.Lerp(_v31, _v32, _t);

            _mainSceneTransform.localScale = _v3C;

            Debug.Log("'_t' is now '" + _t.ToString() + "', and _v3C is now " + _v3C.ToString() + ".");

            yield return null;
        }
    }

    /*IEnumerator AnimatePointingArrow()
    {
        if (_pointingArrowGameObject == null)
        {
            yield break;
        }

        float _currentPosY = _pointingArrowGameObject.transform.localPosition.y;

        Vector3 _pos = _pointingArrowGameObject.transform.localPosition;

        _pointingArrowGameObject.SetActive(true);

        bool _landedArrow = false;

        while(_currentPosY > -500.0f && _terrainFound && !_landedArrow)
        {
            _currentPosY = _currentPosY - (Time.deltaTime * 300.0f);

            if(_currentPosY <= -500.0f)
            {
                _currentPosY = -500.0f;

                _landedArrow = true;
            }

            _pos.y = _currentPosY;

            _pointingArrowGameObject.transform.localPosition = _pos;

            yield return null;
        }

        bool _switch = false;

        float _t = 0.0f;

        Vector3 _pointA = new Vector3(0.0f, -400.0f, 0.0f);

        Vector3 _pointB = new Vector3(0.0f, -500.0f, 0.0f);

        Vector3 _V3Lerp = _pointA;

        Vector3 _currentPoint = _pointA;

        Vector3 _nextPoint = _pointB;

        while(_terrainFound && _landedArrow)
        {
            _t = _t + Time.deltaTime;

            if(_t >= 1.0f)
            {
                _t = 1.0f;

                _switch = true;
            }

            _V3Lerp = Vector3.Lerp(_currentPoint, _nextPoint, _t);

            _currentPosY = _V3Lerp.y;

            _pointingArrowGameObject.transform.localPosition = _V3Lerp;

            if(_switch)
            {
                Vector3 _v3 = _nextPoint;

                _nextPoint = _currentPoint;

                _currentPoint = _v3;

                _switch = false;

                _t = 0.0f;
            }

            yield return null;
        }

        while (_currentPosY < 0.0f && !_terrainFound)
        {
            _currentPosY = _currentPosY + (Time.deltaTime * 300.0f);

            if (_currentPosY >= 0.0f)
            {
                _currentPosY = 0.0f;
            }

            _pos.y = _currentPosY;

            _pointingArrowGameObject.transform.localPosition = _pos;

            yield return null;
        }

        _pointingArrowGameObject.SetActive(false);
    }*/
}
