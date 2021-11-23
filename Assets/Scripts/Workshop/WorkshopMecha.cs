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

    private MasterShaderScript[] _rightWeaponArr = default;
    private MasterShaderScript[] _leftWeaponArr = default;

    private int _index;

    public MasterShaderScript _bodyShader;
    public MasterShaderScript _leftArmShader;
    public MasterShaderScript _rightArmShader;
    public MasterShaderScript _leftLegShader;
    public MasterShaderScript _rightLegShader;

    public void SetEquipment(MechaEquipmentSO equipment, int index)
    {
        _equipment = equipment;
        _index = index;
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
        _bodyShader = _body.GetComponent<MasterShaderScript>();
        _bodyShader.colorMecha = _equipment.GetBodyColor();

        _leftArm = Instantiate(_equipment.body.armsMeshPrefab[0], _leftArmSpawnPosition);
        _leftArm.transform.localPosition = Vector3.zero;
        _leftArmShader = _leftArm.GetComponent<MasterShaderScript>();
        _leftArmShader.colorMecha = _equipment.GetBodyColor();

        var leftGun = Instantiate(_equipment.leftGun.prefab, _leftGunSpawn.transform);
        leftGun.transform.localPosition = Vector3.zero;
        _leftGun = leftGun.gameObject;

        _rightArm = Instantiate(_equipment.body.armsMeshPrefab[1], _rightArmSpawnPosition);
        _rightArm.transform.localPosition = Vector3.zero;
        _rightArmShader = _rightArm.GetComponent<MasterShaderScript>();
        _rightArmShader.colorMecha = _equipment.GetBodyColor();

        var rightGun = Instantiate(_equipment.rightGun.prefab, _rightGunSpawn.transform);
        rightGun.transform.localPosition = Vector3.zero;
        _rightGun = rightGun.gameObject;

        _leftLeg = Instantiate(_equipment.legs.meshPrefab[0], _leftLegSpawnPosition);
        _leftLeg.transform.localPosition = Vector3.zero;
        _leftLegShader = _leftLeg.GetComponent<MasterShaderScript>();
        _leftLegShader.colorMecha = _equipment.GetLegsColor();

        SetLeftWeaponArray();
        SetRightWeaponArray();

        _rightLeg = Instantiate(_equipment.legs.meshPrefab[1], _rightLegSpawnPosition);
        _rightLeg.transform.localPosition = Vector3.zero;
        _rightLegShader = _rightLeg.GetComponent<MasterShaderScript>();
        _rightLegShader.colorMecha = _equipment.GetLegsColor();
    }

    public void ChangeBody(BodySO newBody)
    {
        if (_body && _leftArm && _rightArm)
        {
            Destroy(_body);
            Destroy(_leftArm);
            Destroy(_rightArm);
        }

        _body = Instantiate(newBody.meshPrefab[0], _bodySpawnPosition);
        _body.transform.localPosition = Vector3.zero;
        _bodyShader = _body.GetComponent<MasterShaderScript>();

        _leftArm = Instantiate(newBody.armsMeshPrefab[0], _leftArmSpawnPosition);
        _leftArm.transform.localPosition = Vector3.zero;
        _leftArmShader = _leftArm.GetComponent<MasterShaderScript>();

        _rightArm = Instantiate(newBody.armsMeshPrefab[1], _rightArmSpawnPosition);
        _rightArm.transform.localPosition = Vector3.zero;
        _rightArmShader = _rightArm.GetComponent<MasterShaderScript>();

        _bodyShader.colorMecha = _equipment.GetBodyColor();
        _leftArmShader.colorMecha = _equipment.GetBodyColor();
        _rightArmShader.colorMecha = _equipment.GetBodyColor();
    }

    public void ChangeLeftGun(GunSO newGun)
    {
        if (_leftGun) Destroy(_leftGun);

        var gun = Instantiate(newGun.prefab, _leftGunSpawn);
        gun.transform.localPosition = Vector3.zero;
        _leftGun = gun.gameObject;
        SetLeftWeaponArray();
    }

    public void ChangeRightGun(GunSO newGun)
    {
        if (_rightGun) Destroy(_rightGun);

        var gun = Instantiate(newGun.prefab, _rightGunSpawn);
        gun.transform.localPosition = Vector3.zero;
        _rightGun = gun.gameObject;
        SetRightWeaponArray();
    }

    public void ChangeLegs(LegsSO newLegs)
    {
        if (_leftLeg && _rightLeg)
        {
            Destroy(_leftLeg);
            Destroy(_rightLeg);
        }
        _leftLeg = Instantiate(newLegs.meshPrefab[0], _leftLegSpawnPosition);
        _leftLeg.transform.localPosition = Vector3.zero;
        _leftLegShader = _leftLeg.GetComponent<MasterShaderScript>();

        _rightLeg = Instantiate(newLegs.meshPrefab[1], _rightLegSpawnPosition);
        _rightLeg.transform.localPosition = Vector3.zero;
        _rightLegShader = _rightLeg.GetComponent<MasterShaderScript>();

        _leftLegShader.colorMecha = _equipment.GetLegsColor();
        _rightLegShader.colorMecha = _equipment.GetLegsColor();
    }

    private void SetLeftWeaponArray()
    {
        _leftWeaponArr = new MasterShaderScript[_leftGun.transform.childCount];

        for (int i = 0; i < _leftGun.transform.childCount; i++)
        {
            Transform child = _leftGun.transform.GetChild(i);
            if (child.GetComponent<MasterShaderScript>() != null)
            {
                _leftWeaponArr[i] = child.GetComponent<MasterShaderScript>();
            }
        }
    }

    private void SetRightWeaponArray()
    {
        _rightWeaponArr = new MasterShaderScript[_rightGun.transform.childCount];

        for (int i = 0; i < _rightGun.transform.childCount; i++)
        {
            Transform child = _rightGun.transform.GetChild(i);
            if (child.GetComponent<MasterShaderScript>() != null)
            {
                _rightWeaponArr[i] = child.GetComponent<MasterShaderScript>();
            }
        }
    }

    public MasterShaderScript[] GetLeftWeaponArray()
    {
        return _leftWeaponArr;
    }

    public MasterShaderScript[] GetRightWeaponArray()
    {
        return _rightWeaponArr;
    }

    public MechaEquipmentSO GetEquipment()
    {
        return _equipment;
    }

    public int GetIndex()
    {
        return _index;
    }

    public void UpdateBodyColor(Color color)
    {
        _bodyShader.SetMechaColor(color);
        _leftArmShader.SetMechaColor(color);
        _rightArmShader.SetMechaColor(color);
    }
    
    public void UpdateLegsColor(Color color)
    {
        _leftLegShader.SetMechaColor(color);
        _rightLegShader.SetMechaColor(color);
    }

    private enum WeaponSide
    {
        rightSide,
        leftSide
    };
}
