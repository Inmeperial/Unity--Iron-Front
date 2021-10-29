using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] private MechaEquipmentContainerSO _equipmentContainer;
    public void ManualAwake()
    {
        var chars = FindObjectsOfType<Character>();

        var loadedEquipment = LoadSaveUtility.LoadEquipment();
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
        foreach (var c in chars)
        {
            if (c.GetUnitTeam() == EnumsClass.Team.Green)
            {
                green.Add(c);
            }
        }
        
        for (int i = 0; i < green.Count; i++)
        {
            var equipment = equipmentToUse.GetEquipment(i);
            green[i].SetEquipment(equipment);
        }
    }
}
