﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Gun : EnumsClass, IGun
{
    [SerializeField] protected GameObject[] _models;
    protected Character _myChar;
    protected float _maxHP;
    protected float _currentHP;
    protected GunsType _gunType;
    protected string _gun;
    protected Sprite _icon;
    protected int _maxBullets;
    protected int _availableBullets;
    protected int _bulletsPerClick;
    protected int _damage;
    protected int _critChance;
    protected float _critMultiplier;
    protected int _hitChance;
    protected int _chanceToHitOtherParts;
    protected int _attackRange;
    protected int _bodyPartsSelectionQuantity;

    protected RouletteWheel _roulette;
    protected Dictionary<string, int> _critRoulette = new Dictionary<string, int>();
    protected Dictionary<string, int> _hitRoulette = new Dictionary<string, int>();

    //Nada que ver con la habilidad que se le equipa
    protected bool _gunSkill;
    protected string _location;

    private const int MissHit = 0;
    private const int NormalHit = 1;
    private const int CriticalHit = 2;

    protected Ability _ability;
    
    [SerializeField] protected GameObject _particleSpawn;
    public void SetRightOrLeft(string location)
    {
        _location = location;
    }
    
    # region Getters
    public int GetMaxBullets()
    {
        return _maxBullets;
    }

    public int GetAvailableBullets()
    {
        return _availableBullets;
    }

    public int GetBulletDamage()
    {
        return _damage;
    }

    public int GetAttackRange()
    {
        return _attackRange;
    }

    public int GetCritChance()
    {
        return _critChance;
    }

    public float GetCritMultiplier()
    {
        return _critMultiplier;
    }

    public int GetHitChance()
    {
        return _hitChance;
    }

    public GunsType GetGunType()
    {
        return _gunType;
    }

    public string GetGunTypeString()
    {
        return _gun;
    }

    public int GetAvailableSelections()
    {
        return _bodyPartsSelectionQuantity;
    }
    
    public int GetBulletsPerClick()
    {
        return _bulletsPerClick;
    }

    public Sprite GetIcon()
    {
        return _icon;
    }
    #endregion
    
    /// <summary>
    /// Set Gun stats from given scriptable object.
    /// </summary>
    public virtual void SetGun(GunSO data)
    {
        _maxHP = data.maxHp;
        _currentHP = _maxHP;
        _icon = data.gunImage;
        _maxBullets = data.maxBullets;
        _availableBullets = _maxBullets;
        _bulletsPerClick = data.bulletsPerClick;
        _damage = data.damage;
        _critChance = data.critChance;
        _critMultiplier = data.critMultiplier;
        _hitChance = data.hitChance;
        _chanceToHitOtherParts = data.chanceToHitOtherParts;
        _attackRange = data.attackRange;
        _bodyPartsSelectionQuantity = data.bodyPartsSelectionQuantity;
        _gunSkill = false;
        _ability = data.ability.abilityPrefab;
    }

    public void ReloadGun()
    {
        _availableBullets = _maxBullets;
        _gunSkill = false;
    }

    
    /// <summary>
    /// Reduce this gun available bullets by the amount of this gun bullets per click.  
    /// </summary>
    public void ReduceAvailableBullets()
    {
        if (_availableBullets > 0)
            _availableBullets -= GetBulletsPerClick();
    }

    /// <summary>
    /// Increase this gun available bullets by the amount of this gun bullets per click.  
    /// </summary>
    public void IncreaseAvailableBullets()
    {
        if (_availableBullets < _maxBullets)
            _availableBullets += GetBulletsPerClick();
    }
    
    /// <summary>
    /// Increase this gun available bullets by the given quantity.
    /// </summary>
    public void IncreaseAvailableBullets(int quantity)
    {
        if (_availableBullets < _maxBullets)
            _availableBullets += quantity;
    }

    /// <summary>
    /// Returns a collection of the damage each bullet does and if it miss, hits or crit.
    /// </summary>
    public List<Tuple<int, int>> DamageCalculation(int bullets)
    {
        List<Tuple<int, int>> list = new List<Tuple<int, int>>();

        for (int i = 0; i < bullets; i++)
        {
            Tuple<int, int> t = null;
            //Determines if bullet hits.
            string h = _roulette.ExecuteAction(_hitRoulette);

            //MISS == 0
            //HIT == 1
            //CRIT == 2
            if (h == "Hit")
            {
                //Determines if it crits or not.
                string c = _roulette.ExecuteAction(_critRoulette);

                switch (c)
                {
                    case "Crit" when _gunSkill:
                        t = Tuple.Create((int)(_damage * _critMultiplier) / 2, CriticalHit);
                        break;
                    case "Crit":
                        t = Tuple.Create((int)(_damage * _critMultiplier), CriticalHit);
                        break;
                    case "Normal" when _gunSkill:
                        t = Tuple.Create(_damage / 2, NormalHit);
                        break;
                    case "Normal":
                        t = Tuple.Create(_damage, NormalHit);
                        break;
                }
            }
            else
            {
                t = Tuple.Create(0, MissHit);
            }
            list.Add(t);
        }
        return list;
    }

    public virtual void StartRoulette()
    {
        _roulette = new RouletteWheel();
        _critRoulette = new Dictionary<string, int>();
        _critRoulette.Add("Crit", _critChance);
        int c = 100 - _critChance;
        _critRoulette.Add("Normal", c > 0 ? c : 0);

        _hitRoulette = new Dictionary<string, int>();
        _hitRoulette.Add("Hit", _hitChance);
        int h = 100 - _hitChance;
        _hitRoulette.Add("Miss", h > 0 ? h : 0);
    }

    public abstract void Ability();

    public abstract void Deselect();

    /// <summary>
    /// Return true if the Gun ability was used, false if not.
    /// </summary>
    public bool SkillUsed()
    {
        return _gunSkill;
    }

    public void TurnOff()
    {
        Destroy(gameObject);
        // MeshRenderer[] childs = transform.GetComponentsInChildren<MeshRenderer>();
        // foreach (MeshRenderer m in childs)
        // {
        //     if (m) m.enabled = false;
        // }
        // GetComponent<BoxCollider>().enabled = false;
    }

    public GameObject GetParticleSpawn()
    {
        return _particleSpawn;
    }

    public void ModelsOff()
    {
        foreach (GameObject m in _models)
        {
            m.gameObject.SetActive(false);
        }
    }
    
    public void ModelsOn()
    {
        foreach (GameObject m in _models)
        {
            m.gameObject.SetActive(true);
        }
    }
    
    //Lo ejecuta el ButtonsUIManager, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public void TakeDamage(List<Tuple<int,int>> damages)
    {
        if (_currentHP <= 0) return;
        
        int total = 0;
        Vector3 pos = transform.position;
        for (int i = 0; i < damages.Count; i++)
        {
            total += damages[i].Item1;
            float hp = _currentHP - damages[i].Item1;
            _currentHP = hp > 0 ? hp : 0;
            EffectsController.Instance.PlayParticlesEffect(this.gameObject, EnumsClass.ParticleActionType.Damage);
            EffectsController.Instance.PlayParticlesEffect(this.gameObject, EnumsClass.ParticleActionType.Hit);

            int item = damages[i].Item2;
            switch (item)
            {
                case MissHit:
                    EffectsController.Instance.CreateDamageText("Miss", 0, pos, i == damages.Count - 1 ? true : false);
                    break;
                   
                case NormalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 1, pos, i == damages.Count - 1 ? true : false);
                    break;
               
                case CriticalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 2, pos, i == damages.Count - 1 ? true : false);
                    break;
            }
        }
        
        WorldUI ui = _myChar.GetMyUI();
        ui.ContainerActivation(true);
        
        switch (_location)
        {
            case "Left":
                ui.SetLeftArmSlider(_currentHP);
                ui.UpdateLeftArmSlider(total, (int)_currentHP);
                break;
            
            case "Right":
                ui.SetRightArmSlider(_currentHP);
                ui.UpdateRightArmSlider(total, (int)_currentHP);
                break;
        }
        if (_currentHP <= 0)
        {
            switch (_location)
            {
                case "Left":
                    _myChar.GetLeftGun().TurnOff();
                    break;
                case "Right":
                    _myChar.GetRightGun().TurnOff();
                    break;
            }
        }
        _myChar.MakeNotAttackable();
    }
    
    //Lo ejecuta el mortero, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public void TakeDamage(int damage)
    {
        if (_currentHP <= 0) return;
        
        float hp = _currentHP - damage;
        _currentHP = hp > 0 ? hp : 0;

        EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Damage);
        EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Hit);

        Vector3 pos = transform.position;
        EffectsController.Instance.CreateDamageText(damage.ToString(), 1, pos, true);
        
        WorldUI ui = _myChar.GetMyUI();
        ui.ContainerActivation(true);

        bool isActive = CharacterSelection.Instance.IsActiveCharacter(_myChar);
        
        if (_location == "Left")
        {
            ui.SetLeftArmSlider(_currentHP);
            ui.UpdateLeftArmSlider(damage, (int) _currentHP);

            if (isActive) ButtonsUIManager.Instance.UpdateLeftArmHUD(_currentHP);
        }
        else
        {
            ui.SetRightArmSlider(_currentHP);
            ui.UpdateRightArmSlider(damage, (int) _currentHP);

            if (isActive) ButtonsUIManager.Instance.UpdateRightArmHUD(_currentHP);
        }

        if (_currentHP <= 0)
        {
            if (_location == "Left")
                _myChar.GetLeftGun().TurnOff();
            else _myChar.GetRightGun().TurnOff();
        }
        _myChar.MakeNotAttackable();
    }
    
    public float GetMaxHp()
    {
        return _maxHP;
    }

    public float GetCurrentHp()
    {
        return _currentHP;
    }
}
