using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Parts : MonoBehaviour
{
    protected const int MissHit = 0;
    protected const int NormalHit = 1;
    protected const int CriticalHit = 2;
    protected Character _myChar;
    public AbilitySO abilityData;
    public Ability abilityPrefab;
    protected Ability _ability;
    protected float _maxHP;
    protected float _currentHP;
    protected virtual void Start()
    {
        _myChar = transform.parent.GetComponent<Character>();

        if (abilityData)
        {
            switch (abilityData.abilityType)
            {
                case AbilitySO.AbilityType.LegsOvercharge:
                    _ability = Instantiate(abilityPrefab, transform);
                    break;
                case AbilitySO.AbilityType.Push:
                    _ability = Instantiate(abilityPrefab, transform);
                    break;
                case AbilitySO.AbilityType.Pull:
                    break;
            }
            _ability.Initialize(_myChar, abilityData);
            _myChar.equipables.Add(_ability);
        }
    }

    public abstract void SetPart();

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
