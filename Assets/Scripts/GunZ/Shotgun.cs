using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    private Dictionary<string, int> _multipleHitRoulette = new Dictionary<string, int>();

    private readonly string[] _parts = { "Body", "LArm", "RArm", "Legs" };
    
    public override void SetGun(GunSO data, Character character, Equipable.Location location)
    {
        _gunType = GunsType.Shotgun;
        _gun = "Shotgun";
        base.SetGun(data, character, location);
    }
    
    public override void Ability()
    {
        string m = _roulette.ExecuteAction(_multipleHitRoulette);
        _gunSkill = true;
        switch (m)
        {
            case "Multiple":
            {
                List<int> partsIndex = new List<int>();

                //Determines how many and which parts will be attacked.
                for (int i = 0; i < 4; i++)
                {
                    int r = Random.Range(0, 4);
                    if (partsIndex.Contains(r))
                        partsIndex.Add(-1);
                    else partsIndex.Add(r);
                }

                //Attacks the parts previously determined.
                for (int i = 0; i < partsIndex.Count; i++)
                {
                    if (partsIndex[i] == -1)
                        continue;

                    int bullets = Random.Range(1, _maxBullets);

                    switch (_parts[partsIndex[i]])
                    {
                        case "Body":
                            ButtonsUIManager.Instance.AddBulletsToBody(bullets);
                            break;

                        case "LArm":
                            ButtonsUIManager.Instance.AddBulletsToLArm(bullets);
                            break;

                        case "RArm":
                            ButtonsUIManager.Instance.AddBulletsToRArm(bullets);
                            break;

                        case "Legs":
                            ButtonsUIManager.Instance.AddBulletsToLegs(bullets);
                            break;
                    }
                }

                break;
            }
            case "Normal":
                break;
        }
    }

    public override void Deselect()
    {
    }

    public override void StartRoulette()
    {
        base.StartRoulette();

        _multipleHitRoulette.Add("Multiple", _chanceToHitOtherParts);
        var m = 100 - _chanceToHitOtherParts;
        _multipleHitRoulette.Add("Normal", m > 0 ? m : 0);
    }
}
