using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : Parts
{
    public ArmSO armData;
    private string _location;

    public override void SetPart()
    {
        _maxHP = armData.maxHP;
        _currentHP = _maxHP;
    }

    public override void UpdateHp(float newValue)
    {
        _currentHP = newValue;
    }

    public void SetRightOrLeft(string location)
    {
        _location = location;
    }
    
    //Lo ejecuta el ButtonsUIManager, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public override void TakeDamage(List<Tuple<int,int>> damages)
    {
        if (_currentHP <= 0) return;
        
        int total = 0;
        Vector3 pos = transform.position;
        for (int i = 0; i < damages.Count; i++)
        {
            total += damages[i].Item1;
            float hp = _currentHP - damages[i].Item1;
            _currentHP = hp > 0 ? hp : 0;
            EffectsController.Instance.PlayParticlesEffect(this.gameObject, EnumsClass.ParticleActionType.Damage);
            EffectsController.Instance.PlayParticlesEffect(this.gameObject, EnumsClass.ParticleActionType.Hit);

            int item = damages[i].Item2;
            switch (item)
            {
                case MissHit:
                    EffectsController.Instance.CreateDamageText("Miss", 0, pos, i == damages.Count - 1 ? true : false);
                    break;
                   
                case NormalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 1, pos, i == damages.Count - 1 ? true : false);
                    _myChar.HitSoundMecha();
                    break;
               
                case CriticalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 2, pos, i == damages.Count - 1 ? true : false);
                    _myChar.HitSoundMecha();
                    break;
            }
        }
        
        WorldUI ui = _myChar.GetMyUI();
        ui.ContainerActivation(true);
        
        switch (_location)
        {
            case "Left":
                ui.SetLeftArmSlider(_currentHP);
                ui.UpdateLeftArmSlider(total, (int)_currentHP);
                break;
            
            case "Right":
                ui.SetRightArmSlider(_currentHP);
                ui.UpdateRightArmSlider(total, (int)_currentHP);
                break;
        }
        if (_currentHP <= 0)
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
    public override void TakeDamage(int damage)
    {
        if (_currentHP <= 0) return;
        
        float hp = _currentHP - damage;
        _currentHP = hp > 0 ? hp : 0;
        
        Vector3 pos = transform.position;
        EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Damage);
        EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Hit);
        _myChar.HitSoundMecha();

        EffectsController.Instance.CreateDamageText(damage.ToString(), 1, pos, true);
        
        WorldUI ui = _myChar.GetMyUI();
        ui.ContainerActivation(true);
        
        switch (_location)
        {
            case "Left":
                ui.SetLeftArmSlider(_currentHP);
                ui.UpdateLeftArmSlider(damage, (int)_currentHP);
                break;
            
            case "Right":
                ui.SetRightArmSlider(_currentHP);
                ui.UpdateRightArmSlider(damage, (int)_currentHP);
                break;
        }
        
        if (_currentHP <= 0)
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
}
