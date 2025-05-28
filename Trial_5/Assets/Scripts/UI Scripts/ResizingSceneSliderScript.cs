using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResizingSceneSliderScript : MonoBehaviour
{
    [SerializeField]
    RectTransform _sliderContainer;

    [SerializeField]
    Slider _slider;

    [SerializeField]
    Text _sizeText;

    [SerializeField]
    Transform _sceneTransform;

    [SerializeField]
    Button _showingButton;

    [SerializeField]
    Image _sliderFillImage;

    [SerializeField]
    float _animationSpeed = 5.0f;

    [SerializeField]
    float _waitingSeconds = 10.0f;

    [SerializeField]
    Gradient _gradient;

    float _scale = 1.0f;

    bool _show = false;

    Coroutine _animationCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        if(_slider != null)
        {
            _slider.interactable = false;

            _slider.value = _scale;

            if (_sliderFillImage != null)
            {
                float _ratio = (_slider.value - _slider.minValue) / (_slider.maxValue - _slider.minValue);

                Color _sliderColor = _gradient.Evaluate(_ratio);

                _sliderFillImage.color = _sliderColor;
            }

            if (_sizeText != null)
            {
                _sizeText.text = _slider.value.ToString("0.00");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScene();
    }

    public void ShowSlider()
    {
        if(_show)
        {
            return;
        }

        _showingButton.gameObject.SetActive(false);

        _show = true;

        _animationCoroutine = StartCoroutine(ShowSliderCoroutine());
    }

    IEnumerator ShowSliderCoroutine()
    {
        float _a = 90.0f;

        float _b = -50.0f;

        float _v = _a;

        Vector2 _anchoredPosition = new Vector2(_v, 50.0f);

        bool _loopComplete = false;

        float _t = 0.0f;

        while(!_loopComplete)
        {
            _t = _t + (Time.deltaTime * _animationSpeed);

            _v = ((_t * _b) - (_t * _a)) + _a;

            if (_t >= 1.0f)
            {
                _t = 1.0f;

                _v = _b;

                //Debug.Log("Coming-in is done.");
            }

            _anchoredPosition.x = _v;

            _sliderContainer.anchoredPosition = _anchoredPosition;

            //Debug.Log("Coming-in is in progress at t = " + @"""" + _t.ToString() + @"""" + ".");

            if(_t == 1.0f)
            {
                _loopComplete = true;
            }

            yield return null;
        }

        _show = true;

        _slider.interactable = true;

        _animationCoroutine = StartCoroutine(HideSliderCoroutine());
    }

    IEnumerator HideSliderCoroutine()
    {
        float _a = 90.0f;

        float _b = -50.0f;

        float _v = _b;

        Vector2 _anchoredPosition = new Vector2(_v, 50.0f);

        yield return new WaitForSeconds(_waitingSeconds);

        _slider.interactable = false;

        bool _loopComplete = false;

        float _t = 1.0f;

        while (!_loopComplete)
        {
            _t = _t - (Time.deltaTime * _animationSpeed);

            _v = ((_t * _b) - (_t * _a)) + _a;

            if (_t <= 0.0f)
            {
                _t = 0.0f;

                _v = _a;

                //Debug.Log("Getting-out is done.");
            }

            _anchoredPosition.x = _v;

            _sliderContainer.anchoredPosition = _anchoredPosition;

            //Debug.Log("Getting-out is in progress at t = " + @"""" + _t.ToString() + @"""" + ".");

            if (_t == 0.0f)
            {
                _loopComplete = true;
            }

            yield return null;
        }

        _show = false;

        _showingButton.gameObject.SetActive(true);
    }

    public void ResumeCoroutine(bool _input)
    {
        if (_input)
        {
            _animationCoroutine = StartCoroutine(HideSliderCoroutine());
        }
        else if(_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }
    }

    void UpdateScene()
    {
        if(_slider == null || !_show)
        {
            return;
        }

        Vector3 _newSize = Vector3.one * _slider.value;

        _scale = _slider.value;

        _sceneTransform.localScale = _newSize;

        if (_sliderFillImage != null) {

            float _ratio = (_slider.value - _slider.minValue) / (_slider.maxValue - _slider.minValue);

            Color _sliderColor = _gradient.Evaluate(_ratio);

            _sliderFillImage.color = _sliderColor;
        }

        if(_sizeText == null)
        {
            return;
        }

        string _value = _slider.value.ToString("0.00");

        _sizeText.text = _value;

        if(DraggableManagerClass.GetInstance() == null)
        {
            return;
        }

        if(!DraggableManagerClass.GetInstance().GetDraggableOn())
        {
            return;
        }

        DraggableManagerClass.GetInstance().SetZConstant(this);
    }

    public float GetScale()
    {
        return _scale;
    }

    public bool GetShow()
    {
        return _show;
    }
}
