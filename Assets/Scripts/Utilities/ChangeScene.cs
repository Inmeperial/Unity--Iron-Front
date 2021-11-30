using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ChangeScene : MonoBehaviour
{
    public static bool _firstTimePlaying = false;

    public static ChangeScene Instance;

    [SerializeField] private static bool _firstLoad = true;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (_firstLoad)
        {
            _firstLoad = false;

            if (SceneManager.GetActiveScene().name == "LoadScreen")
            {
                Debug.Log("load menu");
                LoadScene("Menu"); 
            }
        }
        else _firstLoad = false;
    }
    // public void MenuLoadScene()
    // {
    //     if (!_firstTimePlaying)
    //     {
    //         _firstTimePlaying = true;
    //         LoadLevel1();
    //     }
    //     else
    //         LoadMap();
    // }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level 1 NUEVO");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void LoadLevel3()
    {
        
        SceneManager.LoadScene("Level 3");
    }

    public void LoadWorkShop()
    {
        SceneManager.LoadScene("TallerScene");
    }

    public void LoadMap()
    {
        SceneManager.LoadScene("Map");
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Debug.Log("Bye");
        Application.Quit();
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(LoadSceneAsync(scene));
    }
    IEnumerator LoadSceneAsync(string sceneToLoad)
    {
        yield return null;

        AsyncOperation loadScreen = SceneManager.LoadSceneAsync("LoadScreen");
        Debug.Log("load screen");
        yield return new WaitUntil(() => loadScreen.isDone);
        
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneToLoad);
        ao.allowSceneActivation = false;

        float progress = 0;
        while (!ao.isDone)
        {
            progress = ao.progress / 1;

            if (ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
                 
                //fill amount100
                ao.allowSceneActivation = true;
            }
                 

            yield return null;
        }
    } 
}
