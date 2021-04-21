using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun
{
    int GetMaxBullets();

    int GetAvailableBullets();

    int GetBulletDamage();

    int GetAttackRange();

    void ReduceAvailableBullets(int quantity);

    void IncreaseAvailableBullets(int quantity);

    void SetGun();

    int AvailableSelections();
}
