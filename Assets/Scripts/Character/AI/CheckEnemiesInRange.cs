using System.Collections;
using System.Collections.Generic;
using Pada1.BBCore;
using BBUnity.Conditions;
using UnityEngine;

[Condition("Iron Front/AI Conditions/HasEnemiesInRange")]
[Help("Check if Enemy AI has enemies units in attack range.")]

public class CheckEnemiesInRange : GOCondition
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
        
        return _myUnit.HasEnemiesInRange();
    }
}
