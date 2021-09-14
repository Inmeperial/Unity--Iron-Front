using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSpawner : EnumsClass
{
    [Header("Guns")]
    [SerializeField] private AssaultRifle _assaultRifle;
    [SerializeField] private Melee _melee;
    [SerializeField] private Rifle _rifle;
    [SerializeField] private Shield _shield;
    [SerializeField] private Shotgun _shotgun;
    /// <summary>
    /// Spawn a gun of the given type.
    /// </summary>
    /// <param name="type">Type of gun.</param>
    /// <param name="position">Position to spawn.</param>
    /// <param name="parent">Parent object, null if empty.</param>
    /// <returns></returns>
    public Gun SpawnGun(GunsType type, Vector3 position, Transform parent = null)
    {
        Gun gun = null;
        switch (type)
        {
            case GunsType.None:
                return null;
            
            case GunsType.AssaultRifle:
                gun = Instantiate(_assaultRifle, parent);
                break;

            case GunsType.Melee:
                gun =  Instantiate(_melee, parent);
                break;
            
            case GunsType.Rifle:
                gun = Instantiate(_rifle, parent);
                break;
            
            case GunsType.Shield:
                gun =  Instantiate(_shield, parent);
                break;
            
            case GunsType.Shotgun:
                gun =  Instantiate(_shotgun, parent);
                break;
        }
        if (gun)
            gun.transform.localPosition = position;
        return gun;
    }

    // public Item SpawnItem(ItemType type, Character owner)
    // {
    //     Item item = null;
    //     switch (type)
    //     {
    //         case ItemType.Grenade:
    //             item = new Grenade(_grenadeSo, owner, FindObjectOfType<TileHighlight>());
    //             break;
    //     }
    //
    //     return item;
    // }
}
