using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSceneScript : MonoBehaviour
{

    [SerializeField]
    List<MenuSceneCameraClass> _menuSceneCameras;

    [SerializeField]
    Button _pcPlayButton;

    [SerializeField]
    Button _phonePlayButton;

    // Start is called before the first frame update
    void Start()
    {
        EnableCorrectPlayButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<MenuSceneCameraClass> GetMenuSceneCameras()
    {
        return _menuSceneCameras;
    }

    public void ActivateCameraByName(string _input)
    {
        foreach (var _c in _menuSceneCameras)
        {
            if(string.Compare(_c.GetName(), _input, true) == 0)
            {
                _c.GetCamera().gameObject.SetActive(true);
            }
            else
            {
                _c.GetCamera().gameObject.SetActive(false);
            }
        }
    }

    public void ActivateCameraByIndex(int _input)
    {
        for(int _i = 0; _i < _menuSceneCameras.Count; _i++)
        {
            bool _equals = (_i == _input);

            _menuSceneCameras[_i].GetCamera().gameObject.SetActive(_equals);
        }
    }

    public void StartActionPlan()
    {
        if(ActionPlanManagerScript.GetInstance() == null)
        {
            return;
        }

        ActionPlanManagerScript.GetInstance().StartPlan();
    }

    void EnableCorrectPlayButton()
    {
        bool _bool;

        if(_pcPlayButton != null)
        {
            _bool = Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;

            _pcPlayButton.gameObject.SetActive(_bool);
        }

        if(_phonePlayButton != null)
        {
            _bool = Application.platform == RuntimePlatform.Android;

            _phonePlayButton.gameObject.SetActive(_bool);
        }
    }
}
