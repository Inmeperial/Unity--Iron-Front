using UnityEngine;

[CreateAssetMenu(fileName = "Legs", menuName = "Create Parts/ Create Legs")]
public class LegsSO : PartSO
{
    public Legs prefab;
    public int maxSteps;
    public float moveSpeed;
    public float rotationSpeed;
    public int initiative;
}
