using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flamethrower", menuName = "Create Ability/Flamethrower")]
public class FlamethrowerSO : AbilitySO
{
    public int damage;
    public float range;
    public float angle;
    public LayerMask gridMask;
    public LayerMask characterMask;
    public Material lineMaterial;
}
