using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{
    public bool isWalkable;
    public MeshRenderer render;
    public List<Tile> neighbours;

    private void Start()
    {
        GetNeightbours(Vector3.right);
        GetNeightbours(Vector3.left);
        GetNeightbours(Vector3.forward);
        GetNeightbours(Vector3.back);
        GetNeightbours(Vector3.up);
        GetNeightbours(Vector3.down);
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

    public void Change()
    {
        if (isWalkable)
            MakeNotWalkable();
        else MakeWalkable();
    }

    void GetNeightbours(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, 2.2f))
        {
            var neighbour = hit.collider.GetComponent<Tile>();
            if (neighbour != null && neighbour.isWalkable)
                neighbours.Add(neighbour);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
