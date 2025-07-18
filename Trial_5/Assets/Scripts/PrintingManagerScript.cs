﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Linq;
using System;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine.Networking;
using UnityEngine.UI;
using I18N;
using I18N.West;
using System.Diagnostics;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class PrintingManagerScript : MonoBehaviour
{
    string _path = "";

    string _fontPath = "";

    string _genderImagePath = "";

    [SerializeField]
    Color _firstRowColor;

    [SerializeField]
    Color _secondRowColor;

    [SerializeField]
    float _rowHeight = 40.0f;

    [Description("You need to put the ID of the question in the list.")]
    [SerializeField]
    List<string> _questionsToAsk;

    [SerializeField]
    int _subListLeading = 20;

    [SerializeField]
    int _questionsSpace = 2;

    [SerializeField]
    Text _addressText;

    BaseColor _firstRowBaseColor;

    BaseColor _secondRowBaseColor;

    [SerializeField]
    float[] _columnWidths;

    [SerializeField]
    UnityEngine.Font _fontVar;

    [SerializeField]
    Text _statusText;

    [SerializeField]
    Canvas _targetCanvas;

    [SerializeField]
    ProcessStatusEnum _printingProcessStatus;

    string _fullAnswer = "";

    Coroutine _statusTextCoroutine;

    string _errorText;

    // Start is called before the first frame update
    void Start()
    {
        //_path = Application.persistentDataPath + "/Asthma Status Document.pdf";

        //_path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        //_path = _path + "/Asthma Status Document.pdf";

        _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "Asthma Status Document.pdf");

        _firstRowBaseColor = new BaseColor(_firstRowColor.r, _firstRowColor.g, _firstRowColor.b, _firstRowColor.a);

        _secondRowBaseColor = new BaseColor(_secondRowColor.r, _secondRowColor.g, _secondRowColor.b, _secondRowColor.a);

        StartCoroutine(CopyFile("BUBBLE_BOBBLE.TTF"));

        _fontPath = Path.Combine(Application.persistentDataPath, "BUBBLE_BOBBLE.TTF");
    }

    private void OnEnable()
    {
        if (_firstRowBaseColor == null)
        {
            _firstRowBaseColor = new BaseColor(_firstRowColor.r, _firstRowColor.g, _firstRowColor.b, _firstRowColor.a);
        }

        if (_secondRowBaseColor == null)
        {
            _secondRowBaseColor = new BaseColor(_secondRowColor.r, _secondRowColor.g, _secondRowColor.b, _secondRowColor.a);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckCanvasStatus();
    }

    public List<string> GetQuestionsToAsk()
    {
        return _questionsToAsk;
    }

    public void TryGeneratingFile()
    {
        if (_printingProcessStatus != ProcessStatusEnum.Idle)
        {
            return;
        }

        //_statusTextCoroutine = StartCoroutine(DisplayPrintingStatus());

        try
        {
            _printingProcessStatus = ProcessStatusEnum.InProgress;

            _statusTextCoroutine = StartCoroutine(DisplayPrintingStatus());

            GenerateFile();
            
            _printingProcessStatus = ProcessStatusEnum.Complete;
            
        }
        catch (Exception ex)
        {
            _errorText = ex.Message;

            if (_errorText == "Success")
            {
                _printingProcessStatus = ProcessStatusEnum.Complete;
            }
            else
            {
                _printingProcessStatus = ProcessStatusEnum.Cancelled;
            }
        }
    }

    void GenerateFile()
    {
        if (ActionPlanManagerScript.GetInstance() == null)
        {
            _printingProcessStatus = ProcessStatusEnum.Cancelled;

            return;
        }

        bool _allQuestionsAnswered = ActionPlanManagerScript.GetInstance().GetAllQuestionsAnswered();

        if (!_allQuestionsAnswered)
        {
            throw new Exception("Some questions have not been answered yet.");
        }

        GrantPermission();

        List<ActionPlanQuestionScript> _questions = GetQuestions();

        if (File.Exists(_path))
        {
            File.Delete(_path);
        }

        using (var _fileStream = new FileStream(_path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            var _doc = new Document(PageSize.A4, 20.0f, 20.0f, 20.0f, 40.0f);

            var _writer = PdfWriter.GetInstance(_doc, _fileStream);

            string _selectedFont = _fontPath;

            var _baseFont = BaseFont.CreateFont(_selectedFont, BaseFont.CP1252, BaseFont.EMBEDDED);

            var _normalFont = new iTextSharp.text.Font(_baseFont, 14, iTextSharp.text.Font.NORMAL);

            var _miniFont = new iTextSharp.text.Font(_baseFont, 11, iTextSharp.text.Font.NORMAL);

            var _headingFont = new iTextSharp.text.Font(_baseFont, 20, iTextSharp.text.Font.NORMAL);

            _doc.Open();

            Paragraph _par = new Paragraph();

            _par.Font = _miniFont;

            var _timeDifference = DateTime.Now - DateTime.UtcNow;

            char _signChar = _timeDifference.Hours < 0.0f ? '-' : '+';

            string _timeDifferenceSt = _signChar + Math.Abs(_timeDifference.Hours).ToString() + ":" + _timeDifference.Minutes.ToString("00");

            //_par.Add("This document was printed on " + @"""" + DateTime.Now.ToString("MMMM dd, yyyy (dddd), h:mm tt") + @"""" + ",\n\n");

            //_par.Add("The timezone of printing this document was " + @"""" + "UTC " + _timeDifferenceSt + @"""" + ".\n\n\n");

            _par.Add("This document was printed on " + @"""" + DateTime.Now.ToString("MMMM dd, yyyy (dddd), h:mm tt") + " (UTC" + _timeDifferenceSt + ")" + @"""" + ",\n\n\n");

            _doc.Add(_par);

            _par = new Paragraph();

            _par.Font = _headingFont;

            _par.Add("User's Information");

            _doc.Add(_par);

            _par = new Paragraph();

            _par.Font = _normalFont;

            _par.Add("\n");

            _doc.Add(_par);

            int _columnsCount = _columnWidths.Length;

            PdfPTable _table = new PdfPTable(_columnsCount);

            _table.DefaultCell.Border = 2;

            _table.HorizontalAlignment = 0;

            _table.LockedWidth = true;

            _table.TotalWidth = _columnWidths.Sum();

            _table.SetTotalWidth(_columnWidths);

            PrintPreInformation(ref _doc, ref _table, ref _normalFont);

            _table.DefaultCell.Border = 0;

            _doc.Add(_table);

            _par = new Paragraph();

            _par.Font = _headingFont;

            _par.Add("\n\nQuestions\n");

            _doc.Add(_par);

            _par = new Paragraph();

            _par.Font = _normalFont;

            List _l = new List(List.ORDERED, 20.0f);

            List _aList = new List(List.UNORDERED, 20.0f);

            for (int _i = 0; _i < _questions.Count; _i++)
            {
                _l.Add(new ListItem(20, _questions[_i].GetQuestionText(), _normalFont));

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
                    PrintIfEnum(_questions[_i], ref _aList, ref _normalFont);
                }

                if (_questions[_i].GetVariableType() == VariableTypeForAPEnum.Date)
                {
                    PrintIfDate(_questions[_i], ref _aList, ref _normalFont);
                }

                _l.Add(_aList);
            }

            _doc.Add(_l);

            _doc.Close();

            _writer.Close();

            AddPageNumber();
        }

        PrintFile();
    }

    void PrintFile()
    {
        if (_path == null)
        {
            return;
        }

        if (File.Exists(_path))
        {
            StartCoroutine(RequestPermissions());

            UnityEngine.Debug.Log("File is found. We will start printing.");
        }
        else
        {
            UnityEngine.Debug.LogError("File is not found.");

            _printingProcessStatus = ProcessStatusEnum.Cancelled;

            return;
        }

        Process _process = new Process();

        _process.StartInfo.CreateNoWindow = true;
        _process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
        _process.StartInfo.UseShellExecute = true;
        _process.StartInfo.FileName = _path;

        _process.Start();

        _printingProcessStatus = ProcessStatusEnum.Complete;

        //_statusTextCoroutine = StartCoroutine(DisplayPrintingStatus2(_process));

        //Debug.Log("The printing process of the action plan is complete.");
    }


    protected void AddPageNumber()
    {
        byte[] _bytes = File.ReadAllBytes(_path);

        var _baseFont = BaseFont.CreateFont(_fontPath, BaseFont.CP1252, BaseFont.EMBEDDED);

        var _pagesFont = new iTextSharp.text.Font(_baseFont, 12, iTextSharp.text.Font.NORMAL);

        _pagesFont.SetColor(100, 100, 100);

        using (MemoryStream _stream = new MemoryStream())
        {
            PdfReader _reader = new PdfReader(_bytes);

            using (PdfStamper _stamper = new PdfStamper(_reader, _stream))
            {
                int _pages = _reader.NumberOfPages;

                for (int _i = 1; _i <= _pages; _i++)
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

        if (_integerQ == null)
        {
            return;
        }

        _listInput = new List(List.UNORDERED, 10.0f);

        _fullAnswer = _integerQ.GetIntegerAnswer().ToString();

        if (_integerQ.GetUnits() != "")
        {
            _fullAnswer = _fullAnswer + " " + _integerQ.GetUnits();
        }

        _fullAnswer = _fullAnswer + ".";

        AddSpaces();

        _listInput.Add(new ListItem(_subListLeading, _fullAnswer, _fontInput));
    }

    void PrintIfDecimal(ActionPlanQuestionScript _questionInput, ref List _listInput, ref iTextSharp.text.Font _fontInput)
    {
        ActionPlanQuestionDecimal _decimalQ = (ActionPlanQuestionDecimal)(_questionInput);

        if (_decimalQ == null)
        {
            return;
        }

        _listInput = new List(List.UNORDERED, 10.0f);

        _fullAnswer = _decimalQ.GetDecimalAnswer().ToString();

        if (_decimalQ.GetUnits() != "")
        {
            _fullAnswer = _fullAnswer + " " + _decimalQ.GetUnits();
        }

        _fullAnswer = _fullAnswer + ".";

        AddSpaces();

        _listInput.Add(new ListItem(_subListLeading, _fullAnswer, _fontInput));
    }

    void PrintIfText(ActionPlanQuestionScript _questionInput, ref List _listInput, ref iTextSharp.text.Font _fontInput)
    {
        ActionPlanQuestionText _textQ = (ActionPlanQuestionText)(_questionInput);

        if (_textQ == null)
        {
            return;
        }

        _listInput = new List(List.UNORDERED, 10.0f);

        _fullAnswer = _textQ.GetTextAnswer() + ".";

        AddSpaces();

        _listInput.Add(new ListItem(_subListLeading, _fullAnswer, _fontInput));
    }

    void PrintIfEnum(ActionPlanQuestionScript _questionInput, ref List _listInput, ref iTextSharp.text.Font _fontInput)
    {
        //Action

        bool _confirmType = (ActionPlanQuestionEnum_InhalerColor)_questionInput != null;

        if (_confirmType)
        {
            ActionPlanQuestionEnum_InhalerColor _iCQ = (ActionPlanQuestionEnum_InhalerColor)(_questionInput);

            _listInput = new List(List.UNORDERED, 10.0f);

            _fullAnswer = _iCQ.GetTextAnswer().ToString() + ".";

            AddSpaces();

            _listInput.Add(new ListItem(_subListLeading, _fullAnswer, _fontInput));
            //_listInput.Add(new ListItem(_subListLeading, _fullAnswer, _fontInput));

            return;
        }
    }

    void PrintIfDate(ActionPlanQuestionScript _questionInput, ref List _listInput, ref iTextSharp.text.Font _fontInput)
    {
        ActionPlanQuestionDate _dateQ = (ActionPlanQuestionDate)(_questionInput);

        if (_dateQ == null)
        {
            return;
        }

        /*if(!_dateQ.GetDateAnswer().GetDateSet())
        {
            return;
        }*/

        DateTime _newDate = new DateTime(_dateQ.GetAnswer().GetDate().Year, _dateQ.GetAnswer().GetDate().Month, _dateQ.GetAnswer().GetDate().Day);

        _listInput = new List(List.UNORDERED, 10.0f);

        _fullAnswer = _newDate.ToString("MMMM dd, yyyy (dddd)") + ".";

        AddSpaces();

        _listInput.Add(new ListItem(_subListLeading, _fullAnswer, _fontInput));
    }

    List<ActionPlanQuestionScript> GetQuestions()
    {
        List<ActionPlanQuestionScript> _list = new List<ActionPlanQuestionScript>();

        for (int _i = 0; _i < ActionPlanManagerScript.GetInstance().GetQuestionList().Count; _i++)
        {
            ActionPlanQuestionScript _currentQ = ActionPlanManagerScript.GetInstance().GetQuestionList()[_i];

            string _id = _currentQ.GetQuestionID();

            if (_questionsToAsk.Contains(_id))
            {
                _list.Add(_currentQ);
            }
        }

        return _list;
    }

    void GetAge(ref int _ageOutput, ref DateTime _birthDateOutput)
    {
        ActionPlanQuestionScript _q = null;

        ActionPlanQuestionScript _currentQuestion;

        string _questionText = "What is your date of birth?";

        for (int _i = 0; _i < ActionPlanManagerScript.GetInstance().GetQuestionList().Count && _q == null; _i++)
        {
            _currentQuestion = ActionPlanManagerScript.GetInstance().GetQuestionList()[_i];

            if (string.Compare(_questionText, _currentQuestion.GetQuestionText(), true) == 0)
            {
                _q = _currentQuestion;
            }
        }

        if (_q == null)
        {
            return;
        }

        ActionPlanQuestionDate _dateClass = (ActionPlanQuestionDate)(_q);

        if (_dateClass == null)
        {
            return;
        }

        _birthDateOutput = new DateTime(_dateClass.GetAnswer().GetDate().Year, _dateClass.GetAnswer().GetDate().Month, _dateClass.GetAnswer().GetDate().Day);

        int _age = DateTime.Now.Year - _dateClass.GetAnswer().GetDate().Year;

        bool _daysBefore = DateTime.Now.Month == _dateClass.GetAnswer().GetDate().Month && DateTime.Now.Day < _dateClass.GetAnswer().GetDate().Day;

        if (DateTime.Now.Month < _dateClass.GetAnswer().GetDate().Month || _daysBefore)
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

        //DateClass _date = new DateClass(true);

        DateTime _date = new DateTime(2020, 1, 1);

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

            //ColorCellBorder(ref _cell, _firstRowBaseColor);

            _tableInput.AddCell(_cell);

            _cell = new PdfPCell(new Phrase("User's name:", _fontInput));

            _cell.VerticalAlignment = Element.ALIGN_MIDDLE;

            _cell.BackgroundColor = _firstRowBaseColor;

            //ColorCellBorder(ref _cell, _firstRowBaseColor);

            _tableInput.AddCell(_cell);

            _cell = new PdfPCell(new Phrase(_nameText, _fontInput));

            _cell.VerticalAlignment = Element.ALIGN_MIDDLE;

            _cell.BackgroundColor = _firstRowBaseColor;

            _cell.Colspan = 2;

            _tableInput.AddCell(_cell);
        }

        //2. Date of Birth

        _basicInfo++;

        GetAge(ref _age, ref _date);

        DateTime _birthDate = new DateTime(_date.Year, _date.Month, _date.Day);

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

        _cell.Colspan = 2;

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

            _cell.VerticalAlignment = Element.ALIGN_MIDDLE;

            _tableInput.AddCell(_cell);

            _fullAnswer = _age.ToString();

            if (_age == 1)
            {
                _fullAnswer = _fullAnswer + " year";
            }
            else
            {
                _fullAnswer = _fullAnswer + " years";
            }

            _fullAnswer = _fullAnswer + " old.";

            _cell = new PdfPCell(new Phrase(_fullAnswer, _fontInput));

            _cell.BackgroundColor = _firstRowBaseColor;

            _cell.VerticalAlignment = Element.ALIGN_MIDDLE;

            _cell.Colspan = 2;

            _tableInput.AddCell(_cell);
        }

        //4. Gender

        GetGender(ref _gender);

        if (_gender == GenderEnum.Unknown)
        {
            _cell = new PdfPCell(new Phrase("Does not wish to specify gender.", _fontInput));

            _cell.Colspan = _columnWidths.Length;

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

            if (_gender == GenderEnum.Male)
            {
                StartCoroutine(CopyFile("Male Symbol.png"));

                _genderImagePath = Path.Combine(Application.persistentDataPath, "Male Symbol.png");
            }
            else if (_gender == GenderEnum.Female)
            {
                StartCoroutine(CopyFile("Female Symbol.png"));

                _genderImagePath = Path.Combine(Application.persistentDataPath, "Female Symbol.png");
            }

            iTextSharp.text.Image _img = iTextSharp.text.Image.GetInstance(_genderImagePath);

            iTextSharp.text.Image _img2 = iTextSharp.text.Image.GetInstance(_genderImagePath);

            _img2.ScaleAbsoluteWidth(_img.Width / 15.0f);

            _img2.ScaleAbsoluteHeight(_img.Height / 15.0f);

            _cell = new PdfPCell(_img2);

            _cell.BackgroundColor = _secondRowBaseColor;

            _cell.VerticalAlignment = Element.ALIGN_MIDDLE;

            _cell.HorizontalAlignment = Element.ALIGN_CENTER;

            _tableInput.AddCell(_cell);
        }
    }

    void GetGender(ref GenderEnum _output)
    {
        ActionPlanQuestionScript _q2 = null;

        List<ActionPlanQuestionScript> _list2 = ActionPlanManagerScript.GetInstance().GetQuestionList();

        ActionPlanQuestionScript _currentQ2;

        string _text = "What is your gender?";

        for (int _i = 0; _i < _list2.Count && _q2 == null; _i++)
        {
            _currentQ2 = _list2[_i];

            if (string.Compare(_currentQ2.GetQuestionText(), _text, true) == 0)
            {
                _q2 = _currentQ2;
            }
        }

        if (_q2 == null)
        {
            return;
        }

        ActionPlanQuestionEnum_Gender _genderQuestion = (ActionPlanQuestionEnum_Gender)(_q2);

        if (_genderQuestion == null)
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

        for (int _i = 0; _i < _list3.Count && _q3 == null; _i++)
        {
            _currentQ3 = _list3[_i];

            if (string.Compare(_currentQ3.GetQuestionText(), _text3, true) == 0)
            {
                _q3 = _currentQ3;
            }
        }

        if (_q3 == null)
        {
            return;
        }

        ActionPlanQuestionText _textQuestion = (ActionPlanQuestionText)(_q3);

        if (_textQuestion == null)
        {
            return;
        }

        _output = _textQuestion.GetAnswer();
    }

    IEnumerator CopyFile(string _fileNameInput)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, _fileNameInput);
        string savePath = Path.Combine(Application.persistentDataPath, _fileNameInput);

        UnityWebRequest uwr = UnityWebRequest.Get(filePath);
        DownloadHandlerFile dhf = new DownloadHandlerFile(savePath);
        dhf.removeFileOnAbort = true;
        uwr.downloadHandler = dhf;
        yield return uwr.SendWebRequest();

        dhf.Dispose();
        uwr.Dispose();
    }

    void AddSpaces()
    {
        for (int _i = 0; _i < _questionsSpace; _i++)
        {
            _fullAnswer = _fullAnswer + "\n";
        }
    }

    IEnumerator DisplayPrintingStatus()
    {
        if (_statusText == null)
        {
            //_printingProcessStatus = ProcessStatusEnum.Cancelled;

            yield break;
        }

        string[] _str = { "Printing PDF", "Printing PDF.", "Printing PDF..", "Printing PDF..." };

        int _index = 0;

        _statusText.color = Color.white;

        _statusText.gameObject.GetComponent<Outline>().effectColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

        while (_printingProcessStatus == ProcessStatusEnum.InProgress)
        {
            _statusText.text = _str[_index];

            yield return new WaitForSeconds(0.25f);

            _index++;

            if (_index >= _str.Length)
            {
                _index = 0;
            }
        }

        if (_printingProcessStatus == ProcessStatusEnum.Complete)
        {
            _statusText.color = Color.green;

            _statusText.gameObject.GetComponent<Outline>().effectColor = new Color(0.0f, 0.5f, 0.0f, 0.5f);

            _statusText.text = "Printing Complete.";

            UnityEngine.Debug.Log("Printing is complete.");

            Application.OpenURL(_path);

            yield return new WaitForSeconds(3.0f);
        }
        else if (_printingProcessStatus == ProcessStatusEnum.Cancelled || _printingProcessStatus == ProcessStatusEnum.Idle)
        {
            _statusText.color = Color.red;

            _statusText.gameObject.GetComponent<Outline>().effectColor = new Color(0.5f, 0.0f, 0.0f, 0.5f);

            _statusText.text = _errorText;

            UnityEngine.Debug.LogError("Printing error is: " + _errorText);

            yield return new WaitForSeconds(6.0f);
        }

        _printingProcessStatus = ProcessStatusEnum.Idle;

        _errorText = "";

        _statusText.text = "";
    }

    IEnumerator DisplayPrintingStatus2(Process _input)
    {
        if (_input == null || _statusText == null)
        {
            yield break;
        }

        string[] _str = { "Printing PDF", "Printing PDF.", "Printing PDF..", "Printing PDF..." };

        int _index = 0;

        _statusText.color = Color.white;

        _statusText.gameObject.GetComponent<Outline>().effectColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

        while (!_input.HasExited)
        {
            _statusText.text = _str[_index];

            yield return new WaitForSeconds(0.25f);

            _index++;

            if (_index >= _str.Length)
            {
                _index = 0;
            }
        }

        _statusText.color = Color.green;

        _statusText.gameObject.GetComponent<Outline>().effectColor = new Color(0.0f, 0.5f, 0.0f, 0.5f);

        _statusText.text = "Printing Complete.";

        yield return new WaitForSeconds(3.0f);

        _statusText.text = "";
    }

    void CheckCanvasStatus()
    {
        if (_targetCanvas == null)
        {
            return;
        }

        if (!_targetCanvas.gameObject.activeSelf && _statusTextCoroutine != null)
        {
            StopCoroutine(_statusTextCoroutine);

            //_printingInProcess = false;

            _printingProcessStatus = ProcessStatusEnum.Idle;

            _statusText.text = "";
        }
    }

    void ColorCellBorder(ref PdfPCell _cellInput, BaseColor _colorInput, float _borderSizeInput = 1.0f)
    {
        if (_borderSizeInput == 0.0f || _cellInput == null)
        {
            return;
        }

        BaseColor _newColor = new BaseColor((_colorInput.R / 2.0f), (_colorInput.G / 2.0f), (_colorInput.B / 2.0f), _colorInput.A);

        _cellInput.BorderColorBottom = _newColor;

        _cellInput.BorderColorTop = _newColor;

        _cellInput.BorderColorLeft = _newColor;

        _cellInput.BorderColorRight = _newColor;

        _cellInput.BorderWidthBottom = _borderSizeInput;

        _cellInput.BorderWidthLeft = _borderSizeInput;

        _cellInput.BorderWidthRight = _borderSizeInput;

        _cellInput.BorderWidthTop = _borderSizeInput;
    }

    IEnumerator RequestPermissions()
    {
        // Android
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) || !Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            {
                Permission.RequestUserPermissions(new string[] { Permission.ExternalStorageRead, Permission.ExternalStorageWrite });
                yield return new WaitUntil(() => Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) && Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite));
            }
            else
            {
                yield break;
            }
        }
        // iOS (Conceptual - requires UIDocumentPickerViewController implementation)
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            // Implement UIDocumentPickerViewController logic here
            // Example:
            // UIDocumentPickerViewController picker = new UIDocumentPickerViewController(new string[] { "public.data" }, UIDocumentPickerMode.Import);
            // picker.delegate = this;
            // picker.present(true);
        }
    }

    void GrantPermission()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            return; // Wait for permission to be granted
        }
#endif
    }
}