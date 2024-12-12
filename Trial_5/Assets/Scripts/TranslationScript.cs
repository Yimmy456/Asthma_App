using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationScript : TransformChangingScript
{
    // Start is called before the first frame update
    void Start()
    {
        SetNullValues();
    }

    // Update is called once per frame
    void Update()
    {
        DoActionFunction();
    }

    private void OnEnable()
    {
        SetNullValues();
    }

    protected override void DoActionFunction()
    {
        base.DoActionFunction();

        if (_changeTransform)
        {
            _finalTransform.Translate(_finalResult, _space);
        }
    }
}
