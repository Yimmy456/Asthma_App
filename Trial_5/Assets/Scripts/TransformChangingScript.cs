using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformChangingScript : MonoBehaviour
{
    [SerializeField]
    protected bool _doAction;

    [SerializeField]
    protected float _constant = 1.0f;

    [SerializeField]
    protected Vector3 _axis = Vector3.zero;

    [SerializeField]
    protected Transform _transform;

    [SerializeField]
    protected bool _deltaTimeX = true;

    [SerializeField]
    protected bool _deltaTimeY = true;

    [SerializeField]
    protected bool _deltaTimeZ = true;

    [SerializeField]
    protected Space _space = Space.World;

    protected Vector3 _finalResult;

    protected Vector3 _finalResultWithoutDeltaTime;

    protected bool _changeTransform = true;

    protected Transform _finalTransform;

    // Start is called before the first frame update
    void Start()
    {
        SetNullValues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        SetNullValues();
    }

    //Getters

    public bool GetDoAction()
    {
        return _doAction;
    }

    public float GetConstant()
    {
        return _constant;
    }

    public Vector3 GetAxis()
    {
        return _axis;
    }

    public Transform GetTransform()
    {
        return _transform;
    }

    public bool GetDeltaTimeX()
    {
        return _deltaTimeX;
    }

    public bool GetDeltaTimeY()
    {
        return _deltaTimeY;
    }

    public bool GetDeltaTimeZ()
    {
        return _deltaTimeZ;
    }

    public bool GetDeltaTime(bool _allInput = false)
    {
        if (_allInput)
        {
            return (_deltaTimeX && _deltaTimeY && _deltaTimeZ);
        }

        return (_deltaTimeX || _deltaTimeY || _deltaTimeZ);
    }

    public Space GetSpace()
    {
        return _space;
    }

    public Vector3 GetFinalResult()
    {
        return _finalResult;
    }

    public Vector3 GetFinalResultWithoutDeltaTime()
    {
        return _finalResultWithoutDeltaTime;
    }

    //Setters

    public void SetDoAction(bool _input)
    {
        _doAction = _input;
    }

    public void SetConstant(float _input)
    {
        _constant = _input;
    }

    public void SetAxis(float _inputX, float _inputY, float _inputZ)
    {
        _axis.x = _inputX;

        _axis.y = _inputY;

        _axis.z = _inputZ;
    }

    public void SetAxis(Vector3 _input)
    {
        SetAxis(_input.x, _input.y, _input.z);
    }

    public void SetAxisX(float _input)
    {
        _axis.x = _input;
    }

    public void SetAxisY(float _input)
    {
        _axis.y = _input;
    }

    public void SetAxisZ(float _input)
    {
        _axis.z = _input;
    }

    public void SetDeltaTimeX(bool _input)
    {
        _deltaTimeX = _input;
    }

    public void SetDeltaTimeY(bool _input)
    {
        _deltaTimeY = _input;
    }

    public void SetDeltaTimeZ(bool _input)
    {
        _deltaTimeZ = _input;
    }

    public void SetDeltaTime(bool _input)
    {
        _deltaTimeX = _input;

        _deltaTimeY = _input;

        _deltaTimeZ = _input;
    }

    public void SetSpace(Space _input)
    {
        _space = _input;
    }

    public void SetSpace(int _input)
    {
        Space _v;

        switch(_input)
        {
            case 0:
                _v = Space.World;
                break;
            default:
                _v = Space.Self;
                break;
        }

        SetSpace(_v);
    }

    protected virtual void DoActionFunction()
    {
        if(!_doAction)
        {
            _finalResult = Vector3.zero;

            _finalResultWithoutDeltaTime = Vector3.zero;

            _changeTransform = false;

            return;
        }

        _finalResult = _axis * _constant;

        _finalResultWithoutDeltaTime = _finalResult;

        if(_deltaTimeX)
        {
            _finalResult.x = _finalResult.x * Time.deltaTime;
        }

        if(_deltaTimeY)
        {
            _finalResult.y = _finalResult.y * Time.deltaTime;
        }

        if(_deltaTimeZ)
        {
            _finalResult.z = _finalResult.z * Time.deltaTime;
        }

        if(_transform == null)
        {
            _finalTransform = gameObject.transform;
        }
        else
        {
            _finalTransform = _transform;
        }

        _changeTransform = true;
    }

    protected void SetNullValues()
    {
        if(_finalResult == null)
        {
            _finalResult = Vector3.zero;
        }

        if(_finalResultWithoutDeltaTime == null)
        {
            _finalResultWithoutDeltaTime = Vector3.zero;
        }
    }
}
