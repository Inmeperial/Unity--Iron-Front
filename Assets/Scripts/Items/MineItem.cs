using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineItem : Item
{
	MineItemSO _data;
	TileHighlight _highlight;
	HashSet<Tile> _tilesInRange = new HashSet<Tile>();

	public override void Initialize(Character character, EquipableSO data, Location location)
	{
		base.Initialize(character, data, location);
		_data = data as MineItemSO;
		if (!_highlight)
			_highlight = FindObjectOfType<TileHighlight>();
	}

	public override void Select()
	{
		PaintTilesInRange(_character.GetMyPositionTile(), 0);
		_character.DeselectThisUnit();
		_character.EquipableSelectionState(true, this);
	}

	public override void Deselect()
	{
		_highlight.ClearTilesInActivationRange(_tilesInRange);
		_tilesInRange.Clear();
		_character.EquipableSelectionState(false, null);
		_character.SelectThisUnit();
	}

	public override void Use(Action callback = null)
	{
		if (Input.GetMouseButtonDown(0))
		{
			var selectedTile = MouseRay.GetTargetTransform(_character.block).GetComponent<Tile>();
			if (!selectedTile || !_tilesInRange.Contains(selectedTile)) return;
			Instantiate(_data.minePefab, selectedTile.transform.position + Vector3.up, Quaternion.identity);

			if (callback != null)
				callback();
			Deselect();
		}

		if (Input.GetMouseButtonDown(1))
			Deselect();
	}

	private void PaintTilesInRange(Tile currentTile, int count)
	{
		if (count >= _data.useRange) return;

		foreach (var item in currentTile.allNeighbours)
		{
			if (!_tilesInRange.Contains(item))
			{
				if (item && item.IsWalkable() && !item.GetUnitAbove())
				{
					_tilesInRange.Add(item);
					_highlight.MortarPaintTilesInActivationRange(item);
				}
			}
			PaintTilesInRange(item, count + 1);
		}
	}
}
