using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorkshopMecha : MonoBehaviour
{
    [SerializeField] private MechaEquipmentSO _equipment;

    [SerializeField] private Animator _animator;
    [SerializeField] private SkinnedMeshRenderer _bodySkinnedMeshRenderer;
    [SerializeField] private SkinnedMeshRenderer _legsSkinnedMeshRenderer;
    [SerializeField] private Transform _leftGunSpawn;
    [SerializeField] private Transform _rightGunSpawn;

    //private GameObject _body;
    //private GameObject _leftArm;
    //private GameObject _rightArm;
    //private GameObject _leftLeg;
    //private GameObject _rightLeg;
    private GameObject _leftGun;
    private GameObject _rightGun;

    private int _positionIndexInWorkshop;

    [SerializeField] private MasterShaderScript _bodyShader;
    [SerializeField] private MasterShaderScript _legsShader; 
    private MasterShaderScript[] _rightWeaponArr = default;
    private MasterShaderScript[] _leftWeaponArr = default;

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

        SkinnedMeshRenderer updatedBodyMesh = PartMeshChange.UpdateMeshRenderer(_bodySkinnedMeshRenderer, body.skinnedMeshRenderer, transform.parent);
        _bodySkinnedMeshRenderer = updatedBodyMesh;
        //_body = Instantiate(_equipment.body.meshPrefab[0], _bodySpawnPosition);
        //_body.transform.localPosition = Vector3.zero;
        
        _bodyShader.SetData(body.masterShader, body.bodyMaterial, body.jointsMaterial, body.armorMaterial);
        _bodyShader.colorMecha = _equipment.GetBodyColor();
        UpdateBodyColor(_equipment.GetBodyColor());

        //_leftArm = Instantiate(_equipment.body.armsMeshPrefab[0], _leftArmSpawnPosition);
        //_leftArm.transform.localPosition = Vector3.zero;
        //_leftArmShader = _leftArm.GetComponent<MasterShaderScript>();
        //_leftArmShader.colorMecha = _equipment.GetBodyColor();

        Gun leftGun = Instantiate(_equipment.leftGun.prefab, _leftGunSpawn);
        leftGun.transform.localPosition = Vector3.zero;
        _leftGun = leftGun.gameObject;

        //_rightArm = Instantiate(_equipment.body.armsMeshPrefab[1], _rightArmSpawnPosition);
        //_rightArm.transform.localPosition = Vector3.zero;
        //_rightArmShader = _rightArm.GetComponent<MasterShaderScript>();
        //_rightArmShader.colorMecha = _equipment.GetBodyColor();

        Gun rightGun = Instantiate(_equipment.rightGun.prefab, _rightGunSpawn.transform);
        rightGun.transform.localPosition = Vector3.zero;
        _rightGun = rightGun.gameObject;

        LegsSO legs = _equipment.legs;
        SkinnedMeshRenderer updatedLegsMesh = PartMeshChange.UpdateMeshRenderer(_legsSkinnedMeshRenderer, legs.skinnedMeshRenderer, transform.parent);
        _legsSkinnedMeshRenderer = updatedLegsMesh;

        _legsShader.SetData(legs.masterShader, legs.bodyMaterial, legs.jointsMaterial, legs.armorMaterial);
        _legsShader.colorMecha = _equipment.GetLegsColor();
        UpdateLegsColor(_equipment.GetLegsColor());

        //_leftLeg = Instantiate(_equipment.legs.meshPrefab[0], _leftLegSpawnPosition);
        //_leftLeg.transform.localPosition = Vector3.zero;
        //_leftLegShader = _leftLeg.GetComponent<MasterShaderScript>();
        //_leftLegShader.colorMecha = _equipment.GetLegsColor();

        SetLeftWeaponArray();
        SetRightWeaponArray();

        //_rightLeg = Instantiate(_equipment.legs.meshPrefab[1], _rightLegSpawnPosition);
        //_rightLeg.transform.localPosition = Vector3.zero;
        //_rightLegShader = _rightLeg.GetComponent<MasterShaderScript>();
        //_rightLegShader.colorMecha = _equipment.GetLegsColor();
    }

    public void ChangeBody(BodySO newBody)
    {
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
        //if (_leftGun) Destroy(_leftGun);

        //var gun = Instantiate(newGun.prefab, _leftGunSpawn);
        //gun.transform.localPosition = Vector3.zero;
        //_leftGun = gun.gameObject;
        //SetLeftWeaponArray();
    }

    public void ChangeRightGun(GunSO newGun)
    {
        //if (_rightGun) Destroy(_rightGun);

        //var gun = Instantiate(newGun.prefab, _rightGunSpawn);
        //gun.transform.localPosition = Vector3.zero;
        //_rightGun = gun.gameObject;
        //SetRightWeaponArray();
    }

    public void ChangeLegs(LegsSO newLegs)
    {
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
        _leftWeaponArr = new MasterShaderScript[_leftGun.transform.childCount];

        for (int i = 0; i < _leftGun.transform.childCount; i++)
        {
            Transform child = _leftGun.transform.GetChild(i);

            MasterShaderScript masterShader = child.GetComponent<MasterShaderScript>();
            if (masterShader != null)
            {
                _leftWeaponArr[i] = masterShader;
            }
        }
    }

    private void SetRightWeaponArray()
    {
        _rightWeaponArr = new MasterShaderScript[_rightGun.transform.childCount];

        for (int i = 0; i < _rightGun.transform.childCount; i++)
        {
            Transform child = _rightGun.transform.GetChild(i);

            MasterShaderScript masterShader = child.GetComponent<MasterShaderScript>();
            if (masterShader != null)
            {
                _rightWeaponArr[i] = masterShader;
            }
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
    public MasterShaderScript[] GetLeftWeaponShaderArray()
    {
        return _leftWeaponArr;
    }

    public MasterShaderScript[] GetRightWeaponShaderArray()
    {
        return _rightWeaponArr;
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

    private enum WeaponSide
    {
        rightSide,
        leftSide
    };
}
