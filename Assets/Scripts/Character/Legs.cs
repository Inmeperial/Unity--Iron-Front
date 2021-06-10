using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legs : Parts
{
    public LegsSO legsData;
    private float _legsMaxHP;
    private float _legsHP;
    
    private int _maxSteps;
    
    private float _moveSpeed;
    private float _rotationSpeed;

    private int _initiative;

    private bool _brokenLegs = false;

    private void Awake()
    {
        _legsMaxHP = legsData.maxHP;
        _legsHP = _legsMaxHP;
        _maxSteps = legsData.maxSteps;
        _moveSpeed = legsData.moveSpeed;
        _rotationSpeed = legsData.rotationSpeed;
        _initiative = legsData.initiative;
    }
    
    public int GetMaxSteps()
    {
        return _maxSteps;
    }

    public override void UpdateHP(float newValue)
    {
        _legsHP = newValue;
    }
    
    public override float GetMaxHP()
    {
        return _legsMaxHP;
    }

    public override float GetCurrentHP()
    {
        return _legsHP;
    }

    public int GetLegsInitiative()
    {
        return _initiative;
    }

    //Lo ejecuta el ButtonsUIManager, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public void TakeDamageLegs(List<Tuple<int,int>> damages)
    {
        var ui = _myChar.GetMyUI();
        ui.SetLegsSlider(_legsHP);
        int total = 0;
        var legsPos = transform.position;
        for (int i = 0; i < damages.Count; i++)
        {
            total += damages[i].Item1;
            var hp = _legsHP - damages[i].Item1;
            UpdateHP(hp > 0 ? hp : 0);
            _myChar.SetCharacterMove(_legsHP > 0 ? true : false);
            _myChar.effectsController.PlayParticlesEffect(legsPos, "Damage");
            var item = damages[i].Item2;
            switch (item)
            {
                case _missHit:
                    _myChar.effectsController.CreateDamageText("Miss", 0, legsPos, i == damages.Count - 1 ? true : false);
                    break;
                   
                case _normalHit:
                    _myChar.effectsController.CreateDamageText(damages[i].Item1.ToString(), 1, legsPos, i == damages.Count - 1 ? true : false);
                    break;
               
                case _criticalHit:
                    _myChar.effectsController.CreateDamageText(damages[i].Item1.ToString(), 2, legsPos, i == damages.Count - 1 ? true : false);
                    break;
            }
        }
        ui.ContainerActivation(true);
        ui.UpdateLegsSlider(total, _legsHP);
        _myChar.MakeNotAttackable();
    }
    // public void TakeDamageLegs(int damage)
    // {
    //     var ui = _myChar.GetMyUI();
    //     ui.SetLegsSlider(_legsHP);
    //     var hp = _legsHP - damage;
    //     _legsHP = hp > 0 ? hp : 0;
    //     if (_legsHP <= 0 && !_brokenLegs)
    //     {
    //         HalfSteps();
    //         //_myChar.SetCharacterMove(false);
    //     }
    //     _myChar.effectsController.PlayParticlesEffect(transform.position, "Damage");
    //     _myChar.effectsController.CreateDamageText(damage.ToString(), 1, transform.position, true);
    //     ui.ContainerActivation(true);
    //     ui.UpdateLegsSlider(damage, (int)_legsHP);
    //     _myChar.MakeNotAttackable();
    // }
    
    public void TakeDamageLegs(int damage)
    {
        var ui = _myChar.GetMyUI();
        ui.SetLegsSlider(_legsHP);
        ui.ContainerActivation(true);
        ui.UpdateLegsSlider(damage, _legsHP);
        ui.DeactivateWorldUIWithTimer();
        var hp = _legsHP - damage;
        _legsHP = hp > 0 ? hp : 0;
        if (_legsHP <= 0 && !_brokenLegs)
        {
            //_myChar.SetCharacterMove(false);
            HalfSteps();
        }
        _myChar.buttonsManager.UpdateLegsHUD(_legsHP, true);
        _myChar.effectsController.PlayParticlesEffect(transform.position, "Damage");
        _myChar.effectsController.CreateDamageText(damage.ToString(), 1, transform.position, true);
        //_myChar.ShowWorldUI();
    }

    public float GetRotationSpeed()
    {
        return _rotationSpeed;
    }

    public float GetMoveSpeed()
    {
        return _moveSpeed;
    }

    void HalfSteps()
    {
        _maxSteps /= 2;
        _brokenLegs = true;
    }
}
