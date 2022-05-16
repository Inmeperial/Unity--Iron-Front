using System;
using System.Collections.Generic;
using UnityEngine;

public class Legs : Parts
{
    private int _maxSteps;

    private float _moveSpeed;
    private float _rotationSpeed;

    private int _initiative;

    private bool _brokenLegs = false;

    public int GetMaxSteps()
    {
        return _maxSteps;
    }

    public override void SetPart(Character character, PartSO data, Color partColor, Equipable.Location location)
    {
        base.SetPart(character, data, partColor, location);
        LegsSO legsData = data as LegsSO;
        _maxSteps = legsData.maxSteps;
        _moveSpeed = legsData.moveSpeed;
        _rotationSpeed = legsData.rotationSpeed;
        _initiative = legsData.initiative;
    }

    public int GetLegsInitiative()
    {
        return _initiative;
    }

    //Lo ejecuta el ButtonsUIManager, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public override void TakeDamage(List<Tuple<int, int>> damages)
    {
        if (_currentHP <= 0) return;


        WorldUI ui = _myChar.GetMyUI();
        ui.SetLegsSlider(_currentHP);
        int total = 0;
        
        Vector3 legsMidPosition = Vector3.Lerp(_particleSpawner[0].transform.position, _particleSpawner[1].transform.position, 0.5f);
        for (int i = 0; i < damages.Count; i++)
        {
            total += damages[i].Item1;
            float hp = _currentHP - damages[i].Item1;
            _currentHP = hp > 0 ? hp : 0;
            //_myChar.SetCharacterMove(_currentHP > 0 ? true : false);
            EffectsController.Instance.PlayParticlesEffect(_particleSpawner[0], EnumsClass.ParticleActionType.Damage);
            EffectsController.Instance.PlayParticlesEffect(_particleSpawner[1], EnumsClass.ParticleActionType.Damage);
            //EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Hit);
            int item = damages[i].Item2;
            switch (item)
            {
                case MissHit:
                    EffectsController.Instance.CreateDamageText("Miss", 0, legsMidPosition, i == damages.Count - 1 ? true : false);
                    break;

                case NormalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 1, legsMidPosition, i == damages.Count - 1 ? true : false);
                    break;

                case CriticalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 2, legsMidPosition, i == damages.Count - 1 ? true : false);
                    break;
            }
        }
        ui.Show();
        ui.UpdateLegsSlider(total, _currentHP);
        _myChar.MakeNotAttackable();
        TurnManager.Instance.ReducePosition(_myChar);
        
        if (_currentHP <= 0 && !_brokenLegs)
        {
            EffectsController.Instance.PlayParticlesEffect(_particleSpawner[0], EnumsClass.ParticleActionType.Damage);
            EffectsController.Instance.PlayParticlesEffect(_particleSpawner[1], EnumsClass.ParticleActionType.Damage);
            HalfSteps();
        }
    }

    public override void TakeDamage(int damage)
    {
        if (_currentHP <= 0) return;


        WorldUI ui = _myChar.GetMyUI();
        ui.SetLegsSlider(_currentHP);
        ui.Show();
        ui.UpdateLegsSlider(damage, _currentHP);
        ui.HideWithTimer();
        float hp = _currentHP - damage;
        _currentHP = hp > 0 ? hp : 0;
        if (_currentHP <= 0 && !_brokenLegs)
        {
            EffectsController.Instance.PlayParticlesEffect(_particleSpawner[0], EnumsClass.ParticleActionType.Mine);
            EffectsController.Instance.PlayParticlesEffect(_particleSpawner[1], EnumsClass.ParticleActionType.Mine);
            HalfSteps();
        }

        bool isActive = CharacterSelection.Instance.IsActiveCharacter(_myChar);

        if (isActive) ButtonsUIManager.Instance.UpdateLegsHUD(_currentHP);

        EffectsController.Instance.PlayParticlesEffect(_particleSpawner[0], EnumsClass.ParticleActionType.Damage);
        EffectsController.Instance.PlayParticlesEffect(_particleSpawner[1], EnumsClass.ParticleActionType.Damage);
        //EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Hit);
        
        Vector3 legsMidPosition = Vector3.Lerp(_particleSpawner[0].transform.position, _particleSpawner[1].transform.position, 0.5f);
        EffectsController.Instance.CreateDamageText(damage.ToString(), 1,legsMidPosition);
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
    
    public override void Heal(int healAmount)
    {
        base.Heal(healAmount);
        Vector3 legsMidPosition = Vector3.Lerp(_particleSpawner[0].transform.position, _particleSpawner[1].transform.position, 0.5f);
        EffectsController.Instance.CreateDamageText(healAmount.ToString(), 3, legsMidPosition);
        ButtonsUIManager.Instance.UpdateLegsHUD(_currentHP);
    }
}
