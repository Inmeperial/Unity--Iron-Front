using BBUnity.Actions;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;

[Action("Iron Front/AI Actions/End Turn")]
[Help("Enemy AI will end it's turn.")]
public class EndTurnAction : GOAction
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
                return TaskStatus.FAILED;
        }
        
        ButtonsUIManager.Instance.EndTurn();
        _myUnit.OnStartAction(null);
        return TaskStatus.COMPLETED;
    }
}
