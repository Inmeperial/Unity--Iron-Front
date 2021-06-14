﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class TurnManager : EnumsClass, IObservable
{
    [SerializeField] List<Character> _capsuleTeam = new List<Character>();
    [SerializeField] List<Character> _boxTeam = new List<Character>();
    private Character[] _allUnits;
    [SerializeField] CharacterSelection _charSelect;
    [SerializeField] TileHighlight _highlight;
    private ButtonsUIManager _buttonsManager;

    private Team _activeTeam;

	[SerializeField]
    private List<Character> _currentTurnOrder = new List<Character>();
    
    private List<IObserver> _observers = new List<IObserver>();

    public int _turnCounter;
    private Character _actualCharacter;
    private CameraMovement _cameraMovement;

    private void Awake()
    {
        _cameraMovement = FindObjectOfType<CameraMovement>();
        var units = FindObjectsOfType<Character>();
        _allUnits = new Character[units.Length];
        _allUnits = units;
        
    }

    private void Start()
    {
        SeparateByTeam(_allUnits);
        _charSelect = FindObjectOfType<CharacterSelection>();
        _highlight = FindObjectOfType<TileHighlight>();
        _buttonsManager = FindObjectOfType<ButtonsUIManager>();
        _activeTeam = Team.Capsule;

        var mortars = FindObjectsOfType<Mortar>();
        foreach (var mortar in mortars)
        {
            Subscribe(mortar);
        }
        CalculateTurnOrder();

        _actualCharacter = _currentTurnOrder[0];
        _actualCharacter.SetTurn(true);
        _activeTeam = _actualCharacter.GetUnitTeam();
        Action toDo = () =>
        {
            _buttonsManager.ActivateEndTurnButton();
            _charSelect.Selection(_actualCharacter);
        };
        _cameraMovement.MoveTo(_actualCharacter.transform.position, toDo);
    }

    public void UnitIsMoving()
    {
        _charSelect.ActivateCharacterSelection(false);
    }

    public void UnitStoppedMoving()
    {
        _charSelect.ActivateCharacterSelection(true);
    }

    private void SeparateByTeam(Character[] units)
    {
        foreach (var item in units)
        {
            if (item.GetUnitTeam() == EnumsClass.Team.Capsule)
                _capsuleTeam.Add(item);
            else _boxTeam.Add(item);
        }
    }

    public List<Character> GetEnemies(Team myTeam)
    {
        return myTeam == Team.Capsule ? _boxTeam : _capsuleTeam;
    }

    public void EndTurn()
    {
        var character = _charSelect.GetSelectedCharacter();
        if (character != null && character.IsMoving() != false) return;
        
        _buttonsManager.DeactivateEndTurnButton();
            
        ResetTurn(_actualCharacter);

        if (_turnCounter >= _currentTurnOrder.Count-1)
        {
            Debug.Log("calculate");
            CalculateTurnOrder();
            _actualCharacter = _currentTurnOrder[0];
        }
        else
        {
            _turnCounter++;
            _actualCharacter = _currentTurnOrder[_turnCounter];
        }
            
        _actualCharacter.SetTurn(true);
        _activeTeam = _actualCharacter.GetUnitTeam();
        Action toDo = () =>
        {
            _buttonsManager.ActivateEndTurnButton();
            _charSelect.Selection(_actualCharacter);
        };
        _cameraMovement.MoveTo(_actualCharacter.transform.position, toDo);

        NotifyObserver("EndTurn");
        NotifyObserver("Deselect");
    }

    private void ResetTurn(List<Character> team)
    {
        _charSelect.ResetSelector();
        _highlight.EndPreview();
        foreach (var unit in team)
        {
            unit.NewTurn();
        }
    }

    /// <summary>
    /// Reset stats for new turn of the given unit.
    /// </summary>
    private void ResetTurn(Character unit)
    {
        _charSelect.ResetSelector();
        _highlight.EndPreview();
        unit.NewTurn();
    }

    public Team GetActiveTeam()
    {
        return _activeTeam;
    }

    public void UnitCanBeAttacked(Character unit)
    {
        unit.MakeAttackable();
    }

    public void UnitCantBeAttacked(Character unit)
    {
        unit.MakeNotAttackable();
    }

    public Tile GetUnitTile(Character unit)
    {
        return unit.GetTileBelow();
    }

    public void CalculateTurnOrder()
    {
        _turnCounter = 0;
        _currentTurnOrder.Clear();
        var unitsList = new List<Tuple<Character, float>>();
        
        //Adds them to a collection with their initiative
        foreach (var character in _allUnits)
        {
            if (!(character.body.GetCurrentHp() > 0)) continue;
            
            var t = Tuple.Create(character, character.GetCharacterInitiative());
            unitsList.Add(t);
        }

        //Orders them with the highest initiative first
        var ordered = unitsList.OrderByDescending(x => x.Item2);

        foreach (var character in ordered)
        {
            character.Item1.SetTurn(false);
            _currentTurnOrder.Add(character.Item1);
        }
    }

    public int GetMyTurn(Character unit)
    {
        for (int i = 0; i < _currentTurnOrder.Count; i++)
        {
            if (_currentTurnOrder[i] == unit)
                return i + 1;
        }

        return -1;
    }

    public void Subscribe(IObserver observer)
    {
        if (_observers.Contains(observer) == false)
            _observers.Add(observer);
    }

    public void Unsubscribe(IObserver observer)
    {
        if (_observers.Contains(observer))
            _observers.Remove(observer);
    }

    public void NotifyObserver(string action)
    {
        for (int i = _observers.Count - 1; i >= 0; i--)
        {
            _observers[i].Notify(action);
        }
    }
}
