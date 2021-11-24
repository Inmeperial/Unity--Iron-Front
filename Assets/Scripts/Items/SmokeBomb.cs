﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBomb : Item, IObserver
{
	private SmokeBombSO _data;
	private GameObject _smokeScreen;
	private HashSet<Tile> _tilesInRange = new HashSet<Tile>();
	private TileHighlight _highlight;
	private int _turnsLived;
	private delegate void Execute();
	private Dictionary<string, Execute> _actionDic = new Dictionary<string, Execute>();
	public override void Initialize(Character character, EquipableSO data, Location location)
	{
		base.Initialize(character, data, location);
		_data = data as SmokeBombSO;
		_highlight = FindObjectOfType<TileHighlight>();
		_actionDic.Add("End Turn", UpdateLifeSpam);
	}

	public override void Select()
	{
		_character.DeselectThisUnit();
		_character.EquipableSelectionState(true, this);
		PaintTilesInSelectionRange(_character.GetMyPositionTile(), 0);
	}

	private void PaintTilesInSelectionRange(Tile currentTile, int count)
	{
		if (count >= _data.useRange) return;

		foreach (var item in currentTile.allNeighbours)
		{
			if (!_tilesInRange.Contains(item))
			{
				_tilesInRange.Add(item);
				_highlight.MortarPaintTilesInActivationRange(item);
			}
			PaintTilesInSelectionRange(item, count + 1);
		}
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
			//Creo la esfera con el radio y le agrego el collider
			//Para saber la posición donde crear la esfera necesito saber el tile que estoy tocando con un raycast
			var selectedTile = MouseRay.GetTargetTransform(_character.block).GetComponent<Tile>();
			if (!selectedTile || !_tilesInRange.Contains(selectedTile)) return;
			_turnsLived = 0;
			_smokeScreen = Instantiate(_data.smokeGameObject, selectedTile.transform.position, Quaternion.identity);
			//Tengo en cuenta el transcurso de los turnos para saber cuando muere el efecto.
			StartCoroutine(LifeSpan());
			if (callback != null)
				callback();
			Deselect();
		}

		if (Input.GetMouseButtonDown(1))
			Deselect();
		
	}
	private void UpdateLifeSpam()
	{
		_turnsLived++;
	}

	private IEnumerator LifeSpan()
	{
		yield return new WaitUntil(() => _turnsLived >= _data.duration);
		
		Destroy(_smokeScreen);
	}

	public void Notify(string action)
	{
		if (_actionDic.ContainsKey(action))
			_actionDic[action]();
	}
}
