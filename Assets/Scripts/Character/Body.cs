using System;
using System.Collections.Generic;
using UnityEngine;

public class Body : Parts
{
    private float _maxWeight;

    private bool _smokeScreenAvailable;
    private float _smokeScreenHpPercentage;
    private bool _smokeScreenActive;

    public override void SetPartData(Character character, PartSO data, Color partColor)
    {
        base.SetPartData(character, data, partColor);

        BodySO bodyData = data as BodySO;
        _maxWeight = bodyData.maxWeight;
    }
    //Lo ejecuta el ButtonsUIManager, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public override void TakeDamage(List<Tuple<int,int>> damages)
    {
        if (_currentHP <= 0)
            return;
        
        WorldUI worldUI = _myChar.GetMyUI();
        worldUI.SetBodySlider(_currentHP);

        int total = 0;

        for (int i = 0; i < damages.Count; i++)
        {
            total += damages[i].Item1;
            float hp = _currentHP - damages[i].Item1;
            _currentHP = hp > 0 ? hp : 0;

            foreach (GameObject spawner in _particleSpawner)
            {
                EffectsController.Instance.PlayParticlesEffect(spawner, EnumsClass.ParticleActionType.Damage);
            }

            //EffectsController.Instance.PlayParticlesEffect(_particleSpawner[0], EnumsClass.ParticleActionType.Damage);
            //EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Hit);
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

        worldUI.Show();
        worldUI.UpdateBodySlider(total, (int)_currentHP);

        _myChar.MakeNotAttackable();

        CheckSmokeScreen();

        _myChar.SetHurtAnimation();

        if (_myChar.IsSelected())
            OnHealthChanged?.Invoke(_currentHP);

        if (_currentHP <= 0)
            _myChar.Dead();
    }
    
    //Lo ejecuta el mortero, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public override void TakeDamage(int damage)
    {
        if (_currentHP <= 0)
            return;
        
        
        WorldUI ui = _myChar.GetMyUI();
        ui.SetBodySlider(_currentHP);

        float hp = _currentHP - damage;
        _currentHP = hp > 0 ? hp : 0;

        if (_myChar.IsSelected())
            OnHealthChanged?.Invoke(_currentHP);

        foreach (GameObject spawner in _particleSpawner)
        {
            EffectsController.Instance.PlayParticlesEffect(spawner, EnumsClass.ParticleActionType.Damage);
        }
        //EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Hit);

        EffectsController.Instance.CreateDamageText(damage.ToString(), 1, transform.position);

        ui.Show();
        ui.UpdateBodySlider(damage, (int)_currentHP);

        _myChar.MakeNotAttackable();
        
        CheckSmokeScreen();
        _myChar.SetHurtAnimation();

        if (CurrentHP <= 0)
            _myChar.Dead();
    }

    public float GetMaxWeight() => _maxWeight;

    public bool IsSmokeScreenActive() => _smokeScreenActive;

    private void CheckSmokeScreen()
    {
        if (!_smokeScreenAvailable)
            return;

        if (CurrentHP <= 0)
            return;

        float hpPercentage = _currentHP * 100 / _maxHP;

        if (hpPercentage > _smokeScreenHpPercentage)
            return;

        Debug.Log("activo smokescreen");
        _smokeScreenAvailable = false;
        _smokeScreenActive = true;

        _ability.Use();
    }

    public override void Heal(int healAmount)
    {
        base.Heal(healAmount);

        EffectsController.Instance.CreateDamageText(healAmount.ToString(), 3, transform.position);

        if (_myChar.IsSelected())
            OnHealthChanged?.Invoke(_currentHP);
    }

    public void ConfigureSmokeScreen(float percentageToActivate)
    {
        _smokeScreenHpPercentage = percentageToActivate;
        _smokeScreenAvailable = true;
    }
    public void DeactivateSmokeScreen()
    {
        _smokeScreenActive = false;
        _ability.Deselect();
    }
}
