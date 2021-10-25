using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class LoadSaveUtility
{
    private static readonly string _savePath = "/MechaEquipment.save";
    
    /// <summary>
    /// Loads the file that contains the equipment info.
    /// </summary>
    /// <param name="equipmentContainer">The equipment to overwrite with the loaded files.</param>
    /// <returns>Returns the loaded equipment if found, else returns the same equipment.</returns>
    public static MechaEquipmentContainerSO LoadEquipment()
    {
        if (!File.Exists(string.Concat(Application.dataPath, _savePath)))
        {
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(string.Concat(Application.dataPath, _savePath), FileMode.Open);
        
        //Get the string of equipments
        string save = formatter.Deserialize(file).ToString();

        //Separates the string in an array to have each equipment
        char[] separator = {'|'};
        string[] allEquipments = save.Split(separator);

        MechaEquipmentContainerSO newContainer = ScriptableObject.CreateInstance<MechaEquipmentContainerSO>();
        newContainer.name = "es el del load";
        newContainer.equipments = new List<MechaEquipmentSO>();
        //Length-1 because the last element of the array is an empty string
        for (int i = 0; i < allEquipments.Length-1; i++)
        {
            var savedEquipment = allEquipments[i];
            MechaEquipmentSO newEquipment = ScriptableObject.CreateInstance<MechaEquipmentSO>();
            //Overwrites the equipment with the saved one
            JsonUtility.FromJsonOverwrite(savedEquipment, newEquipment);
            newContainer.equipments.Add(newEquipment);
        }
        file.Close();
        return newContainer;
    }
    
    /// <summary>
    /// Save on game folder the given equipment.
    /// </summary>
    /// <param name="equipmentContainer">Equipment to save.</param>
    public static void SaveEquipment(MechaEquipmentContainerSO equipmentContainer)
    {
        Debug.Log("save");
        int amount = equipmentContainer.equipments.Count;
        
        //Array to save al equipments
        string equipmentSaves = "";

        //Converts al equipments to json/string
        for (int i = 0; i < amount; i++)
        {
            if (i != 0)
            {
                equipmentSaves += JsonUtility.ToJson(equipmentContainer.equipments[i], true);
                equipmentSaves += '|';
            }
            else
            {
                equipmentSaves = JsonUtility.ToJson(equipmentContainer.equipments[i], true);
                equipmentSaves += '|';
            }
            
        }
        

        BinaryFormatter formatter = new BinaryFormatter();
        
        FileStream file = File.Create(string.Concat(Application.dataPath, _savePath));
        
        //Serializes the string of equipments
        formatter.Serialize(file, equipmentSaves);
        file.Close();
    }
}
