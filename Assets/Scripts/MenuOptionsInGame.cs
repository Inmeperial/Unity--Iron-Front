using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuOptionsInGame : MonoBehaviour
{
    public Text textVolume = default;

    [SerializeField] private GameObject _menuInGameObj = default;
    [SerializeField] private GameObject _optionsObj = default;
    [SerializeField] private GameObject _devObj = default;
    [SerializeField] private AudioMixer _audioMixer = default;
    
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
        _audioMixer.SetFloat("pitch", 1);
        _menuInGameObj.SetActive(false);
        CloseOptionsMenu();
        CloseDevMenu();
    }

    private void OpenAllMenu()
    {
        _audioMixer.SetFloat("pitch", 0.3f);
        _menuInGameObj.SetActive(true);
    }

    public void SetVolume(float vol) //Used on Inspector
    {
        float numPercentage = vol * 100;
        float volumeToSet = ((((numPercentage * 80) / 100) * -1) + 80) * -1;
        textVolume.text = Mathf.RoundToInt(numPercentage) + "%";
        _audioMixer.SetFloat("volume", volumeToSet);
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
