using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPlanDropdownListScript : MonoBehaviour
{
    [SerializeField]
    Dropdown _dropdownList;

    bool _listDropped = false;

    bool _styleChanged = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckListDropped();
    }

    void CheckListDropped()
    {
        if (_dropdownList == null)
        {
            _listDropped = false;

            _styleChanged = false;

            return;
        }

        if(ActionPlanManagerScript.GetInstance() == null)
        {
            _listDropped = false;

            _styleChanged = false;

            return;
        }

        _listDropped = (_dropdownList.gameObject.GetComponent<RectTransform>().Find("Dropdown List") != null);

        if(_listDropped)
        {
            Debug.Log("The list is dropped.");
        }
        else
        {
            Debug.Log("The list is not dropped.");
        }
    }
}
