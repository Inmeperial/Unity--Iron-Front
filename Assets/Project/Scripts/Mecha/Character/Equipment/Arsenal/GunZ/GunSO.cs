using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Scriptable Objects/Parts/Gun")]
public class GunSO : ArsenalObjectSO
{
    [Header("Prefab")]
    public Gun prefab;

    [Header("Stats")]
    public float maxHp;
    public int maxBullets;
    public int availableBullets;
    public int bulletsPerClick;
    public int damage;
    public int critChance;
    public float critMultiplier;
    public int hitChance;
    public int chanceToHitOtherParts;
    public int attackRange;
    public int bodyPartsSelectionQuantity;
    public float weight;

    [Header("Sound")]
    public SoundData attackSound;
    public SoundData takeDamageSound;
    public SoundData destroySound;

    [Header("Effects")]
    public ParticleSystem[] attackParticles;
    public ParticleSystem damageParticle;
    public ParticleSystem destroyParticle;

    [Header("Animation")]
    public string leftAnimationBoolName;
    public string rightAnimationBoolName;
}
