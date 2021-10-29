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
        char[] equipmentSeparator = {'|'};
        string[] allEquipments = save.Split(equipmentSeparator);

        MechaEquipmentContainerSO newContainer = ScriptableObject.CreateInstance<MechaEquipmentContainerSO>();
        newContainer.name = "es el del load";
        newContainer.equipments = new List<MechaEquipmentSO>();
        //Length-1 because the last element of the array is an empty string
        for (int i = 0; i < allEquipments.Length-1; i++)
        {
            //Gets equipment plus parts.
            string savedEquipment = allEquipments[i];
            
            char[] partSeparator = {'_'};
            
            //Separates equipment (index 0) AND parts.
            string[] allParts = savedEquipment.Split(partSeparator);
            
            MechaEquipmentSO newEquipment = ScriptableObject.CreateInstance<MechaEquipmentSO>();

            newEquipment.body = ScriptableObject.CreateInstance<BodySO>();
            newEquipment.leftGun = ScriptableObject.CreateInstance<GunSO>();
            newEquipment.rightGun = ScriptableObject.CreateInstance<GunSO>();
            newEquipment.legs = ScriptableObject.CreateInstance<LegsSO>();
            
            //Overwrites body data with the saved one.
            JsonUtility.FromJsonOverwrite(allParts[0], newEquipment.body);
            
            //Overwrites left gun data with the saved one.
            JsonUtility.FromJsonOverwrite(allParts[1], newEquipment.leftGun);
            
            //Overwrites right gundata with the saved one.
            JsonUtility.FromJsonOverwrite(allParts[2], newEquipment.rightGun);
            
            //Overwrites legs data with the saved one.
            JsonUtility.FromJsonOverwrite(allParts[3], newEquipment.legs);
            
            //Index 4 is the name of the Mecha.
            newEquipment.name = allParts[4];
            
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
        int amount = equipmentContainer.equipments.Count;
        
        //String to save all equipments
        string equipmentSaves = "";
        
        //Converts al equipments to json/string.
        for (int i = 0; i < amount; i++)
        {
            //Gets parts data and adds a separator to read each part when loading.
            string body = JsonUtility.ToJson(equipmentContainer.equipments[i].body, true);
            body += '_';
            
            string leftGun = JsonUtility.ToJson(equipmentContainer.equipments[i].leftGun, true);
            leftGun += '_';
            
            string rightGun = JsonUtility.ToJson(equipmentContainer.equipments[i].rightGun, true);
            rightGun += '_';
            
            string legs = JsonUtility.ToJson(equipmentContainer.equipments[i].legs, true);
            legs += '_';
            if (i == 0)
            {
                //If it's the first equipment, removes the empty space.
                equipmentSaves = body;
            }
            else
            {
                equipmentSaves += body;
            }

            equipmentSaves += leftGun;
            equipmentSaves += rightGun;
            equipmentSaves += legs;
            equipmentSaves += equipmentContainer.equipments[i].name;

            //Set the end of this equipment.
            equipmentSaves += '|';
        }
        

        BinaryFormatter formatter = new BinaryFormatter();
        
        FileStream file = File.Create(string.Concat(Application.dataPath, _savePath));
        
        //Serializes the string of equipments.
        formatter.Serialize(file, equipmentSaves);
        file.Close();
    }
}
