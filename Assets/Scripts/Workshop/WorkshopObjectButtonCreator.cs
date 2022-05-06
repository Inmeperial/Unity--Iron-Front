using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopObjectButtonCreator : MonoBehaviour
{
    [SerializeField] private WorkshopObjectButton _workshopObjectPrefab;

    public WorkshopObjectButton CreateWorkshopObjectButton(PartSO part, Transform parent)
    {
        WorkshopObjectButton button = InstantiateWorkshopObjectButton(parent, part.partName, part.icon);
        return button;
    }

    public WorkshopObjectButton CreateWorkshopObjectButton(GunSO gun, Transform parent)
    {
        WorkshopObjectButton button = InstantiateWorkshopObjectButton(parent, gun.gunName, gun.gunImage);
        return button;
    }

    public WorkshopObjectButton CreateWorkshopObjectButton(EquipableSO equipable, Transform parent)
    {
        WorkshopObjectButton button = InstantiateWorkshopObjectButton(parent, equipable.equipableName, equipable.equipableIcon);
        return button;
    }

    private WorkshopObjectButton InstantiateWorkshopObjectButton(Transform parent, string name, Sprite icon)
    {
        WorkshopObjectButton button = Instantiate(_workshopObjectPrefab, parent);
        button.transform.localPosition = Vector3.zero;
        button.SetObjectName(name);
        button.SetObjectSprite(icon);
        return button;
    }
}
