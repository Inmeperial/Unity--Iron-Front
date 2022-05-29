using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorkshopUIManager : MonoBehaviour
{

    private readonly int NAME_MAX_CHARS = 9;
    [Header("References")]
    [SerializeField] private WorkshopManager _workshopManager;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Sprite _noneIcon;

    [Header("Overview Texts")]
    [SerializeField] private TextMeshProUGUI _overviewName;
    [SerializeField] private TextMeshProUGUI _overviewBody;
    [SerializeField] private Image _overviewBodyAbility;
    [SerializeField] private TextMeshProUGUI _overviewLeftArm;
    [SerializeField] private Image _overviewLeftArmAbility;
    [SerializeField] private TextMeshProUGUI _overviewRightArm;
    [SerializeField] private Image _overviewRightArmAbility;
    [SerializeField] private TextMeshProUGUI _overviewLegs;
    [SerializeField] private Image _overviewLegsAbility;
    [SerializeField] private Image _overviewItem;



    [Header("Parts List")]
    [SerializeField] private Transform _partsSpawnParent;
    [SerializeField] private TextMeshProUGUI _partsDescription;
    [SerializeField] private TextMeshProUGUI _weightText;
    private Color _weightTextColor;//Guardo el color para cambiarlo si esta en overweight o no
    [SerializeField] private RectTransform _needle;
    [SerializeField] private Vector3 _maxRotationMaxWeight;
    [SerializeField] private Vector3 _maxRotationOverweight;
    [SerializeField] private float _needleRotationTime;

    [Header("Abilities List")]
    [SerializeField] private Transform _abilitiesSpawnParent;
    [SerializeField] private TextMeshProUGUI _abilitiesDescription;
    [SerializeField] private Image _bodyAbilityImage;
    [SerializeField] private Image _leftArmAbilityImage;
    [SerializeField] private Image _rightArmAbilityImage;
    [SerializeField] private Image _legsAbilityImage;

    [Header("Items List")]
    [SerializeField] private Transform _itemsSpawnParent;
    [SerializeField] private TextMeshProUGUI _itemsDescription;
    [SerializeField] private Image _itemImage;

    [Header("Colors")]
    [SerializeField] private Image _bodyColorImage;

    [SerializeField] private Slider _bodyRedSlider;
    [SerializeField] private Slider _bodyGreenSlider;
    [SerializeField] private Slider _bodyBlueSlider;

    [Space]
    [SerializeField] private Image _legsColorImage;
    [SerializeField] private Slider _legsRedSlider;
    [SerializeField] private Slider _legsGreenSlider;
    [SerializeField] private Slider _legsBlueSlider;

    [Space]
    [SerializeField] private TMP_InputField _nameField;

    public delegate void ChangeEquippable(EquipableSO equippableSo, string location);

    public event ChangeEquippable OnChangeEquippable;

    public delegate void ColorChange(Color color);

    public event ColorChange OnBodyColorChange;
    public event ColorChange OnLegsColorChange;

    public event ColorChange OnCopyColorToAllBodies;

    public event ColorChange OnCopyColorToAllLegs;

    public delegate void NameChange(string name);

    public event NameChange OnNameChange;
    private void Start()
    {
        _workshopManager.OnClickPrevious += UpdateOverviewText;
        _workshopManager.OnClickNext += UpdateOverviewText;
        _workshopManager.OnClickCloseEdit += UpdateOverviewText;
        _workshopManager.OnClickEdit += UpdateWeightSlider;
        _workshopManager.OnClickMecha += UpdateOverviewText;
        UpdateOverviewText(_workshopManager.GetMechaIndex());

        _weightTextColor = _weightText.color;//Me guardo el color inicial del texto
    }

    private void UpdateOverviewText(int mechaIndex)
    {
        MechaEquipmentSO equipmentData = _workshopManager.GetMechaEquipment(mechaIndex);

        _overviewName.text = equipmentData.mechaName;

        _overviewBody.text = "Body: \n" + equipmentData.body.partName;
        if (equipmentData.bodyAbility)
            _overviewBodyAbility.sprite = equipmentData.bodyAbility.equipableIcon;
        else
            _overviewBodyAbility.sprite = _noneIcon;

        _overviewLeftArm.text = "Left Gun: \n" + equipmentData.leftGun.gunName;
        if (equipmentData.leftGunAbility)
            _overviewLeftArmAbility.sprite = equipmentData.leftGunAbility.equipableIcon;
        else
            _overviewLeftArmAbility.sprite = _noneIcon;

        _overviewRightArm.text = "Right Gun: \n" + equipmentData.rightGun.gunName;
        if (equipmentData.rightGunAbility) 
            _overviewRightArmAbility.sprite = equipmentData.rightGunAbility.equipableIcon;
        else
            _overviewRightArmAbility.sprite = _noneIcon;

        _overviewLegs.text = "Legs: \n" + equipmentData.legs.partName;
        if (equipmentData.legsAbility)
            _overviewLegsAbility.sprite = equipmentData.legsAbility.equipableIcon;
        else
            _overviewLegsAbility.sprite = _noneIcon;

        if (equipmentData.item) 
            _overviewItem.sprite = equipmentData.item.equipableIcon;
        else 
            _overviewItem.sprite = _noneIcon;

        AbilitiesTabSetAbilitiesIcons();
    }

    public void UpdateWeightSlider(int mechaIndex)
    {
        MechaEquipmentSO equipmentData = _workshopManager.GetMechaEquipment(mechaIndex);

        float weight = equipmentData.body.weight + equipmentData.leftGun.weight + equipmentData.rightGun.weight + equipmentData.legs.weight;

        float maxWeight = equipmentData.body.maxWeight;

        _weightText.text = weight + "";

        _weightText.color = weight > maxWeight ? Color.red : _weightTextColor;//Le cambio el color del texto si llega al overweight 

        StopCoroutine(RotateNeedle(Quaternion.identity));

        Vector3 newRotation = (weight * _maxRotationMaxWeight) / maxWeight;

        if (newRotation.z < _maxRotationOverweight.z) 
            newRotation.z = _maxRotationOverweight.z;

        Quaternion target = Quaternion.Euler(newRotation);

        StartCoroutine(RotateNeedle(target));
    }

    IEnumerator RotateNeedle(Quaternion target)
    {
        float time = 0;
        Quaternion needleRot = _needle.rotation;

        while (time < _needleRotationTime)
        {
            _needle.rotation = Quaternion.Lerp(needleRot, target, time / _needleRotationTime);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _needle.rotation = target;
    }

    public void UpdatePartsList(string part)
    {
        _workshopManager.DestroyWorkshopObjects();

        int mechaIndex = _workshopManager.GetMechaIndex();

        MechaEquipmentSO currentMechaEquipment = _workshopManager.GetMechaEquipment(mechaIndex);

        //TODO: Revisar switch
        switch (part)
        {
            case "Body":
                List<BodySO> bodies = WorkshopDatabaseManager.Instance.GetBodies();
                foreach (BodySO body in bodies)
                {
                    WorkshopObjectButton button = _workshopManager.CreateWorkshopObjectButton(body, _partsSpawnParent);
                    button.SetLeftClick(() =>
                    {
                        _partsDescription.text = "HP: " + body.maxHP +
                                                "\n Weight: " + body.weight +
                                                "\n MaxWeight: " + body.maxWeight;

                        button.Select();

                        _workshopManager.UpdateBody(body);

                        UpdateWeightSlider(mechaIndex);
                    });

                    if (currentMechaEquipment.body.partName == body.partName)
                    {
                        button.Select();
                        _partsDescription.text = "HP: " + body.maxHP +
                                                "\n Weight: " + body.weight +
                                                "\n MaxWeight: " + body.maxWeight;
                    }
                }
                break;

            case "LeftArm":
                List<GunSO> leftGuns = WorkshopDatabaseManager.Instance.GetGuns();
                foreach (GunSO gun in leftGuns)
                {
                    WorkshopObjectButton button = _workshopManager.CreateWorkshopObjectButton(gun, _partsSpawnParent);
                    button.SetLeftClick(() =>
                    {
                        _partsDescription.text = "Damage: " + gun.damage +
                                                "\n HitChance:" + gun.hitChance +
                                                "\n Attack Range: " + gun.attackRange +
                                                "\n Weight: " + gun.weight;

                        button.Select();

                        _workshopManager.UpdateLeftGun(gun);

                        UpdateWeightSlider(mechaIndex);
                    });

                    if (currentMechaEquipment.leftGun.gunName == gun.gunName)
                    {
                        button.Select();
                        _partsDescription.text = "Damage: " + gun.damage +
                                                "\n HitChance:" + gun.hitChance +
                                                "\n Attack Range: " + gun.attackRange +
                                                "\n Weight: " + gun.weight;
                    }
                }
                break;

            case "RightArm":
                List<GunSO> rightGuns = WorkshopDatabaseManager.Instance.GetGuns();
                foreach (GunSO gun in rightGuns)
                {
                    WorkshopObjectButton button = _workshopManager.CreateWorkshopObjectButton(gun, _partsSpawnParent);
                    button.SetLeftClick(() =>
                    {
                        _partsDescription.text = "Damage: " + gun.damage +
                                                "\n HitChance:" + gun.hitChance +
                                                "\n Attack Range: " + gun.attackRange +
                                                "\n Weight: " + gun.weight;

                        button.Select();

                        _workshopManager.UpdateRightGun(gun);

                        UpdateWeightSlider(mechaIndex);
                    });

                    if (currentMechaEquipment.rightGun.gunName == gun.gunName)
                    {
                        button.Select();
                        _partsDescription.text = "Damage: " + gun.damage +
                                                "\n HitChance:" + gun.hitChance +
                                                "\n Attack Range: " + gun.attackRange +
                                                "\n Weight: " + gun.weight;
                    }
                }
                break;

            case "Legs":
                List<LegsSO> legs = WorkshopDatabaseManager.Instance.GetLegs();
                foreach (LegsSO leg in legs)
                {
                    WorkshopObjectButton button = _workshopManager.CreateWorkshopObjectButton(leg, _partsSpawnParent);
                    button.SetLeftClick(() =>
                    {
                        _partsDescription.text = "HP: " + leg.maxHP +
                                                "\n Steps: " + leg.maxSteps +
                                                "\n Weight: " + leg.weight;

                        button.Select();

                        _workshopManager.UpdateLegs(leg);

                        UpdateWeightSlider(mechaIndex);
                    });

                    if (currentMechaEquipment.legs.partName == leg.partName)
                    {
                        button.Select();
                        _partsDescription.text = "HP: " + leg.maxHP +
                                                "\n Steps: " + leg.maxSteps +
                                                "\n Weight: " + leg.weight;
                    }
                }
                break;
        }
    }

    public void UpdateAbilitiesList(string part)
    {
        _workshopManager.DestroyWorkshopObjects();

        _abilitiesDescription.text = "";
        List<AbilitySO> abilities = new List<AbilitySO>();
        abilities = WorkshopDatabaseManager.Instance.GetAbilities();

        int index = _workshopManager.GetMechaIndex();
        MechaEquipmentSO currentMechaEquipment = _workshopManager.GetMechaEquipment(index);

        //TODO: Revisar switch

        switch (part)
        {
            case "Body":
                foreach (AbilitySO ability in abilities)
                {
                    if (ability.partSlot == AbilitySO.PartSlot.Body)
                    {
                        WorkshopObjectButton button = _workshopManager.CreateWorkshopObjectButton(ability, _abilitiesSpawnParent);
                        button.SetLeftClick(() =>
                        {
                            _abilitiesDescription.text = ability.description;
                            if (ability.equipableIcon) _bodyAbilityImage.sprite = ability.equipableIcon;

                            button.Select();

                            OnChangeEquippable?.Invoke(ability, part);
                        });

                        if (currentMechaEquipment.bodyAbility)
                        {
                            if (currentMechaEquipment.bodyAbility.equipableName == ability.equipableName)
                            {
                                button.Select();
                                _abilitiesDescription.text = ability.description;
                            }
                        }
                    }
                }

                break;

            case "LeftArm":

                foreach (AbilitySO ability in abilities)
                {
                    if (ability.partSlot == AbilitySO.PartSlot.Arm)
                    {
                        WorkshopObjectButton button = _workshopManager.CreateWorkshopObjectButton(ability, _abilitiesSpawnParent);
                        button.SetLeftClick(() =>
                        {
                            _abilitiesDescription.text = ability.description;
                            if (ability.equipableIcon) _leftArmAbilityImage.sprite = ability.equipableIcon;

                            button.Select();
                            OnChangeEquippable?.Invoke(ability, part);
                        });

                        if (currentMechaEquipment.leftGunAbility)
                        {
                            if (currentMechaEquipment.leftGunAbility.equipableName == ability.equipableName)
                            {
                                button.Select();
                                _abilitiesDescription.text = ability.description;
                            }
                        }
                    }
                }
                break;

            case "RightArm":

                foreach (AbilitySO ability in abilities)
                {
                    if (ability.partSlot == AbilitySO.PartSlot.Arm)
                    {
                        WorkshopObjectButton button = _workshopManager.CreateWorkshopObjectButton(ability, _abilitiesSpawnParent);
                        button.SetLeftClick(() =>
                        {
                            _abilitiesDescription.text = ability.description;
                            if (ability.equipableIcon) _rightArmAbilityImage.sprite = ability.equipableIcon;

                            button.Select();

                            OnChangeEquippable?.Invoke(ability, part);
                        });

                        if (currentMechaEquipment.rightGunAbility)
                        {
                            if (currentMechaEquipment.rightGunAbility.equipableName == ability.equipableName)
                            {
                                button.Select();
                                _abilitiesDescription.text = ability.description;
                            }
                        }
                    }
                }

                break;

            case "Legs":

                foreach (AbilitySO ability in abilities)
                {
                    if (ability.partSlot == AbilitySO.PartSlot.Legs)
                    {
                        WorkshopObjectButton button = _workshopManager.CreateWorkshopObjectButton(ability, _abilitiesSpawnParent);
                        button.SetLeftClick(() =>
                        {
                            _abilitiesDescription.text = ability.description;
                            if (ability.equipableIcon) _legsAbilityImage.sprite = ability.equipableIcon;

                            button.Select();

                            OnChangeEquippable?.Invoke(ability, part);
                        });

                        if (currentMechaEquipment.legsAbility)
                        {
                            if (currentMechaEquipment.legsAbility.equipableName == ability.equipableName)
                            {
                                button.Select();
                                _abilitiesDescription.text = ability.description;
                            }
                        }
                    }
                }
                break;
        }
    }

    public void UpdateItemsList()
    {
        _workshopManager.DestroyWorkshopObjects();

        _itemsDescription.text = "";

        List<ItemSO> items = WorkshopDatabaseManager.Instance.GetItems();

        int index = _workshopManager.GetMechaIndex();

        MechaEquipmentSO currentMechaEquipment = _workshopManager.GetMechaEquipment(index);

        foreach (ItemSO item in items)
        {
            WorkshopObjectButton button = _workshopManager.CreateWorkshopObjectButton(item, _itemsSpawnParent);
            button.SetLeftClick(() =>
            {
                _itemsDescription.text = item.description;
                _itemImage.sprite = item.equipableIcon;

                button.Select();

                OnChangeEquippable?.Invoke(item, "Item");
            });

            if (currentMechaEquipment.item)
            {
                if (currentMechaEquipment.item.equipableName == item.equipableName)
                {
                    button.Select();
                    _itemsDescription.text = item.description;
                }
            }
        }
    }

    public void UpdateBodyRedColor()
    {
        Color color = _bodyColorImage.color;
        color.r = _bodyRedSlider.value;
        _bodyColorImage.color = color;
        OnBodyColorChange?.Invoke(color);
    }

    public void UpdateBodyGreenColor()
    {
        Color color = _bodyColorImage.color;
        color.g = _bodyGreenSlider.value;
        _bodyColorImage.color = color;
        OnBodyColorChange?.Invoke(color);
    }

    public void UpdateBodyBlueColor()
    {
        Color color = _bodyColorImage.color;
        color.b = _bodyBlueSlider.value;
        _bodyColorImage.color = color;
        OnBodyColorChange?.Invoke(color);
    }

    public void UpdateLegsRedColor()
    {
        Color color = _legsColorImage.color;
        color.r = _legsRedSlider.value;
        _legsColorImage.color = color;
        OnLegsColorChange?.Invoke(color);
    }

    public void UpdateLegsGreenColor()
    {
        Color color = _legsColorImage.color;
        color.g = _legsGreenSlider.value;
        _legsColorImage.color = color;
        OnLegsColorChange?.Invoke(color);
    }

    public void UpdateLegsBlueColor()
    {
        Color color = _legsColorImage.color;
        color.b = _legsBlueSlider.value;
        _legsColorImage.color = color;
        OnLegsColorChange?.Invoke(color);
    }

    public void UpdateMechaName()
    {
        string mechaName = _nameField.text;

        if (mechaName.Length >= NAME_MAX_CHARS)
        {
            char[] chars = new char[NAME_MAX_CHARS];
            char[] nameAsCharArray = mechaName.ToCharArray();

            for (int i = 0; i < NAME_MAX_CHARS; i++)
            {
                chars[i] = nameAsCharArray[i];
            }

            mechaName = new string(chars);
            _nameField.text = mechaName;
        }

        OnNameChange?.Invoke(mechaName);
    }

    public void CopyBodyColorToLegs()
    {
        _legsColorImage.color = _bodyColorImage.color;
        _legsRedSlider.value = _bodyRedSlider.value;
        _legsGreenSlider.value = _bodyGreenSlider.value;
        _legsBlueSlider.value = _bodyBlueSlider.value;

        UpdateLegsRedColor();
        UpdateLegsGreenColor();
        UpdateLegsBlueColor();
    }

    public void CopyLegsColorToBody()
    {
        _bodyColorImage.color = _legsColorImage.color;
        _bodyRedSlider.value = _legsRedSlider.value;
        _bodyGreenSlider.value = _legsGreenSlider.value;
        _bodyBlueSlider.value = _legsBlueSlider.value;

        UpdateBodyRedColor();
        UpdateBodyGreenColor();
        UpdateBodyBlueColor();
    }

    public void CopyColorToAll()
    {
        OnCopyColorToAllBodies?.Invoke(_bodyColorImage.color);
        OnCopyColorToAllLegs?.Invoke(_legsColorImage.color);
    }

    public void CustomizeTabSetBodyColorSliders()
    {
        int index = _workshopManager.GetMechaIndex();
        Color color = _workshopManager.GetMechaEquipment(index).GetBodyColor();
        _bodyColorImage.color = color;
        _bodyRedSlider.value = color.r;
        _bodyGreenSlider.value = color.g;
        _bodyBlueSlider.value = color.b;
    }

    public void CustomizeTabSetLegsColorSliders()
    {
        int index = _workshopManager.GetMechaIndex();
        Color color = _workshopManager.GetMechaEquipment(index).GetLegsColor();
        _legsColorImage.color = color;
        _legsRedSlider.value = color.r;
        _legsGreenSlider.value = color.g;
        _legsBlueSlider.value = color.b;
    }

    public void CustomizeTabSetMechaName()
    {
        int index = _workshopManager.GetMechaIndex();
        _nameField.text = _workshopManager.GetMechaEquipment(index).mechaName;
    }

    public void AbilitiesTabSetAbilitiesIcons()
    {
        int index = _workshopManager.GetMechaIndex();

        MechaEquipmentSO currentMechaEquipment = _workshopManager.GetMechaEquipment(index);

        if (currentMechaEquipment.bodyAbility)
            _bodyAbilityImage.sprite = currentMechaEquipment.bodyAbility.equipableIcon;
        else
            _bodyAbilityImage.sprite = _noneIcon;

        if (currentMechaEquipment.leftGunAbility)
            _leftArmAbilityImage.sprite = currentMechaEquipment.leftGunAbility.equipableIcon;
        else 
            _leftArmAbilityImage.sprite = _noneIcon;

        if (currentMechaEquipment.rightGunAbility)
            _rightArmAbilityImage.sprite = currentMechaEquipment.rightGunAbility.equipableIcon;
        else 
            _rightArmAbilityImage.sprite = _noneIcon;

        if (currentMechaEquipment.legsAbility) 
            _legsAbilityImage.sprite = currentMechaEquipment.legsAbility.equipableIcon;
        else 
            _legsAbilityImage.sprite = _noneIcon;
    }

    public void ItemsTabSetItemIcon()
    {
        int index = _workshopManager.GetMechaIndex();

        MechaEquipmentSO currentMechaEquipment = _workshopManager.GetMechaEquipment(index);

        ItemSO item = currentMechaEquipment.item;

        if (item)
        {
            Sprite icon = item.equipableIcon;

            if (icon)
                _itemImage.sprite = icon;
        }
        else
            _itemImage.sprite = _noneIcon;
    }

    private void OnDestroy()
    {
        _workshopManager.OnClickPrevious -= UpdateOverviewText;
        _workshopManager.OnClickNext -= UpdateOverviewText;
        _workshopManager.OnClickCloseEdit -= UpdateOverviewText;
        _workshopManager.OnClickEdit -= UpdateWeightSlider;
        _workshopManager.OnClickMecha -= UpdateOverviewText;
    }
}
