using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadgesManagerScript : MonoBehaviour, IDataPersistenceScript
{
    static BadgesManagerScript _instance;

    [SerializeField]
    Sprite _unknownBadgeSprite;

    [SerializeField]
    List<BadgeScript> _badges;

    [SerializeField]
    MeterClass _badgesCollectedMeter;

    [ContextMenu("Generate IDs for Badges")]
    void GenerateIDs()
    {
        foreach(BadgeScript _b in _badges)
        {
            if(_b.GetBadgeID().Equals(""))
            {
                string _id = System.Guid.NewGuid().ToString();

                _b.SetID(_id);
            }
        }
    }

    //FileDataHandlerScript _fileDataHandler;

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

        DontDestroyOnLoad(gameObject);

        _badgesCollectedMeter.SetMaxValue(_badges.Count);

        foreach(BadgeScript _b in _badges)
        {
            StartCoroutine(_b.CheckPersistence());
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static BadgesManagerScript GetInstance()
    {
        return _instance;
    }

    public Sprite GetUnknownBadgeSprite()
    {
        return _unknownBadgeSprite;
    }

    public List<BadgeScript> GetBadges()
    {
        return _badges;
    }

    public List<BadgeScript> GetEarnedBadges()
    {
        List<BadgeScript> _list = new List<BadgeScript>();

        foreach(BadgeScript _b in _badges)
        {
            if(_b.GetBadgeCollected())
            {
                _list.Add(_b);
            }
        }

        return _list;
    }

    public List<BadgeScript> GetUnearnedBadges()
    {
        List<BadgeScript> _list = new List<BadgeScript>();

        foreach (BadgeScript _b in _badges)
        {
            if (!_b.GetBadgeCollected())
            {
                _list.Add(_b);
            }
        }

        return _list;
    }

    public BadgeScript GetBadgeByName(string _input)
    {
        foreach(BadgeScript _b in _badges)
        {
            if(string.Compare(_b.GetBadgeName(), _input, true) == 0)
            {
                return _b;
            }
        }

        return null;
    }

    public BadgeScript GetBadgeByName(string _nameInput, bool _capsInput = true)
    {
        foreach(BadgeScript _b in _badges)
        {
            if(string.Compare(_nameInput, _b.GetBadgeName(), _capsInput) == 0)
            {
                return _b;
            }
        }

        return null;
    }

    public MeterClass GetBadgesCollectedMeter()
    {
        return _badgesCollectedMeter;
    }

    public void UpdateBadgesCollectedMeter()
    {
        _badgesCollectedMeter.SetValue(0);

        foreach(BadgeScript _b in _badges)
        {
            if(_b.GetBadgeCollected())
            {
                _badgesCollectedMeter.AddToValue(1);
            }
        }
    }

    public void LoadData(GameDataScript _input)
    {
        foreach(BadgeScript _b2 in _badges)
        {
            string _id2 = _b2.GetBadgeID();

            bool _col;

            _input._badgesCollected.TryGetValue(_id2, out _col);

            _b2.SetBadgeCollected(_col);

            //_input.AddBadge(_b2);
        }
    }

    public void SaveData(ref GameDataScript _input)
    {
        foreach(BadgeScript _b3 in _badges)
        {
            string _id3 = _b3.GetBadgeID();

            if(_input._badgesCollected.ContainsKey(_id3))
            {
                _input._badgesCollected.Remove(_id3);
            }

            _input._badgesCollected.Add(_id3, _b3.GetBadgeCollected());
            //_input.AddBadge(_b3);
        }
    }


    public void SetBadgeEarned(string _nameInput, bool _capsInput = true)
    {
        SetBadgeEarned(GetBadgeByName(_nameInput, _capsInput));
    }

    public void SetBadgeEarned(BadgeScript _input)
    {
        if(_input == null || DataPersistenceManager.GetInstance() == null)
        {
            return;
        }


        /*if(DataPersistenceManager.GetInstance().GetGameData() == null)
        {
            return;
        }*/

        if(!_input.GetBadgeCollected())
        {
            _badgesCollectedMeter.AddToValue(1);
        }

        _input.SetBadgeCollected(true);

        string _id4 = _input.GetBadgeID();

        if(DataPersistenceManager.GetInstance().GetGameData() == null)
        {
            return;
        }

        DataPersistenceManager.GetInstance().GetGameData()._badgesCollected[_id4] = true;

        SaveResultForBadge(ref _input);
    }

    public void SaveResultForBadge(ref BadgeScript _input)
    {
        /*if(DataPersistenceManager.GetInstance() == null)
        {
            return;
        }

        if(DataPersistenceManager.GetInstance().GetGameData() == null)
        {
            return;
        }*/

        GameDataScript _d = DataPersistenceManager.GetInstance().GetGameData();

        _input.SaveData(ref _d);

        DataPersistenceManager.GetInstance().SaveGame();
    }
}
