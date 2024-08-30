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
    float _objectYRotation;

    [SerializeField]
    ExhibitionCanvasScript _exhibitionCanvas;

    [SerializeField]
    float _outlineThickness = 0.1f;

    [SerializeField]
    RotationScript _rotationProperties;

    [SerializeField]
    float _talkingSeconds;

    Quaternion _initalRotation;

    bool _highlighted = false;

    bool _rotateWhenHit;

    bool _descriptionComplete = false;

    ExhibitionGroupClass _objectGroup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PrepareExhibition();
    }

    //Getters

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

    public float GetObjectYRotation()
    {
        return _objectYRotation;
    }

    public ExhibitionCanvasScript GetExhibitionCanvas()
    {
        return _exhibitionCanvas;
    }

    public float GetOutlineThickness()
    {
        return _outlineThickness;
    }

    public Quaternion GetInitalRotation()
    {
        return _initalRotation;
    }

    public RotationScript GetRotationProperties()
    {
        return _rotationProperties;
    }

    public float GetTalkingSeconds()
    {
        return _talkingSeconds;
    }

    public ExhibitionGroupClass GetGroup()
    {
        return _objectGroup;
    }

    //Setters

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

    public void SetObjectYRotation(float _input)
    {
        _objectYRotation = _input;
    }

    public void SetExhibitionCanvas(ExhibitionCanvasScript _input)
    {
        _exhibitionCanvas = _input;
    }

    public void SetOutlineThickness(float _input)
    {
        _outlineThickness = _input;
    }

    public void SetInitialRotation(Quaternion _input)
    {
        _initalRotation = _input;
    }

    public void SetRotateWhenHit(bool _input)
    {
        _rotateWhenHit = _input;
    }

    public void SetRotationProperties(RotationScript _input)
    {
        _rotationProperties = _input;
    }

    public void SetTalkingSeconds(float _input)
    {
        _talkingSeconds = _input;
    }

    public void SetGroup(ExhibitionGroupClass _input)
    {
        _objectGroup = _input;
    }

    void PrepareExhibition()
    {
        if(_objectCollider == null || _camera == null)
        {
            Debug.LogError("We cannot make a raycast for " + @"""" + _objectName + @"""" + ".");

            return;
        }

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

        Ray _ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0.0f));

        if(Physics.Raycast(_ray, out  _hit, _raycastDistance))
        {
            if(_hit.collider == _objectCollider)
            {
                if(_exhibitionCanvas.GetCurrentObject() == null)
                {
                    _exhibitionCanvas.SetCurrentObject(this);

                    Debug.Log("The camera hit " + @"""" + _objectName + @"""" + ".");
                }
            }
            else if(_exhibitionCanvas.GetCurrentObject() == this && _hit.collider != _objectCollider)
            {
                _exhibitionCanvas.SetCurrentObject(null);

                Debug.Log("The camera is not hitting anything now.");
            }
        }
        else if(_exhibitionCanvas.GetCurrentObject() == this)
        {
            _exhibitionCanvas.SetCurrentObject(null);

            Debug.Log("The camera is not hitting anything now.");
        }

        if (_exhibitionCanvas.GetExhibition().GetExhibitionHighlightingMaterial() != null)
        {
            if (_exhibitionCanvas.GetCurrentObject() == this && !_highlighted)
            {
                SetObjectLayer(gameObject, 7);

                _exhibitionCanvas.GetExhibition().GetExhibitionHighlightingMaterial().SetColor("_Base_Color", _objectColor);

                //float _thickness = _exh

                Vector3 _globalS = ToolsStruct.GetGlobalScale(gameObject.transform);

                float _size = ToolsStruct.GetVectorAverage(_globalS);

                _exhibitionCanvas.GetExhibition().GetExhibitionHighlightingMaterial().SetFloat("_Outline_Thickness", _outlineThickness);

                if(_rotateWhenHit && _rotationProperties != null)
                {
                    _rotationProperties.SetDoAction(true);
                }

                _highlighted = true;
            }
            else if (_exhibitionCanvas.GetCurrentObject() != this && _highlighted)
            {
                SetObjectLayer(gameObject, 0);

                if(_rotateWhenHit && _rotationProperties != null)
                {
                    _rotationProperties.SetDoAction(false);

                    gameObject.transform.localRotation = _initalRotation;
                }

                _highlighted = false;
            }
        }
    }

    void SetObjectLayer(GameObject _goInput, int _layerInput)
    {
        _goInput.layer = _layerInput;

        foreach(Transform _t in _goInput.transform)
        {
            _t.gameObject.layer = _layerInput;

            Transform _hasChildren = _t.GetComponentInChildren<Transform>();

            if(_hasChildren != null)
            {
                SetObjectLayer(_t.gameObject, _layerInput);
            }
        }
    }

    public void SetDescriptionComplete()
    {
        if(_descriptionComplete || _objectGroup == null)
        {
            return;
        }

        _objectGroup.GetGroupCompletionMeter().AddToValue(1);

        _descriptionComplete = true;
    }
}
