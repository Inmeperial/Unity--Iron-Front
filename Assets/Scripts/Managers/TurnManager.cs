using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TurnManager : Teams, IObservable
{
    [SerializeField] List<Character> _capsuleTeam = new List<Character>();
    [SerializeField] List<Character> _boxTeam = new List<Character>();
    [SerializeField] CharacterSelection _charSelect;
    [SerializeField] TileHighlight _highlight;
    ButtonsUIManager _buttonsManager;
    public TextMeshProUGUI teamText;
    public GameObject flag1;
    public GameObject flag2;
    
    private Team _activeTeam;
    
    private List<IObserver> _observers = new List<IObserver>();
    void Start()
    {
        var units = FindObjectsOfType<Character>();
        SeparateByTeam(units);
        _charSelect = FindObjectOfType<CharacterSelection>();
        _highlight = FindObjectOfType<TileHighlight>();
        _buttonsManager = FindObjectOfType<ButtonsUIManager>();
        _activeTeam = Team.Capsule;
        //teamText.text = _CapsuleTeamText;
        if (_activeTeam == Team.Capsule)
        {
            flag1.SetActive(true);
            flag2.SetActive(false);
        }
        else
        {
            flag1.SetActive(false);
            flag2.SetActive(true);
        }

        var mortars = FindObjectsOfType<Mortar>();
        foreach (var mortar in mortars)
        {
            Subscribe(mortar);
        }
    }

    public void UnitIsMoving()
    {
        _charSelect.ActivateCharacterSelection(false);
    }

    public void UnitStoppedMoving()
    {
        _charSelect.ActivateCharacterSelection(true);
    }

    void SeparateByTeam(Character[] units)
    {
        foreach (var item in units)
        {
            if (item.GetUnitTeam() == Teams.Team.Capsule)
                _capsuleTeam.Add(item);
            else _boxTeam.Add(item);
        }
    }

    public List<Character> GetEnemies(Teams.Team myTeam)
    {
        if (myTeam == Team.Capsule)
            return _boxTeam;

        else return _capsuleTeam;
    }

    public void EndTurn()
    {
        var character = _charSelect.GetActualChar();
        if (character ==null || character.IsMoving() == false)
        {
            if (_activeTeam == Team.Capsule)
            {
                _activeTeam = Team.Box;
                //teamText.text = _BoxTeamText;
                flag2.SetActive(true);
                flag1.SetActive(false);
                ResetTurn(_boxTeam);
            }
            else
            {
                _activeTeam = Team.Capsule;
                //teamText.text = _CapsuleTeamText;
                flag1.SetActive(true);
                flag2.SetActive(false);
                ResetTurn(_capsuleTeam);
            }
            
            NotifyObserver("EndTurn");
            NotifyObserver("Deselect");
        }
    }

    void ResetTurn(List<Character> team)
    {
        _charSelect.ResetSelector();
        _highlight.EndPreview();
        foreach (var unit in team)
        {
            unit.NewTurn();
        }
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

    public void Subscribe(IObserver observer)
    {
        if (_observers.Contains(observer) == false)
            _observers.Add(observer);
    }

    public void Unsuscribe(IObserver observer)
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
