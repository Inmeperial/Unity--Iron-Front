using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EquipmentContainer", menuName = "Create Equipment Container")]
public class MechaEquipmentContainerSO : ScriptableObject
{
    public List<MechaEquipmentSO> equipments;
}
    