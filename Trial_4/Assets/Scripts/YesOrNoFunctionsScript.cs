using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class YesOrNoFunctionsClass
{
    public static void ReopenCanvas(Canvas _input)
    {
        _input.gameObject.SetActive(false);
    }

    public static void GoToScene(string _input)
    {
        SceneManager.LoadScene(_input);
    }

    public static void GoToScene(int _input)
    {
        SceneManager.LoadScene(_input);
    }

    public static void SetGameToZero(GameGenericMBScript<object> _input)
    {
        if(_input == null)
        {
            return;
        }

        _input.GetGameProperties().GetMeter().SetValue(0);
    }
}
