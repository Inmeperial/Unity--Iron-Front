using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Create ItemSO")]
public class ItemSO : EquipableSO
{
    //TODO: Remover despues
    // public enum ItemType
    // {
    //     Grenade
    // }

    public Item itemPrefab;
    //public ItemType itemType;
    public int damage;
    public int areaOfEffect;

    public int duration;

    public int useRange;
}
