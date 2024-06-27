using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.CoreModule;

[System.Serializable]
public class BadgeScript : IDataPersistenceScript
{
    [SerializeField]
    string _badgeName;

    [SerializeField]
    string _badgeDescription;

    [SerializeField]
    bool _badgeCollected;

    [SerializeField]
    Sprite _badgeSprite;

    [SerializeField]
    string _badgeID;

    [SerializeField]
    string _howToEarn;

    public string GetBadgeName()
    {
        return _badgeName;
    }

    public string GetBadgeDescription() { return _badgeDescription; }

    public bool GetBadgeCollected() { return _badgeCollected; }

    public void SetBadgeCollected(bool _input) { _badgeCollected = _input; }

    public Sprite GetBadgeSprite() { return _badgeSprite; }

    public string GetBadgeID()
    {
        return _badgeID;
    }

    public string GetHowToEarn()
    {
        return _howToEarn;
    }

    public void SetID(string _input)
    {
        _badgeID = _input;
    }

    public void LoadData(GameDataScript _input)
    {
        _input._badgesCollected.TryGetValue(_badgeID, out _badgeCollected);

        _input.AddBadge2(this);

        //_input.AddBadge(this);
    }

    public void SaveData(ref GameDataScript _input)
    { 
        if(_input._badgesCollected.ContainsKey(_badgeID))
        {
            _input._badgesCollected.Remove(_badgeID);
        }

        _input._badgesCollected.Add(_badgeID, _badgeCollected);

        //_input.AddBadge(this);
    }

    public IEnumerator CheckPersistence()
    {
        yield return new WaitForSeconds(1.0f);

        if(DataPersistenceManager.GetInstance() == null)
        {
            yield break;
        }

        if (!DataPersistenceManager.GetInstance().GetGameData()._badgesCollected.ContainsKey(_badgeID))
        {
            DataPersistenceManager.GetInstance().GetGameData().AddBadge2(this);
        }
        else
        {
            //Debug.Log("Pers manager already has \\" + _badgeName + "\\.");

            bool _col = DataPersistenceManager.GetInstance().GetGameData()._badgesCollected[_badgeID];

            _badgeCollected = _col;
        }
    }
}
