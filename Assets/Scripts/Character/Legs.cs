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

    private GameObject _otherLeg;
    public int GetMaxSteps()
    {
        return _maxSteps;
    }

    public override void SetPart(PartSO data, Equipable.Location location)
    {
        base.SetPart(data, location);
        var d = data as LegsSO;
        _maxSteps = d.maxSteps;
        _moveSpeed = d.moveSpeed;
        _rotationSpeed = d.rotationSpeed;
        _initiative = d.initiative;
    }

    public override void UpdateHp(float newValue)
    {
        _currentHP = newValue;
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
        Vector3 legsPos = transform.position;
        for (int i = 0; i < damages.Count; i++)
        {
            total += damages[i].Item1;
            float hp = _currentHP - damages[i].Item1;
            UpdateHp(hp > 0 ? hp : 0);
            //_myChar.SetCharacterMove(_currentHP > 0 ? true : false);
            EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Damage);
            EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Hit);
            int item = damages[i].Item2;
            switch (item)
            {
                case MissHit:
                    EffectsController.Instance.CreateDamageText("Miss", 0, legsPos, i == damages.Count - 1 ? true : false);
                    break;

                case NormalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 1, legsPos, i == damages.Count - 1 ? true : false);
                    break;

                case CriticalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 2, legsPos, i == damages.Count - 1 ? true : false);
                    break;
            }
        }
        ui.ContainerActivation(true);
        ui.UpdateLegsSlider(total, _currentHP);
        _myChar.MakeNotAttackable();
        TurnManager.Instance.ReducePosition(_myChar);
    }

    public override void TakeDamage(int damage)
    {
        if (_currentHP <= 0) return;


        WorldUI ui = _myChar.GetMyUI();
        ui.SetLegsSlider(_currentHP);
        ui.ContainerActivation(true);
        ui.UpdateLegsSlider(damage, _currentHP);
        ui.DeactivateWorldUIWithTimer();
        float hp = _currentHP - damage;
        _currentHP = hp > 0 ? hp : 0;
        if (_currentHP <= 0 && !_brokenLegs)
        {
            HalfSteps();
        }

        bool isActive = CharacterSelection.Instance.IsActiveCharacter(_myChar);

        if (isActive) ButtonsUIManager.Instance.UpdateLegsHUD(_currentHP);

        EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Damage);
        EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Hit);
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

    public Character GetCharacter()
    {
        if (!_myChar) Debug.Log("sin char");
        return _myChar;
    }

    public void SetOtherLeg(GameObject leg)
    {
        _otherLeg = leg;
    }
}
