using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterShaderScript : MonoBehaviour
{
    //Need to add all the Keys in SwitchTextureEnum by hand , we cant know the strings by code.

    // necesito un for para todos los mesh del mesh renderer. Ponerlo como un array porque las piernas tienen 2 para la der e izq

    public Texture textureBase, textureNormal, textureEmission, textureMask = default;
    public Texture[] textureAlbedo = new Texture[4];
    // public Material materialShader = default;
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

            // = materialShader;
            // Renderer _rend = this.GetComponent<Renderer>().materials[i];

            // Material[] _sharedMaterialsCopy;
            // _rend.enabled = true;

            _matArr[i].SetTexture("_TextureBase", textureBase);
            _matArr[i].SetTexture("_TextureNormal", textureNormal);
            _matArr[i].SetTexture("_TextureEmission", textureEmission);
            _matArr[i].SetTexture("_TextureMask", textureMask);
        }

        ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureClean);

        //_material.SetTexture("_TextureBase", textureBase);
        //_material.SetTexture("_TextureNormal", textureNormal);
        //_material.SetTexture("_TextureEmission", textureEmission);
        //_material.SetTexture("_TextureMetallic", textureMetallic);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureClean);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureFresnel);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureHighLight);
        }
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
