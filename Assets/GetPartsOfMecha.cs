using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPartsOfMecha : MonoBehaviour
{
    public GameObject legLSpawn, legRSpawn, chestSpawn, armLSpawn, armRSpawn;
    private Dictionary<PartsMechaEnum, GameObject> _partsDictionary = new Dictionary<PartsMechaEnum, GameObject>();

    // falta RIGHT ARM SPAWN y el left para que aparezca en el shader, cambiar el nombre de ahora de armLspawn a arma y usar estos nombres para los faltantes.

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < legLSpawn.transform.childCount; i++)
        //{
        //    Debug.Log(legLSpawn.transform.GetChild(i) + this.transform.gameObject.name);
        //}

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
            _partsDictionary.Add(PartsMechaEnum.armL, armLSpawn.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject);
        }

        if (armRSpawn != null)
        {
            _partsDictionary.Add(PartsMechaEnum.armR, armRSpawn.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject);
        }
    }

    public Dictionary<PartsMechaEnum, GameObject> GetPartsObj()
    {
        return _partsDictionary;
    }

}
