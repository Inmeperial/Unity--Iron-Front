public class Rifle : Gun
{
    public override void SetGunData(GunSO data, Character character, string tag, string location)
    {
        _gunType = EnumsClass.GunsType.Rifle;
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

        if (_data.attackParticles.Length < 1)
            return;

        EffectsController.Instance.PlayParticlesEffect(_data.attackParticles[0], _shootParticleSpawn.transform.position, _myChar.transform.forward);
    }
}
