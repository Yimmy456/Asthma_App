using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhibitionObjectScript : MonoBehaviour
{
    [SerializeField]
    string _objectName;

    [SerializeField]
    int _objectNumber = -1;

    [SerializeField]
    GameObject _exhibitionObject;

    [SerializeField]
    string _objectDescription;

    [SerializeField]
    Color _objectColor;

    [SerializeField]
    Camera _camera;

    [SerializeField]
    Collider _objectCollider;

    [SerializeField]
    Vector3 _objectPosition;

    [SerializeField]
    float _objectRotation;

    [SerializeField]
    ExhibitionCanvasScript _exhibitionCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PrepareExhibition();
    }

    public string GetObjectName()
    {
        return _objectName;
    }

    public int GetObjectNumber()
    {
        return _objectNumber;
    }

    public string GetObjectDescription()
    {
        return _objectDescription;
    }

    public Color GetObjectColor()
    {
        return _objectColor;
    }

    public GameObject GetObject()
    {
        return _exhibitionObject;
    }

    public Camera GetCamera()
    {
        return _camera;
    }

    public Collider GetObjectCollider()
    {
        return _objectCollider;
    }
    
    public Vector3 GetObjectPosition()
    {
        return _objectPosition;
    }

    public float GetObjectRotation()
    {
        return _objectRotation;
    }

    public ExhibitionCanvasScript GetExhibitionCanvas()
    {
        return _exhibitionCanvas;
    }

    public void SetObjectName(string _input)
    {
        _objectName = _input;
    }

    public void SetObjectNumber(int _input)
    {
        _objectNumber = _input;
    }

    public void SetObject(GameObject _input)
    {
        _exhibitionObject = _input;
    }

    public void SetObjectDescription(string _input)
    {
        _objectDescription = _input;
    }

    public void SetObjectColor(Color _input)
    {
        _objectColor = _input;
    }

    public void SetCamera(Camera _input)
    {
        _camera = _input;
    }

    public void SetObjectCollider(Collider _input)
    {
        _objectCollider = _input;
    }

    public void SetObjectPosition(Vector3 _input)
    {
        _objectPosition = _input;
    }

    public void SetObjectRotation(float _input)
    {
        _objectRotation = _input;
    }

    public void SetExhibitionCanvas(ExhibitionCanvasScript _input)
    {
        _exhibitionCanvas = _input;
    }

    void PrepareExhibition()
    {
        float _raycastDistance = 100.0f;

        string _raycastSettingName = "Exhibition Raycast Distance";

        if(SettingsManager.GetInstance() != null)
        {
            if(SettingsManager.GetInstance().GetDecimalSettingByName(_raycastSettingName) != null)
            {
                _raycastDistance = SettingsManager.GetInstance().GetDecimalSettingByName(_raycastSettingName).GetValue();
            }
        }

        RaycastHit _hit;

        //Vector2 _midPoint = new Vector2(Screen.width / 2, Screen.height / 2);

        Ray _ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        if(Physics.Raycast(_ray, out _hit))
        {
            if(_hit.collider == _objectCollider)
            {
                if(_exhibitionCanvas.GetCurrentObject() == null)
                {
                    _exhibitionCanvas.SetCurrentObject(this);
                }
            }
        }
        else if(_exhibitionCanvas.GetCurrentObject() == this)
        {
            _exhibitionCanvas.SetCurrentObject(null);
        }
    }
}
