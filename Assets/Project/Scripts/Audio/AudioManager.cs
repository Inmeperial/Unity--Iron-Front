using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _globalMixer;
    
    public static AudioManager Instance;

    private HashSet<AudioSource> _audioSourcesToFadeOut = new HashSet<AudioSource>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    private void Update()
    {
        //if (_isFadeOutOn)
        //{
        //    FadeOutEffect();
        //}
    }

    //public void PlaySoundWithParameters(AudioClip nameSoundTarget, GameObject target)
    //{
    //    if (target.GetComponent<AudioSource>())
    //    {
    //        Sound soundVar = Array.Find(_sounds, item => item.clip.name == nameSoundTarget.name);
    //        if (soundVar == null)
    //        {
    //            soundVar = Array.Find(_environmentalSounds, item => item.clip.name == nameSoundTarget.name);
    //            if (soundVar == null)
    //            {
    //                soundVar = Array.Find(_environmentalFXSounds, item => item.clip.name == nameSoundTarget.name);
    //                if (soundVar == null)
    //                {
    //                    Debug.Log("Sound: " + nameSoundTarget.name + " not found!");
    //                    return;
    //                }
    //            }
    //        }
    //        PlayCustomSound(soundVar, target.GetComponent<AudioSource>());
    //    }
    //    else
    //    {
    //        Debug.Log("Object: " + target.gameObject.name + " needs AudioSource customized!!");
    //    }

    //}

    /// <summary>
    /// Play sound. Need sound clip and target that is going to play the sound.
    /// </summary>
    /// <param name="soundAC"></param>
    /// <param name="target"></param>
    //public void PlaySound(AudioClip soundAC, GameObject target)
    //{
    //    //Sound sound = null;

    //    //sound = Array.Find(_sounds, item => item.clip.name == soundAC.name);
    //    //if (sound != null)
    //    //{
    //    //    PlayFXSound(GetOrSetAudioSourceFromObj(target), sound);
    //    //    return;
    //    //}
    //    //sound = Array.Find(_environmentalSounds, item => item.clip.name == soundAC.name);
    //    //if (sound != null)
    //    //{
    //    //    PlayEnvironmentalSound(GetOrSetAudioSourceFromObj(target), sound);
    //    //    return;
    //    //}
    //    //sound = Array.Find(_environmentalFXSounds, item => item.clip.name == soundAC.name);
    //    //if (sound != null)
    //    //{
    //    //    PlayEnvironmentalFXSound(GetOrSetAudioSourceFromObj(target), sound);
    //    //    return;
    //    //}
    //    //if (sound == null)
    //    //{
    //    //    Debug.Log("Sound: " + soundAC.name + " , not found!");
    //    //    return;
    //    //}
    //}

    public void PlaySound(SoundData soundData, GameObject sourceObject)
    {
        if (soundData == null)
        {
            Debug.LogError("Sound Data is null or empty!", sourceObject);
            return;
        }

        if (soundData.Clip == null)
        {
            Debug.LogError("Sound Data " + soundData.name + " Audio Clip is null!", soundData);
            return;
        }

        if (!sourceObject.TryGetComponent<AudioSource>(out AudioSource audioSource))
            audioSource = sourceObject.AddComponent<AudioSource>();

        audioSource.outputAudioMixerGroup = soundData.Mixer;
        audioSource.clip = soundData.Clip;
        audioSource.volume = soundData.Volume;
        audioSource.loop = soundData.Loop;

        if (soundData.FadeOut)
            _audioSourcesToFadeOut.Add(audioSource);

        audioSource.Play();
    }

    public void StopSound(AudioClip aSource, GameObject target)
    {
        var audioSArray = target.GetComponents<AudioSource>();
        for (int i = 0; i < audioSArray.Length; i++)
        {
            if (audioSArray[i].clip == aSource)
            {
                audioSArray[i].Stop();
            }
        }
    }

    //public void StopSoundWithFadeOut(AudioClip aSource, GameObject target)
    //{
    //    var audioSArray = target.GetComponents<AudioSource>();
    //    for (int i = 0; i < audioSArray.Length; i++)
    //    {
    //        if (audioSArray[i].clip == aSource)
    //        {
    //            _aSourceActualSound = target.GetComponent<AudioSource>();
    //            _isFadeOutOn = true;
    //        }
    //    }
    //}

    //private void FadeOutEffect()
    //{
    //    _aSourceActualSound.volume -= Time.deltaTime * 2;
    //    if (_aSourceActualSound.volume <= 0)
    //    {
    //        _aSourceActualSound.Stop();
    //        _isFadeOutOn = false;
    //    }
    //}

    private IEnumerator FadeOut()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        WaitUntil waitForAudiosToFade = new WaitUntil(() => _audioSourcesToFadeOut.Count > 0);

        List<AudioSource> toRemove = new List<AudioSource>();

        while (true)
        {
            yield return waitForAudiosToFade;
            
            foreach (AudioSource audioSource in _audioSourcesToFadeOut)
            {
                audioSource.volume -= Time.deltaTime * 2;

                if (audioSource.volume > 0)
                    continue;

                toRemove.Add(audioSource);
            }

            foreach (AudioSource source in toRemove)
            {
                _audioSourcesToFadeOut.Remove(source);
            }

            toRemove.Clear();

            yield return waitForEndOfFrame;
        }
    }

    private void PlayCustomSound(Sound sound, AudioSource aSource)
    {
        //sound.source = aSource;
        //sound.source.clip = sound.clip;
        //sound.source.playOnAwake = aSource.playOnAwake;
        //sound.source.rolloffMode = aSource.rolloffMode;
        //sound.source.Play();

        //sound.source.volume = sound.volume;
        //sound.source.loop = sound.loop;
        //sound.source.playOnAwake = true;
        //sound.source.spatialBlend = 0;
        //sound.source.Play();

        // ----------

        aSource.clip = sound.clip;
        //aSource.outputAudioMixerGroup = _globalMixer;
        aSource.Play();
    }

    //private void PlayFXSound(AudioSource aSource, Sound sound)
    //{
    //    aSource.clip = sound.clip;
    //    aSource.volume = sound.volume;
    //    aSource.loop = sound.loop;
    //    aSource.playOnAwake = true;
    //    aSource.spatialBlend = 0;
    //    aSource.outputAudioMixerGroup = _globalMixer;
    //    aSource.Play();
    //}

    //private void PlayEnvironmentalSound(AudioSource aSource, Sound sound)
    //{
    //    aSource.clip = sound.clip;
    //    aSource.volume = sound.volume;
    //    aSource.loop = sound.loop;
    //    aSource.playOnAwake = true;
    //    aSource.spatialBlend = 0;
    //    aSource.outputAudioMixerGroup = _globalMixer;
    //    aSource.Play();
    //}

    //private void PlayEnvironmentalFXSound(AudioSource aSource, Sound sound)
    //{
    //    aSource.clip = sound.clip;
    //    aSource.volume = sound.volume;
    //    aSource.loop = sound.loop;
    //    aSource.spatialBlend = 1;
    //    aSource.minDistance = 5f;
    //    aSource.maxDistance = 130f;
    //    aSource.rolloffMode = AudioRolloffMode.Linear;
    //    aSource.playOnAwake = true;
    //    aSource.outputAudioMixerGroup = _globalMixer;
    //    aSource.Play();
    //}

    //private AudioSource GetOrSetAudioSourceFromObj(GameObject target)
    //{
    //    AudioSource audioSource = new AudioSource();
    //    var targetAudio = target.GetComponent<AudioSource>();
    //    if (targetAudio)
    //    {
    //        audioSource = targetAudio;
    //    }
    //    else
    //    {
    //        audioSource = target.AddComponent<AudioSource>();
    //    }
    //    return audioSource;
    //}

    //public void Pause(SoundID cSoundID)
    //{
    //    if (!channels[(int)cSoundID].isPlaying) return;

    //    channels[(int)cSoundID].Pause();
    //}


    //public void Resume(SoundID cSoundID)
    //{
    //    channels[(int)cSoundID].UnPause();
    //}

    //public void Mute(SoundID cSoundID)
    //{
    //    channels[(int)cSoundID].mute = !channels[(int)cSoundID].mute;
    //}

    // private AudioSource GetOrSetAudioSourceFromObj(GameObject target)
    // {
    //     AudioSource audioSource = new AudioSource();
    //
    //     if (target.GetComponents<AudioSource>() != null)
    //     {
    //         foreach (var item in target.GetComponents<AudioSource>())
    //         {
    //             if (!item.GetComponent<AudioSource>().isPlaying)
    //             {
    //                 audioSource = target.GetComponent<AudioSource>();
    //                 break;
    //             }
    //         }
    //     }
    //     if (audioSource == null)
    //     {
    //         audioSource = target.AddComponent<AudioSource>();
    //     }
    //     return audioSource;
    // }
}
