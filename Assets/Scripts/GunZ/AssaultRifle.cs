public class AssaultRifle : Gun
{
    public override void SetGun(GunSO data, Character character)
    {
        _gunType = GunsType.AssaultRifle;
        _gun = "AssaultRifle";
        base.SetGun(data, character);
    }
    public override void Ability()
    {
        
    }

    public override void Deselect()
    {
    }
}
