using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMaterialhandler : MonoBehaviour
{
    public Material MoveMaterial;
    public Material AttackMaterial;
    public Material AttackAndMoveMaterial;
    public Material MortarAoeAttackMaterial;
    public Material ActivationAoeForMorter;
    public Material MechaToAttackInAnimationTileMaterial;
    //----------------------
    private Renderer _rend;
    private Transform _childStatusNode;
    private Transform _childSelectedNode;
    private Material _sharedMaterialCopy;

    void Start()
    {
        GetChilds();
        if (_childStatusNode == null)
        {
            Debug.Log("Cant Find PlaneForMove child in Node");
        }
        if (_childSelectedNode == null)
        {
            Debug.Log("Cant Find PlaneForMouse child in Node");
        }

        DiseableAndEnableSelectedNode(false);
        DiseableAndEnableStatus(false);
    }

    #region Functions

    /// <summary>
    /// Diseable and Enable selected material node.
    /// </summary>
    public void DiseableAndEnableSelectedNode(bool isStatusOn)
    {
        _childSelectedNode.gameObject.SetActive(isStatusOn);
    }

    /// <summary>
    /// Diseable and Enable status of the node.
    /// </summary>
    /// <param name="isStatusOn"></param>
    public void DiseableAndEnableStatus(bool isStatusOn)
    {
        _childStatusNode.gameObject.SetActive(isStatusOn);
    }

    /// <summary>
    /// Change the material of the node to move.
    /// </summary>
    public void StatusToMove()
    {
        _rend = _childStatusNode.gameObject.GetComponent<Renderer>();
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
        _rend = _childStatusNode.gameObject.GetComponent<Renderer>();
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
        _rend = _childStatusNode.gameObject.GetComponent<Renderer>();
        _rend.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).

        _sharedMaterialCopy = _rend.sharedMaterial;
        _sharedMaterialCopy = AttackAndMoveMaterial;
        _rend.sharedMaterial = _sharedMaterialCopy;
    }

    public void StatusMortarAoEOfAttack()
    {
        _rend = _childStatusNode.gameObject.GetComponent<Renderer>();
        _rend.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).

        _sharedMaterialCopy = _rend.sharedMaterial;
        _sharedMaterialCopy = MortarAoeAttackMaterial;
        _rend.sharedMaterial = _sharedMaterialCopy;
    }

    public void StatusMortarBulletAoEOfAttack(bool isStatusOn)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "MortarBullet")
            {
                transform.GetChild(i).gameObject.SetActive(isStatusOn);
            }
        }
    }

    public void StatusTileActivationRageForMorter()
    {
        _rend = _childStatusNode.gameObject.GetComponent<Renderer>();
        _rend.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).

        _sharedMaterialCopy = _rend.sharedMaterial;
        _sharedMaterialCopy = ActivationAoeForMorter;
        _rend.sharedMaterial = _sharedMaterialCopy;
    }

    public void StatusTileToMoveToLastTileSelected()
    {
        _rend = _childStatusNode.gameObject.GetComponent<Renderer>();
        _rend.enabled = true; //we need this because sometimes unity doesn't make the 2 mesh visible (unity bugs).

        _sharedMaterialCopy = _rend.sharedMaterial;
        _sharedMaterialCopy = MechaToAttackInAnimationTileMaterial;
        _rend.sharedMaterial = _sharedMaterialCopy;
    }

    #endregion

    public void GetChilds()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "PlaneForMove")
            {
                _childStatusNode = transform.GetChild(i);
                continue;
            }
            if (transform.GetChild(i).gameObject.name == "PlaneForMouse")
            {
                _childSelectedNode = transform.GetChild(i);
                continue;
            }
        }
    }
}


