﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Body : Parts
{
    private float _maxWeight;

    private bool _smokeScreenAvailable;
    private float _smokeScreenHpPercentage;
    private bool _smokeScreenActive;
    public override void SetPart(Character character, PartSO data, Color partColor, Equipable.Location location)
    {
        base.SetPart(character, data, partColor, location);

        BodySO bodyData = data as BodySO;
        _maxWeight = bodyData.maxWeight;
        
        

        //if (_ability && _ability.GetAbilityEnum() == Ability.Abilities.SmokeScreen)
        //{
        //    _smokeScreenAvailable = true;
        //    var smokeData = d.ability as SmokeScreenSO;
        //    _smokeScreenHpPercentage = smokeData.hpPercentageForSmokeActivation;
        //}
        
        //if (d.item && d.item.itemPrefab)
        //{
        //    _item = Instantiate(d.item.itemPrefab, _myChar.transform);
        //    _item.Initialize(_myChar, d.item, location);
        //    _myChar.AddEquipable(_item);
        //}
    }


    //Lo ejecuta el ButtonsUIManager, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public override void TakeDamage(List<Tuple<int,int>> damages)
    {
        if (_currentHP <= 0)
            return;
        
        WorldUI ui = _myChar.GetMyUI();
        ui.SetBodySlider(_currentHP);

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
        
        
        
        ui.Show();
        ui.UpdateBodySlider(total, (int)_currentHP);

        _myChar.MakeNotAttackable();

        CheckSmokeScreen();
            
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

        bool isActive = CharacterSelection.Instance.IsActiveCharacter(_myChar);

        if (isActive)
            ButtonsUIManager.Instance.UpdateBodyHUD(_currentHP);

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
        
        if (_currentHP <= 0)
            _myChar.Dead();
    }

    public float GetMaxWeight() => _maxWeight;

    public bool IsSmokeScreenActive() => _smokeScreenActive;

    private void CheckSmokeScreen()
    {
        if (!_smokeScreenAvailable)
            return;

        if (_currentHP <= 0)
            return;

        float hpPercentage = _currentHP * 100 / _maxHP;

        if (hpPercentage > _smokeScreenHpPercentage)
            return;

        Debug.Log("activo smokescreen");
        _smokeScreenAvailable = false;
        _smokeScreenActive = true;

        _ability.Use();
    }

    public Item GetItem() => _item;

    public override void Heal(int healAmount)
    {
        base.Heal(healAmount);

        EffectsController.Instance.CreateDamageText(healAmount.ToString(), 3, transform.position);

        ButtonsUIManager.Instance.UpdateBodyHUD(_currentHP);
    }

    public void DeactivateSmokeScreen()
    {
        _smokeScreenActive = false;
        _ability.Deselect();
    }
}
