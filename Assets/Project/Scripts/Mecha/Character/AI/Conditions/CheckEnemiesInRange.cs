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

        if (_myUnit.IsRightGunAlive())
        {
            _myUnit.SelectRightGun();
            if (_myUnit.HasEnemiesInRange())
                return true;
        }

        if (_myUnit.IsLeftGunAlive())
        {
            _myUnit.SelectLeftGun();
            if (_myUnit.HasEnemiesInRange())
                return true;
        }
        return false;
    }
}
