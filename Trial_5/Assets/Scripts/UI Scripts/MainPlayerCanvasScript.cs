using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class MainPlayerCanvasScript : MonoBehaviour
{
    [SerializeField]
    List<Button> _menuButtonList;

    [SerializeField]
    Button _restartButton;

    [SerializeField]
    Button _menuButton;

    bool _showMenuButtons = false;

    [SerializeField]
    float _animationSpeed = 0.4f;

    [SerializeField]
    float _scaleConstant = 1.0f;

    [SerializeField]
    float _buttonSize = 0.8f;

    [SerializeField]
    float _timeBetweenEachButtonAnimation = 0.2f;

    [SerializeField]
    bool _reverseShowAnimation;

    [SerializeField]
    bool _reverseHideAnimation;

    //[Range(0.0f, 360.0f)]

    [SerializeField]
    CircleClass _buttonCircleProperties;

    [SerializeField]
    InhalerPropertiesClass _shakingInhalerProperties;

    [SerializeField]
    TutorialManagerScript _tutorialManager;

    Coroutine _coroutine;

    bool _animationComplete = true;

    int _buttonsAnim = 0;

    List<Coroutine> _animationCoroutines = new List<Coroutine>();

    //CurrentBlocksAndHolesForInhalerClass _currentBlocksAndHolesI = 

    // Start is called before the first frame update
    void Start()
    {
        for (int _i = 0; _i < _menuButtonList.Count; _i++)
        {
            _menuButtonList[_i].gameObject.GetComponent<RectTransform>().localScale = (Vector3.one * _buttonSize);
        }

        _shakingInhalerProperties.SetShowingInhalerBoolean(false);

        _shakingInhalerProperties.SetInitalInhalerRotation();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<Button> GetMenuButtonList()
    {
        return _menuButtonList;
    }

    public float GetAnimationSpeed()
    {
        return _animationSpeed;
    }

    public bool GetAnimationComplete()
    {
        return _animationComplete;
    }

    public bool GetShowingButtons()
    {
        return _showMenuButtons;
    }

    public Coroutine GetCoroutine()
    {
        return _coroutine;
    }

    public List<Coroutine> GetAnimationCoroutines()
    {
        return _animationCoroutines;
    }

    IEnumerator ShowButtons()
    {
        float _angle;

        float _count = _menuButtonList.Count;

        Vector2 _position = Vector2.zero;

        float _alpha = 0.0f;

        Color _c;

        _animationComplete = false;

        float _finalRadius = _buttonCircleProperties.GetRadius() * _scaleConstant;

        float _finalSpeed = _animationSpeed * _scaleConstant * Time.deltaTime;

        bool _amountReached = false;

        for (float _s = 0; _s < _finalRadius && !_amountReached; _s += _finalSpeed)
        {
            if (_s > _finalRadius)
            {
                _s = _finalRadius;

                //Debug.Log("This is the final iteration to increase.");

                _amountReached = true;
            }

            _alpha = _s / _finalRadius;

            for (int _i = 0; _i < _count; _i++)
            {
                if (_menuButtonList[_i] == null)
                {
                    continue;
                }

                _angle = (_count > 1) ? (((_i * _buttonCircleProperties.GetSpreadingDegrees()) / (_count - 1)) + _buttonCircleProperties.GetAdditionalDegrees()) : _buttonCircleProperties.GetDefaultAngle();

                if(!_buttonCircleProperties.GetIsCounterClockwise())
                {
                    _angle = -_angle;
                }

                _position.x = Mathf.Cos(_angle * Mathf.Deg2Rad) * -_s;

                ///_position.x = Mathf.Cos(_angle * Mathf.Deg2Rad) * -_s;

                _position.y = Mathf.Sin(_angle * Mathf.Deg2Rad) * _s;

                if(_buttonCircleProperties.GetSwitchAxes())
                {
                    float _sw = _position.x;

                    _position.x = _position.y;

                    _position.y = _sw;
                }

                _menuButtonList[_i].GetComponent<RectTransform>().anchoredPosition = _position;

                _c = _menuButtonList[_i].image.color;

                _c.a = _alpha;

                _menuButtonList[_i].image.color = _c;
            }

            yield return null;
        }

        for (int _j = 0; _j < _count; _j++)
        {
            if (_menuButtonList[_j] == null)
            {
                continue;
            }

            _menuButtonList[_j].interactable = true;

            _angle = (_count > 1) ? (((_j * _buttonCircleProperties.GetSpreadingDegrees()) / (_count - 1)) + _buttonCircleProperties.GetAdditionalDegrees()) : _buttonCircleProperties.GetDefaultAngle();

            if(!_buttonCircleProperties.GetIsCounterClockwise())
            {
                _angle = -_angle;
            }

            _position.x = Mathf.Cos(_angle * Mathf.Deg2Rad) * -_finalRadius;

            ///_position.x = Mathf.Cos(_angle * Mathf.Deg2Rad) * -_finalRadius;

            _position.y = Mathf.Sin(_angle * Mathf.Deg2Rad) * _finalRadius;

            if(_buttonCircleProperties.GetSwitchAxes())
            {
                float _sw = _position.x;

                _position.x = _position.y;

                _position.y = _sw;
            }

            _menuButtonList[_j].GetComponent<RectTransform>().anchoredPosition = _position;

            _c = _menuButtonList[_j].image.color;

            _c.a = 1.0f;

            _menuButtonList[_j].image.color = _c;
        }

        _animationComplete = true;

        ContinueTutorial();
    }

    IEnumerator HideButtons()
    {
        _animationComplete = false;

        float _angle;

        float _count = _menuButtonList.Count;

        Vector2 _position = Vector2.zero;

        float _alpha = 1.0f;

        Color _c;

        float _finalRadius = _buttonCircleProperties.GetRadius() * _scaleConstant;

        float _finalSpeed = _animationSpeed * _scaleConstant * Time.deltaTime;

        bool _amountReached = false;

        for (int _j = 0; _j < _count; _j++)
        {
            _menuButtonList[_j].interactable = false;
        }

        for (float _s = _finalRadius; _s >= 0 && !_amountReached; _s -= _finalSpeed)
        {
            if (_s < 0)
            {
                _s = 0;

                //Debug.Log("This is the final iteration to decrease.");

                _amountReached = true;
            }

            _alpha = _s / _finalRadius;

            for (int _i = 0; _i < _count; _i++)
            {
                if (_menuButtonList[_i] == null)
                {
                    continue;
                }

                _angle = (_count > 1) ? (((_i * _buttonCircleProperties.GetSpreadingDegrees()) / (_count - 1)) + _buttonCircleProperties.GetAdditionalDegrees()) : _buttonCircleProperties.GetDefaultAngle();

                if(!_buttonCircleProperties.GetIsCounterClockwise())
                {
                    _angle = -_angle;
                }

                _position.x = Mathf.Cos(_angle * Mathf.Deg2Rad) * -_s;

                ///_position.x = Mathf.Cos(_angle * Mathf.Deg2Rad) * -_s;

                _position.y = Mathf.Sin(_angle * Mathf.Deg2Rad) * _s;

                if(_buttonCircleProperties.GetSwitchAxes())
                {
                    float _sw = _position.x;

                    _position.x = _position.y;

                    _position.y = _sw;
                }

                _menuButtonList[_i].GetComponent<RectTransform>().anchoredPosition = _position;

                _c = _menuButtonList[_i].image.color;

                _c.a = _alpha;

                _menuButtonList[_i].image.color = _c;
            }

            yield return null;
        }

        for (int _j = 0; _j < _count; _j++)
        {
            if (_menuButtonList[_j] == null)
            {
                continue;
            }

            _menuButtonList[_j].GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

            _c = _menuButtonList[_j].image.color;

            _c.a = 0.0f;

            _menuButtonList[_j].image.color = _c;
        }

        _animationComplete = true;
    }

    public void PlayAnimation(bool _input)
    {
        if (!_animationComplete)
        {
            return;
        }

        _showMenuButtons = _input;

        TerminateAnimations();

        _animationComplete = false;

        if (_showMenuButtons)
        {
            _coroutine = StartCoroutine(ShowButtons2());
        }
        else if (!_showMenuButtons)
        {
            _coroutine = StartCoroutine(HideButtons2());
        }
    }

    public void PlayAnimation()
    {
        PlayAnimation(!_showMenuButtons);
    }

    public void HideButtonsImmediately()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);

            _animationComplete = true;
        }

        for (int _i = 0; _i < _menuButtonList.Count; _i++)
        {
            if (_menuButtonList[_i] == null)
            {
                continue;
            }

            _menuButtonList[_i].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            _menuButtonList[_i].interactable = false;

            Color _c = _menuButtonList[_i].image.color;

            _c.a = 0.0f;

            _menuButtonList[_i].image.color = _c;

            //_menuButtonList[_i].GetComponent<RectTransform>().Find("Text").gameObject.SetActive(false);
        }

        _showMenuButtons = false;

        _buttonsAnim = 0;
    }

    public Button GetRestartButton()
    {
        return _restartButton;
    }

    public Button GetMenuButton()
    {
        return _menuButton;
    }

    void BringToFront()
    {
        for(int _i = 0; _i < _menuButtonList.Count; _i++)
        {
            Button _currentButton = _menuButtonList[_i];

            if(_currentButton == null)
            {
                continue;
            }

            Text _text = _currentButton.GetComponent<RectTransform>().GetComponentInChildren<Text>();

            if(_text == null)
            {
                continue;
            }
        }
    }

    IEnumerator ShowIndividualButtonAnimation(Button _buttonInput, float _angleInput, bool _finalButtonBooleanInput)
    {
        float _finalRadius = _buttonCircleProperties.GetRadius() * _scaleConstant;

        float _finalSpeed = _animationSpeed * _scaleConstant * Time.deltaTime;

        Vector2 _pos = _buttonInput.GetComponent<RectTransform>().anchoredPosition;

        float _angle = _angleInput;

        float _alpha = _buttonInput.image.color.a;

        Color _c;

        bool _amountReached = false;

        float _s = 0.0f;

        _buttonsAnim++;

        if(!_buttonCircleProperties.GetIsCounterClockwise())
        {
            _angle = -_angle;
        }

        while(!_amountReached)
        {
            _s = _s + _finalSpeed;

            if(_s >= _finalRadius)
            {
                _s = _finalRadius;

                _alpha = 1.0f;

                _amountReached = true;
            }
            else
            {
                _alpha = _s / _finalRadius;
            }

            _pos.x = Mathf.Cos(_angle * Mathf.Deg2Rad) * -_s;

            _pos.y = Mathf.Sin(_angle * Mathf.Deg2Rad) * _s;

            _buttonInput.GetComponent<RectTransform>().anchoredPosition = _pos;

            _c = _buttonInput.image.color;

            _c.a = _alpha;

            _buttonInput.image.color = _c;

            yield return null;
        }

        if(_finalButtonBooleanInput)
        {
            for(int _i = 0; _i < _menuButtonList.Count; _i++)
            {
                _menuButtonList[_i].interactable = true;
            }

            _animationComplete = true;

            _animationCoroutines.Clear();
        }

        _buttonsAnim--;

        //_buttonInput.interactable = true;
    }

    IEnumerator HideIndividualButtonAnimation(Button _buttonInput, float _angleInput, bool _finalButtonBooleanInput)
    {
        float _finalRadius = _buttonCircleProperties.GetRadius() * _scaleConstant;

        float _finalSpeed = _animationSpeed * _scaleConstant * Time.deltaTime;

        Vector2 _pos = new Vector2((Mathf.Cos(_angleInput * Mathf.Deg2Rad)), (Mathf.Sin(_angleInput * Mathf.Deg2Rad)));

        float _angle = _angleInput;

        Color _c;

        float _s = _finalRadius;

        bool _amountReached = false;

        if (!_buttonCircleProperties.GetIsCounterClockwise())
        {
            _pos.x = -_pos.x;

            _angle = -_angle;
        }

        float _alpha;

        _buttonsAnim++;

        while(!_amountReached)  
        {
            _s = _s - _finalSpeed;

            if(_s <= 0.0f)
            {
                _s = 0.0f;

                _alpha = 0.0f;

                _amountReached = true;
            }
            else
            {
                _alpha = _s / _finalRadius;
            }

            _pos.x = Mathf.Cos(_angle * Mathf.Deg2Rad) * -_s;

            _pos.y = Mathf.Sin(_angle * Mathf.Deg2Rad) * _s;

            _buttonInput.GetComponent<RectTransform>().anchoredPosition = _pos;

            _c = _buttonInput.image.color;

            _c.a = _alpha;

            _buttonInput.image.color = _c;

            yield return null;
        }

        Color _c2 = _buttonInput.image.color;

        _c2.a = 0.0f;

        _buttonInput.image.color = _c2;

        if(_finalButtonBooleanInput)
        {
            _animationComplete = true;

            _animationCoroutines.Clear();
        }

        _buttonsAnim--;
    }

    IEnumerator ShowButtons2()
    {
        float _currentAngle = 0.0f;

        Button _currentButton;

        bool _finalButtonBoolean;

        int _j;

        int _divValue = ToolsStruct.GetCircleDivision(_buttonCircleProperties.GetSpreadingDegrees(), _menuButtonList.Count);

        for(int _i = 0; _i < _menuButtonList.Count; _i++)
        {
            if(_reverseShowAnimation)
            {
                _j = _menuButtonList.Count - 1;

                _j = _j - _i;
            }
            else
            {
                _j = _i;
            }

            if (_divValue >= 1)
            {
                _currentAngle = (_j * _buttonCircleProperties.GetSpreadingDegrees()) / _divValue;

                _currentAngle = _currentAngle + _buttonCircleProperties.GetAdditionalDegrees();
            }
            else
            {
                _currentAngle = _buttonCircleProperties.GetDefaultAngle();
            }

            _currentButton = _menuButtonList[_j];

            _finalButtonBoolean = _i == _menuButtonList.Count - 1;

            _animationCoroutines.Add(StartCoroutine(ShowIndividualButtonAnimation(_currentButton, _currentAngle, _finalButtonBoolean)));

            yield return new WaitForSeconds(_timeBetweenEachButtonAnimation);
        }
    }

    IEnumerator HideButtons2()
    {
        for(int _i = 0; _i < _menuButtonList.Count; _i++)
        {
            _menuButtonList[_i].interactable = false;
        }

        float _currentAngle = 0.0f;

        Button _currentButton;

        bool _finalButtonBoolean = false;

        int _j;

        int _divValue = ToolsStruct.GetCircleDivision(_buttonCircleProperties.GetSpreadingDegrees(), _menuButtonList.Count);

        for(int _i = 0; _i < _menuButtonList.Count; _i++)
        {
            if (_reverseHideAnimation)
            {
                _j = _menuButtonList.Count - 1;

                _j = _j - _i;
            }
            else
            {
                _j = _i;
            }

            if (_divValue >= 1)
            {
                _currentAngle = (_j * _buttonCircleProperties.GetSpreadingDegrees()) / _divValue;

                _currentAngle = _currentAngle + _buttonCircleProperties.GetAdditionalDegrees();
            }
            else
            {
                _currentAngle = _buttonCircleProperties.GetDefaultAngle();
            }

            _currentButton = _menuButtonList[_j];

            _finalButtonBoolean = _i == _menuButtonList.Count - 1;

            _animationCoroutines.Add(StartCoroutine(HideIndividualButtonAnimation(_currentButton, _currentAngle, _finalButtonBoolean)));

            yield return new WaitForSeconds(_timeBetweenEachButtonAnimation);
        }
    }

    public void HideButtonsImmediately2()
    {
        TerminateAnimations();

        _showMenuButtons = false;

        //_animationComplete = true;

        Button _currentButton;

        Color _c;

        for(int _i = 0; _i < _menuButtonList.Count; _i++)
        {
            _currentButton = _menuButtonList[_i];

            _currentButton.interactable = false;

            _currentButton.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            _c = _currentButton.image.color;

            _c.a = 0.0f;

            _currentButton.image.color = _c;
        }
    }

    public void ShowButtonsImmediately()
    {
        TerminateAnimations();

        _showMenuButtons = true;

        Button _currentButton;

        Color _c;

        float _angle = 0.0f;

        float _count = _menuButtonList.Count;

        Vector2 _position = Vector2.zero;

        for (int _i = 0; _i < _menuButtonList.Count; _i++)
        {
            if (_menuButtonList[_i] == null)
            {
                continue;
            }

            _angle = (_count > 1) ? (((_i * _buttonCircleProperties.GetSpreadingDegrees()) / (_count - 1)) + _buttonCircleProperties.GetAdditionalDegrees()) : _buttonCircleProperties.GetDefaultAngle();
            
            if (!_buttonCircleProperties.GetIsCounterClockwise())
            {
                _angle = -_angle;
            }

            _position.x = Mathf.Cos(_angle * Mathf.Deg2Rad) * _buttonCircleProperties.GetRadius() * -_scaleConstant;

            _position.y = Mathf.Sin(_angle * Mathf.Deg2Rad) * _buttonCircleProperties.GetRadius() * _scaleConstant;

            if (_buttonCircleProperties.GetSwitchAxes())
            {
                float _sw = _position.x;

                _position.x = _position.y;

                _position.y = _sw;
            }

            _currentButton = _menuButtonList[_i];

            _currentButton.interactable = true;

            _currentButton.GetComponent<RectTransform>().anchoredPosition = _position;

            _c = _currentButton.image.color;

            _c.a = 1.0f;

            _currentButton.image.color = _c;
        }
    }

    public void ToggleShowingButtonsImmediately()
    {
        TerminateAnimations();

        if(_showMenuButtons)
        {
            HideButtonsImmediately2();
        }
        else
        {
            ShowButtonsImmediately();
        }
    }

    void TerminateAnimations()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        for (int _i = 0; _i < _animationCoroutines.Count; _i++)
        {
            if (_animationCoroutines[_i] != null)
            {
                StopCoroutine(_animationCoroutines[_i]);
            }
        }

        _buttonsAnim = 0;

        _animationComplete = true;

        _animationCoroutines.Clear();

        //_showMenuButtons = false;
    }

    public void ShowCorrectButton()
    {
        if(_menuButton == null)
        {
            return;
        }

        if(DataPersistenceManager.GetInstance() == null)
        {
            _menuButton.gameObject.SetActive(true);

            return;
        }

        if(DataPersistenceManager.GetInstance().GetGameData() == null)
        {
            _menuButton.gameObject.SetActive(true);

            return;
        }
        
        _menuButton.gameObject.SetActive(true);
    }

    public InhalerPropertiesClass GetShakingInhalerProperties()
    {
        return _shakingInhalerProperties;
    }

    public void SetShowingInhalerPropertiesBoolean(bool _input)
    {
        _shakingInhalerProperties.SetShowingInhalerBoolean(_input);
    }


    public void SwitchShowingInhalerPropertiesBoolean()
    {
        _shakingInhalerProperties.SwitchShowingInhalerBoolean();
    }

    void ContinueTutorial()
    {
        if(_tutorialManager == null)
        {
            return;
        }

        if(!_tutorialManager.GetTutorialMode())
        {
            return;
        }

        for(int _i = 0; _i < _menuButtonList.Count; _i++)
        {
            _menuButtonList[_i].interactable = false;
        }

        _tutorialManager.GoToNextStepUnderIndexMatchCondition(3);
    }
}
