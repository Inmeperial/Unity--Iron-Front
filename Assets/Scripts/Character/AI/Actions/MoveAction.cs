using BBUnity.Actions;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using UnityEngine;

[Action("Iron Front/AI Actions/Move")]
[Help("Enemy AI will move towards the closest enemy depending on its move range.")]
public class MoveAction : GOAction
{
    private EnemyCharacter _myUnit;

    private bool _fail;
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

        _myUnit.OnStartAction(MakeItFail);
        if (!_myUnit.IsMoving() && _myUnit.FoundPath())
        {
            _myUnit.EnemyMove();
        }
            
        else _myUnit.OnEndAction();

        if (_fail)
        {
            _fail = false;
            return TaskStatus.FAILED;
        }

        if (_myUnit.CanAttack())
        {
            if (!_myUnit.IsMoving())
                _myUnit.SetCharacterMove(false);
            return TaskStatus.COMPLETED;
        }
            
        
        return TaskStatus.FAILED;
    }

    public void MakeItFail()
    {
        _fail = true;
    }
}
