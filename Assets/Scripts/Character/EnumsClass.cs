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

    public enum ItemType
    {
        None,
        Grenade
    };

    public enum ParticleActionType
    {
        MovingBridge,
        HammerPreparation,
        HammerSwing,
        HammerHit,
        Damage,
        FlameThrower,
        Mine,
        ShootGun,
        AssaultRifle,
        AssaultRifleFinalShot,
        Rifle,
        Dead,
        Hit,
        MortarHit
    };
}
