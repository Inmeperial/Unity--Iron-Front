﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorkshopUIManager : MonoBehaviour
{
   [SerializeField] private Button _saveButton;
   [Header("Overview Texts")]
   [SerializeField] private TextMeshProUGUI _overviewBody;
   [SerializeField] private TextMeshProUGUI _overviewLeftArm;
   [SerializeField] private TextMeshProUGUI _overviewRightArm;
   [SerializeField] private TextMeshProUGUI _overviewLegs;
   
   
   [Header("Parts List")]
   [SerializeField] private Transform _partsSpawnParent;
   [SerializeField] private TextMeshProUGUI _partsDescription;
   [SerializeField] private Slider _weightSlider;
   [SerializeField] private TextMeshProUGUI _weightText;
   [SerializeField] private RectTransform _needle;
   [SerializeField] private Vector3 _maxRotationMaxWeight;
   [SerializeField] private Vector3 _maxRotationOverweight;
   [SerializeField] private float _needleRotationTime;
   [SerializeField] private Color _normalWeightColor;
   [SerializeField] private Color _overWeightColor;
   
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

   public static event ChangeEquippable OnChangeEquippable;
   
   public delegate void ColorChange(Color color);

   public static event ColorChange OnBodyColorChange;
   public static event ColorChange OnLegsColorChange;

   public delegate void NameChange(string name);

   public static event NameChange OnNameChange;
   private void Start()
   {
      WorkshopManager.OnClickPrevious += UpdateOverviewText;
      WorkshopManager.OnClickNext += UpdateOverviewText;
      WorkshopManager.OnClickCloseEdit += UpdateOverviewText;
      WorkshopManager.OnClickEdit += UpdateWeightSlider;
      WorkshopManager.OnChangesMade += SaveButtonEnable;
      WorkshopManager.OnSave += SaveButtonDisable;
      UpdateOverviewText(3);
   }

   private void UpdateOverviewText(int mechaIndex)
   {
      var equipmentData = FindObjectOfType<WorkshopManager>().GetMechaEquipment(mechaIndex);
      _overviewBody.text = "Body: \n" + equipmentData.body.partName;
      _overviewLeftArm.text = "Left Gun: \n" + equipmentData.leftGun.gunName;
      _overviewRightArm.text = "Right Gun: \n" + equipmentData.rightGun.gunName;
      _overviewLegs.text = "Legs: \n" + equipmentData.legs.partName;
   }

   public void UpdateWeightSlider(int mechaIndex)
   {
      var equipmentData = FindObjectOfType<WorkshopManager>().GetMechaEquipment(mechaIndex);
      
      float weight = equipmentData.body.weight + equipmentData.leftGun.weight + equipmentData.rightGun.weight +
                     equipmentData.legs.weight;

      float maxWeight = equipmentData.body.maxWeight;
      
      _weightSlider.minValue = 0;
      _weightSlider.maxValue = maxWeight;

      

      _weightSlider.value = weight;
      
      _weightText.text = weight +"";

      StopCoroutine(RotateNeedle(Quaternion.identity));

      Vector3 newRotation = (weight * _maxRotationMaxWeight) / maxWeight;

      if (newRotation.z < _maxRotationOverweight.z)
         newRotation.z = _maxRotationOverweight.z;
      
      Quaternion target = Quaternion.Euler(newRotation);

      StartCoroutine(RotateNeedle(target));
      // if (weight > maxWeight)
      // {
      //    var weightSliderColors = _weightSlider.colors;
      //    weightSliderColors.normalColor = _overWeightColor;
      //    //_weightSlider.colors = weightSliderColors;
      // }
      // else
      // {
      //    var weightSliderColors = _weightSlider.colors;
      //    weightSliderColors.normalColor = _normalWeightColor;
      // }
   }

   IEnumerator RotateNeedle(Quaternion target)
   {
      float time = 0;
      Quaternion needleRot = _needle.rotation;

      while (time < _needleRotationTime)
      {
         Debug.Log("rotation");
         _needle.rotation = Quaternion.Lerp(needleRot, target, time / _needleRotationTime);
         time += Time.deltaTime;
         yield return new WaitForEndOfFrame();
      }

      _needle.rotation = target;
   }
   
   public void UpdatePartsList(string part)
   {
      var workshopManager = FindObjectOfType<WorkshopManager>();
      
      workshopManager.DestroyWorkshopObjects();

      var index = workshopManager.GetIndex();

      _partsDescription.text = "";
      var manager = FindObjectOfType<WorkshopManager>();
      switch (part)
      {
         case "Body":
            var bodies = WorkshopDatabaseManager.Instance.GetBodies();

            foreach (var body in bodies)
            {
               var b = workshopManager.CreateWorkshopObject(body, _partsSpawnParent);
               b.SetLeftClick(() =>
               {
                  _partsDescription.text = "HP: " + body.maxHP;
                  
                  manager.UpdateBody(body);
                  
                  UpdateWeightSlider(index);
               });
            }
            break;
         
         case "LeftArm":
            var gunsA = WorkshopDatabaseManager.Instance.GetGuns();
            foreach (var gun in gunsA)
            {
               var a = workshopManager.CreateWorkshopObject(gun, _partsSpawnParent);
               a.SetLeftClick(() =>
               {
                  _partsDescription.text = "Damage: " + gun.damage +
                                           "\n HitChance:" + gun.hitChance +
                                           "\n Crit Multiplier: " + gun.critMultiplier +
                                           "\n Crit Chance: " + gun.critChance +
                                           "\n Attack Range: " + gun.attackRange;
                                           
                  manager.UpdateLeftGun(gun);
                  
                  UpdateWeightSlider(index);
               });
            }
            break;
            
         case "RightArm":
            var gunsB = WorkshopDatabaseManager.Instance.GetGuns();
            foreach (var gun in gunsB)
            {
               var a = workshopManager.CreateWorkshopObject(gun, _partsSpawnParent);
               a.SetLeftClick(() =>
               {
                  _partsDescription.text = "Damage: " + gun.damage +
                                           "\n HitChance:" + gun.hitChance +
                                           "\n Crit Multiplier: " + gun.critMultiplier +
                                           "\n Crit Chance: " + gun.critChance +
                                           "\n Attack Range: " + gun.attackRange;
                  manager.UpdateRightGun(gun);
                  
                  UpdateWeightSlider(index);
               });
            }
            break;
         
         case "Legs":
            var legs = WorkshopDatabaseManager.Instance.GetLegs();
            foreach (var leg in legs)
            {
               var l = workshopManager.CreateWorkshopObject(leg, _partsSpawnParent);
               l.SetLeftClick(() =>
               {
                  _partsDescription.text = "HP: " + leg.maxHP;
                  manager.UpdateLegs(leg);
                  
                  UpdateWeightSlider(index);
               });
            }
            break;
      }
   }
   
   public void UpdateAbilitiesList(string part)
   {
      var workshopManager = FindObjectOfType<WorkshopManager>();
      
      workshopManager.DestroyWorkshopObjects();
      
      _abilitiesDescription.text = "";
      List<AbilitySO> abilities = new List<AbilitySO>();
      abilities = WorkshopDatabaseManager.Instance.GetAbilities();
      
      switch (part)
      {
            
         case "Body":
            foreach (var ability in abilities)
            {
               if (ability.partSlot == AbilitySO.PartSlot.Body)
               {
                  var obj = workshopManager.CreateWorkshopObject(ability, _abilitiesSpawnParent);
                  obj.SetLeftClick(() =>
                  {
                     _itemsDescription.text = ability.description;
                     if (ability.equipableIcon)
                        _bodyAbilityImage.sprite = ability.equipableIcon;
                     OnChangeEquippable?.Invoke(ability, part);
                  });
               }
            }
            
            break;
         
         case "LeftArm":

            foreach (var ability in abilities)
            {
               if (ability.partSlot == AbilitySO.PartSlot.Arm)
               {
                  var obj = workshopManager.CreateWorkshopObject(ability, _abilitiesSpawnParent);
                  obj.SetLeftClick(() =>
                  {
                     _itemsDescription.text = ability.description;
                     if (ability.equipableIcon)
                        _leftArmAbilityImage.sprite = ability.equipableIcon;
                     OnChangeEquippable?.Invoke(ability, part);
                  });
               }
            }
            break;
            
         case "RightArm":

            foreach (var ability in abilities)
            {
               if (ability.partSlot == AbilitySO.PartSlot.Arm)
               {
                  var obj = workshopManager.CreateWorkshopObject(ability, _abilitiesSpawnParent);
                  obj.SetLeftClick(() =>
                  {
                     _itemsDescription.text = ability.description;
                     if (ability.equipableIcon)
                        _rightArmAbilityImage.sprite = ability.equipableIcon;
                     OnChangeEquippable?.Invoke(ability, part);
                  });
               }
            }
            
            break;
         
         case "Legs":

            foreach (var ability in abilities)
            {
               if (ability.partSlot == AbilitySO.PartSlot.Legs)
               {
                  var obj = workshopManager.CreateWorkshopObject(ability, _abilitiesSpawnParent);
                  obj.SetLeftClick(() =>
                  {
                     _itemsDescription.text = ability.description;
                     if (ability.equipableIcon)
                        _legsAbilityImage.sprite = ability.equipableIcon;
                     OnChangeEquippable?.Invoke(ability, part);
                  });
               }
                  
            }
            break;
      }
   }

   public void UpdateItemsList()
   {
      var workshopManager = FindObjectOfType<WorkshopManager>();
      
      workshopManager.DestroyWorkshopObjects();
      
      _itemsDescription.text = "";
      var items = WorkshopDatabaseManager.Instance.GetItems();

      foreach (var item in items)
      {
         var obj = workshopManager.CreateWorkshopObject(item, _itemsSpawnParent);
         obj.SetLeftClick(() =>
         {
            _itemsDescription.text = item.description;
            //TODO: retirar checkeo cuando haya iconos.
            if (item.equipableIcon)
               _itemImage.sprite = item.equipableIcon;
            OnChangeEquippable?.Invoke(item, "Item");
         });
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
      OnNameChange?.Invoke(mechaName);
   }
   
   public void SetBodyColorSliders()
   {
      var workshop = FindObjectOfType<WorkshopManager>();
      var index = workshop.GetIndex();
      var color = workshop.GetMechaEquipment(index).bodyColor;
      _bodyRedSlider.value = color.red;
      _bodyGreenSlider.value = color.green;
      _bodyBlueSlider.value = color.blue;
      _bodyColorImage.color = new Color(color.red, color.green, color.blue);
   }
   
   public void SetLegsColorSliders()
   {
      var workshop = FindObjectOfType<WorkshopManager>();
      var index = workshop.GetIndex();
      var color = workshop.GetMechaEquipment(index).legsColor;
      _legsRedSlider.value = color.red;
      _legsGreenSlider.value = color.green;
      _legsBlueSlider.value = color.blue;
      _legsColorImage.color = new Color(color.red, color.green, color.blue, 1);
   }

   public void SetMechaName()
   {
      var workshop = FindObjectOfType<WorkshopManager>();
      var index = workshop.GetIndex();
      _nameField.text = workshop.GetMechaEquipment(index).name;
      
   }

   public void SaveButtonEnable()
   {
      _saveButton.interactable = true;
   }
   
   public void SaveButtonDisable()
   {
      _saveButton.interactable = false;
   }
}
