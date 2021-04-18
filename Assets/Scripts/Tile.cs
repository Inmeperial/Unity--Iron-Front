using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{
    [SerializeField] private bool isWalkable;
    public MeshRenderer render;
    public List<Tile> neighboursForMove = new List<Tile>();
    public List<Tile> allNeighbours = new List<Tile>();
    public LayerMask obstacle;
    public LayerMask character;
    [SerializeField] private bool _isFree = true;
    public bool painted;
    Material _mat;
    public bool showLineGizmo = true;

    public bool _hasTileAbove;
    private void Awake()
    {
        _isFree = true;
        TileAbove();
    }
    private void Start()
    {
        _mat = new Material(render.sharedMaterial);
        if (HasTileAbove() == false)
            GetNeighbours();
        else MakeNotWalkableColor();
    }

    void GetNeighbours()
    {
        RayForMoveNeighbours(Vector3.right);
        RayForMoveNeighbours(Vector3.left);
        RayForMoveNeighbours(Vector3.forward);
        RayForMoveNeighbours(Vector3.back);
        RayForMoveNeighbours((Vector3.up + Vector3.right + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.up + Vector3.back + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.up + Vector3.left + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.up + Vector3.forward + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.down + Vector3.right + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.down + Vector3.back + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.down + Vector3.left + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.down + Vector3.forward + new Vector3(0, 0.01f, 0)).normalized);
        RayToAllNeighbours(Vector3.right);
        RayToAllNeighbours(Vector3.left);
        RayToAllNeighbours(Vector3.forward);
        RayToAllNeighbours(Vector3.back);
        RayToAllNeighbours((Vector3.up + Vector3.right + new Vector3(0, 0.01f, 0)).normalized);
        RayToAllNeighbours((Vector3.up + Vector3.back + new Vector3(0, 0.01f, 0)).normalized);
        RayToAllNeighbours((Vector3.up + Vector3.left + new Vector3(0, 0.01f, 0)).normalized);
        RayToAllNeighbours((Vector3.up + Vector3.forward + new Vector3(0, 0.01f, 0)).normalized);
        RayToAllNeighbours((Vector3.down + Vector3.right + new Vector3(0, 0.01f, 0)).normalized);
        RayToAllNeighbours((Vector3.down + Vector3.back + new Vector3(0, 0.01f, 0)).normalized);
        RayToAllNeighbours((Vector3.down + Vector3.left + new Vector3(0, 0.01f, 0)).normalized);
        RayToAllNeighbours((Vector3.down + Vector3.forward + new Vector3(0, 0.01f, 0)).normalized);
    }

    //Cast a ray to neighbouring tiles and check if they are walkable.
    void RayForMoveNeighbours(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, 1))
        {
            var neighbour = hit.collider.GetComponent<Tile>();
            if (neighbour != null && neighbour.isWalkable)
                if (!neighboursForMove.Contains(neighbour))
                    neighboursForMove.Add(neighbour);
        }
    }

    void RayToAllNeighbours(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, 1))
        {
            var neighbour = hit.collider.GetComponent<Tile>();
            if (neighbour != null)
                if (!allNeighbours.Contains(neighbour) && !_hasTileAbove)
                    allNeighbours.Add(neighbour);
        }
    }

    //Check if there is a tile above.
    bool TileAbove()
    {
        return _hasTileAbove = Physics.Raycast(transform.position, transform.up, 1, obstacle);
    }

    public bool HasTileAbove()
    {
        return _hasTileAbove;
    }

    bool IsCharacterAbove()
    {
        return Physics.Raycast(transform.position, transform.up, 1, character);
    }

    public void RemoveMoveNeighbour(Tile tile)
    {
        if (neighboursForMove.Contains(tile))
            neighboursForMove.Remove(tile);
    }

    public void AddMoveNeighbour(Tile tile)
    {
        neighboursForMove.Add(tile);
    }

    #region Color methods.
    //Make this tile walkable.
    public void MakeWalkableColor()
    {
        isWalkable = true;
        var mat = new Material(render.sharedMaterial);
        mat.color = Color.green;

        render.sharedMaterial = mat;
    }
    //Make this tile not walkable.
    public void MakeNotWalkableColor()
    {
        isWalkable = false;
        var mat = new Material(render.sharedMaterial);
        mat.color = Color.red;

        render.sharedMaterial = mat;
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

    public bool IsWalkable()
    {
        return isWalkable;
    }

    public void MakeTileOccupied()
    {
        _isFree = false;
        foreach (var tile in neighboursForMove)
        {
            tile.RemoveMoveNeighbour(this);
        }
    }

    public void MakeTileFree()
    {
        _isFree = true;
        foreach (var tile in neighboursForMove)
        {
            tile.AddMoveNeighbour(this);
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
