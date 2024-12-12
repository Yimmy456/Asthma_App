using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : TransformChangingScript
{

    // Start is called before the first frame update
    void Start()
    {
        SetNullValues();
    }

    private void OnEnable()
    {
        SetNullValues();
    }

    // Update is called once per frame
    void Update()
    {
        DoActionFunction();
    }

    protected override void DoActionFunction()
    {
        base.DoActionFunction();

        if (_changeTransform)
        {
            _finalTransform.Rotate(_finalResult, _space);
        }
    }

    public Quaternion GetQuaternionRotation()
    {
        return Quaternion.Euler(_finalResult);
    }

    public Quaternion GetQuaternionRotationWithOutDeltaTime()
    {
        return Quaternion.Euler(_finalResultWithoutDeltaTime);
    }
}
