using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        Character closestEnemy = _myUnit.GetClosestEnemy();
        ButtonsUIManager.Instance.SetPlayerCharacter(_myUnit);
        ButtonsUIManager.Instance.SetEnemy(closestEnemy);
        
        var rot = _myUnit.InitialRotation;
        _myUnit.RotateTowardsEnemy(closestEnemy.transform.position);
        bool body = _myUnit.RayToPartsForAttack(closestEnemy.GetBodyPosition(), "Body", false);
        bool leftArm = _myUnit.RayToPartsForAttack(closestEnemy.GetLArmPosition(), "LArm",false);
        bool rightArm = _myUnit.RayToPartsForAttack(closestEnemy.GetRArmPosition(), "RArm", false);
        bool legs = _myUnit.RayToPartsForAttack(closestEnemy.GetLegsPosition(), "Legs", false);

        Dictionary<string, float> parts = new Dictionary<string, float>();
        
        if (body)
            parts.Add("Body", closestEnemy.body.GetCurrentHp());
        if (leftArm)
            parts.Add("LArm", closestEnemy.leftArm.GetCurrentHp());
        if (rightArm)
            parts.Add("RArm", closestEnemy.rightArm.GetCurrentHp());
        if (legs)
            parts.Add("Legs", closestEnemy.legs.GetCurrentHp());

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

        var gun = _myUnit.GetSelectedGun();
        switch (partToAttack)
        {
            case "Body":
                ButtonsUIManager.Instance.AddBulletsToBody(gun.GetAvailableBullets());
                break;
                
            case "LArm":
                ButtonsUIManager.Instance.AddBulletsToLArm(gun.GetAvailableBullets());
                break;
            
            case "RArm":
                ButtonsUIManager.Instance.AddBulletsToRArm(gun.GetAvailableBullets());
                break;
            
            case "Legs":
                ButtonsUIManager.Instance.AddBulletsToLegs(gun.GetAvailableBullets());
                break;
            
            case "DEFAULT":
                Debug.Log("sin partes para atacar");
                break;
        }
        _myUnit.OnEndAction();
        return TaskStatus.COMPLETED;

    }
}
