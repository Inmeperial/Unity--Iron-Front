public class Melee : Gun
{
    public override void SetGun(GunSO data, Character character)
    {
        _gunType = GunsType.Melee;
        _gun = "Melee";
        base.SetGun(data, character);
    }
    
    public override void Ability()
    {
       
    }

    public override void Deselect()
    {
    }
}
