using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TileHighlight : MonoBehaviour
{
    Character _character;
    Transform previousTile;
    public AStarAgent agent;
    public LayerMask tileMask;
    List<Tile> _previewPath = new List<Tile>();
    public bool characterMoving;
    private Stack<List<Tile>> _inMoveRangeTiles = new Stack<List<Tile>>();
    private LineRenderer _lineRenderer;
    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        // if (previousTile)
        // {
        //     MouseExitsTile();
        // }

        // if (!characterMoving)
        //     RayToTile();
    }
    
    // private void RayToTile()
    // {
    //     var obj = MouseRay.GetTargetTransform(tileMask);
    //     
    //     if (!obj || !obj.CompareTag("GridBlock")) return;
    //     
    //     var tile = obj.gameObject.GetComponent<Tile>();
    //     if (tile.IsWalkable())
    //     {
    //         MouseOverTile(tile);
    //     }
    // }

    #region Change tile color methods.
    // //Pinta los tiles cuando el mouse está encima
    // private void MouseOverTile(Tile tile)
    // {
    //     tile.MouseOverColor();
    //     previousTile = tile.transform;
    // }

    
    //Despinta los tiles cuando el mouse no está encima
    // private void MouseExitsTile()
    // {
    //     Tile tile = previousTile.gameObject.GetComponent<Tile>();
    //     tile.EndMouseOverColor();
    // }

    /// <summary>
    /// Unpaint movement tiles and clear path lines.
    /// </summary>
    public void EndPreview()
    {
        previousTile = null;
        if (_previewPath.Count <= 0) return;
        
        ClearTilesInMoveRange(_previewPath);
        _previewPath.Clear();
        _lineRenderer.positionCount = 0;
    }
    
    /// <summary>
    /// Paint the given tile with attack range color.
    /// </summary>
    public void PaintTilesInAttackRange(Tile tile)
    {
        tile.CanBeAttackedColor();
    }

    /// <summary>
    /// Paint the given tile with movement range color.
    /// </summary>
    public void PaintTilesInMoveRange(Tile tile)
    {
        tile.InMoveRangeColor();
    }
    
    /// <summary>
    /// Paint tiles of the given path with movement range color.
    /// </summary>
    public void PathPreview(List<Tile> path)
    {
        _previewPath = path;
        foreach (var tile in path)
        {
            if (tile.IsWalkable() && tile.IsUnitFree())
                tile.InMoveRangeColor();
        }
    }
    
    /// <summary>
    /// Paint the given tile with movement and attack range color.
    /// </summary>
    public void PaintTilesInMoveAndAttackRange(Tile tile)
    {
        tile.CanMoveAndAttackColor();
    }
    
    /// <summary>
    /// Paint the given tile with attack preview range color.
    /// </summary>
    public void PaintTilesInPreviewRange(Tile tile)
    {
        tile.InAttackPreviewColor();
    }

    public void PaintTilesInActivationRageMorter(Tile tile)
    {
        tile.ActivationRangeMorter();
    }

    public void AddTilesInMoveRange(List<Tile> tiles)
    {
        _inMoveRangeTiles.Push(tiles);
    }
    
    /// <summary>
    /// Clear attack range color of the given tiles .
    /// </summary>
    public void ClearTilesInAttackRange(HashSet<Tile> tiles)
    {
        foreach (var item in tiles)
        {
            item.EndCanBeAttackedColor();
            item.EndCanMoveAndAttackColor();
        }
    }

    /// <summary>
    /// Clear movement range color of the given tiles .
    /// </summary>
    public void ClearTilesInMoveRange(List<Tile> tiles)
    {
        foreach (var item in tiles)
        {
            item.EndInMoveRangeColor();
            item.EndCanMoveAndAttackColor();
        }
    }

    
    /// <summary>
    /// Clear attack preview range color of the given tiles .
    /// </summary>
    public void ClearTilesInPreview(HashSet<Tile> tiles)
    {
        foreach (var tile in tiles)
        {
            tile.EndAttackPreviewColor();
        }
    }
    
    /// <summary>
    /// Paint given tiles with activation range color for mortar.
    /// </summary>
    public void PaintTilesInActivationRange(Tile tile)
    {
        tile.ActivationRangeColor();
    }

    /// <summary>
    /// Clear activation range color of the given tiles.
    /// </summary>
    public void ClearTilesInActivationRange(HashSet<Tile> tiles)
    {
        foreach (var tile in tiles)
        {
            tile.EndActivationRangeColor();
        }
    }

    /// <summary>
    /// Paint the given tile with last tile in path color.
    /// </summary>
    public void PaintLastTileInPath(Tile tile)
    {
        tile.LastInPathColor();
    }

    /// <summary>
    /// Clear last tile in path color of the given tile.
    /// </summary>
    public void EndLastTileInPath(Tile tile)
    {
        tile.EndLastInPathColor();
    }
    #endregion
    
    public void ChangeActiveCharacter(Character character)
    {
        _character = character;
    }

    public void Undo()
    {
        if (_inMoveRangeTiles.Count <= 0) return;
        
        var removed = _inMoveRangeTiles.Pop();
        ClearTilesInMoveRange(removed);

        var path = _character.GetPath();

        CreatePathLines(path);

    }

    /// <summary>
    /// Create lines to show movement path.
    /// </summary>
    public void CreatePathLines(List<Tile> path)
    {
        _lineRenderer.positionCount = path.Count;
        
        if (path.Count <= 0) return;
        
        var v = path[0].transform.position;
        v.y += path[0].transform.localScale.y;
        _lineRenderer.positionCount = path.Count;
        _lineRenderer.transform.position = v;
        
        for (int i = 0; i < path.Count; i++)
        {
            v = path[i].transform.position;
            v.y += path[i].transform.localScale.y;
            _lineRenderer.SetPosition(i, v);
        }
    }

    /// <summary>
    /// Clear all path lines.
    /// </summary>
    public void PathLinesClear()
    {
        _lineRenderer.positionCount = 0;
    }

    

}
