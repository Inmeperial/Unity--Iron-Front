using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopMecha : MonoBehaviour
{
    private MechaEquipmentSO _equipment;

    [SerializeField] private Transform _bodySpawnPosition;
    [SerializeField] private Transform _leftArmSpawnPosition;
    [SerializeField] private Transform _rightArmSpawnPosition;
    [SerializeField] private Transform _leftGunSpawn;
    [SerializeField] private Transform _rightGunSpawn;
    [SerializeField] private Transform _leftLegSpawnPosition;
    [SerializeField] private Transform _rightLegSpawnPosition;

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
        
        var leftArm = Instantiate(_equipment.leftArm.prefab, _leftArmSpawnPosition);

        leftArm.transform.localPosition = Vector3.zero;
        leftArm.SetPart(_equipment.leftArm);
        leftArm.SetRightOrLeft("Left");
        

        if (_equipment.leftGun)
        {
            var leftGun = Instantiate(_equipment.leftGun.prefab, _leftGunSpawn.transform);

            if (leftGun)
            {
                leftGun.transform.localPosition = Vector3.zero;
                leftGun.gameObject.tag = "LArm";
            } 
        }
        
        
        var rightArm = Instantiate(_equipment.rightArm.prefab, _rightArmSpawnPosition);
        rightArm.transform.localPosition = Vector3.zero;
        rightArm.SetPart(_equipment.rightArm);
        rightArm.SetRightOrLeft("Right");
        
        if (_equipment.rightGun)
        {
            var rightGun = Instantiate(_equipment.rightGun.prefab, _rightGunSpawn.transform);

            if (rightGun)
            {
                rightGun.transform.localPosition = Vector3.zero;
                rightGun.gameObject.tag = "RArm";
            } 
        }
        
        var legs = Instantiate(_equipment.legs.prefab, _rightLegSpawnPosition);
        //1 is right 0 is left
        Destroy(legs.meshFilter[0].gameObject);
        var otherLeg = Instantiate(_equipment.legs.prefab, _leftLegSpawnPosition);
        Destroy(otherLeg.meshFilter[1].gameObject);
        otherLeg.gameObject.name = "other leg";
        
        legs.CreateRightLeg(_equipment.legs.mesh[1]);
        otherLeg.CreateLeftLeg(_equipment.legs.mesh[0]);
        
        legs.transform.localPosition = Vector3.zero;
        legs.SetPart(_equipment.legs);
    }
}
