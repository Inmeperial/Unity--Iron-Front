using UnityEngine;

public class PartSO : ScriptableObject
{
    public string partName;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Sprite icon;
    public float maxHP;
    public float weight;
    public MasterShaderScript masterShader;
    public Material bodyMaterial;
    public Material jointsMaterial;
    public Material armorMaterial;
}
