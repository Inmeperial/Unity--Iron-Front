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
    protected Item _item;
    protected float _maxHP;
    protected float _currentHP;
    protected float _weight;
    protected List<GameObject> _particleSpawner = new List<GameObject>();
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
        
        //if(data.ability && data.ability.abilityPrefab)
        //{
        //    _ability = Instantiate(data.ability.abilityPrefab, _myChar.transform);
        //    _ability.Initialize(_myChar, data.ability, location);
        //    _myChar.AddEquipable(_ability);
        //}
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

    public virtual void Heal(int healAmount)
    {
        if (_currentHP >= _maxHP)
        {
            healAmount = (int)_maxHP - (int)_currentHP;
            _currentHP = _maxHP;
        }
        else _currentHP += healAmount;
    }

    public Ability GetAbility()
    {
        return _ability;
    }

    public void SetParticleSpawner(GameObject spawner)
    {
        _particleSpawner.Add(spawner);
    }

    public Character GetCharacter()
    {
        return _myChar;
    }
}
