using UnityEngine;

public class PartSO : ArsenalObjectSO
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public float maxHP;
    public float weight;
    public MasterShaderScript masterShader;
    public Material bodyMaterial;
    public Material jointsMaterial;
    public Material armorMaterial;
    public SoundData damageSound;
    public SoundData destroySound;
}
