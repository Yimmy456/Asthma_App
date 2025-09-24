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

        StayInXnZRange();
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

    protected void MaintainVelocity()
    {
        if(_draggableProperties.GetDragged() || _blockPlaced || _draggableProperties.GetBody() == null)
        {
            return;
        }

        Vector3 _vel = _draggableProperties.GetBody().velocity;

        _vel.x = 0.0f;

        _vel.z = 0.0f;

        _draggableProperties.SetBodyVelocity(_vel);

        Vector3 _aVel = _draggableProperties.GetBody().angularVelocity;

        _aVel.x = 0.0f;

        _aVel.z = 0.0f;

        _draggableProperties.SetBodyAngularVelocity(_aVel);

        MaintainFromFalling();
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

    void MaintainFromFalling()
    {
        float _yPosition = gameObject.transform.localPosition.y;

        if(_yPosition <= -20.0f)
        {
            Debug.LogError(gameObject.name + " is falling! (1)");

            Vector3 _pos = gameObject.transform.localPosition;

            _pos.y = 50.0f;

            gameObject.transform.localPosition = _pos;
        }

        /*_yPosition = gameObject.transform.localPosition.y;

        if (_yPosition <= -20.0f)
        {
            Debug.LogError(gameObject.name + " is falling! (2)");

            Vector3 _pos = gameObject.transform.localPosition;

            _pos.y = 10.0f;

            gameObject.transform.localPosition = _pos;
        }*/
    }

    protected void StayInXnZRange()
    {
        if (_draggableProperties.GetDragged() || _blockPlaced || _draggableProperties.GetBody() == null)
        {
            return;
        }

        Vector3 _pos3 = gameObject.transform.localPosition;

        if(!(_pos3.x <= 900 && _pos3.x >= -900.0f))
        {
            float _randX = Random.Range(-900.0f, 900.0f);

            _pos3.x = _randX;
        }

        if (!(_pos3.z <= 900 && _pos3.z >= -900.0f))
        {
            float _randZ = Random.Range(-900.0f, 900.0f);

            _pos3.z = _randZ;
        }

        gameObject.transform.localPosition = _pos3;
    }
}
