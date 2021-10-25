using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class WorkshopMecha : MonoBehaviour
{
    [SerializeField] private MechaEquipmentSO _equipment;

    [SerializeField] private Transform _bodySpawnPosition;
    [SerializeField] private Transform _leftArmSpawnPosition;
    [SerializeField] private Transform _rightArmSpawnPosition;
    [SerializeField] private Transform _leftGunSpawn;
    [SerializeField] private Transform _rightGunSpawn;
    [SerializeField] private Transform _leftLegSpawnPosition;
    [SerializeField] private Transform _rightLegSpawnPosition;

    private GameObject _body;
    private GameObject _leftArm;
    private GameObject _rightArm;
    private GameObject _leftLeg;
    private GameObject _rightLeg;
    private GameObject _leftGun;
    private GameObject _rightGun;
    public void SetEquipment(MechaEquipmentSO equipment)
    {
        _equipment = equipment;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            SpawnParts();
    }

    public void SpawnParts()
    {
        _body = Instantiate(_equipment.body.meshPrefab[0], _bodySpawnPosition);
        _body.transform.localPosition = Vector3.zero;

        _leftArm = Instantiate(_equipment.leftArm.meshPrefab[0], _leftArmSpawnPosition);

        _leftArm.transform.localPosition = Vector3.zero;


        if (_equipment.leftGun)
        {
            var leftGun = Instantiate(_equipment.leftGun.prefab, _leftGunSpawn.transform);

            if (leftGun)
            {
                leftGun.transform.localPosition = Vector3.zero;
                _leftGun = leftGun.gameObject;
            } 
        }
        
        
        _rightArm = Instantiate(_equipment.rightArm.meshPrefab[1], _rightArmSpawnPosition);
        _rightArm.transform.localPosition = Vector3.zero;

        if (_equipment.rightGun)
        {
            var rightGun = Instantiate(_equipment.rightGun.prefab, _rightGunSpawn.transform);

            if (rightGun)
            {
                rightGun.transform.localPosition = Vector3.zero;
                _rightGun = rightGun.gameObject;
            } 
        }
        
        _leftLeg = Instantiate(_equipment.legs.meshPrefab[0], _leftLegSpawnPosition);
        _leftLeg.transform.localPosition = Vector3.zero;
        
        _rightLeg = Instantiate(_equipment.legs.meshPrefab[1], _rightLegSpawnPosition);
        _rightLeg.transform.localPosition = Vector3.zero;

    }

    public void ChangeBody(BodySO newBody)
    {
        if (_body) Destroy(_body);
        
        var body = Instantiate(newBody.prefab, _bodySpawnPosition);
        body.transform.localPosition = Vector3.zero;
        body.SetPart(newBody);
        _body = body.gameObject;
    }
    
    public void ChangeLeftGun(GunSO newGun)
    {
        if (_leftGun) Destroy(_leftGun);
        
        var gun = Instantiate(newGun.prefab, _leftGunSpawn);
        gun.transform.localPosition = Vector3.zero;
        gun.SetRightOrLeft("Left");
        _leftGun = gun.gameObject;
    }
    
    public void ChangeRightGun(GunSO newGun)
    {
        if (_rightGun) Destroy(_rightGun);
        
        var gun = Instantiate(newGun.prefab, _rightGunSpawn);
        gun.transform.localPosition = Vector3.zero;
        gun.SetRightOrLeft("Right");
        _rightGun = gun.gameObject;
    }
    
    public void ChangeLegs(LegsSO newLegs)
    {
        if (_leftLeg && _rightLeg)
        {
            Destroy(_leftLeg);
            Destroy(_rightLeg);
        }
        var legs = Instantiate(newLegs.prefab, _rightLegSpawnPosition);

        legs.transform.localPosition = Vector3.zero;
        legs.SetPart(_equipment.legs);

        _leftLeg = legs.gameObject;
    }

    public MechaEquipmentSO GetEquipment()
    {
        return _equipment;
    }
}
