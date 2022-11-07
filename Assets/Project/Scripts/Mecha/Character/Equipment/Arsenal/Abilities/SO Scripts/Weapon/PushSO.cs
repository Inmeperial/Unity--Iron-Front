using UnityEngine;

[CreateAssetMenu(fileName = "Push", menuName = "Scriptable Objects/Abilities/Weapons/Push")]
public class PushSO : GunAbilitySO
{
    public int collisionDamage = 50;
    public int pushDamage = 25;
    public float pushLerpDuration = .75f;
    public int pushUseRange;
    public int pushDistance;
}
