using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Gun
{
    public override void SetGun(GunSO data)
    {
        _gunType = GunsType.Melee;
        _gun = "Melee";
        base.SetGun(data);
    }
    
    public override void Ability()
    {
       
    }

    public override void Deselect()
    {
    }
}
