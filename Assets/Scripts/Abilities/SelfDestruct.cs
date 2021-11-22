using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : Ability
{
    private HashSet<Tile> _tilesInAttackRange = new HashSet<Tile>();
    private Dictionary<Tile, int> _tilesForAttackChecked = new Dictionary<Tile, int>();
    private TileHighlight _highlight;
    public override void Select()
    {
        _character.DeselectThisUnit();
        _character.EquipableSelectionState(true, this);
        _highlight = _character.highlight;
        PaintTilesInAttackRange(_character.GetMyPositionTile(), 0);
    }

    public override void Deselect()
    {
        _highlight.ClearTilesInPreview(_tilesInAttackRange);
        _highlight.ClearTilesInActivationRange(_tilesInAttackRange);
        _highlight.ClearTilesInAttackRange(_tilesInAttackRange);
        _highlight.MortarClearTilesInAttackRange(_tilesInAttackRange);
        _tilesInAttackRange.Clear();
        _tilesForAttackChecked.Clear();
        _character.SelectThisUnit();
    }

    public override void Use(Action callback = null)
    {
        if (Input.GetMouseButtonDown(0))
        {
            var selectedTile = MouseRay.GetTargetTransform(_character.block).GetComponent<Tile>();
            if (!selectedTile || !_tilesInAttackRange.Contains(selectedTile)) return;
            Debug.Log("use self destruct");

            _character.GetBody().TakeDamage((int)_character.GetBody().GetMaxHp());
            _character.GetLegs().TakeDamage((int)_character.GetLegs().GetMaxHp());

            var myRGun = _character.GetRightGun();
            if (myRGun) myRGun.TakeDamage((int)myRGun.GetMaxHp());
            
            var myLGun = _character.GetLeftGun();
            if (myLGun) myLGun.TakeDamage((int)myLGun.GetMaxHp());
            
            foreach (var tile in _tilesInAttackRange)
            {
                var characterAbove = tile.GetCharacterAbove();

                if (characterAbove && characterAbove != _character)
                {
                    characterAbove.GetBody().TakeDamage(_abilityData.selfDestructDamage);
                    
                    characterAbove.GetLegs().TakeDamage(_abilityData.selfDestructDamage);

                    var lGun = characterAbove.GetLeftGun();
                    
                    if (lGun) lGun.TakeDamage(_abilityData.selfDestructDamage);

                    var rGun = characterAbove.GetRightGun();
                    
                    if (rGun) rGun.TakeDamage(_abilityData.selfDestructDamage);
                }

                var mine = tile.GetMineAbove();
                
                if (mine) mine.DestroyMine();
            }
            Deselect();
        }
    }

    private void PaintTilesInAttackRange(Tile currentTile, int count)
    {
        if (count >= _abilityData.selfDestructDamageRange) return;
        
        if ((_tilesForAttackChecked.ContainsKey(currentTile) && _tilesForAttackChecked[currentTile] <= count))
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
                    _highlight.PaintTilesInAttackRange(tile);
                }
            }
            PaintTilesInAttackRange(tile, count + 1);
        }
    }
}
