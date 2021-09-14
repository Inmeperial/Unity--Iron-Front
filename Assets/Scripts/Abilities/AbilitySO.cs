using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilitySO", menuName = "Create Ability")]
public class AbilitySO : EquipableSO
{
    public enum PartSlot
    {
        Body,
        Arm,
        Legs
    };
    
    public enum AbilityType
    {
        LegsOvercharge
    }

    public PartSlot partSlot;
    public AbilityType abilityType;
    public string description;
}
