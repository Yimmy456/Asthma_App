using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhibitionHandlerScript : MonoBehaviour
{
    static ExhibitionHandlerScript _instance;

    [SerializeField]
    ExhibitionScript _exhibition;

    [SerializeField]
    DialoguesScript _dialogues;

    [SerializeField]
    Animator _animator;

    [SerializeField]
    AudioSource _audioSource;

    Coroutine _coroutine;

    AudioClip _audioClip;

    ExhibitionListItemClass _currentItem;

    // Start is called before the first frame update
    void Start()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(_instance.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ExhibitionHandlerScript GetInstance()
    {
        return _instance;
    }

    public DialoguesScript GetDialogues()
    {
        return _dialogues;
    }

    public Coroutine GetCoroutine()
    {
        return _coroutine;
    }

    public void PlayDialogue(string _input)
    {
        if(_exhibition == null || _dialogues == null || _audioSource == null || _animator == null)
        {
            return;
        }

        ExhibitionListItemClass _item = _exhibition.GetExhibitItemByID(_input);

        if (_item == null)
        {
            return;
        }

        AudioClipClass _clipClass = _dialogues.GetClip(_item.GetAudioClip2());

        if(_clipClass == null)
        {
            return;
        }

        if(_clipClass.GetClip() == null)
        {
            return;
        }

        _audioClip = _clipClass.GetClip();

        _currentItem = _item;

        StopCurrentDialogue();

        _dialogues.StopCurrentDialogue();

        _coroutine = StartCoroutine(PlayDialogueCoroutine());

        _dialogues.SetDialogueCoroutine(_coroutine);
    }

    public void StopCurrentDialogue()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        if(_animator != null)
        {
            _animator.SetBool("Talking", false);
        }

        if(_audioSource != null)
        {
            _audioSource.Stop();

            _audioClip = null;
        }
    }

    IEnumerator PlayDialogueCoroutine()
    {
        float _seconds = _audioClip.length;

        _audioSource.clip = _audioClip;

        _animator.SetBool("Talking", true);

        _audioSource.Play();

        yield return new WaitForSeconds(_seconds);

        _animator.SetBool("Talking", false);

        _audioClip = null;


        if (!_currentItem.GetDisplayExplained())
        {
            _exhibition.GetCurrentGroup().GetGroupCompletionMeter().AddToValue(1);

            _currentItem.SetDisplayExplained(true);
        }


        if(_exhibition.GetCurrentGroup().GetGroupCompletionMeter().GetPercentage() == 100.0f && !_exhibition.GetCurrentGroup().GetGroupComplete())
        {
            yield return new WaitForSeconds(2.0f);

            _exhibition.GetCurrentGroup().SetGroupComplete(true);

            _exhibition.RewardBadge();
        }
    }
}
