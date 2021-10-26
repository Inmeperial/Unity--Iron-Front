using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Gun", menuName = "Create Gun")]
public class GunSO : ScriptableObject
{
    public string gunName;
    public Gun prefab;
    public float maxHp;
    public int maxBullets;
    public Sprite gunImage;
    public int availableBullets;
    public int bulletsPerClick;
    public int damage;
    public int critChance;
    public float critMultiplier;
    public int hitChance;
    public int chanceToHitOtherParts;
    public int attackRange;
    public int bodyPartsSelectionQuantity;
}
