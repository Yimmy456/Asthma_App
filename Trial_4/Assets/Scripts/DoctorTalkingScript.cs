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

    [SerializeField]
    AudioSource _doctorAudioSource;

    AudioClip _currentClip;

    bool _isTalking = false;

    // Start is called before the first frame update
    void Start()
    {
        if (DataPersistenceManager.GetInstance() != null)
        {
            if (_talkAtStart && _animator != null && !DataPersistenceManager.GetInstance().GetDoctorGreets())
            {
                StartTalking(_secondsToTalkAtStart);

                DataPersistenceManager.GetInstance().SetDoctorGreets(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Talk(float _secondsInput)
    {
        //_animator.SetBool(_talkingString, true);

        Debug.Log("Talking begins now...");

        yield return new WaitForSeconds(_secondsInput);

        Debug.Log("Talking ends now.");

        //_animator.SetBool(_talkingString, false);

        AbortTalking();
    }

    public void StartTalking(float _secondsInput = 5.0f)
    {
        StopTalking();

        if(_animator == null)
        {
            return;
        }

        _animator.SetBool(_talkingString, true);

        _isTalking = true;

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

    public void StartTalking(AudioClip _clipInput)
    {
        if(_clipInput == null || _animator == null || _doctorAudioSource == null)
        {
            return;
        }

        AbortTalking();

        float _duration = _clipInput.length;

        _animator.SetBool(_talkingString, true);

        _doctorAudioSource.clip = _clipInput;

        _currentClip = _clipInput;

        _doctorAudioSource.Play();

        _isTalking = true;

        _coroutine = StartCoroutine(Talk(_duration));
    }

    public void AbortTalking()
    {
        if(_coroutine != null || _isTalking)
        {
            StopCoroutine(_coroutine);

            _animator.SetBool(_talkingString, false);

            _doctorAudioSource.Stop();

            _doctorAudioSource.clip = null;

            _currentClip = null;

            _coroutine = null;

            _isTalking = false;
        }
    }
}
