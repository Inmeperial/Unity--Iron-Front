using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    public AudioMixerGroup mixer;

    [Range(0f, 1f)]
    public float volume = 1f;

    public bool loop = false;
}
