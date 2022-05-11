﻿using System.Collections.Generic;
using UnityEngine;
//using Random = UnityEngine.Random;

public class WorkshopMecha : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MechaEquipmentSO _equipment;

    [SerializeField] private Animator _animator;
    [SerializeField] private SkinnedMeshRenderer _bodySkinnedMeshRenderer;
    [SerializeField] private SkinnedMeshRenderer _legsSkinnedMeshRenderer;
    [SerializeField] private Transform _leftGunSpawn;
    [SerializeField] private Transform _rightGunSpawn;

    [SerializeField] private MasterShaderScript _bodyShader;
    [SerializeField] private MasterShaderScript _legsShader;

    private GameObject _leftGun;
    private GameObject _rightGun;

    private int _positionIndexInWorkshop;

    
    private List<MasterShaderScript> _rightWeaponList = new List<MasterShaderScript>();
    private List<MasterShaderScript> _leftWeaponList = new List<MasterShaderScript>();

    private void Start()
    {        
        float randomStart = Random.Range(0, _animator.GetCurrentAnimatorStateInfo(0).length);
        
        _animator.Play("Iddle", 0, randomStart);

        float randomSpeed = Random.Range(0.8f, 1.2f);

        _animator.speed = randomSpeed;
    }

    public void SetEquipment(MechaEquipmentSO equipment, int index)
    {
        _equipment = equipment;
        _positionIndexInWorkshop = index;
    }

    public void SpawnParts()
    {
        BodySO body = _equipment.body;

        ChangeBody(body);

        GunSO leftGun = _equipment.leftGun;
        ChangeLeftGun(leftGun);

        GunSO rightGun = _equipment.rightGun;
        ChangeRightGun(rightGun);

        LegsSO legs = _equipment.legs;
        ChangeLegs(legs);
    }
    public void ChangeBody(BodySO newBody)
    {
        SkinnedMeshRenderer updatedBodyMesh = PartMeshChange.UpdateMeshRenderer(_bodySkinnedMeshRenderer, newBody.skinnedMeshRenderer, transform.parent);
        _bodySkinnedMeshRenderer = updatedBodyMesh;

        _bodyShader.SetData(newBody.masterShader, newBody.bodyMaterial, newBody.jointsMaterial, newBody.armorMaterial);
        _bodyShader.colorMecha = _equipment.GetBodyColor();
        UpdateBodyColor(_equipment.GetBodyColor());
    }

    public void ChangeLeftGun(GunSO newGun)
    {
        if (_leftGun) Destroy(_leftGun);

        Gun leftGun = Instantiate(newGun.prefab, _leftGunSpawn);
        leftGun.transform.localPosition = Vector3.zero;
        _leftGun = leftGun.gameObject;

        SetLeftWeaponArray();
    }

    public void ChangeRightGun(GunSO newGun)
    {
        if (_rightGun) Destroy(_rightGun);

        Gun rightGun = Instantiate(newGun.prefab, _rightGunSpawn.transform);
        rightGun.transform.localPosition = Vector3.zero;
        _rightGun = rightGun.gameObject;
        SetRightWeaponArray();
    }

    public void ChangeLegs(LegsSO newLegs)
    {
        SkinnedMeshRenderer updatedLegsMesh = PartMeshChange.UpdateMeshRenderer(_legsSkinnedMeshRenderer, newLegs.skinnedMeshRenderer, transform.parent);
        _legsSkinnedMeshRenderer = updatedLegsMesh;

        _legsShader.SetData(newLegs.masterShader, newLegs.bodyMaterial, newLegs.jointsMaterial, newLegs.armorMaterial);
        _legsShader.colorMecha = _equipment.GetLegsColor();
        UpdateLegsColor(_equipment.GetLegsColor());
    }

    private void SetLeftWeaponArray()
    {
        _leftWeaponList = new List<MasterShaderScript>();

        for (int i = 0; i < _leftGun.transform.childCount; i++)
        {
            Transform child = _leftGun.transform.GetChild(i);

            MasterShaderScript masterShader = child.GetComponent<MasterShaderScript>();

            if (masterShader != null) _leftWeaponList.Add(masterShader);
        }
    }

    private void SetRightWeaponArray()
    {
        _rightWeaponList = new List<MasterShaderScript>();

        for (int i = 0; i < _rightGun.transform.childCount; i++)
        {
            Transform child = _rightGun.transform.GetChild(i);

            MasterShaderScript masterShader = child.GetComponent<MasterShaderScript>();

            if (masterShader != null) _rightWeaponList.Add(masterShader);
        }
    }

    public MasterShaderScript GetBodyShader()
    {
        return _bodyShader;
    }

    public MasterShaderScript GetLegsShader()
    {
        return _legsShader;
    }
    public List<MasterShaderScript> GetLeftWeaponShaderList()
    {
        return _leftWeaponList;
    }

    public List<MasterShaderScript> GetRightWeaponShaderList()
    {
        return _rightWeaponList;
    }

    public MechaEquipmentSO GetEquipment()
    {
        return _equipment;
    }

    public int GetPositionIndex()
    {
        return _positionIndexInWorkshop;
    }

    public void UpdateBodyColor(Color color)
    {
        _bodyShader.SetMechaColor(color);
    }
    
    public void UpdateLegsColor(Color color)
    {
        _legsShader.SetMechaColor(color);
    }
}
