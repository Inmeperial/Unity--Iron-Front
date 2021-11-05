using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradientShaderScript : MonoBehaviour
{
    private Material _mat = default;
    
    void Awake()
    {
        GetMat();
    }

    public void SetColorGradientRed(float num)
    {
        GetMat();
        _mat.SetFloat("_Red", num);  
    }

    public void SetColorGradientGreen(float num)
    {
        GetMat();
        _mat.SetFloat("_Green", num);
    }

    public void SetColorGradientBlue(float num)
    {
        GetMat();
        _mat.SetFloat("_Blue", num);
    }

    void GetMat()
    {
        if (!_mat)
        {
            var mat = this.GetComponent<Image>().material;

            _mat = new Material(mat); 
        }
    }

}
