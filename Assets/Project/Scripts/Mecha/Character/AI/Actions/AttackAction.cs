using System;
using System.Collections.Generic;
using BBUnity.Actions;
using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;

[Action("Iron Front/AI Actions/Attack")]
[Help("Enemy AI will attack the closest enemy depending on its move range (debug ).")]
public class AttackAction : GOAction
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
                return TaskStatus.FAILED;
        }

        _myUnit.OnStartAction(Fail);

        Character closestEnemy = _myUnit.GetClosestEnemy();

        Quaternion initialRotation = _myUnit.RotationBeforeLookingAtEnemy;

        _myUnit.RotateTowardsEnemy(closestEnemy.transform);

        if (_myUnit.IsLeftGunAlive() || _myUnit.IsRightGunAlive())
        {
            //_myUnit.OnEndAction();
            //return TaskStatus.COMPLETED;

            Gun gun = _myUnit.GetSelectedGun();

            if (gun.GetAvailableBullets() <= 0)
            {
                Debug.Log("0 BULLETS");
                return TaskStatus.COMPLETED;
            }
        
            if (!closestEnemy.IsOnElevator())
            {
                Dictionary<string, MechaPart> parts = new Dictionary<string, MechaPart>();
            
                if (_myUnit.IsEnemyBodyInSight(closestEnemy))
                    parts.Add("Body", closestEnemy.GetBody());

                if (_myUnit.IsEnemyLeftGunInSight(closestEnemy))
                    parts.Add("LGun", closestEnemy.GetLeftGun());

                if (_myUnit.IsEnemyRightGunInSight(closestEnemy))
                    parts.Add("RGun", closestEnemy.GetRightGun());

                if (_myUnit.IsEnemyLegsInSight(closestEnemy))
                    parts.Add("Legs", closestEnemy.GetLegs());

                string partToAttack = "DEFAULT";

                float lowest = 1; 

                foreach (KeyValuePair<string, MechaPart> kvp in parts)
                {
                    MechaPart part = kvp.Value;
                    if (!part)
                        continue;

                    if (part.CurrentHP <= 0)
                        continue;

                    float hpPercentage = part.CurrentHP / part.MaxHp;

                    if (hpPercentage <= lowest)
                    {
                        lowest = hpPercentage;
                        partToAttack = kvp.Key;
                    }
                }

                if (parts.ContainsKey(partToAttack))
                {
                    gun.Attack(parts[partToAttack], gun.GetAvailableBullets());

                    _myUnit.SetCharacterMoveState(false);
                }

                if (partToAttack != "DEFAULT")
                    _myUnit.OnEndActionWithDelay(0);
                else 
                    _myUnit.OnEndAction();
            }
            else
            {
                Elevator elevator = closestEnemy.GetPositionTile().GetElevatorAbove();
            
                if (_myUnit.RayToElevator(elevator.GetColliderForAttack().transform.position))
                {            
                    _myUnit.GetSelectedGun().Attack(elevator, gun.GetMaxBullets());
                    _myUnit.OnEndActionWithDelay(0);
                    _myUnit.SetCharacterMoveState(false);
                    _myUnit.OnEndActionWithDelay(0);
                }
                else
                {
                    _myUnit.OnEndAction();
                }
            }
        }
        else
        {
            if (!_myUnit.IsEnemyBodyInSight(closestEnemy))
            {
                _myUnit.OnEndAction();
                return TaskStatus.COMPLETED;
            }

            Body myBody = _myUnit.GetBody();

            closestEnemy.GetBody().ReceiveDamage(myBody.GetData().attackDamage);

            myBody.ReceiveDamage(myBody.GetData().attackDamage);

            Debug.Log("Body attack!" + _myUnit.GetCharacterName() + " -->" + closestEnemy.GetCharacterName());

            _myUnit.DoAttackAction();

            _myUnit.OnEndActionWithDelay(0);
        }

        if (!_myUnit.CanAttack())
        {
            _myUnit.SetCharacterMoveState(false);
            _myUnit.ResetTilesInAttackRange();
            _myUnit.ResetTilesInMoveRange();
        }

        if (_fail)
        {
            _fail = false;
            return TaskStatus.FAILED;
        }
        
        return TaskStatus.COMPLETED;
    }
    
    private void Fail()
    {
        _fail = true;
    }
}
