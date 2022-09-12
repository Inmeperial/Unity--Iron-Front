using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class Gun : MechaPart
{
    [SerializeField] protected Renderer[] _renderers;
    [SerializeField] protected GameObject _damageParticleSpawner;
    [SerializeField] protected GameObject _shootParticleSpawn;
    protected EnumsClass.GunsType _gunType;
    protected string _gunName;
    protected Sprite _icon;
    protected int _maxBullets;
    protected int _availableBullets;
    protected int _bulletsPerClick;
    protected int _damage;
    protected int _critChance;
    protected float _critMultiplier;
    protected int _hitChance;
    protected int _chanceToHitOtherParts;
    protected int _attackRange;
    protected int _bodyPartsSelectionQuantity;

    protected RouletteWheel _roulette;
    protected Dictionary<string, int> _critRoulette = new Dictionary<string, int>();
    protected Dictionary<string, int> _hitRoulette = new Dictionary<string, int>();

    //Nada que ver con la habilidad que se le equipa
    protected bool _gunSkillAvailable;
    protected string _location;
    protected AnimationMechaHandler _animationMechaHandler;

    //private bool _abilityCreated;

    #region Getters
    public int GetMaxBullets()
    {
        return _maxBullets;
    }

    public int GetAvailableBullets()
    {
        return _availableBullets;
    }

    public int GetBulletDamage()
    {
        return _damage;
    }

    public int GetAttackRange()
    {
        return _attackRange;
    }

    public int GetCritChance()
    {
        return _critChance;
    }

    public float GetCritMultiplier()
    {
        return _critMultiplier;
    }

    public int GetHitChance()
    {
        return _hitChance;
    }

    public EnumsClass.GunsType GetGunType()
    {
        return _gunType;
    }

    public string GetGunName()
    {
        return _gunName;
    }

    public int GetAvailableSelections()
    {
        return _bodyPartsSelectionQuantity;
    }
    
    public int GetBulletsPerClick()
    {
        return _bulletsPerClick;
    }

    public Sprite GetIcon()
    {
        return _icon;
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
    
    /// <summary>
    /// Set Gun stats from given scriptable object.
    /// </summary>
    public virtual void SetGunData(GunSO data, Character character, string tag, string location)
    {
        _gunName = data.objectName;
        _myChar = character;
        _maxHP = data.maxHp;
        _currentHP = MaxHP;
        _icon = data.objectImage;
        _maxBullets = data.maxBullets;
        _availableBullets = _maxBullets;
        _bulletsPerClick = data.bulletsPerClick;
        _damage = data.damage;
        _critChance = data.critChance;
        _critMultiplier = data.critMultiplier;
        _hitChance = data.hitChance;
        _chanceToHitOtherParts = data.chanceToHitOtherParts;
        _attackRange = data.attackRange;
        _bodyPartsSelectionQuantity = data.bodyPartsSelectionQuantity;
        _weight = data.weight;
        _gunSkillAvailable = true;

        _collider.gameObject.tag = tag;
        _location = location;

        _masterShader.Initialize();

        StartRoulette();

        //if(!_abilityCreated && data.ability && data.ability.abilityPrefab)
        //{
        //    _ability = Instantiate(data.ability.abilityPrefab, _myChar.transform);
        //    _ability.Initialize(_myChar, data.ability, location);
        //    _myChar.AddEquipable(_ability);
        //    _abilityCreated = true;
        //}
    }

    public override void SetAbilityData(AbilitySO abilityData)
    {
        if (!abilityData)
            return;

        _ability = Instantiate(abilityData.abilityPrefab, _myChar.transform) as WeaponAbility;

        _ability.Initialize(_myChar, abilityData);
        _ability.SetPart(this);
        _myChar.AddEquipable(_ability);
        //_abilityCreated = true;
    }

    public void SetAnimationHandler(AnimationMechaHandler animationMechaHandler) => _animationMechaHandler = animationMechaHandler;

    public void ResetGun()
    {
        _availableBullets = _maxBullets;
        _gunSkillAvailable = true;
    }

    /// <summary>
    /// Modify gun range.
    /// </summary>
    /// <param name="extraRange">The amount of range to modify. If negative, it decrease.</param>
    public void ModifyRange(int extraRange)
    {
        _attackRange += extraRange;
    }
    
    /// <summary>
    /// Modify critical chance.
    /// </summary>
    /// <param name="extraChance">The amount of critical chance to modify. If negative, it decrease.</param>
    public void ModifyCritChance(int extraChance)
    {
        _critChance += extraChance;
    }

    /// <summary>
    /// Reduce this gun available bullets by the amount of this gun bullets per click.  
    /// </summary>
    public void ReduceAvailableBullets()
    {
        _availableBullets -= GetBulletsPerClick();

        if (_availableBullets < 0)
            _availableBullets = 0;
    }

    /// <summary>
    /// Increase this gun available bullets by the amount of this gun bullets per click.  
    /// </summary>
    public void IncreaseAvailableBullets()
    {
        _availableBullets += GetBulletsPerClick();

        if (_availableBullets > _maxBullets)
            _availableBullets = _maxBullets;
    }
    
    /// <summary>
    /// Increase this gun available bullets by the given quantity.
    /// </summary>
    public void IncreaseAvailableBullets(int quantity)
    {
        _availableBullets += quantity;
        
        if (_availableBullets > _maxBullets)
            _availableBullets = _maxBullets;
    }

    public void Attack(MechaPart partToAttack, int bullets)
    {
        List<Tuple<int, int>> damages = GetCalculatedDamage(bullets);

        partToAttack.ReceiveDamage(damages);
        
        AttackAnimation();
    }

    /// <summary>
    /// Returns a collection of the damage each bullet does and if it miss, hits or crit.
    /// </summary>
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
                        t = Tuple.Create((int)(_damage * _critMultiplier) / 2, CriticalHit);
                        break;
                    case "Crit":
                        t = Tuple.Create((int)(_damage * _critMultiplier), CriticalHit);
                        break;
                    case "Normal" when !_gunSkillAvailable:
                        t = Tuple.Create(_damage / 2, NormalHit);
                        break;
                    case "Normal":
                        t = Tuple.Create(_damage, NormalHit);
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

    public virtual void StartRoulette()
    {
        _roulette = new RouletteWheel();
        _critRoulette = new Dictionary<string, int>();
        _critRoulette.Add("Crit", _critChance);
        int c = 100 - _critChance;
        _critRoulette.Add("Normal", c > 0 ? c : 0);

        _hitRoulette = new Dictionary<string, int>();
        _hitRoulette.Add("Hit", _hitChance);
        int h = 100 - _hitChance;
        _hitRoulette.Add("Miss", h > 0 ? h : 0);
    }

    public abstract void GunSkill(MechaPart targetPart);

    public abstract void Deselect();

    /// <summary>
    /// Return true if the Gun ability was used, false if not.
    /// </summary>


    public void TurnOff()
    {
        ChangeMeshRenderStatus(false);

        _collider.enabled = false;
    }


    public void ChangeMeshRenderStatus(bool status)
    {
        foreach (Renderer renderer in _renderers)
        {
            renderer.enabled = status;
        }

        _collider.enabled = status;
    }
    
    //Lo ejecuta el ButtonsUIManager, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public override void ReceiveDamage(List<Tuple<int,int>> damages)
    {
        if (_currentHP <= 0) return;
        
        int totalDamage = 0;
        Vector3 pos = transform.position;
        for (int i = 0; i < damages.Count; i++)
        {
            totalDamage += damages[i].Item1;
            float hp = _currentHP - damages[i].Item1;
            _currentHP = hp > 0 ? hp : 0;
            EffectsController.Instance.PlayParticlesEffect(_damageParticleSpawner, EnumsClass.ParticleActionType.Damage);
            //EffectsController.Instance.PlayParticlesEffect(this.gameObject, EnumsClass.ParticleActionType.Hit);

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
        //WorldUI worldUI = _myChar.GetMyUI();
        //worldUI.Show();
        
        //switch (_location)
        //{
        //    case "Left":
        //        worldUI.SetLeftGunHPBar(CurrentHP);
        //        worldUI.UpdateLeftGunHPBar(totalDamage);
        //        break;
            
        //    case "Right":
        //        worldUI.SetRightGunHPBar(CurrentHP);
        //        worldUI.UpdateRightGunHPBar(totalDamage);
        //        break;
        //}

        if (_myChar.IsSelected())
            OnHealthChanged?.Invoke(_currentHP);

        _myChar.MechaOutsideAttackRange();
        
        if (_currentHP <= 0)
        {
            EffectsController.Instance.PlayParticlesEffect(_damageParticleSpawner, EnumsClass.ParticleActionType.DestroyPart);
            _myChar.ArmDestroyed(_location, _ability);
            gameObject.SetActive(false);
        }
        
    }
    
    //Lo ejecuta el mortero, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public override void ReceiveDamage(int damage)
    {
        if (_currentHP <= 0) return;
        
        float hp = _currentHP - damage;
        _currentHP = hp > 0 ? hp : 0;

        EffectsController.Instance.PlayParticlesEffect(_damageParticleSpawner, EnumsClass.ParticleActionType.Damage);
        //EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.Hit);

        Vector3 pos = transform.position;
        EffectsController.Instance.CreateDamageText(damage.ToString(), 1, pos);

        OnDamageTaken?.Invoke(_myChar, damage);

        //WorldUI worldUI = _myChar.GetMyUI();
        //worldUI.Show();


        //if (_location == "Left")
        //{
        //    worldUI.SetLeftGunHPBar(_currentHP);
        //    worldUI.UpdateLeftGunHPBar(damage);
        //}
        //else
        //{
        //    worldUI.SetRightGunHPBar(_currentHP);
        //    worldUI.UpdateRightGunHPBar(damage);
        //}
        
        if (_myChar.IsSelected())
            OnHealthChanged?.Invoke(_currentHP);

        _myChar.MechaOutsideAttackRange();
        
        if (CurrentHP <= 0)
        {
            EffectsController.Instance.PlayParticlesEffect(_damageParticleSpawner, EnumsClass.ParticleActionType.DestroyPart);
            _myChar.ArmDestroyed(_location, _ability);
            TurnOff();
        }
        
    }

    public override void Heal(int healAmount)
    {
        if (_currentHP >= _maxHP)
        {
            healAmount = (int)_maxHP - (int)_currentHP;
            _currentHP = _maxHP;
        }
        else _currentHP += healAmount;
        
        var pos = transform.position;
        EffectsController.Instance.CreateDamageText(healAmount.ToString(), 3, pos);


        if (_myChar.IsSelected())
            OnHealthChanged?.Invoke(_currentHP);
    }
    public MasterShaderScript GetMasterShader() => _masterShader;

    public virtual void AttackAnimation()
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

    protected abstract void PlayLeftSideAttackAnimation();
    protected abstract void PlayRightSideAttackAnimation();
    private void OnDestroy()
    {
        OnHealthChanged = null;
        OnDamageTaken = null;
    }
}
