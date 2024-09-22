using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueCanvasScript : MonoBehaviour
{
    [SerializeField]
    List<Image> _panels;

    [SerializeField]
    int _currentPanelNumber = -1;

    [SerializeField]
    RectTransform _doctorsImage;

    [SerializeField]
    public DoctorTalkingScript _doctorTalkingProperties;

    [SerializeField]
    public Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Image> GetPanels()
    {
        return _panels;
    }

    public int GetCurrentPanelNumber()
    {
        return _currentPanelNumber;
    }

    public void SetCurrentPanel(int _input)
    {
        _currentPanelNumber = _input;

        for(int _i = 0; _i < _panels.Count; _i++)
        {
            if (_panels[_i] != null)
            {
                _panels[_i].gameObject.SetActive(_currentPanelNumber == _i);
            }
        }
    }

    public RectTransform GetDoctorsImage()
    {
        return _doctorsImage;
    }

    public DoctorTalkingScript GetDoctorTalkingProperties()
    {
        return _doctorTalkingProperties;
    }

    public void SetImageAnchoredPosition(Vector2 _input)
    {
        _doctorsImage.anchoredPosition = _input;
    }

    public void SetUniformScale(float _input)
    {
        Vector3 _size = Vector3.one * _input;

        _doctorsImage.localScale = _size;
    }

    public void ResetDoctorsImageToOriginalValues()
    {
        SetImageAnchoredPosition(new Vector2(0.0f, -217.0f));

        SetUniformScale(1.0f);
    }
}
