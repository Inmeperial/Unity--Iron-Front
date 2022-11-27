using System;
using System.Collections.Generic;
using UnityEngine;

public class Body : MechaPart
{
    private BodySO _data;

    public override void SetPartData(Character character, PartSO data, Color partColor)
    {
        base.SetPartData(character, data, partColor);

        _data = data as BodySO;
    }

    public override void ReceiveDamage(List<Tuple<int,int>> damages)
    {
        base.ReceiveDamage(damages);

        int totalDamage = 0;

        for (int i = 0; i < damages.Count; i++)
        {
            totalDamage += damages[i].Item1;
            float hp = _currentHP - damages[i].Item1;
            _currentHP = hp > 0 ? hp : 0;

            int item = damages[i].Item2;
            switch (item)
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

        OnDamageTaken?.Invoke(_myChar, totalDamage);

        _myChar.MechaOutsideAttackRange();

        if (_myChar.GetUnitTeam() == EnumsClass.Team.Red)
            Debug.Log("AI " + _myChar.GetCharacterName() + " took damage!");

        _myChar.PlayReceiveDamageAnimation();

        if (_myChar.IsSelected())
            OnHealthChanged?.Invoke(_currentHP);

        if (IsPartBroken())
            DestroyPart();
    }
    
    public override void ReceiveDamage(int damage)
    {
        base.ReceiveDamage(damage);

        float hp = _currentHP - damage;
        _currentHP = hp > 0 ? hp : 0;

        if (_myChar.IsSelected())
            OnHealthChanged?.Invoke(_currentHP);

        EffectsController.Instance.CreateDamageText(damage.ToString(), 1, transform.position);

        OnDamageTaken?.Invoke(_myChar, damage);

        _myChar.MechaOutsideAttackRange();

        _myChar.PlayReceiveDamageAnimation();

        if (IsPartBroken())
            DestroyPart();
    }

    public float GetMaxWeight()
    {
        return _data.maxWeight;
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
        EffectsController.Instance.PlayParticlesEffect(_data.destroyParticle, transform.position, transform.forward, out ParticleSystem particle);
        particle.transform.localScale *= 5;
    }

    protected override void DestroyPart()
    {
        base.DestroyPart();
        _myChar.Dead();
    }    

    public BodySO GetData()
    {
        return _data;
    }
}
