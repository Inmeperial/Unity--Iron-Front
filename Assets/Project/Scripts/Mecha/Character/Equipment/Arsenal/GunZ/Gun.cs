﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;


public abstract class Gun : MechaPart
{
    [Header("References")]
    [SerializeField] protected Renderer[] _renderers;
    [SerializeField] protected GameObject _damageParticleSpawner;
    [SerializeField] protected GameObject _shootParticleSpawn;

    protected GunSO _data;
    protected EnumsClass.GunsType _gunType;
    protected int _availableBullets;
    protected int _currentAttackRange;
    protected int _currentCritChance;
    protected RouletteWheel _roulette;
    protected Dictionary<string, int> _critRoulette = new Dictionary<string, int>();
    protected Dictionary<string, int> _hitRoulette = new Dictionary<string, int>();

    protected bool _gunSkillAvailable;
    protected string _location;

    protected Animator _animator;

    protected List<Action> _animationEvents = new List<Action>();
    #region Getters
    public int GetMaxBullets()
    {
        return _data.maxBullets;
    }

    public int GetAvailableBullets()
    {
        return _availableBullets;
    }

    public int GetBulletDamage()
    {
        return _data.damage;
    }

    public int GetAttackRange()
    {
        return _currentAttackRange;
    }

    public int GetCritChance()
    {
        return _currentCritChance;
    }

    public float GetCritMultiplier()
    {
        return _data.critMultiplier;
    }

    public int GetHitChance()
    {
        return _data.hitChance;
    }

    public EnumsClass.GunsType GetGunType()
    {
        return _gunType;
    }

    public string GetGunName()
    {
        return _data.objectName;
    }

    public int GetAvailableSelections()
    {
        return _data.bodyPartsSelectionQuantity;
    }
    
    public int GetBulletsPerClick()
    {
        return _data.bulletsPerClick;
    }

    public Sprite GetIcon()
    {
        return _data.objectImage;
    }
    
    public GameObject GetParticleSpawn()
    {
        return _shootParticleSpawn;
    }
    
    public bool IsGunSkillAvailable()
    {
        return _gunSkillAvailable;
    }
    
    public string GetLocation()
    {
        return _location;
    }

    public Vector3 GetObjectCenterCenter()
    {
        return _collider.bounds.center;
    }
    #endregion
    
    public virtual void SetGunData(GunSO data, Character character, string tag, string location, Animator animator)
    {
        _data = data;
        _myChar = character;
        _maxHP = data.maxHp;
        _currentHP = _maxHP;
        _availableBullets = data.maxBullets;
        _weight = data.weight;
        _gunSkillAvailable = true;
        _currentAttackRange = data.attackRange;
        _currentCritChance = data.critChance;
        _collider.gameObject.tag = tag;
        _location = location;

        _masterShader.Initialize();

        _animator = animator;
        StartRoulette();
    }

    public override void SetAbilityData(AbilitySO abilityData)
    {
        if (!abilityData)
            return;

        _ability = Instantiate(abilityData.abilityPrefab, _myChar.transform) as WeaponAbility;

        _ability.Initialize(_myChar, abilityData);
        _ability.SetPart(this);
        _myChar.AddEquipable(_ability);
    }

    public void ResetGun()
    {
        _availableBullets = _data.maxBullets;
        _gunSkillAvailable = true;
    }

    public void ModifyRange(int extraRange)
    {
        _currentAttackRange += extraRange;
    }
    
    public void ModifyCritChance(int extraChance)
    {
        _currentCritChance += extraChance;
    }

    public void ReduceAvailableBullets()
    {
        _availableBullets -= GetBulletsPerClick();

        if (_availableBullets < 0)
            _availableBullets = 0;
    }

    public void IncreaseAvailableBullets()
    {
        _availableBullets += GetBulletsPerClick();

        if (_availableBullets > _data.maxBullets)
            _availableBullets = _data.maxBullets;
    }
    
    public void IncreaseAvailableBullets(int quantity)
    {
        _availableBullets += quantity;
        
        if (_availableBullets > _data.maxBullets)
            _availableBullets = _data.maxBullets;
    }

    public virtual void Attack(IDamageable target, int bullets)
    {
        List<Tuple<int, int>> damages = GetCalculatedDamage(bullets);

        _myChar.DoAttackAction();

        target.ReceiveDamage(damages);

        ExecuteAttackAnimation();
    }

    public List<Tuple<int, int>> GetCalculatedDamage(int bullets)
    {
        List<Tuple<int, int>> list = new List<Tuple<int, int>>();

        for (int i = 0; i < bullets; i++)
        {
            Tuple<int, int> t = null;
            //Determines if bullet hits.
            string h = _roulette.ExecuteAction(_hitRoulette);

            //MISS == 0
            //HIT == 1
            //CRIT == 2
            if (h == "Hit")
            {
                //Determines if it crits or not.
                string c = _roulette.ExecuteAction(_critRoulette);

                switch (c)
                {
                    case "Crit" when !_gunSkillAvailable:
                        t = Tuple.Create((int)(_data.damage * _data.critMultiplier) / 2, CriticalHit);
                        break;
                    case "Crit":
                        t = Tuple.Create((int)(_data.damage * _data.critMultiplier), CriticalHit);
                        break;
                    case "Normal" when !_gunSkillAvailable:
                        t = Tuple.Create(_data.damage / 2, NormalHit);
                        break;
                    case "Normal":
                        t = Tuple.Create(_data.damage, NormalHit);
                        break;
                }
            }
            else
            {
                t = Tuple.Create(0, MissHit);
            }
            list.Add(t);
        }
        return list;
    }

    protected virtual void StartRoulette()
    {
        _roulette = new RouletteWheel();
        _critRoulette = new Dictionary<string, int>();
        _critRoulette.Add("Crit", _currentCritChance);
        int c = 100 - _currentCritChance;
        _critRoulette.Add("Normal", c > 0 ? c : 0);

        _hitRoulette = new Dictionary<string, int>();
        _hitRoulette.Add("Hit", _data.hitChance);
        int h = 100 - _data.hitChance;
        _hitRoulette.Add("Miss", h > 0 ? h : 0);
    }

    public abstract void GunSkill(MechaPart targetPart);

    public abstract void Deselect();

    public void TurnOff()
    {
        ChangeMeshRenderStatus(false);

        DisableCollider();
    }


    public void ChangeMeshRenderStatus(bool status)
    {
        foreach (Renderer renderer in _renderers)
        {
            renderer.enabled = status;
        }

        _collider.enabled = status;
    }
    
    public override void ReceiveDamage(List<Tuple<int,int>> damages)
    {
        base.ReceiveDamage(damages);
        
        int totalDamage = 0;
        Vector3 pos = transform.position;
        for (int i = 0; i < damages.Count; i++)
        {
            totalDamage += damages[i].Item1;
            float hp = _currentHP - damages[i].Item1;
            _currentHP = hp > 0 ? hp : 0;

            int item = damages[i].Item2;
            switch (item)
            {
                case MissHit:
                    EffectsController.Instance.CreateDamageText("Miss", 0, pos, i == damages.Count - 1 ? true : false);
                    break;
                   
                case NormalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 1, pos, i == damages.Count - 1 ? true : false);
                    break;
               
                case CriticalHit:
                    EffectsController.Instance.CreateDamageText(damages[i].Item1.ToString(), 2, pos, i == damages.Count - 1 ? true : false);
                    break;
            }
        }

        OnDamageTaken?.Invoke(_myChar, totalDamage);

        _myChar.PlayReceiveDamageAnimation();

        if (_myChar.IsSelected())
            OnHealthChanged?.Invoke(_currentHP);

        _myChar.MechaOutsideAttackRange();
        
        if (IsPartBroken())
            DestroyPart();
    }
    
    public override void ReceiveDamage(int damage)
    {
        base.ReceiveDamage(damage);

        float hp = _currentHP - damage;
        _currentHP = hp > 0 ? hp : 0;

        Vector3 pos = transform.position;
        EffectsController.Instance.CreateDamageText(damage.ToString(), 1, pos);

        OnDamageTaken?.Invoke(_myChar, damage);

        _myChar.PlayReceiveDamageAnimation();

        if (_myChar.IsSelected())
            OnHealthChanged?.Invoke(_currentHP);

        _myChar.MechaOutsideAttackRange();

        if (IsPartBroken())
            DestroyPart();
    }

    public override void Heal(int healAmount)
    {
        if (_currentHP >= _maxHP)
        {
            healAmount = (int)_maxHP - (int)_currentHP;
            _currentHP = _maxHP;
        }
        else 
            _currentHP += healAmount;
        
        var pos = transform.position;
        EffectsController.Instance.CreateDamageText(healAmount.ToString(), 3, pos);


        if (_myChar.IsSelected())
            OnHealthChanged?.Invoke(_currentHP);
    }
    public MasterShaderScript GetMasterShader()
    {
        return _masterShader;
    }

    public virtual void ExecuteAttackAnimation()
    {
        switch (_location)
        {
            case "Left":
                PlayLeftSideAttackAnimation();
                break;

            case "Right":
                PlayRightSideAttackAnimation();
                break;

            default:
                break;
        }
    }
    protected virtual void PlayLeftSideAttackAnimation()
    {
        _animator.Play(_data.leftAttackAnimation.name);
    }
    protected virtual void PlayRightSideAttackAnimation()
    {
        _animator.Play(_data.rightAttackAnimation.name);
    }

    public void EndAnimationEvent() //Call in animation
    {
        Debug.Log("end animation event");
    }

    public void PlayAnimationEvent(int index)
    {
        if (index > _animationEvents.Count - 1)
            return;

        _animationEvents[index]();
    }

    protected override void DestroyPart()
    {
        base.DestroyPart();
        _myChar.ArmDestroyed(_location, _ability);
        TurnOff();
    }

    public override void PlayTakeDamageSound()
    {
        AudioManager.Instance.PlaySound(_data.takeDamageSound, gameObject);
    }

    public override void PlayDestroySound()
    {
        AudioManager.Instance.PlaySound(_data.destroySound, gameObject);
    }
    public override void PlayTakeDamageVFX()
    {
        EffectsController.Instance.PlayParticlesEffect(_data.damageParticle, transform.position, transform.forward);
    }

    public override void PlayDestroyVFX()
    {
        EffectsController.Instance.PlayParticlesEffect(_data.destroyParticle, transform.position, transform.forward);
    }
    private void OnDestroy()
    {
        OnHealthChanged = null;
        OnDamageTaken = null;
    }
}
