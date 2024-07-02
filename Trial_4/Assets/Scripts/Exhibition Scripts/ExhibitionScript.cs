using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhibitionScript : MonoBehaviour
{
    [SerializeField]
    float _radius = 10.0f;

    [SerializeField]
    Transform _parent;

    [SerializeField]
    MeterClass _objectsComplete;

    [SerializeField]
    Camera _camera;

    bool _exhibitionOn = false;

    List<GameObject> _exhibitionObjects;

    // Start is called before the first frame update
    void Start()
    {
        _exhibitionObjects = new List<GameObject>();

        _objectsComplete = new MeterClass();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetRadius()
    {
        return _radius;
    }

    public void SetRadius(float _input)
    {
        _radius = _input;
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

    public void StartExhibition(List<GameObject> _input)
    {
        if(_parent == null || _exhibitionOn || _camera == null)
        {
            return;
        }

        Vector3 _pos = Vector3.zero;

        float _angle = 0;

        int _count = _input.Count;

        _objectsComplete.SetMaxValue(_count);

        _objectsComplete.SetValue(0);

        GameObject _go;

        for(int _i = 0; _i < _count; _i++)
        {
            _angle = (_i * 360.0f) / _count;

            _angle = _angle * Mathf.Deg2Rad;

            _go = Instantiate(_input[_i]);

            _exhibitionObjects.Add(_go);

            _go.transform.parent = _parent;

            _pos.x = Mathf.Cos(_angle) * _radius;

            _pos.z = Mathf.Sin(_angle) * _radius;

            _go.transform.localPosition = _pos;
        }

        _exhibitionOn = true;
    }

    public void EndExhibition()
    {
        if(!_exhibitionOn)
        {
            return;
        }

        foreach(GameObject _go in _exhibitionObjects)
        {
            Destroy(_go);
        }

        _exhibitionObjects.Clear();

        _exhibitionOn = false;
    }
    


    public List<GameObject> GetExhibitionObjects() { return _exhibitionObjects; }
}
