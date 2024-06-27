using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Linq;
using System;
using System.ComponentModel;
using Unity.VisualScripting;
//using UnityEditorInternal;

public class PrintingManagerScript : MonoBehaviour
{
    string _path = "";

    string _fontPath = "";

    [SerializeField]
    Color _firstRowColor;

    [SerializeField]
    Color _secondRowColor;

    [SerializeField]
    float _rowHeight = 40.0f;

    [Description("You need to put the ID of the question in the list.")]
    [SerializeField]
    List<string> _questionsToAsk;

    BaseColor _firstRowBaseColor;

    BaseColor _secondRowBaseColor;

    [SerializeField]
    float[] _columnWidths;

    [SerializeField]
    UnityEngine.Font _fontVar;

    //Html

    // Start is called before the first frame update
    void Start()
    {
        _path = Application.persistentDataPath + "/AsthmaStatusDoc.pdf";

        _fontPath = Application.dataPath + "/Bubble Bobble.ttf";

        /*if (Application.platform == RuntimePlatform.Android)
        {
            _fontPath = Path.Combine("/system/fonts/", "BUBBLE BOBBLE.TTF");
        }*/

        Debug.Log("Font is '" + _fontVar.name + "'.");

        Debug.Log("Font path is '" + _fontPath + "'.");

        _firstRowBaseColor = new BaseColor(_firstRowColor.r, _firstRowColor.g, _firstRowColor.b, _firstRowColor.a);

        _secondRowBaseColor = new BaseColor(_secondRowColor.r, _secondRowColor.g, _secondRowColor.b, _secondRowColor.a);
    }

    private void OnEnable()
    {
        if(_firstRowBaseColor == null)
        {
            _firstRowBaseColor = new BaseColor(_firstRowColor.r, _firstRowColor.g, _firstRowColor.b, _firstRowColor.a);
        }

        if(_secondRowBaseColor == null)
        {
            _secondRowBaseColor = new BaseColor(_secondRowColor.r, _secondRowColor.g, _secondRowColor.b, _secondRowColor.a);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<string> GetQuestionsToAsk()
    {
        return _questionsToAsk;
    }

    public void GenerateFile()
    {

        if(ActionPlanManagerScript.GetInstance() == null)
        {
            return;
        }

        bool _allQuestionsAnswered = ActionPlanManagerScript.GetInstance().GetAllQuestionsAnswered();

        if(!_allQuestionsAnswered)
        {
            Debug.LogError("Some answers have not been answered yet. You need to answer all of the questions before you can print.");
            
            return;
        }

        List<ActionPlanQuestionScript> _questions = GetQuestions();

        if(File.Exists(_path))
        {
            File.Delete(_path);
        }

        using(var _fileStream = new FileStream(_path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            var _doc = new Document(PageSize.A4, 20.0f, 20.0f, 20.0f, 40.0f);

            var _writer = PdfWriter.GetInstance(_doc, _fileStream);

            //File.Copy(_fontVar, Application.persistentDataPath)

            //var _baseFont = BaseFont.CreateFont(_fontPath, BaseFont.IDENTITY_H, true);

            /*if (Application.platform == RuntimePlatform.Android) {

                FontFactory.Register(_fontPath, "BUBBLE BOBBLE");
            }*/
            //var _baseFont = BaseFont.CreateFont(FontFactory.HELVETICA, BaseFont.CP1252, false);

            //BaseFont _baseFont = BaseFont.CreateFont(_fontPath, BaseFont.IDENTITY_H, true);

            //var _pagesFont = new iTextSharp.text.Font(_baseFont, 12, iTextSharp.text.Font.NORMAL);

            //var _baseFont = BaseFont.CreateFont(_fontPath, BaseFont.IDENTITY_H, true);

            /*FontFactory.Register(Application.dataPath + "/" + _fontVar.name + ".ttf", "Bubble Bobble");

            if(Application.platform == RuntimePlatform.Android)
            {
                string _tempPath = Path.Combine("/system/fonts/", _fontVar.name + ".ttf");

                FontFactory.Register(_tempPath, "Bubble Bobble");
            }*/

            var _baseFont = BaseFont.CreateFont(FontFactory.HELVETICA, BaseFont.IDENTITY_H, true);

            var _normalFont = new iTextSharp.text.Font(_baseFont, 14, iTextSharp.text.Font.NORMAL);

            var _miniFont = new iTextSharp.text.Font(_baseFont, 11, iTextSharp.text.Font.NORMAL);

            var _headingFont = new iTextSharp.text.Font(_baseFont, 20, iTextSharp.text.Font.NORMAL);

            //var _normalFont = FontFactory.GetFont("Bubble Bobble", BaseFont.IDENTITY_H, false, 14, iTextSharp.text.Font.NORMAL);

            //var _miniFont = FontFactory.GetFont("Bubble Bobble", BaseFont.IDENTITY_H, false, 11, iTextSharp.text.Font.NORMAL);

            //var _headingFont = FontFactory.GetFont("Bubble Bobble", BaseFont.IDENTITY_H, false, 20, iTextSharp.text.Font.NORMAL);

            //FontFactory.Register()

            /*if(Application.platform == RuntimePlatform.Android)
            {
                _normalFont = FontFactory.GetFont("BUBBLE BOBBLE",
                    BaseFont.IDENTITY_H,
                    false,
                    14,
                    iTextSharp.text.Font.NORMAL);

                _miniFont = FontFactory.GetFont("BUBBLE BOBBLE",
                    BaseFont.IDENTITY_H,
                    false,
                    11,
                    iTextSharp.text.Font.NORMAL);

                _headingFont = FontFactory.GetFont("BUBBLE BOBBLE",
                    BaseFont.IDENTITY_H,
                    false,
                    20,
                    iTextSharp.text.Font.NORMAL);
            }*/

            _doc.Open();           
            
            Paragraph _par = new Paragraph();

            _par.Font = _miniFont;

            _par.Add("This document was printed on " + @"""" + DateTime.Now.ToString("MMMM d, yyyy (dddd), h:mm tt") + @"""" + ",\n\n");

            var _timeDifference = DateTime.Now - DateTime.UtcNow;

            char _signChar = _timeDifference.Hours < 0.0f ? '-' : '+';

            string _timeDifferenceSt = _signChar + Math.Abs(_timeDifference.Hours).ToString() + ":" + _timeDifference.Minutes.ToString("00");

            _par.Add("The timezone of printing this document was " + @"""" + "UTC" + _timeDifferenceSt + @"""" + ".\n\n\n");

            _doc.Add(_par);

            _par = new Paragraph();

            _par.Font = _headingFont;

            _par.Add("User's Information");

            _doc.Add(_par);

            _par = new Paragraph();

            _par.Font = _normalFont;

            _par.Add("\n");

            _doc.Add(_par);

            PdfPTable _table = new PdfPTable(3);

            _table.DefaultCell.Border = 2;

            _table.HorizontalAlignment = 0;

            _table.LockedWidth = true;

            _table.TotalWidth = _columnWidths.Sum();

            _table.SetTotalWidth(_columnWidths);

            //_table.TotalWidth = 270.0f

            //_table.SetTotalWidth(new float[] { 20.0f, 90.0f, 160.0f });

            PrintPreInformation(ref _doc, ref _table, ref _normalFont);

            _table.DefaultCell.Border = 2;

            //_table.DefaultCell.

            _doc.Add(_table);

            _par = new Paragraph();

            _par.Font = _headingFont;

            _par.Add("\n\nQuestions");

            _doc.Add(_par);

            _par = new Paragraph();

            _par.Font = _normalFont;

            //Chunk _chunk;

            List _l = new List(List.ORDERED, 20.0f);

            //_l.SetListSymbol("Question ");

            List _aList = new List(List.UNORDERED, 20.0f);

            //_aList.SetListSymbol("\u2022");

            for(int _i = 0; _i < _questions.Count; _i++)
            {
                //_par = new Paragraph();

                //_par.Add("Question " + (_i + 1).ToString() + ". ");

                _l.Add(new ListItem(40, _questions[_i].GetQuestionText(), _normalFont));

                if (_questions[_i].GetVariableType() == VariableTypeForAPEnum.Integer)
                {
                    PrintIfInteger(_questions[_i], ref _aList, ref _normalFont);
                }

                if (_questions[_i].GetVariableType() == VariableTypeForAPEnum.Decimal)
                {
                    PrintIfDecimal(_questions[_i], ref _aList, ref _normalFont);
                }

                if (_questions[_i].GetVariableType() == VariableTypeForAPEnum.Text)
                {
                    PrintIfText(_questions[_i], ref _aList, ref _normalFont);
                }

                if (_questions[_i].GetVariableType() == VariableTypeForAPEnum.Enum)
                {
                    PrintIfEnum(_questions[_i], ref _aList, ref _normalFont );
                }

                if (_questions[_i].GetVariableType() == VariableTypeForAPEnum.Date)
                {
                    PrintIfDate(_questions[_i], ref _aList, ref _normalFont);
                }

                _l.Add(_aList);

                //if (_questions[_i].GetVariableType() == VariableTypeForAPEnum.Enum)

                //_par.TabSettings = new TabSettings(200);

                //_par.Add(Chunk.TABBING);

                //_doc.Add(_par);

                //_chunk = new Chunk("Apple");

                //_doc.Add(_chunk);
            }

            /*_par = new Paragraph();

            _par.Add(_l);

            _par.SpacingAfter = 15.0f;*/

            _doc.Add(_l);

            _doc.Close();

            _writer.Close();

            AddPageNumber();
        }

        PrintFile();
    }

    void PrintFile()
    {
        if(_path == null)
        {
            return;
        }

        if(File.Exists(_path))
        {
            Debug.Log("File is found. We will start printing.");
        }
        else
        {
            Debug.LogError("File is not found.");
            return;
        }

        System.Diagnostics.Process _process = new System.Diagnostics.Process();

        _process.StartInfo.CreateNoWindow = true;
        _process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
        _process.StartInfo.UseShellExecute = true;
        _process.StartInfo.FileName = _path;

        _process.Start();
    }


    protected void AddPageNumber()
    {
        byte[] _bytes = File.ReadAllBytes(_path);

        //BaseFont _baseFont = BaseFont.CreateFont(_fontPath, BaseFont.IDENTITY_H, true);

        var _baseFont = BaseFont.CreateFont(FontFactory.HELVETICA, BaseFont.CP1252, false);

        var _pagesFont = new iTextSharp.text.Font(_baseFont, 12, iTextSharp.text.Font.NORMAL);

        _pagesFont.SetColor(100, 100, 100);

        using(MemoryStream _stream = new MemoryStream())
        {
            PdfReader _reader = new PdfReader(_bytes);

            using(PdfStamper _stamper = new PdfStamper(_reader, _stream))
            {
                int _pages = _reader.NumberOfPages;

                for(int _i = 1; _i <= _pages; _i++)
                {
                    ColumnText.ShowTextAligned(_stamper.GetUnderContent(_i), Element.ALIGN_CENTER, new Phrase(("Page: " + _i.ToString() + "/" + _pages.ToString()), _pagesFont), 284f, 20f, 0);
                }
            }
            _bytes = _stream.ToArray();
        }
        File.WriteAllBytes(_path, _bytes);
    }

    void PrintIfInteger(ActionPlanQuestionScript _questionInput, ref List _listInput, ref iTextSharp.text.Font _fontInput)
    {
        ActionPlanQuestionInteger _integerQ = (ActionPlanQuestionInteger)(_questionInput);

        if(_integerQ == null)
        {
            return;
        }

        _listInput = new List(List.UNORDERED, 10.0f);

        _listInput.Add(new ListItem(_integerQ.GetIntegerAnswer().ToString(), _fontInput));

        //_paragraphInput.Add(_integerQ.GetQuestionText());

        //_paragraphInput.Add("\n\n");

        //_paragraphInput.Add("A. " + _integerQ.GetIntegerAnswer().ToString());

        //_paragraphInput.Add("A." + Paragraph.)
    }

    void PrintIfDecimal(ActionPlanQuestionScript _questionInput, ref List _listInput, ref iTextSharp.text.Font _fontInput)
    {
        ActionPlanQuestionDecimal _decimalQ = (ActionPlanQuestionDecimal)(_questionInput);

        if(_decimalQ == null)
        {
            return;
        }

        _listInput = new List(List.UNORDERED, 10.0f);

        _listInput.Add(new ListItem(_decimalQ.GetDecimalAnswer().ToString(), _fontInput));

        //_paragraphInput.Add(_decimalQ.GetQuestionText() + "\n\n");

        //_paragraphInput.Add("A. " + _decimalQ.GetAnswer().ToString());
    }

    void PrintIfText(ActionPlanQuestionScript _questionInput, ref List _listInput, ref iTextSharp.text.Font _fontInput)
    {
        ActionPlanQuestionText _textQ = (ActionPlanQuestionText)(_questionInput);

        if(_textQ == null)
        {
            return;
        }

        _listInput = new List(List.UNORDERED, 10.0f);

        _listInput.Add(new ListItem(_textQ.GetTextAnswer(), _fontInput));


        //_paragraphInput.Add(_textQ.GetQuestionText() + "\n\n");

        //_paragraphInput.Add("A. " + _textQ.GetAnswer());
    }

    void PrintIfEnum(ActionPlanQuestionScript _questionInput, ref List _listInput, ref iTextSharp.text.Font _fontInput)
    {
        //Action

        bool _confirmType = (ActionPlanQuestionEnum_InhalerColor)_questionInput != null;

        if(_confirmType)
        {
            ActionPlanQuestionEnum_InhalerColor _iCQ = (ActionPlanQuestionEnum_InhalerColor)(_questionInput);

            //_paragraphInput.Add(_iCQ.GetQuestionText() + "\n\n");

            //_paragraphInput.Add("A. " + _iCQ.GetAnswer().ToString());

            _listInput = new List(List.UNORDERED, 10.0f);

            _listInput.Add(new ListItem(_iCQ.GetAnswer().ToString(), _fontInput));

            return;
        }
    }

    void PrintIfDate(ActionPlanQuestionScript _questionInput, ref List _listInput, ref iTextSharp.text.Font _fontInput)
    {
        ActionPlanQuestionDate _dateQ = (ActionPlanQuestionDate)(_questionInput);

        if(_dateQ == null)
        {
            return;
        }

        if(!_dateQ.GetDateAnswer().GetDateSet())
        {
            return;
        }

        DateTime _newDate = new DateTime(_dateQ.GetAnswer().GetYear(), _dateQ.GetAnswer().GetMonth(), _dateQ.GetAnswer().GetDay());

        //_paragraphInput.Add(_dateQ.GetQuestionText() + "\n\n");

        //_paragraphInput.Add("A. " + _newDate.ToString("MMMM dd (dddd), yyyy"));

        _listInput = new List(List.UNORDERED, 10.0f);

        _listInput.Add(new ListItem(_newDate.ToString("MMMM dd, yyyy (dddd)"), _fontInput));
    }

    List<ActionPlanQuestionScript> GetQuestions()
    {
        List<ActionPlanQuestionScript> _list = new List<ActionPlanQuestionScript>();
        for(int _i = 0; _i < ActionPlanManagerScript.GetInstance().GetQuestionList().Count; _i++)
        {
            ActionPlanQuestionScript _currentQ = ActionPlanManagerScript.GetInstance().GetQuestionList()[_i];

            string _id = _currentQ.GetQuestionID();

            if(_questionsToAsk.Contains(_id))
            {
                _list.Add(_currentQ);
            }
        }

        return _list;
    }

    void GetAge(ref int _ageOutput, ref DateClass _birthDateOutput)
    {
        ActionPlanQuestionScript _q = null;

        ActionPlanQuestionScript _currentQuestion;

        string _questionText = "What is your date of birth?";

        for(int _i = 0; _i < ActionPlanManagerScript.GetInstance().GetQuestionList().Count && _q == null; _i++)
        {
            _currentQuestion = ActionPlanManagerScript.GetInstance().GetQuestionList()[_i];

            if(string.Compare(_questionText, _currentQuestion.GetQuestionText(), true) == 0)
            {
                _q = _currentQuestion;
            }
        }

        if(_q == null)
        {
            return;
        }

        ActionPlanQuestionDate _dateClass = (ActionPlanQuestionDate)(_q);

        if(_dateClass == null)
        {
            return;
        }

        _birthDateOutput.SetDate(_dateClass.GetAnswer().GetDay(), _dateClass.GetAnswer().GetMonth(), _dateClass.GetAnswer().GetYear());

        //_birthDateOutput.SetMonth()

        int _age = DateTime.Now.Year - _dateClass.GetAnswer().GetYear();

        bool _daysBefore = DateTime.Now.Month == _dateClass.GetAnswer().GetMonth() && DateTime.Now.Day < _dateClass.GetAnswer().GetDay();

        if(DateTime.Now.Month < _dateClass.GetAnswer().GetMonth() || _daysBefore)
        {
            _age--;
        }

        _ageOutput = _age;
    }

    void PrintPreInformation(ref Document _docInput, ref PdfPTable _tableInput, ref iTextSharp.text.Font _fontInput)
    {
        int _basicInfo = 1;

        int _age = -1;

        GenderEnum _gender = GenderEnum.Unknown;

        DateClass _date = new DateClass(true);

        string _nameText = "";

        //1. Name

        GetName(ref _nameText);

        PdfPCell _cell = new PdfPCell();

        if (_nameText != "")
        {
            _cell = new PdfPCell(new Phrase(_basicInfo.ToString() + ".", _fontInput));

            _cell.BorderColor = new BaseColor(0.0f, 0.0f, 0.0f, 0.0f);

            _cell.VerticalAlignment = Element.ALIGN_MIDDLE;

            _cell.FixedHeight = _rowHeight;

            _cell.BackgroundColor = _firstRowBaseColor;

            _tableInput.AddCell(_cell);

            _cell = new PdfPCell(new Phrase("User's name:", _fontInput));

            _cell.VerticalAlignment= Element.ALIGN_MIDDLE;

            _cell.BackgroundColor = _firstRowBaseColor;

            _tableInput.AddCell(_cell);

            _cell = new PdfPCell(new Phrase(_nameText, _fontInput));

            _cell.VerticalAlignment = Element.ALIGN_MIDDLE;

            _cell.BackgroundColor = _firstRowBaseColor;

            _tableInput.AddCell(_cell);
        }

        //2. Date of Birth

        _basicInfo++;

        if(!_date.GetDateSet())
        {
            return;
        }

        GetAge(ref _age, ref _date);

        DateTime _birthDate = new DateTime(_date.GetYear(), _date.GetMonth(), _date.GetDay());

        _cell = new PdfPCell(new Phrase(_basicInfo.ToString() + ".", _fontInput));

        _cell.VerticalAlignment = Element.ALIGN_MIDDLE;

        _cell.BackgroundColor = _secondRowBaseColor;

        _cell.FixedHeight = _rowHeight;

        _tableInput.AddCell(_cell);

        _cell = new PdfPCell(new Phrase("Date of Birth:", _fontInput));

        _cell.VerticalAlignment = Element.ALIGN_MIDDLE;

        _cell.BackgroundColor = _secondRowBaseColor;

        _tableInput.AddCell(_cell);

        _cell = new PdfPCell(new Phrase(_birthDate.ToString("MMMM dd, yyyy (dddd)"), _fontInput));

        _cell.VerticalAlignment = Element.ALIGN_MIDDLE;

        _cell.BackgroundColor = _secondRowBaseColor;

        _tableInput.AddCell(_cell);

        //3. Age

        if (_age != -1)
        {
            _basicInfo++;

            _cell = new PdfPCell(new Phrase(_basicInfo.ToString() + ".", _fontInput));

            _cell.BackgroundColor = _firstRowBaseColor;

            _cell.VerticalAlignment = Element.ALIGN_MIDDLE;

            _cell.FixedHeight = _rowHeight;

            _tableInput.AddCell(_cell);

            _cell = new PdfPCell(new Phrase("Age:", _fontInput));

            _cell.BackgroundColor = _firstRowBaseColor;

            _cell.VerticalAlignment= Element.ALIGN_MIDDLE;

            _tableInput.AddCell(_cell);

            _cell = new PdfPCell(new Phrase(_age.ToString() + " years old", _fontInput));

            _cell.BackgroundColor = _firstRowBaseColor;

            _cell.VerticalAlignment = Element.ALIGN_MIDDLE;

            _tableInput.AddCell(_cell);
        }

        //4. Gender

        GetGender(ref _gender);

        if (_gender == GenderEnum.Unknown)
        {
            //_paragraphInput.Add("Does not wish to specify gender.");

            _cell = new PdfPCell(new Phrase("Does not wish to specify gender.", _fontInput));

            _cell.Colspan = 3;

            _cell.HorizontalAlignment = 0;

            _cell.BackgroundColor = _secondRowBaseColor;

            _cell.VerticalAlignment = Element.ALIGN_MIDDLE;

            _cell.FixedHeight = _rowHeight;

            _tableInput.AddCell(_cell);
        }
        else
        {
            _basicInfo++;

            _cell = new PdfPCell(new Phrase(_basicInfo.ToString() + ".", _fontInput));

            _cell.BackgroundColor = _secondRowBaseColor;

            _cell.FixedHeight = _rowHeight;

            _cell.VerticalAlignment = Element.ALIGN_MIDDLE;

            _tableInput.AddCell(_cell);

            _cell = new PdfPCell(new Phrase("Gender:", _fontInput));

            _cell.VerticalAlignment = Element.ALIGN_MIDDLE;

            _cell.BackgroundColor = _secondRowBaseColor;

            _tableInput.AddCell(_cell);

            _cell = new PdfPCell(new Phrase(_gender.ToString(), _fontInput));

            _cell.VerticalAlignment = Element.ALIGN_MIDDLE;

            _cell.BackgroundColor = _secondRowBaseColor;

            _tableInput.AddCell(_cell);
        }
    }

    void GetGender(ref GenderEnum _output)
    {
        ActionPlanQuestionScript _q2 = null;

        List<ActionPlanQuestionScript> _list2 = ActionPlanManagerScript.GetInstance().GetQuestionList();

        ActionPlanQuestionScript _currentQ2;

        string _text = "What is your gender?";

        for(int _i = 0; _i < _list2.Count && _q2 == null; _i++)
        {
            _currentQ2 = _list2[_i];

            if(string.Compare(_currentQ2.GetQuestionText(), _text, true) == 0)
            {
                _q2 = _currentQ2;
            }
        }

        if(_q2 == null)
        {
            return;
        }

        ActionPlanQuestionEnum_Gender _genderQuestion = (ActionPlanQuestionEnum_Gender)(_q2);

        if(_genderQuestion == null)
        {
            return;
        }

        _output = _genderQuestion.GetAnswer();
    }

    void GetName(ref string _output)
    {
        ActionPlanQuestionScript _q3 = null;

        List<ActionPlanQuestionScript> _list3 = ActionPlanManagerScript.GetInstance().GetQuestionList();

        ActionPlanQuestionScript _currentQ3;

        string _text3 = "What is your name?";

        for(int _i = 0; _i < _list3.Count && _q3 == null; _i++)
        {
            _currentQ3 = _list3[_i];

            if(string.Compare(_currentQ3.GetQuestionText(), _text3, true) == 0)
            {
                _q3 = _currentQ3;
            }
        }

        if(_q3 == null)
        {
            return;
        }

        ActionPlanQuestionText _textQuestion = (ActionPlanQuestionText)(_q3);

        if(_textQuestion == null)
        {
            return;
        }

        _output = _textQuestion.GetAnswer();
    }
}
