﻿using System;

public class Item : Equipable
{
    protected bool _itemUsed;
    public override void Initialize(Character character, EquipableSO data, Location location)
    {
        _character = character;
        _availableUses = data.maxUses;
        _location = location;
    }

    public override void Select()
    {
        
    }

    public override void Deselect()
    {
        
    }

    public override void Use(Action callback = null)
    {
        
    }

    public override string GetEquipableName()
    {
        return "";
    }

    protected void ItemUsed()
    {
        _availableUses--;
        _itemUsed = true;
    }
    public override void UpdateEquipableState()
    {
        _itemUsed = false;
    }

    public override bool CanBeUsed()
    {
        if (_itemUsed)
            return false;

        if (_availableUses <= 0)
            return false;
        
        if (_availableUses > 0 && !_itemUsed)
            return true;
        return false;
    }
}
