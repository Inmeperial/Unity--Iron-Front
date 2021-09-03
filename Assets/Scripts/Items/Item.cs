using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public class Item
{
    public ItemSO itemData;

    protected int _usesAmount;

    protected Character _character;

    protected delegate void _delegate();

    public virtual void SelectItem()
    {
        
    }

    public virtual void DeselectItem()
    {
        
    }

    public virtual void Use(Action callback = null)
    {
        
    }

    public string GetItemName()
    {
        return itemData.itemName;
    }

    public int GetItemDamage()
    {
        return itemData.damage;
    }

    public int GetItemAoE()
    {
        return itemData.areaOfEffect;
    }

    public int GetItemDuration()
    {
        return itemData.duration;;
    }

    public int GetItemRange()
    {
        return itemData.useRange;
    }

    public int GetItemUses()
    {
        return _usesAmount;
    }

    protected void SetItem()
    {
        _usesAmount = itemData.usesAmount;
    }

    protected virtual void UpdateUses()
    {
        _usesAmount--;
    }
}
