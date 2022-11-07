using UnityEngine;

[CreateAssetMenu(fileName = "Self Destruct", menuName = "Scriptable Objects/Abilities/Body/Self Destruct")]
public class SelfDestructSO : BodyAbilitySO
{
    public float selfDestructRange;
    public int selfDestructDamage;
}
