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
			EffectsController.Instance.PlayParticlesEffect(this.gameObject, EnumsClass.ParticleActionType.RepairKit);
			var selectedTile = MouseRay.GetTargetTransform(_character.block);
		
			if (!selectedTile) return;
		
			var tile = selectedTile.GetComponent<Tile>();

			if (!tile) return;
		
			if (!_tilesInRange.Contains(tile)) return;
			
			var selectedUnit = tile.GetUnitAbove();
			if (!selectedUnit) return;

			//Para que solo puedas curar aliados
			var unitTeam = selectedUnit.GetUnitTeam();
			//if (unitTeam == EnumsClass.Team.Red) return;
			//Lo cambio por si en algun momento la IA lo usa
			if (unitTeam != _character.GetUnitTeam()) return;

			RepairUnit(selectedUnit);

			ItemUsed();
			UpdateButtonText(_availableUses.ToString(), _data);
			_button.interactable = false;
			
			if (callback != null)
				callback();
			Deselect();
		}

		if (Input.GetMouseButtonDown(1))
			Deselect();
	}

	private void RepairUnit(Character unitToRepair)
	{
		var body = unitToRepair.GetBody();
		
		if(body.GetCurrentHp() > 0)
		{
			//Cantidad de curacion
			var bodyHealAmount = body.GetMaxHp() * _data.healPercentage / 100;
			
			body.Heal((int)bodyHealAmount);
			
			//Chequear si la cantidad de curacion excederia la vida maxima, si es el caso sumar hasta llegar a la vida maxima y no pasarse.
			// if(unitToRepair.GetBody().GetCurrentHp() + bodyHealAmmount > unitToRepair.GetBody().GetMaxHp())
			// 	bodyHealAmmount = unitToRepair.GetBody().GetMaxHp() - unitToRepair.GetBody().GetCurrentHp();

			//Take Damage con un menos para sumarlo en vez de restarlo
			//unitToRepair.GetBody().TakeDamage(-Mathf.RoundToInt(bodyHealAmmount));
		}

		var leftGun = unitToRepair.GetLeftGun();
		if (leftGun)
		{
			//Cantidad de curacion
			var leftGunHealAmount = leftGun.GetMaxHp() * _data.healPercentage / 100;

			leftGun.Heal((int)leftGunHealAmount);

			//Chequear si la cantidad de curacion excederia la vida maxima, si es el caso sumar hasta llegar a la vida maxima y no pasarse.
			// if (unitToRepair.GetLeftGun().GetCurrentHp() + leftGunHealAmmount > unitToRepair.GetLeftGun().GetMaxHp())
			// 	leftGunHealAmmount = unitToRepair.GetLeftGun().GetMaxHp() - unitToRepair.GetLeftGun().GetCurrentHp();
			//
			// //Take Damage con un menos para sumarlo en vez de restarlo
			// unitToRepair.GetLeftGun().TakeDamage(-Mathf.RoundToInt(leftGunHealAmmount));
		}

		var rightGun = unitToRepair.GetRightGun();
		
		if (rightGun)
		{
			//Cantidad de curacion
			var rightGunHealAmount = rightGun.GetMaxHp() * _data.healPercentage / 100;
			rightGun.Heal((int)rightGunHealAmount);

			//Chequear si la cantidad de curacion excederia la vida maxima, si es el caso sumar hasta llegar a la vida maxima y no pasarse.
			// if (unitToRepair.GetRightGun().GetCurrentHp() + rightGunHealAmmount > unitToRepair.GetRightGun().GetMaxHp())
			// 	rightGunHealAmmount = unitToRepair.GetRightGun().GetMaxHp() - unitToRepair.GetRightGun().GetCurrentHp();
			//
			// //Take Damage con un menos para sumarlo en vez de restarlo
			// unitToRepair.GetRightGun().TakeDamage(-Mathf.RoundToInt(rightGunHealAmmount));
		}

		var legs = unitToRepair.GetLegs();
		
		if (legs.GetCurrentHp() > 0)
		{
			//Cantidad de curacion
			var rightGunHealAmount = legs.GetMaxHp() * _data.healPercentage / 100;

			legs.Heal((int)rightGunHealAmount);
			
			//Chequear si la cantidad de curacion excederia la vida maxima, si es el caso sumar hasta llegar a la vida maxima y no pasarse.
			// if (unitToRepair.GetLegs().GetCurrentHp() + rightGunHealAmount > unitToRepair.GetLegs().GetMaxHp())
			// 	rightGunHealAmount = unitToRepair.GetLegs().GetMaxHp() - unitToRepair.GetLegs().GetCurrentHp();
			//
			// //Take Damage con un menos para sumarlo en vez de restarlo
			// unitToRepair.GetLegs().TakeDamage(-Mathf.RoundToInt(rightGunHealAmount));
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
