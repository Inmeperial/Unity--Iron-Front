using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Push", menuName = "Create Ability/Push")]
public class PushSO : AbilitySO
{
    public int collisionDamage = 50;
    public int pushDamage = 25;
    public float pushLerpDuration = .75f;
    public int pushUseRange;
    public int pushDistance;
}
