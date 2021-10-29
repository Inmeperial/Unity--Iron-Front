using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipableSO : ScriptableObject
{
    //TODO: remover despues
    public enum EquipableType
     {
         Item,
         Ability,
         ActiveAbility
     };

    public EquipableType equipableType;
    public string equipableName;
    public Sprite equipableIcon;
    public string description;
    public int maxUses;
    public int cooldown;
    

    [Header("SOLO PARA PUSH, DESP HAGO EL CUSTOM")]
    public int pushUseRange;
    public int pushDistance;

    [Space] public bool ES_PARA_DEJAR_UN_ESPACIO;
}
