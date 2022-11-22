using System;
using System.Collections.Generic;

public interface IGun
{
    int GetMaxBullets();

    int GetAvailableBullets();

    int GetBulletDamage();

    int GetAttackRange();

    void ReduceAvailableBullets();

    void IncreaseAvailableBullets();
    void IncreaseAvailableBullets(int quantity);

    void SetGun(GunSO data, Character character);

    int GetAvailableSelections();

    int GetBulletsPerClick();

    List<Tuple<int, int>> DamageCalculation(int bullets);

    void Ability();
}
