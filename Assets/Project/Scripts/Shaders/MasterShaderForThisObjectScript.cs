using UnityEngine;

public class MasterShaderForThisObjectScript : MonoBehaviour
{
    public Texture[] textures = new Texture[2];
    public Color colorMecha;
    //private MasterShaderScript _masterShader;
    //private string[] _shaderArrayString;

    // Start is called before the first frame update
    void Start()
    {
        //_masterShader = new MasterShaderScript();
        //_shaderArrayString = new string[Enum.GetNames(typeof(SwitchTextureEnum)).Length];
        //_shaderArrayString = _masterShader.GetArrayStringForShaderSwitch();
        SetShaderForChilds();
    }

    private void SetShaderForChilds()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).GetComponent<MeshRenderer>())
            {
                Material mat = this.transform.GetChild(i).GetComponent<MeshRenderer>().material;
                mat.SetTexture("_TextureAlbedo", textures[0]);
                mat.SetTexture("_TextureNormal", textures[1]);
                mat.EnableKeyword("_SwitchTexture_Key0");
                mat.SetColor("_ColorAlbedo", colorMecha);
            }
        }
    }

    //private void SetTextureInShader(Material material)
    //{
    //    for (int j = 0; j < _shaderArrayString.Length; j++)
    //    {
    //        if (_shaderArrayString[j] == "_SwitchTexture_Key0")
    //        {
    //            material.EnableKeyword(_shaderArrayString[j]);
    //        }
    //        else
    //        {
    //            material.DisableKeyword(_shaderArrayString[j]);
    //        }
    //    }
    //}
}
