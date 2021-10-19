using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Create ItemSO")]
public class ItemSO : EquipableSO
{
    public enum ItemType
    {
        Grenade
    }

    public Item itemPrefab;
    public ItemType itemType;
    public int damage;
    public int areaOfEffect;

    public int duration;

    public int useRange;
}
