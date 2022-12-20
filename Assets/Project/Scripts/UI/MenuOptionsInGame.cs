using GameSettings;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;

public class MenuOptionsInGame : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _container;
    [SerializeField] private GameObject[] _windowsToClose;
    [SerializeField] private SettingItem[] _settingItems;
    
    private InputsReader _inputsReader;
    private bool _canCheckInputs;

    // PProcess Enrique 
    // [SerializeField] private AudioMixer _audioMixer = default;
    private GameObject _cameraWorldObj = default;
    private MenuPausePPPPSSettings _menuPPSettings = default;

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
        //StoptMenuPausePP();

        if (!_inputsReader)
            return;

        if (_canCheckInputs)
            _inputsReader.EnableKeysCheck();

        _canCheckInputs = false;
    }

    private void OpenOptions()
    {
        _container.SetActive(true);
        //StartMenuPausePP();

        if (!_inputsReader)
            return;

        _canCheckInputs = _inputsReader.CanCheckKeys;
        
        _inputsReader.DisableKeysCheck();
    }

    //private void StoptMenuPausePP()
    //{
    //    _menuPPSettings.enabled.value = false;
    //    _menuPPSettings._lerpPower.value = 0;
    //    //_audioMixer.SetFloat("pitch", 1);
    //}

    //private void StartMenuPausePP()
    //{
    //    if (_cameraWorldObj == null)
    //    {
    //        _cameraWorldObj = GameObject.Find("/CameraFocus/CameraWorld");
    //        _menuPPSettings = _cameraWorldObj.GetComponent<PostProcessVolume>().profile.GetSetting<MenuPausePPPPSSettings>();
    //    }
        
    //    _menuPPSettings.enabled.value = true;
    //    _menuPPSettings._lerpPower.value = 1;
    //    //_audioMixer.SetFloat("pitch", 0.3f);
    //}

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
