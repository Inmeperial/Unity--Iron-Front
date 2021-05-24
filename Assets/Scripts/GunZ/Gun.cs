using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Gun : MonoBehaviour, IGun
{
    public enum GunType
    {
        AssaultRifle,
        Melee,
        Rifle,
        Shotgun
    };

    [Header("Weapon")]
    [SerializeField] protected GunSO _data;

    [Header("DON'T MODIFY BELOW THIS")]
    [SerializeField] protected GunType _gunType;
    protected string _gun;
    [SerializeField] protected int _maxBullets;
    [SerializeField] protected int _availableBullets;
    [SerializeField] protected int _bulletsPerClick;
    [SerializeField] protected int _damage;
    [SerializeField] protected int _critChance;
    [SerializeField] protected int _critMultiplier;
    [SerializeField] protected int _hitChance;
    [SerializeField] protected int _chanceToHitOtherParts;
    [SerializeField] protected int _attackRange;
    [SerializeField] protected int _bodyPartsSelectionQuantity;

    protected RouletteWheel _roulette;
    protected Dictionary<string, int> _critRoulette = new Dictionary<string, int>();
    protected Dictionary<string, int> _hitRoulette = new Dictionary<string, int>();

    protected bool _abilityUsed;

    private readonly int _missHit = 0;
    private readonly int _normalHit = 1;
    private readonly int _criticalHit = 2;
    // Start is called before the first frame update
    public void Start()
    {
        SetGun();
        StartRoulette();
    }

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

    public int GetCritMultiplier()
    {
        return _critMultiplier;
    }

    public int GetHitChance()
    {
        return _hitChance;
    }

    public GunType GetGunType()
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
    public void SetGun()
    {
        _gunType = (GunType)_data.gunType;
        _maxBullets = _data.maxBullets;
        _availableBullets = _maxBullets;
        _bulletsPerClick = _data.bulletsPerClick;
        _damage = _data.damage;
        _critChance = _data.critChance;
        _critMultiplier = _data.critMultiplier;
        _hitChance = _data.hitChance;
        _chanceToHitOtherParts = _data.chanceToHitOtherParts;
        _attackRange = _data.attackRange;
        _bodyPartsSelectionQuantity = _data.bodyPartsSelectionQuantity;
        _abilityUsed = false;

        switch (_gunType)
        {
            case GunType.AssaultRifle:
                _gun = "AssaultRifle";
                break;
            case GunType.Melee:
                _gun = "Melee";
                break;
            case GunType.Rifle:
                _gun = "Rifle";
                break;
            case GunType.Shotgun:
                _gun = "Shotgun";
                break;
        }
    }

    public void ReloadGun()
    {
        _availableBullets = _maxBullets;
        _abilityUsed = false;
    }

    public int BulletsPerClick()
    {
        return _bulletsPerClick;
    }
    public void ReduceAvailableBullets()
    {
        if (_availableBullets > 0)
            _availableBullets -= BulletsPerClick();
    }

    public void IncreaseAvailableBullets()
    {
        if (_availableBullets < _maxBullets)
            _availableBullets += BulletsPerClick();
    }
    public void IncreaseAvailableBullets(int quantity)
    {
        if (_availableBullets < _maxBullets)
            _availableBullets += quantity;
    }


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

                if (c == "Crit")
                {
                    if (AbilityUsed())
                    {
                        t = Tuple.Create((_damage * _critMultiplier) / 2, _criticalHit);
                    }
                    else
                    {
                        t = Tuple.Create(_damage * _critMultiplier, _criticalHit);
                    }
                }
                else if (c == "Normal")
                {
                    if (AbilityUsed())
                    {
                        t = Tuple.Create(_damage / 2, _normalHit);
                    }
                    else
                    {
                        t = Tuple.Create(_damage, _normalHit);
                    }
                }
            }
            else
            {
                t = Tuple.Create(0, _missHit);
            }
            list.Add(t);
        }
        //Returns the damage each bullet will deal.
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

    public bool AbilityUsed()
    {
        return _abilityUsed;
    }
}
