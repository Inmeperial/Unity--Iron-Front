using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMechaHandler : MonoBehaviour
{
    [SerializeField] private AudioClip _soundMotorStart;
    [SerializeField] private AudioClip _soundWalk;
    [SerializeField] private AudioClip _soundHit;
    public void SetPlayMotorStart()
    {
        AudioManager.audioManagerInstance.PlaySound(_soundMotorStart, this.gameObject);
    }

    public void SetPlayHit()
    {
        AudioManager.audioManagerInstance.PlaySound(_soundHit, this.gameObject);
    }

    public void SetPlayWalk()
    {
        AudioManager.audioManagerInstance.PlaySound(_soundWalk, this.gameObject);
    }

    public void SetMuteWalk()
    {
        AudioManager.audioManagerInstance.StopSoundWithFadeOut(_soundMotorStart, gameObject);
    }
    
}
