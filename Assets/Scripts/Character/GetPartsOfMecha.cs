using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPartsOfMecha : MonoBehaviour
{
    public GameObject legLSpawn, legRSpawn, chestSpawn, armLSpawn, armRSpawn, weaponLSpawn, weaponRSpawn;
    private Dictionary<PartsMechaEnum, GameObject> _partsDictionary = new Dictionary<PartsMechaEnum, GameObject>();
    
    // cambiar el scale del normal, 2.28 para el mecha A y 1.5 para el mecha B.

    // Start is called before the first frame update
    public void ManualStart()
    {
        if (legLSpawn != null)
        {
            _partsDictionary.Add(PartsMechaEnum.legL, legLSpawn.transform.GetChild(0).gameObject);
        }

        if (legRSpawn != null)
        {
            _partsDictionary.Add(PartsMechaEnum.legR, legRSpawn.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject);
        }

        if (chestSpawn != null)
        {
            _partsDictionary.Add(PartsMechaEnum.body, chestSpawn.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject);
        }

        if (armLSpawn != null)
        {
            _partsDictionary.Add(PartsMechaEnum.armL, armLSpawn.transform.GetChild(0).gameObject);
        }

        if (armRSpawn != null)
        {
            _partsDictionary.Add(PartsMechaEnum.armR, armRSpawn.transform.GetChild(0).gameObject);
        }

        if (weaponLSpawn != null)
        {
            _partsDictionary.Add(PartsMechaEnum.weaponL, weaponLSpawn.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject);
        }

        if (weaponRSpawn != null)
        {
            _partsDictionary.Add(PartsMechaEnum.weaponR, weaponRSpawn.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject);
        }
    }

    public Dictionary<PartsMechaEnum, GameObject> GetPartsObj()
    {
        return _partsDictionary;
    }

}
