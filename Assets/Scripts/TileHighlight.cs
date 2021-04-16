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

    private void Start()
    {
        _charSelector = GetComponent<CharacterSelection>();
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
        Debug.Log("stack size: " + _inMoveRangeTiles.Count);
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
        var removed = _inMoveRangeTiles.Pop();
        Debug.Log("stack size: " + _inMoveRangeTiles.Count);
        foreach (var tile in removed)
        {
            Debug.Log("Reset color");
            tile.ResetColor();
        }
    }
}
