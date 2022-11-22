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
        ChangeScene changeScene = FindObjectOfType<ChangeScene>();

        WaitForSeconds wait = new WaitForSeconds(0.5f);

        while (!changeScene)
        {
            changeScene = FindObjectOfType<ChangeScene>();

            yield return wait;
        }

        OnLeftClick.RemoveAllListeners();
        
        MenuOptionsInGame menuOptions = FindObjectOfType<MenuOptionsInGame>();
        
        if (menuOptions)
            OnLeftClick.AddListener(menuOptions.CloseAllMenu);
        
        OnLeftClick.AddListener(() => changeScene.LoadScene(_mapSceneName));
    }
}
