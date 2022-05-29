public class Rifle : Gun
{
    public override void SetGunData(GunSO data, Character character)
    {
        _gunType = GunsType.Rifle;
        _gun = "Rifle";
        base.SetGunData(data, character);
    }
    
    public override void Ability()
    {
        
    }

    public override void Deselect()
    {
    }
}
