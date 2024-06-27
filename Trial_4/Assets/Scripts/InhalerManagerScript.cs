using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhalerManagerScript : MonoBehaviour
{
    static InhalerManagerScript _instance;

    [SerializeField]
    List<InhalerInformationClass> _inhalerInfoList;

    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static InhalerManagerScript GetInstance()
    {
        return _instance;
    }

    public List<InhalerInformationClass> GetInhalerInfoList()
    {
        return _inhalerInfoList;
    }

    public InhalerInformationClass GetInhalerInformationByName(string _nameInput, bool _capsInput = true)
    {
        for (int _i = 0; _i < _inhalerInfoList.Count; _i++)
        {
            bool _comp = string.Compare(_inhalerInfoList[_i].GetObjectName(), _nameInput, _capsInput) == 0;

            if (_comp)
            {
                return _inhalerInfoList[_i];
            }
        }
        return null;
    }

    public List<InhalerMatchingObjectScript> GetBlocks()
    {
        List<InhalerMatchingObjectScript> _list = new List<InhalerMatchingObjectScript>();

        for (int _i = 0; _i < _inhalerInfoList.Count; _i++)
        {
            _list.Add(_inhalerInfoList[_i].GetInhalerMatchingObjectScript());
        }

        return _list;
    }

    public List<InhalerMatchingObjectHoleScript> GetHoles()
    {
        List<InhalerMatchingObjectHoleScript> _list = new List<InhalerMatchingObjectHoleScript>();

        for(int _i = 0; _i < _inhalerInfoList.Count; _i++)
        {
            _list.Add(_inhalerInfoList[_i].GetHole());
        }

        return _list;
    }

    public List<GameObject> GetObjects()
    {
        List<GameObject> _list = new List<GameObject>();

        for(int _i = 0; _i < _inhalerInfoList.Count; _i++)
        {
            _list.Add(_inhalerInfoList[_i].GetObject());
        }

        return _list;
    }
}


[System.Serializable]
public class InhalerInformationClass
{
    [SerializeField]
    string _objectName;

    [SerializeField]
    GameObject _object;

    [SerializeField]
    InhalerMatchingObjectScript _matchingGameObject;

    [SerializeField]
    InhalerMatchingObjectHoleScript _matchingGameHole;

    [SerializeField]
    Color _objectColor;

    [SerializeField]
    string _objectDesc;

    [SerializeField]
    bool _objectIncludedInMatchingGame = true;

    public string GetObjectName()
    {
        return _objectName;
    }

    public GameObject GetObject()
    {
        return _object;
    }

    public InhalerMatchingObjectScript GetInhalerMatchingObjectScript()
    {
        return _matchingGameObject;
    }

    public string GetObjectDescription() {
        return _objectDesc;
    }

    public Color GetObjectColor()
    {
        return _objectColor;
    }

    public Color GetColorWithChangedAlpha(float _alphaInput = 1.0f)
    {
        if(!(_alphaInput >= 0.0 && _alphaInput < 1.0f))
        {
            return _objectColor;
        }

        Color _c = _objectColor;

        _c.a = _alphaInput;

        return _c;
    }

    public Color GetColorWithDifferentShade(float _shadeInput = 1.0f)
    {
        if(_shadeInput <= 0.0f)
        {
            return _objectColor;
        }

        Color _c = _objectColor;

        _c.r = _c.r / _shadeInput;

        if(_c.r > 1.0f)
        {
            _c.r = 1.0f;
        }

        _c.g = _c.g / _shadeInput;

        if(_c.g > 1.0f)
        {
            _c.g = 1.0f;
        }

        _c.b = _c.b / _shadeInput;

        if(_c.b > 1.0f)
        {
            _c.b = 1.0f;
        }

        return _c;
    }

    public Color GetColorWitrhDifferentShadeOrAlpha(float _shadeInput = 1.0f, float _alphaInput = 1.0f)
    {
        Color _c1 = GetColorWithDifferentShade(_shadeInput);

        Color _c2 = GetColorWithChangedAlpha(_alphaInput);

        Color _finalColor = _c1;

        _finalColor.a = _c2.a;

        return _finalColor;
    }


    public GameObject InstantiateMatchingGameObject()
    {
        GameObject _go = GameObject.Instantiate(_matchingGameObject.gameObject);

        return _go;
    }

    public InhalerMatchingObjectHoleScript GetHole()
    {
        return _matchingGameHole;
    }

    public bool GetObjectIncludedInMatchingGame()
    {
        return _objectIncludedInMatchingGame;
    }
}