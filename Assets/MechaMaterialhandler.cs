using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaMaterialhandler : MonoBehaviour
{
    public Material baseMaterial;
    public Material selectedMechaPartMaterial;
    public Material selectedMechaMaterial;
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
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    SetSelectedPartMaterialToBody(MechaParts.Body, true);
        //}
        //if (Input.GetKeyDown(KeyCode.W))
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
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    SetSelectedMechaMaterial(true);
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    SetSelectedMechaMaterial(false);
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
            _rend = null;
            _child = transform.GetChild(i);
            _rend = transform.GetChild(i).gameObject.GetComponent<Renderer>();
            if (_rend != null)
            {
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
    /// Set the fresnel material to the second material.
    /// </summary>
    public void SetSelectedMechaMaterial(bool isEffectOn)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _child = transform.GetChild(i);
            _rend = transform.GetChild(i).gameObject.GetComponent<Renderer>();
            if (_rend != null)
            {
                if (isEffectOn)
                {
                    _rend.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).
                    _sharedMaterialsCopy = _rend.sharedMaterials;
                    _sharedMaterialsCopy[1] = selectedMechaMaterial;
                    _rend.sharedMaterials = _sharedMaterialsCopy;
                }
                else
                {
                    _rend.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).
                    _sharedMaterialsCopy = _rend.sharedMaterials;
                    _sharedMaterialsCopy[1] = baseMaterial;
                    _rend.sharedMaterials = _sharedMaterialsCopy;
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