using Pada1.BBCore;
using BBUnity.Conditions;

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

        if (_myUnit.IsMoving()) 
            return false;

        if (!_myUnit.CanAttack())
            return false;

        //if (!_myUnit.IsLeftGunAlive() && !_myUnit.IsRightGunAlive())
        //    return false;

        return true;
    }
}
