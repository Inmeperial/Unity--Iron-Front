using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBomb : Item
{
	private SmokeBombSO _data;
	private GameObject _smokeScreen;
	private HashSet<Tile> _tilesInRange = new HashSet<Tile>();
	private int _turnsLived;

	public override void Initialize(Character character, EquipableSO data)
	{
		base.Initialize(character, data);

		_data = data as SmokeBombSO;
	}

	public override void Select()
	{
		OnEquipableSelected?.Invoke();

		_character.DeselectThisUnit();

		_character.EquipableSelectionState(true, this);

		PaintTilesInSelectionRange(_character.GetPositionTile(), 0);

		if (!_smokeScreen)
			_smokeScreen = Instantiate(_data.smokeGameObject);
		else
			_smokeScreen.SetActive(true);
	}

	private void PaintTilesInSelectionRange(Tile currentTile, int count)
	{
		if (count >= _data.useRange)
			return;

		foreach (var item in currentTile.allNeighbours)
		{
			if (!_tilesInRange.Contains(item))
			{
				_tilesInRange.Add(item);
				TileHighlight.Instance.MortarPaintTilesInAttackRange(item);
			}
			PaintTilesInSelectionRange(item, count + 1);
		}
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
        Transform selectedTile = MouseRay.GetTargetTransform(_character.GetBlockLayerMask());
		
		if (!selectedTile)
			return;

        Tile tile = selectedTile.GetComponent<Tile>();

		if (!tile)
			return;
		
		if (!_tilesInRange.Contains(tile))
			return;
		
		_smokeScreen.transform.position = selectedTile.transform.position;

		if (Input.GetMouseButtonDown(0))
		{
			UseItem();
		}

		if (Input.GetMouseButtonDown(1))
		{
			_smokeScreen.SetActive(false);
			Deselect();
		}
		
	}

	private void UseItem()
    {
		EffectsController.Instance.PlayParticlesEffect(_data.particleEffect, _smokeScreen.transform.position, _smokeScreen.transform.forward);

		AudioManager.Instance.PlaySound(_data.sound, _smokeScreen);

		//TurnManager.Instance.Subscribe(this);

		GameManager.Instance.OnEndTurn += UpdateLifeSpan;

		//Creo la esfera con el radio y le agrego el collider
		//Para saber la posición donde crear la esfera necesito saber el tile que estoy tocando con un raycast

		_turnsLived = 0;
        //_smokeScreen = Instantiate(_data.smokeGameObject, selectedTile.transform.position, Quaternion.identity);
        //Tengo en cuenta el transcurso de los turnos para saber cuando muere el efecto.

        OnEquipableUsed?.Invoke();

        ItemUsed();

		UpdateButtonText(_availableUses.ToString(), _data);

		_button.interactable = false;

		Deselect();
	}

	private void UpdateLifeSpan()
	{
		_turnsLived++;

		if (_turnsLived >= _data.duration)
			StartCoroutine(DestroyDelay());
	}

	//Para evitar que se destruya en el mismo frame que se hace el notify, sino da error al modificar la coleccion del turn manager mientras se la usa.
	private IEnumerator DestroyDelay()
	{
		yield return new WaitForEndOfFrame();

		//TurnManager.Instance.Unsubscribe(this);

		GameManager.Instance.OnEndTurn -= UpdateLifeSpan;
		Destroy(_smokeScreen);
	}
}
