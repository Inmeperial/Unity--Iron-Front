using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorkshopUIManager : MonoBehaviour
{
   [Header("Overview Texts")]
   [SerializeField] private TextMeshProUGUI _overviewBody;
   [SerializeField] private TextMeshProUGUI _overviewLeftArm;
   [SerializeField] private TextMeshProUGUI _overviewRightArm;
   [SerializeField] private TextMeshProUGUI _overviewLegs;

   
   
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

   public delegate void ChangeEquippable(EquipableSO equipableSo, string location);

   public static event ChangeEquippable OnChangeEquippable;
   
   private void Start()
   {
      WorkshopManager.OnClickPrevious += UpdateOverviewText;
      WorkshopManager.OnClickNext += UpdateOverviewText;
      WorkshopManager.OnClickCloseEdit += UpdateOverviewText;
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
   
   public void UpdatePartsList(string part)
   {
      var workshopManager = FindObjectOfType<WorkshopManager>();
      
      workshopManager.DestroyWorkshopObjects();
      
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
               });
            }
            break;
      }
   }

   //TODO: Funcionalidad de mostrar
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
            OnChangeEquippable?.Invoke(item, "Item");
         });
      }
   }
}
