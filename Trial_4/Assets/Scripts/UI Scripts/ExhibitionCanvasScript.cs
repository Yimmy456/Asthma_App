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

    ExhibitionObjectScript _currentObject;

    // Start is called before the first frame update
    void Start()
    {
        DisplayName();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateExhibitionStatus();
    }

    public void QuitExhibition()
    {
        _mainCanvases.SetCanvasesOn(true);

        if(_mainCanvases.GetDoctorCanvas() != null)
        {
            _mainCanvases.GetDoctorCanvas().gameObject.SetActive(true);
        }
    }

    public ExhibitionObjectScript GetCurrentObject()
    {
        return _currentObject;
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
            Color _c = _currentObject.GetObjectColor();

            _c.r = _c.r / 2.0f;

            _c.g = _c.g / 2.0f;

            _c.b = _c.b / 2.0f;

            _c.a = 0.5f;

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
}
