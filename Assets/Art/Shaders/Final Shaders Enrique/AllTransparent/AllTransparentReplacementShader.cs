using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AllTransparentReplacementShader : MonoBehaviour
{
    [Range(0, 1)]
    public float transparency = 0.5f;

    public Shader transparentReplacement;
    public Color mainColor;
    public Color tresnel;
    
    private bool _isActive;

    private void Update()
    {
        Shader.SetGlobalColor("GL_MainColor", mainColor);
        Shader.SetGlobalColor("GL_FresnelColor", tresnel);
        Shader.SetGlobalFloat("GL_Transparency", transparency);

        if (!_isActive && Input.GetKeyUp(KeyCode.Space))
        {
            GetComponent<Camera>().SetReplacementShader(transparentReplacement, "");
            _isActive = true;
        }
        else if (_isActive && Input.GetKeyUp(KeyCode.Space))
        {
            GetComponent<Camera>().ResetReplacementShader();
            _isActive = false;
        }
    }

}
