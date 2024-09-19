using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCenterBound : MonoBehaviour
{
    [SerializeField]
    Collider _collider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_collider != null)
        {
            Vector3 _bound = _collider.bounds.center;

            Debug.Log("The center is equal to " + @"""" + _bound.ToString() + @"""" + ".");
        }
    }
}
