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
    /// <returns>Returns the loaded equipment if found, else returns the default equipment.</returns>
    public static MechaEquipmentContainerSO LoadEquipment()
    {
        MechaEquipmentContainerSO defaultEquipment = Resources.Load<MechaEquipmentContainerSO>("Equipment/DefaultContainer");
        if (!File.Exists(string.Concat(Application.dataPath, _savePath)))
        {
            SaveEquipment(defaultEquipment);
            return defaultEquipment;
        }
        ObjectsDatabase database = Resources.Load<ObjectsDatabase>("Database/Database");

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(string.Concat(Application.dataPath, _savePath), FileMode.Open);
        
        //Get the string of equipments
        string saveFile = formatter.Deserialize(file).ToString();

        //Separates the string in an array to have each equipment
        char[] equipmentSeparator = {'#'};
        string[] allEquipments = saveFile.Split(equipmentSeparator);

        MechaEquipmentContainerSO newContainer = ScriptableObject.CreateInstance<MechaEquipmentContainerSO>();
        newContainer.name = "es el del load";
        newContainer.equipments = new List<MechaEquipmentSO>();
        //Length-1 because the last element of the array is an empty string
        for (int i = 0; i < allEquipments.Length-1; i++)
        {
            //Gets equipment plus parts.
            string savedEquipment = allEquipments[i];
            
            char[] partSeparator = {'|'};
            
            //Separates equipment (index 0) AND parts.
            string[] allParts = savedEquipment.Split(partSeparator);
            
            MechaEquipmentSO newEquipment = ScriptableObject.CreateInstance<MechaEquipmentSO>();
            
            //BODY
            string bodyID = allParts[0];

            BodySO bodySO;

            if (!string.IsNullOrEmpty(bodyID))
                bodySO = database.GetBodySOByID(bodyID);
            else
                bodySO = defaultEquipment.GetEquipment(i).body;

            newEquipment.body = bodySO;

            //LEFT GUN
            string leftGunID = allParts[1];

            GunSO leftGunSO;

            if (!string.IsNullOrEmpty(leftGunID))
                leftGunSO = database.GetGunSOByID(leftGunID);
            else
                leftGunSO = defaultEquipment.GetEquipment(i).leftGun;

            newEquipment.leftGun = leftGunSO;

            string rightGunID = allParts[2];

            //RIGHT GUN
            GunSO rightGunSO;

            if (!string.IsNullOrEmpty(rightGunID))
                rightGunSO = database.GetGunSOByID(rightGunID);
            else
                rightGunSO = defaultEquipment.GetEquipment(i).rightGun;

            newEquipment.rightGun = rightGunSO;

            //LEGS
            string legsID = allParts[3];

            LegsSO legsSO;

            if (!string.IsNullOrEmpty(legsID))
                legsSO = database.GetLegsSOByID(legsID);
            else
                legsSO = defaultEquipment.GetEquipment(i).legs;

            newEquipment.legs = legsSO;

            //BODY ABILITY
            string bodyAbilityID = allParts[4];

            BodyAbilitySO bodyAbilitySO;
            if (!string.IsNullOrEmpty(bodyAbilityID))
                bodyAbilitySO = database.GetAbilitySOByID(bodyAbilityID) as BodyAbilitySO;

            else
                bodyAbilitySO = defaultEquipment.GetEquipment(i).bodyAbility as BodyAbilitySO;

            newEquipment.bodyAbility = bodyAbilitySO;

            //LEFT GUN ABILITY
            string leftGunAbilityID = allParts[5];

            GunAbilitySO leftGunAbilitySO;
            if (!string.IsNullOrEmpty(leftGunAbilityID))
                leftGunAbilitySO = database.GetAbilitySOByID(leftGunAbilityID) as GunAbilitySO;
            else
                leftGunAbilitySO = defaultEquipment.GetEquipment(i).leftGunAbility as GunAbilitySO;

            newEquipment.leftGunAbility = leftGunAbilitySO;

            //RIGHT GUN ABILITY
            string rightGunAbilityID = allParts[6];

            GunAbilitySO rightGunAbilitySO;
            if (!string.IsNullOrEmpty(rightGunAbilityID))
                rightGunAbilitySO = database.GetAbilitySOByID(rightGunAbilityID) as GunAbilitySO;
            else
                rightGunAbilitySO = defaultEquipment.GetEquipment(i).rightGunAbility as GunAbilitySO;

            newEquipment.rightGunAbility = rightGunAbilitySO;

            //LEGS ABILITY
            string legsAbilityID = allParts[7];

            LegsAbilitySO legsAbilitySO;
            if (!string.IsNullOrEmpty(legsAbilityID))
                legsAbilitySO = database.GetAbilitySOByID(legsAbilityID) as LegsAbilitySO;
            else
                legsAbilitySO = defaultEquipment.GetEquipment(i).legsAbility as LegsAbilitySO;

            newEquipment.legsAbility = legsAbilitySO;

            //ITEM
            string itemID = allParts[8];

            ItemSO itemSO;
            if (!string.IsNullOrEmpty(itemID))
                itemSO = database.GetItemSOByID(itemID);
            else
                itemSO = defaultEquipment.GetEquipment(i).item;

            newEquipment.item = itemSO;

            //Overwrites body color.
            newEquipment.bodyColor = JsonUtility.FromJson<MechaEquipmentSO.ColorData>(allParts[9]);
            
            //Overwrites legs color.
            newEquipment.legsColor =JsonUtility.FromJson<MechaEquipmentSO.ColorData>(allParts[10]);
            
            //Index 6 is the name of the Mecha.
            newEquipment.mechaName = allParts[11];
            
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
            string body = equipmentContainer.equipments[i].body.partName;
            body += '|';

            string bodyAbility = "";
            if (equipmentContainer.equipments[i].bodyAbility)
                bodyAbility = equipmentContainer.equipments[i].bodyAbility.equipableName;
            bodyAbility += '|';

            string item = "";            
            if (equipmentContainer.equipments[i].item)
                item = equipmentContainer.equipments[i].item.equipableName;
            item += '|';

            string leftGun = equipmentContainer.equipments[i].leftGun.gunName;
            leftGun += '|';

            string leftGunAbility = "";
            if (equipmentContainer.equipments[i].leftGunAbility)
                leftGunAbility = equipmentContainer.equipments[i].leftGunAbility.equipableName;
            leftGunAbility += '|';

            string rightGun = equipmentContainer.equipments[i].rightGun.gunName;
            rightGun += '|';

            string rightGunAbility = "";
            if (equipmentContainer.equipments[i].rightGunAbility)
                rightGunAbility = equipmentContainer.equipments[i].rightGunAbility.equipableName;
            rightGunAbility += '|';

            string legs = equipmentContainer.equipments[i].legs.partName;
            legs += '|';

            string legsAbility = "";
            if (equipmentContainer.equipments[i].legsAbility)
                legsAbility = equipmentContainer.equipments[i].legsAbility.equipableName;
            legsAbility += '|';

            string bodyColor = JsonUtility.ToJson(equipmentContainer.equipments[i].bodyColor, true);
            bodyColor += '|';

            string legsColor = JsonUtility.ToJson(equipmentContainer.equipments[i].legsColor, true);
            legsColor += '|';

            if (i == 0)
            {
                //If it's the first equipment, removes the empty space.
                equipmentSaves = body;
            }
            else
                equipmentSaves += body;

            //BODY == 0
            equipmentSaves += leftGun; // == 1
            equipmentSaves += rightGun; // == 2
            equipmentSaves += legs; // == 3

            equipmentSaves += bodyAbility; // == 4
            equipmentSaves += leftGunAbility; // == 5
            equipmentSaves += rightGunAbility; // == 6
            equipmentSaves += legsAbility; // == 7

            equipmentSaves += item; // == 8

            equipmentSaves += bodyColor; // == 9
            equipmentSaves += legsColor; // == 10

            equipmentSaves += equipmentContainer.equipments[i].mechaName; // == 11

            //Set the end of this equipment.
            equipmentSaves += '#';
        }
        

        BinaryFormatter formatter = new BinaryFormatter();
        
        FileStream file = File.Create(string.Concat(Application.dataPath, _savePath));
        
        //Serializes the string of equipments.
        formatter.Serialize(file, equipmentSaves);
        file.Close();
    }
}
