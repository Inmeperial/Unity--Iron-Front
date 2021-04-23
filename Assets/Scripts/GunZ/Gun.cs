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
    [SerializeField] protected GunType _gunType;
    [SerializeField] protected GunSO _data;
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
    // Start is called before the first frame update
    public void Start()
    {
        SetGun();
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

    public GunType GetGunType()
    {
        return _gunType;
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
    }

    public void ReloadGun()
    {
        _availableBullets = _maxBullets;
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


    public int[] DamageCalculation(int bullets)
    {
        var damage = new int[bullets];

        for (int i = 0; i < bullets; i++)
        {
            //Determines if bullet hits.
            var h = _roulette.ExecuteAction(_hitRoulette);

            if (h == "Hit")
            {
                //Determines if it crits or not.
                var c = _roulette.ExecuteAction(_critRoulette);

                if (c == "Crit")
                {
                    damage[i] = _damage * _critMultiplier;
                }
                else if (c == "Normal")
                {
                    damage[i] = _damage;
                }
            }
            else if (h == "Miss")
            {
                damage[i] = 0;
            }
        }
        //Returns the damage each bullet will deal.
        return damage;
    }

    public virtual void StartRoulette()
    {
        _roulette = new RouletteWheel();
        _critRoulette.Add("Crit", _critChance);
        var c = 100 - _critChance;
        _critRoulette.Add("Normal", c > 0 ? c : 0);

        _hitRoulette.Add("Hit", _hitChance);
        var h = 100 - _critChance;
        _hitRoulette.Add("Miss", h > 0 ? h : 0);
    }

    public abstract void Ability();
}
