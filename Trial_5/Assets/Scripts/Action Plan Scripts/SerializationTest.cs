using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using TMPro;

public class SerializationTest : MonoBehaviour
{
    [SerializeReference]
    List<A> _list = new List<A>();

    [ContextMenu("Add Integer")]
    void AddI()
    {
        _list.Add(new B());
    }

    [ContextMenu("Add Decimal")]
    void AddD()
    {
        _list.Add(new C());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual int GetIntegerValue()
    {
        return 0;
    }
}

[Serializable]
public abstract class A
{
    [SerializeField]
    string _name;

    protected VariableTypeForAPEnum _variableType;

    public string GetName()
    {
        return _name;
    }

    public VariableTypeForAPEnum GetVariableType()
    {
        return _variableType;
    }

    public virtual System.Type GetAnswer()
    {
        return Type.GetType("");
    }

    public virtual int GetI()
    {
        return 0;
    }
}

[Serializable]
public class A<T> : A
{
    [SerializeField]
    protected T _value;

    public T GetValue()
    {
        return _value;
    }
}

[Serializable]
public class B : A<int>
{
    public B() : base() { _variableType = VariableTypeForAPEnum.Integer; }

    public override int GetI()
    {
        return _value;
    }
}

[Serializable]
public class C : A<float>
{
    public C() : base() { _variableType = VariableTypeForAPEnum.Decimal; }
}
