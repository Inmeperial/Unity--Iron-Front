using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsEnvironmentalFxRotation : MonoBehaviour
{
    public float minWaitTimeBetweenSounds = 0f;
    public float maxWaitTimeBetweenSounds = 6f;
    public AudioClip[] soundsEnvironmentalFxArray;
    private float _durationSound = 1;

    /*
        TO-DO
        Ver de normalizar los audios en el vegas.
     */



    void Start()
    {
        ChangeRandomPosition();
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
            ChangeRandomPosition();
            _durationSound = clip.length + UnityEngine.Random.Range(minWaitTimeBetweenSounds, maxWaitTimeBetweenSounds);
        }
    }

    private AudioClip ChangeRandomSound()
    {
        int num = UnityEngine.Random.Range(0, soundsEnvironmentalFxArray.Length);
        return soundsEnvironmentalFxArray[num];
    }

    private void ChangeRandomPosition()
    {
        this.transform.position = new Vector3(UnityEngine.Random.Range(0f, 133), 0, UnityEngine.Random.Range(0f, 133));
    }
}