using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileShader : MonoBehaviour
{
    private bool isFresnelOn = false;
    private Material material;
    private GameObject childObj;

    // Start is called before the first frame update
    void Start()
    {
        childObj = this.transform.GetChild(0).gameObject;
        material = childObj.gameObject.GetComponent<Renderer>().sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFresnelOn)
        {
            //material.SetInt("_isMoveAvailable_ON", 1);
        }
        else
        {
            //material.SetInt("_isMoveAvailable_ON", 0);
        }
    }
}
