using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InhalerScript : MonoBehaviour
{
    [SerializeField]
    float _neededSpeed = 10.0f;

    [SerializeField]
    int _numberOfNeededShakes = 5;

    //float _speed = 0.0f;

    int _shakes = 0;

    bool _shakesDone = false;

    //PlayerController _controller;

    //bool _follow = false;

    [SerializeField]
    Text _text;

    //[SerializeField]
    //Camera _camera;

    //[SerializeField]
    //float _minimumTouchDistance = 5.0f;

    ///bool _touchStarted = false;

    bool _opp = false;

    [SerializeField]
    DraggableClass _draggableClass;

    // Start is called before the first frame update
    void Start()
    {
        /*if(_controller == null)
        {
            _controller = new PlayerController();
            _controller.Enable();
        }
        _controller.Touch.TouchItself.started += ctx => StartTouchFunction();
        _controller.Touch.TouchItself.performed += ctx => PerformedTouchFunction();
        _controller.Touch.TouchItself.canceled += ctx => EndTouchFunction();*/
    }

    // Update is called once per frame
    void Update()
    {
        /*if(/*_controller.Touch.Position.phase == InputActionPhase.Performed _controller.Touch.Position.WasPressedThisFrame() _controller.Touch.Position.IsPressed() _controller.Touch.TouchItself.phase == InputActionPhase.Started && _camera != null)
        {
            //Vector3 _touchPos = _controller.Touch.Position.ReadValue<Vector2>();

            Vector3 _touchPos = _controller.Touch.Position.ReadValue<Vector2>();

            Vector3 _inhalerPosition = gameObject.transform.position;

            _inhalerPosition = _camera.WorldToScreenPoint(_inhalerPosition);

            float _distance = Vector2.Distance(_touchPos, _inhalerPosition);

            if(_distance <= _minimumTouchDistance)
            {
                _follow = true;
            }

            if(_text != null)
            {
                _text.text = "(" + _touchPos.x.ToString("0.00") + ", " + _touchPos.y.ToString("0.00") + ")";
                if(_follow)
                {
                    _text.text = _text.text + ", following.";
                }
            }
        }
        else if(/*_controller.Touch.Position.phase == InputActionPhase.Canceled _controller.Touch.TouchItself.phase == InputActionPhase.Canceled_controller.Touch.Position.WasReleasedThisFrame())
        {
            _follow = false;
        }*/

        /*if(_controller.Touch.Position.phase == InputActionPhase.Started)
        {
            _follow = true;
        }
        else
        {
            _follow = false;
        }

        bool _p = _controller.Touch.TouchItself.WasPressedThisFrame() || _controller.Touch.TouchItself.IsPressed() || _controller.Touch.TouchItself.phase == InputActionPhase.Started;

        bool _r = _controller.Touch.TouchItself.WasReleasedThisFrame() || _controller.Touch.TouchItself.phase == InputActionPhase.Canceled;

        if(_controller.Touch.TouchItself.WasPressedThisFrame() _p)
        {
            if(_text != null)
            {
                _text.text = "Is touched.";
            }
        }

        if(_controller.Touch.TouchItself.WasReleasedThisFrame() _r)
        {
            if(_text != null)
            {
                _text.text = "Is released.";
            }
        }

        if(_controller.Touch.Click.phase == InputActionPhase.Started || _controller.Touch.Click.WasPressedThisFrame() || _controller.Touch.Click.IsPressed())
        {
            Vector2 _pos1 = _controller.Touch.Position.ReadValue<Vector2>();

            Vector3 _gameObjectPos = gameObject.transform.position;

            Vector2 _pos2 = new Vector2();

            _pos2.x = _gameObjectPos.x;

            _pos2.y = _gameObjectPos.y;

            _pos2 = _camera.WorldToScreenPoint(_pos2);

            float _distance = Vector2.Distance(_pos1, _pos2);

            if(_distance <= _minimumTouchDistance)
            {
                _follow = true;
            }

            if(_text != null)
            {
                _text.text = "The touch is done.";
            }
        }
        if(_controller.Touch.Click.phase == InputActionPhase.Canceled || _controller.Touch.Click.WasReleasedThisFrame())
        {
            _follow = false;
        }

        FollowTouch();*/

        /*Vector3 _tP = _camera.WorldToScreenPoint(transform.position);

        Vector3 _d = _controller.Touch.Position.ReadValue<Vector3>() - _tP;

        transform.position = _d;
        if (_controller.Touch.Click.WasPressedThisFrame() || _controller.Touch.Click.phase == InputActionPhase.Started || _controller.Touch.Click.IsPressed())
        {
            Vector3 _touchPosition = _controller.Touch.Position.ReadValue<Vector2>();

            Vector3 _worldP = _camera.ScreenToWorldPoint(_touchPosition);

            _worldP = _worldP + (_camera.transform.position * 100.0f);

            transform.position = _worldP;
        }*/

        //FollowTouch2();

        if(_draggableClass != null)
        {
            GetShakes();
        }
    }

    /*

    void FollowTouch2()
    {
        if((_controller.Touch.TouchItself.phase == InputActionPhase.Started || _controller.Touch.Position.phase == InputActionPhase.Started _controller.Touch.TouchItself.started) && _controller.Touch.TouchPhase.ReadValue<UnityEngine.TouchPhase>() == UnityEngine.TouchPhase.Began && _text != null && _touchStarted)
        {
            _text.text = "The touch has begun.";

            Vector2 _v2 = _controller.Touch.Position.ReadValue<Vector2>();

            Ray _ray = _camera.ScreenPointToRay(_v2);
            RaycastHit _hit;

            if(Physics.Raycast(_ray, out _hit))
            {
                if(_hit.collider.gameObject == gameObject || _hit.collider.tag == "Inhaler")
                {
                    _text.text = gameObject.name;                    
                }
            }

            //_touchStarted = true;

            if(_touchStarted)
            {
                Vector3 _v3 = _camera.ScreenToWorldPoint(_v2);

                gameObject.transform.position = _v3;
            }
        }
        /*
        else if((_controller.Touch.TouchItself.phase == InputActionPhase.Canceled || _controller.Touch.Position.phase == InputActionPhase.Canceled _controller.Touch.TouchItself.ReadValue<Touch>().phase == UnityEngine.TouchPhase.Ended || _controller.Touch.TouchItself.ReadValue<Touch>().phase == UnityEngine.TouchPhase.Canceled) && _touchStarted)
        {
            _touchStarted = false;
        }

        if(TouchManagerScript.GetInstance() == null)
        {
            return;
        }

        UnityEngine.TouchPhase _p = TouchManagerScript.GetInstance().GetPhase();

        //Vector3 _distance = Vector3.zero;

        if(/*_p == UnityEngine.TouchPhase.Began _controller.Touch.TouchPress.WasPressedThisFrame() && _text != null)
        {
            _text.text = "The touch has began at " + _controller.Touch.Position.ReadValue<Vector2>().ToString(".00") + ".";

            //Vector2 _v2 = _camera.ScreenToWorldPoint(_controller.Touch.Position.ReadValue<Vector2>());

            Ray _ray = _camera.ScreenPointToRay(_controller.Touch.Position.ReadValue<Vector2>());

            RaycastHit _hit;

            if(Physics.Raycast(_ray, out _hit))
            {
                if(_hit.collider.gameObject == gameObject || _hit.collider.tag == "Inhaler")
                {
                    _text.text = "The touch has hit the inhaler.";
                    //_distance = CalcOffset() - _controller.Touch.Position.ReadValue<Vector3>();
                    _follow = true;
                }
            }
        }
        else if(_controller.Touch.TouchPress.WasReleasedThisFrame())
        {
            _follow = false;
        }

        if(_follow)
        {
            Vector3 _p3 = _controller.Touch.Position.ReadValue<Vector2>();

            //_p3.z = _p3.z + 50000.0f;

            _p3.z = _camera.WorldToScreenPoint(gameObject.transform.position).z + 500.0f;

            Vector3 _v3 = _camera.ScreenToWorldPoint(_p3);

            //Vector3 _v3 = _controller.Touch.Position.ReadValue<Vector3>() - _distance;

            gameObject.transform.position = _v3;
        }
    }

    void FollowTouch()
    {
        if(_follow)
        {
            Vector3 _touchPos2 = _controller.Touch.Position.ReadValue<Vector2>();

            //_touchPos2.z = gameObject.transform.position.z;

            Vector3 _touchWorldPos = _camera.ScreenToWorldPoint(_touchPos2);

            _touchPos2.z = gameObject.transform.position.z;

            gameObject.transform.position = _touchWorldPos;
        }
    }

    void StartTouchFunction()
    {
        _touchStarted = true;
    }

    void PerformedTouchFunction()
    {
        _touchStarted = false;
    }

    void EndTouchFunction()
    {
        _touchStarted = false;
    }
    
    Vector3 CalcOffset()
    {
        if(_camera == null)
        {
            return Vector3.zero;
        }

        Vector3 _offset = _camera.WorldToScreenPoint(gameObject.transform.position);
        _offset.z = _camera.ScreenToWorldPoint(gameObject.transform.position).z;
        return _offset;
    }
    */
    void GetShakes()
    {
        if (!_draggableClass.GetDragged())
        {
            _shakes = 0;
            return;
        }

        Vector2 _delta = _draggableClass.GetDraggingVelocity();

        /*_speed = _delta.y;

        if ((_delta.y < -_neededSpeed && _opp || _delta.y > _neededSpeed && !_opp) && Mathf.Abs(_delta.x) < 10.0f)
        {
            _shakes++;
            _opp = !_opp;
            _shakesDone = (_shakes >= _numberOfNeededShakes * 2);
        }*/

        //float _speed = _draggableClass.GetDraggingSpeed();

        bool _correctDirection = _opp ? _draggableClass.DirectionWithinRange(-160.0f, true, 160.0f, true, false) : _draggableClass.DirectionWithinRange(-20.0f, true, 20.0f, true, false);

        if((_delta.y < -_neededSpeed && _opp || _delta.y > _neededSpeed && !_opp) && _correctDirection)
        {
            _shakes++;
            _opp = !_opp;
            _shakesDone = (_shakes >= (_numberOfNeededShakes * 2));
        }

        if(_shakesDone && _text != null)
        {
            _text.text = "You shaked the inhaler correctly!";
            _shakesDone = false;
            _shakes = 0;
        }
    }
}
