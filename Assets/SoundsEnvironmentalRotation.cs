using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsEnvironmentalRotation : MonoBehaviour
{
    public AudioClip[] soundsEnvironmentalFxArray;
    private float _durationSound = 0;

    void Start()
    {
        this.transform.position = new Vector3(66.5f, 0, 66.5f);
    }

    void Update()
    {
        PlaySoundRandomPos();
    }

    private void PlaySoundRandomPos()
    {
        _durationSound -= Time.deltaTime;
        if (_durationSound <= 0)
        {
            AudioClip clip = ChangeRandomSound();
            AudioManager.audioManagerInstance.PlaySound(clip, this.gameObject);
            _durationSound = clip.length;
        }
    }

    private AudioClip ChangeRandomSound()
    {
        int num = UnityEngine.Random.Range(0, soundsEnvironmentalFxArray.Length);
        return soundsEnvironmentalFxArray[num];
    }
}
