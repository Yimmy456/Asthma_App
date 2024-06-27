using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DraggableClass : MonoBehaviour
{
    static bool _touchingSomething = false;

    bool _dragged = false;

    Vector2 _onScreenPosition = Vector2.zero;

    PlayerController _controller;

    Vector2 _draggingVelocity = Vector2.zero;

    float _draggingSpeed = 0.0f;

    float _draggingDirection = -181.0f;

    UnityEngine.TouchPhase _touchPhase = UnityEngine.TouchPhase.Canceled;

    [SerializeField]
    bool _draggableOn = true;

    [SerializeField]
    Rigidbody _body;

    [SerializeField]
    Camera _camera;

    [SerializeField]
    bool _applyGravity;

    [SerializeField]
    float _zOffset;

    [SerializeField]
    bool _maintainZDistance = true;

    [SerializeField]
    DraggableTypeEnum _draggableType;

    // Start is called before the first frame update
    void Start()
    {
        if(_controller == null)
        {
            _controller = new PlayerController();
            _controller.Enable();
        }

        if (DraggableManagerClass.GetInstance() != null)
        {
            if (!DraggableManagerClass.GetInstance().GetDraggables().Contains(this))
            {
                DraggableManagerClass.GetInstance().AddDraggable(this);
            }
        }
    }

    private void OnEnable()
    {
        if(_controller == null)
        {
            _controller = new PlayerController();
            _controller.Enable();
        }

        if(DraggableManagerClass.GetInstance() != null)
        {
            if (!DraggableManagerClass.GetInstance().GetDraggables().Contains(this))
            {
                DraggableManagerClass.GetInstance().AddDraggable(this);
            }
        }
    }

    private void OnDestroy()
    {
        if(DraggableManagerClass.GetInstance() != null)
        {
            if(DraggableManagerClass.GetInstance().GetDraggables().Contains(this))
            {
                DraggableManagerClass.GetInstance().RemoveDraggable(this);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        DetectTouch();
    }

    void DetectTouch()
    {
        if(_camera == null || _controller == null || DraggableManagerClass.GetInstance() == null)
        {
            return;
        }

        if(_controller.Touch.TouchPress.WasPressedThisFrame())
        { 

            Vector2 _touchPosition = _controller.Touch.Position.ReadValue<Vector2>();

            if(Application.platform == RuntimePlatform.WindowsEditor)
            {
                _touchPosition = _controller.Move.CursorPosition.ReadValue<Vector2>();
            }

            Ray _ray = _camera.ScreenPointToRay(_touchPosition);

            RaycastHit _hit;

            if(Physics.Raycast(_ray, out _hit))
            {
                if(_hit.collider.transform == gameObject.transform && _draggableOn && DraggableManagerClass.GetInstance().GetDraggableOn())
                {
                    _dragged = true;
                    _touchingSomething = true;
                    _touchPhase = UnityEngine.TouchPhase.Began;
                    DraggableManagerClass.GetInstance().SetCurrentlyDraggedObject(this);

                    if (DraggableManagerClass.GetInstance().GetTouchPhase() == UnityEngine.TouchPhase.Ended || DraggableManagerClass.GetInstance().GetTouchPhase() == UnityEngine.TouchPhase.Canceled)
                    {
                        DraggableManagerClass.GetInstance().SetTouchPhase(UnityEngine.TouchPhase.Began);
                    }

                    DraggableManagerClass.GetInstance().AddToBeginsDragging(this);

                    Debug.Log("You clicked on " + @"""" + gameObject.name + @"""" + ".");
                }
                else
                {
                    Debug.Log("You clicked something (1)...");
                }
            }
            else
            {
                Debug.Log("You clicked something (2)...");
            }
        }
        else if(_touchPhase == UnityEngine.TouchPhase.Began)
        {
            _touchPhase = UnityEngine.TouchPhase.Moved;

            if(DraggableManagerClass.GetInstance().GetTouchPhase() != UnityEngine.TouchPhase.Moved)
            {
                DraggableManagerClass.GetInstance().SetTouchPhase(UnityEngine.TouchPhase.Moved);
            }

            DraggableManagerClass.GetInstance().AddToIsDragging(this);
        }

        if(_controller.Touch.TouchPress.WasReleasedThisFrame())
        {
            _touchingSomething = false;

            if(_dragged)
            {
                ResetValues();
            }
        }
        else if(_touchPhase == UnityEngine.TouchPhase.Ended)
        {
            _touchPhase = UnityEngine.TouchPhase.Canceled;

            if(DraggableManagerClass.GetInstance().GetTouchPhase() != UnityEngine.TouchPhase.Canceled)
            {
                DraggableManagerClass.GetInstance().SetTouchPhase(UnityEngine.TouchPhase.Canceled);
            }

            DraggableManagerClass.GetInstance().RemoveDraggable(this);
        }

        if(_dragged)
        {
            DragFunction();
        }
    }

    void DragFunction()
    {
        //1. Setting the Position of the Object.

        Vector3 _pos = _controller.Touch.Position.ReadValue<Vector2>();

        if(Application.platform == RuntimePlatform.WindowsEditor)
        {
            _pos = _controller.Move.CursorPosition.ReadValue<Vector2>();
        }

        if (_maintainZDistance)
        {
            _pos.z = _camera.WorldToScreenPoint(gameObject.transform.position).z + _zOffset;
        }
        else if(GetFront() != null)
        {
            _pos.z = _camera.WorldToScreenPoint(GetFront().position).z + _zOffset;
        }

        Vector3 _v3 = _camera.ScreenToWorldPoint(_pos);

        gameObject.transform.position = _v3;

        //2. Setting the Variables.

        _onScreenPosition = _pos;

        _draggingVelocity = _controller.Touch.Shake.ReadValue<Vector2>();

        _draggingSpeed = Mathf.Pow(_draggingVelocity.x, 2) + Mathf.Pow(_draggingVelocity.y, 2);

        _draggingSpeed = Mathf.Sqrt(_draggingSpeed);

        if(_draggingSpeed == 0.0f)
        {
            _draggingDirection = -181.0f;
        }
        else
        {
            _draggingDirection = Mathf.Atan2(_draggingVelocity.x, _draggingVelocity.y);

            _draggingDirection = _draggingDirection * Mathf.Rad2Deg;
        }
    }

    void ResetValues()
    {
        _dragged = false;

        _draggingVelocity = Vector2.zero;

        _draggingSpeed = 0.0f;

        _draggingDirection = -181.0f;

        _touchPhase = UnityEngine.TouchPhase.Ended;

        if (DraggableManagerClass.GetInstance().GetCurrentlyDraggedObject() == this)
        {
            DraggableManagerClass.GetInstance().SetCurrentlyDraggedObject(null);
        }

        if(DraggableManagerClass.GetInstance().GetTouchPhase() == UnityEngine.TouchPhase.Began || DraggableManagerClass.GetInstance().GetTouchPhase() == UnityEngine.TouchPhase.Moved)
        {
            DraggableManagerClass.GetInstance().SetTouchPhase(UnityEngine.TouchPhase.Ended);
        }

        DraggableManagerClass.GetInstance().AddToEndsDragging(this);

        if(_body != null)
        {
            _body.useGravity = _applyGravity;
        }    
    }

    public static bool GetTouchingSomething()
    {
        return _touchingSomething;
    }

    public bool GetDragged()
    {
        return _dragged;
    }

    public Vector2 GetOnScreenPosition()
    {
        return _onScreenPosition;
    }    

    public Vector2 GetDraggingVelocity()
    {
        return _draggingVelocity;
    }

    public float GetDraggingSpeed()
    {
        return _draggingSpeed;
    }

    public float GetDraggingDirection()
    {
        return _draggingDirection;
    }

    public Rigidbody GetBody()
    {
        return _body;
    }

    public bool GetApplyGravity()
    {
        return _applyGravity;
    }

    public bool GetDraggableOn()
    {
        return _draggableOn;
    }

    public UnityEngine.TouchPhase GetTouchPhase()
    {
        return _touchPhase;
    }

    public void SetBodyVelocity(Vector3 _v3Input, float _constantInput = 1.0f)
    {
        if (_body == null)
        {
            return;
        }

        Vector3 _vel = (_v3Input * _constantInput);

        _body.velocity = _vel;
    }

    public void SetBodyAngularVelocity(Vector3 _v3Input, float _constantInput = 1.0f)
    {
        if(_body == null)
        {
            return;
        }

        Vector3 _aVel = (_v3Input * _constantInput);

        _body.angularVelocity = _aVel;
    }

    public void SetBodyVelocity(float _inputX, float _inputY, float _inputZ, float _constantInput = 1.0f)
    {
        SetBodyVelocity(new Vector3(_inputX, _inputY, _inputZ), _constantInput);
    }

    public void SetBodyAngularVelocity(float _inputX, float _inputY, float _inputZ, float _constantInput = 1.0f)
    {
        SetBodyAngularVelocity(new Vector3(_inputX, _inputY, _inputZ), _constantInput);
    }

    public float GetDraggingDirection(bool _coterminalInput = true, bool _radiansInput = false)
    {
        if(_draggingDirection == -181.0f)
        {
            return -181.0f;
        }

        float _d = _draggingDirection;

        if(_coterminalInput && _d < 0.0f)
        {
            _d = _d + 360.0f;
        }

        if(_radiansInput)
        {
            _d = _d * Mathf.Deg2Rad;
        }

        return _d;
    }

    public DraggableTypeEnum GetDraggableType()
    {
        return _draggableType;
    }

    public void SetApplyGravity(bool _input)
    {
        _applyGravity = _input;
    }

    public bool DirectionWithinRange(float _min = 0.0f, bool _incMin = true, float _max = 0.0f, bool _incMax = true, bool _inRadians = false)
    {
        if(_draggingDirection == -181.0f)
        {
            return false;
        }

        if(_min > _max)
        {
            float _r = _min;
            _min = _max;
            _max = _r;
        }

        float _d = GetDraggingDirection(true, _inRadians);

        float _dir1 = _max;

        float _fullCircle = _inRadians ? (Mathf.PI * 2.0f) : 360.0f;

        if(_dir1 >= _fullCircle)
        {
            _dir1 = _dir1 - _fullCircle;
        }

        bool _bool1 = _incMax ? _d >= _dir1 : _d > _dir1;

        float _dir2 = _min;

        if(_dir2 < 0.0f)
        {
            _dir2 = _dir2 + _fullCircle;
        }

        bool _bool2 = _incMin ? _d <= _dir2 : _d < _dir2;

        bool _finalBool = _bool1 && _bool2;

        return _finalBool;
    }

    public bool GetMaintainZDistance()
    {
        return _maintainZDistance;
    }

    public float GetZOffset()
    {
        return _zOffset;
    }

    public Transform GetFront()
    {
        if(_camera == null)
        {
            return null;
        }

        Transform _t = _camera.transform.Find("Front");

        return _t;
    }

    public Camera GetCamera()
    {
        return _camera;
    }

    public void SetCamera(Camera _input)
    {
        _camera = _input;
    }

    public void SetDraggableOn(bool _input)
    {
        _draggableOn = _input;
    }

    public void RemoveRigidBody()
    {
        if(_body != null)
        {
            Destroy(_body);
        }
    }
}
