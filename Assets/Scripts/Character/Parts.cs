using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Parts : MonoBehaviour
{
    //public MeshFilter[] meshFilter;
    protected const int MissHit = 0;
    protected const int NormalHit = 1;
    protected const int CriticalHit = 2;
    protected Character _myChar;
    protected Ability _ability;
    protected float _maxHP;
    protected float _currentHP;
    protected float _weight;
    public virtual void ManualStart(Character character)
    {
        _myChar = character;
    }

    public virtual void SetPart(PartSO data, Equipable.Location location)
    {
        _maxHP = data.maxHP;
        _currentHP = _maxHP;
        _weight = data.weight;
        
        if (!_myChar) return;
        
        if(data.ability && data.ability.abilityPrefab)
        {
            _ability = Instantiate(data.ability.abilityPrefab, transform.GetChild(0));
            _ability.Initialize(_myChar, data.ability, location);
            _myChar.AddEquipable(_ability);
        }
    }

    public float GetMaxHp()
    {
        return _maxHP;
    }

    public float GetCurrentHp()
    {
        return _currentHP;
    }
    
    public float GetWeight()
    {
        return _weight;
    }

    public abstract void TakeDamage(List<Tuple<int, int>> damages);

    public abstract void TakeDamage(int damage);

    public Ability GetAbility()
    {
        return _ability;
    }
}
