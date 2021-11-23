using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Ability : Equipable
{
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
    }
    public string AbilityStatus()
    {
        return "";
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
        return "";
    }

    public Abilities GetAbilityEnum()
    {
        return _ability;
    }
}
