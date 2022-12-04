using GameSettings;
using UnityEngine;

public class MenuOptionsInGame : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _container;
    [SerializeField] private GameObject[] _windowsToClose;
    [SerializeField] private SettingItem[] _settingItems;

    private bool _canCheckInputs;

    private void Start()
    {
        foreach (SettingItem item in _settingItems)
        {
            item.Initialize();
        }

        _container.SetActive(false);

        if (!GameManager.Instance)
            return;
        GameManager.Instance.InputsReader.OnMenuKeyPressed += ChangeWindowState;
    }

    private void ChangeWindowState()
    {
        CloseWindows();

        if (_container.activeSelf)
            CloseOptions();
        else
            OpenOptions();
    }

    private void CloseOptions()
    {
        _container.SetActive(false);

        if (_canCheckInputs)
            GameManager.Instance.InputsReader.EnableKeysCheck();

        _canCheckInputs = false;
    }

    private void OpenOptions()
    {
        _container.SetActive(true);

        _canCheckInputs = GameManager.Instance.InputsReader.CanCheckKeys;
        
        GameManager.Instance.InputsReader.DisableKeysCheck();
    }

    private void CloseWindows()
    {
        foreach (GameObject window in _windowsToClose)
        {
            window.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        if (!GameManager.Instance)
            return;
        GameManager.Instance.InputsReader.OnMenuKeyPressed -= ChangeWindowState;
    }
}
