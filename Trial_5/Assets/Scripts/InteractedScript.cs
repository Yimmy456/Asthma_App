using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractedScript : MonoBehaviour
{
    [SerializeField]
    bool _interacted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetInteracted()
    {
        return _interacted;
    }

    public void SetInteracted(bool _input)
    {
        _interacted = _input;
    }
}
