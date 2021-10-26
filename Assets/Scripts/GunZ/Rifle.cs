using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun
{
    public override void SetGun(GunSO data, Character character)
    {
        _gunType = GunsType.Rifle;
        _gun = "Rifle";
        base.SetGun(data, character);
    }
    
    public override void Ability()
    {
        
    }

    public override void Deselect()
    {
    }
}
