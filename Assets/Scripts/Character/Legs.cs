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

    // Start is called before the first frame update

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

    public void UpdateHP(int newValue)
    {
        _legsHP = newValue;
    }
    
    public float GetLegsMaxHP()
    {
        return _legsMaxHP;
    }

    public float GetLegsHP()
    {
        return _legsHP;
    }

    public int GetLegsInitiative()
    {
        return _initiative;
    }

    public void TakeDamageLegs(int damage)
    {
        var ui = _myChar.GetMyUI();
        ui.SetRightArmSlider(_legsHP);
        var hp = _legsHP - damage;
        _legsHP = hp > 0 ? hp : 0;
        if (_legsHP <= 0 && !_brokenLegs)
        {
            HalfSteps();
            //_myChar.SetCharacterMove(false);
        }
        _myChar.effectsController.PlayParticlesEffect(transform.position, "Damage");
        _myChar.effectsController.CreateDamageText(damage.ToString(), 1, transform.position, true);
        ui.ContainerActivation(true);
        ui.UpdateLegsSlider(damage, (int)_legsHP);
        _myChar.MakeNotAttackable();
    }
    
    public void TakeDamageFromMine(int damage)
    {
        var ui = _myChar.GetMyUI();
        ui.SetLegsSlider(_legsHP);
        ui.ContainerActivation(true);
        ui.UpdateLegsSlider(damage, (int)_legsHP);
        ui.DeactivateWorldUIWithTimer();
        var hp = _legsHP - damage;
        _legsHP = hp > 0 ? hp : 0;
        if (_legsHP <= 0 && !_brokenLegs)
        {
            //_myChar.SetCharacterMove(false);
            HalfSteps();
        }
        _myChar.buttonsManager.UpdateLegsHUD((int)_legsHP, true);
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
