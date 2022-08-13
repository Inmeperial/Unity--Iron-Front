public class Rifle : Gun
{
    public override void SetGunData(GunSO data, Character character, string tag, string location)
    {
        _gunType = GunsType.Rifle;
        _gun = "Rifle";
        base.SetGunData(data, character, tag, location);
    }
    
    public override void Ability()
    {
        
    }

    public override void Deselect()
    {
    }
}
