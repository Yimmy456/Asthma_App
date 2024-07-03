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

    ExhibitionObjectScript _currentObject;

    // Start is called before the first frame update
    void Start()
    {
        DisplayName();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
