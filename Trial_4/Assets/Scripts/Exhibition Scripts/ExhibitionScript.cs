using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhibitionScript : MonoBehaviour
{
    [SerializeField]
    Transform _parent;

    [SerializeField]
    MeterClass _objectsComplete;

    [SerializeField]
    Camera _camera;

    [SerializeField]
    List<ExhibitionGroupClass> _exhibitionGroups;

    [SerializeField]
    CircleClass _circleProperties;

    [SerializeField]
    float _objectHeight = 5.0f;

    [SerializeField]
    ExhibitionCanvasScript _canvas;

    [SerializeField]
    Material _exhibitionHighlightingMaterial1;

    [SerializeField]
    Material _exhibitionHighlightingMaterial2;

    [SerializeField]
    TwoVariablesClass<float, float> _highlightMaterialSizes;

    [SerializeField]
    float _highlightMaterialAnimationSpeed = 2.0f;

    [SerializeField]
    float _additionalThichnessFotMat2 = 0.5f;

    [SerializeField]
    float _exhibitionUniformScale = 1.0f;

    //[SerializeField]
    ExhibitionGroupClass _currentGroup;

    bool _exhibitionOn = false;

    List<ExhibitionObjectScript> _exhibitions = new List<ExhibitionObjectScript>();

    List<GameObject> _exhibitionsGO = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //_exhibitionObjects = new List<GameObject>();

        _objectsComplete = new MeterClass();

        if(_highlightMaterialSizes.GetVariable1() > _highlightMaterialSizes.GetVariable2())
        {
            float _v = _highlightMaterialSizes.GetVariable1();

            _highlightMaterialSizes.SetVariable1(_highlightMaterialSizes.GetVariable2());

            _highlightMaterialSizes.SetVariable2(_v);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public MeterClass GetObjectComplete()
    {
        return _objectsComplete;
    }

    public bool GetExhibitionOn()
    {
        return _exhibitionOn;
    }

    public Camera GetCamera()
    {
        return _camera;
    }

    public Material GetExhibitionHighlightingMaterial1()
    {
        return _exhibitionHighlightingMaterial1;
    }

    public Material GetExhibitionHighlightingMaterial2()
    {
        return _exhibitionHighlightingMaterial2;
    }

    public TwoVariablesClass<float, float> GetHighlightMaterialSizes()
    {
        return _highlightMaterialSizes;
    }

    public float GetHighlightMaterialAnimationSpeed()
    {
        return _highlightMaterialAnimationSpeed;
    }

    public float GetAdditionalThicknessForMat2()
    {
        return _additionalThichnessFotMat2;
    }

    public float GetExhibitionUniformScale()
    {
        return _exhibitionUniformScale;
    }

    public void StartExhibition(int _input)
    {
        if(!(_input >= 0 && _input < _exhibitionGroups.Count))
        {
            return;
        }

        string _name = _exhibitionGroups[_input].GetGroupName();

        StartExhibition(_name);
    }

    public void StartExhibition(string _input)
    {
        if(_exhibitionOn || _parent == null)
        {
            return;
        }

        _currentGroup = GetGroupByName(_input);

        if(_currentGroup == null)
        {
            return;
        }

        int _divValue = ToolsStruct.GetCircleDivision(_circleProperties.GetSpreadingDegrees(), _currentGroup.GetExhibitionGroupListItems().Count);

        int _count = _currentGroup.GetExhibitionGroupListItems().Count;

        float _angle = 0.0f;

        Vector3 _v3 = Vector3.zero;

        ExhibitionObjectScript _currentExh;

        GameObject _go;

        ExhibitionListItemClass _currentListItem;

        MeshCollider _collider;

        //MeshCollider _collider2;

        float _rotAngle;

        for(int _i = 0; _i < _count; _i++)
        {
            _angle = (_i * _circleProperties.GetSpreadingDegrees()) / _divValue;

            _angle = _angle + _circleProperties.GetAdditionalDegrees();

            _rotAngle = 90.0f - _angle;

            if(_circleProperties.GetIsCounterClockwise())
            {
                _angle = -_angle;
            }

            _v3.x = Mathf.Cos(_angle * Mathf.Deg2Rad) * _circleProperties.GetRadius();

            _v3.z = Mathf.Sin(_angle * Mathf.Deg2Rad) * _circleProperties.GetRadius();

            if(_circleProperties.GetSwitchAxes())
            {
                float _pos = _v3.x;

                _v3.x = _v3.z;

                _v3.z = _pos;
            }

            _v3.y = _objectHeight;

            _currentListItem = _currentGroup.GetExhibitionGroupListItems()[_i];

            _go = Instantiate(_currentListItem.GetListItemGameObject());

            _go.transform.parent = _parent;

            _go.transform.localPosition = _v3;

            _go.transform.localEulerAngles = new Vector3(0.0f, _rotAngle, 0.0f);

            _go.transform.localScale = (Vector3.one * _currentListItem.GetLocalScaleConstant() * _exhibitionUniformScale * _currentGroup.GetExhibitionGroupUniformScale());

            _collider = _go.GetComponent<MeshCollider>();

            //_collider2 = _go.AddComponent<MeshCollider>();

            _exhibitionsGO.Add(_go);

            _currentExh = _go.AddComponent<ExhibitionObjectScript>();

            _currentExh.SetObjectNumber(_i + 1);

            _currentExh.SetObjectName(_currentListItem.GetListItemName());

            _currentExh.SetObjectDescription(_currentListItem.GetListItemDescription());

            _currentExh.SetObjectColor(_currentListItem.GetListItemColor());

            _currentExh.SetObject(_go);

            _currentExh.SetCamera(_camera);

            if (_collider != null)
            {
                _currentExh.SetObjectCollider(_collider);
            }

            _currentExh.SetObjectPosition(_v3);

            _currentExh.SetObjectYRotation(_rotAngle);

            _currentExh.SetExhibitionCanvas(_canvas);

            _currentExh.SetOutlineThickness(_currentListItem.GetOutlineThickness());

            _currentExh.SetInitialRotation(_go.transform.localRotation);

            _currentExh.SetTalkingSeconds(_currentListItem.GetTalkingSeconds());

            _currentExh.SetGroup(_currentGroup);

            _currentExh.SetExhibitionRaycastOn(true);

            if(_currentGroup.GetRotateInRaycast())
            {
                GiveRotationalProperties(_currentExh);
            }
            else
            {
                _currentExh.SetRotateWhenHit(false);
            }

            _exhibitions.Add(_currentExh);
        }

        _exhibitionOn = true;

        _currentGroup.SetExhibitionInPlay(true);
    }

    public void EndExhibition()
    {
        if(!_exhibitionOn)
        {
            return;
        }

        _currentGroup.SetExhibitionInPlay(false);

        foreach(GameObject _go in _exhibitionsGO)
        {
            Destroy(_go);
        }

        _exhibitionsGO.Clear();

        _exhibitions.Clear();

        _currentGroup = null;

        _exhibitionOn = false;
    }

    void GiveRotationalProperties(ExhibitionObjectScript _input)
    {
        if(_input == null)
        {
            return;
        }

        GameObject _go = _input.gameObject;

        RotationScript _rp;

        if(_go.GetComponent<RotationScript>() == null)
        {
            _rp = _go.AddComponent<RotationScript>();
        }
        else
        {
            _rp = _go.GetComponent<RotationScript>();
        }

        _input.SetRotateWhenHit(true);

        _rp.SetDeltaTime(true);

        _rp.SetAxis(Vector3.up);

        _rp.SetConstant(100.0f);

        _input.SetRotationProperties(_rp);
    }

    ExhibitionGroupClass GetGroupByName(string _nameInput)
    {
        ExhibitionGroupClass _g;

        for(int _i = 0; _i < _exhibitionGroups.Count; _i++)
        {
            _g = _exhibitionGroups[_i];

            bool _found = string.Compare(_g.GetGroupName(), _nameInput, true) == 0;

            if (_found)
            {
                return _g;
            }
        }

        return null;
    }

    public void SetRaycastOn(bool _input)
    {
        foreach(ExhibitionObjectScript _exhO in _exhibitions)
        {
            _exhO.SetExhibitionRaycastOn(_input);
        }
    }
}


[System.Serializable]
public class ExhibitionGroupClass
{
    [SerializeField]
    string _groupName;

    [SerializeField]
    List<ExhibitionListItemClass> _exhibitionGroupListItems;

    [SerializeField]
    MeterClass _groupCompletionMeter;

    [SerializeField]
    bool _rotateInRaycast = true;

    [SerializeField]
    float _exhibitionGroupUniformScale = 1.0f;

    bool _exhibitionInPlay = false;

    public string GetGroupName()
    {
        return _groupName;
    }

    public List<ExhibitionListItemClass> GetExhibitionGroupListItems()
    {
        return _exhibitionGroupListItems;
    }

    public bool GetExhibitionInPlay()
    {
        return _exhibitionInPlay;
    }

    public MeterClass GetGroupCompletionMeter()
    {
        return _groupCompletionMeter;
    }

    public bool GetRotateInRaycast()
    {
        return _rotateInRaycast;
    }

    public void SetExhibitionInPlay(bool _input)
    {
        _exhibitionInPlay = _input;
    }

    public float GetExhibitionGroupUniformScale()
    {
        return _exhibitionGroupUniformScale;
    }
}

[System.Serializable]
public class ExhibitionListItemClass
{
    [SerializeField]
    GameObject _listItemGameObject;

    [SerializeField]
    string _listItemName;

    [SerializeField]
    string _listItemDescription;

    [SerializeField]
    Color _listItemColor;

    [SerializeField]
    float _localScaleConstant = 10.0f;

    [SerializeField]
    float _outlineThickness = 0.1f;

    [SerializeField]
    float _talkingSeconds = 5.0f;

    public GameObject GetListItemGameObject()
    {
        return _listItemGameObject;
    }

    public string GetListItemName()
    {
        return _listItemName;
    }

    public string GetListItemDescription()
    {
        return _listItemDescription;
    }

    public Color GetListItemColor()
    {
        return _listItemColor;
    }

    public float GetLocalScaleConstant()
    {
        return _localScaleConstant;
    }

    public float GetOutlineThickness()
    {
        return _outlineThickness;
    }

    public float GetTalkingSeconds()
    {
        return _talkingSeconds;
    }
}
