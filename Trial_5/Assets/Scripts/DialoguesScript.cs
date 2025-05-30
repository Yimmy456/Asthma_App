using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialoguesScript : MonoBehaviour
{
    [SerializeField]
    List<AudioClipClass> _audioClips;

    [SerializeField]
    AudioSource _audioSource;

    [SerializeField]
    Animator _animator;

    //Stopwatch _stopWatch;

    System.Diagnostics.Stopwatch _stopWatch;

    System.Diagnostics.Stopwatch _subStopWatch;

    AudioClipClass _currentAudioClip;

    Coroutine _dialogueCoroutine;

    const string _talkingString  = "Talking";

    bool _isTalking = false;

    TalkingStatusEnum _overallTalkingStatus;

    TalkingStatusEnum _singleTalkingStatus;

    ExhibitionObjectScript _exhibit;

    // Start is called before the first frame update
    void Start()
    {
        if(_stopWatch == null)
        {
            _stopWatch = new System.Diagnostics.Stopwatch();
        }

        if(_subStopWatch == null)
        {
            _subStopWatch = new System.Diagnostics.Stopwatch();
        }
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

    public TalkingStatusEnum GetOverallTalkingStatus()
    {
        return _overallTalkingStatus;
    }

    public TalkingStatusEnum GetSingleTalkingStatus()
    {
        return _singleTalkingStatus;
    }

    public void PlayClip(string _input)
    {
        if (_input == "")
        {
            return;
        }

        StopCurrentDialogue();

        _dialogueCoroutine = StartCoroutine(PlayDialogue(_input, 0.0f));
    }

    public void PlayClip(string _input, float _delayInput = 0.0f)
    {
        if(_input == "")
        {
            return;
        }

        StopCurrentDialogue();

        _dialogueCoroutine = StartCoroutine(PlayDialogue(_input, _delayInput));
    }

    public void PlayClip(string _input1, string _input2, float _dialogueDelayInput = 0.0f)
    {
        if (_input1 == "" || _input2 == "")
        {
            return;
        }


        StopCurrentDialogue();

        _dialogueCoroutine = StartCoroutine(PlayDialogue(_input1, _input2, _dialogueDelayInput));
    }

    public void PlayClip(string _input1, string _input2, string _input3, float _initialDelayInput = 0.0f)
    {
        if (_input1 == "" || _input2 == "")
        {
            return;
        }


        StopCurrentDialogue();

        _dialogueCoroutine = StartCoroutine(PlayDialogue(_input1, _input2, _input3, _initialDelayInput));
    }

    IEnumerator PlayDialogue(string _input, float _initialiDelayInput = 0.0f)
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

        if(_initialiDelayInput > 0.0f)
        {
            yield return new WaitForSeconds(_initialiDelayInput);
        }

        _currentAudioClip = _clip;

        float _seconds = _clip.GetClip().length;

        _audioSource.clip = _clip.GetClip();

        _audioSource.Play();

        _isTalking = true;

        _animator.SetBool(_talkingString, true);

        Debug.Log("We are starting to talk about '" + _input + "'.");

        //_stopWatch.Restart();

        //_subStopWatch.Restart();

        _overallTalkingStatus = TalkingStatusEnum.Starting;

        _singleTalkingStatus = TalkingStatusEnum.Starting;

        for (int _i = 0; _i < 1; _i++)
        {
            yield return null;
        }

        _overallTalkingStatus = TalkingStatusEnum.Talking;

        _singleTalkingStatus = TalkingStatusEnum.Talking;

        yield return new WaitForSeconds(_seconds);

        //_stopWatch.Stop();

        //_subStopWatch.Stop();

        _overallTalkingStatus = TalkingStatusEnum.Completed;

        _singleTalkingStatus = TalkingStatusEnum.Completed;

        for (int _i = 0; _i < 1; _i++)
        {
            yield return null;
        }

        _overallTalkingStatus = TalkingStatusEnum.Not_Talking;

        _singleTalkingStatus = TalkingStatusEnum.Not_Talking;

        _currentAudioClip = null;

        _audioSource.clip = null;

        _isTalking = false;

        Debug.Log("We are stopping talking about '" + _input + "'.");

        _animator.SetBool(_talkingString, false);
    }

    IEnumerator PlayDialogue(string _input1, string _input2, float _initialDelayInput = 0.0f)
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

        if(_initialDelayInput > 0.0f)
        {
            yield return new WaitForSeconds(_initialDelayInput);
        }

        _currentAudioClip = _clip;

        float _seconds = _clip.GetClip().length;

        _audioSource.clip = _clip.GetClip();

        _audioSource.Play();

        _isTalking = true;

        _animator.SetBool(_talkingString, true);

        //_stopWatch.Restart();

        //_subStopWatch.Restart();

        _overallTalkingStatus = TalkingStatusEnum.Starting;

        _singleTalkingStatus = TalkingStatusEnum.Starting;

        for(int _f = 0; _f < 1; _f++)
        {
            yield return null;
        }

        _overallTalkingStatus = TalkingStatusEnum.Talking;

        _singleTalkingStatus = TalkingStatusEnum.Talking;

        //yield return new wait

        Debug.Log("1. We are starting to talk about '" + _input1 + "'.");

        yield return new WaitForSeconds(_seconds);

        _singleTalkingStatus = TalkingStatusEnum.Completed;

        //_subStopWatch.Stop();

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

            //_stopWatch.Stop();

            for (int _f = 0; _f < 1; _f++)
            {
                yield return null;
            }

            _overallTalkingStatus = TalkingStatusEnum.Cancelled;

            for (int _f = 0; _f < 1; _f++)
            {
                yield return null;
            }

            _overallTalkingStatus = TalkingStatusEnum.Not_Talking;

            _singleTalkingStatus = TalkingStatusEnum.Not_Talking;

            yield break;
        }

        if (_clip.GetClip() == null)
        {
            _currentAudioClip = null;

            _audioSource.clip = null;

            _animator.SetBool(_talkingString, false);

            _isTalking = false;

            Debug.LogError("2. Error 2: There is no file for the clip by the name '" + _input2 + "'.");

            //_stopWatch.Stop();

            for (int _f = 0; _f < 1; _f++)
            {
                yield return null;
            }

            _overallTalkingStatus = TalkingStatusEnum.Cancelled;

            for (int _f = 0; _f < 1; _f++)
            {
                yield return null;
            }

            _overallTalkingStatus = TalkingStatusEnum.Not_Talking;

            _singleTalkingStatus = TalkingStatusEnum.Not_Talking;

            yield break;
        }

        _currentAudioClip = _clip;

        _seconds = _clip.GetClip().length;

        _audioSource.clip = _clip.GetClip();

        _audioSource.Play();

        //_subStopWatch.Restart();

        for (int _f = 0; _f < 1; _f++)
        {
            yield return null;
        }

        _singleTalkingStatus = TalkingStatusEnum.Starting;

        for (int _f = 0; _f < 1; _f++)
        {
            yield return null;
        }

        _singleTalkingStatus = TalkingStatusEnum.Talking;

        Debug.Log("2. We are starting to talk about '" + _input2 + "'.");

        yield return new WaitForSeconds(_seconds);

        //_stopWatch.Stop();

        //_subStopWatch.Stop();


        _isTalking = false;

        _currentAudioClip = null;

        _audioSource.clip = null;

        Debug.Log("2. We are stopping talking about '" + _input2 + "'.");

        _singleTalkingStatus = TalkingStatusEnum.Completed;

        _overallTalkingStatus = TalkingStatusEnum.Completed;

        for (int _f = 0; _f < 1; _f++)
        {
            yield return null;
        }

        _overallTalkingStatus = TalkingStatusEnum.Not_Talking;

        _singleTalkingStatus = TalkingStatusEnum.Not_Talking;

        _animator.SetBool(_talkingString, false);
    }


    IEnumerator PlayDialogue(string _input1, string _input2, string _input3, float _initialDelayInput = 0.0f)
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

        if (_initialDelayInput > 0.0f)
        {
            yield return new WaitForSeconds(_initialDelayInput);
        }

        _currentAudioClip = _clip;

        float _seconds = _clip.GetClip().length;

        _audioSource.clip = _clip.GetClip();

        _audioSource.Play();

        _isTalking = true;

        _animator.SetBool(_talkingString, true);

        _overallTalkingStatus = TalkingStatusEnum.Starting;

        _singleTalkingStatus = TalkingStatusEnum.Starting;

        for (int _f = 0; _f < 1; _f++)
        {
            yield return null;
        }

        _overallTalkingStatus = TalkingStatusEnum.Talking;

        _singleTalkingStatus = TalkingStatusEnum.Talking;

        Debug.Log("1. We are starting to talk about '" + _input1 + "'.");

        yield return new WaitForSeconds(_seconds);

        _singleTalkingStatus = TalkingStatusEnum.Completed;

        Debug.Log("1. We are stopping talking about '" + _input1 + "'.");

        //Audio Clip 2

        _clip = GetClip(_input2);

        if (_clip == null)
        {
            _currentAudioClip = null;

            _audioSource.clip = null;

            _animator.SetBool(_talkingString, false);

            _isTalking = false;

            Debug.LogError("2. Error 1: There is no clip by the name '" + _input2 + "'.");

            for (int _f = 0; _f < 1; _f++)
            {
                yield return null;
            }

            _overallTalkingStatus = TalkingStatusEnum.Cancelled;

            for (int _f = 0; _f < 1; _f++)
            {
                yield return null;
            }

            _overallTalkingStatus = TalkingStatusEnum.Not_Talking;

            _singleTalkingStatus = TalkingStatusEnum.Not_Talking;

            yield break;
        }

        if (_clip.GetClip() == null)
        {
            _currentAudioClip = null;

            _audioSource.clip = null;

            _animator.SetBool(_talkingString, false);

            _isTalking = false;

            Debug.LogError("2. Error 2: There is no file for the clip by the name '" + _input2 + "'.");

            for (int _f = 0; _f < 1; _f++)
            {
                yield return null;
            }

            _overallTalkingStatus = TalkingStatusEnum.Cancelled;

            for (int _f = 0; _f < 1; _f++)
            {
                yield return null;
            }

            _overallTalkingStatus = TalkingStatusEnum.Not_Talking;

            _singleTalkingStatus = TalkingStatusEnum.Not_Talking;

            yield break;
        }

        _currentAudioClip = _clip;

        _seconds = _clip.GetClip().length;

        _audioSource.clip = _clip.GetClip();

        _audioSource.Play();

        Debug.Log("2. We are starting to talk about '" + _input2 + "'.");

        _singleTalkingStatus = TalkingStatusEnum.Starting;

        for (int _f = 0; _f < 1; _f++)
        {
            yield return null;
        }

        _singleTalkingStatus = TalkingStatusEnum.Talking;

        yield return new WaitForSeconds(_seconds);

        _singleTalkingStatus = TalkingStatusEnum.Completed;

        _clip = GetClip(_input3);

        if (_clip == null)
        {
            _currentAudioClip = null;

            _audioSource.clip = null;

            _animator.SetBool(_talkingString, false);

            _isTalking = false;

            Debug.LogError("3. Error 1: There is no clip by the name '" + _input3 + "'.");

            for (int _f = 0; _f < 1; _f++)
            {
                yield return null;
            }

            _overallTalkingStatus = TalkingStatusEnum.Cancelled;

            for (int _f = 0; _f < 1; _f++)
            {
                yield return null;
            }

            _overallTalkingStatus = TalkingStatusEnum.Not_Talking;

            _singleTalkingStatus = TalkingStatusEnum.Not_Talking;

            yield break;
        }

        if (_clip.GetClip() == null)
        {
            _currentAudioClip = null;

            _audioSource.clip = null;

            _animator.SetBool(_talkingString, false);

            _isTalking = false;

            Debug.LogError("3. Error 2: There is no file for the clip by the name '" + _input3 + "'.");

            for (int _f = 0; _f < 1; _f++)
            {
                yield return null;
            }

            _overallTalkingStatus = TalkingStatusEnum.Cancelled;

            for (int _f = 0; _f < 1; _f++)
            {
                yield return null;
            }

            _overallTalkingStatus = TalkingStatusEnum.Not_Talking;

            _singleTalkingStatus = TalkingStatusEnum.Not_Talking;

            yield break;
        }

        _currentAudioClip = _clip;

        _seconds = _clip.GetClip().length;

        _audioSource.clip = _clip.GetClip();

        _audioSource.Play();

        _singleTalkingStatus = TalkingStatusEnum.Starting;

        for (int _f = 0; _f < 1; _f++)
        {
            yield return null;
        }

        _singleTalkingStatus = TalkingStatusEnum.Talking;

        Debug.Log("3. We are starting to talk about '" + _input3 + "'.");

        yield return new WaitForSeconds(_seconds);

        _singleTalkingStatus = TalkingStatusEnum.Completed;

        _overallTalkingStatus = TalkingStatusEnum.Completed;

        for (int _f = 0; _f < 1; _f++)
        {
            yield return null;
        }

        _singleTalkingStatus = TalkingStatusEnum.Not_Talking;

        _overallTalkingStatus = TalkingStatusEnum.Not_Talking;

        _isTalking = false;

        _currentAudioClip = null;

        _audioSource.clip = null;

        Debug.Log("3. We are stopping talking about '" + _input3 + "'.");

        _animator.SetBool(_talkingString, false);
    }

    public void PlayClips(List<string> _input)
    {
        if (_input == null)
        {
            return;
        }


        StopCurrentDialogue();

        _dialogueCoroutine = StartCoroutine(PlayDialogues(_input));
    }

    IEnumerator PlayDialogues(List<string> _input)
    {
        if(_audioSource == null || _animator == null)
        {
            yield break;
        }

        AudioClipClass _audioClip;

        _isTalking = true;

        _animator.SetBool(_talkingString, true);

        bool _waiting = false;

        for (int _i = 0; _i < _input.Count; _i++)
        {
            _audioClip = GetClip(_input[_i]);

            if(_audioClip == null)
            {
                continue;
            }

            if(_audioClip.GetClip() == null || _audioClip.GetClipName() == "")
            {
                continue;
            }

            _currentAudioClip = _audioClip;

            float _seconds = _audioClip.GetClip().length;

            _audioSource.clip = _audioClip.GetClip();

            _audioSource.Play();

            _isTalking = true;

            _animator.SetBool(_talkingString, true);

            Debug.Log((_i + 1).ToString() + ". We are starting to talk about '" + _input[_i] + "'.");

            if (!_waiting)
            {
                _waiting = true;

                yield return new WaitForSeconds(_seconds);

                _waiting = false;

                Debug.Log((_i + 1).ToString() + ". We are stopping to talk about '" + _input[_i] + "'.");
            }
        }

        _isTalking = false;

        _currentAudioClip = null;

        _audioSource.clip = null;

        _animator.SetBool(_talkingString, false);
    }


    public void StopCurrentDialogue()
    {
        if(_dialogueCoroutine != null)
        {
            StopCoroutine(_dialogueCoroutine);
        }

        _audioSource.Stop();

        _animator.SetBool(_talkingString, false);

        _currentAudioClip = null;

        _dialogueCoroutine = null;

        _overallTalkingStatus = TalkingStatusEnum.Cancelled;

        _singleTalkingStatus = TalkingStatusEnum.Cancelled;

        for (int _f = 0; _f < 1; _f++) ;

        _overallTalkingStatus = TalkingStatusEnum.Not_Talking;

        _singleTalkingStatus = TalkingStatusEnum.Not_Talking;

        _isTalking = false;

    }

    public void SetDialogueCoroutine(Coroutine _input)
    {
        _dialogueCoroutine = _input;
    }

    public void PlayRandomDialogue(List<string> _clipNamesInput)
    {

        if(_audioSource == null || _clipNamesInput == null)
        {
            return;
        }

        if(_clipNamesInput.Count == 0)
        {
            return;
        }

        List<AudioClip> _selectedClips = new List<AudioClip>();

        bool _clipFound = false;

        string _currentName;

        for (int _i = 0; _i < _clipNamesInput.Count; _i++)
        {
            _currentName = _clipNamesInput[_i];

            for (int _j = 0; _j < _audioClips.Count && !_clipFound; _j++)
            {
                if (_audioClips[_j].GetClipName() == _currentName)
                {
                    _selectedClips.Add(_audioClips[_j].GetClip());

                    _clipFound = true;
                }
            }

            _clipFound = false;
        }

        if (_selectedClips.Count == 0)
        {
            return;
        }

        int _selectedIndex = Random.Range(0, _selectedClips.Count);

        AudioClip _clip = _selectedClips[_selectedIndex];

        _audioSource.clip = _clip;

        _audioSource.Play();
    }

    public void PlayRandomDialogueWithOutput(List<string> _clipNamesInput, out AudioClip _audioClipOutput)
    {
        _audioClipOutput = null;

        if (_audioSource == null || _clipNamesInput == null)
        {
            return;
        }

        if (_clipNamesInput.Count == 0)
        {
            return;
        }

        List<AudioClip> _selectedClips = new List<AudioClip>();

        bool _clipFound = false;

        string _currentName;

        for (int _i = 0; _i < _clipNamesInput.Count; _i++)
        {
            _currentName = _clipNamesInput[_i];

            for (int _j = 0; _j < _audioClips.Count && !_clipFound; _j++)
            {
                if (_audioClips[_j].GetClipName() == _currentName)
                {
                    _selectedClips.Add(_audioClips[_j].GetClip());

                    _clipFound = true;
                }
            }

            _clipFound = false;
        }

        if (_selectedClips.Count == 0)
        {
            return;
        }

        int _selectedIndex = Random.Range(0, _selectedClips.Count);

        AudioClip _clip = _selectedClips[_selectedIndex];

        _audioSource.clip = _clip;

        _audioClipOutput = _clip;

        _audioSource.Play();
    }


    public void PlayRandomDialogue(List<string> _clipNamesInput, float _initialDelayInput = 0.0f)
    {
        if (_audioSource == null || _clipNamesInput == null)
        {
            return;
        }

        if (_clipNamesInput.Count == 0)
        {
            return;
        }

        if (_initialDelayInput > 0.0f)
        {
            for (float _t = 0.0f; _t < _initialDelayInput; _t += Time.deltaTime);
        }

        List<AudioClip> _selectedClips = new List<AudioClip>();

        bool _clipFound = false;

        string _currentName;

        for (int _i = 0; _i < _clipNamesInput.Count; _i++)
        {
            _currentName = _clipNamesInput[_i];

            for (int _j = 0; _j < _audioClips.Count && !_clipFound; _j++)
            {
                if (_audioClips[_j].GetClipName() == _currentName)
                {
                    _selectedClips.Add(_audioClips[_j].GetClip());

                    _clipFound = true;
                }
            }

            _clipFound = false;
        }

        if (_selectedClips.Count == 0)
        {
            return;
        }

        int _selectedIndex = Random.Range(0, _selectedClips.Count);

        AudioClip _clip = _selectedClips[_selectedIndex];

        _audioSource.clip = _clip;

        _audioSource.Play();
    }

    public AudioClipClass GetRandomDialogue(List<string> _clipNamesInput)
    {

        if (_clipNamesInput == null)
        {
            return null;
        }

        if (_clipNamesInput.Count == 0)
        {
            return null;
        }

        List<AudioClipClass> _selectedClips = new List<AudioClipClass>();

        bool _clipFound = false;

        string _currentName;

        for (int _i = 0; _i < _clipNamesInput.Count; _i++)
        {
            _currentName = _clipNamesInput[_i];

            for (int _j = 0; _j < _audioClips.Count && !_clipFound; _j++)
            {
                if (_audioClips[_j].GetClipName() == _currentName)
                {
                    _selectedClips.Add(_audioClips[_j]);

                    _clipFound = true;
                }
            }

            _clipFound = false;
        }

        if (_selectedClips.Count == 0)
        {
            return null;
        }

        int _selectedIndex = Random.Range(0, _selectedClips.Count - 1);

        AudioClipClass _clip = _selectedClips[_selectedIndex];

        return _clip;
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
