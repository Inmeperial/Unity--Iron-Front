using System;
using System.Collections.Generic;
using UnityEngine;

public class Body : Parts
{
    private float _maxWeight;

    private bool _smokeScreenAvailable;
    private float _smokeScreenHpPercentage;
    private bool _smokeScreenActive;
    public override void SetPart(PartSO data, Equipable.Location location)
    {
        var d = data as BodySO;
        _maxWeight = d.maxWeight;
        
        base.SetPart(data, location);

        if (_ability && _ability.GetAbilityEnum() == Ability.Abilities.SmokeScreen)
        {
            _smokeScreenAvailable = true;
            var smokeData = d.ability as SmokeScreenSO;
            _smokeScreenHpPercentage = smokeData.hpPercentageForSmokeActivation;
        }
        
        if (d.item && d.item.itemPrefab)
        {
            _item = Instantiate(d.item.itemPrefab, _myChar.transform);
            _item.Initialize(_myChar, d.item, location);
            _myChar.AddEquipable(_item);
        }
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

            EffectsController.Instance.PlayParticlesEffect(_particleSpawner[0], EnumsClass.ParticleActionType.Damage);
            //EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Hit);
            int item = damages[i].Item2;
            switch (item)
            {
                case MissHit:
                    EffectsController.Instance.CreateDamageText("Miss", 0, bodyPos, i == damages.Count - 1 ? true : false);
                    break;
                   
                case NormalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 1, bodyPos, i == damages.Count - 1 ? true : false);
                    break;
               
                case CriticalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 2, bodyPos, i == damages.Count - 1 ? true : false);
                    break;
            }
        }
        
        
        
        ui.ContainerActivation(true);
        ui.UpdateBodySlider(total, (int)_currentHP);
        _myChar.MakeNotAttackable();

        CheckSmokeScreen();
        
            
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

        bool isActive = CharacterSelection.Instance.IsActiveCharacter(_myChar);
        if (isActive) ButtonsUIManager.Instance.UpdateBodyHUD(_currentHP);
        
        EffectsController.Instance.PlayParticlesEffect(_particleSpawner[0], EnumsClass.ParticleActionType.Damage);
        //EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Hit);
        
        Vector3 bodyPos = transform.position;
        EffectsController.Instance.CreateDamageText(damage.ToString(), 1, bodyPos);

        ui.ContainerActivation(true);
        ui.UpdateBodySlider(damage, (int)_currentHP);
        _myChar.MakeNotAttackable();
        
        CheckSmokeScreen();
        
        if (_currentHP <= 0)
            _myChar.Dead();
    }

    public void MechaDeath()
    {
        _myChar.NotSelectable();
        //_myChar.effectsController.PlayParticlesEffect(this.gameObject, "Dead");
    }
    
    public float GetMaxWeight()
    {
        return _maxWeight;
    }

    public bool IsSmokeScreenActive()
    {
        return _smokeScreenActive;
    }

    private void CheckSmokeScreen()
    {
        if (_smokeScreenAvailable)
        {
            if (_currentHP > 0)
            {
                var hpPerc = _currentHP * 100 / _maxHP;

                if (hpPerc <= _smokeScreenHpPercentage)
                {
                    Debug.Log("activo smokescreen");
                    _smokeScreenAvailable = false;
                    _smokeScreenActive = true;

                    _ability.Use();
                }
            }  
        }
    }

    public Item GetItem()
    {
        return _item;
    }

    public override void Heal(int healAmount)
    {
        base.Heal(healAmount);
        ButtonsUIManager.Instance.UpdateBodyHUD(_currentHP);
    }
}
