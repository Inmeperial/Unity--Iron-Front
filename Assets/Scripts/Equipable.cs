using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipable : MonoBehaviour
{

    protected EquipableSO.EquipableType _equipableType;
    protected int _availableUses;
    protected int _currentCooldown;
    protected Character _character;
    protected EquipmentButton _button;
    public abstract void Initialize(Character character, EquipableSO data);

    public abstract void Select();

    public abstract void Deselect();

    public abstract void Use(Action callback = null);

    public EquipableSO.EquipableType GetEquipableType()
    {
        return _equipableType;
    }

    public abstract string GetEquipableName();

    public void SetButton(EquipmentButton button)
    {
        _button = button;
    }
}
