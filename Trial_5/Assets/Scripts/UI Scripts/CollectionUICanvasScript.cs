using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionUICanvasScript : CanvasScript
{

    [Header("2. Variables of any Collection UI Canvas.")]
    [SerializeField]
    protected Canvas _selectedItemCanvas;

    [SerializeField]
    protected GameObject _rowObjectTemplate;

    [SerializeField]
    protected float _desiredHeight = 500.0f;

    [SerializeField]
    protected float _additionalHeightForContent = 100.0f;    

    [SerializeField]
    protected RectTransform _contentArea;

    [SerializeField]
    protected List<GameObject> _rowObjects;

    [SerializeField]
    protected Vector2 _initialPosition;

    [SerializeField]
    protected Vector2 _distance;

    [SerializeField]
    protected CollectionUIProperties _collectionUIProperties;

    protected GameObject _currentRowObject;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Canvas GetSelectedItemCanvas()
    {
        return _selectedItemCanvas;
    }

    public virtual void ClearList()
    {

    }

    public virtual void ShowItems()
    {

    }

    protected void ContentAreaHeightFunction()
    {
        if(_contentArea == null)
        {
            return;
        }

        Vector2 _ds = _contentArea.sizeDelta;

        _ds.y = (_desiredHeight * _rowObjects.Count) + _additionalHeightForContent;

        _contentArea.sizeDelta = _ds;
    }
}
