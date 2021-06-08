using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Legs", menuName = "Create Parts/ Create Legs")]
public class LegsSO : PartSO
{
    public int maxSteps;
    public float moveSpeed;
    public float rotationSpeed;
    public int initiative;
}
