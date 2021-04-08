using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    // Update is called once per frame
    void Update()
    {
        
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

    public void ChangeTurn()
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

    void ResetTurn(List<Character> team)
    {
        _charSelect.DeselectUnit();
        foreach (var unit in team)
        {
            unit.NewTurn();
        }
    }

    public Team GetActiveTeam()
    {
        return _activeTeam;
    }
}
