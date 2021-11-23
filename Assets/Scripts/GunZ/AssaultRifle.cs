public class AssaultRifle : Gun
{
    public override void SetGun(GunSO data, Character character, Equipable.Location location)
    {
        _gunType = GunsType.AssaultRifle;
        _gun = "AssaultRifle";
        base.SetGun(data, character,location);
    }
    public override void Ability()
    {
        
    }

    public override void Deselect()
    {
    }
}
