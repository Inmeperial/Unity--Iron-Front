public class Melee : Gun
{
    public override void SetGun(GunSO data, Character character, Equipable.Location location)
    {
        _gunType = GunsType.Melee;
        _gun = "Melee";
        base.SetGun(data, character, location);
    }
    
    public override void Ability()
    {
       
    }

    public override void Deselect()
    {
    }
}
