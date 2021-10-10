using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Push : Ability
{
    private int _abilityUseRange;
    private int _pushDistance;
    private TileHighlight _highlight;
    //private Dictionary<Tile, int> _tilesInRangeChecked = new Dictionary<Tile, int>();
    private HashSet<Tile> _tilesInRange = new HashSet<Tile>();
    public override void Select()
    {
        Debug.Log("pinto tiles push");
        if (!_highlight)
        {
            _highlight = FindObjectOfType<TileHighlight>();
        }
        _abilityUseRange = _abilityData.pushUseRange;
        _pushDistance = _abilityData.pushDistance;
        PaintUseTiles(_character.GetMyPositionTile(), 0, Vector3.zero);
    }

    public override void Deselect()
    {
        _highlight.ClearTilesInActivationRange(_tilesInRange);
        _tilesInRange.Clear();
    }

    public override void Use(Action callback = null)
    {
        
    }
    
    private void PaintUseTiles(Tile currentTile, int count, Vector3 dir)
    {
        if (count >= _abilityUseRange) //|| (_tilesInRangeChecked.ContainsKey(currentTile) && _tilesInRangeChecked[currentTile] <= count))
            return;

        _tilesInRange.Add(currentTile);
        count++;
        if (dir == Vector3.zero)
        {
            Debug.Log("comienzo");
            RaycastHit forwardHit;
            RaycastHit leftHit;
            RaycastHit rightHit;
            RaycastHit backHit;
    
            var position = currentTile.transform.position;
            Physics.Raycast(position, currentTile.transform.forward, out forwardHit);
            Physics.Raycast(position, currentTile.transform.right * -1, out leftHit);
            Physics.Raycast(position, currentTile.transform.right, out rightHit);
            Physics.Raycast(position, currentTile.transform.forward * -1, out backHit);
    
            if (forwardHit.transform)
            {
                Tile t = forwardHit.transform.GetComponent<Tile>();
    
                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    PaintUseTiles(t, count, forwardHit.transform.forward);                    
                }
            }
            
            if (leftHit.transform)
            {
                Tile t = leftHit.transform.GetComponent<Tile>();
    
                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    PaintUseTiles(t, count, leftHit.transform.right * -1);                    
                }
            }
            
            if (rightHit.transform)
            {
                Tile t = rightHit.transform.GetComponent<Tile>();
    
                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    PaintUseTiles(t, count, rightHit.transform.right);                    
                }
            }
            
            if (backHit.transform)
            {
                Tile t = backHit.transform.GetComponent<Tile>();
    
                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    PaintUseTiles(t, count, backHit.transform.forward * -1);                    
                }
            }
        }

        if (dir == Vector3.forward)
        {
            Debug.Log("forward count: " + count);
            RaycastHit forwardHit;
            
            var position = currentTile.transform.position;
            Physics.Raycast(position, currentTile.transform.forward, out forwardHit);
            if (forwardHit.transform)
            {
                Tile t = forwardHit.transform.GetComponent<Tile>();
    
                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    PaintUseTiles(t, count, forwardHit.transform.forward);                    
                }
            }
            else return;
        }

        if (dir == Vector3.left)
        {
            RaycastHit leftHit;
            
            var position = currentTile.transform.position;
            Physics.Raycast(position, currentTile.transform.right * -1, out leftHit);
            
            if (leftHit.transform)
            {
                Tile t = leftHit.transform.GetComponent<Tile>();
    
                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    PaintUseTiles(t, count, leftHit.transform.right * -1);                    
                }
            }
            else return;
        }
        
        if (dir == Vector3.right)
        {
            RaycastHit rightHit;
            
            var position = currentTile.transform.position;
            Physics.Raycast(position, currentTile.transform.right * -1, out rightHit);
            
            if (rightHit.transform)
            {
                Tile t = rightHit.transform.GetComponent<Tile>();
    
                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    PaintUseTiles(t, count, rightHit.transform.right);                    
                }
            }
            else return;
        }
        
        if (dir == Vector3.back)
        {
            RaycastHit backHit;
            
            var position = currentTile.transform.position;
            Physics.Raycast(position, currentTile.transform.forward * -1, out backHit);
            
            if (backHit.transform)
            {
                Tile t = backHit.transform.GetComponent<Tile>();
    
                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    PaintUseTiles(t, count, backHit.transform.forward * -1);                    
                }
            }
            else return;
        }
        


            // _tilesInRangeChecked[currentTile] = count;
        //
        // foreach (Tile tile in currentTile.allNeighbours)
        // {
        //     if (!_tilesInRange.Contains(tile))
        //     {
        //         if (!tile.HasTileAbove() && tile.IsWalkable())
        //         {
        //             _tilesInRange.Add(tile);
        //             tile.inAttackRange = true;
        //             _highlight.PaintTilesInPreviewRange(tile);
        //         }
        //
        //     }
        //     PaintUseTiles(tile, count + 1);
        // }
    }
}
