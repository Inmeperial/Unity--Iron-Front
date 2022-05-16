using System;
using System.Collections.Generic;
using UnityEngine;

public class MasterShaderScript : MonoBehaviour
{
    //Need to add all the Keys in SwitchTextureEnum by hand , we cant know the strings by code.

    //public TexturesForMasterShader textureForMasterShader;

    [SerializeField] private Renderer _renderer;
    public bool isWeapon = false;
    public MechaEnum mechaEnum;
    public Texture[] texturesCuerpo = new Texture[3];
    public Texture[] textureArmadura = new Texture[3];
    public Texture[] textureArticulaciones = new Texture[4];
    public Color colorMecha;
    public int materialArticulacion;
    public int materialCuerpo;
    public int materialArmadura;
    private string[] _shaderArrayString;
    private List<Material> _matList = new List<Material>();

    private void Initialize()
    {
        _shaderArrayString = new string[Enum.GetNames(typeof(StringTextureEnum)).Length];

        _shaderArrayString[0] = StringTextureEnum._SwitchTexture_Key0.ToString();
        _shaderArrayString[1] = StringTextureEnum._SwitchTexture_Key1.ToString();
        _shaderArrayString[2] = StringTextureEnum._SwitchTexture_Key2.ToString();

        Material[] mats = _renderer.materials;
        _matList = new List<Material>();

        for (int i = 0; i < mats.Length; i++)
        {
            _matList.Add(mats[i]);
        }
        for (int i = 0; i < _matList.Count; i++)
        {
            if (isWeapon)
            {
                _matList[i].SetInt("_isWeapon", 1);
            }
            else
            {
                _matList[i].SetInt("_isWeapon", 0);
            }
        }

        //SetMaterialsForMecha();
        ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureClean);
        SetMechaColor(colorMecha);
        SetOutLine(false);
    }

    public void SetMaterialsForMecha()
    {
        SetTexturesToMaterial(materialCuerpo, texturesCuerpo, false);
        SetTexturesToMaterial(materialArmadura, textureArmadura, false);
        SetTexturesToMaterial(materialArticulacion, textureArticulaciones, true);
    }

    public string[] GetArrayStringForShaderSwitch()
    {
        return _shaderArrayString;
    }

    public void SetMechaColor(Color color)
    {
        for (int i = 0; i < _matList.Count; i++)
        {
            _matList[i].SetColor("_ColorAlbedo", color);
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
            case SwitchTextureEnum.TextureOutLine:
                SetOutLine(true);
                break;
            default:
                SetTextureInShader(StringTextureEnum._SwitchTexture_Key0);
                break;
        }
    }

    private void SetOutLine(bool key)
    {

        //if (_matArr == null || _matArr.Length < 1)
        //{
        //    if (_matArr == null)
        //        Debug.Log("null");
        //    return;
        //}
        

        for (int i = 0; i < _matList.Count; i++)
        {
            

            if (key)
            {
                _matList[i].SetInt("_isOutLineOn", 1);
            }
            else
            {
                _matList[i].SetInt("_isOutLineOn", 0);
            }
        }

    }

    private void SetTexturesToMaterial(int matArrayNum, Texture[] arr, bool setEmission)
    {
        if (arr[0] != null)
        {
            _matList[matArrayNum].SetTexture("_TextureAlbedo", arr[0]);
        }
        if (arr[1] != null)
        {
            _matList[matArrayNum].SetTexture("_TextureNormal", arr[1]);
        }
        if (!isWeapon)
        {
            if (arr[2] != null)
            {
                _matList[matArrayNum].SetTexture("_MaskAlbedo", arr[2]);
            }

            if (setEmission)
            {
                _matList[matArrayNum].SetInt("_IsEmissionON", 1);
                _matList[matArrayNum].SetTexture("_TextureEmission", arr[3]);
            }

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
        SetOutLine(false);
        for (int i = 0; i < _matList.Count; i++)
        {
            for (int j = 0; j < _shaderArrayString.Length; j++)
            {
                if (_shaderArrayString[j] == key.ToString())
                {
                    _matList[i].EnableKeyword(_shaderArrayString[j]);
                }
                else
                {
                    _matList[i].DisableKeyword(_shaderArrayString[j]);
                }
            }
        }
    }

    public void SetData(MasterShaderScript prefab, Material bodyMat, Material jointsMat, Material armorMat)
    {
        isWeapon = prefab.isWeapon;
        mechaEnum = prefab.mechaEnum;
        texturesCuerpo = prefab.texturesCuerpo;
        textureArmadura = prefab.textureArmadura;
        textureArticulaciones = prefab.textureArticulaciones;
        colorMecha = prefab.colorMecha;
        materialArticulacion = prefab.materialArticulacion;
        materialCuerpo = prefab.materialCuerpo;
        materialArmadura = prefab.materialArmadura;

        //SkinnedMeshRenderer renderer = GetComponent<SkinnedMeshRenderer>();

        Material[] orderedMaterials = new Material[3];
        
        orderedMaterials[materialCuerpo] = bodyMat;
        orderedMaterials[materialArmadura] = armorMat;
        orderedMaterials[materialArticulacion] = jointsMat;

        foreach (Material material in orderedMaterials)
        {
            material.EnableKeyword("_TextureNormal");
            material.EnableKeyword("_MaskAlbedo");
            material.EnableKeyword("_TextureEmission");
        }

        //prefab.texturesXXX[1] is NormalMap
        orderedMaterials[materialCuerpo].SetTexture("_TextureNormal", prefab.texturesCuerpo[1]);
        orderedMaterials[materialArmadura].SetTexture("_TextureNormal", prefab.textureArmadura[1]);
        orderedMaterials[materialArticulacion].SetTexture("_TextureNormal", prefab.textureArticulaciones[1]);

        //prefab.texturesXXX[2] is MaskMap
        orderedMaterials[materialCuerpo].SetTexture("_MaskAlbedo", prefab.texturesCuerpo[2]);
        orderedMaterials[materialArmadura].SetTexture("_MaskAlbedo", prefab.textureArmadura[2]);
        orderedMaterials[materialArticulacion].SetTexture("_MaskAlbedo", prefab.textureArticulaciones[2]);

        //prefab.texturesXXX[3] is Emission
        orderedMaterials[materialArticulacion].SetTexture("_TextureEmission", prefab.textureArticulaciones[3]);

        _renderer.materials = orderedMaterials;
        Initialize();
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
    TextureHighLight,
    TextureOutLine
};

//TODO: Ver de borrar
public enum MechaEnum
{
    Mecha1,
    Mecha2
}
