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

    public void SetColor(Color color)
    {
        GetMat();
        _mat.SetFloat("_Red", color.r);
        _mat.SetFloat("_Green", color.g); 
        _mat.SetFloat("_Blue", color.b);
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
            var image = this.GetComponent<Image>();
            _mat = new Material(image.material);
            image.material = _mat;
        }
    }

}
