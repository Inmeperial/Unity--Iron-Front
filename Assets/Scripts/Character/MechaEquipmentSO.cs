using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
[CreateAssetMenu(fileName = "Equipment", menuName = "Create Equipment")]
public class MechaEquipmentSO : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private ObjectsDatabase _objectsDatabase;
    public string name;
    public BodySO body;
    public ArmSO leftArm;
    public GunSO leftGun;
    public ArmSO rightArm;
    public GunSO rightGun;
    public LegsSO legs;

    public void AddBody(int partID)
    {
        Debug.Log("add body");
        var part = _objectsDatabase.bodiesSO[partID];
        body = part;
    }
    
    public void AddArm(int partID, bool isLeft)
    {
        var part = _objectsDatabase.armsSO[partID];
        if (isLeft)
        {
            leftArm = part;
        }

        else
        {
            rightArm = part;
        }
    }

    public void AddLegs(int partID)
    {
        var part = _objectsDatabase.legsSO[partID];
        legs = part;
    }

    public void AddGun(int gunID, bool isLeft)
    {
        var gun = _objectsDatabase.gunSO[gunID];
        if (isLeft)
            leftGun = gun;
        else rightGun = gun;
    }

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter formatter = new BinaryFormatter();

        string path = string.Concat("/", name, ".save");
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, path));
        formatter.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        string path = string.Concat("/", name, ".save");
        if (!File.Exists(string.Concat(Application.persistentDataPath, path))) return;
        
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(string.Concat(Application.persistentDataPath, path), FileMode.Open);
        JsonUtility.FromJsonOverwrite(formatter.Deserialize(file).ToString(), this);
        file.Close();
    }
    
    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {

    }

    private void OnEnable()
    {
        LoadObjectsDatabase(); 
    }

    void LoadObjectsDatabase()
    {
        _objectsDatabase = Resources.Load<ObjectsDatabase>("Database/Database");
    }
}
