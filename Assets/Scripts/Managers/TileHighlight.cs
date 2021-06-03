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
    public void MouseOverTile(Tile tile)
    {
        tile.MouseOverColor();
        previousTile = tile.transform;
    }

    public void MouseExitsTile()
    {
        Tile tile = previousTile.gameObject.GetComponent<Tile>();
        tile.EndMouseOverColor();
    }

    public void EndPreview()
    {
        previousTile = null;
        if (_previewPath.Count > 0)
        {
            foreach (var item in _previewPath)
            {
                item.EndInMoveRangeColor();
            }
            _previewPath.Clear();
            _lineRenderer.positionCount = 0;
        }
    }
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

    public void PaintTilesInAttackRange(Tile tile)
    {
        tile.CanBeAttackedColor();
    }

    public void PaintTilesInMoveRange(Tile tile)
    {
        tile.InMoveRangeColor();
    }

    public void PaintTilesInMoveAndAttackRange(Tile tile)
    {
        tile.CanMoveAndAttackColor();
    }

    public void PaintTilesInPreviewRange(Tile tile)
    {
        tile.InAttackPreviewColor();
    }

    public void AddTilesInMoveRange(List<Tile> tiles)
    {
        _inMoveRangeTiles.Push(tiles);
    }

    public void ClearTilesInAttackRange(HashSet<Tile> tiles)
    {
        foreach (var item in tiles)
        {
            item.EndCanBeAttackedColor();
            item.EndCanMoveAndAttackColor();
        }
    }

    public void ClearTilesInMoveRange(List<Tile> tiles)
    {
        foreach (var item in tiles)
        {
            item.EndInMoveRangeColor();
            item.EndCanMoveAndAttackColor();
        }
    }

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

    public void PathLinesClear()
    {
        _lineRenderer.positionCount = 0;
    }
}
