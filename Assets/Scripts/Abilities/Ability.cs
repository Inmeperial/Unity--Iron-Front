using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : Equipable
{
    protected AbilitySO _abilityData;
    protected AbilitySO.PartSlot _partSlot;

    public override void Initialize(Character character, EquipableSO data)
    {
        _character = character;
        _abilityData = data as AbilitySO;
        _partSlot = _abilityData.partSlot;
        //TODO: remover despues
        _equipableType = _abilityData.equipableType;
    }
    public string AbilityStatus()
    {
        switch (_equipableType)
        {
            case EquipableSO.EquipableType.PassiveAbility:
                return _availableUses > 0 ? "Enabled" : "Disabled";
            case EquipableSO.EquipableType.ActiveAbility:
                if (InCooldown())
                {
                    return "In Cooldown. Remaining: " + _currentCooldown;
                }
        
                return "x" + _availableUses;
        }

        return "EMPTY";
    }

    protected virtual bool InCooldown()
    {
        return false;
    }

    

    public override void Select()
    {
        Debug.Log("select ability");
    }

    public override void Deselect()
    {
        Debug.Log("deselect ability");
    }

    public override void Use(Action callback = null)
    {
        Debug.Log("use ability");
    }

    public override string GetEquipableName()
    {
        return _abilityData.equipableName;
    }
}
