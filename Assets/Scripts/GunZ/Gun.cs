using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Gun : EnumsClass, IGun
{
    [SerializeField] protected GunSO _weaponData;
    
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

    protected bool _abilityUsed;
    protected string _location;

    private const int MissHit = 0;
    private const int NormalHit = 1;
    private const int CriticalHit = 2;

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
    public virtual void SetGun()
    {
        _gunType = (GunsType)_weaponData.gunType;
        _icon = _weaponData.gunImage;
        _maxBullets = _weaponData.maxBullets;
        _availableBullets = _maxBullets;
        _bulletsPerClick = _weaponData.bulletsPerClick;
        _damage = _weaponData.damage;
        _critChance = _weaponData.critChance;
        _critMultiplier = _weaponData.critMultiplier;
        _hitChance = _weaponData.hitChance;
        _chanceToHitOtherParts = _weaponData.chanceToHitOtherParts;
        _attackRange = _weaponData.attackRange;
        _bodyPartsSelectionQuantity = _weaponData.bodyPartsSelectionQuantity;
        _abilityUsed = false;

        switch (_gunType)
        {
            case GunsType.None:
                break;
            case GunsType.AssaultRifle:
                _gun = "AssaultRifle";
                break;
            case GunsType.Melee:
                _gun = "Melee";
                break;
            case GunsType.Rifle:
                _gun = "Rifle";
                break;
            case GunsType.Shotgun:
                _gun = "Shotgun";
                break;
            case GunsType.Shield:
                _gun = "Shield";
                break;
        }
    }

    public void ReloadGun()
    {
        _availableBullets = _maxBullets;
        _abilityUsed = false;
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
        var list = new List<Tuple<int, int>>();
        var damage = new int[bullets];

        for (int i = 0; i < bullets; i++)
        {
            Tuple<int, int> t = null;
            //Determines if bullet hits.
            var h = _roulette.ExecuteAction(_hitRoulette);

            //MISS == 0
            //HIT == 1
            //CRIT == 2
            if (h == "Hit")
            {
                //Determines if it crits or not.
                var c = _roulette.ExecuteAction(_critRoulette);

                switch (c)
                {
                    case "Crit" when _abilityUsed:
                        t = Tuple.Create((int)(_damage * _critMultiplier) / 2, CriticalHit);
                        break;
                    case "Crit":
                        t = Tuple.Create((int)(_damage * _critMultiplier), CriticalHit);
                        break;
                    case "Normal" when _abilityUsed:
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
        _critRoulette.Add("Crit", _critChance);
        var c = 100 - _critChance;
        _critRoulette.Add("Normal", c > 0 ? c : 0);

        _hitRoulette.Add("Hit", _hitChance);
        var h = 100 - _hitChance;
        _hitRoulette.Add("Miss", h > 0 ? h : 0);
    }

    public abstract void Ability();

    public abstract void Deselect();

    /// <summary>
    /// Return true if the Gun ability was used, false if not.
    /// </summary>
    public bool AbilityUsed()
    {
        return _abilityUsed;
    }

    public void TurnOff()
    {
        var childs = transform.GetComponentsInChildren<MeshRenderer>();
        foreach (var m in childs)
        {
            if (m) m.enabled = false;
        }
        GetComponent<BoxCollider>().enabled = false;
    }

    public GameObject GetParticleSpawn()
    {
        return _particleSpawn;
    }
}
