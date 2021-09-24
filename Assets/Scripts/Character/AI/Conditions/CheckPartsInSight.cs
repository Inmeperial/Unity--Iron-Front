﻿using System.Collections;
using System.Collections.Generic;
using Pada1.BBCore;
using BBUnity.Conditions;
using Pada1.BBCore.Tasks;
using UnityEngine;
using TaskStatus = System.Threading.Tasks.TaskStatus;

[Condition("Iron Front/AI Conditions/CheckPartsInSight")]
[Help("Checks which enemy parts are in sight for attack.")]
public class CheckPartsInSight : GOCondition
{
    private EnemyCharacter _myUnit;
    
    public override bool Check()
    {
        if (!_myUnit)
        {
            _myUnit = gameObject.GetComponent<EnemyCharacter>();

            if (!_myUnit)
                return false;
        }

        if (_myUnit.checkedParts) return true;
        
        var enemies = TurnManager.Instance.GetEnemies(_myUnit.GetUnitTeam());

        Character closestEnemy = _myUnit.GetClosestEnemy();
        var rot = _myUnit.InitialRotation;
        //_myUnit.RotateTowardsEnemy(closestEnemy.transform.position);
        _myUnit.RotateTowardsEnemy(closestEnemy.transform);
        bool body = _myUnit.RayToPartsForAttack(closestEnemy.GetBodyPosition(), "Body", false);
        bool leftArm = _myUnit.RayToPartsForAttack(closestEnemy.GetLArmPosition(), "LArm", false);
        bool rightArm = _myUnit.RayToPartsForAttack(closestEnemy.GetRArmPosition(), "RArm", false);
        bool legs = _myUnit.RayToPartsForAttack(closestEnemy.GetLegsPosition(), "Legs", false);

        if (body || leftArm || rightArm || legs)
            _myUnit.checkedParts = true;
        //_myUnit.ResetRotationAndRays(rot);
        _myUnit.ResetRotationAndRays();
        return body || leftArm || rightArm || legs;
        }
}
