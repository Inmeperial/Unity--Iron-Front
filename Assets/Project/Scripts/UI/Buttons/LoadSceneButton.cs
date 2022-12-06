using System.Collections;
using UnityEngine;
using SceneReference;

public class LoadSceneButton : CustomButton
{
    [SerializeField] private ReferenceToScene _referenceToScene;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        StartCoroutine(ConfigureButton());
    }

    private IEnumerator ConfigureButton()
    {
        yield return null;
        
        OnLeftClick.RemoveAllListeners();
        
        OnLeftClick.AddListener(() => ChangeScene.Instance.LoadScene(_referenceToScene.sceneName));
    }

    public void SetSceneReference(ReferenceToScene reference)
    {
        _referenceToScene = reference;
    }
}
