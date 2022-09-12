using System;
using System.Collections.Generic;

public interface IDamageable
{
    void ReceiveDamage(List<Tuple<int, int>> damages);

    void ReceiveDamage(int damage);
}