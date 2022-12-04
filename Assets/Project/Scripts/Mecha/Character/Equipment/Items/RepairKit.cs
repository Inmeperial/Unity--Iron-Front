using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairKit : Item
{
	RepairKitSO _data;
	HashSet<Tile> _tilesInRange = new HashSet<Tile>();

	private bool _characterSelectionState = false;

	public override void Initialize(Character character, EquipableSO data)
	{
		base.Initialize(character, data);

		_data = data as RepairKitSO;
	}

	public override void Select()
	{
		OnEquipableSelected?.Invoke();
		PaintTilesInRange(_character.GetPositionTile(), 0, Vector3.forward);
		PaintTilesInRange(_character.GetPositionTile(), 0, -Vector3.forward);
		PaintTilesInRange(_character.GetPositionTile(), 0, Vector3.right);
		PaintTilesInRange(_character.GetPositionTile(), 0, -Vector3.right);

		_character.DeselectThisUnit();

		_character.EquipableSelectionState(true, this);

		_characterSelectionState = CharacterSelector.Instance.IsSelectionEnabled();

		CharacterSelector.Instance.DisableCharacterSelection();
	}
	
	public override void Deselect()
	{
		OnEquipableDeselected?.Invoke();

		if (_characterSelectionState)
			CharacterSelector.Instance.EnableCharacterSelection();

		_characterSelectionState = false;

		TileHighlight.Instance.MortarClearTilesInAttackRange(_tilesInRange);

		_tilesInRange.Clear();
		_character.EquipableSelectionState(false, null);
		_character.SelectThisUnit();
	}

	public override void Use()
	{
		if (Input.GetMouseButtonDown(0))
		{
			UseItem();
		}

		if (Input.GetMouseButtonDown(1))
			Deselect();
	}

	private void UseItem()
    {	
		Transform selectedTile = MouseRay.GetTargetTransform(_character.GetBlockLayerMask());

		if (!selectedTile)
			return;

        Tile tile = selectedTile.GetComponent<Tile>();

		if (!tile)
			return;

		if (tile == _character.GetPositionTile())
			Debug.Log("mismo tile");

		if (!_tilesInRange.Contains(tile))
			return;

        Character selectedUnit = tile.GetUnitAbove();

        if (!selectedUnit && _character.GetPositionTile() != tile)
            return;

        //Para que solo puedas curar aliados
        EnumsClass.Team unitTeam = selectedUnit.GetUnitTeam();

		//if (unitTeam == EnumsClass.Team.Red) return;
		//Lo cambio por si en algun momento la IA lo usa
		if (unitTeam != _character.GetUnitTeam())
			return;

        EffectsController.Instance.PlayParticlesEffect(_data.particleEffect, selectedUnit.transform.position, selectedUnit.transform.forward);

        AudioManager.Instance.PlaySound(_data.sound, _character.gameObject);

        RepairUnit(selectedUnit);

		ItemUsed();

		UpdateButtonText(_availableUses.ToString(), _data);

		_button.interactable = false;

		OnEquipableUsed?.Invoke();

		Deselect();
	}

	private void RepairUnit(Character unitToRepair)
	{
		int healPercentage = _data.healPercentage;

		Body body = unitToRepair.GetBody();

		HealPart(body);

        Gun leftGun = unitToRepair.GetLeftGun();

		HealPart(leftGun);

        Gun rightGun = unitToRepair.GetRightGun();
		
		HealPart(rightGun);

        Legs legs = unitToRepair.GetLegs();

		HealPart(legs);	
	}

	private void HealPart(MechaPart part)
	{
		if (part.CurrentHP <= 0)
			return;

		if (part.CurrentHP == part.MaxHp)
			return;

        float healAmount = part.MaxHp * _data.healPercentage / 100;

		if (part.CurrentHP + healAmount >= part.MaxHp)
			healAmount = part.MaxHp - part.CurrentHP;

		part.Heal((int)healAmount);
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

    public override string GetEquipableName()
    {
        return _data.objectName;
    }

    public override string GetEquipableDescription()
    {
        return _data.objectDescription;
    }
}
