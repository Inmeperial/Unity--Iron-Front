using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Database", menuName = "Scriptable Objects/Equipments/Objects Database")]
public class ObjectsDatabase : ScriptableObject
{
    public List<BodySO> bodiesList;

    public List<LegsSO> legsList;

    public List<GunSO> gunsList;

    public List<AbilitySO> abilitiesList;
    
    public List<ItemSO> itemsList;
 

    public BodySO GetBodySOByID(string id)
    {
        foreach(BodySO body in bodiesList)
        {
            if (body.objectName == id)
                return body;
        }

        return null;
    }

    public LegsSO GetLegsSOByID(string id)
    {
        foreach (LegsSO legs in legsList)
        {
            if (legs.objectName == id)
                return legs;
        }

        return null;
    }

    public GunSO GetGunSOByID(string id)
    {
        foreach (GunSO gun in gunsList)
        {
            if (gun.objectName == id)
                return gun;
        }

        return null;
    }

    public AbilitySO GetAbilitySOByID(string id)
    {
        foreach (AbilitySO ability in abilitiesList)
        {
            if (ability.objectName == id)
                return ability;
        }

        return null;
    }

    public ItemSO GetItemSOByID(string id)
    {
        foreach (ItemSO item in itemsList)
        {
            if (item.objectName == id)
                return item;
        }

        return null;
    }
}
