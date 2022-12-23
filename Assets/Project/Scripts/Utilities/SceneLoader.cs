using SceneReference;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private FadePostProcessController _fadeController;
    [SerializeField] private ReferenceToScene _sceneReference;


    public void SubscribeToFade()
    {
        Debug.Log("Subscribe to fade: " + _sceneReference.sceneName);
        _fadeController.OnTransitionFinished += LoadScene;
    }

    public void LoadScene()
    {
        Debug.Log("Load scene: " + _sceneReference.sceneName);
        ChangeScene.Instance.LoadScene(_sceneReference.sceneName);
    }

    private void OnDestroy()
    {
        _fadeController.OnTransitionFinished -= LoadScene;
    }
}
