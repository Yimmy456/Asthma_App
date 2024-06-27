using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueCanvasScript : MonoBehaviour
{
    [SerializeField]
    List<Image> _panels;

    [SerializeField]
    int _currentPanelNumber = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Image> GetPanels()
    {
        return _panels;
    }

    public int GetCurrentPanelNumber()
    {
        return _currentPanelNumber;
    }

    public void SetCurrentPanel(int _input)
    {
        _currentPanelNumber = _input;

        for(int _i = 0; _i < _panels.Count; _i++)
        {
            if (_panels[_i] != null)
            {
                _panels[_i].gameObject.SetActive(_currentPanelNumber == _i);
            }
        }
    }
}
