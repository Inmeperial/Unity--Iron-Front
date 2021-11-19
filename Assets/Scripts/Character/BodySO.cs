using UnityEngine;

[CreateAssetMenu(fileName = "Body", menuName = "Create Parts/ Create Body")]
public class BodySO : PartSO
{
    public float maxWeight;
    public Body prefab;
    public ItemSO item;
    [Header(" 1 is RightArm")]
    [Header("Mesh GO for arm: 0 is LeftArm,")]
    public GameObject[] armsMeshPrefab;
}
