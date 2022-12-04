using GameSettings;
using System.Collections;
using UnityEngine;

public class SettingsInitializer : Initializable
{
    [SerializeField] private Settings _settings;
    public override void Initialize()
    {
        StartCoroutine(LoadAndApplySettings());
    }

    private IEnumerator LoadAndApplySettings()
    {
        _settings.Initialize();

        yield return new WaitForEndOfFrame();

        _settings.ApplySettingsOnGameOpen();
    }
}
