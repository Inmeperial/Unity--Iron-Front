using System.Collections;
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
            {
                return TaskStatus.FAILED;
            }
        }

        _myUnit.OnStartAction(MakeItFail);

        if (!_myUnit.LeftGunAlive() && !_myUnit.RightGunAlive())
        {
            _myUnit.OnEndAction();
            return TaskStatus.COMPLETED;
        }

        if (!_myUnit.GetLeftGun() && !_myUnit.GetRightGun())
        {
            _myUnit.OnEndAction();
            return TaskStatus.COMPLETED;
        }
        
        Character closestEnemy = _myUnit.GetClosestEnemy();
        var initialRotation = _myUnit.InitialRotation;
        _myUnit.RotateTowardsEnemy(closestEnemy.transform);
        var gun = _myUnit.GetSelectedGun();

        if (gun.GetAvailableBullets() <= 0)
        {
            Debug.Log("0 BULLETS");
            return TaskStatus.COMPLETED;
        }
        
        if (!closestEnemy.IsOnElevator())
        {
            ButtonsUIManager.Instance.SetPlayerCharacter(_myUnit);
            ButtonsUIManager.Instance.SetEnemy(closestEnemy);
            
            bool body = _myUnit.RayToPartsForAttack(closestEnemy.GetBodyPosition(), "Body", false);
            bool leftGun = _myUnit.RayToPartsForAttack(closestEnemy.GetLArmPosition(), "LGun",false);
            bool rightGun = _myUnit.RayToPartsForAttack(closestEnemy.GetRArmPosition(), "RGun", false);
            bool legs = _myUnit.RayToPartsForAttack(closestEnemy.GetLegsPosition(), "Legs", false);

            Dictionary<string, float> parts = new Dictionary<string, float>();
            
            if (body && closestEnemy.GetBody().GetCurrentHp() > 0)
                parts.Add("Body", closestEnemy.GetBody().GetCurrentHp());
            if (leftGun && closestEnemy.GetLeftGun().GetCurrentHp() > 0)
                parts.Add("LGun", closestEnemy.GetLeftGun().GetCurrentHp());
            if (rightGun && closestEnemy.GetRightGun().GetCurrentHp() > 0)
                parts.Add("RGun", closestEnemy.GetRightGun().GetCurrentHp());
            if (legs && closestEnemy.GetLegs().GetCurrentHp() > 0)
                parts.Add("Legs", closestEnemy.GetLegs().GetCurrentHp());

            string partToAttack = "DEFAULT";
            float lowest = 100000; 
            foreach (var part in parts)
            {
                if (part.Value <= 0) continue;
                
                if (part.Value <= lowest)
                {
                    lowest = part.Value;
                    partToAttack = part.Key;
                }
            }
            switch (partToAttack)
            {
                case "Body":
                    Debug.Log("attack");
                    ButtonsUIManager.Instance.AddBulletsToBody(gun.GetAvailableBullets());
                    break;
                    
                case "LGun":
                    Debug.Log("attack");
                    ButtonsUIManager.Instance.AddBulletsToLArm(gun.GetAvailableBullets());
                    break;
                
                case "RGun":
                    Debug.Log("attack");
                    ButtonsUIManager.Instance.AddBulletsToRArm(gun.GetAvailableBullets());
                    break;
                
                case "Legs":
                    Debug.Log("attack");
                    ButtonsUIManager.Instance.AddBulletsToLegs(gun.GetAvailableBullets());
                    break;
                
                case "DEFAULT":
                    _myUnit.transform.rotation = initialRotation;
                    break;
            }
            if (partToAttack != "DEFAULT")
                _myUnit.OnEndActionWithDelay(0);
            else _myUnit.OnEndAction();
        }
        else
        {
            var elevator = closestEnemy.GetMyPositionTile().GetElevatorAbove();
            
            if (_myUnit.RayToElevator(elevator.GetColliderForAttack().transform.position))
            {
                var damage = gun.DamageCalculation(gun.GetMaxBullets());
            
                elevator.TakeDamage(damage);
                _myUnit.Shoot();
                _myUnit.DeactivateAttack();
                _myUnit.OnEndActionWithDelay(0);
                Debug.Log("attack");
            }
            else
            {
                _myUnit.OnEndAction();
            }
        }
        
        // if (_fail)
        // {
        //     _fail = false;
        //     return TaskStatus.FAILED;
        // }
        
        if (_myUnit.CanAttack()) return TaskStatus.FAILED;
        return TaskStatus.COMPLETED;
    }
    
    public void MakeItFail()
    {
        Debug.Log("fail");
        _fail = true;
    }
}
