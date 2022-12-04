using SceneReference;
using System.Collections;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private Initializable[] _initializables;
    [SerializeField] private ReferenceToScene _menuSceneReference;

    private void Awake()
    {
        StartCoroutine(InitializeSystems());
    }
    
    private IEnumerator InitializeSystems()
    {
        foreach (Initializable initializable in _initializables)
        {
            initializable.Initialize();

            yield return new WaitForSeconds(initializable.InitializeDelaySeconds);
        }

        ChangeScene.Instance.LoadScene(_menuSceneReference.sceneName);
    }
}
