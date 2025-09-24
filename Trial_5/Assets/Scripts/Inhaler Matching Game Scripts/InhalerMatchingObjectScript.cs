using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhalerMatchingObjectScript : MatchingGameBlockScript
{
    [Header("2. Variables for Inhaler Matching Blocks")]

    [SerializeField]
    protected string _objectName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBlockWhenDragged();

        MaintainVelocity();

        StayInXnZRange();
    }

    public string GetObjectName()
    {
        return _objectName;
    }

    public void SetObjectName(string _input)
    {
        _objectName = _input;
    }

    private void OnCollisionEnter(Collision collision)
    {
        UpdateBody(collision, true);
    }

    private void OnCollisionExit(Collision collision)
    {
        UpdateBody(collision, false);
    }

    void UpdateBody(Collision _collision, bool _kinematicInput)
    {
        if(_draggableProperties == null)
        {
            return;
        }

        if(_draggableProperties.GetBody())
        {
            return;
        }

        Rigidbody _rb = _draggableProperties.GetBody();

        if(_collision.gameObject.name == "Floor")
        {
            _rb.isKinematic = _kinematicInput;
        }
    }
}
