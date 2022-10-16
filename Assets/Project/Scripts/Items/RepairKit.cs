using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairKit : Item
{
	RepairKitSO _data;
	HashSet<Tile> _tilesInRange = new HashSet<Tile>();

	public override void Initialize(Character character, EquipableSO data)
	{
		base.Initialize(character, data);

		_data = data as RepairKitSO;
	}

	public override void Select()
	{
		PaintTilesInRange(_character.GetPositionTile(), 0, Vector3.forward);
		PaintTilesInRange(_character.GetPositionTile(), 0, -Vector3.forward);
		PaintTilesInRange(_character.GetPositionTile(), 0, Vector3.right);
		PaintTilesInRange(_character.GetPositionTile(), 0, -Vector3.right);

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
			UseItem(callback);
		}

		if (Input.GetMouseButtonDown(1))
			Deselect();
	}

	private void UseItem(Action callback = null)
    {
		EffectsController.Instance.PlayParticlesEffect(_character.GetPositionTile().gameObject, EnumsClass.ParticleActionType.RepairKit);
        
		Transform selectedTile = MouseRay.GetTargetTransform(_character.GetBlockLayerMask());

		if (!selectedTile)
			return;

        Tile tile = selectedTile.GetComponent<Tile>();

		if (!tile)
			return;

		if (!_tilesInRange.Contains(tile))
			return;

        Character selectedUnit = tile.GetUnitAbove();

		if (!selectedUnit)
			return;

        //Para que solo puedas curar aliados
        EnumsClass.Team unitTeam = selectedUnit.GetUnitTeam();

		//if (unitTeam == EnumsClass.Team.Red) return;
		//Lo cambio por si en algun momento la IA lo usa
		if (unitTeam != _character.GetUnitTeam())
			return;

		RepairUnit(selectedUnit);

		ItemUsed();

		UpdateButtonText(_availableUses.ToString(), _data);

		_button.interactable = false;

		callback?.Invoke();

		Deselect();
	}

	private void RepairUnit(Character unitToRepair)
	{
		int healPercentage = _data.healPercentage;

		Body body = unitToRepair.GetBody();
		
		if(body.CurrentHP > 0)
		{
            float bodyHealAmount = body.MaxHp * healPercentage / 100;
			
			body.Heal((int)bodyHealAmount);
		}

        Gun leftGun = unitToRepair.GetLeftGun();

		if (leftGun)
		{
            float leftGunHealAmount = leftGun.MaxHP * healPercentage / 100;

			leftGun.Heal((int)leftGunHealAmount);
		}

        Gun rightGun = unitToRepair.GetRightGun();
		
		if (rightGun)
		{
            float rightGunHealAmount = rightGun.MaxHP * healPercentage / 100;
			rightGun.Heal((int)rightGunHealAmount);
		}

        Legs legs = unitToRepair.GetLegs();
		
		if (legs.CurrentHP > 0)
		{
            float rightGunHealAmount = legs.MaxHp * healPercentage / 100;

			legs.Heal((int)rightGunHealAmount);
		}
		
	}

	private void PaintTilesInRange(Tile currentTile, int count, Vector3 dir)
	{
		if (!_tilesInRange.Contains(currentTile))
			_tilesInRange.Add(currentTile);

		if (count >= _data.useRange)
			return;

		count++;

        Physics.Raycast(currentTile.transform.position, dir, out RaycastHit hit);
        Tile tile = hit.transform.GetComponent<Tile>();

		if (tile && tile.IsWalkable())
		{
			TileHighlight.Instance.MortarPaintTilesInAttackRange(tile);
			PaintTilesInRange(tile, count, dir);
		}
		return;
	}
}
