using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Gun
{
    public override void SetGun(GunSO data)
    {
        _gunType = GunsType.AssaultRifle;
        _gun = "AssaultRifle";
        base.SetGun(data);
    }
    public override void Ability()
    {
        
    }

    public override void Deselect()
    {
    }
}
