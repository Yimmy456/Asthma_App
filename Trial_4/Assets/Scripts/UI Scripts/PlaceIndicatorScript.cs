using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlaceIndicatorScript : MonoBehaviour
{
    private ARRaycastManager _raycastManager;
    private GameObject _indicator;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    [SerializeField]
    float _speed = 100.0f;

    [SerializeField]
    Button _startButton;

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

    // Start is called before the first frame update
    void Start()
    {
        _raycastManager = FindObjectOfType<ARRaycastManager>();
        _indicator = transform.GetChild(0).gameObject;
        _indicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        var _ray = new Vector2(Screen.width / 2, Screen.height / 2);

        if (_action != null)
        {
            _startButton.onClick.RemoveListener(_action);
        }

        if(_raycastManager.Raycast(_ray, _hits, TrackableType.Planes) && !_started)
        {
            Pose _hitPose = _hits[0].pose;

            transform.position = _hitPose.position;
            transform.rotation = _hitPose.rotation;

            _indicator.SetActive(true);

            RotateIndicator();

            if(_startButton != null)
            {
                _startButton.gameObject.SetActive(true);

                //_action = delegate { SpawnObject(_hitPose.position); };

                //_startButton.onClick.AddListener(delegate { SpawnObject(_hitPose.position); });

                _action = delegate { _started = true; };

                _startButton.onClick.AddListener(_action);
            }

            if(_canvasGroup != null)
            {
                _canvasGroup.alpha = 1.0f;

                _canvasGroup.interactable = true;
            }

            if(_inhaler != null)
            {
                _inhaler.SetActive(true);
            }
        }
        else
        {
            if (_started)
            {
                Material _m = _indicator.GetComponent<MeshRenderer>().materials[0];

                _m.SetColor("_Color", Color.green);

                RotateIndicator();
            }
            else
            {
                _indicator.SetActive(false);

                if(_inhaler != null)
                {
                    _inhaler.SetActive(false);
                }
            }

            if(_startButton != null)
            {
                _startButton.gameObject.SetActive(false);
            }
        }
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
}
