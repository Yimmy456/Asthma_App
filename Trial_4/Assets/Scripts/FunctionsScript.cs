using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FunctionsScript : MonoBehaviour
{
    [SerializeField]
    ScriptGraphAsset _graph;

    [SerializeField]
    Variables _variables;

    [SerializeField]
    Text _text;

    [SerializeField]
    FastyScript _fasty;

    [SerializeField]
    Gradient _textGradient;

    //[SerializeField]
    //Variables _vars;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene1(int _input)
    {
        SceneManager.LoadScene(_input);
    }

    public void LoadScene2(string _input)
    {
        SceneManager.LoadScene(_input);
    }

    public void RemoveParent(Transform _input)
    {
        _input.transform.parent = null;
    }

    public void RemoveParent2(string _input)
    {
        GameObject _go = GameObject.Find(_input);

        if(_go != null)
        {
            _go.transform.parent = null;
        }
    }

    public void SetObjectActive(string _nameInput)
    {
        GameObject _go = GameObject.Find(_nameInput);

        if(_go != null)
        {
            _go.SetActive(true);
        }
    }

    public void SetObjectInactive(string _nameInput)
    {
        GameObject _go = GameObject.Find(_nameInput);

        if(_go != null)
        {
            _go.SetActive(false);
        }
    }

    public void SetObjectsActive(string[] _namesInput)
    {
        foreach(string _name in _namesInput)
        {
            GameObject _go = GameObject.Find(_name);

            if(_go != null)
            {
                _go.SetActive(true);
            }
        }
    }

    public void SetCanvasGroupStatus(string _canvasGroupNameInput, float _alphaInput, bool _interactableInput)
    {
        GameObject _go = GameObject.Find(_canvasGroupNameInput);

        if(_go == null)
        {
            return;
        }

        CanvasGroup _cg = _go.GetComponent<CanvasGroup>();

        if(_cg == null)
        {
            return;
        }

        _cg.alpha = _alphaInput;

        _cg.interactable = _interactableInput;
    }

    public void SetCanvasGroupInteractive(string _nameInput)
    {
        GameObject _go = GameObject.Find(_nameInput);

        if(_go == null)
        {
            return;
        }

        CanvasGroup _cg = _go.GetComponent<CanvasGroup>();

        if(_cg == null)
        {
            return;
        }

        _cg.alpha = 1.0f;

        _cg.interactable = true;
    }


    public void DebugDisplay(string _input)
    {
        Debug.Log(_input);
    }

    public void SetIndicatorEnable(Toggle _input)
    {
        if(SettingsManager.GetInstance() == null || _input == null)
        {
            return;
        }

        //SettingsManager.GetInstance().SetIndicatorEnabled(_input.isOn);
    }

    public void SetObjectActiveToOpposite(GameObject _input)
    {
        bool _isActive = _input.activeSelf;
        _input.SetActive(!_isActive);
    }

    public void SetIndicatorBound(Slider _input)
    {
        if(SettingsManager.GetInstance() == null || _input == null)
        {
            return;
        }

        //SettingsManager.GetInstance().SetIndicatorBound(_input.value);
    }

    public void SetUserTypeFunction(ToggleGroup _input)
    {
        if(ApplicationManagerScript.GetInstance() == null)
        {
            return;
        }

        Toggle _t = ToolsStruct.GetSelectedToggle(_input);

        if(_t == null)
        {
            return;
        }

        int _v = (int)_t.gameObject.GetComponent<Variables>().declarations.Get("Value");

        Debug.Log("Value is " + _v.ToString() + ".");

        ApplicationManagerScript.GetInstance().SetUserType(_v);
    }

    public void SetFirstUseFunctionComplete(Variables _input)
    {
        if(DataPersistenceManager.GetInstance() == null || _input == null)
        {
            return;
        }

        //_input.declarations.Get("Value")

        int _val = (int)_input.declarations.ElementAt(0).value;

        UserTypeEnum _e = (UserTypeEnum)(_val);

        DataPersistenceManager.GetInstance().GetGameData().ConfirmUserTypeStatus(_e);
    }

    /*public void SetFirstUseComplete(bool _input = true)
    {
        if(ApplicationManagerScript.GetInstance() == null)
        {
            return;
        }

        ApplicationManagerScript.GetInstance().SetFirstUseComplete(_input);
    }*/

    public void CountText(float _countInput = 10.0f)
    {
        if(_text == null)
        {
            return;
        }

        StartCoroutine(CountCoroutine(_countInput));
    }

    public void CountText2(float _countInput)
    {
        if (_text == null)
        {
            return;
        }

        StartCoroutine(CountCoroutine2(_countInput));
    }

    IEnumerator CountCoroutine(float _input)
    {
        int _t = 0;

        for(float _f = 0.0f; _f < 10.0f; _f += Time.deltaTime)
        {
            _t = (int)_f + 1;

            _text.text = _t.ToString();

            yield return null;
        }

        _text.text = "";
    }

    IEnumerator CountCoroutine2(float _countInput)
    {
        int _t = 0;

        float _key;

        Color _textColor, _outlineColor;

        for (float _f = 0.0f; _f < 10.0f; _f += Time.deltaTime)
        {
            _t = (int)_f + 1;

            _text.text = _t.ToString();

            _key = (_t - 1) / (_countInput - 1);

            _textColor = _textGradient.Evaluate(_key);

            _text.color = _textColor;

            if(_text.gameObject.GetComponent<Outline>() != null)
            {
                Outline _outline = _text.gameObject.GetComponent<Outline>();

                _outlineColor = ToolsStruct.ChangeColorValue(_textColor, 0.5f, 0.5f, false);

                _outline.effectColor = _outlineColor;
            }

            yield return null;
        }

        _text.text = "";
    }

    public void StartCountdownFromFasty()
    {
        if(_fasty == null)
        {
            return;
        }

        _fasty.CountToTenFunction();
    }
}
