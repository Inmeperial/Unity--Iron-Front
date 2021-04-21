using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : MonoBehaviour, IGun
{
    [Header("Weapon")]
    [SerializeField] private GunSO _data;
    [SerializeField] private int _maxBullets;
    [SerializeField] private int _availableBullets;
    [SerializeField] private int _damage;
    [SerializeField] private int _attackRange;
    [SerializeField] private int _bodyPartsSelectionQuantity;
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

    public void ReduceAvailableBullets(int quantity)
    {
        Debug.Log("reduzco balas");
        if (_availableBullets > 0)
            _availableBullets -= quantity;
    }

    public void IncreaseAvailableBullets(int quantity)
    {
        if (_availableBullets < _maxBullets)
            _availableBullets += quantity;
    }


    public int AvailableSelections()
    {
        return _bodyPartsSelectionQuantity;
    }
    public void SetGun()
    {
        _maxBullets = _data.maxBullets;
        _availableBullets = _maxBullets;
        _damage = _data.damage;
        _attackRange = _data.attackRange;
        _bodyPartsSelectionQuantity = _data.bodyPartsSelectionQuantity;
    }
}
