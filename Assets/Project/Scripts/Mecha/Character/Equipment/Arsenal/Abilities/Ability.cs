using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Ability : Equipable
{
    [SerializeField] protected Abilities _ability;

    protected bool _inCooldown;
    protected int _currentCooldown;
    protected MechaPart _part;

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
    
    public override void Initialize(Character character, EquipableSO data)
    {
        _character = character;
        _icon = data.objectImage;
        _equipableType = data.equipableType;
        _equipableName = data.objectName;
    }

    public override void Select() => Debug.Log("select ability");

    public override void Deselect() => Debug.Log("deselect ability");

    public override void Use(Action callback = null) => Debug.Log("use ability");

    public virtual void SetPart(MechaPart part) => _part = part;
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

    public override bool CanBeUsed() => !_inCooldown;

    public Abilities GetAbilityEnum() => _ability;
}
