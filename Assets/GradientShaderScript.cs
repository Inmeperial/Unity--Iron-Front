using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientShaderScript : MonoBehaviour
{
    private Material mat = default;
    
    void Start()
    {
        mat = this.GetComponent<Renderer>().material;
    }

    public void SetColorGradientRed(float num)
    {
        mat.SetFloat("_Red", 0);  
    }

    public void SetColorGradientGreen(float num)
    {
        mat.SetFloat("_Green", 0);
    }

    public void SetColorGradientBlue(float num)
    {
        mat.SetFloat("_Blue", 0);
    }

}
