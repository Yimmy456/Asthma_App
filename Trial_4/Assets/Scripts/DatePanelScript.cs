using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DatePanelScript : MonoBehaviour
{
    [SerializeField]
    InputField _dayPanel;

    [SerializeField]
    InputField _monthPanel;

    [SerializeField]
    InputField _yearPanel;

    DateClass _date;

    // Start is called before the first frame update
    void Start()
    {
        if (_date == null)
        {
            _date = new DateClass(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if(_date == null)
        {
            _date = new DateClass(false);
        }
    }

    public InputField GetDayPanel()
    {
        return _dayPanel;
    }

    public InputField GetMonthPanel()
    {
        return _monthPanel;
    }

    public InputField GetYearPanel()
    {
        return _yearPanel;
    }

    public DateClass GetDate()
    {
        return _date;
    }

    public void ConfirmDate()
    {
        if(_dayPanel == null || _monthPanel == null || _yearPanel == null)
        {
            return;
        }

        if(_dayPanel.text == "" || _monthPanel.text == "" || _yearPanel.text == "")
        {
            return;
        }

        int.TryParse(_dayPanel.text, out int _day);

        int.TryParse(_monthPanel.text, out int _month);

        int.TryParse(_yearPanel.text, out int _year);

        _date.SetYear(_year);

        _date.SetMonth(_month);

        _date.SetDay(_day);
    }

    public void SetDayText(int _input)
    {
        _dayPanel.text = _input.ToString();

        if(_date == null)
        {
            Debug.LogError("The date is null (D).");
            //return;
        }

        _date.SetDay(_input);
    }

    public void SetMonthText(int _input)
    {
        _monthPanel.text = _input.ToString();

        if (_date == null)
        {
            Debug.LogError("The date is null (M).");
            //return;
        }

        _date.SetMonth(_input);
    }

    public void SetYearText(int _input)
    {
        _yearPanel.text = _input.ToString();

        if (_date == null)
        {
            Debug.LogError("The date is null (Y).");
            //return;
        }

        _date.SetYear(_input);
    }
}
