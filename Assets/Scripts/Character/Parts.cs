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
    public AbilitySO abilitySO;
    public Ability abilityPrefab;
    protected Ability _ability; 
    protected virtual void Start()
    {
        _myChar = transform.parent.GetComponent<Character>();

        if (abilitySO)
        {
            Debug.Log("agrego habilidad");

            switch (abilitySO.abilityType)
            {
                case AbilitySO.AbilityType.LegsOvercharge:
                    _ability = Instantiate(abilityPrefab, transform);
                    break;
            }
            _ability.Initialize(_myChar, abilitySO);
            _myChar.equipables.Add(_ability);
        }
    }

    public abstract float GetMaxHp();

    public abstract float GetCurrentHp();

    public abstract void UpdateHp(float value);

    public abstract void TakeDamage(List<Tuple<int,int>> damages);
    public abstract void TakeDamage(int damage);
    public Ability GetAbility()
    {
        return _ability;
    }
}
