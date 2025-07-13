using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;

public class PrintingScript : MonoBehaviour
{
    string _path;

    [SerializeField]
    private Text _printingStatusTextField;

    [SerializeField]
    private GameObject _panel;

    private bool _printing = false;
    private Coroutine _coroutine;

    void Start()
    {
        _path = Path.Combine(Application.persistentDataPath, "Sample Document.pdf");
    }

    public void CreateAndExportPDF()
    {
        if (_printing) return;

        _printing = true;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(SetText("Printing has commenced.", Color.white, 2.0f));

        try
        {
            CreatePDF();

            _coroutine = StartCoroutine(SetText("Printing is complete.", Color.green, 2.0f));

            OpenPDF(_path);
        }
        catch (Exception e)
        {
            Debug.LogError("PDF creation/export failed: " + e.Message);
            _coroutine = StartCoroutine(SetText("Error: " + e.Message, Color.red, 5.0f));
        }

        _printing = false;
    }

    void CreatePDF()
    {
        using (var fileStream = new FileStream(_path, FileMode.Create, FileAccess.Write))
        {
            var doc = new Document(PageSize.A4, 20, 20, 20, 40);
            var writer = PdfWriter.GetInstance(doc, fileStream);

            doc.Open();

            var timeDifference = DateTime.Now - DateTime.UtcNow;
            char signChar = timeDifference.Hours < 0 ? '-' : '+';
            string offset = signChar + Math.Abs(timeDifference.Hours).ToString() + ":" + timeDifference.Minutes.ToString("00");

            string content = $"This document was printed on \"{DateTime.Now:MMMM dd, yyyy (dddd), h:mm tt} (UTC{offset})\".\n\n\n";
            doc.Add(new Paragraph(content));

            doc.Close();
            writer.Close();
        }
    }

    void OpenPDF(string path)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", "android.intent.action.VIEW");

            AndroidJavaClass fileClass = new AndroidJavaClass("java.io.File");
            AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", path);

            if (!fileObject.Call<bool>("exists"))
            {
                Debug.LogError("PDF file does not exist: " + path);
                return;
            }

            AndroidJavaClass fileProvider = new AndroidJavaClass("androidx.core.content.FileProvider");
            string authority = "com.defaultcompany.printingsample.fileprovider";

            AndroidJavaObject uri = fileProvider.CallStatic<AndroidJavaObject>("getUriForFile", currentActivity, authority, fileObject);

            intent.Call<AndroidJavaObject>("setDataAndType", uri, "application/pdf");
            intent.Call<AndroidJavaObject>("addFlags", 1); // FLAG_GRANT_READ_URI_PERMISSION
            intent.Call<AndroidJavaObject>("addFlags", 268435456); // FLAG_ACTIVITY_NEW_TASK

            currentActivity.Call("startActivity", intent);
        }
        catch (Exception e)
        {
            Debug.LogError("Error opening PDF: " + e.Message);
        }
#else
        Debug.Log("PDF opening is supported only on Android devices.");
#endif
    }

    IEnumerator SetText(string message, Color color, float duration)
    {
        if (_printingStatusTextField == null || _panel == null || duration <= 0.0f)
            yield break;

        _panel.SetActive(true);
        _printingStatusTextField.text = message;
        _printingStatusTextField.color = color;

        yield return new WaitForSeconds(duration);

        _panel.SetActive(false);
    }
}