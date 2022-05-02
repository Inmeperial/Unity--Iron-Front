using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Create Equipment")]
public class MechaEquipmentSO : ScriptableObject
{
    public string mechaName;
    public BodySO body;
    public BodyAbilitySO bodyAbility;
    public ItemSO item;
    public GunSO leftGun;
    public WeaponAbilitySO leftGunAbility;
    public GunSO rightGun;
    public WeaponAbilitySO rightGunAbility;
    public LegsSO legs;
    public LegsAbilitySO legsAbility;
    //public Color bodyColor;
    //public Color legsColor;
    public ColorData bodyColor;
    public ColorData legsColor;

    public Color GetBodyColor()
    {
        Color color = new Color(bodyColor.red, bodyColor.green, bodyColor.blue, 1);
        return color;
    }
    
    public Color GetLegsColor()
    {
        Color color = new Color(legsColor.red, legsColor.green, legsColor.blue, 1);
        return color;
    }

    [Serializable]
    public struct ColorData
    {
        public float red;
        public float green;
        public float blue;
    }
}
