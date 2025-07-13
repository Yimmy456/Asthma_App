using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCanvasScript : MonoBehaviour
{
    [SerializeField]
    TutorialManagerScript _tutorialManager;

    [SerializeField]
    Canvas _canvas;

    [SerializeField]
    RectTransform _arrowTransform;

    [SerializeField]
    Text _uiText;

    [SerializeField]
    Transform _targetObject;

    [SerializeField]
    Camera _camera;

    [SerializeField]
    Image _panel;

    string _currentText;

    [SerializeField]
    List<Image> _uiMasks;

    [SerializeField]
    Image _defaultUiMask;

    public TutorialManagerScript TutorialManager
    {
        get
        {
            return _tutorialManager;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Canvas GetCanvas()
    {
        return _canvas;
    }

    public void SetArrowPositionBasedOnObject(TutorialLessonClass _input)
    {
        SetArrowPositionBasedOnObject(_input.GetGameObject(), _input.GetText(), _input.GetArrowPosition(), _input.GetZRotation(), _input.GetScale());

        PrepareMask(_input);
    }

    public Image GetPanel()
    {
        return _panel;
    }

    public void SetArrowPositionBasedOnObject(GameObject _gameObjectInput, string _textInput, Vector2 _positionInput, float _rotationInput = 0.0f, float _scaleInput = 1.0f)
    {
        if(_gameObjectInput == null)
        {
            _arrowTransform.gameObject.SetActive(false);

            _arrowTransform.anchoredPosition = Vector3.zero;

            _arrowTransform.anchorMin = new Vector2(0.5f, 0.5f);

            _arrowTransform.anchorMax = new Vector2(0.5f, 0.5f);

            _arrowTransform.rotation = Quaternion.identity;

            _arrowTransform.localScale = Vector3.one;

            _targetObject = null;

            _currentText = "";
        }
        else if(_gameObjectInput.GetComponent<RectTransform>() != null)
        {
            _arrowTransform.gameObject.SetActive(true);

            _arrowTransform.anchoredPosition = _positionInput;

            _arrowTransform.anchorMin = _gameObjectInput.GetComponent<RectTransform>().anchorMin;

            _arrowTransform.anchorMax = _gameObjectInput.GetComponent<RectTransform>().anchorMax;

            _arrowTransform.rotation = Quaternion.Euler(0.0f, 0.0f, _rotationInput);

            _arrowTransform.localScale = Vector3.one * _scaleInput;

            _targetObject = _gameObjectInput.GetComponent<RectTransform>();

            AdjustText();

            _currentText = _textInput;
        }
        else if(_gameObjectInput.transform != null)
        {
            _arrowTransform.gameObject.SetActive(true);

            _arrowTransform.anchoredPosition = _positionInput;

            _arrowTransform.anchorMin = new Vector2(0.5f, 0.5f);

            _arrowTransform.anchorMax = new Vector2(0.5f, 0.5f);

            _arrowTransform.rotation = Quaternion.Euler(0.0f, 0.0f, _rotationInput);

            _arrowTransform.localScale = Vector3.one * _scaleInput;

            _targetObject = _gameObjectInput.transform;

            AdjustText();

            _currentText = _textInput;
        }

        if (_uiText != null)
        {
            _uiText.text = _textInput;
        }
    }

    void AdjustText()
    {
        if(_uiText == null)
        {
            return;
        }

        _uiText.GetComponent<RectTransform>().parent = _arrowTransform;

        _uiText.GetComponent<RectTransform>().rotation = Quaternion.identity;

        _uiText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, -250.0f);

        _uiText.GetComponent<RectTransform>().parent = gameObject.GetComponent<RectTransform>();

        _uiText.GetComponent<RectTransform>().rotation = Quaternion.identity;
    }

    void UpdatePositionByObject()
    {
        if(_targetObject == null || _camera == null || _tutorialManager == null || _arrowTransform == null)
        {
            return;
        }

        if(!_tutorialManager.GetTutorialMode() || _targetObject.GetComponent<RectTransform>() != null)
        {
            return;
        }

        Vector3 _pos = _camera.WorldToScreenPoint(_targetObject.transform.position);

        bool _withinScreen = _pos.z > 0.0f && _pos.x > 0.0f && _pos.x < Screen.width && _pos.y > 0.0f && _pos.y < Screen.height;

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

            Vector3 _bounds = _screenCenter;

            _pos = _cos > 0.0f ? new Vector3(_bounds.y / _m, _bounds.y, 0.0f) : new Vector3(-_bounds.y / _m, -_bounds.y, 0.0f);


            if (_pos.x > _bounds.x)
            {
                _pos = new Vector3(_bounds.x, _bounds.x * _m, 0.0f);
            }
            else if (_pos.x < -_bounds.x)
            {
                _pos = new Vector3(-_bounds.x, -_bounds.x * _m, 0.0f);
            }

            _arrowTransform.transform.localPosition = _pos;

            _arrowTransform.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, _angle * Mathf.Rad2Deg);
        }
        else
        {
            float _d = Vector3.Distance(_camera.transform.position, _targetObject.transform.position);

            _arrowTransform.transform.localPosition = (_pos - _screenCenter);

            _arrowTransform.transform.localRotation = Quaternion.identity;
            
        }
    }

    public void ResetCanvas()
    {
        if(_arrowTransform == null)
        {
            return;
        }

        _arrowTransform.parent = gameObject.GetComponent<RectTransform>();

        _arrowTransform.anchorMin = new Vector2(0.5f, 0.0f);

        _arrowTransform.anchorMax = new Vector2(0.5f, 0.0f);

        _arrowTransform.anchoredPosition = new Vector2(0.0f, 635.0f);

        _arrowTransform.sizeDelta = new Vector2(141.9507f, 200);

        _arrowTransform.rotation = Quaternion.identity;

        _arrowTransform.localScale = Vector3.one;

        _arrowTransform.gameObject.SetActive(false);
    }

    void PrepareMask(TutorialLessonClass _input)
    {

        if (_defaultUiMask != null)
        {
            _defaultUiMask.gameObject.SetActive(false);
        }

        for (int _i = 0; _i < _uiMasks.Count; _i++)
        {
            Image _mask = _uiMasks[_i];

            if(_mask != null)
            {
                _mask.gameObject.SetActive(false);
            }
        }

        if(_input.GetMask() != null)
        {
            _input.GetMask().gameObject.SetActive(true);
        }
        else if(_defaultUiMask != null)
        {
            _defaultUiMask.gameObject.SetActive(true);
        }
    }
}
