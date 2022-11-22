using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MatrixAllReplacementShader : MonoBehaviour
{
    public Shader replacementShader;

    public Texture numbers, mask;

    [Range(0, 1)]
    public float speed, speed2, speed3, speed4;

    public int t_X_1, t_Y_1, t_X_2, t_Y_2, t_X_3, t_Y_3, t_X_4, t_Y_4;

    bool _isActive;

    private void Update()
    {
        SetParametersForReplacementShader();

        if (!_isActive && Input.GetKeyUp(KeyCode.Space))
        {
            GetComponent<Camera>().SetReplacementShader(replacementShader, "");
           
            _isActive = true;
        }
        else if (_isActive && Input.GetKeyUp(KeyCode.Space))
        {
            GetComponent<Camera>().ResetReplacementShader();
            _isActive = false;
        }
    }

    private void SetParametersForReplacementShader()
    {
        // Textures
        Shader.SetGlobalTexture("numbers1", numbers);
        Shader.SetGlobalTexture("numbers2", numbers);
        Shader.SetGlobalTexture("mask1", mask);
        Shader.SetGlobalTexture("mask2", mask);

        // Speed
        Shader.SetGlobalFloat("S_1", speed);
        Shader.SetGlobalFloat("S_2", speed2);
        Shader.SetGlobalFloat("S_3", speed3);
        Shader.SetGlobalFloat("S_4", speed4);

        // Tiling
        Shader.SetGlobalFloat("T_X_1", t_X_1);
        Shader.SetGlobalFloat("T_Y_1", t_Y_1);
        Shader.SetGlobalFloat("T_X_2", t_X_2);
        Shader.SetGlobalFloat("T_Y_2", t_Y_2);
        Shader.SetGlobalFloat("T_X_3", t_X_3);
        Shader.SetGlobalFloat("T_Y_3", t_Y_3);
        Shader.SetGlobalFloat("T_X_4", t_X_4);
        Shader.SetGlobalFloat("T_Y_4", t_Y_4);
    }

}
