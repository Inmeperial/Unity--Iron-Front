using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsEnvironmentalRadio : MonoBehaviour
{
    public AudioClip[] soundsEnvironmentalRadio;
    public AudioClip[] soundsStatic;
    public AudioSource _audioSource = default;
    //public AudioSource _audioSourceStatic = default;
    private int _numTrack = 0;
    private bool _isStatickPlaying = false;

    void Start()
    {
        this.transform.position = new Vector3(66.5f, 0, 66.5f);
        _audioSource = this.GetComponent<AudioSource>();
        PlayRadioClip();
    }
    void Update()
    {
        if (!_audioSource.isPlaying)
        {
            PlayRadioClip();
        }
    }
    private void PlayRadioClip()
    {
        if (_isStatickPlaying)
        {
            AudioManager.audioManagerInstance.PlaySound(soundsEnvironmentalRadio[_numTrack], this.gameObject);
            _isStatickPlaying = false;
            if (_numTrack >= soundsEnvironmentalRadio.Length)
            {
                _numTrack = 0;
            }
            else
            {
                _numTrack++;
            }
        }
        else
        {
            AudioManager.audioManagerInstance.PlaySound(SetRandomSoundStatic(), this.gameObject);
            _isStatickPlaying = true;
        }
    }
    private AudioClip SetRandomSoundStatic()
    {
        int num = UnityEngine.Random.Range(0, soundsStatic.Length);
        return soundsStatic[num];
    }
}

#region radioWithStaticRandom
/*
    public bool playWithStaticRandom = false;
    public int timesToTryRandomRadioStaticTotal = 0;
    private int _timesToTryRandomRadioStatic = 0;
    public AudioClip[] soundsEnvironmentalRadio;
    public AudioClip[] soundsStatic;
    private float _timerToChangeWithStatic = 0;
    private float _timerToChangeWithStaticTotal = 0;
    private int _numTrack = 0;
    private AudioSource _audioSource = default;


        //1) Get First from soundsEnvironmentalRadio.
        //2) Get Lenght from clip and divide that in _timerToChangeWithStaticTotal.
        //3) Play clip.
        //4) If IsChanceToChange == true
        //    * play static random sound.
        //    * when that ends play next clip in soundsEnvironmentalRadio.
        //5) If IsChanceToChange == false
        //    * try again when the time comes from _timerToChangeWithStatic and minus 1 on trys.
 

    void Start()
    {
        this.transform.position = new Vector3(66.5f, 0, 66.5f);
        _audioSource = this.GetComponent<AudioSource>();
        PlayRadioClip();
    }

    void Update()
    {
        SetFlowSounds();
    }

    private void SetFlowSounds()
    {
        if (playWithStaticRandom)
        {
            if (_timesToTryRandomRadioStatic >= 0)
            {
                _timerToChangeWithStatic -= Time.deltaTime;
                if (_timerToChangeWithStatic <= 0)
                {
                    if (IsChanceToChange())
                    {
                        AudioManager.audioManagerInstance.PlaySound(SetRandomSoundStatic(), this.gameObject);
                    }
                    _timerToChangeWithStatic = _timerToChangeWithStaticTotal;
                    _timesToTryRandomRadioStatic--;
                }
            }
            else
            {
                _timesToTryRandomRadioStatic = timesToTryRandomRadioStaticTotal;
                _numTrack++;
                PlayRadioClip();
            }
        }
        else
        {
            if (!_audioSource.isPlaying)
            {

            }
        }
    }

    private void PlayRadioClip()
    {
        for (int i = 0; i < soundsEnvironmentalRadio.Length; i++)
        {
            if (i == _numTrack)
            {
                SetTimerToChange(soundsEnvironmentalRadio[i].length);
                AudioManager.audioManagerInstance.PlaySound(soundsEnvironmentalRadio[i], this.gameObject);
                if (_numTrack >= soundsEnvironmentalRadio.Length - 1)
                {
                    _numTrack = 0;
                }
                else
                {
                    _numTrack++;
                }
            }
        }
    }

    private void SetTimerToChange(float num)
    {
        _timerToChangeWithStaticTotal = num / timesToTryRandomRadioStaticTotal;
        _timerToChangeWithStatic = _timerToChangeWithStaticTotal;
    }

    private bool IsChanceToChange()
    {
        int maxChance = 3;
        float num = UnityEngine.Random.Range(0f, maxChance);
        bool result = false;
        if (num == maxChance)
        {
            result = true;
        }
        return result;
    }

    private void SetClipWithRadioStatic()
    {

    }

    private AudioClip SetRandomSoundStatic()
    {
        int num = UnityEngine.Random.Range(0, soundsStatic.Length);
        return soundsStatic[num];
    }

 */
#endregion
