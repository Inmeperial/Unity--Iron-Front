using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{
    public bool isWalkable;
    public MeshRenderer render;
    public List<Tile> neighbours;
    public LayerMask obstacle;
    public LayerMask character;
    [SerializeField] private bool _isFree = true;
    public bool painted;
    Material _mat;
    public bool showLineGizmo = true;
    private void Awake()
    {
        _isFree = true;
    }
    private void Start()
    {
        _mat = new Material(render.sharedMaterial);
        if (TileAbove() == false)
            GetNeighbours();
        else MakeNotWalkableColor();
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

    //Check if there is a tile above.
    bool TileAbove()
    {
        return Physics.Raycast(transform.position, transform.up, 1, obstacle);
    }

    bool IsCharacterAbove()
    {
        return Physics.Raycast(transform.position, transform.up, 1, character);
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

    #region Color methods.
    //Make this tile walkable.
    public void MakeWalkableColor()
    {
        isWalkable = true;
        _mat.color = Color.green;

        render.sharedMaterial = _mat;
    }
    //Make this tile not walkable.
    public void MakeNotWalkableColor()
    {
        isWalkable = false;
        _mat.color = Color.red;

        render.sharedMaterial = _mat;
    }

    //Change tile color for pathfinding preview.
    public void PathFindingPreviewColor()
    {
        _mat.color = Color.blue;

        render.sharedMaterial = _mat;
        painted = true;
    }

    //Revert tile color when pathfinding preview ends.
    public void ResetColor()
    {
        if (isWalkable)
            _mat.color = Color.green;
        else _mat.color = Color.red;

        render.sharedMaterial = _mat;
        painted = false;
    }

    public void MouseOverColor()
    {
        if (painted == false)
        {
            _mat.color = Color.yellow;

            render.sharedMaterial = _mat;
        }
        
    }

    public void NotSelectedColor()
    {
        if (painted == false)
        {
            _mat.color = Color.green;

            render.sharedMaterial = _mat;
        }
        
    }

    public void InRangeColor()
    {
        if (painted == false)
        {
            painted = true;
            _mat.color = Color.white;

            render.sharedMaterial = _mat;
        }
        
    }

    public void CanBeAttackedColor()
    {
        painted = true;
        _mat.color = Color.blue;

        render.sharedMaterial = _mat;
    }
    #endregion

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

    public void ShowGizmo()
    {
        showLineGizmo = !showLineGizmo;
    }

    private void OnDrawGizmos()
    {
        if (showLineGizmo)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }
}
