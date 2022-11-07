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
    protected override void PlayLeftSideAttackAnimation()
    {
        _animationMechaHandler.SetIsSniperAttackLeftAnimatorTrue();
    }

    protected override void PlayRightSideAttackAnimation()
    {
        _animationMechaHandler.SetIsSniperAttackRightAnimatorTrue();
    }    
}
