using UnityEngine;
using static UnityEngine.ParticleSystem;

public class AssaultRifle : Gun
{
    private void Start()
    {
        _animationEvents.Add(PlayShootParticle);
        _animationEvents.Add(PlayFinalShootParticle);
    }

    public override void SetGunData(GunSO data, Character character, string tag, string location, UnityEngine.Animator animator)
    {
        _gunType = EnumsClass.GunsType.AssaultRifle;
        base.SetGunData(data, character, tag, location, animator);
    }
    public override void GunSkill(MechaPart targetPart)
    {
        
    }

    public override void Deselect()
    {
    }
    
    private void PlayShootParticle() //call in Animaton
    {
        //Anim keyFrame = 26.1 - 45.3 - 63.9 - 81.6 - 

        Debug.Log("play shoot particle");
        EffectsController.Instance.PlayParticlesEffect(_data.attackParticles[0], _shootParticleSpawn.transform.position, _myChar.transform.forward, out ParticleSystem particle);
        particle.transform.parent = _shootParticleSpawn.transform;
        AudioManager.Instance.PlaySound(_data.attackSounds[0], gameObject);
    }

    private void PlayFinalShootParticle() //call in Animaton
    {
        //Anim keyFrame = 26.1 - 45.3 - 63.9 - 81.6 - 
        Debug.Log("play final shoot particle");
        EffectsController.Instance.PlayParticlesEffect(_data.attackParticles[1], _shootParticleSpawn.transform.position, _myChar.transform.forward, out ParticleSystem particle);
        particle.transform.parent = _shootParticleSpawn.transform;
        AudioManager.Instance.PlaySound(_data.attackSounds[1], gameObject);
    }
}
