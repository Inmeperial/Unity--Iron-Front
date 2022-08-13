using UnityEngine;


public class AbilitySO : EquipableSO
{
    public enum PartSlot
    {
        Body,
        Arm,
        Legs
    };

    public Ability abilityPrefab;
    public PartSlot partSlot;
}
