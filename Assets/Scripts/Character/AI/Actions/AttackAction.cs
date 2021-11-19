using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
        if (!closestEnemy.IsOnElevator())
        {
            ButtonsUIManager.Instance.SetPlayerCharacter(_myUnit);
            ButtonsUIManager.Instance.SetEnemy(closestEnemy);
            
            bool body = _myUnit.RayToPartsForAttack(closestEnemy.GetBodyPosition(), "Body", false);
            bool leftGun = _myUnit.RayToPartsForAttack(closestEnemy.GetLArmPosition(), "LGun",false);
            bool rightArm = _myUnit.RayToPartsForAttack(closestEnemy.GetRArmPosition(), "RGun", false);
            bool legs = _myUnit.RayToPartsForAttack(closestEnemy.GetLegsPosition(), "Legs", false);

            Dictionary<string, float> parts = new Dictionary<string, float>();
            
            if (body)
                parts.Add("Body", closestEnemy.GetBody().GetCurrentHp());
            if (leftGun)
                parts.Add("LGun", closestEnemy.GetLeftGun().GetCurrentHp());
            if (rightArm)
                parts.Add("RGun", closestEnemy.GetRightGun().GetCurrentHp());
            if (legs)
                parts.Add("Legs", closestEnemy.GetLegs().GetCurrentHp());

            string partToAttack = "DEFAULT";
            float lowest = 100000; 
            foreach (var part in parts)
            {
                if (part.Value <= lowest)
                {
                    lowest = part.Value;
                    partToAttack = part.Key;
                }
            }

            switch (partToAttack)
            {
                case "Body":
                    ButtonsUIManager.Instance.AddBulletsToBody(gun.GetAvailableBullets());
                    break;
                    
                case "LGun":
                    ButtonsUIManager.Instance.AddBulletsToLArm(gun.GetAvailableBullets());
                    break;
                
                case "RGun":
                    ButtonsUIManager.Instance.AddBulletsToRArm(gun.GetAvailableBullets());
                    break;
                
                case "Legs":
                    ButtonsUIManager.Instance.AddBulletsToLegs(gun.GetAvailableBullets());
                    break;
                
                case "DEFAULT":
                    _myUnit.transform.rotation = initialRotation;
                    Debug.Log("sin partes para atacar");
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
            }
            else
            {
                _myUnit.OnEndAction();
            }
        }
        
        
        
        return TaskStatus.COMPLETED;
    }
}
