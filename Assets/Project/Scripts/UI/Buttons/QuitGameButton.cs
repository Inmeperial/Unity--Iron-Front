using UnityEngine;

public class QuitGameButton : CustomButton
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        if (Application.isEditor)
            return;

        OnLeftClick.AddListener(ChangeScene.Instance.Quit);
    }

    protected override void OnDestroy()
    {
        if (Application.isEditor)
            return;

        OnLeftClick.RemoveListener(ChangeScene.Instance.Quit);
    }
}
