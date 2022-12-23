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
    [SerializeField] private GameObject _canvas;
    [SerializeField] private FadePostProcessController _fadeController;
    [SerializeField] private WorkshopManager _workshopManager;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Sprite _noneIcon;
    [SerializeField] private PlaySound _clickSound;

    [Header("Overview Texts")]
    [SerializeField] private TextMeshProUGUI _overviewName;
    [SerializeField] private OverviewDataText _overviewBody;
    [SerializeField] private OverviewDataImage _overviewBodyAbility;
    [SerializeField] private OverviewDataText _overviewLeftGun;
    [SerializeField] private OverviewDataImage _overviewLeftArmAbility;
    [SerializeField] private OverviewDataText _overviewRightGun;
    [SerializeField] private OverviewDataImage _overviewRightArmAbility;
    [SerializeField] private OverviewDataText _overviewLegs;
    [SerializeField] private OverviewDataImage _overviewLegsAbility;
    [SerializeField] private OverviewDataText _overviewItemText;
    [SerializeField] private OverviewDataImage _overviewItemImage;



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

    public Action OnCopyAbilityToAll;

    public Action OnCopyItemToAll;

    private void Awake()
    {
        _canvas.SetActive(false);

        _fadeController.OnTransitionFinished += EnableCanvas;
    }

    private void EnableCanvas()
    {
        _canvas.SetActive(true);
        _fadeController.OnTransitionFinished -= EnableCanvas;
    }

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

        string bodyName = "Body: \n" + equipmentData.body.objectName;
        _overviewBody.SetData(bodyName);
        _overviewBody.SetTooltipData(equipmentData.body);

        if (equipmentData.bodyAbility)
        {
            _overviewBodyAbility.SetData(equipmentData.bodyAbility.objectImage);
            _overviewBodyAbility.SetTooltipData(equipmentData.bodyAbility);
        }
        else
        {
            _overviewBodyAbility.SetData(_noneIcon);
            _overviewBodyAbility.SetTooltipData("", "");
        }

        string leftGunName = "Left Gun: \n" + equipmentData.leftGun.objectName;
        _overviewLeftGun.SetData(leftGunName);
        _overviewLeftGun.SetTooltipData(equipmentData.leftGun);

        if (equipmentData.leftGunAbility)
        {
            _overviewLeftArmAbility.SetData(equipmentData.leftGunAbility.objectImage);
            _overviewLeftArmAbility.SetTooltipData(equipmentData.leftGunAbility);
        }
        else
        {
            _overviewLeftArmAbility.SetData(_noneIcon);
            _overviewLeftArmAbility.SetTooltipData("", "");
        }

        string rightGunName = "Right Gun: \n" + equipmentData.rightGun.objectName;
        _overviewRightGun.SetData(rightGunName);
        _overviewRightGun.SetTooltipData(equipmentData.rightGun);

        if (equipmentData.rightGunAbility)
        {
            _overviewRightArmAbility.SetData(equipmentData.rightGunAbility.objectImage);
            _overviewRightArmAbility.SetTooltipData(equipmentData.rightGunAbility);
        }
        else
        {
            _overviewRightArmAbility.SetData(_noneIcon);
            _overviewRightArmAbility.SetTooltipData("", "");
        }

        string legsName = "Legs: \n" + equipmentData.legs.objectName;
        _overviewLegs.SetData(legsName);
        _overviewLegs.SetTooltipData(equipmentData.legs);

        if (equipmentData.legsAbility)
        {
            _overviewLegsAbility.SetData(equipmentData.legsAbility.objectImage);
            _overviewLegsAbility.SetTooltipData(equipmentData.legsAbility);
        }
        else
        {
            _overviewLegsAbility.SetData(_noneIcon);
            _overviewLegsAbility.SetTooltipData("", "");
        }


        if (equipmentData.item)
        {
            _overviewItemText.SetData(equipmentData.item.objectName);
            _overviewItemText.SetTooltipData(equipmentData.item);
            _overviewItemImage.SetData(equipmentData.item.objectImage);
            _overviewItemImage.SetTooltipData(equipmentData.item);
        }
        else
        {
            _overviewItemText.SetData("None");
            _overviewItemText.SetTooltipData("", "");
            _overviewItemImage.SetData(_noneIcon);
            _overviewItemImage.SetTooltipData("", "");
        }

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
                        _clickSound.PlayTheSound();
                        _partsDescription.text = body.maxHP + "  Durability \n" +
                                                body.weight + "  Kg \n" +
                                                body.maxWeight + "  Max Weight";

                        button.Select();

                        _workshopManager.UpdateBody(body);

                        UpdateWeightSlider(mechaIndex);
                    });

                    if (currentMechaEquipment.body.objectName == body.objectName)
                    {
                        button.Select();
                        _partsDescription.text = body.maxHP + "  Durability \n" +
                                                body.weight + "  Kg \n" +
                                                body.maxWeight + "  Max Weight";
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
                        _clickSound.PlayTheSound();
                        _partsDescription.text = gun.maxHp + "  Durability  -  " + gun.attackRange + "  Range \n" +
                                                gun.damage + "  Dmg  x  " + gun.maxBullets + "  Hits \n" +
                                                gun.hitChance + "%  Hit Chance \n" +
                                                gun.weight + "  Kg";

                        /*170 Durability
                        35 Dmg x 5 Hits
                        70% Hit Chance
                        3 Range
                        100 Kg*/

                        button.Select();

                        _workshopManager.UpdateLeftGun(gun);

                        UpdateWeightSlider(mechaIndex);
                    });

                    if (currentMechaEquipment.leftGun.objectName == gun.objectName)
                    {
                        button.Select();
                        _partsDescription.text = gun.maxHp + "  Durability  -  " + gun.attackRange + "  Range \n" +
                                                gun.damage + "  Dmg  x  " + gun.maxBullets + "  Hits \n" +
                                                gun.hitChance + "%  Hit Chance \n" +
                                                gun.weight + "  Kg";
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
                        _clickSound.PlayTheSound();
                        _partsDescription.text = gun.maxHp + "  Durability  -  " + gun.attackRange + "  Range \n" +
                                                gun.damage + "  Dmg  x  " + gun.maxBullets + "  Hits \n" +
                                                gun.hitChance + "%  Hit Chance \n" +
                                                gun.weight + "  Kg";

                        button.Select();

                        _workshopManager.UpdateRightGun(gun);

                        UpdateWeightSlider(mechaIndex);
                    });

                    if (currentMechaEquipment.rightGun.objectName == gun.objectName)
                    {
                        button.Select();
                        _partsDescription.text = gun.maxHp + "  Durability  -  " + gun.attackRange + "  Range \n" +
                                                gun.damage + "  Dmg  x  " + gun.maxBullets + "  Hits \n" +
                                                gun.hitChance + "%  Hit Chance \n" +
                                                gun.weight + "  Kg";
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
                        _clickSound.PlayTheSound();
                        _partsDescription.text = leg.maxHP + "  Durability \n" +
                                                leg.maxSteps + "  Steps \n" +
                                                leg.weight + "  Kg";

                        button.Select();

                        _workshopManager.UpdateLegs(leg);

                        UpdateWeightSlider(mechaIndex);
                    });

                    if (currentMechaEquipment.legs.objectName == leg.objectName)
                    {
                        button.Select();
                        _partsDescription.text = leg.maxHP + "  Durability \n" +
                                                leg.maxSteps + "  Steps \n" +
                                                leg.weight + "  Kg";
                    }
                }
                break;
        }
    }

    public void UpdateAbilitiesList(string part)
    {
        _workshopManager.DestroyWorkshopObjects();

        _workshopManager.StartPartFlicker(part);

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
                            _clickSound.PlayTheSound();
                            _abilitiesDescription.text = ability.objectDescription;
                            if (ability.objectImage) _bodyAbilityImage.sprite = ability.objectImage;

                            button.Select();

                            OnChangeEquippable?.Invoke(ability, part);
                        });

                        if (currentMechaEquipment.bodyAbility)
                        {
                            if (currentMechaEquipment.bodyAbility.objectName == ability.objectName)
                            {
                                button.Select();
                                _abilitiesDescription.text = ability.objectDescription;
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
                            _clickSound.PlayTheSound();
                            _abilitiesDescription.text = ability.objectDescription;
                            if (ability.objectImage) _leftArmAbilityImage.sprite = ability.objectImage;

                            button.Select();
                            OnChangeEquippable?.Invoke(ability, part);
                        });

                        if (currentMechaEquipment.leftGunAbility)
                        {
                            if (currentMechaEquipment.leftGunAbility.objectName == ability.objectName)
                            {
                                button.Select();
                                _abilitiesDescription.text = ability.objectDescription;
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
                            _clickSound.PlayTheSound();
                            _abilitiesDescription.text = ability.objectDescription;
                            if (ability.objectImage) _rightArmAbilityImage.sprite = ability.objectImage;

                            button.Select();

                            OnChangeEquippable?.Invoke(ability, part);
                        });

                        if (currentMechaEquipment.rightGunAbility)
                        {
                            if (currentMechaEquipment.rightGunAbility.objectName == ability.objectName)
                            {
                                button.Select();
                                _abilitiesDescription.text = ability.objectDescription;
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
                            _clickSound.PlayTheSound();
                            _abilitiesDescription.text = ability.objectDescription;
                            if (ability.objectImage) _legsAbilityImage.sprite = ability.objectImage;

                            button.Select();

                            OnChangeEquippable?.Invoke(ability, part);
                        });

                        if (currentMechaEquipment.legsAbility)
                        {
                            if (currentMechaEquipment.legsAbility.objectName == ability.objectName)
                            {
                                button.Select();
                                _abilitiesDescription.text = ability.objectDescription;
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
                _clickSound.PlayTheSound();
                _itemsDescription.text = "Uses: " + item.maxUses + "\n" + item.objectDescription;
                _itemImage.sprite = item.objectImage;

                button.Select();

                OnChangeEquippable?.Invoke(item, "Item");
            });

            if (currentMechaEquipment.item)
            {
                if (currentMechaEquipment.item.objectName == item.objectName)
                {
                    button.Select();
                    _itemsDescription.text = item.objectDescription;
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

    public void CopyAbilitiesToAll()
    {
        OnCopyAbilityToAll?.Invoke();
    }

    public void CopyItemToAll()
    {
        OnCopyItemToAll?.Invoke();
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
            _bodyAbilityImage.sprite = currentMechaEquipment.bodyAbility.objectImage;
        else
            _bodyAbilityImage.sprite = _noneIcon;

        if (currentMechaEquipment.leftGunAbility)
            _leftArmAbilityImage.sprite = currentMechaEquipment.leftGunAbility.objectImage;
        else 
            _leftArmAbilityImage.sprite = _noneIcon;

        if (currentMechaEquipment.rightGunAbility)
            _rightArmAbilityImage.sprite = currentMechaEquipment.rightGunAbility.objectImage;
        else 
            _rightArmAbilityImage.sprite = _noneIcon;

        if (currentMechaEquipment.legsAbility) 
            _legsAbilityImage.sprite = currentMechaEquipment.legsAbility.objectImage;
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
            Sprite icon = item.objectImage;

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
