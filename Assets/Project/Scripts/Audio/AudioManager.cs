using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _globalMixer;
    
    public static AudioManager Instance;

    private List<AudioSource> _audioSourcesToFadeOut = new List<AudioSource>();

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
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
}
