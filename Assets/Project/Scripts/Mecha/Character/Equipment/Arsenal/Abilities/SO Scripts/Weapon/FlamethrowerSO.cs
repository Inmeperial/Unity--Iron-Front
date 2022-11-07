using UnityEngine;

[CreateAssetMenu(fileName = "Flamethrower", menuName = "Scriptable Objects/Abilities/Weapons/Flamethrower")]
public class FlamethrowerSO : GunAbilitySO
{
    public int damage;
    public float range;
    public float angle;
    public LayerMask gridMask;
    public LayerMask characterMask;
    public Material lineMaterial;
}
