using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ApplicationManagerScript : MonoBehaviour
{
    static ApplicationManagerScript _instance;

    [SerializeField]
    UserTypeEnum _userType;

    [SerializeField]
    bool _firstUseComplete;

    [SerializeField]
    Variables _variables;

    // Start is called before the first frame update
    void Start()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static ApplicationManagerScript GetInstance()
    {
        return _instance;
    }

    public UserTypeEnum GetUserType()
    {
        return _userType;
    }

    public bool GetFirstUseComplete()
    {
        return _firstUseComplete;
    }

    public Variables GetVariables()
    {
        return _variables;
    }

    public VariableDeclaration GetVariableByName(string _input)
    {
        return _variables.declarations.GetDeclaration(_input);
    }

    public VariableDeclaration GetVariableByIndex(int _input)
    {
        return _variables.declarations.ElementAt(_input);
    }

    public void SetUserType(UserTypeEnum _input)
    {
        _userType = _input;

        if(_variables != null)
        {
           _variables.declarations.Set("User Type", _input);
        }
    }

    public void SetUserType(int _input)
    {
        UserTypeEnum _v;

        switch(_input)
        {
            case 1:
                _v = UserTypeEnum.Doctor;
                break;
            case 2:
                _v = UserTypeEnum.Patient;
                break;
            default:
                _v = UserTypeEnum.Random_User;
                break;
        }

        SetUserType(_v);
    }

    public void SetFirstUseComplete(bool _input)
    {
        _firstUseComplete = _input;

        if(_variables != null)
        {
            _variables.declarations.Set("First Use Complete", _input);
        }
    }

    public void SetValueOfVariableByName(string _valueNameInput, object _valueInput)
    {
        if(_variables == null)
        {
            return;
        }

        _variables.declarations.Set(_valueNameInput, _valueInput);
    }

    public void SetValueOfVariableByIndex(int _valueIndexInput, object _valueInput)
    {
        if(_variables == null)
        {
            return;
        }

        _variables.declarations.ElementAt(_valueIndexInput).value = _valueInput;
    }
}
