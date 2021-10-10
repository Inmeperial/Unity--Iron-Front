using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun
{
    int GetMaxBullets();

    int GetAvailableBullets();

    int GetBulletDamage();

    int GetAttackRange();

    void ReduceAvailableBullets();

    void IncreaseAvailableBullets();
    void IncreaseAvailableBullets(int quantity);

    void SetGun(GunSO data);

    int GetAvailableSelections();

    int GetBulletsPerClick();

    List<Tuple<int, int>> DamageCalculation(int bullets);

    void Ability();
}
