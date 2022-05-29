public class Melee : Gun
{
    public override void SetGunData(GunSO data, Character character)
    {
        _gunType = GunsType.Melee;
        _gun = "Melee";
        base.SetGunData(data, character);
    }
    
    public override void Ability()
    {
       
    }

    public override void Deselect()
    {
    }
}
