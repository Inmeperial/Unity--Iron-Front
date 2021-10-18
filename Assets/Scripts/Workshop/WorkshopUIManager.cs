using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
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

   //TODO: Funcionalidad de mostrar
   public void UpdatePartsList(string part)
   {
      switch (part)
      {
         case "Body":
            Debug.Log("update body list");
            WorkshopDatabaseManager.Instance.GetBodies();
            break;
         
         case "LeftArm":
            Debug.Log("update left list");
            WorkshopDatabaseManager.Instance.GetArms();
            break;
            
         case "RightArm":
            Debug.Log("update right list");
            WorkshopDatabaseManager.Instance.GetArms();
            break;
         
         case "Legs":
            Debug.Log("update legs list");
            WorkshopDatabaseManager.Instance.GetLegs();
            break;
      }
   }

   //TODO: Funcionalidad de mostrar
   public void UpdateAbilitiesList(string part)
   {
      switch (part)
      {
         case "Body":
            Debug.Log("update body abilities list");
            
            break;
         
         case "LeftArm":
            Debug.Log("update left abilities list");
            
            break;
            
         case "RightArm":
            Debug.Log("update right abilities list");
            
            break;
         
         case "Legs":
            Debug.Log("update legs abilities list");
            
            break;
      }
   }
   
   //TODO: Funcionalidad de mostrar
   public void UpdateItemsList()
   {
      Debug.Log("update items list");
   }
}
