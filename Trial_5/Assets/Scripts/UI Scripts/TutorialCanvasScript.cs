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
    RectTransform _arrowContainerTransform;

    [SerializeField]
    Text _uiText;

    [SerializeField]
    Transform _targetObject;

    [SerializeField]
    Camera _camera;

    string _currentText;

    [SerializeField]
    List<Image> _uiMasks;

    [SerializeField]
    Button _nextButton;

    [SerializeField]
    UIIndicatorCanvasScript _uiIndicator;

    [SerializeField]
    UIAnimationClass _arrowAnimation;

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
        UpdateUIIndicator();
    }

    public Canvas GetCanvas()
    {
        return _canvas;
    }

    public UIAnimationClass GetArrowAnimation()
    {
        return _arrowAnimation;
    }

    public void SetArrowPositionBasedOnObject(TutorialLessonClass _input)
    {
        SetArrowPositionBasedOnObject(_input.GetGameObject(), _input.GetText(), _input.GetArrowPosition(), _input.GetTextPosition(), _input.GetTextSize(), _input.GetZRotation(), _input.GetScale());

        PrepareMask(_input);
    }

    public UIIndicatorCanvasScript GetUIIndicator()
    {
        return _uiIndicator;
    }

    public void SetArrowPositionBasedOnObject(GameObject _gameObjectInput, string _textInput, Vector2 _arrowPositionInput, Vector2 _textPositionInput, Vector2 _textSizeInput, float _rotationInput = 0.0f, float _scaleInput = 1.0f)
    {
        if(_gameObjectInput == null)
        {
            _arrowContainerTransform.gameObject.SetActive(false);

            _arrowContainerTransform.anchoredPosition = Vector3.zero;

            _arrowContainerTransform.anchorMin = new Vector2(0.5f, 0.5f);

            _arrowContainerTransform.anchorMax = new Vector2(0.5f, 0.5f);

            _arrowContainerTransform.rotation = Quaternion.identity;

            _arrowTransform.localScale = Vector3.one;

            _targetObject = null;

            _uiText.GetComponent<RectTransform>().parent = gameObject.GetComponent<RectTransform>();

            _uiText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);

            _uiText.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);

            _uiText.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);

            _uiText.GetComponent<RectTransform>().rotation = Quaternion.identity;

            if (_uiIndicator != null)
            {
                _uiIndicator.gameObject.SetActive(false);

                _uiIndicator.SetTargetObject(null);
            }

            _arrowAnimation.SetAnimateBoolean(false);

            //_uiText.GetComponent<RectTransform>().parent = _arrowContainerTransform;

            _uiText.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);

            _uiText.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);

            _uiText.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        else if(_gameObjectInput.GetComponent<RectTransform>() != null)
        {
            if(_tutorialManager.GetCurrentIndex() == 1)
            {
                Debug.Log("Index 1 starts here.");
            }

            _arrowContainerTransform.gameObject.SetActive(true);

            _arrowContainerTransform.parent = _gameObjectInput.GetComponent<RectTransform>();

            _arrowContainerTransform.anchorMin = new Vector2(0.5f, 0.5f);

            _arrowContainerTransform.anchorMax = new Vector2(0.5f, 0.5f);

            _arrowContainerTransform.anchoredPosition = Vector2.zero;

            _arrowContainerTransform.parent = gameObject.GetComponent<RectTransform>();

            _arrowContainerTransform.anchorMin = _gameObjectInput.GetComponent<RectTransform>().anchorMin;

            _arrowContainerTransform.anchorMax = _gameObjectInput.GetComponent<RectTransform>().anchorMax;

            _arrowContainerTransform.anchoredPosition = _arrowPositionInput;

            _arrowContainerTransform.rotation = Quaternion.Euler(0.0f, 0.0f, _rotationInput);

            _arrowTransform.localScale = Vector3.one * _scaleInput;

            _targetObject = _gameObjectInput.GetComponent<RectTransform>();

            //AdjustText();

            if (_uiIndicator != null)
            {
                _uiIndicator.gameObject.SetActive(false);

                _uiIndicator.SetTargetObject(null);
            }

            _arrowAnimation.SetAnimateBoolean(true);

            _uiText.GetComponent<RectTransform>().parent = _arrowContainerTransform;

            _uiText.GetComponent<RectTransform>().anchoredPosition = _textPositionInput;

            _uiText.GetComponent<RectTransform>().parent = _canvas.GetComponent<RectTransform>();
        }
        else if(_gameObjectInput.transform != null)
        {
            _arrowContainerTransform.gameObject.SetActive(false);

            _arrowContainerTransform.anchoredPosition = _arrowPositionInput;

            _arrowContainerTransform.anchorMin = new Vector2(0.5f, 0.5f);

            _arrowContainerTransform.anchorMax = new Vector2(0.5f, 0.5f);

            _arrowContainerTransform.rotation = Quaternion.Euler(0.0f, 0.0f, _rotationInput);

            _arrowTransform.localScale = Vector3.one * _scaleInput;

            _targetObject = _gameObjectInput.transform;

            AdjustText();

            if(_uiIndicator != null)
            {
                _uiIndicator.SetTargetObject(_gameObjectInput);

                _uiIndicator.gameObject.SetActive(true);
            }

            _tutorialManager.SetCurrentObject(_targetObject.gameObject);

            _arrowAnimation.SetAnimateBoolean(false);

            _uiText.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);

            _uiText.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);

            _uiText.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        _uiText.GetComponent<RectTransform>().anchoredPosition = _textSizeInput;

        _currentText = _textInput;

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

        _uiText.GetComponent<RectTransform>().parent = _arrowContainerTransform;

        _uiText.GetComponent<RectTransform>().rotation = Quaternion.identity;

        _uiText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, -250.0f);

        _uiText.GetComponent<RectTransform>().parent = gameObject.GetComponent<RectTransform>();

        _uiText.GetComponent<RectTransform>().rotation = Quaternion.identity;
    }

    void UpdatePositionByObject()
    {
        if(_targetObject == null || _camera == null || _tutorialManager == null || _arrowTransform == null || _arrowContainerTransform == null)
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

            _arrowContainerTransform.transform.localPosition = _pos;

            _arrowContainerTransform.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, _angle * Mathf.Rad2Deg);
        }
        else
        {
            float _d = Vector3.Distance(_camera.transform.position, _targetObject.transform.position);

            _arrowContainerTransform.transform.localPosition = (_pos - _screenCenter);

            _arrowContainerTransform.transform.localRotation = Quaternion.identity;
            
        }
    }

    public void ResetCanvas()
    {
        if(_arrowTransform == null || _arrowContainerTransform == null)
        {
            return;
        }

        _arrowContainerTransform.parent = gameObject.GetComponent<RectTransform>();

        _arrowContainerTransform.anchorMin = new Vector2(0.5f, 0.0f);

        _arrowContainerTransform.anchorMax = new Vector2(0.5f, 0.0f);

        _arrowContainerTransform.anchoredPosition = new Vector2(0.0f, 635.0f);

        _arrowContainerTransform.sizeDelta = new Vector2(141.9507f, 200);

        _arrowContainerTransform.rotation = Quaternion.identity;

        _arrowContainerTransform.localScale = Vector3.one;

        _arrowContainerTransform.gameObject.SetActive(false);

        if(_tutorialManager != null && _nextButton != null)
        {
            _nextButton.gameObject.SetActive(true);

            _nextButton.onClick.RemoveAllListeners();

            _nextButton.onClick.AddListener(delegate { _tutorialManager.GoToNextStepUnderIndexMatchCondition(0); });

            _nextButton.onClick.AddListener(delegate { _nextButton.gameObject.SetActive(false); });
        }
    }

    void PrepareMask(TutorialLessonClass _input)
    {
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
    }

    public Button GetNextButton()
    {
        return _nextButton;
    }

    void UpdateUIIndicator()
    {
        if(_uiIndicator == null || _tutorialManager == null || _uiText == null)
        {
            return;
        }

        if(!_tutorialManager.GetTutorialMode() || _uiIndicator.GetTargetObject() == null)
        {
            return;
        }



        if(_uiIndicator.GetTargetInSight())
        {
            if (_uiText.text == _tutorialManager.GetCurrentLesson().GetText())
            {
                _uiText.text = _tutorialManager.GetCurrentLesson().GetText();
            }
        }
        else
        {
            if (_uiText.text != _tutorialManager.GetCurrentLesson().GetAlternativeText())
            {
                _uiText.text = _tutorialManager.GetCurrentLesson().GetAlternativeText();
            }
        }
    }
}
