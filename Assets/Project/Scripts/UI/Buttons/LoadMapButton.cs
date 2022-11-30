using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMapButton : CustomButton
{
    [SerializeField] private string _mapSceneName;
    protected override void Start()
    {
        base.Start();

        StartCoroutine(ConfigureButton());
    }

    IEnumerator ConfigureButton()
    {
        yield return null;

        OnLeftClick.RemoveAllListeners();
        
        MenuOptionsInGame menuOptions = FindObjectOfType<MenuOptionsInGame>();
        
        if (menuOptions)
            OnLeftClick.AddListener(menuOptions.CloseAllMenu);
        
        OnLeftClick.AddListener(() => ChangeScene.Instance.LoadScene(_mapSceneName));
    }
}
