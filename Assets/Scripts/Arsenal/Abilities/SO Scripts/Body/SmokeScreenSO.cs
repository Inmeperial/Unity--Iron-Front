using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SmokeScreen", menuName = "Create Ability/SmokeScreen")]
public class SmokeScreenSO : BodyAbilitySO
{
    public float hpPercentageForSmokeActivation;
    public GameObject smokeScreenObject;
}
