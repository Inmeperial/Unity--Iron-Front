using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileShader : MonoBehaviour
{
    public bool isEmissionOn = false;
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = this.gameObject.GetComponent<Renderer>().material;
        //materialChild = childObj.gameObject.GetComponent<Renderer>().material;
        //materialChild.SetFloat("_PowerFresnel", num);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(material.name);
        if (isEmissionOn)
        {
            material.SetInt("_SwitchEmission", 1);
            //material.Set
            //material.SetInt("AddEffectToEmission", 1);
        }
        else
        {
            material.SetInt("_SwitchEmission", 0);
            //material.SetInt("AddEffectToEmission", 0);
        }
    }
}


