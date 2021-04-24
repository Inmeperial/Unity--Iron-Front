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
    ButtonsUIManager _buttonsManager;
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
        _buttonsManager = FindObjectOfType<ButtonsUIManager>();
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
        _buttonsManager.DeactivateMoveButton();
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

    public void ActivateMoveButton()
    {
        _buttonsManager.ActivateMoveButton();
    }

    public void DeactivateMoveButton()
    {
        _buttonsManager.DeactivateMoveButton();
    }

    public void ActivateAttackButton()
    {
        _buttonsManager.ActivateExecuteAttackButton();
    }

    public void DeactivateAttackButton()
    {
        _buttonsManager.DeactivateExecuteAttackButton();
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

    //public void DamageEnemy(Character unit, int damage)
    //{
    //    unit.TakeDamage(damage);
    //}
}
