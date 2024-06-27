using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BadgeDisplayScript : MonoBehaviour
{
    [SerializeField]
    RectTransform _displayTransform;

    [SerializeField]
    Image _badgeImage;

    [SerializeField]
    Text _badgeText;

    [SerializeField]
    Text _indexText;

    [SerializeField]
    Button _buttonComponent;

    [SerializeField]
    BadgesUIScript _badgesUI;

    [SerializeField]
    BadgeScript _badge;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public RectTransform GetDisplayTransform()
    {
        return _displayTransform;
    }

    public Image GetBadgeImage()
    {
        return _badgeImage;
    }

    public Text GetBadgeText()
    {
        return _badgeText;
    }

    public Text GetIndexText()
    {
        return _indexText;
    }

    public Button GetButtonComponent()
    {
        return _buttonComponent;
    }

    public BadgesUIScript GetBadgesUI()
    {
        return _badgesUI;
    }

    public BadgeScript GetBadge()
    {
        return _badge;
    }

    public void SetDisplayBasedOnBadge(BadgeScript _badgeInput, int _indexInput)
    {
        _badge = _badgeInput;

        if (_indexText != null)
        {
            _indexText.text = _indexInput.ToString() + ".";
        }

        if (_badgeInput.GetBadgeCollected())
        {
            if (_badgeImage != null)
            {
                _badgeImage.sprite = _badgeInput.GetBadgeSprite();
            }

            if (_badgeText != null)
            {
                _badgeText.text = _badgeInput.GetBadgeName();
            }
        }
        else
        {
            if(_badgeImage != null)
            {
                _badgeImage.sprite = BadgesManagerScript.GetInstance().GetUnknownBadgeSprite();
            }

            if(_badgeText != null)
            {
                _badgeText.text = "???";
            }
        }
    }

    public void SetBadgeImageSprite(Sprite _input)
    {
        _badgeImage.sprite = _input;
    }

    public void SetBadgeNameText(string _input)
    {
        _badgeText.text = _input;
    }

    public void SetIndexTextNumber(int _input)
    {
        _indexText.text = _input.ToString() + ".";
    }

    public void SetBadgesUI(BadgesUIScript _input)
    {
        _badgesUI = _input;
    }

    public void RemoveListener()
    {
        if(_buttonComponent != null)
        {
            _buttonComponent.onClick.RemoveAllListeners();
        }
    }
}
