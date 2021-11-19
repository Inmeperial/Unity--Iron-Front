using BBUnity.Actions;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;

[Action("Iron Front/AI Actions/Move")]
[Help("Enemy AI will move towards the closest enemy depending on its move range.")]
public class MoveAction : GOAction
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

        _myUnit.OnStartAction();
        if (!_myUnit.IsMoving() && _myUnit.FoundPath())
        {
            _myUnit.EnemyMove();
        }
            
        else _myUnit.OnEndAction();
        
        return TaskStatus.COMPLETED;


    }
}
