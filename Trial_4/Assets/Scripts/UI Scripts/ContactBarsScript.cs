using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContactBarsScript : MonoBehaviour
{
    [SerializeField]
    TwoVariablesClass<string, string> _contacts;

    [SerializeField]
    InputField _eMailInputField;

    [SerializeField]
    InputField _phoneNumberInputField;

    // Start is called before the first frame update
    void Start()
    {
        if(_contacts == null)
        {
            _contacts = new TwoVariablesClass<string, string>();
        }    
    }

    private void OnEnable()
    {
        if(_contacts == null)
        {
            _contacts = new TwoVariablesClass<string, string>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public TwoVariablesClass<string, string> GetContacts()
    {
        return _contacts;
    }

    public InputField GetEMailInputField()
    {
        return _eMailInputField;
    }

    public InputField GetPhoneNumberInputField() {  return _phoneNumberInputField; }

    public void ConfirmValues()
    {
        if(_eMailInputField == null || _phoneNumberInputField == null)
        {
            return;
        }

        _contacts.SetVariable1(_eMailInputField.text);

        //int.TryParse(_phoneNumberInputField.text, out int _intV);

        _contacts.SetVariable2(_phoneNumberInputField.text);
    }

    public void SetEmailText(string _input)
    {
        _eMailInputField.text = _input;
    }

    public void SetPhoneNumberText(string _input)
    {
        _phoneNumberInputField.text = _input;
    }

    public void SetPhoneNumberText(int _input)
    {
        _phoneNumberInputField.text = _input.ToString();
    }
}
