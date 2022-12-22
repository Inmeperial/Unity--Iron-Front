using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : Initializable
{
    public static ChangeScene Instance;

    public override void Initialize()
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

    private IEnumerator LoadSceneAsync(string sceneToLoad)
    {
        yield return null;

        AsyncOperation loadScreen = SceneManager.LoadSceneAsync("LoadScreen");
        yield return new WaitUntil(() => loadScreen.isDone);

        //Slider slider = FindObjectOfType<Slider>();

        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneToLoad);
        ao.allowSceneActivation = false;

        
        while (!ao.isDone)
        {
            //float progress = Mathf.Clamp01(ao.progress / 0.9f);
            //if (slider)
            //{
            //    UpdateLoadingBar(slider, progress);
            //}

            //if (ao.progress >= 0.9f)
            //{
            //    if (slider)
            //        UpdateLoadingBar(slider, progress);

            //    ao.allowSceneActivation = true;
            //}
                 

            yield return null;
        }

        Debug.Log("Scene " + sceneToLoad + " finished loading");
    }

    //void UpdateLoadingBar(Slider slider, float value)
    //{
    //    slider.value = value;
    //}
}
