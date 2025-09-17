using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISafeAreaScript : MonoBehaviour
{
    [SerializeField]
    RectTransform _rectTransform;

    [SerializeField]
    Rect _safeArea;

    [SerializeField]
    Vector2 _anchorMin;

    [SerializeField]
    Vector2 _anchorMax;

    void Start()
    {
        _safeArea = Screen.safeArea;

        _anchorMin = _safeArea.position;

        _anchorMax = _anchorMin + _safeArea.size;


        _anchorMin.x = _anchorMin.x / Screen.width;

        _anchorMin.y = _anchorMin.y / Screen.height;

        _anchorMax.x = _anchorMax.x / Screen.width;

        _anchorMax.y = _anchorMax.y / Screen.height;


        _rectTransform.anchorMin = _anchorMin;

        _rectTransform.anchorMax = _anchorMax;
    }
}
