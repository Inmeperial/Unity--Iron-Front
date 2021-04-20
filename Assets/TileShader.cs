using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileShader : MonoBehaviour
{
    public bool isEmissionOn = false;
    public Material mat;
    private Material material;
    private Material materialShared;
    private GameObject childObj;
    
    // Start is called before the first frame update
    void Start()
    {
        //childObj = this.transform.GetChild(0).gameObject;
        material = this.gameObject.GetComponent<Renderer>().material;
        materialShared = this.gameObject.GetComponent<Renderer>().sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {   
        if (isEmissionOn)
        {
            material.SetInt("_IsEmissionOn", 1);
            materialShared.SetInt("_IsEmissionOn", 1);
            mat.SetInt("_IsEmissionOn", 1);
            //Debug.Log(material.name);
        }
        else
        {
            material.SetInt("_IsEmissionOn", 0);
            materialShared.SetInt("_IsEmissionOn", 0);
            mat.SetInt("_IsEmissionOn", 0);
        }
    }
}
