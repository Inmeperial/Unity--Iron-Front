using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMaterialhandler : MonoBehaviour
{
    [Header("Materials")]
    public Material MoveMaterial;
    public Material AttackMaterial;
    public Material AttackAndMoveMaterial;
    public Material MortarAoeAttackMaterial;
    public Material ActivationAoeForMortar;
    public Material MechaToAttackInAnimationTileMaterial;

    [Header("Plane Obj")]
    public GameObject childStatusNode;
    public GameObject childStatusActivationMortar;
    public GameObject childStatusAoEMortar;
    public GameObject childSelectedNode;
    //----------------------
    private Renderer _rend;
    private Material _sharedMaterialCopy;

    void Start()
    {
       
        //GetChilds();

        DiseableAndEnableSelectedNode(false);
        DiseableAndEnableStatus(false);
    }

    #region Functions

    /// <summary>
    /// Diseable and Enable selected material node.
    /// </summary>
    public void DiseableAndEnableSelectedNode(bool isStatusOn)
    {
        childSelectedNode.gameObject.SetActive(isStatusOn);
    }

    public void DiseableAndEnableSelectedNodeForMortar(bool isStatusOn)
    {
        childStatusAoEMortar.gameObject.SetActive(isStatusOn);
    }
    
    public void DiseableAndEnableActivationNodeForMortar(bool isStatusOn)
    {
        childStatusActivationMortar.gameObject.SetActive(isStatusOn);
    }

    /// <summary>
    /// Diseable and Enable status of the node.
    /// </summary>
    /// <param name="isStatusOn"></param>
    public void DiseableAndEnableStatus(bool isStatusOn)
    {
        childStatusNode.gameObject.SetActive(isStatusOn);
    }

    /// <summary>
    /// Change the material of the node to move.
    /// </summary>
    public void StatusToMove()
    {
        _rend = childStatusNode.gameObject.GetComponent<Renderer>();
        _rend.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).

        _sharedMaterialCopy = _rend.sharedMaterial;
        _sharedMaterialCopy = MoveMaterial;
        _rend.sharedMaterial = _sharedMaterialCopy;
    }

    /// <summary>
    /// Change the material of the node to attack.
    /// </summary>
    public void StatusToAttack()
    {
        _rend = childStatusNode.gameObject.GetComponent<Renderer>();
        _rend.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).

        _sharedMaterialCopy = _rend.sharedMaterial;
        _sharedMaterialCopy = AttackMaterial;
        _rend.sharedMaterial = _sharedMaterialCopy;
    }

    /// <summary>
    /// Change the material of the node to attack and move.
    /// </summary>
    public void StatusToAttackAndMove()
    {
        _rend = childStatusNode.gameObject.GetComponent<Renderer>();
        _rend.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).

        _sharedMaterialCopy = _rend.sharedMaterial;
        _sharedMaterialCopy = AttackAndMoveMaterial;
        _rend.sharedMaterial = _sharedMaterialCopy;
    }

    //public void StatusAoEOfAttackForMortar()
    //{
    //    _rend = _childStatusActivationMortar.gameObject.GetComponent<Renderer>();
    //    _rend.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).

    //    _sharedMaterialCopy = _rend.sharedMaterial;
    //    _sharedMaterialCopy = MortarAoeAttackMaterial;
    //    _rend.sharedMaterial = _sharedMaterialCopy;
    //}

    public void StatusBulletAoEOfAttackForMortar(bool isStatusOn)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "MortarBullet")
            {
                transform.GetChild(i).gameObject.SetActive(isStatusOn);
            }
        }
    }

    public void StatusAoEOfAttackForMortar(bool isStatusOn)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "PlaneForAoEMortar")
            {
                transform.GetChild(i).gameObject.SetActive(isStatusOn);
            }
        }
    }

    public void StatusActivationRageForMortar()
    {
        _rend = childStatusActivationMortar.gameObject.GetComponent<Renderer>();
        _rend.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).

        _sharedMaterialCopy = _rend.sharedMaterial;
        _sharedMaterialCopy = ActivationAoeForMortar;
        _rend.sharedMaterial = _sharedMaterialCopy;
    }

    public void StatusTileToMoveToLastTileSelected()
    {
        _rend = childStatusNode.gameObject.GetComponent<Renderer>();
        _rend.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).

        _sharedMaterialCopy = _rend.sharedMaterial;
        _sharedMaterialCopy = MechaToAttackInAnimationTileMaterial;
        _rend.sharedMaterial = _sharedMaterialCopy;
    }

    #endregion

    //public void GetChilds()
    //{
    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        if (transform.GetChild(i).gameObject.name == "PlaneForMove")
    //        {
    //            childStatusNode = transform.GetChild(i);
    //            continue;
    //        }
    //        if (transform.GetChild(i).gameObject.name == "PlaneForMouse")
    //        {
    //            childSelectedNode = transform.GetChild(i);
    //            continue;
    //        }

    //        if (transform.GetChild(i).gameObject.name == "PlaneForAoEMortar")
    //        {
    //            _childStatusAoEMortar = transform.GetChild(i);
    //            continue;
    //        }
            
    //        if (transform.GetChild(i).gameObject.name == "PlaneForMortar")
    //        {
    //            childStatusActivationMortar = transform.GetChild(i);
    //        }
    //    }
    //}
}


