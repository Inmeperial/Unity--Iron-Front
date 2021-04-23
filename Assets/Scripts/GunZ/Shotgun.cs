using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{

    protected Dictionary<string, int> _multipleHitRoulette = new Dictionary<string, int>();

    public override void Ability()
    {
        var m = _roulette.ExecuteAction(_multipleHitRoulette);

        if (m == "Multiple")
        {
            Debug.Log("se activa la habilidad");
        }

        else if (m == "Normal")
        {
            Debug.Log("no se activa la habilidad");
        }
    }

    public override void StartRoulette()
    {
        base.StartRoulette();

        _multipleHitRoulette.Add("Multiple", _chanceToHitOtherParts);
        var m = 100 - _chanceToHitOtherParts;
        _multipleHitRoulette.Add("Normal", m > 0 ? m : 0);
    }
}
