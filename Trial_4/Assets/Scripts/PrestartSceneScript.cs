using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrestartSceneScript : MonoBehaviour
{
    [SerializeField]
    float _secondsToLoad = 5.0f;

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
