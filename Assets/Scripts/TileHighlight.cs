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
    private List<Tile> _inRangeTiles = new List<Tile>();

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
            if (tile.isWalkable)
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
            if (tile.isWalkable && tile.IsFree())
                tile.PathFindingPreviewColor();
        }
        

        //if (_previewPath != null && _previewPath.Count > 0)
        //{
        //    for (int i = 1; i < _previewPath.Count; i++)
        //    {
        //        var tile = _previewPath[i];
        //        tile.PathFindingPreviewColor();
        //    }
        //}
        //if (character.ThisUnitCanMove())
        //{
        //    var start = character.ActualPosition();
        //    if (start)
        //    {
        //        agent.init = start;
        //        agent.finit = previousTile.GetComponent<Tile>();
        //        _previewPath = agent.PathFindingAstar();
        //        if (_previewPath.Count > 0)
        //        {
        //            if (_previewPath.Count <= _characterMoveRadius)
        //            {
        //                for (int i = 0; i < _previewPath.Count; i++)
        //                {
        //                    if (_previewPath[i].isWalkable && _previewPath[i].IsFree())
        //                        _previewPath[i].PathFindingPreviewColor();
        //                }
        //            }
        //            else
        //            {
        //                for (int i = 0; i <= _characterMoveRadius; i++)
        //                {
        //                    if (_previewPath[i].isWalkable && _previewPath[i].IsFree())
        //                        _previewPath[i].PathFindingPreviewColor();
        //                }
        //            }
        //        }
        //    }
        //}
    }

    public void PaintTilesInAttackRange(Tile tile)
    {
        tile.InRangeColor();
    }

    public void ClearTilesInAttackRange(List<Tile> tiles)
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
}
