using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SelfDestruct", menuName = "Create Ability/SelfDestruct")]
public class SelfDestructSO : BodyAbilitySO
{
    public float selfDestructRange;
    public int selfDestructDamage;
}
