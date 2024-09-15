using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExhibitionCanvasScript : MonoBehaviour
{
    [SerializeField]
    Text _objectNameText;

    [SerializeField]
    Camera _camera;

    [SerializeField]
    Button _talkButton;

    [SerializeField]
    ExhibitionScript _exhibition;

    [SerializeField]
    Button _backButton;

    [SerializeField]
    Vector2 _textExtraDistance;

    [SerializeField]
    MainCanvasesClass _mainCanvases;

    [SerializeField]
    Image _cursorImage;

    [SerializeField]
    Text _objectTitleText;

    [SerializeField]
    Text _objectDescriptionText;

    [SerializeField]
    GameObject _rocketContainerGO;

    [SerializeField]
    Animator _doctorAnimator;

    bool _timerStarts;

    float _timeElapsed = 0.0f;

    ExhibitionObjectScript _currentlyTalkedAboutObject;

    ExhibitionObjectScript _currentObject;

    Coroutine _talkingCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        DisplayName();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateExhibitionStatus();

        if (_timerStarts)
        {
            CheckTimeElaped();
        }
    }

    public void QuitExhibition()
    {
        _mainCanvases.SetCanvasesOn(true);

        if(_mainCanvases.GetDoctorCanvas() != null)
        {
            _mainCanvases.GetDoctorCanvas().gameObject.SetActive(true);
        }

        if(_rocketContainerGO != null)
        {
            _rocketContainerGO.SetActive(true);
        }

        StopExplaining();
    }

    public ExhibitionScript GetExhibition()
    {
        return _exhibition;
    }

    public ExhibitionObjectScript GetCurrentObject()
    {
        return _currentObject;
    }

    public ExhibitionObjectScript GetCurrentlyTalkedAboutObject()
    {
        return _currentlyTalkedAboutObject;
    }

    public void SetCurrentObject(ExhibitionObjectScript _input)
    {
        _currentObject = _input;
    }

    void DisplayName()
    {
        if (_currentObject == null || _camera == null)
        {
            _objectNameText.text = "";

            _objectNameText.gameObject.SetActive(false);

            return;
        }

        _objectNameText.text = _currentObject.GetObjectName();

        _objectNameText.gameObject.SetActive(true);

        _objectNameText.color = _currentObject.GetObjectColor();

        Outline _outline = _objectNameText.gameObject.GetComponent<Outline>();

        if (_outline != null)
        {
            Color _c = ToolsStruct.ChangeColorValue(_currentObject.GetObjectColor(), 0.5f, 0.5f, true);

            _outline.effectColor = _c;
        }

        Vector2 _textPos = _camera.WorldToScreenPoint(_currentObject.GetObjectPosition());

        _objectNameText.GetComponent<RectTransform>().localPosition = _textPos;
    }

    void UpdateExhibitionStatus()
    {
        if (_cursorImage == null || _objectNameText == null)
        {
            return;
        }

        if(_cursorImage.sprite == null)
        {
            return;
        }

        if(_currentObject == null)
        {
            _cursorImage.color = Color.red;

            _objectNameText.text = "";

            _objectNameText.gameObject.SetActive(false);

            _talkButton.gameObject.SetActive(false);

            return;
        }

        _cursorImage.color = Color.green;

        _objectNameText.text = _currentObject.GetObjectName();

        _objectNameText.color = _currentObject.GetObjectColor();

        _objectNameText.gameObject.GetComponent<Outline>().effectColor = GetOutlineColor(_currentObject.GetObjectColor());

        _objectNameText.gameObject.SetActive(true);

        _talkButton.gameObject.SetActive(true);
    }

    Color GetOutlineColor(Color _input)
    {
        Color _c = _input;

        float _lightLevel = _c.r + _c.g + _c.b;

        _lightLevel = _lightLevel / 3.0f;

        if(_c == Color.black)
        {
            _c = Color.white;
        }
        else if(_lightLevel < 0.15)
        {
            _c.r = _c.r * 2.0f;

            _c.g = _c.g * 2.0f;

            _c.b = _c.b * 2.0f;
        }
        else
        {
            _c.r = _c.r / 2.0f;

            _c.g = _c.g / 2.0f;

            _c.b = _c.b / 2.0f;
        }

        _c.a = 0.5f;

        return _c;
    }

    public void StopExplaining()
    {
        if(_talkingCoroutine == null)
        {
            return;
        }
        
        StopCoroutine(_talkingCoroutine);
        
        _objectTitleText.text = "";
        
        _objectDescriptionText.text = "";

        _currentlyTalkedAboutObject = null;
    }

    public void StartExplaining()
    {
        if(_currentObject == null)
        {
            return;
        }

        StopExplaining();

        float _time = _currentObject.GetTalkingSeconds();

        int _index = _currentObject.GetObjectNumber();

        string _name = _currentObject.GetObjectName();

        string _description = _currentObject.GetObjectDescription();

        string _fullTitle = _index.ToString() + ". " + _name;

        _talkingCoroutine = StartCoroutine(TalkingCoroutine(_time, _fullTitle, _description));

        Color _color = _currentObject.GetObjectColor();

        ColorTitle(_color);
    }

    IEnumerator TalkingCoroutine(float _timeInput, string _titleInput, string _descriptionInput)
    {
        _objectTitleText.text = _titleInput;

        _objectDescriptionText.text = _descriptionInput;

        _currentlyTalkedAboutObject = _currentObject;

        _timerStarts = true;

        if(_doctorAnimator != null)
        {
            _doctorAnimator.SetBool("Talking", true);
        }

        yield return new WaitForSeconds(_timeInput);

        if (_doctorAnimator != null)
        {
            _doctorAnimator.SetBool("Talking", false);
        }

        _timerStarts = false;

        _timeElapsed = 0.0f;

        _objectDescriptionText.text = "";

        _objectTitleText.text = "";

        _currentlyTalkedAboutObject = null;
    }

    void ColorTitle(Color _input)
    {
        _objectTitleText.color = _input;

        Color _outlineColor = ToolsStruct.ChangeColorValue(_input, 0.5f, 0.5f);

        Outline _outline = _objectTitleText.gameObject.GetComponent<Outline>();

        _outline.effectColor = _outlineColor;
    }


    void CheckTimeElaped()
    {
        _timeElapsed = _timeElapsed + Time.deltaTime;

        Debug.Log("The time elapsed for the coroutine is " + @"""" + _timeElapsed.ToString() + @"""" + ".");
    }
}
