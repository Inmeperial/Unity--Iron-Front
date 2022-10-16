using UnityEngine;

public class AudioMechaHandler : MonoBehaviour
{
    [SerializeField] private AudioClip _soundMotorStart;
    //[SerializeField] private AudioClip _soundMechaExplosion;
    [SerializeField] private AudioClip _soundWalk;
    [SerializeField] private AudioClip _soundHit;

    //public void SetPlayMechaExplosion()
    //{
    //    AudioManager.audioManagerInstance.PlaySound(_soundMechaExplosion, this.gameObject);
    //}

    public void SetPlayMotorStart() => AudioManager.audioManagerInstance.PlaySound(_soundMotorStart, this.gameObject);

    //public void SetPlayHit()
    //{
    //    AudioManager.audioManagerInstance.PlaySound(_soundHit, this.gameObject);
    //}

    //call in Animaton
    public void SetPlayWalk() => AudioManager.audioManagerInstance.PlaySound(_soundWalk, this.gameObject);

    public void SetMuteWalk() => AudioManager.audioManagerInstance.StopSoundWithFadeOut(_soundMotorStart, gameObject);

}
