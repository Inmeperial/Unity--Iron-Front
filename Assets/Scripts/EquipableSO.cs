using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipableSO : ScriptableObject
{
    public enum EquipableType
    {
        Item,
        PassiveAbility,
        ActiveAbility
    };

    public EquipableType equipableType;
    public string equipableName;
    public int maxUses;
    public int cooldown;
}
