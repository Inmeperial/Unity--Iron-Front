using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Parts : MonoBehaviour, IChangeableShader
{
    [SerializeField] protected SkinnedMeshRenderer _skinnedMeshRenderer;
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

    public float GetMaxHp() => _maxHP;

    public float GetCurrentHp() => _currentHP;

    public float GetWeight() => _weight;

    public abstract void TakeDamage(List<Tuple<int, int>> damages);

    public abstract void TakeDamage(int damage);

    public virtual void Heal(int healAmount)
    {
        float finalHP = _currentHP + healAmount;  

        if (finalHP >= _maxHP)
            _currentHP = _maxHP;
        else 
            _currentHP = finalHP;
    }

    public Ability GetAbility() => _ability;

    public Character GetCharacter() => _myChar;

    public void SetShader(SwitchTextureEnum textureEnum) => _masterShader.ConvertEnumToStringEnumForShader(textureEnum);

    public void ChangePartMeshActiveStatus(bool status) => _skinnedMeshRenderer.gameObject.SetActive(status);
}
