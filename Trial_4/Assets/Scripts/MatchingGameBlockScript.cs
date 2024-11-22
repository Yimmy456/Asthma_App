using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class MatchingGameBlockScript : MonoBehaviour
{
    [Header("1. Variables for All Matching Blocks.")]

    [SerializeField]
    protected DraggableClass _draggableProperties;

    [SerializeField]
    protected RotationScript _rotationProperties;

    [SerializeField]
    protected Collider _collider;

    [SerializeField]
    protected MatchingGameCanvasScript _objectCanvas;

    //[SerializeField]
    protected bool _blockPlaced;

    protected MatchingGameHoleScript _matchedHole;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MaintainVelocity();
    }

    public DraggableClass GetDraggableProperties()
    {
        return _draggableProperties;
    }

    public RotationScript GetRotationProperties() {  return _rotationProperties; }

    public bool GetBlockPlaced()
    {
        return _blockPlaced;
    }

    public Collider GetCollider()
    {
        return _collider;
    }

    public MatchingGameHoleScript GetMatchedHole()
    {
        return _matchedHole;
    }

    public MatchingGameCanvasScript GetMatchingGameCanvas()
    {
        return _objectCanvas;
    }

    public void SetBlockPlaced(bool _input)
    {
        _blockPlaced = _input;
    }

    public void SetMatchedHole(MatchingGameHoleScript _input)
    {
        _matchedHole = _input;
    }

    public void SetMatchingGameCanvas(MatchingGameCanvasScript _input)
    {
        _objectCanvas = _input;
    }

    protected void UpdateBlockWhenDragged()
    {
        if (_draggableProperties == null)
        {
            ResetValues();

            return;
        }

        if (!_draggableProperties.GetDragged() || _blockPlaced)
        {
            ResetValues();

            return;
        }

        if (_objectCanvas != null)
        {
            if(_objectCanvas.GetCurrentBlock() == null)
            {
                _objectCanvas.SetCurrentBlock(this);
            }
        }

        gameObject.transform.rotation = Quaternion.identity;

        _draggableProperties.GetBody().useGravity = false;

        _draggableProperties.GetBody().velocity = new Vector3(0.0f, 0.0f, 0.0f);

        _draggableProperties.GetBody().angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void MaintainVelocity()
    {
        if(_draggableProperties.GetDragged() || _blockPlaced || _draggableProperties.GetBody() == null)
        {
            return;
        }

        Vector3 _vel = _draggableProperties.GetBody().velocity;

        _vel.x = 0.0f;

        //_vel.y = _draggableProperties.GetBody().velocity.y;

        _vel.z = 0.0f;

        _draggableProperties.SetBodyVelocity(_vel);

        Vector3 _aVel = _draggableProperties.GetBody().angularVelocity;

        _aVel.x = 0.0f;

        _aVel.z = 0.0f;

        _draggableProperties.SetBodyAngularVelocity(_aVel);
    }

    void ResetValues()
    {
        if(_objectCanvas != null)
        {
            if(_objectCanvas.GetCurrentBlock() == this)
            {
                _objectCanvas.SetCurrentBlock(null);
            }
        }
    }
}
