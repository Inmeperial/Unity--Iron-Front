using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMapButton : CustomButton
{
    [SerializeField] private string _mapSceneName;
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
        
        OnLeftClick.AddListener(() => changeScene.LoadScene(_mapSceneName));
    }
}
