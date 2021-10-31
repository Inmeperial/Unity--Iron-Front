using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuOptionsInGame : MonoBehaviour
{
    public GameObject menuInGameObj = default;
    public GameObject optionsObj = default;
    public GameObject devObj = default;
    public Text textVolume = default;
    public AudioMixer audioMixer = default;

    // Start is called before the first frame update
    void Start()
    {
        //menuInGameObj = GameObject.Find("CanvasMenuInGame").transform.GetChild(0).transform.GetChild(0).gameObject;
        //optionsObj = menuInGameObj.transform.Find("PopUpOptionsPanel").gameObject;
        //devObj = menuInGameObj.transform.Find("PopUpDevPanel").gameObject;
        //textVolume = optionsObj.transform.Find("GeneralVolume").Find("TextVolumenTotal").gameObject.GetComponent<Text>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuInGameObj.activeSelf)
            {
                audioMixer.SetFloat("pitch", 1);
                menuInGameObj.SetActive(false);
                CloseOptionsMenu();
                CloseDevMenu();
            }
            else
            {
                audioMixer.SetFloat("pitch", 0.3f);
                menuInGameObj.SetActive(true);
            }
        }
    }

    public void SetVolume(float vol)
    {
        float numPercentage = vol * 100;
        float volumeToSet = ((((numPercentage * 80) / 100) * -1) + 80) * -1;
        textVolume.text = Mathf.RoundToInt(numPercentage) + "%";
        audioMixer.SetFloat("volume", volumeToSet);
    }

    public void OpenOptionsMenu()
    {
        optionsObj.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        optionsObj.SetActive(false);
    }

    public void OpenDevMenu()
    {
        devObj.SetActive(true);
    }

    public void CloseDevMenu()
    {
        devObj.SetActive(false);
    }
}
