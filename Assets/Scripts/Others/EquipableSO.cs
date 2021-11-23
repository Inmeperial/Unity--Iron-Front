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

}
