using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TempScript : MonoBehaviour
{
    [SerializeField]
    Text _text;


    string _st = "";
    // Start is called before the first frame update
    void Start()
    {
        _st = Path.Combine(Application.persistentDataPath, "AsthmaGame.json");

        if(_text != null)
        {
            _text.text = "Address is: '" + _st + "'.";
        }
        //Application.OpenURL(Application.persistentDataPath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
