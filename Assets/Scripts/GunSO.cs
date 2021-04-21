using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Create Gun")]
public class GunSO : ScriptableObject
{
    public int maxBullets;
    public int availableBullets;
    public int damage;
    public int attackRange;
    public int bodyPartsSelectionQuantity;
}
