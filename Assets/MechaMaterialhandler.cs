using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaMaterialhandler : MonoBehaviour
{
    public Material baseMaterial;
    public Material selectedMechaMaterial;
    public Material fresnelMechaMaterial;
    //----------------------
    private Renderer _rend;
    private Transform _child;
    private Material[] _sharedMaterialsCopy;

    void Start()
    {
        SetBaseAndSecondMaterial();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    SetSelectedPartMaterialToBody(MechaParts.Body, true);
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    SetSelectedPartMaterialToBody(MechaParts.Body, false);
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    SetSelectedPartMaterialToBody(MechaParts.Legs, true);
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    SetSelectedPartMaterialToBody(MechaParts.Legs, false);
        //}
    }

    #region Functions

    /// <summary>
    /// Set the base and second material with the base material.
    /// </summary>
    public void SetBaseAndSecondMaterial()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _child = transform.GetChild(i);
            if (_child.gameObject.name != "WorldCanvas" && _child.gameObject.name != "GameObject" )
            {
                _rend = transform.GetChild(i).gameObject.GetComponent<Renderer>();
                _rend.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).

                _sharedMaterialsCopy = _rend.sharedMaterials;

                for (int j = 0; j < _sharedMaterialsCopy.Length; j++)
                {
                    _sharedMaterialsCopy[j] = baseMaterial;
                }

                _rend.sharedMaterials = _sharedMaterialsCopy;
            }
        }
    }

    /// <summary>
    /// Set the fresnel material to the second material, needs a part to send (Enum MechaParts)
    /// </summary>
    public void SetFresnelMaterial(MechaParts part, bool isEffectOn)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _child = transform.GetChild(i);
            if (_child.gameObject.name == part.ToString())
            {
                if (isEffectOn)
                {
                    _rend = _child.gameObject.GetComponent<Renderer>();
                    _rend.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).

                    _sharedMaterialsCopy = _rend.sharedMaterials;
                    _sharedMaterialsCopy[1] = fresnelMechaMaterial;
                    _rend.sharedMaterials = _sharedMaterialsCopy;
                    break;
                }
                else
                {
                    _rend = _child.gameObject.GetComponent<Renderer>();
                    _rend.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).

                    _sharedMaterialsCopy = _rend.sharedMaterials;
                    _sharedMaterialsCopy[1] = baseMaterial;
                    _rend.sharedMaterials = _sharedMaterialsCopy;
                    break;

                }
            }
        }
    }

    /// <summary>
    /// Set the selected part material to the second material, needs a part to send (Enum MechaParts)
    /// </summary>
    public void SetSelectedPartMaterialToBody(MechaParts part, bool isEffectOn)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _child = transform.GetChild(i);
            if (_child.gameObject.name == part.ToString())
            {
                if (isEffectOn)
                {
                    _rend = _child.gameObject.GetComponent<Renderer>();
                    _rend.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).

                    _sharedMaterialsCopy = _rend.sharedMaterials;
                    _sharedMaterialsCopy[1] = selectedMechaMaterial;
                    _rend.sharedMaterials = _sharedMaterialsCopy;

                    break;
                }
                else
                {
                    _rend = _child.gameObject.GetComponent<Renderer>();
                    _rend.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).

                    _sharedMaterialsCopy = _rend.sharedMaterials;
                    _sharedMaterialsCopy[1] = baseMaterial;
                    _rend.sharedMaterials = _sharedMaterialsCopy;
                    break;

                }
            }
        }
    }

    #endregion
}

public enum MechaParts
{
    Body,
    Legs,
    RArm,
    LArm
}