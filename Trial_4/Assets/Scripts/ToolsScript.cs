//using iTextSharp.text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public struct ToolsStruct
{
    public static bool CompareLetters(char _letter1Input, char _letter2Input, bool _caseInsensitive = true)
    {
        string _str1 = _letter1Input.ToString();

        string _str2 = _letter2Input.ToString();

        bool _finalBool = string.Compare(_str1, _str2, _caseInsensitive) == 0;

        return _finalBool;
    }

    public static Toggle GetSelectedToggle(ToggleGroup _input)
    {
        foreach(Toggle _t in _input.ActiveToggles())
        {
            if(_t.isOn)
            {
                return _t;
            }
        }

        return null;
    }

    public static void SetToNonCoterminalValue(ref float _input, bool _radiansInput = false)
    {
        float _base = (_radiansInput == true) ? Mathf.PI : 180.0f;

        if(_input > _base)
        {
            _input = _input - (_base * 2.0f);
        }
    }

    public static void SetToNonCoterminalValues(ref Vector3 _input, bool _radiansInput = false)
    {
        float _base = (_radiansInput == true) ? Mathf.PI : 180.0f;

        if (_input.x > _base)
        {
            _input.x = _input.x - (_base * 2.0f);
        }

        if(_input.y > _base)
        {
            _input.y = _input.y - (_base * 2.0f);
        }

        if(_input.z > _base)
        {
            _input.z = _input.z - (_base * 2.0f);
        }
    }

    public static void SetToNonCoterminalValues(ref Vector2 _input, bool _radiansInput = false)
    {
        float _base = (_radiansInput == true) ? Mathf.PI : 180.0f;

        if(_input.x > _base)
        {
            _input.x = _input.x - (_base * 2.0f);
        }

        if (_input.y > _base)
        {
            _input.y = _input.y - (_base * 2.0f);
        }
    }

    public static bool CheckParameterExistance(Animator _animatorInput, string _nameInput)
    {
        if(_animatorInput == null)
        {
            return false;
        }

        int _hash = Animator.StringToHash(_nameInput);

        List<int> _validParameters = new List<int>();

        for(int _i = 0; _i < _animatorInput.parameterCount; _i++)
        {
            _validParameters.Add(_animatorInput.parameters[_i].nameHash);
        }

        if(_validParameters.Contains(_hash))
        {
            return true;
        }

        return false;
    }

    public static bool CheckIfIndexExist<T>(List<T> _listInput, int _indexInput)
    {
        bool _evalBool = _indexInput >= 0;

        if(!_evalBool)
        {
            return false;
        }

        _evalBool = _indexInput < _listInput.Count;

        if(!_evalBool)
        {
            return false;
        }

        _evalBool = _listInput[_indexInput] != null;

        if(!_evalBool)
        {
            return false;
        }

        return true;
    }

    public static int GetCircleDivision(float _wholeCircleInput, int _countInput)
    {
        if(_wholeCircleInput < 360.0f)
        {
            return (_countInput - 1);
        }

        return _countInput;
    }

    public static Color ChangeColorValue(Color _colorInput, float _valueInput = 0.5f, float _alphaInput = 1.0f, bool _multiplyValueInput = true)
    {
        //Color _colorOutput = _colorInput;

        //float _h, _s, _v;

        Color.RGBToHSV(_colorInput, out float _h, out float _s, out float _v);

        if (_multiplyValueInput)
        {
            _v = _v * _valueInput;
        }
        else
        {
            _v = _valueInput;
        }

        Color _colorOutput = Color.HSVToRGB(_h, _s, _v);

        _colorOutput.a = _alphaInput;

        //_colorOutput = Color.HS

        return _colorOutput;
    }

    public static Vector3 GetGlobalScale(Transform _input)
    {
        GameObject _obj = GameObject.Instantiate(_input.gameObject);

        _obj.transform.parent = null;

        Vector3 _v3 = _obj.transform.localScale;

        GameObject.Destroy(_obj);

        return _v3;
    }

    public static void SetGlobalScale(Transform _transformInput, float _inputX, float _inputY, float _inputZ)
    {
        Transform _t = _transformInput.parent;

        _transformInput.parent = null;

        _transformInput.localScale = new Vector3(_inputX, _inputY, _inputZ);

        _transformInput.parent = _t;
    }

    public static void SetGlobalScale(Transform _transformInput, Vector3 _scaleInput)
    {
        SetGlobalScale(_transformInput, _scaleInput.x, _scaleInput.y, _scaleInput.z);
    }

    public static float GetVectorAverage(Vector3 _input)
    {
        float _v = _input.x + _input.y + _input.z;

        _v = _v / 3.0f;

        return _v;
    }
}
