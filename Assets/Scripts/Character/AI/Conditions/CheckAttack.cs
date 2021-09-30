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
        var b =_myUnit.CanAttack() && _myUnit.GetSelectedGun() != null;

        if (_myUnit.CanAttack())
        {
            Debug.Log("puedo atacar");
        }
        else
        {
            Debug.Log("no puedo atacar");
        }

        if (_myUnit.GetSelectedGun())
        {
            Debug.Log("tengo arma");
        }
        else
        {
            Debug.Log("no tengo arma");
        }

        return b;

    }
}
