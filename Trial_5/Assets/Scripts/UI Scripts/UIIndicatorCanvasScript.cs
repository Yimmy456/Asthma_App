using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class UIIndicatorCanvasScript : MonoBehaviour
{
    [SerializeField]
    GameObject _targetObject;

    [SerializeField]
    GameObject _arrow;

    [SerializeField]
    Camera _camera;

    [SerializeField]
    Text _resolutionText;

    [SerializeField]
    Sprite _cursorSprite;

    [SerializeField]
    Sprite _arrowSprite;

    [SerializeField]
    float _onscreenDistanceThreshold = 100.0f;

    [SerializeField]
    Text _meterText;

    [SerializeField]
    Vector2 _onscreenOffset;

    [SerializeField]
    Color _indicatorColor;

    bool _on;

    float _threshold;

    bool _targetInSight;

    //Vector2 _anchoredPosition = new Vector2(0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LookForObject();

        if(_resolutionText != null)
        {
            _resolutionText.text = "(" + Screen.width + ", " + Screen.height + ")";
        }
    }

    public Color GetIndicatorColor()
    {
        return _indicatorColor;
    }

    public GameObject GetTargetObject()
    {
        return _targetObject;
    }

    public bool GetTargetInSight()
    {
        if(_targetObject == null)
        {
            return false;
        }

        return _targetInSight;
    }

    public void SetIndicatorColor(Color _input)
    {
        _indicatorColor = _input;
    }

    public void SetTargetObject(GameObject _input)
    {
        _targetObject = _input;
    }

    public void SetColorForMeterText(Color _input)
    {
        if(_meterText == null)
        {
            return;
        }

        _meterText.color = _input;        

        float _h, _s, _v;

        Color.RGBToHSV(_input, out _h, out _s, out _v);

        _v = _v / 2.0f;

        Color _outlineColor = Color.HSVToRGB(_h, _s, _v);

        _meterText.gameObject.GetComponent<Outline>().effectColor = _outlineColor;
    }

    void LookForObject()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("We are looking for the area...");
        }

        if(_targetObject == null || _arrow == null || _camera == null)
        {
            //Debug.Log("We are returning at 1.");

            return;
        }

        _arrow.SetActive(false);

        _on = false;

        _threshold = 0.65f;

        if(SettingsManager.GetInstance() != null)
        {
            bool _continue = GetValuesFromSettings(ref _on, ref _threshold);

            if(!_continue)
            {
                Debug.Log("We are returning at 2.");

                return;
            }
        }

        if(!_targetObject.activeSelf || !_on)
        {
            Debug.Log("We are returning at 3.");

            return;
        }

        Vector3 _pos = _camera.WorldToScreenPoint(_targetObject.transform.position);

        Vector3 _pos2 = _camera.WorldToViewportPoint(_targetObject.transform.position);

        bool _withinScreen = _pos.z > 0.0f && _pos.x > 0.0f && _pos.x < Screen.width && _pos.y > 0.0f && _pos.y < Screen.height;

        _targetInSight = _withinScreen;

        Vector3 _screenCenter = new Vector3(Screen.width, Screen.height, 0.0f) / 2.0f;

        if (!_withinScreen)
        {
            if (_pos.z < 0.0f)
            {
                _pos = _pos * -1.0f;
            }

            _pos -= _screenCenter;

            float _angle = Mathf.Atan2(_pos.y, _pos.x);

            _angle -= 90.0f * Mathf.Deg2Rad;

            float _cos = Mathf.Cos(_angle);

            float _sin = -Mathf.Sin(_angle);

            float _m = _cos / _sin;

            Vector3 _bounds = _screenCenter * _threshold;

            _pos = _cos > 0.0f ? new Vector3(_bounds.y / _m, _bounds.y, 0.0f) : new Vector3(-_bounds.y / _m, -_bounds.y, 0.0f);


            if (_pos.x > _bounds.x)
            {
                _pos = new Vector3(_bounds.x, _bounds.x * _m, 0.0f);
            }
            else if (_pos.x < -_bounds.x)
            {
                _pos = new Vector3(-_bounds.x, -_bounds.x * _m, 0.0f);
            }

            _arrow.SetActive(true);

            _arrow.transform.localPosition = _pos;

            _arrow.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, _angle * Mathf.Rad2Deg);

            _arrow.GetComponent<Image>().sprite = _arrowSprite;

            _arrow.GetComponent<RectTransform>().sizeDelta = new Vector2(604, 851);

            //_anchoredPosition = _arrow.GetComponent<RectTransform>().anchoredPosition;

            if (_arrow.GetComponent<Image>().color != _indicatorColor)
            {
                _arrow.GetComponent<Image>().color = _indicatorColor;
            }

            if(_meterText != null)
            {
                _meterText.text = "";
            }
        }
        else
        {
            float _d = Vector3.Distance(_camera.transform.position, _targetObject.transform.position);

            if (_d >= _onscreenDistanceThreshold)
            {
                _arrow.SetActive(true);

                _arrow.GetComponent<RectTransform>().anchorMin = _pos2;

                _arrow.GetComponent<RectTransform>().anchorMax = _pos2;

                _arrow.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                Vector3 _localP = _arrow.GetComponent<RectTransform>().localPosition;

                _arrow.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);

                _arrow.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);

                _arrow.GetComponent<RectTransform>().localPosition = _localP;

                _arrow.transform.localRotation = Quaternion.identity;

                _arrow.GetComponent<Image>().sprite = _cursorSprite;

                _arrow.GetComponent<Image>().color = _indicatorColor;

                //_anchoredPosition = _arrow.GetComponent<RectTransform>().anchoredPosition;

                _meterText.color = _indicatorColor;

                Color _c = ToolsStruct.ChangeColorValue(_indicatorColor, 0.5f, 0.5f, true);

                _meterText.gameObject.GetComponent<Outline>().effectColor = _c;

                _arrow.GetComponent<RectTransform>().sizeDelta = new Vector2(1124, 1123);

                if(_meterText != null)
                {
                    int _dText = (int)_d - (int)_onscreenDistanceThreshold;

                    _meterText.text = _dText.ToString() + "m";
                }
            }
        }
    }

    bool GetValuesFromSettings(ref bool _onInput, ref float _thresholdInput)
    {
        //if(SettingsManager.GetInstance() == null)
        //{
        //    return false;
        //}

        BooleanSettingsClass _boolSetting = SettingsManager.GetInstance().GetBooleanSettingByName("Indicator On");

        DecimalSettingsClass _decimalSetting = SettingsManager.GetInstance().GetDecimalSettingByName("Indicator Threshold");

        if(_boolSetting == null || _decimalSetting == null)
        {
            return false;
        }

        _onInput = _boolSetting.GetBooleanValue();

        _thresholdInput = _decimalSetting.GetDecimalValue();

        return true;
    }
}
