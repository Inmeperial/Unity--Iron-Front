using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TurnManager : Teams
{
    [SerializeField] List<Character> _capsuleTeam = new List<Character>();
    [SerializeField] List<Character> _boxTeam = new List<Character>();
    [SerializeField] CharacterSelection _charSelect;
    [SerializeField] TileHighlight _highlight;
    public TextMeshProUGUI teamText;
    public string _CapsuleTeamText = "Capsule Team Turn.";
    public string _BoxTeamText = "Box Team Turn.";
    
    public Team _activeTeam;
    void Start()
    {
        var units = FindObjectsOfType<Character>();
        SeparateByTeam(units);
        _charSelect = FindObjectOfType<CharacterSelection>();
        _highlight = FindObjectOfType<TileHighlight>();
        _activeTeam = Team.Capsule;
        teamText.text = _CapsuleTeamText;
    }

    public void UnitIsMoving()
    {
        _charSelect.ActivateCharacterSelection(false);
        DeactivateMoveButton();
    }

    public void UnitStoppedMoving()
    {
        _charSelect.ActivateCharacterSelection(true);
        _charSelect.DeactivateMoveButton();
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
                teamText.text = _BoxTeamText;
                ResetTurn(_boxTeam);
            }
            else
            {
                _activeTeam = Team.Capsule;
                teamText.text = _CapsuleTeamText;
                ResetTurn(_capsuleTeam);
            }
        }
    }

    void ResetTurn(List<Character> team)
    {
        _charSelect.ResetSelector();
        foreach (var unit in team)
        {
            unit.NewTurn();
        }
    }

    public Team GetActiveTeam()
    {
        return _activeTeam;
    }

    public void ActivateMoveButton()
    {
        _charSelect.ActivateMoveButton();
    }

    public void DeactivateMoveButton()
    {
        _charSelect.DeactivateMoveButton();
    }

    public void ActivateAttackButton()
    {
        _charSelect.ActivateAttackButton();
    }

    public void DeactivateAttackButton()
    {
        _charSelect.DeactivateAttackButton();
    }

    public void UpdateHP(int currentHP, int maxHP)
    {
        _charSelect.UpdateHP(currentHP, maxHP);
    }
}
