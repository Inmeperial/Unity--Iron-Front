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

    void SetGun();

    int GetAvailableSelections();

    int BulletsPerClick();

    int DamageCalculation();
}
