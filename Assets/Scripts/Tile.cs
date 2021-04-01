using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{
    public bool isWalkable;
    public MeshRenderer render;
    public List<Tile> neighbours;
    public LayerMask obstacle;
    private bool _isFree = true;
    private void Start()
    {
        _isFree = true;
        if (TileAbove() == false)
            GetNeighbours();
        else MakeNotWalkableColor();
    }

    //Make this tile walkable.
    public void MakeWalkableColor()
    {
        isWalkable = true;
        Material mat = new Material(render.sharedMaterial);
        mat.color = Color.green;

        render.sharedMaterial = mat;
    }
    //Make this tile not walkable.
    public void MakeNotWalkableColor()
    {
        isWalkable = false;
        Material mat = new Material(render.sharedMaterial);
        mat.color = Color.red;

        render.sharedMaterial = mat;
    }

    //Cast a ray to neighbouring tiles and check if they are walkable.
    void RayToNeighbours(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, 1))
        {
            var neighbour = hit.collider.GetComponent<Tile>();
            if (neighbour != null && neighbour.isWalkable)
                if (!neighbours.Contains(neighbour))
                    neighbours.Add(neighbour);
        }
    }

    //Change tile color for pathfinding preview.
    public void PathFindingPreviewColor()
    {
        Material mat = new Material(render.sharedMaterial);
        mat.color = Color.blue;

        render.sharedMaterial = mat;
    }

    //Revert tile color when pathfinding preview ends.
    public void EndPathfindingPreviewColor()
    {
        Material mat = new Material(render.sharedMaterial);

        if (isWalkable)
            mat.color = Color.green;
        else mat.color = Color.red;

        render.sharedMaterial = mat;
    }

    public void MouseOverColor()
    {
        Material mat = new Material(render.sharedMaterial);
        mat.color = Color.yellow;

        render.sharedMaterial = mat;
    }

    public void NotSelectedColor()
    {
        Material mat = new Material(render.sharedMaterial);
        mat.color = Color.green;

        render.sharedMaterial = mat;
    }

    public void InRangeColor()
    {
        Material mat = new Material(render.sharedMaterial);
        mat.color = Color.white;

        render.sharedMaterial = mat;
    }



    void GetNeighbours()
    {
        RayToNeighbours(Vector3.right);
        RayToNeighbours(Vector3.left);
        RayToNeighbours(Vector3.forward);
        RayToNeighbours(Vector3.back);
        RayToNeighbours((Vector3.up + Vector3.right + new Vector3(0, 0.01f, 0)).normalized);
        RayToNeighbours((Vector3.up + Vector3.back + new Vector3(0, 0.01f, 0)).normalized);
        RayToNeighbours((Vector3.up + Vector3.left + new Vector3(0, 0.01f, 0)).normalized);
        RayToNeighbours((Vector3.up + Vector3.forward + new Vector3(0, 0.01f, 0)).normalized);
        RayToNeighbours((Vector3.down + Vector3.right + new Vector3(0, 0.01f, 0)).normalized);
        RayToNeighbours((Vector3.down + Vector3.back + new Vector3(0, 0.01f, 0)).normalized);
        RayToNeighbours((Vector3.down + Vector3.left + new Vector3(0, 0.01f, 0)).normalized);
        RayToNeighbours((Vector3.down + Vector3.forward + new Vector3(0, 0.01f, 0)).normalized);
    }

    //Check if there is a tile above.
    bool TileAbove()
    {
        return Physics.Raycast(transform.position, transform.up, 1, obstacle);
    }

    public bool IsFree()
    {
        return _isFree;
    }

    public void MakeTileOccupied()
    {
        _isFree = false;
        foreach (var tile in neighbours)
        {
            tile.RemoveNeighbour(this);
        }
    }

    public void MakeTileFree()
    {
        _isFree = true;
        foreach (var tile in neighbours)
        {
            tile.AddNeighbour(this);
        }
    }

    public void RemoveNeighbour(Tile tile)
    {
        if (neighbours.Contains(tile))
            neighbours.Remove(tile);
    }

    public void AddNeighbour(Tile tile)
    {
        GetNeighbours();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
