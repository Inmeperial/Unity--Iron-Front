﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Database", menuName = "ObjectsDatabase")]
public class ObjectsDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public List<BodySO> bodiesList;
    public Dictionary<BodySO, int> bodiesID = new Dictionary<BodySO, int>();
    public Dictionary<int, BodySO> bodiesSO = new Dictionary<int, BodySO>();

    public List<LegsSO> legsList;
    public Dictionary<LegsSO, int> legsID = new Dictionary<LegsSO, int>();
    public Dictionary<int, LegsSO> legsSO = new Dictionary<int, LegsSO>();

    public List<GunSO> gunsList;
    public Dictionary<GunSO, int> gunID = new Dictionary<GunSO, int>();
    public Dictionary<int, GunSO> gunSO = new Dictionary<int, GunSO>();

    public List<AbilitySO> abilitiesList;
    public Dictionary<AbilitySO, int> abilityID = new Dictionary<AbilitySO, int>();
    public Dictionary<int, AbilitySO> abilitySO = new Dictionary<int, AbilitySO>();
    
    public List<ItemSO> itemsList;
    public Dictionary<ItemSO, int> itemID = new Dictionary<ItemSO, int>();
    public Dictionary<int, ItemSO> itemSO = new Dictionary<int, ItemSO>();
    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        bodiesID = new Dictionary<BodySO, int>();
        bodiesSO = new Dictionary<int, BodySO>();

        for (int i = 0; i < bodiesList.Count; i++)
        {
            bodiesID.Add(bodiesList[i], i);
            bodiesSO.Add(i, bodiesList[i]);
        }

        legsID = new Dictionary<LegsSO, int>();
        legsSO = new Dictionary<int, LegsSO>();
        
        for (int i = 0; i < legsList.Count; i++)
        {
            legsID.Add(legsList[i], i);
            legsSO.Add(i, legsList[i]);
        }

        gunID = new Dictionary<GunSO, int>();
        
        for (int i = 0; i < gunsList.Count; i++)
        {
            gunID.Add(gunsList[i], i);
            gunSO.Add(i, gunsList[i]);
        }
        
        abilityID = new Dictionary<AbilitySO, int>();
        
        for (int i = 0; i < abilitiesList.Count; i++)
        {
            abilityID.Add(abilitiesList[i], i);
            abilitySO.Add(i, abilitiesList[i]);
        }
        
        itemID = new Dictionary<ItemSO, int>();
        
        for (int i = 0; i < itemsList.Count; i++)
        {
            itemID.Add(itemsList[i], i);
            itemSO.Add(i, itemsList[i]);
        }
    }
}
