using UnityEngine.Audio;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Data", menuName = "Scriptable Objects/Sound Data")]
public class SoundData : ScriptableObject
{
    [Header("Data")]
    [SerializeField] private AudioClip _sound;
    [SerializeField] private AudioMixerGroup _mixerGroup;

    [Header("Configs")]
    [Range(0f,1f)]
    [SerializeField] private float _volume;
    [SerializeField] private bool _loop;
    [SerializeField] private bool _fadeOut;
    public AudioClip Clip => _sound;
    public AudioMixerGroup Mixer => _mixerGroup;
    public float Volume => _volume;
    public bool Loop => _loop;
    public bool FadeOut => _fadeOut;
}
