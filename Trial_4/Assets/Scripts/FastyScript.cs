using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastyScript : MonoBehaviour
{
    [SerializeField]
    GameObject _fastyModel;

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
    }

    void RotateFasty()
    {
        if(_fastyModel == null)
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
            var _lookPos = _defaultRotation - _fastyModel.transform.position;

            _lookPos.y = 0.0f;

            var _rot = Quaternion.LookRotation(_lookPos);

            _fastyModel.transform.localRotation = Quaternion.Slerp(_fastyModel.transform.localRotation, _rot, (_rotationSpeed * Time.deltaTime));

            return;
        }

        if (_camera != null)
        {
            var _lookPosCam = _camera.transform.position - _fastyModel.transform.position;

            _lookPosCam.y = 0.0f;

            var _rotCam = Quaternion.LookRotation(-_lookPosCam);

            _fastyModel.transform.localRotation = Quaternion.Slerp(_fastyModel.transform.localRotation, _rotCam, (_rotationSpeed * Time.deltaTime));
        }
    }

    public void SetRotate(bool _input)
    {
        _rotate = _input;
    }
}
