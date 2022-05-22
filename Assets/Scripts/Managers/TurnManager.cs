using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TurnManager : EnumsClass, IObservable, IObserver
{
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private float _endPanelDelay;

    private List<Character> _greenTeam = new List<Character>();
    private int _greenDeadCount;

    private List<Character> _redTeam = new List<Character>();
    private int _redDeadCount;

    private Character[] _allUnits;

    private Team _activeTeam;

    [Header("Turns")]
	[SerializeField] private List<Character> _charactersInCurrentTurnOrder = new List<Character>();
    
    private List<IObserver> _observers = new List<IObserver>();

    private Character _actualCharacter;
    private CameraMovement _cameraMovement;

    [Header("Mortar Turn")]
    public float waitForMortarAttack;
    private bool _mortarAttack;

    private delegate void Execute();

    Dictionary<string, Execute> _actionsDic = new Dictionary<string, Execute>();

    public static TurnManager Instance;

    public void ManualAwake()
    {
        _cameraMovement = FindObjectOfType<CameraMovement>();

        Character[] units = FindObjectsOfType<Character>();

        _allUnits = new Character[units.Length];
        _allUnits = units;
        
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    public void ManualStart()
    {
        SeparateByTeam(_allUnits);
        //_activeTeam = Team.Green;

        Mortar[] mortars = FindObjectsOfType<Mortar>();

        foreach (Mortar mortar in mortars)
        {
            Subscribe(mortar);
        }

        Elevator[] elevators = FindObjectsOfType<Elevator>();

        foreach (Elevator elevator in elevators)
        {
            Subscribe(elevator);
        }

        SetFirstTurn();

        _actualCharacter = _charactersInCurrentTurnOrder[0];

        _activeTeam = _actualCharacter.GetUnitTeam();
        
        _actionsDic.Add("GreenDead", GreenUnitDied);
        _actionsDic.Add("RedDead", RedUnitDied);
        
        
        Action toDo = () =>
        {
            _actualCharacter.SetTurn(true);
            CharacterSelection.Instance.Selection(_actualCharacter);
        };
        if (_activeTeam == Team.Green)
        {
            CharacterSelection.Instance.ActivateCharacterSelection(true);

            ButtonsUIManager.Instance.RightWeaponCircleState(true);
            ButtonsUIManager.Instance.LeftWeaponCircleState(true);
            ButtonsUIManager.Instance.RightWeaponBarButtonState(true);
            ButtonsUIManager.Instance.LeftWeaponBarButtonState(true);
            
            toDo += () => ButtonsUIManager.Instance.ActivateEndTurnButton();
        }
        else
        {
            CharacterSelection.Instance.ActivateCharacterSelection(false);

            ButtonsUIManager.Instance.DeactivateEndTurnButton();
            
            ButtonsUIManager.Instance.RightWeaponCircleState(false);
            ButtonsUIManager.Instance.LeftWeaponCircleState(false);
            ButtonsUIManager.Instance.RightWeaponBarButtonState(false);
            ButtonsUIManager.Instance.LeftWeaponBarButtonState(false);
            
        }
        _cameraMovement.MoveTo(_actualCharacter.transform, toDo, _actualCharacter.transform);
    }


    public void UnitIsMoving() => CharacterSelection.Instance.ActivateCharacterSelection(false);

    public void UnitStoppedMoving() => CharacterSelection.Instance.ActivateCharacterSelection(true);

    private void SeparateByTeam(Character[] units)
    {
        foreach (Character unit in units)
        {
            if (unit.GetUnitTeam() == Team.Green)
                _greenTeam.Add(unit);
            else
                _redTeam.Add(unit);
        }
    }

    public List<Character> GetEnemies(Team myTeam) => myTeam == Team.Green ? _redTeam : _greenTeam;

    public void EndTurn()
    {
        Character character = CharacterSelection.Instance.GetSelectedCharacter();

        if (character == null)
            return;
        
        if (character.IsMoving())
            return;

        if (character.GetUnitTeam() == Team.Red)
        {
            EnemyCharacter enemy = character as EnemyCharacter;

            if (enemy)
                enemy.ForceEnd();
        }

        CharacterSelection.Instance.ResetSelector();

        NotifyObserver("EndTurn");

        NotifyObserver("Deselect");
        
        StartCoroutine(OnEndTurn());
    }

    IEnumerator OnEndTurn()
    {
        if (_mortarAttack)
        {
            yield return new WaitUntil(() => !_mortarAttack);
            yield return new WaitForSeconds(waitForMortarAttack);
        }
        
        MoveToLast();

        ResetTurn(_actualCharacter);

        _actualCharacter = _charactersInCurrentTurnOrder[0];

        while (_actualCharacter.GetBody().GetCurrentHp() <= 0 || !_actualCharacter.IsUnitEnabled())
        {
            MoveToLast();

            yield return new WaitForSeconds(1);

            _actualCharacter = _charactersInCurrentTurnOrder[0];
        }

        Action toDo = () => CharacterSelection.Instance.Selection(_actualCharacter);

        _actualCharacter.SetTurn(true);

        _activeTeam = _actualCharacter.GetUnitTeam();

        if (_activeTeam == Team.Green)
        {
            CharacterSelection.Instance.ActivateCharacterSelection(true);

            ButtonsUIManager.Instance.RightWeaponCircleState(true);
            ButtonsUIManager.Instance.LeftWeaponCircleState(true);
            ButtonsUIManager.Instance.RightWeaponBarButtonState(true);
            ButtonsUIManager.Instance.LeftWeaponBarButtonState(true);
            
            toDo += () => ButtonsUIManager.Instance.ActivateEndTurnButton();
        }
        else
        {
            CharacterSelection.Instance.ActivateCharacterSelection(false);
            
            ButtonsUIManager.Instance.DeactivateEndTurnButton();
            
            ButtonsUIManager.Instance.RightWeaponCircleState(false);
            ButtonsUIManager.Instance.LeftWeaponCircleState(false);
            ButtonsUIManager.Instance.RightWeaponBarButtonState(false);
            ButtonsUIManager.Instance.LeftWeaponBarButtonState(false);
        }
         
        _cameraMovement.MoveTo(_actualCharacter.transform, toDo, _actualCharacter.transform);
    }

    void MoveToLast()
    {
        List<FramesUI> portraits = PortraitsController.Instance.GetPortraits();

        ButtonsUIManager.Instance.DeactivateEndTurnButton();

        PortraitsController.Instance.MovePortrait(_actualCharacter, 0, PortraitsController.Instance.GetPortraitsRectPosition().Count-1);

        _charactersInCurrentTurnOrder.RemoveAt(0);

        _charactersInCurrentTurnOrder.Insert(_charactersInCurrentTurnOrder.Count, _actualCharacter);
        
        Dictionary<Character, int> previous = new Dictionary<Character, int>();

        for (int i = 0; i < _charactersInCurrentTurnOrder.Count; i++)
        {
            Character character = _charactersInCurrentTurnOrder[i];
            previous.Add(character, i);
        }
        
        for (int i = 0; i < portraits.Count-1; i++)
        {
            Character character = _charactersInCurrentTurnOrder[i];
            
            PortraitsController.Instance.MovePortrait(character, previous[character], i);
        }
    }

    /// <summary>
    /// Reset stats for new turn of the given unit.
    /// </summary>
    private void ResetTurn(Character unit)
    {
        TileHighlight.Instance.EndPreview();
        unit.NewTurn();
    }

    public Team GetActiveTeam() => _activeTeam;

    public void UnitCanBeAttacked(Character unit) => unit.MakeAttackable();

    public void UnitCantBeAttacked(Character unit) => unit.MakeNotAttackable();

    public Tile GetUnitTile(Character unit) => unit.GetMyPositionTile();

    private void SetFirstTurn()
    {
        List<Tuple<Character,float>> orderedUnits = GetOrderedUnits();

        int count = 0;

        List<FramesUI> portraits = PortraitsController.Instance.GetPortraits();

        foreach (Tuple<Character, float> tuple in orderedUnits)
        {
            if (count == portraits.Count)
                return;
            
            Character character = tuple.Item1;

            character.transform.parent.name = character.GetCharacterName();
            
            FramesUI portrait = PortraitsController.Instance.SetPortrait(character, count, character.GetCharacterSprite(), character.GetCharacterName(), character.GetUnitTeam(),
                () => _cameraMovement.MoveTo(character.transform));

            count++;

            _charactersInCurrentTurnOrder.Add(character);

            PortraitsController.Instance.AddCharAndFrame(Tuple.Create(character,portrait));

            character.CheckWeight();

            if (!character.IsUnitEnabled())
                portrait.selectionButton.interactable = false;                
        }
    }

    public void ReducePosition(Character unit)
    {
        int oldPos = GetMyTurn(unit);
        
        //If last, return
        if (oldPos == _charactersInCurrentTurnOrder.Count -1)
            return;

        //Move unit portrait one position behind
        int newPos;

        if (unit.GetLegs().GetCurrentHp() > 0)
        {
            if (unit.IsOverweight())
            {
                newPos = oldPos + 3;

                if (newPos >= _charactersInCurrentTurnOrder.Count - 1)
                    newPos = _charactersInCurrentTurnOrder.Count - 1;
            }
            else
                newPos = oldPos + 1;

            PortraitsController.Instance.MovePortrait(unit, oldPos, newPos);

            //var othersTurns = newPos - oldPos;

            for (int i = newPos; i > oldPos; i--)
            {
                PortraitsController.Instance.MovePortrait(_charactersInCurrentTurnOrder[i], i, i-1);
            }

            Character other = _charactersInCurrentTurnOrder[newPos];

            //PortraitsController.Instance.MovePortrait(other, newPos, oldPos);
            _charactersInCurrentTurnOrder.RemoveAt(oldPos);

            _charactersInCurrentTurnOrder.Insert(newPos, unit);
        }
        else
        {
            newPos = PortraitsController.Instance.GetPortraitsRectPosition().Count - 1;

            PortraitsController.Instance.MovePortrait(unit, oldPos, newPos);

            _charactersInCurrentTurnOrder.RemoveAt(oldPos);

            _charactersInCurrentTurnOrder.Insert(newPos, unit);

            for (int i = oldPos; i < newPos; i++)
            {
                Character u = _charactersInCurrentTurnOrder[i];

                PortraitsController.Instance.MovePortrait(u, i+1, i);
            }
        } 
    }

    List<Tuple<Character, float>> GetOrderedUnits()
    {
        List<Tuple<Character, float>> unitsList = new List<Tuple<Character, float>>();
        
        //Adds them to a collection with their initiative
        foreach (Character character in _allUnits)
        {
            Tuple<Character, float> tuple = Tuple.Create(character, character.GetCharacterInitiative());

            unitsList.Add(tuple);
        }

        List<Tuple<Character, float>> orderedUnits = unitsList.OrderByDescending(x => x.Item2).ToList();

        List<Tuple<Character, float>> orderedWithEnabled = new List<Tuple<Character, float>>();

        int disabledCount = 0;

        foreach (Tuple<Character, float> tuple in orderedUnits)
        {
            if (tuple.Item1.IsUnitEnabled())
            {
                if (disabledCount == 0)
                    orderedWithEnabled.Add(tuple);
                else 
                    orderedWithEnabled.Insert(orderedWithEnabled.Count - disabledCount, tuple);
            }
                
            else
            {
                if (orderedWithEnabled.Count > 1)
                    orderedWithEnabled.Insert(orderedWithEnabled.Count-1, tuple);
                else
                    orderedWithEnabled.Add(tuple);

                disabledCount++;
            }
        }
        return orderedWithEnabled;
    }

    public int GetMyTurn(Character unit)
    {
        for (int i = 0; i < _charactersInCurrentTurnOrder.Count; i++)
        {
            if (_charactersInCurrentTurnOrder[i] == unit)
                return i;
        }

        return -1;
    }

    public Character[] GetAllUnits() => _allUnits;

    private void GreenUnitDied()
    {
        _greenDeadCount++;

        if (_greenDeadCount < _greenTeam.Count)
            return;

        StartCoroutine(EndPanelDelayActivation("Lose"));
    }

    private void RedUnitDied()
    {
        _redDeadCount++;

        if (_redDeadCount < _redTeam.Count) 
            return;

        StartCoroutine(EndPanelDelayActivation("Win"));
    }

    IEnumerator EndPanelDelayActivation(string panel)
    {
        yield return new WaitForSeconds(_endPanelDelay);

        foreach (EnemyCharacter unit in _redTeam)
        {
            unit.DisableUnit();
        }
        
        if (panel == "Win")
            _winPanel.SetActive(true);

        if (panel == "Lose")
            _losePanel.SetActive(true);
    }

    public void SetMortarAttack(bool state) => _mortarAttack = state;

    public void Notify(string action)
    {
        if (_actionsDic.ContainsKey(action))
            _actionsDic[action]();
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
        foreach (var obs in _observers)
        {
            obs.Notify(action);
        }
    }
}
