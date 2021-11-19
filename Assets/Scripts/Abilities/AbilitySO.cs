﻿using UnityEngine;

[CreateAssetMenu(fileName = "AbilitySO", menuName = "Create Ability")]
public class AbilitySO : EquipableSO
{
    public enum PartSlot
    {
        Body,
        Arm,
        Legs
    };
    
    //TODO: remover despues
    // public enum AbilityType
    // {
    //     LegsOvercharge,
    //     Push,
    //     Pull
    // }

    public Ability abilityPrefab;
    public PartSlot partSlot;
    //public AbilityType abilityType;
    
}
