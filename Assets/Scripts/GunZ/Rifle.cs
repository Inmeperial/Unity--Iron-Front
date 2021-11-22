public class Rifle : Gun
{
    public override void SetGun(GunSO data, Character character, Equipable.Location location)
    {
        _gunType = GunsType.Rifle;
        _gun = "Rifle";
        base.SetGun(data, character, location);
    }
    
    public override void Ability()
    {
        
    }

    public override void Deselect()
    {
    }
}
