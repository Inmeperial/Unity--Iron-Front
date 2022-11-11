using UnityEngine.TextCore.Text;

public class AssaultRifle : Gun
{
    public override void SetGunData(GunSO data, Character character, string tag, string location)
    {
        _gunType = EnumsClass.GunsType.AssaultRifle;
        base.SetGunData(data, character, tag, location);
    }
    public override void GunSkill(MechaPart targetPart)
    {
        
    }

    public override void Deselect()
    {
    }

    public void PlayShootParticle() //call in Animaton
    {
        if (_myChar.IsDead())
            return;
        //Anim keyFrame = 26.1 - 45.3 - 63.9 - 81.6 - 

        if (_data.attackParticles.Length < 1)
            return;

        EffectsController.Instance.PlayParticlesEffect(_data.attackParticles[0], _shootParticleSpawn.transform.position, _myChar.transform.forward);
    }

    public void PlayFinalShootParticle() //call in Animaton
    {
        if (_myChar.IsDead())
            return;
        //Anim keyFrame = 26.1 - 45.3 - 63.9 - 81.6 - 


        if (_data.attackParticles.Length < 2)
            return;
        EffectsController.Instance.PlayParticlesEffect(_data.attackParticles[1], _shootParticleSpawn.transform.position, _myChar.transform.forward);
    }
}
