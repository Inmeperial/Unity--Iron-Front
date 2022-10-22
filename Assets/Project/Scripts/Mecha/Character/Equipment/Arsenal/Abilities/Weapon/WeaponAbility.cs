
public class WeaponAbility : Ability
{
    protected Gun _gun;
    public override void SetPart(MechaPart part)
    {
        _gun = part as Gun;
    }
}
