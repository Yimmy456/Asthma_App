using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExhibitionCanvasScript : MonoBehaviour
{
    [SerializeField]
    Text _objectNameText;

    [SerializeField]
    Camera _camera;

    [SerializeField]
    Button _talkButton;

    [SerializeField]
    ExhibitionScript _exhibition;

    [SerializeField]
    Button _backButton;

    [SerializeField]
    Vector2 _textExtraDistance;

    [SerializeField]
    MainCanvasesClass _mainCanvases;

    [SerializeField]
    Image _cursorImage;

    [SerializeField]
    Text _objectTitleText;

    [SerializeField]
    Text _objectDescriptionText;

    [SerializeField]
    GameObject _rocketContainerGO;

    //[SerializeField]
    //DoctorSalemDialoguesClass _doctorSalemDialogues;

    [SerializeField]
    DialoguesScript _doctorSalemDialogues;

    [SerializeField]
    Text _countText;

    [SerializeField]
    Text _percentageText;

    [SerializeField]
    GameObject _panelTemplate;

    [SerializeField]
    List<GameObject> _panels;

    [SerializeField]
    RectTransform _panelParent;

    bool _timerStarts;

    float _timeElapsed = 0.0f;

    ExhibitionObjectScript _currentlyTalkedAboutObject;

    ExhibitionObjectScript _currentObject;

    Coroutine _talkingCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        DisplayName();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateExhibitionStatus();

        if (_timerStarts)
        {
            CheckTimeElaped();
        }

        if(_doctorSalemDialogues != null && _currentlyTalkedAboutObject != null)
        {
            if(_doctorSalemDialogues.GetOverallTalkingStatus() == TalkingStatusEnum.Completed)
            {
                ExhibitionListItemClass _item = _exhibition.GetCurrentGroup().GetExhibitionGroupListItems()[_currentlyTalkedAboutObject.GetObjectNumber() - 1];

                if(!_item.GetDisplayExplained())
                {
                    _exhibition.GetCurrentGroup().GetGroupCompletionMeter().AddToValue(1);

                    _item.SetDisplayExplained(true);

                    Debug.Log("Explanation is complete for the first time.");
                }

                Debug.Log("Explanation is complete.");
            }
        }
    }

    public void QuitExhibition()
    {
        _mainCanvases.SetCanvasesOn(true);

        if(_mainCanvases.GetDoctorCanvas() != null)
        {
            _mainCanvases.GetDoctorCanvas().gameObject.SetActive(true);
        }

        if(_rocketContainerGO != null)
        {
            _rocketContainerGO.SetActive(true);
        }

        StopExplaining();
    }

    public ExhibitionScript GetExhibition()
    {
        return _exhibition;
    }

    public ExhibitionObjectScript GetCurrentObject()
    {
        return _currentObject;
    }

    public ExhibitionObjectScript GetCurrentlyTalkedAboutObject()
    {
        return _currentlyTalkedAboutObject;
    }

    public void SetCurrentObject(ExhibitionObjectScript _input)
    {
        _currentObject = _input;
    }

    void DisplayName()
    {
        if (_currentObject == null || _camera == null)
        {
            _objectNameText.text = "";

            _objectNameText.gameObject.SetActive(false);

            return;
        }

        _objectNameText.text = _currentObject.GetObjectName();

        _objectNameText.gameObject.SetActive(true);

        _objectNameText.color = _currentObject.GetObjectColor();

        Outline _outline = _objectNameText.gameObject.GetComponent<Outline>();

        if (_outline != null)
        {
            Color _c = ToolsStruct.ChangeColorValue(_currentObject.GetObjectColor(), 0.5f, 0.5f, true);

            _outline.effectColor = _c;
        }

        Vector2 _textPos = _camera.WorldToScreenPoint(_currentObject.GetObjectPosition());

        _objectNameText.GetComponent<RectTransform>().localPosition = _textPos;
    }

    void UpdateExhibitionStatus()
    {
        if (_cursorImage == null || _objectNameText == null)
        {
            return;
        }

        if(_cursorImage.sprite == null)
        {
            return;
        }

        if(_currentObject == null)
        {
            _cursorImage.color = Color.red;

            _objectNameText.text = "";

            _objectNameText.gameObject.SetActive(false);

            _talkButton.gameObject.SetActive(false);

            return;
        }

        _cursorImage.color = Color.green;

        _objectNameText.text = _currentObject.GetObjectName();

        _objectNameText.color = _currentObject.GetObjectColor();

        _objectNameText.gameObject.GetComponent<Outline>().effectColor = GetOutlineColor(_currentObject.GetObjectColor());

        _objectNameText.gameObject.SetActive(true);

        _talkButton.gameObject.SetActive(true);
    }

    Color GetOutlineColor(Color _input)
    {
        Color _c = _input;

        float _lightLevel = _c.r + _c.g + _c.b;

        _lightLevel = _lightLevel / 3.0f;

        if(_c == Color.black)
        {
            _c = Color.white;
        }
        else if(_lightLevel < 0.15)
        {
            _c.r = _c.r * 2.0f;

            _c.g = _c.g * 2.0f;

            _c.b = _c.b * 2.0f;
        }
        else
        {
            _c.r = _c.r / 2.0f;

            _c.g = _c.g / 2.0f;

            _c.b = _c.b / 2.0f;
        }

        _c.a = 0.5f;

        return _c;
    }

    public void StopExplaining()
    {
        if(_talkingCoroutine == null)
        {
            return;
        }
        
        StopCoroutine(_talkingCoroutine);
        
        _objectTitleText.text = "";
        
        _objectDescriptionText.text = "";

        _currentlyTalkedAboutObject = null;
    }

    public void StartExplaining()
    {
        if(_currentObject == null)
        {
            return;
        }

        StopExplaining();

        float _time = _currentObject.GetTalkingSeconds();

        int _index = _currentObject.GetObjectNumber();

        string _name = _currentObject.GetObjectName();

        string _description = _currentObject.GetObjectDescription();

        string _fullTitle = _index.ToString() + ". " + _name;

        /*if (_audioSource != null && _currentObject.GetObjectAudioClip() != null)
        {
            _talkingCoroutine = StartCoroutine(TalkingCoroutine2(_currentObject.GetObjectAudioClip(), _fullTitle, _description));
        }
        else
        {
            _talkingCoroutine = StartCoroutine(TalkingCoroutine(_time, _fullTitle, _description));
        }*/

        Color _color = _currentObject.GetObjectColor();

        ColorTitle(_color);

        if(_exhibition.GetDialogues() != null)
        {
            _exhibition.GetDialogues().PlayExhibitClip(_currentObject);
        }

        _currentlyTalkedAboutObject = _currentObject;
    }


    /*
    IEnumerator TalkingCoroutine(float _timeInput, string _titleInput, string _descriptionInput)
    {
        _objectTitleText.text = _titleInput;

        _objectDescriptionText.text = _descriptionInput;

        _currentlyTalkedAboutObject = _currentObject;

        _timerStarts = true;

        if(_doctorAnimator != null)
        {
            _doctorAnimator.SetBool("Talking", true);
        }

        yield return new WaitForSeconds(_timeInput);

        if (_doctorAnimator != null)
        {
            _doctorAnimator.SetBool("Talking", false);
        }

        _timerStarts = false;

        _timeElapsed = 0.0f;

        _objectDescriptionText.text = "";

        _objectTitleText.text = "";

        _currentlyTalkedAboutObject = null;
    }


    */
    /*
    IEnumerator TalkingCoroutine2(AudioClip _clipInput, string _titleInput, string _descriptionInput)
    {
        if(_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }

        float _seconds = _clipInput.length;

        _audioSource.clip = _clipInput;

        _objectTitleText.text = _titleInput;

        _objectDescriptionText.text = _descriptionInput;

        _currentlyTalkedAboutObject = _currentObject;

        _timerStarts = true;

        if (_doctorAnimator != null)
        {
            _doctorAnimator.SetBool("Talking", true);
        }

        _audioSource.Play();

        yield return new WaitForSeconds(_seconds);

        if (_doctorAnimator != null)
        {
            _doctorAnimator.SetBool("Talking", false);
        }

        _timerStarts = false;

        _timeElapsed = 0.0f;

        _objectDescriptionText.text = "";

        _objectTitleText.text = "";

        _currentlyTalkedAboutObject = null;

        _audioSource.clip = null;
    }
    */

    void ColorTitle(Color _input)
    {
        _objectTitleText.color = _input;

        Color _outlineColor = ToolsStruct.ChangeColorValue(_input, 0.5f, 0.5f);

        Outline _outline = _objectTitleText.gameObject.GetComponent<Outline>();

        _outline.effectColor = _outlineColor;
    }


    void CheckTimeElaped()
    {
        _timeElapsed = _timeElapsed + Time.deltaTime;

        Debug.Log("The time elapsed for the coroutine is " + @"""" + _timeElapsed.ToString() + @"""" + ".");
    }

    void CheckIfItemIsComplete()
    {
        if(_currentlyTalkedAboutObject == null)
        {
            return;
        }

        //_currentlyTalkedAboutObject.
    }

    public void PreparePanels()
    {
        if (_exhibition == null || _panelTemplate == null || _panelParent == null)
        {
            return;
        }

        if(_exhibition.GetCurrentGroup() == null)
        {
            return;
        }

        int _total = _exhibition.GetCurrentGroup().GetExhibitionGroupListItems().Count;

        int _completed = 0;

        GameObject _currentPanel;

        ExhibitionListItemClass _exhibitItem;

        Text _tx;

        for(int _i = 0; _i < _total; _i++)
        {
            _exhibitItem = _exhibition.GetCurrentGroup().GetExhibitionGroupListItems()[_i];

            _currentPanel = Instantiate(_panelTemplate);

            _currentPanel.SetActive(true);

            _currentPanel.GetComponent<RectTransform>().parent = _panelParent;

            _currentPanel.name = "Panel No. " + (_i + 1).ToString();

            _panels.Add(_currentPanel);

            if(_currentPanel.GetComponent<Image>() != null)
            {
                _currentPanel.GetComponent<Image>().enabled = true;

                Color _c = _exhibitItem.GetListItemColor();

                _c.a = 0.9f;

                _currentPanel.GetComponent<Image>().color = _c;
            }


            //Exhibit Number
            if(_currentPanel.GetComponent<RectTransform>().Find("Exhibit Number Panel") != null)
            {
                if(_currentPanel.GetComponent<RectTransform>().Find("Exhibit Number Panel").gameObject.GetComponent<RectTransform>().Find("Text") != null)
                {
                    if(_currentPanel.GetComponent<RectTransform>().Find("Exhibit Number Panel").gameObject.GetComponent<RectTransform>().Find("Text").GetComponent<Text>() != null)
                    {
                        _tx = _currentPanel.GetComponent<RectTransform>().Find("Exhibit Number Panel").gameObject.GetComponent<RectTransform>().Find("Text").GetComponent<Text>();

                        _tx.text = (_i + 1).ToString() + ".";
                    }
                }
            }


            //Exhibit Name
            if (_currentPanel.GetComponent<RectTransform>().Find("Exhibit Name Panel") != null)
            {
                if (_currentPanel.GetComponent<RectTransform>().Find("Exhibit Name Panel").gameObject.GetComponent<RectTransform>().Find("Text") != null)
                {
                    if (_currentPanel.GetComponent<RectTransform>().Find("Exhibit Name Panel").gameObject.GetComponent<RectTransform>().Find("Text").GetComponent<Text>() != null)
                    {
                        _tx = _currentPanel.GetComponent<RectTransform>().Find("Exhibit Name Panel").gameObject.GetComponent<RectTransform>().Find("Text").GetComponent<Text>();

                        _tx.text = _exhibitItem.GetListItemName();
                    }
                }
            }

            //Exhibit Animation

            if(_currentPanel.GetComponent<RectTransform>().Find("Exhibit Animation Panel") != null)
            {
                Destroy(_currentPanel.GetComponent<RectTransform>().Find("Exhibit Animation Panel").gameObject.GetComponent<RectTransform>().Find("Animation Image Samples").gameObject);

                if (_currentPanel.GetComponent<RectTransform>().Find("Exhibit Animation Panel").gameObject.GetComponent<RectTransform>().Find("Animation Image") != null)
                {
                    Image _img = _currentPanel.GetComponent<RectTransform>().Find("Exhibit Animation Panel").gameObject.GetComponent<RectTransform>().Find("Animation Image").GetComponent<Image>();

                    if (_img != null && _exhibitItem.GetUIAnimatorController() != null) { 

                        _img.gameObject.GetComponent<Animator>().runtimeAnimatorController = _exhibitItem.GetUIAnimatorController();

                        if (_img != null)
                        {
                            if (_currentPanel.GetComponent<RectTransform>().Find("Exhibit Animation Panel").gameObject.GetComponent<RectTransform>().Find("Text") != null)
                            {
                                _currentPanel.GetComponent<RectTransform>().Find("Exhibit Animation Panel").gameObject.GetComponent<RectTransform>().Find("Text").gameObject.SetActive(false);
                            }
                        }
                    }
                }
            }
            //Exhibit Talked About

            if (_currentPanel.GetComponent<RectTransform>().Find("Exhibit Talked About Panel") != null)
            {
                if (_currentPanel.GetComponent<RectTransform>().Find("Exhibit Talked About Panel").gameObject.GetComponent<RectTransform>().Find("Text") != null)
                {
                    if (_currentPanel.GetComponent<RectTransform>().Find("Exhibit Talked About Panel").gameObject.GetComponent<RectTransform>().Find("Text").GetComponent<Text>() != null)
                    {
                        _tx = _currentPanel.GetComponent<RectTransform>().Find("Exhibit Talked About Panel").gameObject.GetComponent<RectTransform>().Find("Text").GetComponent<Text>();

                        Outline _outl = _tx.gameObject.GetComponent<Outline>();

                        bool _talked = _exhibitItem.GetDisplayExplained();

                        if (_talked)
                        {
                            _tx.text = "Yes";

                            _tx.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);

                            _outl.effectColor = new Color(0.0f, 0.5f, 0.0f, 0.5f);

                            _completed++;
                        }
                        else
                        {
                            _tx.text = "No";

                            _tx.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

                            _outl.effectColor = new Color(0.5f, 0.0f, 0.0f, 0.5f);
                        }
                    }
                }
            }
        }

        _countText.text = _completed.ToString() + " / " + _total.ToString();

        if(_total > 0 && _exhibition != null)
        {
            float _ratio = (float)_completed / (float)_total;

            if (_exhibition.GetObjectComplete() != null)
            {
                if (_exhibition.GetObjectComplete().GetTextGradient() != null)
                {
                    Color _c1 = _exhibition.GetObjectComplete().GetTextGradient().Evaluate(_ratio);

                    _percentageText.color = _c1;

                    Color _c2 = ToolsStruct.ChangeColorValue(_c1, 0.5f, 0.5f, true);

                    Outline _outline = _percentageText.gameObject.GetComponent<Outline>();

                    if (_outline != null)
                    {
                        _outline.effectColor = _c2;
                    }
                }
            }

            _ratio = _ratio * 100.0f;

            _percentageText.text = _ratio.ToString("0.00") + " %";
        }
    }

    public void EndPanels()
    {
        foreach(GameObject _obj in _panels)
        {
            Destroy(_obj);
        }

        _panels.Clear();
    }
}
