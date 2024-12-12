using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatLookingScript : MonoBehaviour
{
    [SerializeField]
    Transform _hingeTransform;

    [SerializeField]
    Vector2 _minRotation;

    [SerializeField]
    Vector2 _maxRotation;

    Quaternion _baseRotation = new Quaternion();

    PlayerController _controller;

    Vector3 _rotation = new Vector3();

    [SerializeField]
    bool _lookAround;

    [SerializeField]
    bool _inverseXControl;

    [SerializeField]
    bool _inverseYControl;

    [SerializeField]
    bool _swapAxes = true;

    [SerializeField]
    bool _deltaTimeX = true;

    [SerializeField]
    bool _deltaTimeY = true;

    [SerializeField]
    float _constant = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        Initiate();
    }

    private void OnEnable()
    {
        Initiate();
    }

    // Update is called once per frame
    void Update()
    {
        LookAroundFunction();
    }

    public Quaternion GetBaseRotation()
    {
        return _baseRotation;
    }

    public Transform GetHingeTransform()
    {
        return _hingeTransform;
    }

    public Vector2 GetMinRotation()
    {
        return _minRotation;
    }

    public Vector2 GetMaxRotation()
    {
        return _maxRotation;
    }

    public Vector3 GetRotation()
    {
        return _rotation;
    }

    public bool GetLookAraound()
    {
        return _lookAround;
    }

    public void SetLookAround(bool _input)
    {
        _lookAround = _input;
    }

    public bool GetInverseXControl()
    {
        return _inverseXControl;
    }

    public bool GetInverseYControl()
    { 
        return _inverseYControl;
    }

    public bool GetDeltaTimeX()
    {
        return _deltaTimeX;
    }

    public bool GetDeltaTimeY()
    {
        return _deltaTimeY;
    }


    public void SetDeltaTimeX(bool _input)
    {
        _deltaTimeX = _input;
    }

    public void SetDeltaTimeY(bool _input)
    {
        _deltaTimeY = _input;
    }

    public void SetDeltaTime(bool _input)
    {
        _deltaTimeX = _input;

        _deltaTimeY = _input;
    }

    public bool GetInverseControls(bool _input = false)
    {
        if(_input == true)
        {
            return (_inverseXControl && _inverseYControl);
        }

        return (_inverseXControl || _inverseYControl);
    }

    public void SetInverseXControl(bool _input)
    {
        _inverseXControl = _input;
    }

    public void SetInverseYControl(bool _input)
    {
        _inverseYControl = _input;
    }

    public void SetInverseControls(bool _input)
    {
        _inverseXControl = _input;

        _inverseYControl = _input;
    }

    void Initiate()
    {
        _baseRotation = _hingeTransform.localRotation;

        if(_controller == null)
        {
            _controller = new PlayerController();

            _controller.Enable();
        }

        if(_minRotation.x > _maxRotation.x)
        {
            float _vx = _minRotation.x;

            _minRotation.x = _maxRotation.x;

            _maxRotation.x = _vx;
        }

        if(_minRotation.y > _maxRotation.y)
        {
            float _vy = _minRotation.y;

            _minRotation.y = _maxRotation.y;

            _maxRotation.y = _vy;
        }
    }

    public void ResetValues()
    {
        _hingeTransform.localRotation = _baseRotation;

        _rotation = Vector3.zero;
    }

    void LookAroundFunction()
    {
        if(_hingeTransform == null)
        {
            return;
        }

        if(!_lookAround || _controller == null)
        {
            ResetValues();

            return;
        }

        if(Application.platform == RuntimePlatform.WindowsEditor)
        {
            if(_controller.Touch.Click.phase != UnityEngine.InputSystem.InputActionPhase.Performed)
            {
                return;
            }
        }

        Vector2 _addedValue = _controller.Touch.Swipe.ReadValue<Vector2>() * _constant;

        //Evaluate the values
        EvaluateValuesFunction(ref _addedValue);

        Vector2 _currentAngles = _hingeTransform.localEulerAngles;

        ToolsStruct.SetToNonCoterminalValues(ref _currentAngles);

        Vector2 _nextAngles = _currentAngles + _addedValue;

        //Maintain the values
        MaintainValuesFunction(ref _nextAngles);

        _rotation = _nextAngles;

        _hingeTransform.localEulerAngles = _rotation;
    }

    void EvaluateValuesFunction(ref Vector2 _input)
    {
        if (_inverseXControl)
        {
            _input.x = (_input.x * (-1.0f));
        }

        if (_inverseYControl)
        {
            _input.y = (_input.y * (-1.0f));
        }

        if (_deltaTimeX)
        {
            _input.x = (_input.x * Time.deltaTime);
        }

        if (_deltaTimeY)
        {
            _input.y = (_input.y * Time.deltaTime);
        }

        if (_swapAxes)
        {
            float _sv = _input.x;

            _input.x = _input.y;

            _input.y = _sv;
        }
    }

    void MaintainValuesFunction(ref Vector2 _input)
    {
        if (_input.x < _minRotation.x)
        {
            _input.x = _minRotation.x;
        }

        if (_input.x > _maxRotation.x)
        {
            _input.x = _maxRotation.x;
        }

        if (_input.y < _minRotation.y)
        {
            _input.y = _minRotation.y;
        }

        if (_input.y > _maxRotation.y)
        {
            _input.y = _maxRotation.y;
        }
    }
}
