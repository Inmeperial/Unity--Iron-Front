using System;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    private Dictionary<string, int> _multipleHitRoulette = new Dictionary<string, int>();

    private void Start()
    {
        _animationEvents.Add(PlayShootParticle);
    }

    public override void SetGunData(GunSO data, Character character, string tag, string location, Animator animator)
    {
        _gunType = EnumsClass.GunsType.Shotgun;
        base.SetGunData(data, character, tag, location, animator);
    }
    
    public override void GunSkill(MechaPart targetPart)
    {
        if (!_gunSkillAvailable)
            return;

        string result = _roulette.ExecuteAction(_multipleHitRoulette);
        _gunSkillAvailable = false;
        switch (result)
        {
            case "Multiple":
            {
                    Character targetMecha = targetPart.GetCharacter();
                    
                    List<MechaPart> parts = new List<MechaPart>();
                    parts.Add(targetMecha.GetBody());
                    parts.Add(targetMecha.GetLeftGun());
                    parts.Add(targetMecha.GetRightGun());
                    parts.Add(targetMecha.GetLegs());

                    List<int> partsIndex = new List<int>();

                    //Determines how many and which parts will be attacked.
                    for (int i = 0; i < 4; i++)
                    {
                        int index = UnityEngine.Random.Range(0, 4);
                        if (partsIndex.Contains(index))
                            partsIndex.Add(-1);
                        else partsIndex.Add(index);
                    }

                    int tempMaxBullets = _data.maxBullets;
                    //Attacks the parts previously determined.
                    for (int i = 0; i < partsIndex.Count; i++)
                    {
                        int partToAttackIndex = partsIndex[i];

                        if (partToAttackIndex == -1)
                            continue;

                        MechaPart partToAttack = parts[partToAttackIndex];

                        if (!partToAttack)
                            continue;

                        if (tempMaxBullets <= 0)
                            return;                        

                        int bullets = UnityEngine.Random.Range(1, tempMaxBullets);

                        tempMaxBullets -= bullets;

                        List<Tuple<int, int>> damage = GetCalculatedDamage(bullets);

                        partToAttack.ReceiveDamage(damage);
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

    protected override void StartRoulette()
    {
        base.StartRoulette();

        _multipleHitRoulette.Add("Multiple", _data.chanceToHitOtherParts);
        int m = 100 - _data.chanceToHitOtherParts;
        _multipleHitRoulette.Add("Normal", m > 0 ? m : 0);
    }

    private void PlayShootParticle() //call in Animaton
    {
        EffectsController.Instance.PlayParticlesEffect(_data.attackParticles[0], _shootParticleSpawn.transform.position, _myChar.transform.forward, out ParticleSystem particle);
        particle.transform.parent = _shootParticleSpawn.transform;
        AudioManager.Instance.PlaySound(_data.attackSounds[0], gameObject);
    }
}
