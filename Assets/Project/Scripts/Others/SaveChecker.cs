using System.Collections;
using UnityEngine;

public class SaveChecker : Initializable
{
    public override void Initialize()
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
