using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private Sound[] _sounds;
    [SerializeField]
    private Sound[] _environmentalSounds;
    [SerializeField]
    private Sound[] _environmentalFXSounds;

    public static AudioManager audioManagerInstance;

    void Awake()
    {
        if (audioManagerInstance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            audioManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Play sound. Need sound clip and target that is going to play the sound.
    /// </summary>
    /// <param name="soundAC"></param>
    /// <param name="target"></param>
    public void PlaySound(AudioClip soundAC, GameObject target)
    {
        Sound sound = null;

        sound = Array.Find(_sounds, item => item.clip.name == soundAC.name);
        if (sound != null)
        {
            PlayFXSound(GetOrSetAudioSourceFromObj(target), sound);
            return;
        }
        sound = Array.Find(_environmentalSounds, item => item.clip.name == soundAC.name);
        if (sound != null)
        {
            PlayEnvironmentalSound(GetOrSetAudioSourceFromObj(target), sound);
            return;
        }
        sound = Array.Find(_environmentalFXSounds, item => item.clip.name == soundAC.name);
        if (sound != null)
        {
            PlayEnvironmentalFXSound(GetOrSetAudioSourceFromObj(target), sound);
            return;
        }
        if (sound == null)
        {
            Debug.LogWarning("Sound: " + soundAC.name + " not found!");
            return;
        }
    }

    public void StopSound(GameObject target)
    {
        target.GetComponent<AudioSource>().Stop();
    }

    private void PlayFXSound(AudioSource aSource, Sound sound)
    {
        sound.source = aSource;
        sound.source.clip = sound.clip;
        sound.source.volume = sound.volume;
        sound.source.loop = sound.loop;
        sound.source.playOnAwake = true;
        sound.source.spatialBlend = 0;
        sound.source.Play();
    }

    private void PlayEnvironmentalSound(AudioSource aSource, Sound sound)
    {
        sound.source = aSource;
        sound.source.clip = sound.clip;
        sound.source.volume = sound.volume;
        sound.source.loop = sound.loop;
        sound.source.playOnAwake = true;
        sound.source.spatialBlend = 0;
        sound.source.Play();
    }
    
    private void PlayEnvironmentalFXSound(AudioSource aSource, Sound sound)
    {
        sound.source = aSource;
        sound.source.clip = sound.clip;
        sound.source.volume = sound.volume;
        sound.source.loop = sound.loop;
        sound.source.spatialBlend = 1;
        sound.source.minDistance = 5f;
        sound.source.maxDistance = 130f;
        sound.source.rolloffMode = AudioRolloffMode.Linear;
        sound.source.playOnAwake = true;
        sound.source.Play();
    }

    private AudioSource GetOrSetAudioSourceFromObj(GameObject target)
    {
        AudioSource audioSource = target.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource not found in:" + target.name + " adding one.");
            audioSource = target.AddComponent<AudioSource>();
        }
        return audioSource;
    }
}

//[System.Serializable]
//public class AudioRollOff
//{
//    public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;

//    public float distanceMin = 1;
//    public float distanceMax = 500;
//    public bool setCustomCurve = false;

//    public AnimationCurve distanceCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1), new Keyframe(1, 0) });


//    public void Set(AudioSource audioSource)
//    {

//        audioSource.rolloffMode = rolloffMode;
//        audioSource.minDistance = distanceMin;
//        audioSource.maxDistance = distanceMax;

//        if (setCustomCurve)
//            audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, distanceCurve);
//    }
//}