using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWorkshopButton : CustomButton
{
    [SerializeField] private string _workshopSceneName;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        StartCoroutine(ConfigureButton());
    }

    IEnumerator ConfigureButton()
    {
        yield return null;
        
        OnLeftClick.RemoveAllListeners();
        
        OnLeftClick.AddListener(() => ChangeScene.Instance.LoadScene(_workshopSceneName));
    }

}
