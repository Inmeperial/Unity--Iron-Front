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

        LoadSaveUtility.LoadEquipment(_equipmentContainer);

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
            var equipment = _equipmentContainer.GetEquipment(i);
            green[i].SetEquipment(equipment);
        }
    }
}
