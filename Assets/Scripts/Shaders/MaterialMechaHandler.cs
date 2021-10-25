using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialMechaHandler : MonoBehaviour
{
    private Material _bodyMaterial;
    private Material _armMaterial;
    private Material _legMaterial;
    public Material selectedMechaPartMaterial;
    public Material selectedMechaMaterial;
    //----------------------
    private Renderer _rend;
    private Transform _child;
    private Material[] _sharedMaterialsCopy;

    private Body _body;
    private Arm _leftArm;
    private Arm _rightArm;
    private Legs _legs;
    void Start()
    {
        //SetBaseAndSecondMaterial();
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        SetSelectedPartMaterialToBody(MechaParts.Body, true);
    //    }
    //    if (Input.GetKeyDown(KeyCode.W))
    //    {
    //        SetSelectedPartMaterialToBody(MechaParts.Body, false);
    //    }
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        SetSelectedPartMaterialToBody(MechaParts.Legs, true);
    //    }
    //    if (Input.GetKeyDown(KeyCode.R))
    //    {
    //        SetSelectedPartMaterialToBody(MechaParts.Legs, false);
    //    }
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        SetSelectedMechaMaterial(true);
    //    }
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        SetSelectedMechaMaterial(false);
    //    }
    //}

    public void SetHandlerMaterial(Material bodyMaterial, Material armMaterial, Material legMaterial)
    {
        _bodyMaterial = bodyMaterial;
        _armMaterial = armMaterial;
        _legMaterial = legMaterial;
    }

    public void SetPartGameObject(Body body, Arm leftArm, Arm rightArm, Legs legs)
    {
        _body = body;
        _leftArm = leftArm;
        _rightArm = rightArm;
        _legs = legs;
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
            if (_rend != null && _child.gameObject.GetComponent<ParticleSystem>() == null)
            {
                _rend.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).
                _sharedMaterialsCopy = _rend.sharedMaterials;

                for (int j = 0; j < _sharedMaterialsCopy.Length; j++)
                {
                    _sharedMaterialsCopy[j] = _bodyMaterial;
                }
                _rend.sharedMaterials = _sharedMaterialsCopy;
            }

        }
    }

    //TODO: revisar cuando este los materiales
    /// <summary>
    /// Set the fresnel material to the second material.
    /// </summary>
    public void SetSelectedMechaMaterial(bool isEffectOn)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _child = transform.GetChild(i);
            _rend = transform.GetChild(i).gameObject.GetComponent<Renderer>();
            if (_rend != null && _child.gameObject.GetComponent<ParticleSystem>() == null)
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
                    _sharedMaterialsCopy[1] = _bodyMaterial;
                    _rend.sharedMaterials = _sharedMaterialsCopy;
                }
            }
        }
    }

    //TODO: revisar cuando este los materiales
    /// <summary>
    /// Set the selected part material to the second material, needs a part to send (Enum MechaParts)
    /// </summary>
    // public void SetSelectedPartMaterialToBody(MechaParts part, bool isEffectOn)
    // {
    //     MeshFilter[] filter = new MeshFilter[0];
    //     Material partMaterial = new Material(_bodyMaterial);
    //     switch (part)
    //     {
    //         case MechaParts.Body:
    //             filter = _body.meshFilter;
    //             partMaterial = _bodyMaterial;
    //             break;
    //         case MechaParts.Legs:
    //             filter = _legs.meshFilter;
    //             partMaterial = _legMaterial;
    //             break;
    //         case MechaParts.RArm:
    //             filter = _rightArm.meshFilter;
    //             partMaterial = _armMaterial;
    //             break;
    //         case MechaParts.LArm:
    //             filter = _leftArm.meshFilter;
    //             partMaterial = _armMaterial;
    //             break;
    //     }
    //
    //     MeshRenderer meshRenderer;
    //     foreach (var meshFilter in filter)
    //     {
    //         if (isEffectOn)
    //         {
    //             meshRenderer = meshFilter.gameObject.GetComponent<MeshRenderer>();
    //             meshRenderer.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).
    //
    //             _sharedMaterialsCopy = meshRenderer.sharedMaterials;
    //             _sharedMaterialsCopy[1] = selectedMechaPartMaterial;
    //             meshRenderer.sharedMaterials = _sharedMaterialsCopy;
    //             break;
    //         }
    //         else
    //         {
    //             meshRenderer = meshFilter.gameObject.GetComponent<MeshRenderer>();
    //             meshRenderer.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).
    //
    //             _sharedMaterialsCopy = meshRenderer.sharedMaterials;
    //             _sharedMaterialsCopy[1] = partMaterial;
    //             meshRenderer.sharedMaterials = _sharedMaterialsCopy;
    //             break;
    //
    //         }
    //     }
        // for (int i = 0; i < transform.childCount; i++)
        // {
        //     _child = transform.GetChild(i);
        //     if (_child.gameObject.name == part.ToString())
        //     {
        //         if (isEffectOn)
        //         {
        //             _rend = _child.gameObject.GetComponent<Renderer>();
        //             _rend.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).
        //
        //             _sharedMaterialsCopy = _rend.sharedMaterials;
        //             _sharedMaterialsCopy[1] = selectedMechaPartMaterial;
        //             _rend.sharedMaterials = _sharedMaterialsCopy;
        //             break;
        //         }
        //         else
        //         {
        //             _rend = _child.gameObject.GetComponent<Renderer>();
        //             _rend.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).
        //
        //             _sharedMaterialsCopy = _rend.sharedMaterials;
        //             _sharedMaterialsCopy[1] = bodyMaterial;
        //             _rend.sharedMaterials = _sharedMaterialsCopy;
        //             break;
        //
        //         }
        //     }
        // }
//     }
//
    #endregion
}

public enum MechaParts
{
    Body,
    Legs,
    RArm,
    LArm
}