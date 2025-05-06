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
        
    }

    public Canvas GetCanvas()
    {
        return _canvas;
    }
}
