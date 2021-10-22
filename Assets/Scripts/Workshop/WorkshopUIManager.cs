using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorkshopUIManager : MonoBehaviour
{
   
   
   
   [Header("Overview Texts")]
   [SerializeField] private MechaEquipmentContainerSO _equipmentContainer;
   [SerializeField] private TextMeshProUGUI _overviewBody;
   [SerializeField] private TextMeshProUGUI _overviewLeftArm;
   [SerializeField] private TextMeshProUGUI _overviewRightArm;
   [SerializeField] private TextMeshProUGUI _overviewLegs;

   [Space]
   [SerializeField] private WorkshopObjectButton _workshopObjectPrefab;
   
   [Header("Parts List")]
   [SerializeField] private Transform _partsSpawnParent;
   [SerializeField] private TextMeshProUGUI _partsDescription;
   
   [Header("Abilities List")]
   [SerializeField] private Transform _abilitiesSpawnParent;
   [SerializeField] private TextMeshProUGUI _abilitiesDescription;
   
   [Header("Items List")]
   [SerializeField] private Transform _itemsSpawnParent;
   [SerializeField] private TextMeshProUGUI _itemsDescription;
   [SerializeField] private Image _itemImage;

   private List<WorkshopObjectButton> _createdObjectButtonList = new List<WorkshopObjectButton>();
   private void Start()
   {
      WorkshopManager.OnClickPrevious += UpdateOverviewText;
      WorkshopManager.OnClickNext += UpdateOverviewText;
      WorkshopManager.OnClickCloseEdit += UpdateOverviewText;
      UpdateOverviewText(3);
   }

   private void UpdateOverviewText(int mechaIndex)
   {
      var equipmentData = _equipmentContainer.GetEquipment(mechaIndex);
      _overviewBody.text = "Body: \n" + equipmentData.body.partName;
      _overviewLeftArm.text = "Left Arm: \n" + equipmentData.leftArm.partName;
      _overviewRightArm.text = "Right Arm: \n" + equipmentData.rightArm.partName;
      _overviewLegs.text = "Legs: \n" + equipmentData.legs.partName;
   }
   
   public void UpdatePartsList(string part)
   {
      DestroyWorkshopObjects();
      
      switch (part)
      {
         case "Body":
            var bodies = WorkshopDatabaseManager.Instance.GetBodies();

            foreach (var body in bodies)
            {
               CreateWorkshopObject(body, _partsSpawnParent, _partsDescription);
            }
            break;
         
         case "LeftArm":
            var lArms = WorkshopDatabaseManager.Instance.GetArms();
            foreach (var arm in lArms)
            {
               CreateWorkshopObject(arm, _partsSpawnParent, _partsDescription);
            }
            break;
            
         case "RightArm":
            var rArms = WorkshopDatabaseManager.Instance.GetArms();
            foreach (var arm in rArms)
            {
               CreateWorkshopObject(arm, _partsSpawnParent, _partsDescription);
            }
            break;
         
         case "Legs":
            var legs = WorkshopDatabaseManager.Instance.GetLegs();
            foreach (var leg in legs)
            {
               CreateWorkshopObject(leg, _partsSpawnParent, _partsDescription);
            }
            break;
      }
   }

   //TODO: Funcionalidad de mostrar
   public void UpdateAbilitiesList(string part)
   {
      DestroyWorkshopObjects();
      
      List<AbilitySO> abilities = new List<AbilitySO>();
      abilities = WorkshopDatabaseManager.Instance.GetAbilities();
      
      switch (part)
      {
            
         case "Body":

            foreach (var ability in abilities)
            {
               if (ability.partSlot == AbilitySO.PartSlot.Body)
                  CreateWorkshopObject(ability, _abilitiesSpawnParent, _abilitiesDescription);
            }
            
            break;
         
         case "LeftArm":

            foreach (var ability in abilities)
            {
               if (ability.partSlot == AbilitySO.PartSlot.Arm)
                  CreateWorkshopObject(ability, _abilitiesSpawnParent, _abilitiesDescription);
            }
            break;
            
         case "RightArm":

            foreach (var ability in abilities)
            {
               if (ability.partSlot == AbilitySO.PartSlot.Arm)
                  CreateWorkshopObject(ability, _abilitiesSpawnParent, _abilitiesDescription);
            }
            
            break;
         
         case "Legs":

            foreach (var ability in abilities)
            {
               if (ability.partSlot == AbilitySO.PartSlot.Legs)
                  CreateWorkshopObject(ability, _abilitiesSpawnParent, _abilitiesDescription);
            }
            break;
      }
   }

   
   
   public void UpdateItemsList()
   {
      DestroyWorkshopObjects();
      
      var items = WorkshopDatabaseManager.Instance.GetItems();

      foreach (var item in items)
      {
         CreateWorkshopObject(item, _itemsSpawnParent, _itemsDescription);
      }
   }

   
   private void CreateWorkshopObject(PartSO part, Transform parent, TextMeshProUGUI descriptionField)
   {
      var obj = Instantiate(_workshopObjectPrefab, parent);
      obj.transform.localPosition = Vector3.zero;
      obj.SetObjectName(part.partName);
      obj.SetObjectSprite(part.icon);
      obj.SetLeftClick(() => descriptionField.text = "HP: " + part.maxHP);
      _createdObjectButtonList.Add(obj);
   }
   
   private void CreateWorkshopObject(EquipableSO equipable, Transform parent, TextMeshProUGUI descriptionField)
   {
      var obj = Instantiate(_workshopObjectPrefab, parent);
      obj.transform.localPosition = Vector3.zero;
      obj.SetObjectName(equipable.equipableName);
      obj.SetObjectSprite(equipable.equipableIcon);
      //obj.SetDescriptionTextField(descriptionField);
      obj.SetLeftClick(() => descriptionField.text = equipable.description);
      _createdObjectButtonList.Add(obj);
   }

   private void DestroyWorkshopObjects()
   {
      foreach (var obj in _createdObjectButtonList)
      {
         Destroy(obj.gameObject);
      }

      _createdObjectButtonList = new List<WorkshopObjectButton>();
   }
}
