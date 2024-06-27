using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FirstUseSceneScript : MonoBehaviour
{
    [SerializeField]
    ToggleGroup _group;

    [SerializeField]
    Button _backButton;

    [SerializeField]
    Button _selectButton;

    [SerializeField]
    Variables _variables;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SeeChoiceSelected();
    }

    void SeeChoiceSelected()
    {
        if(_group == null || _selectButton == null)
        {
            return;
        }

        bool _on = _group.AnyTogglesOn();

        _selectButton.enabled = _on;
    }

    public void SetValue()
    {
        if(_group == null || _variables == null)
        {
            return;
        }

        string _valueName = "User Type Value";

        VariableDeclaration _vd1 = _variables.declarations.GetDeclaration(_valueName);

        int _val = GetActiveToggle(); 

        if(_vd1 == null)
        {
            Debug.LogError("No value of the name \\(" + _valueName + ")\\ exists.");
            return;
        }

        if(_vd1.value.GetType() != typeof(int))
        {
            Debug.LogError("No integer of the name \\(" + _valueName + ")\\ exists.");
            return;
        }

        //_vd1.value = _val;

        _vd1.value = _val;
    }

    public void ConfirmUserStatus()
    {
        if (DataPersistenceManager.GetInstance() == null || _variables == null)
        {
            return;
        }

        string _valueName = "User Type Value";

        //int _val = GetActiveToggle();

        VariableDeclaration _vd = _variables.declarations.GetDeclaration(_valueName);

        if (_vd == null)
        {
            Debug.LogError("No value of the name \\(" + _valueName + ")\\ exists.");
            return;
        }

        if (_vd.value.GetType() != typeof(int))
        {
            Debug.LogError("No integer of the name \\" + _valueName + "\\ exists.");
            return;
        }

        int _integerValue = (int)_vd.value;

        UserTypeEnum _ut;

        switch (_integerValue)
        {
            case 0:
                _ut = UserTypeEnum.Doctor;
                break;
            case 1:
                _ut = UserTypeEnum.Patient;
                break;
            default:
                _ut = UserTypeEnum.Random_User;
                break;
        }

        DataPersistenceManager.GetInstance().GetGameData().ConfirmUserTypeStatus(_ut);
    }

    int GetActiveToggle()
    {
        RectTransform _rectT = _group.GetComponent<RectTransform>();

        int _count = _rectT.childCount;

        Toggle _tog;

        for(int _i = 0; _i < _count; _i++)
        {
            _tog = _rectT.GetChild(_i).GetComponent<Toggle>();

            if(_tog == null)
            {
                continue;
            }

            if(_tog.isOn)
            {
                return (_i);
            }
        }

        return -1;
    }
}
