using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GamePropertiesClass<T>
{
    [SerializeField]
    List<T> _listOfObjects;

    [SerializeField]
    List<GameObject> _listOfObjectsAsGO;

    public List<T> GetListOfObjects() { return _listOfObjects; }

    public void SetListOfObjects(List<T> _input)
    {
        _listOfObjects = _input;
    }

    public void SetListOfObjectsAsGO(List<GameObject> _input)
    {
        _listOfObjectsAsGO = _input;
    }

    public void ClearGame()
    {

        foreach(GameObject _go in _listOfObjectsAsGO)
        {
            Object.Destroy(_go);
        }

        _listOfObjects.Clear();

        _listOfObjectsAsGO.Clear();
    }

    public void AddObjectToList(T _input)
    {
        _listOfObjects.Add(_input);
    }

    public List<GameObject> GetListOfObjectsAsGO()
    {
        return _listOfObjectsAsGO;
    }

    public void AddObjectsAsGO(GameObject _input)
    {
        _listOfObjectsAsGO.Add(_input);
    }

    public void ClearObjectLists()
    {
        foreach(GameObject _obj in _listOfObjectsAsGO)
        {
            GameObject.Destroy(_obj);
        }

        _listOfObjectsAsGO.Clear();

        _listOfObjects.Clear();
    }
}
