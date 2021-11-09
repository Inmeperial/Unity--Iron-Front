﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public abstract class Equipable : MonoBehaviour
{

    //TODO: remover despues
    protected EquipableSO.EquipableType _equipableType;
    protected int _availableUses;
    protected int _currentCooldown;
    protected Character _character;
    protected EquipmentButton _button;
    protected Sprite _icon;
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

    public Sprite GetIcon()
    {
        return _icon;
    }
}
