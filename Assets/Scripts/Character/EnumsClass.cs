using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnumsClass: MonoBehaviour
{
    public enum Team
    {
        Green,
        Red
    };

    public enum GunsType
    {
        None,
        AssaultRifle,
        Melee,
        Rifle,
        Shield,
        Shotgun
    };
}
