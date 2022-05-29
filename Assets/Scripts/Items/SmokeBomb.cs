using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBomb : Item, IObserver
{
	private SmokeBombSO _data;
	private GameObject _smokeScreen;
	private HashSet<Tile> _tilesInRange = new HashSet<Tile>();
	private int _turnsLived;

	private delegate void Execute();

	private Dictionary<string, Execute> _actionDic = new Dictionary<string, Execute>();

	public override void Initialize(Character character, EquipableSO data)
	{
		base.Initialize(character, data);

		_data = data as SmokeBombSO;
		_actionDic.Add("EndTurn", UpdateLifeSpan);
	}

	public override void Select()
	{
		_character.DeselectThisUnit();

		_character.EquipableSelectionState(true, this);

		PaintTilesInSelectionRange(_character.GetMyPositionTile(), 0);

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
		TileHighlight.Instance.MortarClearTilesInAttackRange(_tilesInRange);

		_tilesInRange.Clear();

		_character.EquipableSelectionState(false, null);

		_character.SelectThisUnit();
	}

	public override void Use(Action callback = null)
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
			UseItem(callback);
		}

		if (Input.GetMouseButtonDown(1))
		{
			_smokeScreen.SetActive(false);
			Deselect();
		}
		
	}

	private void UseItem(Action callback = null)
    {
		EffectsController.Instance.PlayParticlesEffect(gameObject, EnumsClass.ParticleActionType.SmokeBomb);

		TurnManager.Instance.Subscribe(this);

		//Creo la esfera con el radio y le agrego el collider
		//Para saber la posición donde crear la esfera necesito saber el tile que estoy tocando con un raycast

		_turnsLived = 0;
        //_smokeScreen = Instantiate(_data.smokeGameObject, selectedTile.transform.position, Quaternion.identity);
        //Tengo en cuenta el transcurso de los turnos para saber cuando muere el efecto.

        callback?.Invoke();

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
	IEnumerator DestroyDelay()
	{
		yield return new WaitForEndOfFrame();

		TurnManager.Instance.Unsubscribe(this);

		Destroy(_smokeScreen);
	}

	public void Notify(string action)
	{
		if (_actionDic.ContainsKey(action))
			_actionDic[action]();
	}
}
