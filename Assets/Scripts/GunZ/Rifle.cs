using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun
{
    public override void SetGun(GunSO data)
    {
        _gunType = GunsType.Rifle;
        _gun = "Rifle";
        base.SetGun(data);
    }
    
    public override void Ability()
    {
        
    }

    public override void Deselect()
    {
    }
}
