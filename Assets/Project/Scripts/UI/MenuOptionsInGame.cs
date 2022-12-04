using GameSettings;
using UnityEngine;

public class MenuOptionsInGame : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _container;
    [SerializeField] private GameObject[] _windowsToClose;
    [SerializeField] private SettingItem[] _settingItems;

    private InputsReader _inputsReader;
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
        _inputsReader = GameManager.Instance.InputsReader;
        _inputsReader.OnMenuKeyPressed += ChangeWindowState;
    }

    private void Update()
    {
        if (_inputsReader)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            ChangeWindowState();
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

        if (!_inputsReader)
            return;

        if (_canCheckInputs)
            _inputsReader.EnableKeysCheck();

        _canCheckInputs = false;
    }

    private void OpenOptions()
    {
        _container.SetActive(true);

        if (!_inputsReader)
            return;

        _canCheckInputs = _inputsReader.CanCheckKeys;
        
        _inputsReader.DisableKeysCheck();
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
        if (!_inputsReader)
            return;

        _inputsReader.OnMenuKeyPressed -= ChangeWindowState;
    }
}
