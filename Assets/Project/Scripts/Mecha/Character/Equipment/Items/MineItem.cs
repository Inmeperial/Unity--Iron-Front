using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineItem : Item
{
	MineItemSO _data;
	HashSet<Tile> _tilesInRange = new HashSet<Tile>();

	public override void Initialize(Character character, EquipableSO data)
	{
		base.Initialize(character, data);

		_data = data as MineItemSO;
	}

	public override void Select()
	{
		OnEquipableSelected?.Invoke();

        _character.DeselectCurrentEquipable();

        PaintTilesInRange(_character.GetPositionTile(), 0);

		_character.DeselectThisUnit();

		_character.EquipableSelectionState(true, this);
	}

	public override void Deselect()
	{
		OnEquipableDeselected?.Invoke();

		TileHighlight.Instance.MortarClearTilesInAttackRange(_tilesInRange);
		_tilesInRange.Clear();
		_character.EquipableSelectionState(false, null);
		_character.SelectThisUnit();
	}

	public override void Use()
	{
		if (Input.GetMouseButtonDown(0))
		{
            ExecuteAbility();
		}

		if (Input.GetMouseButtonDown(1))
			Deselect();
	}

	private void ExecuteAbility()
    {
		Transform selectedTile = MouseRay.GetTargetTransform(_character.GetBlockLayerMask());

		if (!selectedTile)
			return;

		Tile tile = selectedTile.GetComponent<Tile>();

		if (!tile)
			return;

		if (!_tilesInRange.Contains(tile))
			return;

		Instantiate(_data.minePefab, selectedTile.transform.position + Vector3.up * 2, Quaternion.Euler(-90, 0, 0));
		EffectsController.Instance.PlayParticlesEffect(_data.particleEffect, selectedTile.transform.position + Vector3.up * 2, Vector3.forward);

		ItemUsed();

		UpdateButtonText(_availableUses.ToString(), _data);

		_button.interactable = false;

		OnEquipableUsed?.Invoke();

		Deselect();
	}

	private void PaintTilesInRange(Tile currentTile, int count)
	{
		if (count >= _data.useRange)
			return;

		foreach (var item in currentTile.allNeighbours)
		{
			if (!_tilesInRange.Contains(item))
			{
				if (item && item.IsWalkable() && !item.GetUnitAbove())
				{
					_tilesInRange.Add(item);
					TileHighlight.Instance.MortarPaintTilesInAttackRange(item);
				}
			}
			PaintTilesInRange(item, count + 1);
		}
	}

    public override string GetEquipableName()
    {
        return _data.objectName;
    }

    public override string GetEquipableDescription()
    {
        return _data.objectDescription;
    }
}
