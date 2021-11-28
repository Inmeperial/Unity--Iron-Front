using BBUnity.Actions;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;

[Action("Iron Front/AI Actions/Find Closest Enemy")]
public class FindClosestEnemy : GOAction
{
    private EnemyCharacter _myUnit;

    public override void OnStart()
    {
        
        _myUnit = gameObject.GetComponent<EnemyCharacter>();
    }

    public override TaskStatus OnUpdate()
    {
        
        if (!_myUnit)
        {
            _myUnit = gameObject.GetComponent<EnemyCharacter>();
            if (!_myUnit)
            {
                return TaskStatus.FAILED;
            }
        }

        if (!_myUnit.IsMyTurn())
        {
            _myUnit.ForceBehaviorPause();
            return TaskStatus.FAILED;
        }

        if (!_myUnit.CanMove() && !_myUnit.CanAttack())
            return TaskStatus.FAILED;
        
        
        if (_myUnit.GetClosestEnemy() != null)
            return TaskStatus.FAILED;


        _myUnit.OnStartAction();
        var enemy = _myUnit.CalculateClosestEnemy();

        if (!enemy)
        {
            _myUnit.OnEndAction();
            return TaskStatus.FAILED;
        }
        _myUnit.SetClosestEnemy(enemy);
        _myUnit.OnEndAction();
        
        return TaskStatus.COMPLETED;
    }
}
