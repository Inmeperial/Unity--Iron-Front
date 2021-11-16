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


    public override void Initialize(Character character, EquipableSO data)
    {
        base.Initialize(character, data);
        _highlight = FindObjectOfType<TileHighlight>();
    }
    public override void Select()
    {
        _character.DeselectThisUnit();
        _character.EquipableSelectionState(true, this);
        _tile = null;
        // _tilesInSelectionRange = new HashSet<Tile>();
        // _tilesForSelectionChecked = new Dictionary<Tile, int>();
        // _tilesInAttackRange = new HashSet<Tile>();
        // _tilesForAttackChecked = new Dictionary<Tile, int>();
        PaintTilesInSelectionRange(_character.GetMyPositionTile(), 0);
       
    }

    public override void Deselect()
    {
        _character.EquipableSelectionState(false, null);
        
        _highlight.ClearTilesInPreview(_tilesInAttackRange);
        _highlight.ClearTilesInActivationRange(_tilesInSelectionRange);
        _tile = null;
        _tilesForAttackChecked.Clear();
        _tilesInAttackRange.Clear();
        _tilesForSelectionChecked.Clear();
        _tilesInSelectionRange.Clear();
        _character.SelectThisUnit();
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
            
            Debug.Log("selection: " + selection.gameObject.name);
            if (!IsValidBlock(selection))
            {
                Debug.Log("no es valid");
                return;
            }
                
            Debug.Log("Selection: " + selection.name);

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
                    Debug.Log("tile: " + tile.name);
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
            
            unit.GetBody().TakeDamage(GetItemDamage());
            
            if (unit.GetLeftGun())
                unit.GetLeftGun().TakeDamage(GetItemDamage());
            
            if (unit.GetRightGun())
                unit.GetRightGun().TakeDamage(GetItemDamage());
            
            unit.GetLegs().TakeDamage(GetItemDamage());
        }
        UpdateUses();
        _button.SetButtonName(_itemData.equipableName + " x" + _availableUses);
        _button.OnRightClick?.Invoke();
        _button.interactable = false;
    }
    
    private bool IsValidBlock(Transform target)
    {
        Debug.Log("if event");
        if (EventSystem.current.IsPointerOverGameObject()) return false;

        Debug.Log("if target");
        if (!target) return false;

        Debug.Log("if layermask");
        if (LayerMask.LayerToName(target.gameObject.layer) != "GridBlock") return false;

        var t = target.GetComponent<Tile>();
        
        Debug.Log("return contains");
        return _tilesInSelectionRange.Contains(t);
    }
}
