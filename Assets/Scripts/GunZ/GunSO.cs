﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Gun", menuName = "Create Gun")]
public class GunSO : ScriptableObject
{
    public enum GunsType
    {
        None,
        AssaultRifle,
        Melee,
        Rifle,
        Shield,
        Shotgun
    };

    [HideInInspector] public Gun prefab;
    
    //TODO: Quitar gunType
    public GunsType gunType;
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
