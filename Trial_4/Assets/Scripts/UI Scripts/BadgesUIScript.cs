using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadgesUIScript : CollectionUICanvasScript
{
    [Header("2. Variables of the Badges Canvas.")]
    [SerializeField]
    BadgeDisplayScript _badgeDisplayTemplate;

    [SerializeField]
    Text _badgesEarnedNumberText;

    [SerializeField]
    Text _badgesEarnedPercentageText;

    [SerializeField]
    List<BadgeDisplayScript> _badgeDisplays;

    [SerializeField]
    int _badgesInARow = 3;

    // Start is called before the first frame update
    void Start()
    {
        ShowItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        ShowItems();
    }

    private void OnDisable()
    {
        ClearList();
    }

    public override void ShowItems()
    {
        if(BadgesManagerScript.GetInstance() == null || _badgesInARow <= 0 || _rowObjectTemplate == null)
        {
            return;
        }

        ClearList();

        List<BadgeScript> _la = BadgesManagerScript.GetInstance().GetEarnedBadges();

        List<BadgeScript> _lb = BadgesManagerScript.GetInstance().GetUnearnedBadges();

        List<BadgeScript> _l = new List<BadgeScript>();

        _l.AddRange(_la);

        _l.AddRange(_lb);

        Vector3 _pos = _initialPosition;

        _pos.z = 0.0f;

        if(_badgesEarnedNumberText != null)
        {
            _badgesEarnedNumberText.text = _la.Count.ToString() + " / " + _l.Count.ToString();
        }

        if(_badgesEarnedPercentageText != null)
        {
            BadgesManagerScript.GetInstance().UpdateBadgesCollectedMeter();

            _badgesEarnedPercentageText.text = BadgesManagerScript.GetInstance().GetBadgesCollectedMeter().GetPercentage().ToString("0.00") + "%";

            BadgesManagerScript.GetInstance().GetBadgesCollectedMeter().SetTextColor(_badgesEarnedPercentageText);
        }

        MakeNewRow(ref _currentRowObject, ref _pos.y);

        for(int _i = 0; _i < _l.Count; _i++)
        {
            GameObject _bGO = Instantiate(_badgeDisplayTemplate.gameObject, Vector3.zero, Quaternion.identity);

            BadgeDisplayScript _b = _bGO.GetComponent<BadgeDisplayScript>();

            BadgeScript _currentBadge = _l[_i];

            _bGO.SetActive(true);

            _b.SetDisplayBasedOnBadge(_currentBadge, (_i + 1));

            DelegateDisplayButton(ref _b, _currentBadge, (_i + 1), _currentBadge.GetBadgeCollected());

            _b.GetDisplayTransform().parent = _currentRowObject.GetComponent<RectTransform>();

            _b.GetDisplayTransform().anchoredPosition3D = new Vector3(_pos.x, 0.0f, 0.0f);

            _b.GetDisplayTransform().localRotation = Quaternion.identity;

            _b.GetDisplayTransform().localScale = Vector3.one * 0.5f;

            _b.SetBadgesUI(this);

            if (_i != (_l.Count - 1))
            {
                if (((_i + 1) % _badgesInARow) == 0)
                {
                    _pos.x = _initialPosition.x;

                    MakeNewRow(ref _currentRowObject, ref _pos.y);
                }
                else
                {
                    _pos.x = _pos.x + _distance.x;
                }
            }

            _badgeDisplays.Add(_b);
        }

        ContentAreaHeightFunction();
    }

    public BadgeDisplayScript GetBadgeDisplayTemplate()
    {
        return _badgeDisplayTemplate;
    }

    public Text GetBadgesEarnedNumberText()
    {
        return _badgesEarnedNumberText;
    }

    public Text GetBadgesEarnedPercentageText()
    {
        return _badgesEarnedPercentageText;
    }

    public List<BadgeDisplayScript> GetBadgeDisplays()
    {
        return _badgeDisplays;
    }

    public override void ClearList()
    {
        foreach(BadgeDisplayScript _b in _badgeDisplays)
        {
            Destroy(_b.gameObject);
        }

        foreach(GameObject _r in _rowObjects)
        {
            Destroy(_r);
        }

        _badgeDisplays.Clear();

        _rowObjects.Clear();

        if(_badgesEarnedNumberText != null)
        {
            _badgesEarnedNumberText.text = "";
        }
    }

    void MakeNewRow(ref GameObject _gameObjectInput, ref float _posYInput)
    {
        _gameObjectInput = Instantiate(_rowObjectTemplate);

        _gameObjectInput.GetComponent<RectTransform>().parent = _contentArea.GetComponent<RectTransform>();

        _gameObjectInput.GetComponent<RectTransform>().localRotation = Quaternion.identity;

        _gameObjectInput.GetComponent<RectTransform>().localScale = Vector3.one;

        _gameObjectInput.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0.0f, _posYInput, 0.0f);

        _gameObjectInput.SetActive(true);

        _posYInput = _posYInput - _distance.y;

        _rowObjects.Add(_gameObjectInput);
    }

    void DelegateDisplayButton(ref BadgeDisplayScript _displayInput, BadgeScript _badgeInput, int _indexInput, bool _collectedInput = true)
    {
        if(_displayInput == null || _badgeInput == null || _selectedItemCanvas == null)
        {
            return;
        }

        if(_displayInput.GetButtonComponent() == null)
        {
            return;
        }

        Button _bt = _displayInput.GetButtonComponent();

        Sprite _sp = _collectedInput ? _badgeInput.GetBadgeSprite() : BadgesManagerScript.GetInstance().GetUnknownBadgeSprite();

        string _descT = _collectedInput ? _badgeInput.GetBadgeDescription() : _badgeInput.GetHowToEarn();

        string _nameT = _collectedInput ? _badgeInput.GetBadgeName() : "???";

        _bt.onClick.AddListener(delegate { _selectedItemCanvas.gameObject.SetActive(true); });

        _bt.onClick.AddListener(delegate { _selectedItemCanvas.gameObject.transform.Find("Image").GetComponent<Image>().sprite = _sp; });

        _bt.onClick.AddListener(delegate { _selectedItemCanvas.gameObject.transform.Find("Description Text").GetComponent<Text>().text = _descT; });

        _bt.onClick.AddListener(delegate { _selectedItemCanvas.gameObject.transform.Find("Badge Name Text").GetComponent<Text>().text = _nameT; });

        _bt.onClick.AddListener(delegate { _selectedItemCanvas.gameObject.transform.Find("Index Text").GetComponent<Text>().text = _indexInput.ToString() + "."; });

        _bt.onClick.AddListener(delegate { gameObject.SetActive(false); });
    }
}
