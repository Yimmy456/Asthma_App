using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableManagerClass : MonoBehaviour
{
    static DraggableManagerClass _instance;

    [SerializeField]
    List<DraggableClass> _draggables;

    [SerializeField]
    DraggableClass _currentlyDraggedObject;

    TouchPhase _touchPhase = TouchPhase.Canceled;

    [SerializeField]
    bool _draggingOn = true;

    List<DraggableClass> _beginsDragging = new List<DraggableClass>();

    List<DraggableClass> _isDragging = new List<DraggableClass>();

    List<DraggableClass> _endsDragging = new List<DraggableClass>();

    float _zIndexConstant = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        if(_instance == null)
        {
            _instance = this;
        }else if(_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(!_draggingOn)
        {
            _touchPhase = TouchPhase.Canceled;
        }
    }

    public static DraggableManagerClass GetInstance()
    {
        return _instance;
    }

    public List<DraggableClass> GetDraggables()
    {
        return _draggables;
    }

    public DraggableClass GetCurrentlyDraggedObject()
    {
        return _currentlyDraggedObject;
    }

    public bool GetDraggableOn()
    {
        return _draggingOn;
    }

    public bool GetDraggingSomething()
    {
        return (_currentlyDraggedObject != null);
    }

    public TouchPhase GetTouchPhase()
    {
        return _touchPhase;
    }

    public float GetZIndexConstant()
    {
        return _zIndexConstant;
    }

    public void AddDraggable(DraggableClass _input)
    {
        _draggables.Add(_input);
    }

    public void RemoveDraggable(DraggableClass _input)
    {
        _draggables.Remove(_input);
    }

    public void SetCurrentlyDraggedObject(DraggableClass _input)
    {
        _currentlyDraggedObject = _input;
    }

    public void SetDraggingOn(bool _input)
    {
        _draggingOn = _input;
    }

    public void SetTouchPhase(TouchPhase _input)
    {
        _touchPhase = _input;
    }

    public List<DraggableClass> GetBeginsDraggingList()
    {
        return _beginsDragging;
    }

    public List<DraggableClass> GetIsDraggingList()
    {
        return _isDragging;
    }

    public List<DraggableClass> GetEndsDragging()
    {
        return _endsDragging;
    }

    public void AddToBeginsDragging(DraggableClass _input)
    {
        _beginsDragging.Add(_input);
    }

    public void AddToIsDragging(DraggableClass _input)
    {
        if(!_beginsDragging.Contains(_input))
        {
            return;
        }

        _isDragging.Add(_input);

        _beginsDragging.Remove(_input);
    }

    public void AddToEndsDragging(DraggableClass _input)
    {
        if(!_beginsDragging.Contains( _input) && !_isDragging.Contains(_input))
        {
            return;
        }

        _endsDragging.Add(_input);

        if(_beginsDragging.Contains(_input))
        {
            _beginsDragging.Remove(_input);
        }
        else if(_isDragging.Contains(_input))
        {
            _isDragging.Remove(_input);
        }
    }

    public void RemoveFromEndsDragging(DraggableClass _input)
    {
        if(!_endsDragging.Contains(_input))
        {
            return;
        }

        _endsDragging.Remove(_input);
    }

    public void SetZConstant(ResizingSceneSliderScript _input)
    {
        if(_input == null)
        {
            return;
        }

        _zIndexConstant = _input.GetScale();
    }

    public bool GetDraggingBegins(DraggableClass _input)
    {
        return (_beginsDragging.Contains(_input));
    }

    public bool GetDraggingContinues(DraggableClass _input)
    {
        return (_isDragging.Contains(_input));
    }

    public bool GetDraggingEnds(DraggableClass _input)
    {
        return (_endsDragging.Contains( _input));
    }

    public bool GetNotDragging(DraggableClass _input)
    {
        bool _b = !_beginsDragging.Contains(_input);

        _b = _b && !_isDragging.Contains(_input);

        _b = _b && !_endsDragging.Contains(_input);

        return _b;
    }
}
