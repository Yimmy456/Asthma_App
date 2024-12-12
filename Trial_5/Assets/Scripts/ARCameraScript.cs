using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARCameraScript : MonoBehaviour
{
    [SerializeField]
    Camera _camera;

    [SerializeField]
    ARPoseDriver _poseDriver;

    [SerializeField]
    ARCameraManager _cameraManager;

    [SerializeField]
    ARCameraBackground _background;

    [SerializeField]
    ARSessionOrigin _sessionOrigin;

    [SerializeField]
    ARRaycastManager _raycastManager;

    [SerializeField]
    ARPlaneManager _planeManager;

    [SerializeField]
    GameObject _indicatorObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOnAR(bool _input = true)
    {
        if(_poseDriver != null)
        {
            _poseDriver.enabled = _input;
        }

        if(_cameraManager != null)
        {
            _cameraManager.enabled = _input;
        }

        if(_sessionOrigin != null)
        {
            _sessionOrigin.enabled = _input;
        }

        if(_background != null)
        {
            _background.enabled = _input;
        }

        if(_planeManager != null)
        {
            _planeManager.enabled = _input;
        }

        if(_raycastManager != null)
        {
            _raycastManager.enabled = _input;
        }

        if(_indicatorObject != null)
        {
            _indicatorObject.SetActive(_input);
        }
    }
}
