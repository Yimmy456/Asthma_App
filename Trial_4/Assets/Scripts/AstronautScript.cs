using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AstronautScript : MonoBehaviour
{
    [SerializeField]
    GameObject _defaultModel;

    [SerializeField]
    GameObject _inhalerModel;

    [SerializeField]
    Animator _inhalerAnimator;

    [SerializeField]
    bool _usingInhaler;

    [SerializeField]
    bool _triggerAnimation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TriggerInhalerAnimation();

        CheckUsingInhaler();
    }

    void CheckUsingInhaler()
    {
        if(!_usingInhaler)
        {
            return;
        }

        /*bool _bool = _inhalerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Inhaler");

        if(!_bool)
        {
            _defaultModel.SetActive(true);

            _inhalerModel.SetActive(false);

            _usingInhaler = false;
        }*/
    }

    public void TriggerInhalerAnimation()
    {
        if (_inhalerModel == null || _inhalerAnimator == null || !_triggerAnimation)
        {
            _usingInhaler = false;

            return;
        }

        if(_usingInhaler)
        {
            return;
        }

        _defaultModel.SetActive(false);

        _inhalerModel.SetActive(true);

        _inhalerAnimator.SetTrigger("Trigger Animation");

        _usingInhaler = true;

        _triggerAnimation = false;
    }

    public void AbordInhalerAnimation()
    {
        if(_inhalerAnimator == null)
        {
            return;
        }

        _inhalerAnimator.SetTrigger("Abort Animation");
    }

}
