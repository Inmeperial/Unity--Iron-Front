using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaEquipmentSO : ScriptableObject
{
    public string name;
    public BodySO body;
    public ArmSO leftArm;
    public GunSO leftGun;
    public ArmSO rightArm;
    public GunSO rightGun;
    public LegsSO legs;
}
