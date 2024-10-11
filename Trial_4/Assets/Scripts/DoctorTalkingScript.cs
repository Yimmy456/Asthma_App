using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorTalkingScript : MonoBehaviour, PlayDialogueInterface
{
    [SerializeField]
    Animator _animator;

    Coroutine _coroutine;

    const string _talkingString = "Talking";

    [SerializeField]
    AudioSource _doctorAudioSource;

    AudioClip _currentClip;

    [Header("Variables for the start.")]
    [SerializeField]
    bool _talkAtStart = true;

    [SerializeField]
    float _secondsToTalkAtStart = 10.0f;

    [SerializeField]
    AudioClip _welcomingClip;

    bool _isTalking = false;

    [SerializeField]
    DialoguesScript _dialogues;

    // Start is called before the first frame update
    void Start()
    {
        if (DataPersistenceManager.GetInstance() != null)
        {
            if (_talkAtStart && _animator != null && !DataPersistenceManager.GetInstance().GetDoctorGreets())
            {
                //StartTalking();

                _dialogues.PlayClip("Welcoming");

                DataPersistenceManager.GetInstance().SetDoctorGreets(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*

    IEnumerator WelcomingTalk()
    {
        if(_doctorAudioSource == null || _welcomingClip == null)
        {
            yield break;
        }

        yield return new WaitForSeconds(0.2f);

        float _seconds = _welcomingClip.length;

        _isTalking = true;

        Debug.Log("Talking begins now...");

        _doctorAudioSource.clip = _welcomingClip;

        _currentClip = _welcomingClip;

        _doctorAudioSource.Play();

        yield return new WaitForSeconds(_seconds);

        _isTalking = false;

        Debug.Log("Talking ends now.");

        AbortTalking();
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

    */

    public void StartTalking(float _secondsInput = 5.0f)
    {
        StopTalking();

        if(_animator == null)
        {
            return;
        }

        _animator.SetBool(_talkingString, true);

        _isTalking = true;

        //_coroutine = StartCoroutine(Talk(_secondsInput));
    }

    public void StartTalking()
    {
        StopTalking();

        if (_animator == null)
        {
            return;
        }

        _animator.SetBool(_talkingString, true);

        _isTalking = true;

        //_coroutine = StartCoroutine(WelcomingTalk());
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

        //_coroutine = StartCoroutine(Talk(_duration));
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

    public void IPlayDialogue()
    {

    }
}
