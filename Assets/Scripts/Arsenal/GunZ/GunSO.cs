using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Create Gun")]
public class GunSO : ArsenalObjectSO
{
    public Gun prefab;
    public float maxHp;
    public int maxBullets;
    public int availableBullets;
    public int bulletsPerClick;
    public int damage;
    public int critChance;
    public float critMultiplier;
    public int hitChance;
    public int chanceToHitOtherParts;
    public int attackRange;
    public int bodyPartsSelectionQuantity;
    public float weight;
}
