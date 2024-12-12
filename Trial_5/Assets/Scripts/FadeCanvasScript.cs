using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCanvasScript : MonoBehaviour
{
    [SerializeField]
    Image _panel;

    [SerializeField]
    float _fadeSpeed = 1.0f;

    [SerializeField]
    bool _startFadeOut;

    // Start is called before the first frame update
    void Start()
    {
        if(_startFadeOut)
        {
            StartCoroutine(FadeOut());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FadeOut()
    {
        while(_panel.color.a > 0.0f)
        {
            //yield return null;

            Color _c = _panel.color;

            _c.a = _c.a - (_fadeSpeed * Time.deltaTime);

            if(_c.a <= 0.0f)
            {
                _c.a = 0.0f;
            }

            _panel.color = _c;

            Debug.Log("Alpha is " + _c.a + ".");

            yield return null;
        }

        gameObject.SetActive(false);
    }

    public Image GetPanel()
    {
        return _panel;
    }
}
