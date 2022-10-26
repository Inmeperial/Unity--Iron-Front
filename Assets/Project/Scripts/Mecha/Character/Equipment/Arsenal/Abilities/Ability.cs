using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Ability : Equipable
{
    protected bool _inCooldown;
    protected int _currentCooldown;
    protected MechaPart _part;
    
    public override void Initialize(Character character, EquipableSO data)
    {
        _character = character;
        _icon = data.objectImage;
        _equipableType = data.equipableType;
        _equipableName = data.objectName;
        OnEquipableUsed += _character.OnEquipableUsed;
    }

    public override void Select() {
        Debug.Log("select ability");
    }

    public override void Deselect()
    {
        Debug.Log("deselect ability");
    }

    public override void Use()
    {
        Debug.Log("use ability");
    }

    public virtual void SetPart(MechaPart part) => _part = part;
    protected void AbilityUsed(AbilitySO data)
    {
        _inCooldown = true;
        _currentCooldown = data.cooldown;
        _character.OnMechaTurnStart += UpdateEquipableState;
    }

    public override void UpdateEquipableState()
    {
        _currentCooldown--;

        if (_currentCooldown > 0)
            return;

        _inCooldown = false;
        _character.OnMechaTurnStart -= UpdateEquipableState;
    }

    public override bool CanBeUsed()
    {
        return !_inCooldown && _character.CanAttack();
    }

    public int GetRemainingCooldown()
    {
        return _currentCooldown;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _character.OnMechaTurnStart -= UpdateEquipableState;
    }
}