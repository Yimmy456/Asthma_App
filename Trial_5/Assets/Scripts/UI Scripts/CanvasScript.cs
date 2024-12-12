using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    [SerializeField]
    protected Canvas _canvas;

    [SerializeField]
    protected Button _quitButton;

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

    public Button GetQuitButton()
    {
        return _quitButton;
    }
}
