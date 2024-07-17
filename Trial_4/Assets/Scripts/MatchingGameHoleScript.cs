using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingGameHoleScript : MonoBehaviour
{
    [Header("1. Variables for All Matching Block Holes")]

    [SerializeField]
    protected Renderer _renderer;

    [SerializeField]
    protected GameObject _holeObject;

    [SerializeField]
    protected float _expandedSizeValue = 1.5f;

    [SerializeField]
    protected float _defaultSizeValue = 1.0f;

    [SerializeField]
    protected float _draggingDistance = 5.0f;

    [SerializeField]
    protected float _animationSpeed = 1.0f;

    [SerializeField]
    protected Camera _camera;

    [SerializeField]
    protected PlayerController _playerController;

    [SerializeField]
    protected Collider _collider;

    [SerializeField]
    protected DraggableTypeEnum _draggableTypeNeeded;

    [SerializeField]
    public ErrorIdentifierClass _errorIdentifier;

    [SerializeField]
    protected MatchingGameCanvasScript _holeCanvas;

    protected float _currentSizeValue;

    protected bool _objectPlaced = false;

    protected IncreasOrDecreaseEnum _animIncreaseOrDecrease = IncreasOrDecreaseEnum.None;

    protected Coroutine _animationCoroutine;

    //protected static MatchingGameHoleScript _selectedHole;

    protected bool _hoveredOn = false;

    protected MatchingGameBlockScript _currentObject;

    protected Vector3 _originalSize;

    protected bool _canEvaluate = false;

    public MatchingGameHoleScript()
    {
        if (_holeObject != null) { _originalSize = _holeObject.transform.localScale; }
        else { _originalSize = Vector3.one; }

        _currentSizeValue = _defaultSizeValue;

        _originalSize = _originalSize * _currentSizeValue;

        if(_holeObject != null)
        {
            _holeObject.transform.localScale = _originalSize;
        }
    }

    public Renderer GetRenderer()
    {
        return _renderer;
    }

    public float GetExpandedSizeValue()
    {
        return _expandedSizeValue;
    }

    public float GetDefaultSizeValue()
    {
        return _defaultSizeValue;
    }

    public float GetCurrentSizeValue()
    {
        return _currentSizeValue;
    }

    public bool GetObjectPlaced()
    {
        return _objectPlaced;
    }

    public Collider GetCollider()
    {
        return _collider;
    }

    public MatchingGameCanvasScript GetMatchingGameCanvas()
    {
        return _holeCanvas;
    }

    public void SetCamera(Camera _input)
    {
        _camera = _input;
    }

    protected virtual IEnumerator DecreaseSizeAnimation()
    {
        while (_currentSizeValue > _defaultSizeValue)
        {
            _currentSizeValue = (_currentSizeValue - (Time.deltaTime * _animationSpeed));

            if (_currentSizeValue <= _defaultSizeValue)
            {
                _currentSizeValue = _defaultSizeValue;
            }

            _holeObject.transform.localScale = (_originalSize * _currentSizeValue);

            yield return null;
        }

        _animIncreaseOrDecrease = IncreasOrDecreaseEnum.None;
    }

    protected virtual IEnumerator IncreaseSizeAnimation()
    {
        while (_currentSizeValue < _expandedSizeValue)
        {
            _currentSizeValue = (_currentSizeValue + (Time.deltaTime * _animationSpeed));

            if (_currentSizeValue >= _expandedSizeValue)
            {
                _currentSizeValue = _expandedSizeValue;
            }

            _holeObject.transform.localScale = (_originalSize * _currentSizeValue);

            yield return null;
        }

        _animIncreaseOrDecrease = IncreasOrDecreaseEnum.None;
    }

    protected bool GetDistanceReached(Vector3 _input)
    {
        Vector2 _screenPos = _camera.WorldToScreenPoint(gameObject.transform.position);

        float _dist = Vector2.Distance(_input, _screenPos);

        bool _finalBool = _dist >= 0.0f && _dist <= _draggingDistance;

        return _finalBool;
    }

    public virtual void SetHoleGameCanvas(MatchingGameCanvasScript _input)
    {
        _holeCanvas = _input;
    }

    protected virtual void SeeObject()
    {

    }

    protected bool CheckBasicNeeds()
    {
        if (DraggableManagerClass.GetInstance() == null || _camera == null || _playerController == null || _holeCanvas == null || _objectPlaced)
        {
            if(_errorIdentifier.DisplayHere())
            {
                _errorIdentifier.DisplayTextByIndex(0);
            }

            ResetValues();

            return false;
        }

        if (DraggableManagerClass.GetInstance().GetCurrentlyDraggedObject() == null)
        {
            if(_errorIdentifier.DisplayHere())
            {
                _errorIdentifier.DisplayTextByIndex(1);
            }

            ResetValues();

            return false;
        }

        if (DraggableManagerClass.GetInstance().GetCurrentlyDraggedObject().GetDraggableType() != _draggableTypeNeeded)
        {
            if(_errorIdentifier.DisplayHere())
            {
                _errorIdentifier.DisplayTextByIndex(2);
            }

            ResetValues();

            return false;
        }

        return true;
    }

    protected void ResetValues()
    {
        if(_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);

            _animIncreaseOrDecrease = IncreasOrDecreaseEnum.None;
        }

        if(_holeCanvas != null)
        {
            if(_holeCanvas.GetCurrentBlocksAndHoles().GetSelectedHole() == this)
            {
                _holeCanvas.GetCurrentBlocksAndHoles().SetSelectedHole(null);
            }
        }

        if(_animIncreaseOrDecrease != IncreasOrDecreaseEnum.Decrease && _currentSizeValue > _defaultSizeValue)
        {
            _animationCoroutine = StartCoroutine(DecreaseSizeAnimation());

            _animIncreaseOrDecrease = IncreasOrDecreaseEnum.Decrease;
        }
    }

    public bool GetIsSelectedHole()
    {
        if(_holeCanvas == null)
        {
            return false;
        }

        return (_holeCanvas.GetCurrentBlocksAndHoles().GetSelectedHole() == this);
    }

    protected void CheckHoveringStatus()
    {
        Vector2 _touchPosition = _playerController.Touch.Position.ReadValue<Vector2>();

        if(Application.platform == RuntimePlatform.WindowsEditor)
        {
            _touchPosition = _playerController.Move.CursorPosition.ReadValue<Vector2>();
        }

        Ray _ray = _camera.ScreenPointToRay(_touchPosition);

        RaycastHit _hit;

        bool _distanceReachedBool = GetDistanceReached(_touchPosition);

        if (Physics.Raycast(_ray, out _hit))
        {
            _hoveredOn = (_hit.collider.transform == _collider.transform) || (_distanceReachedBool);
        }

        _currentObject = _holeCanvas.GetCurrentBlocksAndHoles().GetSelectedBlock();

        if(_currentObject == null && _errorIdentifier.DisplayHere())
        {
            _errorIdentifier._textField.text = "No block was selected.";
        }

        if (_holeCanvas.GetCurrentBlocksAndHoles().GetSelectedHole() == null && _hoveredOn)
        {
            _holeCanvas.GetCurrentBlocksAndHoles().SetSelectedHole(this);
        }
        else if (_holeCanvas.GetCurrentBlocksAndHoles().GetSelectedHole() == this && !_hoveredOn)
        {
            _holeCanvas.GetCurrentBlocksAndHoles().SetSelectedHole(null);
        }

        _canEvaluate = _hoveredOn;

        if (_currentSizeValue < _expandedSizeValue && _holeCanvas.GetCurrentBlocksAndHoles().GetSelectedHole() == this && _animIncreaseOrDecrease != IncreasOrDecreaseEnum.Increase)
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }

            _animationCoroutine = StartCoroutine(IncreaseSizeAnimation());

            _animIncreaseOrDecrease = IncreasOrDecreaseEnum.Increase;
        }

        if (_currentSizeValue > _defaultSizeValue && _holeCanvas.GetCurrentBlocksAndHoles().GetSelectedHole() != this && _animIncreaseOrDecrease != IncreasOrDecreaseEnum.Decrease)
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }

            _animationCoroutine = StartCoroutine(DecreaseSizeAnimation());

            _animIncreaseOrDecrease = IncreasOrDecreaseEnum.Decrease;
        }
    }

    protected virtual void EvaluateHole()
    {
        
    }

    protected virtual bool CheckMatch()
    {
        return true;
    }

    protected virtual void ConfirmMatch()
    {
        _objectPlaced = true;

        if (_renderer != null)
        {
            _renderer.materials[0].SetFloat("_Alpha", 0.0f);
        }

        _collider.enabled = false;

        if (_currentObject.GetDraggableProperties() != null)
        {
            if (_currentObject.GetDraggableProperties().GetBody() != null)
            {
                _currentObject.GetDraggableProperties().GetBody().useGravity = false;

                _currentObject.GetDraggableProperties().SetApplyGravity(false);

                _currentObject.GetDraggableProperties().GetBody().velocity = Vector3.zero;

                _currentObject.GetDraggableProperties().GetBody().angularVelocity = Vector3.zero;

                _currentObject.GetDraggableProperties().RemoveRigidBody();
            }

            _currentObject.GetDraggableProperties().SetDraggableOn(false);
        }

        _currentObject.gameObject.transform.parent = _holeObject.transform;

        _currentObject.gameObject.transform.localPosition = Vector3.zero;

        _currentObject.gameObject.transform.localRotation = Quaternion.identity;

        _currentObject.gameObject.transform.localScale = Vector3.one;

        _currentObject.SetBlockPlaced(true);

        _currentObject.GetCollider().enabled = false;

        _currentObject.SetMatchedHole(this);

        //_holeObject = _currentObject.gameObject;

        _holeCanvas.GetGameProperties().GetMeter().AddToValue(1);

        _holeCanvas.GetGameProperties().SignalToUpdateUI();

        //_holeCanvas.SetProgressUpdated(true);

        if(_holeCanvas.GetProcedureStateMachine() != null)
        {
            if(_holeCanvas.GetProcedureStateMachine().GetMachineOn() && _holeCanvas.GetProcedureStateMachine().GetProcedureCanvas() != null)
            {
                ProcedureStateMachineCanvasScript _procedureCanvas = _holeCanvas.GetProcedureStateMachine().GetProcedureCanvas();

                if(_procedureCanvas.gameObject.activeSelf)
                {
                    _holeCanvas.GetProcedureStateMachine().GetProcedureCompletionMeter().AddToValue(1);

                    _holeCanvas.GetProcedureStateMachine().SignalToUpdateMeter();
                }
            }
        }

        _hoveredOn = false;

        _canEvaluate = false;

        if(_holeCanvas.GetCurrentBlocksAndHoles().GetSelectedBlock() == _currentObject.GetComponent<MatchingGameBlockScript>())
        {
            _holeCanvas.GetCurrentBlocksAndHoles().SetSelectedBlock(null);
        }

        if(_errorIdentifier.DisplayHere())
        {
            _errorIdentifier._textField.color = Color.red;

            _errorIdentifier.DisplayTextByIndex(-1);
        }

        ResetValues();
    }
}
