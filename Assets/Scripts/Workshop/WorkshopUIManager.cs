﻿using System;
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
        UpdateOverviewText(3);

        _weightTextColor = _weightText.color;//Me guardo el color inicial del texto
    }

    private void UpdateOverviewText(int mechaIndex)
    {
        var equipmentData = _workshopManager.GetMechaEquipment(mechaIndex);

        _overviewName.text = equipmentData.mechaName;

        //_overviewBody.text = "Body: \n" + equipmentData.body.partName;
        //if (equipmentData.body.ability)
        //    _overviewBodyAbility.sprite = equipmentData.body.ability.equipableIcon;
        //else _overviewBodyAbility.sprite = _noneIcon;

        _overviewLeftArm.text = "Left Gun: \n" + equipmentData.leftGun.gunName;
        if (equipmentData.leftGun.ability)
            _overviewLeftArmAbility.sprite = equipmentData.leftGun.ability.equipableIcon;
        else _overviewLeftArmAbility.sprite = _noneIcon;

        _overviewRightArm.text = "Right Gun: \n" + equipmentData.rightGun.gunName;
        if (equipmentData.rightGun.ability)
            _overviewRightArmAbility.sprite = equipmentData.rightGun.ability.equipableIcon;
        else _overviewRightArmAbility.sprite = _noneIcon;

        _overviewLegs.text = "Legs: \n" + equipmentData.legs.partName;
        //if (equipmentData.legs.ability)
        //    _overviewLegsAbility.sprite = equipmentData.legs.ability.equipableIcon;
        //else _overviewLegsAbility.sprite = _noneIcon;

        //if (equipmentData.body.item)
        //    _overviewItem.sprite = equipmentData.body.item.equipableIcon;
        //else _overviewItem.sprite = _noneIcon;

        AbilitiesTabSetAbilitiesIcons();
    }

    public void UpdateWeightSlider(int mechaIndex)
    {
        var equipmentData = _workshopManager.GetMechaEquipment(mechaIndex);

        float weight = equipmentData.body.weight + equipmentData.leftGun.weight + equipmentData.rightGun.weight +
        equipmentData.legs.weight;

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

        var index = _workshopManager.GetIndex();


        var manager = FindObjectOfType<WorkshopManager>();
        var currentMechaEquipment = manager.GetMechaEquipment(index);
        switch (part)
        {
            case "Body":
                var bodies = WorkshopDatabaseManager.Instance.GetBodies();
                foreach (var body in bodies)
                {


                    var b = _workshopManager.CreateWorkshopObject(body, _partsSpawnParent);
                    b.SetLeftClick(() =>
                    {
                        _partsDescription.text = "HP: " + body.maxHP +
                    "\n Weight: " + body.weight +
                    "\n MaxWeight: " + body.maxWeight;

                        b.Select();

                        manager.UpdateBody(body);

                        UpdateWeightSlider(index);
                    });

                    if (currentMechaEquipment.body.partName == body.partName)
                    {
                        b.Select();
                        _partsDescription.text = "HP: " + body.maxHP +
                                        "\n Weight: " + body.weight +
                                        "\n MaxWeight: " + body.maxWeight;
                    }
                }
                break;

            case "LeftArm":
                var gunsA = WorkshopDatabaseManager.Instance.GetGuns();
                foreach (var gun in gunsA)
                {
                    var a = _workshopManager.CreateWorkshopObject(gun, _partsSpawnParent);
                    a.SetLeftClick(() =>
                    {
                        _partsDescription.text = "Damage: " + gun.damage +
                    "\n HitChance:" + gun.hitChance +
                    "\n Attack Range: " + gun.attackRange +
                    "\n Weight: " + gun.weight;

                        a.Select();

                        manager.UpdateLeftGun(gun);

                        UpdateWeightSlider(index);
                    });

                    if (currentMechaEquipment.leftGun.gunName == gun.gunName)
                    {
                        a.Select();
                        _partsDescription.text = "Damage: " + gun.damage +
                                        "\n HitChance:" + gun.hitChance +
                                        "\n Attack Range: " + gun.attackRange +
                                        "\n Weight: " + gun.weight;
                    }
                }
                break;

            case "RightArm":
                var gunsB = WorkshopDatabaseManager.Instance.GetGuns();
                foreach (var gun in gunsB)
                {
                    var a = _workshopManager.CreateWorkshopObject(gun, _partsSpawnParent);
                    a.SetLeftClick(() =>
                    {
                        _partsDescription.text = "Damage: " + gun.damage +
                    "\n HitChance:" + gun.hitChance +
                    "\n Attack Range: " + gun.attackRange +
                    "\n Weight: " + gun.weight;

                        a.Select();

                        manager.UpdateRightGun(gun);

                        UpdateWeightSlider(index);
                    });

                    if (currentMechaEquipment.rightGun.gunName == gun.gunName)
                    {
                        a.Select();
                        _partsDescription.text = "Damage: " + gun.damage +
                                        "\n HitChance:" + gun.hitChance +
                                        "\n Attack Range: " + gun.attackRange +
                                        "\n Weight: " + gun.weight;
                    }
                }
                break;

            case "Legs":
                var legs = WorkshopDatabaseManager.Instance.GetLegs();
                foreach (var leg in legs)
                {
                    var l = _workshopManager.CreateWorkshopObject(leg, _partsSpawnParent);
                    l.SetLeftClick(() =>
                    {
                        _partsDescription.text = "HP: " + leg.maxHP +
                    "\n Steps: " + leg.maxSteps +
                    "\n Weight: " + leg.weight;

                        l.Select();

                        manager.UpdateLegs(leg);

                        UpdateWeightSlider(index);
                    });

                    if (currentMechaEquipment.legs.partName == leg.partName)
                    {
                        l.Select();
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

        var index = _workshopManager.GetIndex();
        var currentMechaEquipment = _workshopManager.GetMechaEquipment(index);

        switch (part)
        {

            case "Body":
                foreach (var ability in abilities)
                {
                    if (ability.partSlot == AbilitySO.PartSlot.Body)
                    {
                        var obj = _workshopManager.CreateWorkshopObject(ability, _abilitiesSpawnParent);
                        obj.SetLeftClick(() =>
                        {
                            _abilitiesDescription.text = ability.description;
                            if (ability.equipableIcon)
                                _bodyAbilityImage.sprite = ability.equipableIcon;

                            obj.Select();

                            OnChangeEquippable?.Invoke(ability, part);
                        });

                        //if (currentMechaEquipment.body.ability)
                        //{
                        //    if (currentMechaEquipment.body.ability.equipableName == ability.equipableName)
                        //    {
                        //        obj.Select();
                        //        _abilitiesDescription.text = ability.description;
                        //    }
                        //}
                    }
                }

                break;

            case "LeftArm":

                foreach (var ability in abilities)
                {
                    if (ability.partSlot == AbilitySO.PartSlot.Arm)
                    {
                        var obj = _workshopManager.CreateWorkshopObject(ability, _abilitiesSpawnParent);
                        obj.SetLeftClick(() =>
                        {
                            _abilitiesDescription.text = ability.description;
                            if (ability.equipableIcon)
                                _leftArmAbilityImage.sprite = ability.equipableIcon;

                            obj.Select();
                            OnChangeEquippable?.Invoke(ability, part);
                        });

                        if (currentMechaEquipment.leftGun.ability)
                        {
                            if (currentMechaEquipment.leftGun.ability.equipableName == ability.equipableName)
                            {
                                obj.Select();
                                _abilitiesDescription.text = ability.description;
                            }
                        }
                    }
                }
                break;

            case "RightArm":

                foreach (var ability in abilities)
                {
                    if (ability.partSlot == AbilitySO.PartSlot.Arm)
                    {
                        var obj = _workshopManager.CreateWorkshopObject(ability, _abilitiesSpawnParent);
                        obj.SetLeftClick(() =>
                        {
                            _abilitiesDescription.text = ability.description;
                            if (ability.equipableIcon)
                                _rightArmAbilityImage.sprite = ability.equipableIcon;

                            obj.Select();

                            OnChangeEquippable?.Invoke(ability, part);
                        });

                        if (currentMechaEquipment.rightGun.ability)
                        {
                            if (currentMechaEquipment.rightGun.ability.equipableName == ability.equipableName)
                            {
                                obj.Select();
                                _abilitiesDescription.text = ability.description;
                            }
                        }
                    }
                }

                break;

            case "Legs":

                foreach (var ability in abilities)
                {
                    if (ability.partSlot == AbilitySO.PartSlot.Legs)
                    {
                        var obj = _workshopManager.CreateWorkshopObject(ability, _abilitiesSpawnParent);
                        obj.SetLeftClick(() =>
                        {
                            _abilitiesDescription.text = ability.description;
                            if (ability.equipableIcon)
                                _legsAbilityImage.sprite = ability.equipableIcon;

                            obj.Select();

                            OnChangeEquippable?.Invoke(ability, part);
                        });

                        //if (currentMechaEquipment.legs.ability)
                        //{
                        //    if (currentMechaEquipment.legs.ability.equipableName == ability.equipableName)
                        //    {
                        //        obj.Select();
                        //        _abilitiesDescription.text = ability.description;
                        //    }
                        //}

                    }

                }
                break;
        }
    }

    public void UpdateItemsList()
    {
        _workshopManager.DestroyWorkshopObjects();

        _itemsDescription.text = "";
        var items = WorkshopDatabaseManager.Instance.GetItems();

        var index = _workshopManager.GetIndex();
        var currentMechaEquipment = _workshopManager.GetMechaEquipment(index);

        foreach (var item in items)
        {
            var obj = _workshopManager.CreateWorkshopObject(item, _itemsSpawnParent);
            obj.SetLeftClick(() =>
            {
                _itemsDescription.text = item.description;
    //TODO: retirar checkeo cuando haya iconos.
    if (item.equipableIcon)
                    _itemImage.sprite = item.equipableIcon;

                obj.Select();

                OnChangeEquippable?.Invoke(item, "Item");
            });

            //if (currentMechaEquipment.body.item)
            //{
            //    if (currentMechaEquipment.body.item.equipableName == item.equipableName)
            //    {
            //        obj.Select();
            //        _itemsDescription.text = item.description;
            //    }
            //}
        }
    }

    public void UpdateBodyRedColor()
    {
        var color = _bodyColorImage.color;
        color.r = _bodyRedSlider.value;
        _bodyColorImage.color = color;
        OnBodyColorChange?.Invoke(color);
    }

    public void UpdateBodyGreenColor()
    {
        var color = _bodyColorImage.color;
        color.g = _bodyGreenSlider.value;
        _bodyColorImage.color = color;
        OnBodyColorChange?.Invoke(color);
    }

    public void UpdateBodyBlueColor()
    {
        var color = _bodyColorImage.color;
        color.b = _bodyBlueSlider.value;
        _bodyColorImage.color = color;
        OnBodyColorChange?.Invoke(color);
    }

    public void UpdateLegsRedColor()
    {
        var color = _legsColorImage.color;
        color.r = _legsRedSlider.value;
        _legsColorImage.color = color;
        OnLegsColorChange?.Invoke(color);
    }

    public void UpdateLegsGreenColor()
    {
        var color = _legsColorImage.color;
        color.g = _legsGreenSlider.value;
        _legsColorImage.color = color;
        OnLegsColorChange?.Invoke(color);
    }

    public void UpdateLegsBlueColor()
    {
        var color = _legsColorImage.color;
        color.b = _legsBlueSlider.value;
        _legsColorImage.color = color;
        OnLegsColorChange?.Invoke(color);
    }

    public void UpdateMechaName()
    {
        var mechaName = _nameField.text;

        if (mechaName.Length >= NAME_MAX_CHARS)
        {
            char[] chars = new char[NAME_MAX_CHARS];
            var nameAsCharArray = mechaName.ToCharArray();

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
        var index = _workshopManager.GetIndex();
        var color = _workshopManager.GetMechaEquipment(index).GetBodyColor();
        _bodyColorImage.color = color;
        _bodyRedSlider.value = color.r;
        _bodyGreenSlider.value = color.g;
        _bodyBlueSlider.value = color.b;
    }

    public void CustomizeTabSetLegsColorSliders()
    {       
        var index = _workshopManager.GetIndex();
        var color = _workshopManager.GetMechaEquipment(index).GetLegsColor();
        _legsColorImage.color = color;
        _legsRedSlider.value = color.r;
        _legsGreenSlider.value = color.g;
        _legsBlueSlider.value = color.b;
    }

    public void CustomizeTabSetMechaName()
    {
        var index = _workshopManager.GetIndex();
        _nameField.text = _workshopManager.GetMechaEquipment(index).mechaName;
    }

    public void AbilitiesTabSetAbilitiesIcons()
    {
        var index = _workshopManager.GetIndex();

        var currentMechaEquipment = _workshopManager.GetMechaEquipment(index);

        //if (currentMechaEquipment.body.ability)
        //{
        //    _bodyAbilityImage.sprite = currentMechaEquipment.body.ability.equipableIcon;
        //}
        //else _bodyAbilityImage.sprite = _noneIcon;

        if (currentMechaEquipment.leftGun.ability)
        {
            _leftArmAbilityImage.sprite = currentMechaEquipment.leftGun.ability.equipableIcon;
        }
        else _leftArmAbilityImage.sprite = _noneIcon;

        if (currentMechaEquipment.rightGun.ability)
        {
            _rightArmAbilityImage.sprite = currentMechaEquipment.rightGun.ability.equipableIcon;
        }
        else _rightArmAbilityImage.sprite = _noneIcon;

        //if (currentMechaEquipment.legs.ability)
        //{
        //    _legsAbilityImage.sprite = currentMechaEquipment.legs.ability.equipableIcon;
        //}
        //else _legsAbilityImage.sprite = _noneIcon;
    }

    public void ItemsTabSetItemIcon()
    {
        var index = _workshopManager.GetIndex();

        var currentMechaEquipment = _workshopManager.GetMechaEquipment(index);

        //var item = currentMechaEquipment.body.item;

        //if (item)
        //{
        //    var icon = item.equipableIcon;

        //    if (icon)
        //        _itemImage.sprite = icon;
        //}

        //else _itemImage.sprite = _noneIcon;
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
