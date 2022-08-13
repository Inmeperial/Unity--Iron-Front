public class Melee : Gun
{
    public override void SetGunData(GunSO data, Character character, string tag, string location)
    {
        _gunType = GunsType.Melee;
        _gun = "Melee";
        base.SetGunData(data, character, tag, location);
    }
    
    public override void Ability()
    {
       
    }

    public override void Deselect()
    {
    }
}
