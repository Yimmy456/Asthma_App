using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManagerScript : MonoBehaviour
{
    static TouchManagerScript _instance;

    PlayerController _controller;

    UnityEngine.TouchPhase _phase;

    // Start is called before the first frame update
    void Start()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        if(_controller == null)
        {
            _controller = new PlayerController();
            _controller.Enable();
        }

        _phase = UnityEngine.TouchPhase.Canceled;

        SetTouchFunctions();
    }

    // Update is called once per frame
    void Update()
    {
        if(_phase == UnityEngine.TouchPhase.Began)
        {
            _phase = UnityEngine.TouchPhase.Stationary;
        }

        if(_phase == UnityEngine.TouchPhase.Ended)
        {
            _phase = UnityEngine.TouchPhase.Canceled;
        }
    }

    void SetTouchFunctions()
    {
        _controller.Touch.TouchItself.started += ctx => TouchStarted();
        _controller.Touch.TouchItself.canceled += ctx => TouchEnded();
    }

    void TouchStarted()
    {
        _phase = UnityEngine.TouchPhase.Began;
    }

    void TouchEnded()
    {
        _phase = UnityEngine.TouchPhase.Ended;
    }

    public static TouchManagerScript GetInstance()
    {
        return _instance;
    }

    public UnityEngine.TouchPhase GetPhase()
    {
        return _phase;
    }
}
