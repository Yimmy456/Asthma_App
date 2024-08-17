using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSMovingScript : MonoBehaviour
{
    [SerializeField]
    Camera _camera;

    [SerializeField]
    float _movingSpeed = 10.0f;

    [SerializeField]
    float _lookingSpeed = 5.0f;

    [SerializeField]
    Transform _transform;

    [SerializeField]
    Transform _rotator;

    [SerializeField]
    Transform _hingeTransform;

    [SerializeField]
    float _minAngle = -20.0f;

    [SerializeField]
    float _maxAngle = 40.0f;

    [SerializeField]
    PlayerController _controller;

    // Start is called before the first frame update
    void Start()
    {
        if(_controller == null)
        {
            _controller = new PlayerController();

            _controller.Enable();
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        LookAround();
    }

    void MovePlayer()
    {
        if (_controller == null || _transform == null || _camera == null)
        {
            return;
        }

        Vector3 _directionInputV3 = _controller.Move.Move.ReadValue<Vector3>();

        Vector3 _direction = (_camera.transform.right * _directionInputV3.x) + (_camera.transform.up * _directionInputV3.y) + (_camera.transform.forward * _directionInputV3.z);

        Vector3 _finalMovingVelocity = _direction * _movingSpeed * Time.deltaTime;

        Vector3 _pos = _transform.position;

        _pos = _pos + _finalMovingVelocity;

        _transform.position = _pos;
    }

    void LookAround()
    {
        if(_camera == null || _controller == null || _rotator == null)
        {
            return;
        }

        if(_controller.Move.LookTrigger.phase != InputActionPhase.Performed)
        {
            return;
        }

        Vector2 _mouseDelta = _controller.Move.Look.ReadValue<Vector2>();

        _mouseDelta = _mouseDelta * _lookingSpeed * Time.deltaTime;

        Vector3 _looking = _rotator.localEulerAngles;

        _looking.y = _looking.y + _mouseDelta.x;

        _rotator.localEulerAngles = _looking;

        if(_hingeTransform != null)
        {
            Vector3 _lt = _hingeTransform.localEulerAngles;

            if(_lt.x > 180.0f)
            {
                _lt.x = _lt.x - 360.0f;
            }

            _lt.x = _lt.x - _mouseDelta.y;

            if(_lt.x > _maxAngle)
            {
                _lt.x = _maxAngle;
            }

            if(_lt.x < _minAngle)
            {
                _lt.x = _minAngle;
            }

            _hingeTransform.localEulerAngles = _lt;
        }
    }
}
