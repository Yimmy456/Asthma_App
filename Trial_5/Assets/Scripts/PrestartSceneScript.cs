using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PrestartSceneScript : MonoBehaviour
{
    [SerializeField]
    float _secondsToLoad = 5.0f;

    [SerializeField]
    Image _logo;

    //SerializeField]
    string _sceneName;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(_secondsToLoad);

        _sceneName = CheckPersistenceManager() ? "First Use Scene" : "Menu Scene";

        SceneManager.LoadScene(_sceneName);
    }

    IEnumerator ImageColorC()
    {
        if(_logo == null)
        {
            yield break;
        }

        Color _c = Color.white;

        _c.a = 0.0f;

        for(float _a = 0.0f; _a < 1.0f; _a += (Time.deltaTime * 2))
        {
            if(_a >= 1.0f)
            {
                _a = 1.0f;
            }

            _c.a = _a;

            _logo.color = _c;

            yield return null;
        }

        yield return new WaitForSeconds(4.0f);

        for (float _a = 1.0f; _a > 0.0f; _a -= (Time.deltaTime * 2))
        {
            if (_a <= 0.0f)
            {
                _a = 0.0f;
            }

            _c.a = _a;

            _logo.color = _c;

            yield return null;
        }
    }

    bool CheckApplicationManager()
    {
        if(ApplicationManagerScript.GetInstance() == null)
        {
            return false;
        }

        return !ApplicationManagerScript.GetInstance().GetFirstUseComplete();
    }

    bool CheckPersistenceManager()
    {
        if(DataPersistenceManager.GetInstance() == null)
        {
            return false;
        }

        return !DataPersistenceManager.GetInstance().GetGameData()._userTypeSelected;
    }

    IEnumerator LoadBadges()
    {
        yield return new WaitForSeconds(1);

        if(BadgesManagerScript.GetInstance() == null || DataPersistenceManager.GetInstance() == null)
        {
            yield break;
        }

        foreach(BadgeScript _b in BadgesManagerScript.GetInstance().GetBadges())
        {

        }
    }
}
