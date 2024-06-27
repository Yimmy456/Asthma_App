using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationCanvasScript : MonoBehaviour
{
    [SerializeField]
    Canvas _infoCanvas;

    [SerializeField]
    Text _text;

    [SerializeField]
    Outline _outline;

    [SerializeField]
    Button _nextButton;

    //Canvas _nextCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Canvas GetInfoCanvas()
    {
        return _infoCanvas;
    }

    public Text GetText()
    {
        return _text;
    }

    public Outline GetOutline()
    {
        return _outline;
    }

    public Button GetNextButton()
    {
        return _nextButton;
    }

    public void SetText(string _input)
    {
        if(_text == null)
        {
            return;
        }

        _text.text = _input;
    }

    public void SetText(string _textInput, Color _textColorInput, Color _outlineColorInput, Vector2 _outlineDistanceInput)
    {
        if(_text == null)
        {
            return;
        }

        _text.text = _textInput;

        _text.color = _textColorInput;

        if(_outline == null)
        {
            return;
        }

        _outline.effectColor = _outlineColorInput;

        _outline.effectDistance = _outlineDistanceInput;
    }
}
