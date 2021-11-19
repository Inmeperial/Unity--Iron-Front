using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Create Equipment")]
public class MechaEquipmentSO : ScriptableObject
{
    [SerializeField] private ObjectsDatabase _objectsDatabase;
    public string name;
    public BodySO body;
    public GunSO leftGun;
    public GunSO rightGun;
    public LegsSO legs;
    //public Color bodyColor;
    //public Color legsColor;
    public ColorData bodyColor;
    public ColorData legsColor;

    private void OnEnable()
    {
        LoadObjectsDatabase(); 
    }
    
    void LoadObjectsDatabase()
    {
        _objectsDatabase = Resources.Load<ObjectsDatabase>("Database/Database");
    }

    public Color GetBodyColor()
    {
        Color color = new Color(bodyColor.red, bodyColor.green, bodyColor.blue);
        return color;
    }
    
    public Color GetLegsColor()
    {
        Color color = new Color(legsColor.red, legsColor.green, legsColor.blue);
        return color;
    }

    [Serializable]
    public struct ColorData
    {
        public float red;
        public float green;
        public float blue;
    }
}
