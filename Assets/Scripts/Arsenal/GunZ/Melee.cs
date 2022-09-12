public class Melee : Gun
{
    public override void SetGunData(GunSO data, Character character, string tag, string location)
    {
        _gunType = EnumsClass.GunsType.Melee;
        base.SetGunData(data, character, tag, location);
    }

    public override void GunSkill(MechaPart targetPart)
    {
       
    }

    public override void Deselect()
    {
    }

    protected override void PlayLeftSideAttackAnimation() => _animationMechaHandler.SetIsHammerAttackLeftAnimatorTrue();

    protected override void PlayRightSideAttackAnimation() => _animationMechaHandler.SetIsHammerAttackRightAnimatorTrue();
}
