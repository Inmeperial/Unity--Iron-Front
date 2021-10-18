﻿using System;
using System.Collections;
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
}
