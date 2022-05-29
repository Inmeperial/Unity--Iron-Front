using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CripplingShot : WeaponAbility
{
	private HashSet<Tile> _tilesInRange = new HashSet<Tile>();
	private EnemyCharacter _enemy;
	
	private CripplingShotSO _abilityData;

	public override void Initialize(Character character, EquipableSO data)
	{
		base.Initialize(character, data);
	    
		_abilityData = data as CripplingShotSO;
	}
	
	public override void Select()
	{
		if (_inCooldown || !_character.CanAttack())
			return;

		if (!_gun)
			return;

		PaintTilesInRange(_character.GetMyPositionTile(), 0);

		_character.EquipableSelectionState(true, this);
		_character.DeselectThisUnit();
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
			//Selecionar al enemigo que esta en rango como si lo estás atacando normalmente.
			_enemy = MouseRay.GetTargetTransform(LayerMask.NameToLayer("Character")).GetComponent<EnemyCharacter>();

			//Agregar lo de la rotación y el rayo a las piernas

			if (!_enemy)
				return;

			ExecuteAbility(callback);
		}

		if (Input.GetMouseButtonDown(1))
			Deselect();
	}

	private void ExecuteAbility(Action callback = null)
    {
		_character.RotateTowardsEnemy(_enemy.transform);

		//Vector3 dir = _enemy.GetLegsPosition() - _character.transform.position;

		//Si puede ver las piernas, le dispara, hace daño y evita que se mueva el proximo turno
		if (_character.RayToPartsForAttack(_enemy.GetLegsPosition(), "Legs", true))
		{
			Legs legs = _enemy.GetLegs();
			legs.TakeDamage(_abilityData.damage);

			_enemy.MovementReduction(_abilityData.stepsReduction);

			_character.DeactivateAttack();

			AbilityUsed(_abilityData);

			UpdateButtonText(_availableUses.ToString(), _abilityData);

			_button.interactable = false;

			Deselect();
		}

        callback?.Invoke();
    }

	/*private void OnMouseOver()
	{
		if (Cursor.lockState == CursorLockMode.Locked) return;

		if (EventSystem.current.IsPointerOverGameObject()) return;

		if (!_selectedForAttack && _canBeSelected)
			ShowWorldUI();

		if (_canBeAttacked && !_selectedForAttack)
		{
			RotateWithRays();
		}
	}

	private void RotateWithRays()
	{
		Character c = CharacterSelection.Instance.GetSelectedCharacter();
		if (!c._selected) return;

		if (c.IsMoving()) return;

		if (c._rotated) return;
		c._rotated = true;
		c.SetInitialRotation(c.transform.rotation);
		var posToLook = transform.position;
		posToLook.y = c.transform.position.y;
		c.transform.LookAt(posToLook);

		bool body = c.RayToPartsForAttack(GetBodyPosition(), "Body", true) && _body.GetCurrentHp() > 0;
		bool lArm = c.RayToPartsForAttack(GetLArmPosition(), "LGun", true) && _leftGun;
		bool rArm = c.RayToPartsForAttack(GetRArmPosition(), "RGun", true) && _rightGun;
		bool legs = c.RayToPartsForAttack(GetLegsPosition(), "Legs", true) && _legs.GetCurrentHp() > 0;
	}*/

	void PaintTilesInRange(Tile currentTile, int count)
	{
		
		if (count >= _gun.GetAttackRange() || !currentTile) return;

		foreach (var item in currentTile.allNeighbours)
		{
			if (!_tilesInRange.Contains(currentTile)) 
			{
				if(item && item.IsWalkable())
				{
					_tilesInRange.Add(currentTile);
					TileHighlight.Instance.MortarPaintTilesInAttackRange(item);
				}
			}
			PaintTilesInRange(item, count + 1);
		}
	}
}
