using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorTalkingScript : MonoBehaviour
{
    [SerializeField]
    Animator _animator;

    Coroutine _coroutine;

    const string _talkingString = "Talking";

    [Header("Variables for the start.")]
    [SerializeField]
    bool _talkAtStart = true;

    [SerializeField]
    float _secondsToTalkAtStart = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        if(_talkAtStart && _animator != null)
        {
            _coroutine = StartCoroutine(Talk(_secondsToTalkAtStart));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Talk(float _secondsInput)
    {
        _animator.SetBool(_talkingString, true);

        Debug.Log("Talking begins now...");

        yield return new WaitForSeconds(_secondsInput);

        Debug.Log("Talking ends now.");

        _animator.SetBool(_talkingString, false);
    }

    public void StartTalking(float _secondsInput = 5.0f)
    {
        StopTalking();

        if(_animator == null)
        {
            return;
        }

        _coroutine = StartCoroutine(Talk(_secondsInput));
    }

    public Coroutine GetCoroutine()
    {
        return _coroutine;
    }

    public void StopTalking()
    {
        if(_coroutine == null || _animator == null)
        {
            return;
        }

        StopCoroutine(_coroutine);

        _animator.SetBool(_talkingString, false);
    }
}
