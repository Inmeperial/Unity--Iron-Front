using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameButton : CustomButton
{
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

        //Cambiar si al cerrar hay que hacer algo mas.
        OnLeftClick.AddListener(changeScene.Quit);
    }
}
