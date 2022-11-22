using System.Collections.Generic;
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

    private GameObject _leftGunGameObject;
    private GameObject _rightGunGameObject;

    private int _positionIndexInWorkshop;

    
    private MasterShaderScript _rightGunShader;
    private MasterShaderScript _leftGunShader;

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
        UpdateBodyColor(_equipment.GetBodyColor());
    }

    public void ChangeLeftGun(GunSO newGun)
    {
        if (_leftGunGameObject)
            Destroy(_leftGunGameObject);

        Gun leftGun = Instantiate(newGun.prefab, _leftGunSpawn);
        leftGun.transform.localPosition = Vector3.zero;
        _leftGunGameObject = leftGun.gameObject;

        _leftGunShader = leftGun.GetMasterShader();

        _leftGunShader.Initialize();
    }

    public void ChangeRightGun(GunSO newGun)
    {
        if (_rightGunGameObject)
            Destroy(_rightGunGameObject);

        Gun rightGun = Instantiate(newGun.prefab, _rightGunSpawn.transform);
        rightGun.transform.localPosition = Vector3.zero;
        _rightGunGameObject = rightGun.gameObject;

        _rightGunShader = rightGun.GetMasterShader();
        
        _rightGunShader.Initialize();
    }

    public void ChangeLegs(LegsSO newLegs)
    {
        SkinnedMeshRenderer updatedLegsMesh = PartMeshChange.UpdateMeshRenderer(_legsSkinnedMeshRenderer, newLegs.skinnedMeshRenderer, transform.parent);
        _legsSkinnedMeshRenderer = updatedLegsMesh;

        _legsShader.SetData(newLegs.masterShader, newLegs.bodyMaterial, newLegs.jointsMaterial, newLegs.armorMaterial);

        UpdateLegsColor(_equipment.GetLegsColor());
    }

    public MasterShaderScript GetBodyShader() => _bodyShader;

    public MasterShaderScript GetLegsShader() => _legsShader;
    public MasterShaderScript GetLeftGunShader() => _leftGunShader;

    public MasterShaderScript GetRightGunShader() => _rightGunShader;

    public MechaEquipmentSO GetEquipment() => _equipment;

    public int GetPositionIndex() => _positionIndexInWorkshop;

    public void UpdateBodyColor(Color color) => _bodyShader.SetMechaColor(color);

    public void UpdateLegsColor(Color color) => _legsShader.SetMechaColor(color);
}
