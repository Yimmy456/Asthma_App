using System;
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

    [SerializeField]
    Dropdown _dayDropdown;

    [SerializeField]
    Dropdown _monthDropdown;

    DateTime _date;

    // Start is called before the first frame update
    void Start()
    {
        //_date
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if(_date == null)
        {
            //_date = new DateClass();
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

    /*public DateClass GetDate()
    {
        return _date;
    }*/

    public Dropdown GetDayDropdown()
    {
        return _dayDropdown;
    }

    public Dropdown GetMonthDropdown()
    {
        return _monthDropdown;
    }

    public void ConfirmDate()
    {
        /*if(_dayPanel == null || _monthPanel == null || _yearPanel == null)
        {
            return;
        }

        if(_dayPanel.text == "" || _monthPanel.text == "" || _yearPanel.text == "")
        {
            return;
        }*/

        if(_dayDropdown == null || _monthDropdown == null || _yearPanel == null)
        {
            return;
        }

        if(_yearPanel.text == "")
        {
            return;
        }

        //int.TryParse(_dayPanel.text, out int _day);

        //int.TryParse(_monthPanel.text, out int _month);

        int.TryParse(_yearPanel.text, out int _year);

        int _day = _dayDropdown.value + 1;

        int _month = _monthDropdown.value + 1;

        //_date.SetYear(_year);

        //_date.SetMonth(_month);

        //_date.SetDay(_day);
    }

    public void SetDayText(int _input)
    {

        if (_dayPanel != null)
        {
            _dayPanel.text = _input.ToString();
        }
        if(_dayDropdown != null)
        {
            _dayDropdown.value = _input - 1;
        }

        if (_date == null)
        {
            Debug.LogError("The date is null (D).");
            //return;
        }

        //_date.SetDay(_input);
    }

    public void SetMonthText(int _input)
    {
        if (_monthPanel != null)
        {
            _monthPanel.text = _input.ToString();
        }
        if (_monthDropdown != null)
        {
            _monthDropdown.value = _input - 1;
        }

        if (_date == null)
        {
            Debug.LogError("The date is null (M).");
            //return;
        }

        //_date.SetMonth(_input);
    }

    public void SetYearText(int _input)
    {
        if (_yearPanel != null)
        {
            _yearPanel.text = _input.ToString();
        }

        if (_date == null)
        {
            Debug.LogError("The date is null (Y).");
            //return;
        }

        //_date.SetYear(_input);
    }

    public void ResetDays()
    {
        if(_dayDropdown == null || _monthDropdown == null || _yearPanel == null)
        {
            return;
        }

        if(_yearPanel.text == "")
        {
            _dayDropdown.interactable = false;

            _monthDropdown.interactable = false;

            return;
        }

        int _year = int.Parse(_yearPanel.text);

        if(_year == -1)
        {
            _dayDropdown.interactable = false;

            _monthDropdown.interactable = false;

            return;
        }
        else
        {

            _dayDropdown.interactable = true;

            _monthDropdown.interactable = true;
        }

        int _selectedDay = _dayDropdown.value;

        int _days = 31;

        int _m = _monthDropdown.value;

        if(_m == 1)
        {
            if((_year % 4) == 0)
            {
                _days = 29;
            }
            else
            {
                _days = 28;
            }
        }
        else if(_m == 3 || _m == 5 || _m == 8 || _m == 10)
        {
            _days = 30;
        }

        _dayDropdown.ClearOptions();

        List<string> _list = new List<string>();

        for(int _i = 0; _i < _days; _i++)
        {
            _list.Add((_i + 1).ToString("00"));
        }

        _dayDropdown.AddOptions(_list);

        if(_selectedDay >= _dayDropdown.options.Count)
        {
            _selectedDay = _dayDropdown.options.Count - 1;
        }

        _dayDropdown.value = _selectedDay;
    }
}
