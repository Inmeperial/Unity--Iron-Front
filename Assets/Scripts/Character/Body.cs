﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : Parts
{
    public BodySO bodyData;

    public override void SetPart()
    {
        _maxHP = bodyData.maxHP;
        _currentHP = _maxHP;
    }

    public override void UpdateHp(float newValue)
    {
        _currentHP = newValue;
    }

    //Lo ejecuta el ButtonsUIManager, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public override void TakeDamage(List<Tuple<int,int>> damages)
    {
        if (_currentHP <= 0) return;
        
        
        WorldUI ui = _myChar.GetMyUI();
        ui.SetBodySlider(_currentHP);
        int total = 0;
        Vector3 bodyPos = transform.position;
        for (int i = 0; i < damages.Count; i++)
        {
            total += damages[i].Item1;
            float hp = _currentHP - damages[i].Item1;
            _currentHP = hp > 0 ? hp : 0;

            EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Damage);
            EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Hit);
            int item = damages[i].Item2;
            switch (item)
            {
                case MissHit:
                    EffectsController.Instance.CreateDamageText("Miss", 0, bodyPos, i == damages.Count - 1 ? true : false);
                    break;
                   
                case NormalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 1, bodyPos, i == damages.Count - 1 ? true : false);
                    _myChar.HitSoundMecha();
                    break;
               
                case CriticalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 2, bodyPos, i == damages.Count - 1 ? true : false);
                    _myChar.HitSoundMecha();
                    break;
            }
        }
        
        ui.ContainerActivation(true);
        ui.UpdateBodySlider(total, (int)_currentHP);
        _myChar.MakeNotAttackable();
        
        if (_currentHP <= 0)
            _myChar.Dead();
    }
    
    //Lo ejecuta el mortero, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public override void TakeDamage(int damage)
    {
        if (_currentHP <= 0) return;
        
        
        WorldUI ui = _myChar.GetMyUI();
        ui.SetBodySlider(_currentHP);
        float hp = _currentHP - damage;
        _currentHP = hp > 0 ? hp : 0;

        Vector3 bodyPos = transform.position;
        EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Damage);
        EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Hit);
        _myChar.HitSoundMecha();
        EffectsController.Instance.CreateDamageText(damage.ToString(), 1, bodyPos, true);
        ui.ContainerActivation(true);
        ui.UpdateBodySlider(damage, (int)_currentHP);
        _myChar.MakeNotAttackable();
        
        if (_currentHP <= 0)
            _myChar.Dead();
    }

    public void MechaDeath()
    {
        _myChar.NotSelectable();
        //_myChar.effectsController.PlayParticlesEffect(this.gameObject, "Dead");
    }

}
