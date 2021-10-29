using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[CreateAssetMenu(fileName = "Equipment", menuName = "Create Equipment")]
public class MechaEquipmentSO : ScriptableObject
{
    [SerializeField] private ObjectsDatabase _objectsDatabase;
    public string name;
    public BodySO body;
    public GunSO leftGun;
    public GunSO rightGun;
    public LegsSO legs;

    private void OnEnable()
    {
        LoadObjectsDatabase(); 
    }
    
    void LoadObjectsDatabase()
    {
        _objectsDatabase = Resources.Load<ObjectsDatabase>("Database/Database");
    }
    
    
}
