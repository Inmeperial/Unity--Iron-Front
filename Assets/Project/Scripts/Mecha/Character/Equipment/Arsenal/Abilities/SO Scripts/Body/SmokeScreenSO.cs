using UnityEngine;

[CreateAssetMenu(fileName = "Smoke Screen", menuName = "Scriptable Objects/Abilities/Body/Smoke Screen")]
public class SmokeScreenSO : BodyAbilitySO
{
    public float hpPercentageForSmokeActivation;
    public GameObject smokeScreenObject;
}
