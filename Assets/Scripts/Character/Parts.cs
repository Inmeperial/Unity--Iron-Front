using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Parts : MonoBehaviour
{
    public MeshFilter[] meshFilter;
    protected const int MissHit = 0;
    protected const int NormalHit = 1;
    protected const int CriticalHit = 2;
    protected Character _myChar;
    protected Ability _ability;
    protected float _maxHP;
    protected float _currentHP;
    public virtual void ManualStart(Character character)
    {
        _myChar = character;
    }

    public virtual void SetPart(PartSO data)
    {
        _maxHP = data.maxHP;
        _currentHP = _maxHP;

        if(data.ability && data.ability.abilityPrefab)
        {
            _ability = Instantiate(data.ability.abilityPrefab, transform);
            _ability.Initialize(_myChar, data.ability);
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

    public abstract void UpdateHp(float value);

    public abstract void TakeDamage(List<Tuple<int, int>> damages);

    public abstract void TakeDamage(int damage);

    public Ability GetAbility()
    {
        return _ability;
    }
}
