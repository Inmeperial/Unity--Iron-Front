using UnityEngine;

public class AudioMechaHandler : MonoBehaviour
{
    [SerializeField] private SoundData _motorStart;
    [SerializeField] private SoundData _walk;
    [SerializeField] private SoundData _hit;
    //public void SetPlayMechaExplosion()
    //{
    //    AudioManager.audioManagerInstance.PlaySound(_soundMechaExplosion, this.gameObject);
    //}

    //public void SetPlayMotorStart()
    //{
    //    AudioManager.Instance.PlaySound(_motorStart, this.gameObject);
    //}

    //public void SetPlayHit()
    //{
    //    AudioManager.audioManagerInstance.PlaySound(_soundHit, this.gameObject);
    //}

    ////call in Animaton
    //public void SetPlayWalk()
    //{
    //    AudioManager.Instance.PlaySound(_walk, this.gameObject);
    //}

    //public void SetMuteWalk()
    //{
    //    AudioManager.Instance.PlaySound(_motorStart, gameObject);
    //}
}
