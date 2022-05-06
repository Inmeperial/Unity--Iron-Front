using System.Collections.Generic;
using UnityEngine;
//using Random = UnityEngine.Random;

public class WorkshopMecha : MonoBehaviour
{
    [SerializeField] private MechaEquipmentSO _equipment;

    [SerializeField] private Animator _animator;
    [SerializeField] private SkinnedMeshRenderer _bodySkinnedMeshRenderer;
    [SerializeField] private SkinnedMeshRenderer _legsSkinnedMeshRenderer;
    [SerializeField] private Transform _leftGunSpawn;
    [SerializeField] private Transform _rightGunSpawn;

    private GameObject _leftGun;
    private GameObject _rightGun;

    private int _positionIndexInWorkshop;

    [SerializeField] private MasterShaderScript _bodyShader;
    [SerializeField] private MasterShaderScript _legsShader; 
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
        //SkinnedMeshRenderer updatedBodyMesh = PartMeshChange.UpdateMeshRenderer(_bodySkinnedMeshRenderer, body.skinnedMeshRenderer, transform.parent);
        //_bodySkinnedMeshRenderer = updatedBodyMesh;
        //_body = Instantiate(_equipment.body.meshPrefab[0], _bodySpawnPosition);
        //_body.transform.localPosition = Vector3.zero;

        //_bodyShader.SetData(body.masterShader, body.bodyMaterial, body.jointsMaterial, body.armorMaterial);
        //_bodyShader.colorMecha = _equipment.GetBodyColor();
        //UpdateBodyColor(_equipment.GetBodyColor());

        //_leftArm = Instantiate(_equipment.body.armsMeshPrefab[0], _leftArmSpawnPosition);
        //_leftArm.transform.localPosition = Vector3.zero;
        //_leftArmShader = _leftArm.GetComponent<MasterShaderScript>();
        //_leftArmShader.colorMecha = _equipment.GetBodyColor();

        GunSO leftGun = _equipment.leftGun;
        ChangeLeftGun(leftGun);

        //Gun leftGun = Instantiate(_equipment.leftGun.prefab, _leftGunSpawn);
        //leftGun.transform.localPosition = Vector3.zero;
        //_leftGun = leftGun.gameObject;


        //_rightArm = Instantiate(_equipment.body.armsMeshPrefab[1], _rightArmSpawnPosition);
        //_rightArm.transform.localPosition = Vector3.zero;
        //_rightArmShader = _rightArm.GetComponent<MasterShaderScript>();
        //_rightArmShader.colorMecha = _equipment.GetBodyColor();

        GunSO rightGun = _equipment.rightGun;
        ChangeRightGun(rightGun);

        //Gun rightGun = Instantiate(_equipment.rightGun.prefab, _rightGunSpawn.transform);
        //rightGun.transform.localPosition = Vector3.zero;
        //_rightGun = rightGun.gameObject;

        LegsSO legs = _equipment.legs;
        ChangeLegs(legs);
        //SkinnedMeshRenderer updatedLegsMesh = PartMeshChange.UpdateMeshRenderer(_legsSkinnedMeshRenderer, legs.skinnedMeshRenderer, transform.parent);
        //_legsSkinnedMeshRenderer = updatedLegsMesh;

        //_legsShader.SetData(legs.masterShader, legs.bodyMaterial, legs.jointsMaterial, legs.armorMaterial);
        //_legsShader.colorMecha = _equipment.GetLegsColor();
        //UpdateLegsColor(_equipment.GetLegsColor());

        //_leftLeg = Instantiate(_equipment.legs.meshPrefab[0], _leftLegSpawnPosition);
        //_leftLeg.transform.localPosition = Vector3.zero;
        //_leftLegShader = _leftLeg.GetComponent<MasterShaderScript>();
        //_leftLegShader.colorMecha = _equipment.GetLegsColor();

        //_rightLeg = Instantiate(_equipment.legs.meshPrefab[1], _rightLegSpawnPosition);
        //_rightLeg.transform.localPosition = Vector3.zero;
        //_rightLegShader = _rightLeg.GetComponent<MasterShaderScript>();
        //_rightLegShader.colorMecha = _equipment.GetLegsColor();
    }
    public void ChangeBody(BodySO newBody)
    {
        SkinnedMeshRenderer updatedBodyMesh = PartMeshChange.UpdateMeshRenderer(_bodySkinnedMeshRenderer, newBody.skinnedMeshRenderer, transform.parent);
        _bodySkinnedMeshRenderer = updatedBodyMesh;

        _bodyShader.SetData(newBody.masterShader, newBody.bodyMaterial, newBody.jointsMaterial, newBody.armorMaterial);
        _bodyShader.colorMecha = _equipment.GetBodyColor();
        UpdateBodyColor(_equipment.GetBodyColor());
        //if (_body && _leftArm && _rightArm)
        //{
        //    Destroy(_body);
        //    Destroy(_leftArm);
        //    Destroy(_rightArm);
        //}

        //_body = Instantiate(newBody.meshPrefab[0], _bodySpawnPosition);
        //_body.transform.localPosition = Vector3.zero;
        //_bodyShader = _body.GetComponent<MasterShaderScript>();

        //_leftArm = Instantiate(newBody.armsMeshPrefab[0], _leftArmSpawnPosition);
        //_leftArm.transform.localPosition = Vector3.zero;
        //_leftArmShader = _leftArm.GetComponent<MasterShaderScript>();

        //_rightArm = Instantiate(newBody.armsMeshPrefab[1], _rightArmSpawnPosition);
        //_rightArm.transform.localPosition = Vector3.zero;
        //_rightArmShader = _rightArm.GetComponent<MasterShaderScript>();

        //_bodyShader.colorMecha = _equipment.GetBodyColor();
        //_leftArmShader.colorMecha = _equipment.GetBodyColor();
        //_rightArmShader.colorMecha = _equipment.GetBodyColor();
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

        //if (_leftLeg && _rightLeg)
        //{
        //    Destroy(_leftLeg);
        //    Destroy(_rightLeg);
        //}
        //_leftLeg = Instantiate(newLegs.meshPrefab[0], _leftLegSpawnPosition);
        //_leftLeg.transform.localPosition = Vector3.zero;
        //_leftLegShader = _leftLeg.GetComponent<MasterShaderScript>();

        //_rightLeg = Instantiate(newLegs.meshPrefab[1], _rightLegSpawnPosition);
        //_rightLeg.transform.localPosition = Vector3.zero;
        //_rightLegShader = _rightLeg.GetComponent<MasterShaderScript>();

        //_leftLegShader.colorMecha = _equipment.GetLegsColor();
        //_rightLegShader.colorMecha = _equipment.GetLegsColor();
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
        //_leftArmShader.SetMechaColor(color);
        //_rightArmShader.SetMechaColor(color);
    }
    
    public void UpdateLegsColor(Color color)
    {
        _legsShader.SetMechaColor(color);
        //_leftLegShader.SetMechaColor(color);
        //_rightLegShader.SetMechaColor(color);
    }

    //private enum WeaponSide
    //{
    //    rightSide,
    //    leftSide
    //};
}
