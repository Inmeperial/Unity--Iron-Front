using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
   [SerializeField] private MechaEquipmentContainerSO _equipmentContainer;
   [SerializeField] private TextMeshProUGUI _overviewBody;
   [SerializeField] private TextMeshProUGUI _overviewLeftArm;
   [SerializeField] private TextMeshProUGUI _overviewRightArm;
   [SerializeField] private TextMeshProUGUI _overviewLegs;

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
}
