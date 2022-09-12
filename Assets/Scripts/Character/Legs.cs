using System;
using System.Collections.Generic;
using UnityEngine;

public class Legs : MechaPart
{
    private int _maxSteps;

    private float _moveSpeed;
    private float _rotationSpeed;

    private int _initiative;

    private bool _brokenLegs = false;

    public Action<Character> OnDamageTakenByAttack;
    public int GetMaxSteps()
    {
        return _maxSteps;
    }

    public override void SetPartData(Character character, PartSO data, Color partColor)
    {
        base.SetPartData(character, data, partColor);

        LegsSO legsData = data as LegsSO;

        _maxSteps = legsData.maxSteps;
        _moveSpeed = legsData.moveSpeed;
        _rotationSpeed = legsData.rotationSpeed;
        _initiative = legsData.initiative;
    }

    public int GetLegsInitiative() => _initiative;

    //Lo ejecuta el ButtonsUIManager, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public override void ReceiveDamage(List<Tuple<int, int>> damages)
    {
        if (_currentHP <= 0)
            return;


        //WorldUI worldUI = _myChar.GetMyUI();
        //worldUI.SetLegsHPBar(_currentHP);
        int totalDamage = 0;
        
        for (int i = 0; i < damages.Count; i++)
        {
            totalDamage += damages[i].Item1;
            float hp = _currentHP - damages[i].Item1;
            _currentHP = hp > 0 ? hp : 0;
            //_myChar.SetCharacterMove(_currentHP > 0 ? true : false);

            foreach (GameObject spawner in _particleSpawner)
            {
                EffectsController.Instance.PlayParticlesEffect(spawner, EnumsClass.ParticleActionType.Damage);
            }

            int hitType = damages[i].Item2;

            switch (hitType)
            {
                case MissHit:
                    EffectsController.Instance.CreateDamageText("Miss", 0, transform.position, i == damages.Count - 1 ? true : false);
                    break;

                case NormalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 1, transform.position, i == damages.Count - 1 ? true : false);
                    break;

                case CriticalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 2, transform.position, i == damages.Count - 1 ? true : false);
                    break;
            }
        }

        //worldUI.Show();
        //worldUI.UpdateLegsHPBar(totalDamage);

        _myChar.MechaOutsideAttackRange();

        //TurnManager.Instance.ReducePosition(_myChar);

        OnDamageTaken?.Invoke(_myChar, totalDamage);
        
        OnDamageTakenByAttack?.Invoke(_myChar);

        if (_myChar.IsSelected())
            OnHealthChanged?.Invoke(_currentHP);

        if (CurrentHP <= 0 && !_brokenLegs)
        {
            //EffectsController.Instance.PlayParticlesEffect(_particleSpawner[0], EnumsClass.ParticleActionType.Damage);
            //EffectsController.Instance.PlayParticlesEffect(_particleSpawner[1], EnumsClass.ParticleActionType.Damage);
            foreach (GameObject spawner in _particleSpawner)
            {
                EffectsController.Instance.PlayParticlesEffect(spawner, EnumsClass.ParticleActionType.Mine);
            }
            HalfSteps();
        }

        _myChar.SetHurtAnimation();
    }

    public override void ReceiveDamage(int damage)
    {
        if (_currentHP <= 0)
            return;


        //WorldUI worldUI = _myChar.GetMyUI();

        //worldUI.SetLegsHPBar(_currentHP);
        //worldUI.Show();
        //worldUI.UpdateLegsHPBar(damage);
        //worldUI.HideWithTimer();

        float hp = _currentHP - damage;

        _currentHP = hp > 0 ? hp : 0;

        if (_currentHP <= 0 && !_brokenLegs)
        {
            foreach (GameObject spawner in _particleSpawner)
            {
                EffectsController.Instance.PlayParticlesEffect(spawner, EnumsClass.ParticleActionType.Mine);
            }

            HalfSteps();
        }

        OnDamageTaken?.Invoke(_myChar, damage);

        if (_myChar.IsSelected())
            OnHealthChanged?.Invoke(_currentHP);

        foreach (GameObject spawner in _particleSpawner)
        {
            EffectsController.Instance.PlayParticlesEffect(spawner, EnumsClass.ParticleActionType.Damage);
        }

        EffectsController.Instance.CreateDamageText(damage.ToString(), 1, transform.position);

        _myChar.SetHurtAnimation();
    }

    public float GetRotationSpeed() => _rotationSpeed;

    public float GetMoveSpeed() => _moveSpeed;

    void HalfSteps()
    {
        _maxSteps /= 2;
        _brokenLegs = true;
    }
    
    public override void Heal(int healAmount)
    {
        base.Heal(healAmount);

        EffectsController.Instance.CreateDamageText(healAmount.ToString(), 3, transform.position);
        
        if (_myChar.IsSelected())
            OnHealthChanged?.Invoke(_currentHP);
    }
}
