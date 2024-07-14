using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YesOrNoCanvasScript : MonoBehaviour
{
    [SerializeField]
    Button _yesButton;

    [SerializeField]
    Button _noButton;

    [SerializeField]
    Text _text;

    public Button NoButtonVar {  get { return _noButton; } }

    public Button YesButtonVar { get { return _yesButton; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Text GetText()
    {
        return _text;
    }

    public Button GetYesButton()
    {
        return _yesButton;
    }

    public Button GetNoButton()
    {
        return _noButton;
    }

    public void SetText(string _input)
    {
        if(_text == null)
        {
            return;
        }

        _text.text = _input;
    }

    public void RemoveListenersOnYesButton()
    {
        if(_yesButton == null)
        {
            return;
        }
        _yesButton.onClick.RemoveAllListeners();
    }

    public void RemoveListenersOnNoButton()
    {
        if(_noButton == null)
        {
            return;
        }

        _noButton.onClick.RemoveAllListeners();
    }
}
