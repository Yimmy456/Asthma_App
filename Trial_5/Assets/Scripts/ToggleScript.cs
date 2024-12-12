using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    [SerializeField]
    Toggle _toggle;

    [Header("If it is on.")]

    [SerializeField]
    ColorBlock _colorsIfSelected;

    /*[SerializeField]
    Color _defaultColorY;

    [SerializeField]
    Color _highlightedColorY;

    [SerializeField]
    Color _pressedColorY;*/

    [Header("If it is not on.")]

    [SerializeField]
    ColorBlock _colorsIfUnselected;

    // Update is called once per frame
    void Update()
    {
        if(_toggle.isOn)
        {
            //ColorBlock _cb = _colorsIfSelected;

            /*_cb.normalColor = _defaultColorY;
            _cb.highlightedColor = _highlightedColorY;
            _cb.pressedColor = _colo*/

            _toggle.colors = _colorsIfSelected;

            //_toggle.colors.normalColor = _defaultColorY;
        }
        else
        {
            _toggle.colors = _colorsIfUnselected;
        }
    }

    public Toggle GetToggle()
    {
        return _toggle;
    }
}
