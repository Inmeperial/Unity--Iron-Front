using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMaterialhandler : MonoBehaviour
{
    public Material MoveMaterial;
    public Material AttackMaterial;
    public Material AttackAndMoveMaterial;
    //----------------------
    private Renderer _rend;
    private Transform _childStatusNode;
    private Transform _childSelectedNode;
    private Material _sharedMaterialCopy;

    void Start()
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
    
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    DiseableAndEnableStatus(true);
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    DiseableAndEnableStatus(false);
        //}
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    StatusToMove();
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    StatusToAttack();
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    StatusToAttackAndMove();
        //}
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    DiseableAndEnableSelectedNode(true);
        //}
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    DiseableAndEnableSelectedNode(false);
        //}
    }
    #region Functions

    /// <summary>
    /// Diseable and Enable selected material node.
    /// </summary>
    public void DiseableAndEnableSelectedNode(bool status)
    {
        _childSelectedNode.gameObject.SetActive(status);
    }

    /// <summary>
    /// Diseable and Enable status of the node.
    /// </summary>
    /// <param name="status"></param>
    public void DiseableAndEnableStatus(bool status)
    {
        _childStatusNode.gameObject.SetActive(status);
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

    #endregion
}


