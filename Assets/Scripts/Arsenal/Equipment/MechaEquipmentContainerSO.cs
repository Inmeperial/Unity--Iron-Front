using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EquipmentContainer", menuName = "Create Equipment Container")]
public class MechaEquipmentContainerSO : ScriptableObject
{
    public List<MechaEquipmentSO> equipments;
    
    public MechaEquipmentSO GetEquipment(int pos)
    {
        if (pos < equipments.Count)
            return equipments[pos];
        
        return null;
    }
}
    