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

    public override void UpdateHp(float newValue)
    {
        _legsHP = newValue;
    }
    
    public override float GetMaxHp()
    {
        return _legsMaxHP;
    }

    public override float GetCurrentHp()
    {
        return _legsHP;
    }

    public int GetLegsInitiative()
    {
        return _initiative;
    }

    //Lo ejecuta el ButtonsUIManager, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public override void TakeDamage(List<Tuple<int,int>> damages)
    {
        WorldUI ui = _myChar.GetMyUI();
        ui.SetLegsSlider(_legsHP);
        int total = 0;
        Vector3 legsPos = transform.position;
        for (int i = 0; i < damages.Count; i++)
        {
            total += damages[i].Item1;
            float hp = _legsHP - damages[i].Item1;
            UpdateHp(hp > 0 ? hp : 0);
            _myChar.SetCharacterMove(_legsHP > 0 ? true : false);
            EffectsController.Instance.PlayParticlesEffect(gameObject, "Damage");
            EffectsController.Instance.PlayParticlesEffect(gameObject, "Hit");
            int item = damages[i].Item2;
            switch (item)
            {
                case MissHit:
                    EffectsController.Instance.CreateDamageText("Miss", 0, legsPos, i == damages.Count - 1 ? true : false);
                    break;
                   
                case NormalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 1, legsPos, i == damages.Count - 1 ? true : false);
                    _myChar.HitSoundMecha();
                    break;
               
                case CriticalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 2, legsPos, i == damages.Count - 1 ? true : false);
                    _myChar.HitSoundMecha();
                    break;
            }
        }
        ui.ContainerActivation(true);
        ui.UpdateLegsSlider(total, _legsHP);
        _myChar.MakeNotAttackable();
        TurnManager.Instance.ReducePosition(_myChar);
    }

    public override void TakeDamage(int damage)
    {
        WorldUI ui = _myChar.GetMyUI();
        ui.SetLegsSlider(_legsHP);
        ui.ContainerActivation(true);
        ui.UpdateLegsSlider(damage, _legsHP);
        ui.DeactivateWorldUIWithTimer();
        float hp = _legsHP - damage;
        _legsHP = hp > 0 ? hp : 0;
        if (_legsHP <= 0 && !_brokenLegs)
        {
            HalfSteps();
        }
        ButtonsUIManager.Instance.UpdateLegsHUD(_legsHP, true);
        EffectsController.Instance.PlayParticlesEffect(gameObject, "Damage");
        EffectsController.Instance.PlayParticlesEffect(gameObject, "Hit");
        _myChar.HitSoundMecha();
        EffectsController.Instance.CreateDamageText(damage.ToString(), 1, transform.position, true);
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
