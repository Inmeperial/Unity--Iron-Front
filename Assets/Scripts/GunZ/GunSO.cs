using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Create Gun")]
public class GunSO : ScriptableObject
{
    public enum GunType
    {
        AssaultRifle,
        Melee,
        Rifle,
        Shotgun
    };

    public GunType gunType;
    public int maxBullets;
    public int availableBullets;
    public int bulletsPerClick;
    public int damage;
    public int critChance;
    public int critMultiplier;
    public int hitChance;
    public int chanceToHitOtherParts;
    public int attackRange;
    public int bodyPartsSelectionQuantity;
}
