using System;
using UnityEngine;

[Serializable]
public class DisposableEffect
{
    public GameObject effect;
    public float remainingTime;

    public DisposableEffect(GameObject effect, float remainingTime)
    {
        this.effect = effect;
        this.remainingTime = remainingTime;
    }
}
