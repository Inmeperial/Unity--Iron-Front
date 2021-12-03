using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeInGame : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer = default;

    public void SetVolume(float vol) //Used on Inspector
    {
        //float numPercentage = vol * 100;
        //float volumeToSet = ((((numPercentage * 30) / 100) * -1) + 30) * -1;
        //textVolume.text = Mathf.RoundToInt(numPercentage) + "%";
        //_audioMixer.SetFloat("volume", volumeToSet);
        _audioMixer.SetFloat("volume", vol);
    }

}
