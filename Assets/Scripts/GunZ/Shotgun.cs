using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{

    protected Dictionary<string, int> _multipleHitRoulette = new Dictionary<string, int>();

    string[] _parts = { "Body", "LArm", "RArm", "Legs" };
    public override void Ability()
    {
        Debug.Log("ability used");
        var m = _roulette.ExecuteAction(_multipleHitRoulette);
        Debug.Log("string: " + m);
        _abilityUsed = true;
        if (m == "Multiple")
        {
            var mgr = FindObjectOfType<ButtonsUIManager>();

            var partsIndex = new List<int>();

            //Determines how many and which parts will be attacked.
            for (int i = 0; i < 4; i++)
            {
                var r = Random.Range(0, 4);
                if (partsIndex.Contains(r))
                    partsIndex.Add(-1);
                else partsIndex.Add(r);
            }

            //Attacks the parts previously determined.
            for (int i = 0; i < partsIndex.Count; i++)
            {
                if (partsIndex[i] == -1)
                    continue;

                var bullets = Random.Range(1, _maxBullets);

                switch (_parts[partsIndex[i]])
                {
                    case "Body":
                        mgr.AddBulletsToBody(bullets);
                        break;

                    case "LArm":
                        mgr.AddBulletsToLArm(bullets);
                        break;

                    case "RArm":
                        mgr.AddBulletsToRArm(bullets);
                        break;

                    case "Legs":
                        mgr.AddBulletsToLegs(bullets);
                        break;
                }
            }
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
