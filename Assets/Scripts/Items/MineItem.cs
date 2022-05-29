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
		PaintTilesInRange(_character.GetMyPositionTile(), 0);

		_character.DeselectThisUnit();

		_character.EquipableSelectionState(true, this);
	}

	public override void Deselect()
	{
		TileHighlight.Instance.MortarClearTilesInAttackRange(_tilesInRange);
		_tilesInRange.Clear();
		_character.EquipableSelectionState(false, null);
		_character.SelectThisUnit();
	}

	public override void Use(Action callback = null)
	{
		if (Input.GetMouseButtonDown(0))
		{
            ExecuteAbility(callback);
		}

		if (Input.GetMouseButtonDown(1))
			Deselect();
	}

	private void ExecuteAbility(Action callback = null)
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

		ItemUsed();

		UpdateButtonText(_availableUses.ToString(), _data);

		_button.interactable = false;

		callback?.Invoke();

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
}
