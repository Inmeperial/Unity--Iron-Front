using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class MenuOptionsInGame : MonoBehaviour
{
    public Text textVolume = default;

    [SerializeField] private GameObject _menuInGameObj = default;
    [SerializeField] private GameObject _optionsObj = default;
    [SerializeField] private GameObject _devObj = default;
    [SerializeField] private AudioMixer _audioMixer = default;
    [SerializeField] private GameObject _cameraWorldObj = default;
    private MenuPausePPPPSSettings _menuPPSettings = default;

    private void Start()
    {
        _menuPPSettings = _cameraWorldObj.GetComponent<PostProcessVolume>().profile.GetSetting<MenuPausePPPPSSettings>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_menuInGameObj.activeSelf)
            {
                CloseAllMenu();
            }
            else
            {
                OpenAllMenu();
            }
        }
    }

    public void CloseAllMenu()
    {
        _menuPPSettings.enabled.value = false;
        _menuPPSettings._lerpPower.value = 0;
        _audioMixer.SetFloat("pitch", 1);
        _menuInGameObj.SetActive(false);
        CloseOptionsMenu();
        CloseDevMenu();
    }

    private void OpenAllMenu()
    {
        _menuPPSettings.enabled.value = true;
        _menuPPSettings._lerpPower.value = 1;
        _audioMixer.SetFloat("pitch", 0.3f);
        _menuInGameObj.SetActive(true);
    }
    
    public void CloseOptionsMenu()
    {
        if (_optionsObj)
            _optionsObj.SetActive(false);
    }

    public void CloseDevMenu()
    {
        if (_devObj)
            _devObj.SetActive(false);
    }
}
