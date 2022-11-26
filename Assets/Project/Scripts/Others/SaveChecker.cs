using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveChecker : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(LoadAndSave());
    }

    private IEnumerator LoadAndSave()
    {
        MechaEquipmentContainerSO save = LoadSaveUtility.LoadEquipment();

        yield return new WaitForSeconds(1);

        LoadSaveUtility.SaveEquipment(save);
    }
}
