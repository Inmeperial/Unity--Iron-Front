using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : Parts
{
    public BodySO bodyData;
    private float _bodyMaxHP;
    private float _bodyHP;

    private void Awake()
    {
        _bodyMaxHP = bodyData.maxHP;
        _bodyHP = _bodyMaxHP;
    }
    public override void UpdateHp(float newValue)
    {
        _bodyHP = newValue;
    }
    
    public override float GetMaxHp()
    {
        return _bodyMaxHP;
    }

    public override float GetCurrentHp()
    {
        return _bodyHP;
    }
    
    //Lo ejecuta el ButtonsUIManager, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public void TakeDamageBody(List<Tuple<int,int>> damages)
    {
        var ui = _myChar.GetMyUI();
        ui.SetBodySlider(_bodyHP);
        int total = 0;
        var bodyPos = transform.position;
        for (int i = 0; i < damages.Count; i++)
        {
            total += damages[i].Item1;
            var hp = _bodyHP - damages[i].Item1;
            _bodyHP = hp > 0 ? hp : 0;
            _myChar.effectsController.PlayParticlesEffect(bodyPos, "Damage");
            var item = damages[i].Item2;
            switch (item)
            {
                case MissHit:
                    _myChar.effectsController.CreateDamageText("Miss", 0, bodyPos, i == damages.Count - 1 ? true : false);
                    break;
                   
                case NormalHit:
                    _myChar.effectsController.CreateDamageText(damages[i].Item1.ToString(), 1, bodyPos, i == damages.Count - 1 ? true : false);
                    break;
               
                case CriticalHit:
                    _myChar.effectsController.CreateDamageText(damages[i].Item1.ToString(), 2, bodyPos, i == damages.Count - 1 ? true : false);
                    break;
            }
        }
        if (_bodyHP <= 0)
        {
            _myChar.NotSelectable();
        }
        ui.ContainerActivation(true);
        ui.UpdateBodySlider(total, (int)_bodyHP);
        _myChar.MakeNotAttackable();
    }
    
    //Lo ejecuta el mortero, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public void TakeDamageBody(int damage)
    {
        var ui = _myChar.GetMyUI();
        ui.SetBodySlider(_bodyHP);
        var hp = _bodyHP - damage;
        _bodyHP = hp > 0 ? hp : 0;
        if (_bodyHP <= 0)
        {
            _myChar.NotSelectable();
        }

        var bodyPos = transform.position;
        _myChar.effectsController.PlayParticlesEffect(bodyPos, "Damage");
        _myChar.effectsController.CreateDamageText(damage.ToString(), 1, bodyPos, true);
        ui.ContainerActivation(true);
        ui.UpdateBodySlider(damage, (int)_bodyHP);
        _myChar.MakeNotAttackable();
    }

    
}
