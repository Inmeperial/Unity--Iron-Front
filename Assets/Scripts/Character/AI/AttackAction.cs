using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BBUnity.Actions;
using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;

[Action("Iron Front/AI Actions/Attack")]
[Help("Enemy AI will attack the closest enemy depending on its move range (debug ).")]
public class AttackAction : GOAction
{
    private EnemyCharacter _myUnit;

    public override void OnStart()
    {
        
        _myUnit = gameObject.GetComponent<EnemyCharacter>();
        
        if (_myUnit)
            Debug.Log("si encontre el enemy character en el start");
    }

    public override TaskStatus OnUpdate()
    {
        
        if (!_myUnit)
        {
            _myUnit = gameObject.GetComponent<EnemyCharacter>();
            if (!_myUnit)
            {
                Debug.Log("no encontre al enemy character");

                return TaskStatus.FAILED;
            }
        }

        Debug.Log("ataco");
        _myUnit.SetAttackFORTEST();
        return TaskStatus.COMPLETED;

    }
}
