using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
[CreateAssetMenu(fileName = "Equipment", menuName = "Create Equipment")]
public class MechaEquipmentSO : ScriptableObject, ISerializationCallbackReceiver
{
    private string _path;
    public ObjectsDatabase objectsDatabase;
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
        var part = objectsDatabase.bodiesSO[partID];
        body = part;
    }
    
    public void AddArm(int partID, bool isLeft)
    {
        var part = objectsDatabase.armsSO[partID];
        if (isLeft)
        {
            Debug.Log("add l arm");
            leftArm = part;
        }

        else
        {
            Debug.Log("add r arm");
            rightArm = part;
        }
    }

    public void AddLegs(int partID)
    {
        Debug.Log("add legs");
        var part = objectsDatabase.legsSO[partID];
        legs = part;
    }

    public void AddGun(int gunID, bool isLeft)
    {
        Debug.Log("add gun");
        var gun = objectsDatabase.gunSO[gunID];
        if (isLeft)
            leftGun = gun;
        else rightGun = gun;
    }

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, _path));
        formatter.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, _path)))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, _path), FileMode.Open);
            JsonUtility.FromJsonOverwrite(formatter.Deserialize(file).ToString(), this);
            file.Close();
        }
    }
    
    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {

    }
}
