using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnumsClass: MonoBehaviour
{
    public enum Team
    {
        Capsule,
        Box
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
