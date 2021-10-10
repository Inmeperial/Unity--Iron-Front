using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Arm", menuName = "Create Parts/ Create Arm")]
public class ArmSO : PartSO
{
    [Header(" 1 is RightArm mesh")]
    [Header("Mesh for arm: 0 is LeftArm mesh,")]
    public Arm prefab;
}
