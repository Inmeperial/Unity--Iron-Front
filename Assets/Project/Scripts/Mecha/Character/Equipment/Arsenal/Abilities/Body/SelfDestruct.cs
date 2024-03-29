﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : Ability
{
    private HashSet<Tile> _tilesInAttackRange = new HashSet<Tile>();
    private Dictionary<Tile, int> _tilesForAttackChecked = new Dictionary<Tile, int>();
    
    private SelfDestructSO _abilityData;

    public override void Initialize(Character character, EquipableSO data)
    {
        base.Initialize(character, data);
	    
        _abilityData = data as SelfDestructSO;
    }
    
    public override void Select()
    {
        OnEquipableSelected?.Invoke();

        _character.DeselectCurrentEquipable();

        _character.DeselectThisUnit();

        _character.EquipableSelectionState(true, this);

        PaintTilesInAttackRange(_character.GetPositionTile(), 0);
    }

    public override void Deselect()
    {
        OnEquipableDeselected?.Invoke();
        if(_tilesInAttackRange.Count != 0)
		{
            TileHighlight.Instance.ClearTilesInPreview(_tilesInAttackRange);
            TileHighlight.Instance.ClearTilesInActivationRange(_tilesInAttackRange);
            TileHighlight.Instance.ClearTilesInAttackRange(_tilesInAttackRange);
            TileHighlight.Instance.MortarClearTilesInAttackRange(_tilesInAttackRange);
        }
        
        _tilesInAttackRange.Clear();

        _tilesForAttackChecked.Clear();

        _character.SelectThisUnit();
    }

    public override void Use()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UseAbility();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Deselect();
        }
    }

    private void UseAbility()
    {
        Transform selectedTile = MouseRay.GetTargetTransform(_character.GetBlockLayerMask());

        if (!selectedTile)
            return;

        Tile mechaTile = selectedTile.GetComponent<Tile>();

        if (!mechaTile)
            return;

        if (!_tilesInAttackRange.Contains(mechaTile))
            return;

        Debug.Log("use self destruct");

        Body body = _character.GetBody();
        body.ReceiveDamage((int)body.MaxHp);

        Legs legs = _character.GetLegs();
        legs.ReceiveDamage((int)legs.MaxHp);

        Gun rightGun = _character.GetRightGun();
        if (rightGun)
            rightGun.ReceiveDamage((int)rightGun.MaxHp);

        Gun leftGun = _character.GetLeftGun();
        if (leftGun)
            leftGun.ReceiveDamage((int)leftGun.MaxHp);

        PlayVFX(_abilityData.particleEffect, _character.GetBody().transform.position, _character.transform.forward);

        PlaySound(_abilityData.sound, gameObject);
        int selfDestructDamage = _abilityData.selfDestructDamage;
        foreach (Tile tile in _tilesInAttackRange)
        {
            Character characterAbove = tile.GetUnitAbove();

            if (characterAbove && characterAbove != _character)
            {
                characterAbove.GetBody().ReceiveDamage(selfDestructDamage);

                characterAbove.GetLegs().ReceiveDamage(selfDestructDamage);

                Gun lGun = characterAbove.GetLeftGun();

                if (lGun)
                    lGun.ReceiveDamage(selfDestructDamage);

                Gun rGun = characterAbove.GetRightGun();

                if (rGun)
                    rGun.ReceiveDamage(selfDestructDamage);
            }

            LandMine mine = tile.GetMineAbove();

            if (mine)
                mine.DestroyMine();
        }

        UpdateButtonText(_availableUses.ToString(), _abilityData);

        _button.interactable = false;

        AbilityUsed(_abilityData);

        OnEquipableUsed?.Invoke();

        Deselect();
    }
    private void PaintTilesInAttackRange(Tile currentTile, int count)
    {
        if (count >= _abilityData.selfDestructRange)
            return;
        
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
                    TileHighlight.Instance.PaintTilesInAttackRange(tile);
                }
            }
            PaintTilesInAttackRange(tile, count + 1);
        }
    }

    public override string GetEquipableName()
    {
        return _abilityData.objectName;
    }

    public override string GetEquipableDescription()
    {
        return _abilityData.objectDescription;
    }
}
