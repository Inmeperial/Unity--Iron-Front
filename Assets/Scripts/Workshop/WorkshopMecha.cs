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
        var body = Instantiate(_equipment.body.prefab, _bodySpawnPosition);
        body.transform.localPosition = Vector3.zero;
        body.SetPart(_equipment.body);
        _body = body.gameObject;

        var leftArm = Instantiate(_equipment.leftArm.prefab, _leftArmSpawnPosition);

        leftArm.transform.localPosition = Vector3.zero;
        leftArm.SetPart(_equipment.leftArm);
        leftArm.SetRightOrLeft("Left");
        _leftArm = leftArm.gameObject;
        

        if (_equipment.leftGun)
        {
            var leftGun = Instantiate(_equipment.leftGun.prefab, _leftGunSpawn.transform);

            if (leftGun)
            {
                leftGun.transform.localPosition = Vector3.zero;
                leftGun.gameObject.tag = "LArm";
                _leftGun = leftGun.gameObject;
            } 
        }
        
        
        var rightArm = Instantiate(_equipment.rightArm.prefab, _rightArmSpawnPosition);
        rightArm.transform.localPosition = Vector3.zero;
        rightArm.SetPart(_equipment.rightArm);
        rightArm.SetRightOrLeft("Right");
        _rightArm = rightArm.gameObject;
        
        if (_equipment.rightGun)
        {
            var rightGun = Instantiate(_equipment.rightGun.prefab, _rightGunSpawn.transform);

            if (rightGun)
            {
                rightGun.transform.localPosition = Vector3.zero;
                rightGun.gameObject.tag = "RArm";
                _rightGun = rightGun.gameObject;
            } 
        }
        
        var legs = Instantiate(_equipment.legs.prefab, _rightLegSpawnPosition);
        //1 is right 0 is left
        Destroy(legs.meshFilter[0].gameObject);
        var otherLeg = Instantiate(_equipment.legs.prefab, _leftLegSpawnPosition);
        Destroy(otherLeg.meshFilter[1].gameObject);

        legs.CreateRightLeg(_equipment.legs.mesh[1]);
        otherLeg.CreateLeftLeg(_equipment.legs.mesh[0]);
        
        legs.transform.localPosition = Vector3.zero;
        legs.SetPart(_equipment.legs);

        _leftLeg = legs.gameObject;
        _rightLeg = otherLeg.gameObject;
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
        //1 is right 0 is left
        Destroy(legs.meshFilter[0].gameObject);
        var otherLeg = Instantiate(newLegs.prefab, _leftLegSpawnPosition);
        Destroy(otherLeg.meshFilter[1].gameObject);

        legs.CreateRightLeg(_equipment.legs.mesh[1]);
        otherLeg.CreateLeftLeg(_equipment.legs.mesh[0]);
        
        legs.transform.localPosition = Vector3.zero;
        legs.SetPart(_equipment.legs);

        _leftLeg = legs.gameObject;
        _rightLeg = otherLeg.gameObject;
    }

    public MechaEquipmentSO GetEquipment()
    {
        return _equipment;
    }
}
