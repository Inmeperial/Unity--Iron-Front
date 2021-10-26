using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Body", menuName = "Create Parts/ Create Body")]
public class BodySO : PartSO
{
    public Body prefab;
    [Header(" 1 is RightArm")]
    [Header("Mesh GO for arm: 0 is LeftArm,")]
    public GameObject[] armsMeshPrefab;
}
