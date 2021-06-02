using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [HideInInspector]
    public AudioSource source;

    public AudioClip clip;
    public bool loop = false;

    [Range(0f, 1f)]
    public float volume = 1f;

}
