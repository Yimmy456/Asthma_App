using Org.BouncyCastle.Asn1.BC;
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
        SetArrowPositionBasedOnObject(_input.GetGameObject(), _input.GetTextProperties(), _input.GetArrowProperties());

        PrepareMask(_input);
    }

    public UIIndicatorCanvasScript GetUIIndicator()
    {
        return _uiIndicator;
    }

    public void SetArrowPositionBasedOnObject(GameObject _gameObjectInput, TutorialLessonTextPropoertiesClass _textPropertiesInput, TutorialLessonArrowPropoertiesClass _arrowPropertiesInput)
    {
        //_uiText.GetComponent<RectTransform>().anchoredPosition = _textSizeInput;

        if(_gameObjectInput == null)
        {
            _targetObject = null;

            if (_uiIndicator != null)
            {
                _uiIndicator.gameObject.SetActive(false);

                _uiIndicator.SetTargetObject(null);
            }

            _arrowAnimation.SetAnimateBoolean(false);
        }
        else if(_gameObjectInput.GetComponent<RectTransform>() != null)
        {
            _targetObject = _gameObjectInput.GetComponent<RectTransform>();

            if (_uiIndicator != null)
            {
                _uiIndicator.gameObject.SetActive(false);

                _uiIndicator.SetTargetObject(null);
            }

            _arrowAnimation.SetAnimateBoolean(true);
        }
        else if(_gameObjectInput.transform != null)
        {
            if (_uiIndicator != null)
            {
                _uiIndicator.SetTargetObject(_gameObjectInput);

                _uiIndicator.gameObject.SetActive(true);
            }

            _tutorialManager.SetCurrentObject(_targetObject.gameObject);

            _arrowAnimation.SetAnimateBoolean(false);
        }

        AdjustText(_textPropertiesInput);

        SetArrow(_arrowPropertiesInput);
    }

    void AdjustText(TutorialLessonTextPropoertiesClass _input)
    {
        if(_uiText == null || _input == null)
        {
            return;
        }

        if(_input.GetInRelationToArrow())
        {
            _uiText.GetComponent<RectTransform>().parent = _arrowContainerTransform;

            _uiText.GetComponent<RectTransform>().anchorMin = _input.GetAnchorMin();

            _uiText.GetComponent<RectTransform>().anchorMax = _input.GetAnchorMax();

            _uiText.GetComponent<RectTransform>().anchoredPosition = _input.GetAnchoredPosition();

            _uiText.GetComponent<RectTransform>().sizeDelta = _input.GetSize();

            _uiText.GetComponent<RectTransform>().localScale = Vector2.one * _input.GetSizeConstant();

            //_uiText.GetComponent<RectTransform>().offsetMin = _input.GetOffsetMin();

            //_uiText.GetComponent<RectTransform>().offsetMax = _input.GetOffsetMax();

            //_uiText.GetComponent<RectTransform>().parent = _canvas.GetComponent<RectTransform>();
        }
        else
        {
            _uiText.GetComponent<RectTransform>().anchorMin = _input.GetAnchorMin();

            _uiText.GetComponent<RectTransform>().anchorMax = _input.GetAnchorMax();

            _uiText.GetComponent<RectTransform>().anchoredPosition = _input.GetAnchoredPosition();

            _uiText.GetComponent<RectTransform>().sizeDelta = _input.GetSize();

            _uiText.GetComponent<RectTransform>().localScale = Vector2.one * _input.GetSizeConstant();

            //_uiText.GetComponent<RectTransform>().offsetMin = _input.GetOffsetMin();

            //_uiText.GetComponent<RectTransform>().offsetMax = _input.GetOffsetMax();
        }

        SetUITextOffset(_input.GetOffsetMin(), _input.GetOffsetMax());

        _currentText = _input.GetText();

        if(_tutorialManager.GetCurrentLesson().GetShowNextButton() && _tutorialManager.GetCurrentLesson().GetName() != "Welcome" && _tutorialManager.GetCurrentLesson().GetName() != "End")
        {
            if (_tutorialManager.GetCurrentLesson().GetName() == "Inhaler")
            {
                _currentText = _currentText + "\n\nPress the green arrow button above the grey button that has the red " + @"""" + "No" + @"""" + " sign to proceed.";
            }
            else
            {
                _currentText = _currentText + "\n\nPress the green arrow button on the bottom corner to continue.";
            }

            Vector2 _size = _uiText.GetComponent<RectTransform>().sizeDelta;

            _size.y = _size.y + 400.0f;

            _uiText.GetComponent<RectTransform>().sizeDelta = _size;
        }

        _uiText.text = _currentText;
    }

    void SetArrow(TutorialLessonArrowPropoertiesClass _input)
    {
        if(_input == null)
        {
            return;
        }

        if(!_input.GetArrowInvolved())
        {
            _arrowContainerTransform.gameObject.SetActive(false);

            _arrowContainerTransform.anchorMin = new Vector2(0.5f, 0.5f);

            _arrowContainerTransform.anchorMax = new Vector2(0.5f, 0.5f);

            _arrowContainerTransform.anchoredPosition = new Vector2(0.0f, 0.0f);

            _arrowContainerTransform.rotation = Quaternion.identity;

            _arrowContainerTransform.localScale = Vector3.one;

            return;
        }

        _arrowContainerTransform.gameObject.SetActive(true);

        _arrowContainerTransform.anchorMin = _input.GetAnchorMin();

        _arrowContainerTransform.anchorMax = _input.GetAnchorMax();

        _arrowContainerTransform.anchoredPosition = _input.GetAnchoredPosition();

        _arrowContainerTransform.rotation = Quaternion.Euler(0.0f, 0.0f, _input.GetZRotation());

        _arrowContainerTransform.localScale = _input.GetFinalV3Scale();
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
        if(_tutorialManager == null || _uiText == null)
        {
            Debug.Log("We are leaving 'Land Button' at stage 1.");

            return;
        }

        if(!_tutorialManager.GetTutorialMode())
        {
            Debug.Log("We are leaving 'Land Button' at stage 2.");

            return;
        }


        if(_tutorialManager.GetCurrentLesson().GetName() == "Land Button")
        {
            PlaceIndicatorScript _pi =  _tutorialManager.GetPlaceIndicator();            

            if(_pi == null)
            {
                return;
            }          
            
            _pi.SetIndicatorOn(true);

            if(_pi.GetIsReady())
            {
                _currentText = _tutorialManager.GetCurrentLesson().GetTextProperties().GetText();                
            }
            else
            {
                _currentText = _tutorialManager.GetCurrentLesson().GetTextProperties().GetAlternativeText();
            }

            _uiText.text = _currentText;

            return;
        }


        if(_uiIndicator == null)
        {
            return;
        }

        if (_tutorialManager.GetCurrentLesson().GetName() == "Fasty" || _tutorialManager.GetCurrentLesson().GetName() == "Inhaler")
        {
            if (_uiIndicator.GetTargetInSight())
            {
                _currentText = _tutorialManager.GetCurrentLesson().GetTextProperties().GetText();

                bool _nb = _tutorialManager.GetCurrentLesson().GetShowNextButton();

                _nextButton.gameObject.SetActive(_nb);

                _nextButton.GetComponent<RectTransform>().anchoredPosition = _tutorialManager.GetCurrentLesson().GetNextButtonPosition();

                if (_nb)
                {
                    string _s = _currentText;

                    if (_tutorialManager.GetCurrentLesson().GetName() == "Inhaler")
                    {
                        _s = _s + "\n\nPress the green arrow button above the grey button that has the red " + @"""" + "No" + @"""" + " sign to proceed.";
                    }
                    else
                    {
                        _s = _s + "\n\nPress the green arrow button on the bottom corner to continue.";
                    }

                    _currentText = _s;
                }
            }
            else
            {
                _currentText = _tutorialManager.GetCurrentLesson().GetTextProperties().GetAlternativeText();

                _nextButton.gameObject.SetActive(false);
            }

            _uiText.text = _currentText;
        }

    }

    void SetUITextOffset(Vector2 _minInput, Vector2 _maxInput)
    {
        if(_uiText == null)
        {
            return;
        }

        if(_uiText.GetComponent<RectTransform>() == null)
        {
            return;
        }

        //Min Values

        if(_minInput.x != 0.0f && _minInput.y != 0.0f)
        {
            _uiText.GetComponent<RectTransform>().offsetMin = _minInput;
        }
        else if(_minInput.x != 0.0f)
        {
            Vector2 _v2 = _uiText.GetComponent<RectTransform>().offsetMin;

            _v2.x = _minInput.x;

            _uiText.GetComponent<RectTransform>().offsetMin = _v2;
        }
        else if (_minInput.y != 0.0f)
        {
            Vector2 _v2 = _uiText.GetComponent<RectTransform>().offsetMin;

            _v2.y = _minInput.y;

            _uiText.GetComponent<RectTransform>().offsetMin = _v2;
        }

        //Max Values

        if (_maxInput.x != 0.0f && _maxInput.y != 0.0f)
        {
            _uiText.GetComponent<RectTransform>().offsetMax = -_maxInput;
        }
        else if (_maxInput.x != 0.0f)
        {
            Vector2 _v2 = _uiText.GetComponent<RectTransform>().offsetMax;

            _v2.x = -_maxInput.x;

            _uiText.GetComponent<RectTransform>().offsetMax = _v2;
        }
        else if (_maxInput.y != 0.0f)
        {
            Vector2 _v2 = _uiText.GetComponent<RectTransform>().offsetMax;

            _v2.y = -_maxInput.y;

            _uiText.GetComponent<RectTransform>().offsetMax = _v2;
        }
    }
}
