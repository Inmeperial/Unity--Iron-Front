using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ReplacementTesting : MonoBehaviour
{
    public Shader shaderReplace;
    public Texture numbers, mask;
    public Color enemyColor = Color.red;

    private bool _isActive;

    private void Update()
    {
        
        Shader.SetGlobalTexture("GL_numbersText", numbers);
        Shader.SetGlobalTexture("GL_textMask", mask);
        Shader.SetGlobalColor("GL_colorWord", enemyColor);

        if (!_isActive && Input.GetKeyUp(KeyCode.R))
        {
            //GetComponent<Camera>().SetReplacementShader(shaderReplace, "");
            //sceneView.SetSceneViewShaderReplace(shaderReplace, "");

            GetComponent<Camera>().SetReplacementShader(shaderReplace, "");
            _isActive = true;
        }
        else if (_isActive && Input.GetKeyUp(KeyCode.R))
        {
            GetComponent<Camera>().ResetReplacementShader();
            _isActive = false;
        }
    }

}
