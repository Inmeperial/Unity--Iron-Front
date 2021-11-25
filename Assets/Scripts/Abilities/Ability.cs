using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Ability : Equipable
{
    protected bool _inCooldown;
    protected int _currentCooldown;
    //Agregar nuevas al final, sino se modifican en el prefab
    public enum Abilities
    {
        CripplingShot,
        Flamethrower,
        IncendiaryAmmo,
        LegsOvercharge,
        PiercingShot,
        Push,
        SelfDestruct,
        SmokeScreen
    }
    [SerializeField] protected Abilities _ability;
    public override void Initialize(Character character, EquipableSO data, Location location)
    {
        _character = character;
        _icon = data.equipableIcon;
        _location = location;
        _equipableType = data.equipableType;
        _equipableName = data.equipableName;
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

    protected void AbilityUsed(AbilitySO data)
    {
        _inCooldown = true;
        _currentCooldown = data.cooldown;
    }

    public override void UpdateEquipableState()
    {
        _currentCooldown--;
        
        if (_currentCooldown <= 0)
            _inCooldown = false;
    }

    public override bool CanBeUsed()
    {
        return !_inCooldown;
    }

    public Abilities GetAbilityEnum()
    {
        return _ability;
    }
}
