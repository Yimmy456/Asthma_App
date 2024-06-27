using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SliderScript : MonoBehaviour
{
    [SerializeField]
    Slider _slider;

    [SerializeField]
    Text _text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_slider != null && _text != null)
        {
            _text.text = _slider.value.ToString();
        }
    }
}
