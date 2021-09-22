using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixerGroup _mixer;
    [SerializeField]
    private Sound[] _sounds;
    [SerializeField]
    private Sound[] _environmentalSounds;
    [SerializeField]
    private Sound[] _environmentalFXSounds;


    public static AudioManager audioManagerInstance;
    private bool _isFadeOutOn = false;
    private AudioSource _aSourceActualSound;

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

    private void Update()
    {
        if (_isFadeOutOn)
        {
            FadeOutEffect();
        }
    }

    public void PlaySoundWithParameters(GameObject target)
    {
        if (target.GetComponent<AudioSource>())
        {
            string nameSoundTarget = target.GetComponent<SoundsEnvironmentalFX>().clip.name;
            Sound soundVar = Array.Find(_sounds, item => item.clip.name == nameSoundTarget);
            if (soundVar == null)
            {
                soundVar = Array.Find(_environmentalSounds, item => item.clip.name == nameSoundTarget);
                if (soundVar == null)
                {
                    soundVar = Array.Find(_environmentalFXSounds, item => item.clip.name == nameSoundTarget);
                    if (soundVar == null)
                    {
                        Debug.Log("Sound: " + nameSoundTarget + " not found!");
                        return;
                    }
                }
            }
            PlayCustomSound(soundVar, target.GetComponent<AudioSource>());
        }
        else
        {
            Debug.Log("Object: " + target.gameObject.name + " needs AudioSource customized!!");
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
            Debug.Log("Sound: " + soundAC.name + " not found!");
            return;
        }
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

    public void StopSoundWithFadeOut(AudioClip aSource, GameObject target)
    {
        var audioSArray = GetComponents<AudioSource>();
        for (int i = 0; i < audioSArray.Length; i++)
        {
            if (audioSArray[i].clip == aSource)
            {
                _aSourceActualSound = target.GetComponent<AudioSource>();
                _isFadeOutOn = true;
            }
        }
    }

    private void FadeOutEffect()
    {
        _aSourceActualSound.volume -= Time.deltaTime * 2;
        if (_aSourceActualSound.volume <= 0)
        {
            _aSourceActualSound.Stop();
            _isFadeOutOn = false;
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
        aSource.Play();
    }

    private void PlayFXSound(AudioSource aSource, Sound sound)
    {
        //sound.source = aSource;
        //sound.source.clip = sound.clip;
        //sound.source.volume = sound.volume;
        //sound.source.loop = sound.loop;
        //sound.source.playOnAwake = true;
        //sound.source.spatialBlend = 0;
        //sound.source.Play();

        aSource.clip = sound.clip;
        aSource.volume = sound.volume;
        aSource.loop = sound.loop;
        aSource.playOnAwake = true;
        aSource.spatialBlend = 0;
        aSource.Play();
    }

    private void PlayEnvironmentalSound(AudioSource aSource, Sound sound)
    {
        //sound.source = aSource;
        //sound.source.clip = sound.clip;
        //sound.source.volume = sound.volume;
        //sound.source.loop = sound.loop;
        //sound.source.playOnAwake = true;
        //sound.source.spatialBlend = 0;
        //sound.source.Play();

        aSource.clip = sound.clip;
        aSource.volume = sound.volume;
        aSource.loop = sound.loop;
        aSource.playOnAwake = true;
        aSource.spatialBlend = 0;
        aSource.outputAudioMixerGroup = _mixer;
        aSource.Play();
    }

    private void PlayEnvironmentalFXSound(AudioSource aSource, Sound sound)
    {
        //sound.source = aSource;
        //sound.source.clip = sound.clip;
        //sound.source.volume = sound.volume;
        //sound.source.loop = sound.loop;
        //sound.source.spatialBlend = 1;
        //sound.source.minDistance = 5f;
        //sound.source.maxDistance = 130f;
        //sound.source.rolloffMode = AudioRolloffMode.Linear;
        //sound.source.playOnAwake = true;
        //sound.source.Play();

        aSource.clip = sound.clip;
        aSource.volume = sound.volume;
        aSource.loop = sound.loop;
        aSource.spatialBlend = 1;
        aSource.minDistance = 5f;
        aSource.maxDistance = 130f;
        aSource.rolloffMode = AudioRolloffMode.Linear;
        aSource.playOnAwake = true;
        aSource.Play();
    }

    private AudioSource GetOrSetAudioSourceFromObj(GameObject target)
    {
        AudioSource audioSource = new AudioSource();
        var targetAudio = target.GetComponent<AudioSource>();
        if (targetAudio)
        {
            //Debug.Log("target tiene audiosource " + target.name);
            audioSource = targetAudio;
        }
        else
        {
            //Debug.Log("creo audiosource: " + target.name);
            audioSource = target.AddComponent<AudioSource>();
        }
        return audioSource;
    }

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
