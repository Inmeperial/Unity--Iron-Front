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
    void Update()
    {
        if (previousTile != null)
        {
            MouseExitsTile();
        }

        if (!characterMoving)
            RayToTile();
    }

    //Check if mouse is over a tile.
    void RayToTile()
    {
        var obj = MouseRay.GetTargetTransform(tileMask);
        if (obj && obj.CompareTag("GridBlock"))
        {
            var tile = obj.gameObject.GetComponent<Tile>();
            if (tile.IsWalkable())
            {
                MouseOverTile(tile);
            }
        }
    }

    #region Change tile color methods.
    
    //Pinta los tiles cuando el mouse está encima
    public void MouseOverTile(Tile tile)
    {
        tile.MouseOverColor();
        previousTile = tile.transform;
    }

    
    //Despinta los tiles cuando el mouse no está encima
    public void MouseExitsTile()
    {
        Tile tile = previousTile.gameObject.GetComponent<Tile>();
        tile.EndMouseOverColor();
    }

    //Despinta los tiles de movimiento y deshace las lineas del camino
    public void EndPreview()
    {
        previousTile = null;
        if (_previewPath.Count > 0)
        {
           ClearTilesInMoveRange(_previewPath);
            _previewPath.Clear();
            _lineRenderer.positionCount = 0;
        }
    }
    
    //Pinta los tiles del path
    public void PathPreview(List<Tile> path)
    {
        _previewPath = path;
        for (int i = 0; i < path.Count; i++)
        {
            var tile = path[i];
            if (tile.IsWalkable() && tile.IsFree())
                tile.InMoveRangeColor();
        }
    }

    //Pinta los tiles en rango de ataque
    public void PaintTilesInAttackRange(Tile tile)
    {
        tile.CanBeAttackedColor();
    }

    //Pinta los tiles en rango de movimiento
    public void PaintTilesInMoveRange(Tile tile)
    {
        tile.InMoveRangeColor();
    }

    //Pinta los tiles en rango de ataque y movimiento
    public void PaintTilesInMoveAndAttackRange(Tile tile)
    {
        tile.CanMoveAndAttackColor();
    }

    //Pinta los tiles en la preview de ataque
    public void PaintTilesInPreviewRange(Tile tile)
    {
        tile.InAttackPreviewColor();
    }

    public void AddTilesInMoveRange(List<Tile> tiles)
    {
        _inMoveRangeTiles.Push(tiles);
    }

    //Despinta los tiles en rango de ataque
    public void ClearTilesInAttackRange(HashSet<Tile> tiles)
    {
        foreach (var item in tiles)
        {
            item.EndCanBeAttackedColor();
            item.EndCanMoveAndAttackColor();
        }
    }

    //Despinta los tiles en rango de movimiento
    public void ClearTilesInMoveRange(List<Tile> tiles)
    {
        foreach (var item in tiles)
        {
            item.EndInMoveRangeColor();
            item.EndCanMoveAndAttackColor();
        }
    }

    
    //Despinta los tiles en preview de ataque
    public void ClearTilesInPreview(HashSet<Tile> tiles)
    {
        foreach (var tile in tiles)
        {
            tile.EndAttackPreviewColor();
        }
    }
    #endregion

    public void ChangeActiveCharacter(Character character)
    {
        _character = character;
    }

    public void Undo()
    {
        if (_inMoveRangeTiles.Count > 0)
        {
            var removed = _inMoveRangeTiles.Pop();
            ClearTilesInMoveRange(removed);

            var path = _character.GetPath();

            CreatePathLines(path);
        }
        
    }

    //Crea las lineas del path
    public void CreatePathLines(List<Tile> path)
    {
        _lineRenderer.positionCount = path.Count;
        if (path.Count > 0)
        {
            var v = path[0].transform.position;
            v.y = v.y + path[0].transform.localScale.y;
            _lineRenderer.positionCount = path.Count;
            _lineRenderer.transform.position = v;
            for (int i = 0; i < path.Count; i++)
            {
                v = path[i].transform.position;
                v.y = v.y + path[i].transform.localScale.y;
				_lineRenderer.SetPosition(i, v);
            }
        }
    }

    //Deshace las lineas del path
    public void PathLinesClear()
    {
        _lineRenderer.positionCount = 0;
    }

    
    //Pinta los tiles del rango de activacion del mortero
    public void PaintTilesInActivationRange(HashSet<Tile> tiles)
    {
        foreach (var tile in tiles)
        {
            tile.ActivationRangeColor();
        }
    }

    //Despinta los tiles del rango de activacion del mortero
    public void ClearTilesInActivationRange(HashSet<Tile> tiles)
    {
        foreach (var tile in tiles)
        {
            tile.EndActivationRangeColor();
        }
    }

    public void PaintLastTileInPath(Tile tile)
    {
        tile.LastInPathColor();
    }

    public void EndLastTileInPath(Tile tile)
    {
        tile.EndLastInPathColor();
    }
}
