using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    bool _landed;

    [SerializeField]
    List<GameObject> _gameObjectsToActivateWhenLanded;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDialoguePanel(int _input)
    {
        DialogueCanvasScript _go = FindObjectOfType<DialogueCanvasScript>();

        if(_go == null)
        {
            return;
        }

        _go.SetCurrentPanel(_input);
    }

    public bool GetLanded()
    {
        return _landed;
    }

    public void SetLanded(bool _input = true)
    {
        _landed = _input;

        SetGameObjectsActiveWhenLanded();
    }

    public void SetLandedToTrue()
    {
        _landed = true;

        SetGameObjectsActiveWhenLanded();
    }

    public void SetLandedToFalse()
    {
        _landed = false;

        SetGameObjectsActiveWhenLanded();
    }

    void SetGameObjectsActiveWhenLanded()
    {
        foreach(GameObject _go in _gameObjectsToActivateWhenLanded)
        {
            if(_go != null)
            {
                _go.SetActive(_landed);
            }
        }
    }
}
