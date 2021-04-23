using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteWheel
{    public string ExecuteAction(Dictionary<string, int> actions)
    {
        int totalWeight = 0;

        foreach (var item in actions)
        {
            totalWeight += item.Value;
        }

        int random = Random.Range(0, totalWeight);

        foreach (var item in actions)
        {
            Debug.Log("item es: " + item);
            random -= item.Value;

            if (random <= 0)
            {
                return item.Key;
            }
        }
        return default;
    }
}
