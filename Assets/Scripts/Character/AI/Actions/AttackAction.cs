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

        if (!_myUnit.GetLeftGun() && !_myUnit.GetRightGun())
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
        var initialRotation = _myUnit.RotationBeforeLookingAtEnemy;
        _myUnit.RotateTowardsEnemy(closestEnemy.transform);
        var gun = _myUnit.GetSelectedGun();

        if (gun.GetAvailableBullets() <= 0)
        {
            Debug.Log("0 BULLETS");
            return TaskStatus.COMPLETED;
        }
        
        if (!closestEnemy.IsOnElevator())
        {
            GameManager.Instance.CurrentTurnMecha = _myUnit;
            GameManager.Instance.SelectedEnemy = closestEnemy;
            //ButtonsUIManager.Instance.SetPlayerCharacter(_myUnit);
            //ButtonsUIManager.Instance.SetEnemy(closestEnemy);

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

            float lowest = 100000; 

            foreach (KeyValuePair<string, MechaPart> kvp in parts)
            {
                MechaPart part = kvp.Value;
                if (!part)
                    continue;

                if (part.CurrentHP <= 0)
                    continue;
                
                if (part.CurrentHP <= lowest)
                {
                    lowest = part.CurrentHP;
                    partToAttack = kvp.Key;
                }
            }

            if (parts.ContainsKey(partToAttack))
                parts[partToAttack].ReceiveDamage(gun.GetAvailableBullets());

            //switch (partToAttack)
            //{
            //    case "Body":
            //        ButtonsUIManager.Instance.AddBulletsToBody(gun.GetAvailableBullets());
            //        break;
                    
            //    case "LGun":
            //        ButtonsUIManager.Instance.AddBulletsToLArm(gun.GetAvailableBullets());
            //        break;
                
            //    case "RGun":
            //        ButtonsUIManager.Instance.AddBulletsToRArm(gun.GetAvailableBullets());
            //        break;
                
            //    case "Legs":
            //        ButtonsUIManager.Instance.AddBulletsToLegs(gun.GetAvailableBullets());
            //        break;
                
            //    case "DEFAULT":
            //        _myUnit.transform.rotation = initialRotation;
            //        break;
            //}
            if (partToAttack != "DEFAULT")
                _myUnit.OnEndActionWithDelay(0);
            else _myUnit.OnEndAction();
        }
        else
        {
            Elevator elevator = closestEnemy.GetPositionTile().GetElevatorAbove();
            
            if (_myUnit.RayToElevator(elevator.GetColliderForAttack().transform.position))
            {
                var damage = gun.GetCalculatedDamage(gun.GetMaxBullets());
            
                elevator.ReceiveDamage(damage);
                _myUnit.GetSelectedGun().AttackAnimation();
                _myUnit.DeactivateAttack();
                _myUnit.OnEndActionWithDelay(0);
                Debug.Log("attack");
            }
            else
            {
                _myUnit.OnEndAction();
            }
        }
        
        if (_fail)
        {
            _fail = false;
            return TaskStatus.FAILED;
        }
        
        if (_myUnit.CanMove()) return TaskStatus.COMPLETED;
        return TaskStatus.FAILED;
    }
    
    public void MakeItFail()
    {
        Debug.Log("fail");
        _fail = true;
    }
}
