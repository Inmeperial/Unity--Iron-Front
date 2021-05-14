using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Legs", menuName = "Create Parts/ Create Legs")]
public class LegsSO : ScriptableObject
{
    public int legsMaxHp;
    public int maxSteps;
    public float moveSpeed;
    public float rotationSpeed;
}
