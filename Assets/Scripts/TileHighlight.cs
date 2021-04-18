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
    CharacterSelection _charSelector;
    private Stack<List<Tile>> _inMoveRangeTiles = new Stack<List<Tile>>();
    private LineRenderer _lineRenderer;
    private void Start()
    {
        _charSelector = GetComponent<CharacterSelection>();
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
        if (obj != null && obj.CompareTag("GridBlock"))
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
        //if (_character != null && _character.IsSelected() && !characterMoving)
        //{
        //    PathPreview(_charSelector.GetActualChar());
        //}
    }

    public void MouseExitsTile()
    {
        Tile tile = previousTile.gameObject.GetComponent<Tile>();
        tile.NotSelectedColor();
    }

    public void EndPreview()
    {
        previousTile = null;
        if (_previewPath.Count > 0)
        {
            foreach (var item in _previewPath)
            {
                item.ResetColor();
            }
            _previewPath.Clear();
        }
    }
    public void PathPreview(List<Tile> path)
    {
        _previewPath = path;
        for (int i = 0; i < path.Count; i++)
        {
            var tile = path[i];
            if (tile.IsWalkable() && tile.IsFree())
                tile.PathFindingPreviewColor();
        }
    }

    public void PaintTilesInAttackRange(Tile tile)
    {
        tile.CanBeAttackedColor();
    }

    public void PaintTilesInMoveRange(Tile tile)
    {
        tile.InRangeColor();
    }

    public void AddTilesInMoveRange(List<Tile> tiles)
    {
        _inMoveRangeTiles.Push(tiles);
    }

    public void ClearTilesInRange(List<Tile> tiles)
    {
        foreach (var item in tiles)
        {
            item.ResetColor();
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
            foreach (var tile in removed)
            {
                tile.ResetColor();
            }

            var path = _character.GetPath();

            CreatePathLines(path);
        }
        
    }

    public void CreatePathLines(List<Tile> path)
    {
        Debug.Log("path size: " + path.Count);
        _lineRenderer.positionCount = path.Count;
        if (path.Count > 0)
        {
            var v = path[0].transform.position;
            v.y = v.y + 1;
            _lineRenderer.positionCount = path.Count;
            _lineRenderer.transform.position = v;
            for (int i = 0; i < path.Count; i++)
            {
                v = path[i].transform.position;
                v.y = v.y + 1;
                _lineRenderer.SetPosition(i, v);
            }
        }
    }
}
