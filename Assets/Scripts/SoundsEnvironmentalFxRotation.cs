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
        El stop necesita una fade out, se hace bajandole el volumen, cuando llega a 0, que se frene, bajarlo en 1 segundo.
		En el audioManager, un play mas que sea de FX estaticos, para cosas como motores en casas.
        Poner sliders para audios y tambien un boton de mute de cada uno => dejar para lo ultimo
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