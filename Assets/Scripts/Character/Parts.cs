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

    protected virtual void Start()
    {
        _myChar = transform.parent.GetComponent<Character>();
    }

    public abstract float GetMaxHp();

    public abstract float GetCurrentHp();

    public abstract void UpdateHp(float value);

    public abstract void TakeDamage(List<Tuple<int,int>> damages);
    public abstract void TakeDamage(int damage);
}
