using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorScript : MonoBehaviour
{
    [SerializeField]
    Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerVar(string _input)
    {
        if(_animator == null)
        {
            return;
        }

        _animator.SetTrigger(_input);
    }

    public void SetTrigger()
    {
        _animator.SetTrigger("Intro Begin");
    }
}
