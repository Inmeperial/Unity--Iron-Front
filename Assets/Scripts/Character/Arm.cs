using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : Parts
{
    public ArmSO armData;
    private float _armMaxHP;
    private float _armHP;
    private string _location;
    private void Awake()
    {
        _armMaxHP = armData.maxHP;
        _armHP = _armMaxHP;
    }

    public override void UpdateHp(float newValue)
    {
        _armHP = newValue;
    }
    
    public override float GetMaxHp()
    {
        return _armMaxHP;
    }
    
    public override float GetCurrentHp()
    {
        return _armHP;
    }

    public void SetRightOrLeft(string location)
    {
        _location = location;
    }
    
    //Lo ejecuta el ButtonsUIManager, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public void TakeDamageArm(List<Tuple<int,int>> damages)
    {
        
        int total = 0;
        var pos = transform.position;
        for (int i = 0; i < damages.Count; i++)
        {
            total += damages[i].Item1;
            var hp = _armHP - damages[i].Item1;
            _armHP = hp > 0 ? hp : 0;
            _myChar.effectsController.PlayParticlesEffect(pos, "Damage");

            var item = damages[i].Item2;
            switch (item)
            {
                case MissHit:
                    _myChar.effectsController.CreateDamageText("Miss", 0, pos, i == damages.Count - 1 ? true : false);
                    break;
                   
                case NormalHit:
                    _myChar.effectsController.CreateDamageText(damages[i].Item1.ToString(), 1, pos, i == damages.Count - 1 ? true : false);
                    break;
               
                case CriticalHit:
                    _myChar.effectsController.CreateDamageText(damages[i].Item1.ToString(), 2, pos, i == damages.Count - 1 ? true : false);
                    break;
            }
        }
        
        var ui = _myChar.GetMyUI();
        ui.ContainerActivation(true);
        
        switch (_location)
        {
            case "Left":
                ui.SetLeftArmSlider(_armHP);
                ui.UpdateLeftArmSlider(total, (int)_armHP);
                break;
            
            case "Right":
                ui.SetRightArmSlider(_armHP);
                ui.UpdateRightArmSlider(total, (int)_armHP);
                break;
        }
        if (_armHP <= 0)
        {
            switch (_location)
            {
                case "Left":
                    _myChar.GetLeftGun().TurnOff();
                    break;
                case "Right":
                    _myChar.GetRightGun().TurnOff();
                    break;
            }
            
        }
        _myChar.CheckArms();
        _myChar.MakeNotAttackable();
    }
    
    //Lo ejecuta el mortero, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public void TakeDamageArm(int damage)
    {
        
        var hp = _armHP - damage;
        _armHP = hp > 0 ? hp : 0;
        
        var pos = transform.position;
        _myChar.effectsController.PlayParticlesEffect(pos, "Damage");
        _myChar.effectsController.CreateDamageText(damage.ToString(), 1, pos, true);
        
        var ui = _myChar.GetMyUI();
        ui.ContainerActivation(true);
        
        switch (_location)
        {
            case "Left":
                ui.SetLeftArmSlider(_armHP);
                ui.UpdateLeftArmSlider(damage, (int)_armHP);
                break;
            
            case "Right":
                ui.SetRightArmSlider(_armHP);
                ui.UpdateRightArmSlider(damage, (int)_armHP);
                break;
        }
        
        _myChar.CheckArms();
        _myChar.MakeNotAttackable();
    }
}
