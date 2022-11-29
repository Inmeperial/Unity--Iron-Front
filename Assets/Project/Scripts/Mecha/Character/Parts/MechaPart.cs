using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MechaPart : MonoBehaviour, IDamageable
{
    [SerializeField] protected SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] protected Collider _collider;
    [SerializeField] protected List<GameObject> _particleSpawner = new List<GameObject>();
    [SerializeField] protected MasterShaderScript _masterShader;

    protected const int MissHit = 0;
    protected const int NormalHit = 1;
    protected const int CriticalHit = 2;
    protected Character _myChar;
    protected Ability _ability;
    protected float _maxHP;
    protected float _currentHP;
    protected float _weight;
    public float MaxHp => _maxHP;
    public float CurrentHP => _currentHP;

    public Action<float> OnHealthChanged;
    public Action<Character, float> OnDamageTaken;

    public virtual void SetPartData(Character character, PartSO data, Color partColor)
    {
        _myChar = character;
        _maxHP = data.maxHP;
        _currentHP = _maxHP;
        _weight = data.weight;

        if (!_myChar)
            return;

        SkinnedMeshRenderer updatedMesh = PartMeshChange.UpdateMeshRenderer(_skinnedMeshRenderer, data.skinnedMeshRenderer, _myChar.transform.parent);
        _skinnedMeshRenderer = updatedMesh;

        _masterShader.SetData(data.masterShader, data.bodyMaterial, data.jointsMaterial, data.armorMaterial);
        _masterShader.SetMechaColor(partColor);
        
        
        //if(data.ability && data.ability.abilityPrefab)
        //{
        //    _ability = Instantiate(data.ability.abilityPrefab, _myChar.transform);
        //    _ability.Initialize(_myChar, data.ability, location);
        //    _myChar.AddEquipable(_ability);
        //}
    }

    public virtual void SetAbilityData(AbilitySO abilityData)
    {
        if (!abilityData)
            return;
            
        _ability = Instantiate(abilityData.abilityPrefab, transform);
        _ability.Initialize(_myChar, abilityData);
        _myChar.AddEquipable(_ability);
    }

    

    public float GetWeight()
    {
        return _weight;
    }

    public virtual void ReceiveDamage(List<Tuple<int, int>> damages)
    {
        if (IsPartBroken())
            return;

        int totalDamage = 0;

        foreach (Tuple<int, int> kvp in damages)
        {
            totalDamage += kvp.Item1;
        }

        if (totalDamage > 0)
        {
            PlayTakeDamageSound();
            PlayTakeDamageVFX();
        }        
    }

    public virtual void ReceiveDamage(int damage)
    {
        if (_myChar.IsDead())
            return;

        if (IsPartBroken())
            return;

        if (damage > 0)
        {
            PlayTakeDamageSound();
            PlayTakeDamageVFX();
        }        
    }

    public virtual void Heal(int healAmount)
    {
        float finalHP = _currentHP + healAmount;  

        if (finalHP >= _maxHP)
            _currentHP = _maxHP;
        else
            _currentHP = finalHP;
    }

    public Ability GetAbility()
    {
        return _ability;
    }

    public Character GetCharacter()
    {
        return _myChar;
    }

    public void SetShader(SwitchTextureEnum textureEnum)
    {
        _masterShader.ConvertEnumToStringEnumForShader(textureEnum);
    }

    public void ChangePartMeshActiveStatus(bool status)
    {
        _skinnedMeshRenderer.gameObject.SetActive(status);
        _collider.enabled = status;
    }

    public abstract void PlayTakeDamageSound();
    public abstract void PlayDestroySound();
    public abstract void PlayTakeDamageVFX();
    public abstract void PlayDestroyVFX();
    protected virtual bool IsPartBroken()
    {
        return _currentHP <= 0;
    }

    protected virtual void DestroyPart()
    {
        DisableCollider();
        PlayDestroySound();
        PlayDestroyVFX();
    }

    public void DisableCollider()
    {
        _collider.enabled = false;
    }
}
