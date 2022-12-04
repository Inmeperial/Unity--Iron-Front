using System;
using UnityEngine;

public abstract class Item : Equipable
{
    protected bool _isItemAvailable;
    public override void Initialize(Character character, EquipableSO data)
    {
        _character = character;
        _availableUses = data.maxUses;
        _icon = data.objectImage;
        _equipableType = data.equipableType;
        _isItemAvailable = true;
        OnEquipableUsed += _character.OnEquipableUsed;
    }

    public override void Select()
    {
        
    }

    public override void Deselect()
    {
        
    }

    public override void Use()
    {
        
    }

    protected void ItemUsed()
    {
        _availableUses--;
        _isItemAvailable = false;
        _character.OnMechaTurnStart += UpdateEquipableState;
    }
    public override void UpdateEquipableState()
    {
        _isItemAvailable = true;
        _character.OnMechaTurnStart -= UpdateEquipableState;
    }

    public override bool CanBeUsed()
    {
        if (_availableUses <= 0 || !_isItemAvailable)
            return false;

        return true;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
