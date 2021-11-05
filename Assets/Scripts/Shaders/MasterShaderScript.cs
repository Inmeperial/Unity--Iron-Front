using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterShaderScript : MonoBehaviour
{
    //Need to add all the Keys in SwitchTextureEnum by hand , we cant know the strings by code.

    //public TexturesForMasterShader textureForMasterShader;

    public MechaEnum mechaEnum;
    public Texture[] texturesCuerpo = new Texture[2];
    public Texture[] textureArmadura = new Texture[2];
    public Texture[] textureArticulaciones = new Texture[2];
    public Color colorMecha;
    public int materialArticulacion;
    public int materialCuerpo;
    public int materialArmadura;
    private string[] _shaderArrayString;
    private Material[] _matArr = default;

    void Start()
    {
        _shaderArrayString = new string[Enum.GetNames(typeof(SwitchTextureEnum)).Length];

        _shaderArrayString[0] = StringTextureEnum._SwitchTexture_Key0.ToString();
        _shaderArrayString[1] = StringTextureEnum._SwitchTexture_Key1.ToString();
        _shaderArrayString[2] = StringTextureEnum._SwitchTexture_Key2.ToString();

        _matArr = new Material[this.GetComponent<Renderer>().materials.Length];

        for (int i = 0; i < _matArr.Length; i++)
        {
            _matArr[i] = this.GetComponent<Renderer>().materials[i];
        }
        SetMaterialsForMecha();
        ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureClean);
        SetMechaColor(colorMecha);
    }

    public void SetMaterialsForMecha()
    {
        SetTexturesToMaterial(materialCuerpo, texturesCuerpo);
        SetTexturesToMaterial(materialArmadura, textureArmadura);
        SetTexturesToMaterial(materialArticulacion, textureArticulaciones);
    }

    public string[] GetArrayStringForShaderSwitch()
    {
        return _shaderArrayString;
    }

    public void SetMechaColor(Color color)
    {
        for (int i = 0; i < _matArr.Length; i++)
        {
           _matArr[i].SetColor("_ColorAlbedo", color);
        }
    }

    public void ConvertEnumToStringEnumForShader(SwitchTextureEnum enumKey)
    {
        switch (enumKey)
        {
            case SwitchTextureEnum.TextureClean:
                SetTextureInShader(StringTextureEnum._SwitchTexture_Key0);
                break;
            case SwitchTextureEnum.TextureFresnel:
                SetTextureInShader(StringTextureEnum._SwitchTexture_Key1);
                break;
            case SwitchTextureEnum.TextureHighLight:
                SetTextureInShader(StringTextureEnum._SwitchTexture_Key2);
                break;
            default:
                SetTextureInShader(StringTextureEnum._SwitchTexture_Key0);
                break;
        }
    }

    private void SetTexturesToMaterial(int matArrayNum, Texture[] arr)
    {
        if (arr[0] != null)
        {
            _matArr[matArrayNum].SetTexture("_TextureAlbedo", arr[0]);
        }
        if (arr[1] != null)
        {
            _matArr[matArrayNum].SetTexture("_TextureNormal", arr[1]);
        }
        //if (arr[2] != null)
        //{
        //    _matArr[matArrayNum].SetTexture("_TextureMask", arr[2]);
        //}
        //if (arr.Length > 3)
        //{
        //    if (arr[3] != null)
        //    {
        //        _matArr[matArrayNum].SetTexture("_TextureEmission", arr[3]);
        //    }
        //}
    }

    private void SetTextureInShader(StringTextureEnum key)
    {
        for (int i = 0; i < _matArr.Length; i++)
        {
            for (int j = 0; j < _shaderArrayString.Length; j++)
            {
                if (_shaderArrayString[j] == key.ToString())
                {
                    _matArr[i].EnableKeyword(_shaderArrayString[j]);
                }
                else
                {
                    _matArr[i].DisableKeyword(_shaderArrayString[j]);
                }
            }
        }
    }

    private enum StringTextureEnum
    {
        _SwitchTexture_Key0,
        _SwitchTexture_Key1,
        _SwitchTexture_Key2
    };

}

public enum SwitchTextureEnum
{
    TextureClean,
    TextureFresnel,
    TextureHighLight
};

public enum MechaEnum
{
    Mecha1,
    Mecha2
}
