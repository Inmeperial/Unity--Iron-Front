﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legs : MonoBehaviour
{
    public LegsSO legsData;
    private int _legsMaxHP;
    private int _legsHP;
    
    private int _maxSteps;
    
    private float _moveSpeed;
    private float _rotationSpeed;

    private Character _myChar;
    // Start is called before the first frame update

    private void Awake()
    {
        _myChar = GetComponent<Character>();
        _legsMaxHP = legsData.maxHP;
        _legsHP = _legsMaxHP;
        _maxSteps = legsData.maxSteps;
        _moveSpeed = legsData.moveSpeed;
        _rotationSpeed = legsData.rotationSpeed;
    }

    void Start()
    {
        _myChar = transform.parent.GetComponent<Character>();
    }
    public int GetMaxSteps()
    {
        return _maxSteps;
    }

    public void UpdateHP(int newValue)
    {
        _legsHP = newValue;
    }
    
    public int GetLegsMaxHP()
    {
        return _legsMaxHP;
    }

    public int GetLegsHP()
    {
        return _legsHP;
    }
    
    public void TakeDamageLegs(int damage)
    {
        var ui = _myChar.GetMyUI();
        ui.SetRightArmSlider(_legsHP);
        var hp = _legsHP - damage;
        _legsHP = hp > 0 ? hp : 0;
        if (_legsHP <= 0)
        {
            _myChar.SetCharacterMove(false);
        }
        _myChar.effectsController.PlayParticlesEffect(transform.position, "Damage");
        _myChar.effectsController.CreateDamageText(damage.ToString(), 1, transform.position, true);
        ui.ContainerActivation(true);
        ui.UpdateLeftArmSlider(damage, _legsHP);
        _myChar.MakeNotAttackable();
    }
    
    public void TakeDamageFromMine(int damage)
    {
        var ui = _myChar.GetMyUI();
        ui.SetLegsSlider(_legsHP);
        ui.ContainerActivation(true);
        ui.UpdateLegsSlider(damage, _legsHP);
        ui.DeactivateWorldUIWithTimer();
        var hp = _legsHP - damage;
        _legsHP = hp > 0 ? hp : 0;
        if (_legsHP <= 0)
        {
            _myChar.SetCharacterMove(false);
        }
        _myChar.buttonsManager.UpdateLegsHUD(_legsHP, true);
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
}
