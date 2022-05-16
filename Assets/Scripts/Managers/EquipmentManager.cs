using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] private MechaEquipmentContainerSO _equipmentContainer;
    public void ManualAwake()
    {
        Character[] chars = FindObjectsOfType<Character>();

        MechaEquipmentContainerSO loadedEquipment = LoadSaveUtility.LoadEquipment();
        MechaEquipmentContainerSO equipmentToUse = null;

        if (loadedEquipment == null)
        {
            equipmentToUse = _equipmentContainer;
            // equipmentToUse = ScriptableObject.Instantiate(_equipmentContainer);
            // for (int i = 0; i < _equipmentContainer.equipments.Count; i++)
            // {
            //     equipmentToUse.equipments[i] = ScriptableObject.Instantiate(_equipmentContainer.equipments[i]);
            //     _equipmentContainer = equipmentToUse;
            // }
        }
        else
        {
            //_equipmentContainer = loadedEquipment;
            equipmentToUse = loadedEquipment;
        }
        
        List<Character> green = new List<Character>();
        foreach (Character character in chars)
        {
            if (character.GetUnitTeam() == EnumsClass.Team.Green)
            {
                green.Add(character);
            }
        }
        
        for (int i = 0; i < green.Count; i++)
        {
            MechaEquipmentSO equipment = equipmentToUse.GetEquipment(i);
            green[i].SetEquipment(equipment);
        }
    }
}
