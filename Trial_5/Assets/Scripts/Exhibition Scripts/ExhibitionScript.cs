using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class ExhibitionScript : MonoBehaviour, ExperienceInterface, RewardingBadgeInterface
{
    [SerializeField]
    Transform _parent;

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
    HighlightingAnimationClass _highlightingAnimationProperties;

    [SerializeField]
    float _exhibitionUniformScale = 1.0f;

    [SerializeField]
    DoctorSalemDialoguesClass _doctorDialogue;

    [SerializeField]
    AudioClip _welcomingClip;

    [SerializeField]
    DialoguesScript _dialogues;

    [SerializeField]
    public BadgePreperationClass _badgePreperation;

    bool _exhibitionRaycastOn = false;

    [ContextMenu("Give IDs to exhibits items")]
    void GiveIDs()
    {
        ExhibitionListItemClass _item;

        string _id;

        for (int _i = 0; _i < _exhibitionGroups.Count; _i++)
        {
            for (int _j = 0; _j < _exhibitionGroups[_i].GetExhibitionGroupListItems().Count; _j++)
            {
                _item = _exhibitionGroups[_i].GetExhibitionGroupListItems()[_j];

                _id = _item.GetExhibitID();

                if (_id == "")
                {
                    _id = System.Guid.NewGuid().ToString();

                    _item.SetExhibitID(_id);
                }
            }
        }
    }

    //[SerializeField]
    ExhibitionGroupClass _currentGroup;

    bool _exhibitionOn = false;

    List<ExhibitionObjectScript> _exhibitions = new List<ExhibitionObjectScript>();

    List<GameObject> _exhibitionsGO = new List<GameObject>();

    Coroutine _doctorSalemTalkingCoroutine;

    BadgeScript _currentBadge;

    float _percentageRef;

    // Start is called before the first frame update
    void Start()
    {
        InitializeExhibition();
    }

    void OnEnable()
    {
        InitializeExhibition();
    }

    // Update is called once per frame
    void Update()
    {
        if (_exhibitionOn && _currentGroup != null)
        {
            IUpdateExperience();

            //Debug.Log("Exibition is " + _currentGroup.GetGroupCompletionMeter().GetPercentage().ToString("0.00") + "% complete.");
        }
    }

    public bool GetExhibitionOn()
    {
        return _exhibitionOn;
    }

    public Camera GetCamera()
    {
        return _camera;
    }

    public HighlightingAnimationClass GetHighlightingAnimationProperties()
    {
        return _highlightingAnimationProperties;
    }

    public float GetExhibitionUniformScale()
    {
        return _exhibitionUniformScale;
    }

    public DoctorSalemDialoguesClass GetDoctorDialogue()
    {
        return _doctorDialogue;
    }

    public ExhibitionGroupClass GetCurrentGroup()
    {
        return _currentGroup;
    }

    public bool GetExhibitionRaycastOn()
    {
        return _exhibitionRaycastOn;
    }

    public ExhibitionListItemClass GetExhibitItemByID(string _input)
    {
        ExhibitionListItemClass _item;

        string _id;

        for (int _i = 0; _i < _exhibitionGroups.Count; _i++)
        {
            for (int _j = 0; _j < _exhibitionGroups[_i].GetExhibitionGroupListItems().Count; _j++)
            {
                _item = _exhibitionGroups[_i].GetExhibitionGroupListItems()[_j];

                if (_item == null)
                {
                    continue;
                }

                _id = _item.GetExhibitID();

                if (_id == "")
                {
                    continue;
                }

                if (string.Compare(_id, _input, false) == 0)
                {
                    return _item;
                }
            }
        }

        return null;
    }

    public DialoguesScript GetDialogues()
    {
        return _dialogues;
    }

    public void IStartExperience()
    {

    }

    public void IStartExperience(int _input)
    {
        if (!(_input >= 0 && _input < _exhibitionGroups.Count))
        {
            return;
        }

        string _name = _exhibitionGroups[_input].GetGroupName();

        IStartExperience(_name);
    }

    public void IStartExperience(string _input)
    {
        if (_exhibitionOn || _parent == null)
        {
            return;
        }

        _currentGroup = GetGroupByName(_input);

        if (_currentGroup == null)
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

        _badgePreperation.SetBadgeName(_currentGroup.GetBadgeName());

        if (BadgesManagerScript.GetInstance() != null)
        {
            _currentBadge = BadgesManagerScript.GetInstance().GetBadgeByName(_currentGroup.GetBadgeName());
        }

        for (int _i = 0; _i < _count; _i++)
        {
            _angle = (_i * _circleProperties.GetSpreadingDegrees()) / _divValue;

            _angle = _angle + _circleProperties.GetAdditionalDegrees();

            _rotAngle = 90.0f - _angle;

            if (_circleProperties.GetIsCounterClockwise())
            {
                _angle = -_angle;
            }

            _v3.x = Mathf.Cos(_angle * Mathf.Deg2Rad) * _circleProperties.GetRadius();

            _v3.z = Mathf.Sin(_angle * Mathf.Deg2Rad) * _circleProperties.GetRadius();

            if (_circleProperties.GetSwitchAxes())
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

            _currentExh.SetObjectAudioClip(_currentListItem.GetAudioClip());

            _currentExh.SetID(_currentListItem.GetExhibitID());

            if (_dialogues != null)
            {
                _currentExh.SetDialogues(_dialogues);

                _currentExh.SetObjectAudioClip2(_currentListItem.GetAudioClip2());
            }

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

            _currentExh.SetExhibition(this);

            if (_currentGroup.GetRotateInRaycast())
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

        _exhibitionRaycastOn = true;

        _canvas.ShowStati(false);

        //_objectsComplete = _currentGroup.GetGroupCompletionMeter();

        if (DataPersistenceManager.GetInstance() != null)
        {
            if (!DataPersistenceManager.GetInstance().GetExhibitionIntroduced())
            {
                //PlayDialogue(_welcomingClip);

                PlayDialogue("Dr. Salem Exhibition Introduction");

                DataPersistenceManager.GetInstance().SetExhibitionIntroduced(true);
            }
        }
    }

    public void IResumeExperience()
    {

    }

    public void IUpdateExperience()
    {
        if(_currentGroup.GetGroupCompletionMeter().GetPercentage() == 100.0f && !_currentGroup.GetGroupComplete())
        {
            StartCoroutine(IWaitUntilCompletion());

            _currentGroup.SetGroupComplete(true);
        }
    }

    public IEnumerator IWaitUntilCompletion()
    {
        Debug.Log("Wait for 2 seconds until completion...");

        yield return new WaitForSeconds(2.0f);

        ICompleteExperience();
    }

    public void IStopExperience()
    {
        if (!_exhibitionOn)
        {
            return;
        }

        _currentGroup.SetExhibitionInPlay(false);

        foreach (GameObject _go in _exhibitionsGO)
        {
            Destroy(_go);
        }

        _exhibitionsGO.Clear();

        _exhibitions.Clear();

        if (_badgePreperation.GetNewBadgeCanvas() != null)
        {
            _badgePreperation.GetNewBadgeCanvas().gameObject.SetActive(false);
        }

        _badgePreperation.SetBadgeName("");

        _canvas.GetMainCanvases().SetCanvasesOn(true);

        _currentGroup = null;

        _exhibitionRaycastOn = false;

        _exhibitionOn = false;
    }

    void GiveRotationalProperties(ExhibitionObjectScript _input)
    {
        if (_input == null)
        {
            return;
        }

        GameObject _go = _input.gameObject;

        RotationScript _rp;

        if (_go.GetComponent<RotationScript>() == null)
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

        for (int _i = 0; _i < _exhibitionGroups.Count; _i++)
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
        foreach (ExhibitionObjectScript _exhO in _exhibitions)
        {
            _exhO.SetExhibitionRaycastOn(_input);
        }
    }

    public void SetExhibitionRaycastOn(bool _input)
    {
        _exhibitionRaycastOn = _input;
    }

    public void PlayDialogue(AudioClip _clipInput)
    {
        if (_doctorSalemTalkingCoroutine != null)
        {
            StopCoroutine(_doctorSalemTalkingCoroutine);
        }

        if (_doctorDialogue.GetTalkingCoroutine() != null)
        {
            StopCoroutine(_doctorDialogue.GetTalkingCoroutine());
        }

        _doctorSalemTalkingCoroutine = StartCoroutine(_doctorDialogue.TalkCoroutine(_clipInput, false));

        _doctorDialogue.SetTalkingCoroutine(_doctorSalemTalkingCoroutine);
    }

    public void PlayDialogue(string _input)
    {
        if (_dialogues == null)
        {
            return;
        }

        _dialogues.PlayClip(_input);
    }

    public void IRewardBadge()
    {
        if (_badgePreperation == null || BadgesManagerScript.GetInstance() == null)
        {
            return;
        }

        if (_badgePreperation.GetNewBadgeCanvas() == null)
        {
            return;
        }

        BadgeScript _badge = BadgesManagerScript.GetInstance().GetBadgeByName(_currentGroup.GetBadgeName());

        if (_badge == null)
        {
            return;
        }

        _badgePreperation.GetNewBadgeCanvas().gameObject.SetActive(true);

        Text _newBadgeText = _badgePreperation.GetNewBadgeCanvas().gameObject.GetComponent<RectTransform>().Find("Congratulations Text").gameObject.GetComponent<Text>();

        _exhibitionRaycastOn = false;

        Image _badgeImage = _badgePreperation.GetNewBadgeCanvas().gameObject.GetComponent<RectTransform>().Find("Badge Image").gameObject.GetComponent<Image>();

        if (_badgeImage != null)
        {
            _badgeImage.sprite = _badge.GetBadgeSprite();
        }

        if (_dialogues != null)
        {
            if (!_badge.GetBadgeCollected())
            {
                _dialogues.PlayClip("New Badge Earned");
            }
            else
            {
                _dialogues.PlayClip("Badge Already Earned");
            }
        }

        if(_newBadgeText != null)
        {
            if (!_badge.GetBadgeCollected())
            {
                _newBadgeText.text = "Congratulations! You have earned a new badge! It is the " + @"""" + _badgePreperation.GetBadgeName() + @"""" + " badge!";
            }
            else
            {
                _newBadgeText.text = "Well Done! Although, you have already earned the " + @"""" + _badgePreperation.GetBadgeName() + @"""" + "badge.";
            }
        }    

        BadgesManagerScript.GetInstance().SetBadgeEarned(_badge);

        Button _nextButton = _badgePreperation.GetNewBadgeCanvas().gameObject.GetComponent<RectTransform>().Find("Next Button").gameObject.GetComponent<Button>();

        Button _backButton = _badgePreperation.GetNewBadgeCanvas().gameObject.GetComponent<RectTransform>().Find("Back Button").gameObject.GetComponent<Button>();

        /*if (_gameProperties.GetGameIndicatorCanvas() != null)
        {
            if (_gameProperties.GetGameIndicatorCanvas().gameObject.activeSelf)
            {
                _gameProperties.GetGameIndicatorCanvas().gameObject.SetActive(false);
            }
        }*/

        if (_nextButton != null)
        {
            _nextButton.onClick.AddListener(delegate
            {
                _canvas.QuitExhibition();

                IStopExperience();

                _badgePreperation.GetNewBadgeCanvas().gameObject.SetActive(false);

                _canvas.ShowStati(false);

                if (_backButton != null)
                {
                    _backButton.onClick.RemoveAllListeners();
                }

                _nextButton.onClick.RemoveAllListeners();
            });
        }

        if (_backButton != null)
        {
            _backButton.gameObject.SetActive(true);

            _backButton.onClick.AddListener(delegate
            {
                _canvas.gameObject.SetActive(true);

                _badgePreperation.GetNewBadgeCanvas().gameObject.SetActive(false);

                _exhibitionRaycastOn = true;

                _canvas.ShowStati(false);

                if (_nextButton != null)
                {
                    _nextButton.onClick.RemoveAllListeners();
                }

                _backButton.onClick.RemoveAllListeners();
            });
        }

        _canvas.gameObject.SetActive(false);
    }

    public void IChooseToQuitExperience()
    {

    }

    public void ICompleteExperience()
    {
        if(_currentGroup == null)
        {
            return;
        }

        IRewardBadge();

        _canvas.gameObject.SetActive(false);
    }

    public void IChooseToRestartExperience()
    {

    }

    void InitializeExhibition()
    {
        for(int _i = 0; _i < _exhibitionGroups.Count; _i++)
        {
            int _count = _exhibitionGroups[_i].GetExhibitionGroupListItems().Count;

            _exhibitionGroups[_i].GetGroupCompletionMeter().SetMaxValue(_count);
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

    [SerializeField]
    string _badgeName;

    bool _exhibitionInPlay = false;

    bool _groupComplete = false;

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

    public string GetBadgeName()
    {
        return _badgeName;
    }

    public bool GetGroupComplete()
    {
        return _groupComplete;
    }

    public void SetExhibitionInPlay(bool _input)
    {
        _exhibitionInPlay = _input;
    }

    public float GetExhibitionGroupUniformScale()
    {
        return _exhibitionGroupUniformScale;
    }

    public void CheckIfGroupComplete()
    {
        if(!_groupComplete && _groupCompletionMeter.GetPercentage() == 100.0f)
        {
            _groupComplete = true;
        }
    }

    public void SetGroupComplete(bool _input)
    {
        _groupComplete = _input;
    }

    IEnumerator RewardPlayer()
    {
        yield return new WaitForSeconds(5.0f);

        //BadgesManagerScript.GetInstance().
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

    [SerializeField]
    AudioClip _audioClip;

    [SerializeField]
    string _audioClip2;

    [SerializeField]
    bool _displayExplained;

    [SerializeField]
    string _exhibitID;

    [SerializeField]
    RuntimeAnimatorController _uiAnimatorController;

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

    public AudioClip GetAudioClip()
    {
        return _audioClip;
    }

    public string GetAudioClip2()
    {
        return _audioClip2;
    }

    public bool GetDisplayExplained()
    {
        return _displayExplained;
    }

    public string GetExhibitID()
    {
        return _exhibitID;
    }

    public RuntimeAnimatorController GetUIAnimatorController()
    {
        return _uiAnimatorController;
    }

    public void SetDisplayExplained(bool _input)
    {
        _displayExplained = _input;
    }

    public void SetExhibitID(string _input)
    {
        _exhibitID = _input;
    }
}
