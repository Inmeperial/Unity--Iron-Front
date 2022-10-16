using Pada1.BBCore;
using BBUnity.Conditions;

[Condition("Iron Front/AI Conditions/CanMove")]
[Help("Check if Enemy AI can move.")]
public class CheckMove : GOCondition
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
        return _myUnit.CanMove();
    }
}
