using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System;
using System.ComponentModel;
using Org.BouncyCastle.Asn1.X509.Qualified;

[Serializable]
public abstract class ActionPlanQuestionScript: IDataPersistenceScript
{
    [Description("This is where you put the question that you want to ask.")]
    [SerializeField]
    protected string _questionText;

    [SerializeField]
    protected string _questionID;

    [SerializeField]
    protected bool _questionAnswered;

    //[SerializeField]
    protected VariableTypeForAPEnum _variableType;

    public string GetQuestionText()
    {
        return _questionText;
    }

    public string GetQuestionID()
    {
        return _questionID;
    }

    public VariableTypeForAPEnum GetVariableType()
    {
        return _variableType;
    }

    public virtual int GetIntegerAnswer()
    {
        return 0;
    }

    public virtual float GetDecimalAnswer()
    {
        return 0.0f;
    }

    public virtual string GetTextAnswer()
    {
        return "";
    }

    public virtual Enum GetEnumAnswer()
    {
        return UserTypeEnum.Random_User;
    }

    public virtual DateClass GetDateAnswer()
    {
        return new DateClass();
    }

    public virtual string GetWholeAnswerAsString()
    {
        return "";
    }

    public bool GetQuestionAnswered()
    {
        return _questionAnswered;
    }

    public virtual void SetIntegerAnswer(int _input) { }

    public virtual void SetDecimalAnswer(float _input) { }

    public virtual void SetTextAnswer(string _input) { }

    public virtual void SetDateAnswer(int _dayInput, int _monthInput, int _yearInput) { }

    public virtual void SetAnswerFromString(string _input) { }

    public void SetQuestionID(string _input)
    {
        _questionID = _input;
    }

    public void SetQuestionAnswered(bool _input)
    {
        _questionAnswered = _input;
    }

    public void LoadData(GameDataScript _input)
    {
        _input._actionPlanAnswers.TryGetValue(_questionID, out var _output);

        SetAnswerBasedOnType(_output);

        _input.AddQuestionAnswer(this);
    }

    protected virtual void IsQuestionAnswered()
    {

    }

    public void SaveData(ref GameDataScript _input)
    {
        if(_input._actionPlanAnswers.ContainsKey(_questionID))
        {
            _input._actionPlanAnswers.Remove(_questionID);
        }

        string _v = "";

        if(_variableType == VariableTypeForAPEnum.Enum)
        {
            _v = GetIntegerAnswer().ToString();
        }
        else
        {
            _v = GetWholeAnswerAsString();
        }

        _input._actionPlanAnswers.Add(_questionID, _v);
    }

    protected void SetAnswerBasedOnType(string _input)
    {
        if(_variableType == VariableTypeForAPEnum.Integer || _variableType == VariableTypeForAPEnum.Enum)
        {
            int _vi = int.Parse(_input);

            SetIntegerAnswer(_vi);
        }

        if(_variableType == VariableTypeForAPEnum.Decimal)
        {
            float _vf = float.Parse(_input);

            SetDecimalAnswer(_vf);
        }

        if(_variableType == VariableTypeForAPEnum.Text)
        {
            SetTextAnswer(_input);
        }

        if(_variableType == VariableTypeForAPEnum.Date)
        {
            SetAnswerFromString(_input);
        }

        if(_variableType == VariableTypeForAPEnum.Contact)
        {
            SetAnswerFromString(_input);
        }
    }

    public IEnumerator CheckPersistence()
    {
        yield return new WaitForSeconds(1.0f);

        if(DataPersistenceManager.GetInstance() == null)
        {
            yield break;
        }

        if(!DataPersistenceManager.GetInstance().GetGameData()._actionPlanAnswers.ContainsKey(_questionID))
        {
            DataPersistenceManager.GetInstance().GetGameData().AddQuestionAnswer(this);
        }
        else
        {
            string _value = DataPersistenceManager.GetInstance().GetGameData()._actionPlanAnswers[_questionID];

            SetAnswerBasedOnType(_value);

            IsQuestionAnswered();
        }
    }
}

public abstract class ActionPlanQuestionScript<T>:ActionPlanQuestionScript
{
    [SerializeField]
    protected T _answer;

    public T GetAnswer()
    {
        return _answer;
    }

    public virtual void SetValue(T _input)
    {
        _answer = _input;
    }
}

public class ActionPlanQuestionInteger : ActionPlanQuestionScript<int>
{
    public ActionPlanQuestionInteger()
    {
        _variableType = VariableTypeForAPEnum.Integer;

        _questionAnswered = true;
    }

    public override int GetIntegerAnswer()
    {
        return _answer;
    }

    public override void SetIntegerAnswer(int _input)
    {
        _answer = _input;
    }

    public override string GetTextAnswer()
    {
        return _answer.ToString();
    }

    public override string GetWholeAnswerAsString()
    {
        return _answer.ToString();
    }

    protected override void IsQuestionAnswered()
    {
        _questionAnswered = true;
    }
}

public class ActionPlanQuestionDecimal : ActionPlanQuestionScript<float>
{
    public ActionPlanQuestionDecimal()
    {
        _variableType = VariableTypeForAPEnum.Decimal;

        _questionAnswered = true;
    }

    public override float GetDecimalAnswer()
    {
        return _answer;
    }

    public override void SetDecimalAnswer(float _input)
    {
        _answer = _input;
    }

    public override string GetTextAnswer()
    {
        return _answer.ToString("0.00");
    }

    public override string GetWholeAnswerAsString()
    {
        return _answer.ToString("0.00");
    }

    protected override void IsQuestionAnswered()
    {
        _questionAnswered = true;
    }
}

public class ActionPlanQuestionText : ActionPlanQuestionScript<string>
{
    [SerializeField]
    string _inputFieldHolderText;

    public ActionPlanQuestionText()
    {
        _variableType = VariableTypeForAPEnum.Text;
    }

    public override string GetTextAnswer()
    {
        return _answer;
    }

    public override string GetWholeAnswerAsString()
    {
        return _answer;
    }

    public override void SetTextAnswer(string _input)
    {
        _answer = _input;
    }

    protected override void IsQuestionAnswered()
    {
        _questionAnswered = _answer != "";
    }

    public string GetInputFieldHolderText()
    {
        return _inputFieldHolderText;
    }
}


[Description("This should only be used for enum variables.")]
public abstract class ActionPlanQuestionEnum<T> : ActionPlanQuestionScript<T> where T : Enum
{
    [SerializeField]
    protected APEnumTypeEnum _enumType;

    [SerializeField]
    protected ActionPlanEnumLabelProperties<T> _labelProperties;

    public ActionPlanQuestionEnum() : base()
    {
        _variableType = VariableTypeForAPEnum.Enum;
    }


    /*protected int _selectedValueIndex = 0;

    public int GetSelectedValueIndex()
    {
        return _selectedValueIndex;
    }*/

    /*
    public override void SetValue(T _input)
    {
        base.SetValue(_input);
    }

    public virtual void SetSelectedValueIndex(int _input)
    {
        _selectedValueIndex = _input;
    }*/

    public APEnumTypeEnum GetEnumType()
    {
        return _enumType;
    }

    public ActionPlanEnumLabelProperties<T> GetLabelProperties()
    {
        return _labelProperties;
    }
}

public class ActionPlanQuestionEnum_InhalerColor: ActionPlanQuestionEnum<InhalerColorEnum>
{
    public ActionPlanQuestionEnum_InhalerColor() : base()
    {
        _questionAnswered = true;

        _enumType = APEnumTypeEnum.Inhaler_Color;
    }

    public override int GetIntegerAnswer()
    {
        return (int)(_labelProperties.GetEnum());
    }

    public override string GetTextAnswer()
    {
        return _answer.ToString();
    }

    public override string GetWholeAnswerAsString()
    {
        return _answer.ToString();
    }

    public override void SetIntegerAnswer(int _input)
    {
        base.SetIntegerAnswer(_input);

        _answer = (InhalerColorEnum)(_input);
    }

    public override Enum GetEnumAnswer()
    {
        return _answer;
    }

    protected override void IsQuestionAnswered()
    {
        _questionAnswered = true;
    }

    /*public override void SetValue(InhalerColorEnum _input)
    {
        base.SetValue(_input);

        _selectedValueIndex = (int)_input;
    }

    public override void SetIntegerAnswer(int _input)
    {
        base.SetIntegerAnswer(_input);

        _answer = (InhalerColorEnum)_input;

        switch(_input)
        {
            case 1:
                _answer = InhalerColorEnum.Brown;
                break;
            case 2:
                _answer = InhalerColorEnum.Orange;
                break;
            case 3:
                _answer = InhalerColorEnum.Purple;
                break;
            default:
                _answer = InhalerColorEnum.Blue;
                break;
        }
    
     }*/
}

public class ActionPlanQuestionEnum_Gender:ActionPlanQuestionEnum<GenderEnum>
{
    public ActionPlanQuestionEnum_Gender():base()
    {
        _questionAnswered = true;

        _enumType = APEnumTypeEnum.Gender;
    }

    public override Enum GetEnumAnswer()
    {
        return (_answer);
    }

    public override int GetIntegerAnswer()
    {
        return (int)(_labelProperties.GetEnum());
    }

    public override string GetTextAnswer()
    {
        return _answer.ToString();
    }

    public override string GetWholeAnswerAsString()
    {
        return _answer.ToString();
    }

    public override void SetIntegerAnswer(int _input)
    {
        _answer = (GenderEnum)_input;
    }

    protected override void IsQuestionAnswered()
    {
        _questionAnswered = true;
    }
}

public class ActionPlanQuestionDate : ActionPlanQuestionGenericTypeScript<DateClass>
{
    public ActionPlanQuestionDate():base()
    {
        _variableType = VariableTypeForAPEnum.Date;

        _answer = new DateClass(false);
    }

    public override DateClass GetDateAnswer()
    {
        return _answer;
    }

    public override string GetTextAnswer()
    {
        return _answer.GetDateText();
    }

    public override string GetWholeAnswerAsString()
    {
        return _answer.GetDateText();
    }

    public override void SetDateAnswer(int _dayInput, int _monthInput, int _yearInput)
    {
        _answer.SetDate(_dayInput, _monthInput, _yearInput);
    }

    public override void SetAnswerFromString(string _input)
    {
        _answer.SetDateFromString(_input);
    }

    protected override void IsQuestionAnswered()
    {
        /*bool _bool = _answer.GetDay() != 0 && _answer.GetMonth() != 0 && _answer.GetYear() != 0;

        _bool = _bool && _answer.GetDay() != -1 && _answer.GetMonth() != -1 && _answer.GetYear() != 1;*/

        _questionAnswered = _answer.GetDateSet();
    }
}

public class ActionPlanQuestionContact : ActionPlanQuestionGenericTypeScript<TwoVariablesClass<string, string>>
{
    public ActionPlanQuestionContact()
    {
        _variableType = VariableTypeForAPEnum.Contact;
    }

    /*public override int GetIntegerAnswer()
    {
        return _answer.GetVariable2();
    }*/

    public override string GetTextAnswer()
    {
        return _answer.GetVariable1() + ", " + _answer.GetVariable2();
    }

    public override string GetWholeAnswerAsString()
    {
        return _answer.GetVariable1() + ", " + _answer.GetVariable2();
    }

    /*public override void SetIntegerAnswer(int _input)
    {
        _answer.SetVariable2(_input);
    }*/

    public override void SetTextAnswer(string _input)
    {
        //_answer.SetVariable1(_input);

        string _st1 = "";

        string _st2 = "";

        int _stage = 0;

        for (int _i = 0; _i < _input.Length; _i++)
        {
            if (_input[_i] == ',')
            {
                _stage++;
            }
            else
            {
                if (_stage == 1 && _input[_i] != ' ')
                {
                    _st2 += _input[_i];
                }
                else if (_stage == 0)
                {
                    _st1 += _input[_i];
                }
            }
        }

        //int.TryParse(_st2, out int _intValue);

        _answer.SetVariables(_st1, _st2);
    }

    public override void SetAnswerFromString(string _input)
    {
        string _st1 = "";

        string _st2 = "";

        int _stage = 0;

        for(int _i = 0; _i < _input.Length; _i++)
        {
            if (_input[_i] == ',')
            {
                _stage++;
            }
            else
            {
                if(_stage == 1 && _input[_i] != ' ')
                {
                    _st2 += _input[_i];
                }
                else if(_stage == 0)
                {
                    _st1 += _input[_i];
                }
            }
        }

        //int.TryParse(_st2, out int _intValue);

        _answer.SetVariables(_st1, _st2);
    }

    protected override void IsQuestionAnswered()
    {
        bool _bool2 = _answer.GetVariable1() != "";

        _bool2 = _bool2 && _answer.GetVariable2() != "";

        _questionAnswered = _bool2;
    }
}
