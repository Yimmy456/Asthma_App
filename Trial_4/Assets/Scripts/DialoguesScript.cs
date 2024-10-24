using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguesScript : MonoBehaviour
{
    [SerializeField]
    List<AudioClipClass> _audioClips;

    [SerializeField]
    AudioSource _audioSource;

    [SerializeField]
    Animator _animator;

    AudioClipClass _currentAudioClip;

    Coroutine _dialogueCoroutine;

    const string _talkingString  = "Talking";

    bool _isTalking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<AudioClipClass> GetAudioClips()
    {
        return _audioClips;
    }

    public AudioClipClass GetCurrentAudioClip()
    {
        return _currentAudioClip;
    }

    public bool GetIsTalking()
    {
        return _isTalking;
    }

    public AudioClipClass GetClip(string _input)
    {
        foreach(AudioClipClass _ac in _audioClips)
        {
            if(_ac.GetClipName() == _input)
            {
                return _ac;
            }
        }

        return null;
    }

    public AudioSource GetAudioSource()
    {
        return _audioSource;
    }

    public void PlayClip(string _input)
    {
        if(_input == "")
        {
            return;
        }

        StopCurrentDialogue();

        _dialogueCoroutine = StartCoroutine(PlayDialogue(_input));
    }

    public void PlayClip(string _input1, string _input2)
    {
        if (_input1 == "" || _input2 == "")
        {
            return;
        }


        StopCurrentDialogue();

        _dialogueCoroutine = StartCoroutine(PlayDialogue(_input1, _input2));
    }

    IEnumerator PlayDialogue(string _input)
    {
        AudioClipClass _clip = GetClip(_input);

        if(_clip == null || _audioSource == null || _animator == null)
        {
            yield break;
        }

        if(_clip.GetClip() == null)
        {
            yield break;
        }

        _currentAudioClip = _clip;

        float _seconds = _clip.GetClip().length;

        _audioSource.clip = _clip.GetClip();

        _audioSource.Play();

        _isTalking = true;

        _animator.SetBool(_talkingString, true);

        Debug.Log("We are starting to talk about '" + _input + "'.");

        yield return new WaitForSeconds(_seconds);

        _currentAudioClip = null;

        _audioSource.clip = null;

        _isTalking = false;

        Debug.Log("We are stopping talking about '" + _input + "'.");

        _animator.SetBool(_talkingString, false);
    }

    IEnumerator PlayDialogue(string _input1, string _input2)
    {
        //Audio Clip 1

        AudioClipClass _clip = GetClip(_input1);

        if (_clip == null || _audioSource == null || _animator == null)
        {
            Debug.LogError("1. Error 1: Either there is no clip by the name '" + _input1 + "', or there is no audio source or animator.");

            yield break;
        }

        if (_clip.GetClip() == null)
        {
            Debug.LogError("1. Error 2: There is no file for the clip by the name '" + _input1 + "'.");

            yield break;
        }

        _currentAudioClip = _clip;

        float _seconds = _clip.GetClip().length;

        _audioSource.clip = _clip.GetClip();

        _audioSource.Play();

        _isTalking = true;

        _animator.SetBool(_talkingString, true);

        Debug.Log("1. We are starting to talk about '" + _input1 + "'.");

        yield return new WaitForSeconds(_seconds);

        Debug.Log("1. We are stopping talking about '" + _input1 + "'.");

        //Audio Clip 2

        _clip = GetClip(_input2);

        if(_clip == null)
        {
            _currentAudioClip = null;

            _audioSource.clip = null;

            _animator.SetBool(_talkingString, false);

            _isTalking = false;

            Debug.LogError("2. Error 1: There is no clip by the name '" + _input2 + "'.");

            yield break;
        }

        if (_clip.GetClip() == null)
        {
            _currentAudioClip = null;

            _audioSource.clip = null;

            _animator.SetBool(_talkingString, false);

            _isTalking = false;

            Debug.LogError("2. Error 2: There is no file for the clip by the name '" + _input2 + "'.");

            yield break;
        }

        _currentAudioClip = _clip;

        _seconds = _clip.GetClip().length;

        _audioSource.clip = _clip.GetClip();

        _audioSource.Play();

        Debug.Log("2. We are starting to talk about '" + _input2 + "'.");

        yield return new WaitForSeconds(_seconds);

        _isTalking = false;

        _currentAudioClip = null;

        _audioSource.clip = null;

        Debug.Log("2. We are stopping talking about '" + _input2 + "'.");

        _animator.SetBool(_talkingString, false);
    }

    public void StopCurrentDialogue()
    {
        if(_dialogueCoroutine != null)
        {
            StopCoroutine(_dialogueCoroutine);

            _audioSource.Stop();

            _animator.SetBool(_talkingString, false);

            _currentAudioClip = null;

            _dialogueCoroutine = null;

            _isTalking = false;
        }
    }
}


[System.Serializable]
public class AudioClipClass
{
    [SerializeField]
    protected AudioClip _clip;

    [SerializeField]
    protected string _clipName;

    public string GetClipName()
    {
        return _clipName;
    }

    public AudioClip GetClip()
    {
        return _clip;
    }
}
