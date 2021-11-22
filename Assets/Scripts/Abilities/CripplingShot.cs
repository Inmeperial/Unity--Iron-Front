using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CripplingShot : Ability
{
	private TileHighlight _highlight;
	private int _attackRange;
	[SerializeField] int _damage;
	private HashSet<Tile> _tilesInRange = new HashSet<Tile>();
	private EnemyCharacter _enemy;
	public override void Select()
	{
		if (InCooldown() || !_character.CanAttack()) return;
		_attackRange = 4;//_character.GetRightGun().GetAttackRange();
		if (!_highlight)
			_highlight = FindObjectOfType<TileHighlight>();
		PaintTilesInRange(_character.GetTileBelow(), 0);
		_character.EquipableSelectionState(true, this);
		_character.DeselectThisUnit();
	}

	public override void Deselect()
	{
		_highlight.MortarClearTilesInAttackRange(_tilesInRange);
		_tilesInRange.Clear();
		_character.EquipableSelectionState(false, null);
		_character.SelectThisUnit();
	}

	public override void Use(Action callback = null)
	{
		Debug.Log("Using Crippling Shot");
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("Crippling Shot");
			//Selecionar al enemigo que esta en rango como si lo estás atacando normalmente.
			_enemy = MouseRay.GetTargetTransform(LayerMask.NameToLayer("Character")).GetComponent<EnemyCharacter>();
			//Agregar lo de la rotación y el rayo a las piernas
			if (!_enemy) return;
			var dir = _enemy.GetLegsPosition() - _character.transform.position;
			
			//Si puede ver las piernas, le dispara, hace daño y evita que se mueva el proximo turno
			if(Physics.Raycast(_character.transform.position, dir, LayerMask.NameToLayer("Character")))
			{
				_enemy.GetLegs().TakeDamage(_damage);
				_enemy.SetHurtAnimation();
				//_enemy.DeactivateMove();//New
				_character.DeactivateAttack();

				Deselect();
			}

			if (callback != null)
				callback();
			
		}

		if (Input.GetMouseButtonDown(1))
			Deselect();
		
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
		
		if (count >= _attackRange || !currentTile) return;

		foreach (var item in currentTile.allNeighbours)
		{
			if (!_tilesInRange.Contains(currentTile)) 
			{
				if(item && item.IsWalkable())
				{
					_tilesInRange.Add(currentTile);
					_highlight.MortarPaintTilesInAttackRange(item);
				}
			}
			PaintTilesInRange(item, count + 1);
		}
	}
}
