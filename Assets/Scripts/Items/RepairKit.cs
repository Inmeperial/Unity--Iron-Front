using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairKit : Item
{
	RepairKitSO _data;
	TileHighlight _highlight;
	HashSet<Tile> _tilesInRange = new HashSet<Tile>();

	public override void Initialize(Character character, EquipableSO data, Location location)
	{
		base.Initialize(character, data, location);
		_data = data as RepairKitSO;
		if(!_highlight)
			_highlight = FindObjectOfType<TileHighlight>();
	}

	public override void Select()
	{
		PaintTilesInRange(_character.GetMyPositionTile(), 0, Vector3.forward);
		PaintTilesInRange(_character.GetMyPositionTile(), 0, -Vector3.forward);
		PaintTilesInRange(_character.GetMyPositionTile(), 0, Vector3.right);
		PaintTilesInRange(_character.GetMyPositionTile(), 0, -Vector3.right);
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
			
			var selectedUnit = selectedTile.GetUnitAbove();
			if (!selectedUnit) return;

			//Para que solo puedas curar aliados
			var unitTeam = selectedUnit.GetUnitTeam();
			if (unitTeam == EnumsClass.Team.Red) return;

			RepairUnit(selectedUnit);

			if (callback != null)
				callback();
			Deselect();
		}

		if (Input.GetMouseButtonDown(1))
			Deselect();
	}

	private void RepairUnit(Character unitToRepair)
	{
		if(unitToRepair.GetBody().GetCurrentHp() > 0)
		{
			//Cantidad de curacion
			var bodyHealAmmount = unitToRepair.GetBody().GetMaxHp() * _data.healPercentage / 100;
			
			//Chequear si la cantidad de curacion excederia la vida maxima, si es el caso sumar hasta llegar a la vida maxima y no pasarse.
			if(unitToRepair.GetBody().GetCurrentHp() + bodyHealAmmount > unitToRepair.GetBody().GetMaxHp())
				bodyHealAmmount = unitToRepair.GetBody().GetMaxHp() - unitToRepair.GetBody().GetCurrentHp();

			//Take Damage con un menos para sumarlo en vez de restarlo
			unitToRepair.GetBody().TakeDamage(-Mathf.RoundToInt(bodyHealAmmount));
		}

		if (unitToRepair.GetLeftGun().GetCurrentHp() > 0)
		{
			//Cantidad de curacion
			var leftGunHealAmmount = unitToRepair.GetLeftGun().GetMaxHp() * _data.healPercentage / 100;

			//Chequear si la cantidad de curacion excederia la vida maxima, si es el caso sumar hasta llegar a la vida maxima y no pasarse.
			if (unitToRepair.GetLeftGun().GetCurrentHp() + leftGunHealAmmount > unitToRepair.GetLeftGun().GetMaxHp())
				leftGunHealAmmount = unitToRepair.GetLeftGun().GetMaxHp() - unitToRepair.GetLeftGun().GetCurrentHp();

			//Take Damage con un menos para sumarlo en vez de restarlo
			unitToRepair.GetLeftGun().TakeDamage(-Mathf.RoundToInt(leftGunHealAmmount));
		}

		if (unitToRepair.GetRightGun().GetCurrentHp() > 0)
		{
			//Cantidad de curacion
			var rightGunHealAmmount = unitToRepair.GetRightGun().GetMaxHp() * _data.healPercentage / 100;

			//Chequear si la cantidad de curacion excederia la vida maxima, si es el caso sumar hasta llegar a la vida maxima y no pasarse.
			if (unitToRepair.GetRightGun().GetCurrentHp() + rightGunHealAmmount > unitToRepair.GetRightGun().GetMaxHp())
				rightGunHealAmmount = unitToRepair.GetRightGun().GetMaxHp() - unitToRepair.GetRightGun().GetCurrentHp();

			//Take Damage con un menos para sumarlo en vez de restarlo
			unitToRepair.GetRightGun().TakeDamage(-Mathf.RoundToInt(rightGunHealAmmount));
		}

		if (unitToRepair.GetLegs().GetCurrentHp() > 0)
		{
			//Cantidad de curacion
			var rightGunHealAmmount = unitToRepair.GetLegs().GetMaxHp() * _data.healPercentage / 100;

			//Chequear si la cantidad de curacion excederia la vida maxima, si es el caso sumar hasta llegar a la vida maxima y no pasarse.
			if (unitToRepair.GetLegs().GetCurrentHp() + rightGunHealAmmount > unitToRepair.GetLegs().GetMaxHp())
				rightGunHealAmmount = unitToRepair.GetLegs().GetMaxHp() - unitToRepair.GetLegs().GetCurrentHp();

			//Take Damage con un menos para sumarlo en vez de restarlo
			unitToRepair.GetLegs().TakeDamage(-Mathf.RoundToInt(rightGunHealAmmount));
		}
		
	}

	private void PaintTilesInRange(Tile currentTile, int count, Vector3 dir)
	{
		if (!_tilesInRange.Contains(currentTile))
			_tilesInRange.Add(currentTile);

		if (count >= _data.useRange)
			return;

		count++;

		RaycastHit hit;
		Physics.Raycast(currentTile.transform.position, dir, out hit);
		Tile t = hit.transform.GetComponent<Tile>();

		if (t && t.IsWalkable())
		{
			_highlight.MortarPaintTilesInActivationRange(t);
			PaintTilesInRange(t, count, dir);
		}
		return;
	}
}
