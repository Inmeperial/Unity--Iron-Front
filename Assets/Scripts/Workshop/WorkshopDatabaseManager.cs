using System.Collections.Generic;
using UnityEngine;

public class WorkshopDatabaseManager : MonoBehaviour
{
    [SerializeField] private ObjectsDatabase _objectsDatabase;

    public static WorkshopDatabaseManager Instance;
    private void Start()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else Destroy(this);

        if (!_objectsDatabase)
        {
            LoadDatabase();
        }
    }

    public List<BodySO> GetBodies()
    {
        return _objectsDatabase.bodiesList;
    }
    
    public List<ArmSO> GetArms()
    {
        return _objectsDatabase.armsList;
    }

    public List<LegsSO> GetLegs()
    {
        return _objectsDatabase.legsList;
    }

    public List<GunSO> GetGuns()
    {
        return _objectsDatabase.gunsList;
    }

    public List<AbilitySO> GetAbilities()
    {
        return _objectsDatabase.abilitiesList;
    }

    public List<ItemSO> GetItems()
    {
        return _objectsDatabase.itemsList;
    }

    void LoadDatabase()
    {
        _objectsDatabase = Resources.Load<ObjectsDatabase>("Database/Database");
    }
}
