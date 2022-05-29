public class AssaultRifle : Gun
{
    public override void SetGunData(GunSO data, Character character)
    {
        _gunType = GunsType.AssaultRifle;
        _gun = "AssaultRifle";
        base.SetGunData(data, character);
    }
    public override void Ability()
    {
        
    }

    public override void Deselect()
    {
    }
}
