using System;
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

    //Nada que ver con la habilidad que se le equipa
    protected bool _gunSkillAvailable;
    protected string _location;

    protected Animator _animator;
    //private bool _abilityCreated;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
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
    
    /// <summary>
    /// Set Gun stats from given scriptable object.
    /// </summary>
    public virtual void SetGunData(GunSO data, Character character, string tag, string location)
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

    //public void SetAnimationHandler(Animator animator)
    //{
    //    _animator = animator;
    //}

    public void ResetGun()
    {
        _availableBullets = _data.maxBullets;
        _gunSkillAvailable = true;
    }

    /// <summary>
    /// Modify gun range.
    /// </summary>
    /// <param name="extraRange">The amount of range to modify. If negative, it decrease.</param>
    public void ModifyRange(int extraRange)
    {
        _currentAttackRange += extraRange;
    }
    
    /// <summary>
    /// Modify critical chance.
    /// </summary>
    /// <param name="extraChance">The amount of critical chance to modify. If negative, it decrease.</param>
    public void ModifyCritChance(int extraChance)
    {
        _currentCritChance += extraChance;
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

        if (_availableBullets > _data.maxBullets)
            _availableBullets = _data.maxBullets;
    }
    
    /// <summary>
    /// Increase this gun available bullets by the given quantity.
    /// </summary>
    public void IncreaseAvailableBullets(int quantity)
    {
        _availableBullets += quantity;
        
        if (_availableBullets > _data.maxBullets)
            _availableBullets = _data.maxBullets;
    }

    public virtual void Attack(MechaPart partToAttack, int bullets)
    {
        List<Tuple<int, int>> damages = GetCalculatedDamage(bullets);

        partToAttack.ReceiveDamage(damages);

        AudioManager.Instance.PlaySound(_data.attackSound, gameObject);
        ExecuteAttackAnimation();
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

    public virtual void StartRoulette()
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

        if (_myChar.IsSelected())
            OnHealthChanged?.Invoke(_currentHP);

        _myChar.MechaOutsideAttackRange();
        
        if (IsPartBroken())
            DestroyPart();        
    }
    
    //Lo ejecuta el mortero, activa las particulas y textos de daño del effects controller, actualiza el world canvas
    public override void ReceiveDamage(int damage)
    {
        base.ReceiveDamage(damage);

        float hp = _currentHP - damage;
        _currentHP = hp > 0 ? hp : 0;

        Vector3 pos = transform.position;
        EffectsController.Instance.CreateDamageText(damage.ToString(), 1, pos);

        OnDamageTaken?.Invoke(_myChar, damage);
        
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
        _animator.SetBool(_data.leftAnimationBoolName, true);
    }
    protected virtual void PlayRightSideAttackAnimation()
    {
        _animator.SetBool(_data.rightAnimationBoolName, true);
    }

    protected void AnimatorSetBool(string boolName, bool state)
    {
        _animator.SetBool(boolName, state);
    }

    public void EndAnimation() //Call in animation
    {
        string boolName = "";

        switch (_location)
        {
            case "Left":
                boolName = _data.leftAnimationBoolName;
                break;

            case "Right":
                boolName = _data.rightAnimationBoolName;
                break;
        }

        _animator.SetBool(boolName, false);
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
