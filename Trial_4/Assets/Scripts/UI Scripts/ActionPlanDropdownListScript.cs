using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPlanDropdownListScript : MonoBehaviour
{
    [SerializeField]
    Dropdown _dropdownList;

    [SerializeField]
    ActionPlanUIScript _actionPlanUI;

    bool _listDropped = false;

    bool _styleChanged = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckListDropped();
    }

    void CheckListDropped()
    {

        if (_dropdownList == null || ActionPlanManagerScript.GetInstance() == null)
        {
            _listDropped = false;

            _styleChanged = false;

            return;
        }

        if (ActionPlanManagerScript.GetInstance().GetCurrentQuestion() == null)
        {
            _listDropped = false;

            _styleChanged = false;

            return;
        }

        if(ActionPlanManagerScript.GetInstance().GetCurrentQuestionIndex() == 1)
        {
            Debug.Log("We will start to debug...");
        }

        ActionPlanQuestionScript _currentQuestion= ActionPlanManagerScript.GetInstance().GetCurrentQuestion();

        System.Type _type = _currentQuestion.GetType();



        _listDropped = (_dropdownList.gameObject.GetComponent<RectTransform>().Find("Dropdown List") != null);

        if (_listDropped && _styleChanged)
        {
            return;
        }
        else if(!_listDropped)
        {
            _styleChanged = false;

            return;
        }

        if (_type == typeof(ActionPlanQuestionEnum_InhalerColor))
        {
            var _styles = ActionPlanManagerScript.GetInstance().GetStylesOfInhalerColorLabels().GetStyles();

            ChangeLabelStyles(_styles);
        }

        if(_type == typeof(ActionPlanQuestionEnum_Gender))
        {
            var _styles = ActionPlanManagerScript.GetInstance().GetStylesOfGenderLabels().GetStyles();

            ChangeLabelStyles(_styles);
        }

        _styleChanged = true;
    }

    void ChangeLabelStyles<T>(List<EnumDropdownLabelStyleClass<T>> _input) where T : System.Enum
    {
        RectTransform _rt = _dropdownList.gameObject.GetComponent<RectTransform>().Find("Dropdown List").gameObject.GetComponent<RectTransform>();

        if (_rt == null)
        {
            Debug.LogError("There is no child called 'Dropdown List'.");

            return;
        }

        _rt = _rt.gameObject.GetComponent<RectTransform>().Find("Viewport").gameObject.GetComponent<RectTransform>();

        if (_rt == null)
        {
            Debug.LogError("There is no child called 'Viewport'.");

            return;
        }

        _rt = _rt.gameObject.GetComponent<RectTransform>().Find("Content").gameObject.GetComponent<RectTransform>();

        if (_rt == null)
        {
            Debug.LogError("There is no child called 'Content'.");

            return;
        }

        RectTransform _rt2, _rt3;

        Image _img;

        //int _size;

        Text _text;

        EnumDropdownLabelStyleClass<T> _selectedStyle = null;

        EnumDropdownLabelStyleClass<T> _currentStyleInLoop;

        Outline _outline;

        bool _stringComp;

        for (int _i = 0; _i < _input.Count; _i++)
        {
            _rt2 = _rt.GetChild(_i + 1).gameObject.GetComponent<RectTransform>();

            if(_rt2 == null)
            {
                Debug.LogError("Child No. " + (_i + 1) + " does not exist.");

                continue;
            }

            _rt3 = _rt2.gameObject.GetComponent<RectTransform>().Find("Item Background").gameObject.GetComponent<RectTransform>();

            _img = _rt3.gameObject.GetComponent<Image>();

            _rt3 = _rt2.gameObject.GetComponent<RectTransform>().Find("Item Label").gameObject.GetComponent<RectTransform>();

            _text = _rt3.gameObject.GetComponent<Text>();

            if(_img == null || _text == null)
            {
                Debug.LogError("Something is missing in label No. " + (_i + 1) + ".");

                continue;
            }

            _outline = _text.gameObject.GetComponent<Outline>();

            for(int _j = 0; _j < _input.Count && _selectedStyle == null; _j++)
            {
                _currentStyleInLoop = _input[_j];

                _stringComp = string.Compare(_currentStyleInLoop.GetAlternativeText(), _text.text, true) == 0;

                if(_stringComp)
                {
                    _selectedStyle = _currentStyleInLoop;
                }
            }

            if(_selectedStyle == null)
            {
                continue;
            }

            _img.color = _selectedStyle.GetLabelColor();

            _text.color = _selectedStyle.GetTextColor();

            _text.fontSize = _selectedStyle.GetTextSize();

            if(_outline != null)
            {
                _outline.effectColor = _selectedStyle.GetOutlineColor();

                _outline.effectDistance = _selectedStyle.GetOutlineEffect();
            }

            _selectedStyle = null;
        }
    }
}
