using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlideshowScript : CanvasScript
{
    [SerializeField]
    Button _previousButton;

    [SerializeField]
    Button _nextButton;

    [SerializeField]
    Button _confirmButton;

    [SerializeField]
    List<RectTransform> _slides;

    [SerializeField]
    Slider _slider;

    int _currentSlideIndex = -1;

    RectTransform _currentSlide;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Button GetPreviousButton()
    {
        return _previousButton;
    }

    public Button GetNextButton()
    {
        return _nextButton;
    }

    public Button GetConfirmButton()
    {
        return _confirmButton;
    }

    public List<RectTransform> GetSlides()
    {
        return _slides;
    }

    public int GetCurrentSlideIndex()
    {
        return _currentSlideIndex;
    }

    public Slider GetSlider()
    {
        return _slider;
    }

    public RectTransform GetCurrentSlide()
    {
        return _currentSlide;
    }

    public void GoToNextSlide()
    {
        _currentSlide.gameObject.SetActive(false);

        _currentSlideIndex++;

        _currentSlide = _slides[_currentSlideIndex];

        _currentSlide.gameObject.SetActive(true);

        if(_slider != null)
        {
            _slider.value = _currentSlideIndex + 1;
        }

        if(_currentSlideIndex == (_slides.Count - 1))
        {
            _nextButton.gameObject.SetActive(false);

            _confirmButton.gameObject.SetActive(true);
        }
        else
        {
            _nextButton.gameObject.SetActive(true);

            _confirmButton.gameObject.SetActive(false);
        }

        _previousButton.gameObject.SetActive(true);

        _quitButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(100.0f, 330.0f);
    }

    public void GoToPreviousSlide()
    {
        _currentSlide.gameObject.SetActive(false);

        _currentSlideIndex--;

        _currentSlide = _slides[_currentSlideIndex];

        _currentSlide.gameObject.SetActive(true);

        if (_slider != null)
        {
            _slider.value = _currentSlideIndex + 1;
        }

        if(_currentSlideIndex == 0)
        {
            _previousButton.gameObject.SetActive(false);

            _quitButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(100.0f, 100.0f);
        }
        else
        {
            _previousButton.gameObject.SetActive(true);

            _quitButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(100.0f, 330.0f);
        }

        if (_currentSlideIndex == (_slides.Count - 1))
        {
            _nextButton.gameObject.SetActive(false);

            _confirmButton.gameObject.SetActive(true);
        }
        else
        {
            _nextButton.gameObject.SetActive(true);

            _confirmButton.gameObject.SetActive(false);
        }
    }

    public void StartSlideshow()
    {
        if (_currentSlide != null)
        {
            _currentSlide.gameObject.SetActive(false);
        }

        _currentSlideIndex = 0;

        _currentSlide = _slides[_currentSlideIndex];

        _currentSlide.gameObject.SetActive(true);

        if (_slider != null)
        {
            _slider.value = _currentSlideIndex + 1;
        }

        _previousButton.gameObject.SetActive(false);
        
        _quitButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(100.0f, 100.0f);

        if (_currentSlideIndex == (_slides.Count - 1))
        {
            _nextButton.gameObject.SetActive(false);

            _confirmButton.gameObject.SetActive(true);
        }
        else
        {
            _nextButton.gameObject.SetActive(true);

            _confirmButton.gameObject.SetActive(false);
        }
    }

    public void EndSlideshow()
    {
        if (_currentSlide != null)
        {
            _currentSlide.gameObject.SetActive(false);

            _currentSlide = null;
        }

        if (_slider != null)
        {
            _slider.value = _slider.minValue;
        }

        _currentSlideIndex = -1;
    }
}
