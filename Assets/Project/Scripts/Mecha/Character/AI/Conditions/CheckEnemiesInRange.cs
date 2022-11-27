using System.Collections.Generic;
using Pada1.BBCore;
using BBUnity.Conditions;


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

        Body body = _myUnit.GetBody();
        Tile tile = _myUnit.GetPositionTile();

        List<Tile> neighbours =  tile.GetNeighbours();
        
        if (neighbours.Count < 1)
            return false;

        List<Character> mechas = new List<Character>();

        foreach(Tile t in neighbours)
        {
            Character mecha = t.GetUnitAbove();

            if (mecha == null) 
                continue;

            if (mecha.IsDead())
                continue;

            if (mecha.GetUnitTeam() == _myUnit.GetUnitTeam())
                continue;

            mechas.Add(mecha);
        }

        if (mechas.Count < 1)
            return false;


        Character target = mechas[0];

        foreach (Character mecha in mechas)
        {
            if (mecha.GetBody().CurrentHP >= target.GetBody().CurrentHP)
                continue;

            target = mecha;
        }

        _myUnit.SetClosestEnemy(target);

        return true;
    }
}
