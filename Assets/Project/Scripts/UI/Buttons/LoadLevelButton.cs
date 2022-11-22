using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevelButton : CustomButton
{
    [SerializeField] private string _levelName;
    // Start is called before the first frame update
    protected override void Start()
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
        
        OnLeftClick.AddListener(() => changeScene.LoadScene(_levelName));
    }
}
