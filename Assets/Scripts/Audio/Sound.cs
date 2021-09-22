using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [HideInInspector]
    public AudioSource source;

    [Range(0f, 1f)]
    public float volume = 1f;

    public AudioClip clip;
    public bool loop = false;
    public AudioMixerGroup mixer;
}
