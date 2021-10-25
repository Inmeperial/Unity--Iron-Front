using UnityEngine;

public class PartSO : ScriptableObject
{
    public string partName;
    public Sprite icon;
    public AbilitySO ability;
    public float maxHP;
    public GameObject[] meshPrefab;
}
