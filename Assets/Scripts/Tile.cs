﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{
    public bool isWalkable;
    public MeshRenderer render;
    public List<Tile> neighbours;

    private void Start()
    {
        GetNeighbours();
    }

    public void MakeWalkable()
    {
        isWalkable = true;
        Material mat = new Material(render.sharedMaterial);
        mat.color = Color.green;

        render.sharedMaterial = mat;
    }
    public void MakeNotWalkable()
    {
        isWalkable = false;
        Material mat = new Material(render.sharedMaterial);
        mat.color = Color.red;

        render.sharedMaterial = mat;
    }

    //public void Change()
    //{
    //    if (isWalkable)
    //        MakeNotWalkable();
    //    else MakeWalkable();
    //}

    void RayToNeighbours(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, 1))
        {
            var neighbour = hit.collider.GetComponent<Tile>();
            if (neighbour != null && neighbour.isWalkable)
                neighbours.Add(neighbour);
        }
    }

    public void PreviewColor()
    {
        Material mat = new Material(render.sharedMaterial);
        mat.color = Color.blue;

        render.sharedMaterial = mat;
    }

    public void EndPreview()
    {
        Material mat = new Material(render.sharedMaterial);

        if (isWalkable)
            mat.color = Color.green;
        else mat.color = Color.red;

        render.sharedMaterial = mat;
    }

    void GetNeighbours()
    {
        RayToNeighbours(Vector3.right);
        RayToNeighbours(Vector3.left);
        RayToNeighbours(Vector3.forward);
        RayToNeighbours(Vector3.back);
        RayToNeighbours(Vector3.up);
        RayToNeighbours((Vector3.up + Vector3.right).normalized);
        RayToNeighbours((Vector3.up + Vector3.back).normalized);
        RayToNeighbours((Vector3.up + Vector3.left).normalized);
        RayToNeighbours((Vector3.up + Vector3.forward).normalized);
        RayToNeighbours(Vector3.down);
        RayToNeighbours((Vector3.down + Vector3.right).normalized);
        RayToNeighbours((Vector3.down + Vector3.back).normalized);
        RayToNeighbours((Vector3.down + Vector3.left).normalized);
        RayToNeighbours((Vector3.down + Vector3.forward).normalized);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
