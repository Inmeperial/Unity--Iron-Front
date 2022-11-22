using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Legs : MechaPart
{
    private LegsSO _data;
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

        _data = data as LegsSO;

        _maxSteps = _data.maxSteps;
        _moveSpeed = _data.moveSpeed;
        _rotationSpeed = _data.rotationSpeed;
        _initiative = _data.initiative;
    }

    public int GetLegsInitiative() => _initiative;

    //Lo ejecuta el ButtonsUIManager, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public override void ReceiveDamage(List<Tuple<int, int>> damages)
    {
        base.ReceiveDamage(damages);

        int totalDamage = 0;
        
        for (int i = 0; i < damages.Count; i++)
        {
            totalDamage += damages[i].Item1;
            float hp = _currentHP - damages[i].Item1;
            _currentHP = hp > 0 ? hp : 0;

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

        _myChar.MechaOutsideAttackRange();

        OnDamageTaken?.Invoke(_myChar, totalDamage);
        
        OnDamageTakenByAttack?.Invoke(_myChar);

        if (_myChar.IsSelected())
            OnHealthChanged?.Invoke(_currentHP);

        if (IsPartBroken() && !_brokenLegs)
            BreakLegs();

        _myChar.PlayReceiveDamageAnimation();
    }

    public override void ReceiveDamage(int damage)
    {
        base.ReceiveDamage(damage);

        float hp = _currentHP - damage;

        _currentHP = hp > 0 ? hp : 0;

        EffectsController.Instance.CreateDamageText(damage.ToString(), 1, transform.position);

        OnDamageTaken?.Invoke(_myChar, damage);

        if (_myChar.IsSelected())
            OnHealthChanged?.Invoke(_currentHP);        

        if (IsPartBroken() && !_brokenLegs)
            BreakLegs();

        _myChar.PlayReceiveDamageAnimation();
    }

    public float GetRotationSpeed()
    {
        return _rotationSpeed;
    }

    public float GetMoveSpeed()
    {
        return _moveSpeed;
    }

    private void BreakLegs()
    {
        PlayDestroySound();
        PlayDestroyVFX();
        HalfSteps();
    }
    private void HalfSteps()
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

    public override void PlayTakeDamageSound()
    {
        AudioManager.Instance.PlaySound(_data.damageSound, gameObject);
    }

    public override void PlayDestroySound()
    {
        AudioManager.Instance.PlaySound(_data.destroySound, gameObject);
    }

    public override void PlayTakeDamageVFX()
    {
        EffectsController.Instance.PlayParticlesEffect(_data.damageParticle, transform.position, transform.forward);
    }
    public override void PlayDestroyVFX()
    {
        EffectsController.Instance.PlayParticlesEffect(_data.destroyParticle, transform.position, transform.forward);
    }
}
