using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pada1.BBCore;
using BBUnity.Conditions;
using Pada1.BBCore.Tasks;
using UnityEngine;
using TaskStatus = System.Threading.Tasks.TaskStatus;

[Condition("Iron Front/AI Conditions/CanAttack")]
[Help("Check if Enemy AI can attack.")]
public class CheckAttack : GOCondition
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

        if (_myUnit.IsMoving()) return false;
        
        return _myUnit.CanAttack() && _myUnit.GetSelectedGun() != null && _myUnit.GetSelectedGun().GetGunType() != EnumsClass.GunsType.Shield;
    }
}
