using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Grenade : Item
{
    private Dictionary<Tile, int> _tilesForAttackChecked = new Dictionary<Tile, int>();
    private HashSet<Tile> _tilesInAttackRange = new HashSet<Tile>();
    private Dictionary<Tile, int> _tilesForSelectionChecked = new Dictionary<Tile, int>();
    private HashSet<Tile> _tilesInSelectionRange = new HashSet<Tile>();
    
    private Tile _tile;

    protected GrenadeSO _itemData;

    public override void Initialize(Character character, EquipableSO data)
    {
        base.Initialize(character, data);

        _itemData = data as GrenadeSO;
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

        PaintTilesInSelectionRange(_character.GetPositionTile(), 0);
       
    }

    public override void Deselect()
    {
        _character.EquipableSelectionState(false, null);
        
        TileHighlight.Instance.ClearTilesInPreview(_tilesInAttackRange);

        TileHighlight.Instance.ClearTilesInActivationRange(_tilesInSelectionRange);

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
            UseItem(callback);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Deselect();
        }
    }

    private void UseItem(Action callback = null)
    {
        Transform selection = MouseRay.GetTargetTransform(_character.GetBlockLayerMask());

        if (!selection)
            return;

        if (!IsValidBlock(selection))
            return;

        Tile newTile = selection.GetComponent<Tile>();

        if (!newTile)
            return;

        if (_tile != null && newTile == _tile)
        {
            Attack();
            callback?.Invoke();
        }
        else
        {
            TileHighlight.Instance.ClearTilesInPreview(_tilesInAttackRange);

            _tilesForAttackChecked.Clear();

            _tilesInAttackRange.Clear();

            _tile = newTile;

            PaintAoeTiles(_tile, 0);
        }
    }
    
    private void PaintAoeTiles(Tile currentTile, int count)
    {
        if (count >= _itemData.areaOfEffect || (_tilesForAttackChecked.ContainsKey(currentTile) && _tilesForAttackChecked[currentTile] <= count))
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
                    TileHighlight.Instance.PaintTilesInPreviewRange(tile);
                }

            }
            PaintAoeTiles(tile, count + 1);
        }
    }

    private void PaintTilesInSelectionRange(Tile currentTile, int count)
    {
        if (count >= _itemData.useRange || (_tilesForSelectionChecked.ContainsKey(currentTile) && _tilesForSelectionChecked[currentTile] <= count))
            return;
        
        _tilesForSelectionChecked[currentTile] = count;

        foreach (Tile tile in currentTile.allNeighbours)
        {
            if (!_tilesInSelectionRange.Contains(tile))
            {
                if (!tile.HasTileAbove() && tile.IsWalkable())
                {
                    _tilesInSelectionRange.Add(tile);
                    TileHighlight.Instance.MortarPaintTilesInActivationRange(tile);
                }
            }
            PaintTilesInSelectionRange(tile, count + 1);
        }
    }

    private void Attack()
    {
        foreach (Tile tile in _tilesInAttackRange)
        {
            Character unit = tile.GetUnitAbove();

            if (!unit) 
                continue;

            Body body = unit.GetBody();
            body.ReceiveDamage(_itemData.damage);

            Gun leftGun = unit.GetLeftGun();
            if (leftGun)
                leftGun.ReceiveDamage(_itemData.damage);
            
            Gun rightGun = unit.GetRightGun();
            if (rightGun)
                rightGun.ReceiveDamage(_itemData.damage);
            
            Legs legs = unit.GetLegs();
            legs.ReceiveDamage(_itemData.damage);

            EffectsController.Instance.PlayParticlesEffect(tile.gameObject, EnumsClass.ParticleActionType.HandGranade);
        }
        ItemUsed();

        UpdateButtonText(_availableUses.ToString(), _itemData);

        _button.interactable = false;

        Deselect();
        
    }
    
    private bool IsValidBlock(Transform target)
    {
        if (EventSystem.current.IsPointerOverGameObject()) 
            return false;
        
        if (!target) 
            return false;
        
        if (LayerMask.LayerToName(target.gameObject.layer) != "GridBlock")
            return false;

        Tile tile = target.GetComponent<Tile>();
        
        return _tilesInSelectionRange.Contains(tile);
    }
}
