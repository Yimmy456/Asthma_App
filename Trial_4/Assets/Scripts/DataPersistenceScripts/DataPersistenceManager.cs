using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    [SerializeField]
    string _fileName;

    [SerializeField]
    GameDataScript _data;

    [SerializeField]
    bool _useEncryption;

    //[SerializeField]
    List<IDataPersistenceInterface> _dataPersistentObjects;

    //List<IDataPersistenceScript> _dataPersistentClasses;

    FileDataHandlerScript _fileHandler;

    bool _doctorGreets = false;

    bool _exhibitionIntroduced = false;

    static DataPersistenceManager _instance;

    bool _fileFound = true;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _fileHandler = new FileDataHandlerScript(Application.persistentDataPath, _fileName, _useEncryption);
        //_dataPersistentObjects = FindAllDataPersistenceObject();
        //_dataPersistentClasses = new List<IDataPersistenceScript>();
        _dataPersistentObjects = new List<IDataPersistenceInterface>();

        Debug.Log("File path is: " + _fileHandler + ".");

        _doctorGreets = false;

        LoadGame();
    }

    private void OnEnable()
    {
        if(_dataPersistentObjects == null)
        {
            _dataPersistentObjects = new List<IDataPersistenceInterface>();
        }
    }

    public void NewGame()
    {
        //System.IO.File.WriteAllText(Application.persistentDataPath + "/" + _fileName, "");
        _data = new GameDataScript();
    } 

    public void LoadGame()
    {
        _data = _fileHandler.LoadData();

        if(_data == null)
        {
            NewGame();
        }



        foreach(IDataPersistenceInterface _object in _dataPersistentObjects)
        {
            if (_object != null)
            {
                _object.LoadData(_data);
            }
        }
    }

    public void SaveGame()
    {

        foreach(IDataPersistenceInterface _object in _dataPersistentObjects)
        {
            if (_object != null)
            {
                _object.SaveData(ref _data);
            }
        }

        if (_fileHandler != null && _data != null)
        {
            _fileHandler.SaveData(_data);
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnApplicationPause(bool pause)
    {
        SaveGame();
    }

    /*private void OnApplicationPause(bool pause)
    {
        SaveGame();
    }*/

    private List<IDataPersistenceInterface> FindAllDataPersistenceObject()
    {
        IEnumerable<IDataPersistenceInterface> _dataPersistenceObject = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistenceInterface>();

        return new List<IDataPersistenceInterface>(_dataPersistenceObject);
    }

    public static DataPersistenceManager GetInstance()
    {
        return _instance;
    }

    public GameDataScript GetGameData()
    {
        return _data;
    }

    public bool GetDoctorGreets()
    {
        return _doctorGreets;
    }

    public bool GetExhibitionIntroduced()
    {
        return _exhibitionIntroduced;
    }

    string GetFilePath()
    {
        string _path = Application.persistentDataPath;

        if(Application.platform == RuntimePlatform.Android)
        {
            _path = "/Android/data/com.Manar_Ajhars_Games.Asthma_App/files/";
        }

        return _path;
    }

    public void SetDoctorGreets(bool _input)
    {
        _doctorGreets = _input;
    }

    public void SetExhibitionIntroduced(bool _input)
    {
        _exhibitionIntroduced = _input;
    }
}
