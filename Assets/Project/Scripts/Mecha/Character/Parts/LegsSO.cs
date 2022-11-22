using UnityEngine;

[CreateAssetMenu(fileName = "Legs", menuName = "Scriptable Objects/Parts/Legs")]
public class LegsSO : PartSO
{
    public int maxSteps;
    public float moveSpeed;
    public float rotationSpeed;
    public int initiative;
}
