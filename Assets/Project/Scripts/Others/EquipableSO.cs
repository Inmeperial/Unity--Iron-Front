using UnityEngine;

public abstract class EquipableSO : ArsenalObjectSO
{
    public enum EquipableType
     {
         Item,
         Active,
         Passive
     };

    public EquipableType equipableType;
    public int maxUses;
    public int cooldown;
    public int buttonTextFontSize;
    public SoundData sound;
}
