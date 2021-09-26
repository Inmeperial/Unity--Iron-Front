using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuOptionsInGame : MonoBehaviour
{
    public AudioMixer audioMixer = default;
    public GameObject canvasObj = default;
    private GameObject _optionsObj = default;
    private GameObject _devObj = default;
    private GameObject _menuInGameObj = default;
    private Text _textVolume = default;

    // Start is called before the first frame update
    void Start()
    {
        _menuInGameObj = canvasObj.transform.Find("MenuInGame").gameObject;
        _optionsObj = _menuInGameObj.transform.Find("PopUpOptionsPanel").gameObject;
        _devObj = _menuInGameObj.transform.Find("PopUpDevPanel").gameObject;
        _textVolume = _optionsObj.transform.Find("GeneralVolume").Find("TextVolumenTotal").gameObject.GetComponent<Text>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_menuInGameObj.activeSelf)
            {
                audioMixer.SetFloat("pitch", 1);
                _menuInGameObj.SetActive(false);
                CloseOptionsMenu();
                CloseDevMenu();
            }
            else
            {
                audioMixer.SetFloat("pitch", 0.3f);
                _menuInGameObj.SetActive(true);
            }
        }
    }

    public void SetVolume(float vol)
    {
        float numPercentage = vol * 100;
        float volumeToSet = ((((numPercentage * 80) / 100) * -1) + 80) * -1;
        _textVolume.text = Mathf.RoundToInt(numPercentage) + "%";
        audioMixer.SetFloat("volume", volumeToSet);
    }

    public void OpenOptionsMenu()
    {
        _optionsObj.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        _optionsObj.SetActive(false);
    }

    public void OpenDevMenu()
    {
        _devObj.SetActive(true);
    }

    public void CloseDevMenu()
    {
        _devObj.SetActive(false);
    }
}
