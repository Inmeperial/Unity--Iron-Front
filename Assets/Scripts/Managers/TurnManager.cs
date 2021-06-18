using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using UnityEngine.Experimental.PlayerLoop;

public class TurnManager : EnumsClass, IObservable
{
    [SerializeField] List<Character> _capsuleTeam = new List<Character>();
    [SerializeField] List<Character> _boxTeam = new List<Character>();
    private Character[] _allUnits;
    [SerializeField] CharacterSelection _charSelect;
    [SerializeField] TileHighlight _highlight;
    private ButtonsUIManager _buttonsUIManager;

    private Team _activeTeam;

	[SerializeField]
    private List<Character> _currentTurnOrder = new List<Character>();
    
    private List<IObserver> _observers = new List<IObserver>();

    public int _turnCounter;
    private Character _actualCharacter;
    private CameraMovement _cameraMovement;

    [SerializeField] private List<FramesUI> _portraits = new List<FramesUI>();
    [SerializeField] private List<RectTransform> _portraitsPositions = new List<RectTransform>();
    [SerializeField] private float _moveTime;

    public int current;
    public int newPos;
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
        _buttonsUIManager = FindObjectOfType<ButtonsUIManager>();
        _activeTeam = Team.Capsule;

        var mortars = FindObjectsOfType<Mortar>();
        foreach (var mortar in mortars)
        {
            Subscribe(mortar);
        }
        CalculateTurnOrder(true);

        _actualCharacter = _currentTurnOrder[0];
        _actualCharacter.SetTurn(true);
        _activeTeam = _actualCharacter.GetUnitTeam();
        Action toDo = () =>
        {
            _buttonsUIManager.ActivateEndTurnButton();
            _charSelect.Selection(_actualCharacter);
        };
        _cameraMovement.MoveTo(_actualCharacter.transform.position, toDo);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(MovePortrait(current, newPos));
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
        
        _buttonsUIManager.DeactivateEndTurnButton();
            
        ResetTurn(_actualCharacter);

        if (_turnCounter >= _currentTurnOrder.Count-1)
        {
            Debug.Log("calculate");
            CalculateTurnOrder(false);
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
            _buttonsUIManager.ActivateEndTurnButton();
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

    public void CalculateTurnOrder(bool firstTurn)
    {
        _turnCounter = 0;
        //_currentTurnOrder.Clear();
        var unitsList = new List<Tuple<Character, float>>();
        
        //Adds them to a collection with their initiative
        foreach (var character in _allUnits)
        {
            //if (!(character.body.GetCurrentHp() > 0)) continue;
            
            var t = Tuple.Create(character, character.GetCharacterInitiative());
            unitsList.Add(t);
        }

        //Orders them with the highest initiative first
        var ordered = unitsList.OrderByDescending(x => x.Item2);

        int count = 0;

        foreach (var character in ordered)
        {
            if (count == _portraits.Count)
                return;
            
            var c = character.Item1;

            if (firstTurn)
            {
                var p = _portraits[count];
                p.mechaImage.sprite = c._myIcon;
                p.mechaName.text = c._myName;
                p.leftGunIcon.sprite = c.GetLeftGun().GetIcon();
                p.rightGunIcon.sprite = c.GetRightGun().GetIcon();
            }
            
            if (!firstTurn)
                StartCoroutine(MovePortrait(GetMyTurn(c)-1, count));
           
            c.SetTurn(false);
            
            count++;
        }
        _currentTurnOrder.Clear();
        foreach (var character in ordered)
        {
            var c = character.Item1;
            _currentTurnOrder.Add(c);
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

    IEnumerator MovePortrait(int current, int newPos)
    {
        var rect = _portraits[current].gameObject.GetComponent<RectTransform>();
        var startV = _portraitsPositions[current].gameObject.GetComponent<RectTransform>();
        var end = _portraitsPositions[newPos].gameObject.GetComponent<RectTransform>();
        var time = 0f;
        
        Debug.Log("empiezo");
        while (time <= _moveTime)
        {
            time += Time.deltaTime;
            var normalized = time / _moveTime;

            rect.anchorMax = Vector2.Lerp(startV.anchorMax, end.anchorMax, normalized);
            rect.anchorMin = Vector2.Lerp(startV.anchorMin, end.anchorMin, normalized);
            yield return new WaitForEndOfFrame();
        }

        rect.anchoredPosition = end.anchoredPosition;
    }
}
