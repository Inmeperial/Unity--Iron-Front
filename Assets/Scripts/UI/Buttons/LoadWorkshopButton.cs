using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWorkshopButton : CustomButton
{
    [SerializeField] private string _workshopSceneName;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        StartCoroutine(ConfigureButton());
    }

    IEnumerator ConfigureButton()
    {
        yield return null;
        var changeScene = FindObjectOfType<ChangeScene>();

        while (!changeScene)
        {
            changeScene = FindObjectOfType<ChangeScene>();
        }
        
        OnLeftClick.RemoveAllListeners();
        
        MenuOptionsInGame menuOptions = FindObjectOfType<MenuOptionsInGame>();
        
        if (menuOptions)
            OnLeftClick.AddListener(menuOptions.CloseAllMenu);
        
        OnLeftClick.AddListener(() => changeScene.LoadScene(_workshopSceneName));
    }

}
