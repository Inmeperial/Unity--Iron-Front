using UnityEngine;

public class Rifle : Gun
{
    private void Start()
    {
        _animationEvents.Add(PlayShootParticle);
    }
    public override void SetGunData(GunSO data, Character character, string tag, string location, UnityEngine.Animator animator)
    {
        _gunType = EnumsClass.GunsType.Rifle;
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
        EffectsController.Instance.PlayParticlesEffect(_data.attackParticles[0], _shootParticleSpawn.transform.position, _myChar.transform.forward, out ParticleSystem particle);
        particle.transform.parent = _shootParticleSpawn.transform;
        AudioManager.Instance.PlaySound(_data.attackSounds[0], gameObject);
    }
}
