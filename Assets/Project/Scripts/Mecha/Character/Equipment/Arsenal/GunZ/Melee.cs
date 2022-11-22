using UnityEngine;

public class Melee : Gun
{
    private void Start()
    {
        _animationEvents.Add(Preparation);
        _animationEvents.Add(Swing);
        _animationEvents.Add(Hit);
    }
    public override void SetGunData(GunSO data, Character character, string tag, string location, UnityEngine.Animator animator)
    {
        _gunType = EnumsClass.GunsType.Melee;
        base.SetGunData(data, character, tag, location, animator);
    }

    public override void GunSkill(MechaPart targetPart)
    {
       
    }

    public override void Deselect()
    {
    }

    private void Preparation() //call in animation
    {
        Debug.Log("preparation");
        AudioManager.Instance.PlaySound(_data.attackSounds[0], gameObject);
    }

    private void Swing() //call in animation
    {
        Debug.Log("swing");
        AudioManager.Instance.PlaySound(_data.attackSounds[1], gameObject);
    }

    private void Hit() //call in Animaton
    {
        Debug.Log("hit");
        EffectsController.Instance.PlayParticlesEffect(_data.attackParticles[0], _shootParticleSpawn.transform.position, _myChar.transform.forward);

        AudioManager.Instance.PlaySound(_data.attackSounds[2], gameObject);
    }
}
