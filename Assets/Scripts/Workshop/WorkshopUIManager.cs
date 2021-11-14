using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorkshopUIManager : MonoBehaviour
{
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
      UpdateOverviewText(3);

      _weightTextColor = _weightText.color;//Me guardo el color inicial del texto
   }

   private void UpdateOverviewText(int mechaIndex)
   {
      var equipmentData = FindObjectOfType<WorkshopManager>().GetMechaEquipment(mechaIndex);

      _overviewName.text = equipmentData.name;
      
      _overviewBody.text = "Body: \n" + equipmentData.body.partName;
      if (equipmentData.body.ability)
         _overviewBodyAbility.sprite = equipmentData.body.ability.equipableIcon;
      else _overviewBodyAbility.sprite = _noneIcon;
      
      _overviewLeftArm.text = "Left Gun: \n" + equipmentData.leftGun.gunName;
      if (equipmentData.leftGun.ability)
         _overviewLeftArmAbility.sprite = equipmentData.leftGun.ability.equipableIcon;
      else _overviewLeftArmAbility.sprite = _noneIcon;
      
      _overviewRightArm.text = "Right Gun: \n" + equipmentData.rightGun.gunName;
      if (equipmentData.rightGun.ability)
         _overviewRightArmAbility.sprite = equipmentData.rightGun.ability.equipableIcon;
      else _overviewRightArmAbility.sprite = _noneIcon;
      
      _overviewLegs.text = "Legs: \n" + equipmentData.legs.partName;
      if (equipmentData.legs.ability)
         _overviewLegsAbility.sprite = equipmentData.legs.ability.equipableIcon;
      else _overviewLegsAbility.sprite = _noneIcon;

      if (equipmentData.body.item)
         _overviewItem.sprite = equipmentData.body.item.equipableIcon;
      else _overviewItem.sprite = _noneIcon;
   }

   public void UpdateWeightSlider(int mechaIndex)
   {
      var equipmentData = FindObjectOfType<WorkshopManager>().GetMechaEquipment(mechaIndex);
      
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
      var workshopManager = FindObjectOfType<WorkshopManager>();
      
      workshopManager.DestroyWorkshopObjects();

      var index = workshopManager.GetIndex();

      _partsDescription.text = "";
      var manager = FindObjectOfType<WorkshopManager>();
      var currentMechaEquipment = manager.GetMechaEquipment(index);
      switch (part)
      {
         case "Body":
            var bodies = WorkshopDatabaseManager.Instance.GetBodies();
            foreach (var body in bodies)
            {
               
               
               var b = workshopManager.CreateWorkshopObject(body, _partsSpawnParent);
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
               }
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
                                           "\n Attack Range: " + gun.attackRange +
                                           "\n Weight: " + gun.weight;
                                           
                  a.Select();
                  
                  manager.UpdateLeftGun(gun);
                  
                  UpdateWeightSlider(index);
               });
               
               if (currentMechaEquipment.leftGun.gunName == gun.gunName)
               {
                  a.Select();
               }
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
                                           "\n Attack Range: " + gun.attackRange +
                                           "\n Weight: " + gun.weight;
                  
                  a.Select();
                  
                  manager.UpdateRightGun(gun);
                  
                  UpdateWeightSlider(index);
               });

               if (currentMechaEquipment.rightGun.gunName == gun.gunName)
               {
                  a.Select();
               }
            }
            break;
         
         case "Legs":
            var legs = WorkshopDatabaseManager.Instance.GetLegs();
            foreach (var leg in legs)
            {
               var l = workshopManager.CreateWorkshopObject(leg, _partsSpawnParent);
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
               }
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

      var index = workshopManager.GetIndex();
      var currentMechaEquipment = workshopManager.GetMechaEquipment(index);
      
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
                     _abilitiesDescription.text = ability.description;
                     if (ability.equipableIcon)
                        _bodyAbilityImage.sprite = ability.equipableIcon;
                     
                     obj.Select();

                     OnChangeEquippable?.Invoke(ability, part);
                  });
                  
                  if (currentMechaEquipment.body.ability.equipableName == ability.equipableName)
                  {
                     obj.Select();
                  }
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
                     _abilitiesDescription.text = ability.description;
                     if (ability.equipableIcon)
                        _leftArmAbilityImage.sprite = ability.equipableIcon;
                     
                     obj.Select();
                     OnChangeEquippable?.Invoke(ability, part);
                  });
                  
                  if (currentMechaEquipment.leftGun.ability.equipableName == ability.equipableName)
                  {
                     obj.Select();
                  }
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
                     _abilitiesDescription.text = ability.description;
                     if (ability.equipableIcon)
                        _rightArmAbilityImage.sprite = ability.equipableIcon;
                     
                     obj.Select();
                     
                     OnChangeEquippable?.Invoke(ability, part);
                  });
                  
                  if (currentMechaEquipment.rightGun.ability.equipableName == ability.equipableName)
                  {
                     obj.Select();
                  }
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
                     _abilitiesDescription.text = ability.description;
                     if (ability.equipableIcon)
                        _legsAbilityImage.sprite = ability.equipableIcon;
                     
                     obj.Select();
                     
                     OnChangeEquippable?.Invoke(ability, part);
                  });
                  
                  if (currentMechaEquipment.legs.ability.equipableName == ability.equipableName)
                  {
                     obj.Select();
                  }
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

      var index = workshopManager.GetIndex();
      var currentMechaEquipment = workshopManager.GetMechaEquipment(index);
      
      foreach (var item in items)
      {
         var obj = workshopManager.CreateWorkshopObject(item, _itemsSpawnParent);
         obj.SetLeftClick(() =>
         {
            _itemsDescription.text = item.description;
            //TODO: retirar checkeo cuando haya iconos.
            if (item.equipableIcon)
               _itemImage.sprite = item.equipableIcon;
            
            obj.Select();
            
            OnChangeEquippable?.Invoke(item, "Item");
         });
         
         if (currentMechaEquipment.body.item.equipableName == item.equipableName)
         {
            obj.Select();
         }
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
      var color = workshop.GetMechaEquipment(index).GetBodyColor();
      _bodyColorImage.color = color;
      _bodyRedSlider.value = color.r;
      _bodyGreenSlider.value = color.g;
      _bodyBlueSlider.value = color.b;
   }
   
   public void SetLegsColorSliders()
   {
      var workshop = FindObjectOfType<WorkshopManager>();
      var index = workshop.GetIndex();
      var color = workshop.GetMechaEquipment(index).GetLegsColor();
      _legsColorImage.color = color;
      _legsRedSlider.value = color.r;
      _legsGreenSlider.value = color.g;
      _legsBlueSlider.value = color.b;
   }

   public void SetMechaName()
   {
      var workshop = FindObjectOfType<WorkshopManager>();
      var index = workshop.GetIndex();
      _nameField.text = workshop.GetMechaEquipment(index).name;
      
   }
}
