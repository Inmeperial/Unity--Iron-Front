using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Grenade : Item
{
    private Dictionary<Tile, int> _tilesForAttackChecked = new Dictionary<Tile, int>();
    private HashSet<Tile> _tilesInAttackRange = new HashSet<Tile>();
    private Dictionary<Tile, int> _tilesForSelectionChecked = new Dictionary<Tile, int>();
    private HashSet<Tile> _tilesInSelectionRange = new HashSet<Tile>();
    
    private TileHighlight _highlight;
    private Tile _tile;
    public Grenade(GrenadeSO itemData, Character character, TileHighlight tileHighlight)
    {
        this.itemData = itemData;
        _character = character;
        _highlight = tileHighlight;
        SetItem();
    }
    public override void SelectItem()
    {
        _character.ItemSelectionState(true);
        _tile = null;
        PaintTilesInSelectionRange(_character.GetMyPositionTile(), 0);
    }

    public override void DeselectItem()
    {
        _character.ItemSelectionState(false);
        _highlight.ClearTilesInPreview(_tilesInAttackRange);
        _highlight.ClearTilesInActivationRange(_tilesInSelectionRange);
        _tile = null;
        _tilesForAttackChecked.Clear();
        _tilesInAttackRange.Clear();
        _tilesForSelectionChecked.Clear();
        _tilesInSelectionRange.Clear();
    }

    public override void Use(Action callback = null)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("granada hice click");
            var selection = MouseRay.GetTargetTransform(_character.block);

            if (selection == null)
            {
                Debug.Log("selectoin null");
                return;
            }
            
            if (!IsValidBlock(selection)) return;
                
            Debug.Log(selection.name);

            var newTile = selection.GetComponent<Tile>();
        
            if (!newTile) return;
            Debug.Log("granada tengo tile");
            if (_tile != null && newTile == _tile)
            {
                Debug.Log("granada ataco");    
                Attack();
                if (callback != null)
                    callback();
            }
            else
            {
                _highlight.ClearTilesInPreview(_tilesInAttackRange);
                _tilesForAttackChecked.Clear();
                _tilesInAttackRange.Clear();
                Debug.Log("granada tile seleccionada");
                _tile = newTile;

                PaintAoeTiles(_tile, 0); 
            }

        }
    }
    
    private void PaintAoeTiles(Tile currentTile, int count)
    {
        if (count >= GetItemAoE() || (_tilesForAttackChecked.ContainsKey(currentTile) && _tilesForAttackChecked[currentTile] <= count))
            return;
        
        _tilesForAttackChecked[currentTile] = count;

        foreach (Tile tile in currentTile.allNeighbours)
        {
            if (!_tilesInAttackRange.Contains(tile))
            {
                if (!tile.HasTileAbove() && tile.IsWalkable())
                {
                    _tilesInAttackRange.Add(tile);
                    tile.inAttackRange = true;
                    _highlight.PaintTilesInPreviewRange(tile);
                }

            }
            PaintAoeTiles(tile, count + 1);
        }
    }

    private void PaintTilesInSelectionRange(Tile currentTile, int count)
    {
        if (count >= GetItemRange() || (_tilesForSelectionChecked.ContainsKey(currentTile) && _tilesForSelectionChecked[currentTile] <= count))
            return;
        
        _tilesForSelectionChecked[currentTile] = count;

        foreach (Tile tile in currentTile.allNeighbours)
        {
            if (!_tilesInSelectionRange.Contains(tile))
            {
                if (!tile.HasTileAbove() && tile.IsWalkable())
                {
                    _tilesInSelectionRange.Add(tile);
                    _highlight.MortarPaintTilesInActivationRange(tile);
                }
            }
            PaintTilesInSelectionRange(tile, count + 1);
        }
    }

    private void Attack()
    {
        foreach (var tile in _tilesInAttackRange)
        {
            var unit = tile.GetUnitAbove();
            if (!unit) continue;
            
            unit.body.TakeDamage(GetItemDamage());
            unit.leftArm.TakeDamage(GetItemDamage());
            unit.rightArm.TakeDamage(GetItemDamage());
            unit.legs.TakeDamage(GetItemDamage());
        }
        UpdateUses();
    }
    
    private bool IsValidBlock(Transform target)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return false;

        if (!target) return false;

        if (LayerMask.LayerToName(target.gameObject.layer) != "GridBlock") return false;

        var t = target.GetComponent<Tile>();
        
        return _tilesInSelectionRange.Contains(t);
    }
}
