using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    [SerializeField] private MechaEquipmentContainerSO _equipmentContainer;


    public MechaEquipmentSO GetEquipment(int pos)
    {
        if (pos < _equipmentContainer.equipments.Count)
            return _equipmentContainer.equipments[pos];
        
        return null;
    }
}
