using System.Collections.Generic;
using UnityEngine;

public class RouletteWheel
{    public string ExecuteAction(Dictionary<string, int> actions)
    {
        int totalWeight = 0;

        foreach (KeyValuePair<string, int> item in actions)
        {
            totalWeight += item.Value;
        }

        int random = Random.Range(0, totalWeight);

        foreach (KeyValuePair<string, int> item in actions)
        {
            random -= item.Value;

            if (random <= 0)
            {
                return item.Key;
            }
        }
        return default;
    }
}
