using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TurnManager : EnumsClass, IObservable, IObserver
{
    private List<Character> _greenTeam = new List<Character>();
    private int _greenDeadCount;
    private List<Character> _redTeam = new List<Character>();
    private int _redDeadCount;
    private Character[] _allUnits;
    private TileHighlight _highlight;
    private ButtonsUIManager _buttonsUIManager;

    private Team _activeTeam;

    [Header("Turns")]
	[SerializeField] private List<Character> _currentTurnOrder = new List<Character>();
    
    private List<IObserver> _observers = new List<IObserver>();

    private int _turnCounter;
    private Character _actualCharacter;
    private CameraMovement _cameraMovement;

    [Header("Mortar Turn")]
    public float waitForMortarAttack;
    private bool _mortarAttack;
    private delegate void Execute();
    Dictionary<string, Execute> _actionsDic = new Dictionary<string, Execute>();
    public static TurnManager Instance;

    private void Awake()
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
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        SeparateByTeam(_allUnits);
        _highlight = FindObjectOfType<TileHighlight>();
        //_activeTeam = Team.Green;

        Mortar[] mortars = FindObjectsOfType<Mortar>();
        foreach (Mortar mortar in mortars)
        {
            Subscribe(mortar);
        }
        SetFirstTurn();

        _actualCharacter = _currentTurnOrder[0];
        
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
            
            Cursor.lockState = CursorLockMode.None;
            
            ButtonsUIManager.Instance.RightWeaponCircleState(true);
            ButtonsUIManager.Instance.LeftWeaponCircleState(true);
            ButtonsUIManager.Instance.RightWeaponBarButtonState(true);
            ButtonsUIManager.Instance.LeftWeaponBarButtonState(true);
            
            toDo += () => ButtonsUIManager.Instance.ActivateEndTurnButton();
        }
        else
        {
            CharacterSelection.Instance.ActivateCharacterSelection(false);
            
            Cursor.lockState = CursorLockMode.Locked;
            
            ButtonsUIManager.Instance.DeactivateEndTurnButton();
            
            ButtonsUIManager.Instance.RightWeaponCircleState(false);
            ButtonsUIManager.Instance.LeftWeaponCircleState(false);
            ButtonsUIManager.Instance.RightWeaponBarButtonState(false);
            ButtonsUIManager.Instance.LeftWeaponBarButtonState(false);
            
        }
        _cameraMovement.MoveTo(_actualCharacter.transform, toDo, _actualCharacter.transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ForceEndTurn();
        }
    }

    private void ForceEndTurn()
    {
        if (_actualCharacter.GetUnitTeam() == Team.Red)
        {
            EnemyCharacter ai = _actualCharacter as EnemyCharacter;
            
            if (ai) ai.ForceEnd();
        }
        EndTurn();
    }

    public void UnitIsMoving()
    {
        CharacterSelection.Instance.ActivateCharacterSelection(false);
    }

    public void UnitStoppedMoving()
    {
        CharacterSelection.Instance.ActivateCharacterSelection(true);
    }

    private void SeparateByTeam(Character[] units)
    {
        foreach (Character unit in units)
        {
            if (unit.GetUnitTeam() == Team.Green)
                _greenTeam.Add(unit);
            else _redTeam.Add(unit);
        }
    }

    public List<Character> GetEnemies(Team myTeam)
    {
        return myTeam == Team.Green ? _redTeam : _greenTeam;
    }

    public void EndTurn()
    {
        Character character = CharacterSelection.Instance.GetSelectedCharacter();
        if (character != null && character.IsMoving()) return;

        if (character.GetUnitTeam() == Team.Red)
        {
            EnemyCharacter enemy = character as EnemyCharacter;
            if (enemy) enemy.ForceEnd();
        }

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

        _actualCharacter = _currentTurnOrder[0];

        while (_actualCharacter.body.GetCurrentHp() <= 0)
        {
            MoveToLast();
            yield return new WaitForSeconds(1);
            _actualCharacter = _currentTurnOrder[0];
        }

        Action toDo = () => CharacterSelection.Instance.Selection(_actualCharacter);
        _actualCharacter.SetTurn(true);
        _activeTeam = _actualCharacter.GetUnitTeam();
        if (_activeTeam == Team.Green)
        {
            CharacterSelection.Instance.ActivateCharacterSelection(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            ButtonsUIManager.Instance.RightWeaponCircleState(true);
            ButtonsUIManager.Instance.LeftWeaponCircleState(true);
            ButtonsUIManager.Instance.RightWeaponBarButtonState(true);
            ButtonsUIManager.Instance.LeftWeaponBarButtonState(true);
            
            toDo += () => ButtonsUIManager.Instance.ActivateEndTurnButton();
        }
        else
        {
            CharacterSelection.Instance.ActivateCharacterSelection(false);
            Cursor.lockState = CursorLockMode.Locked;
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
        _currentTurnOrder.RemoveAt(0);
        _currentTurnOrder.Insert(_currentTurnOrder.Count, _actualCharacter);
        
        Dictionary<Character, int> previous = new Dictionary<Character, int>();

        for (int i = 0; i < _currentTurnOrder.Count; i++)
        {
            Character c = _currentTurnOrder[i];
            previous.Add(c, i);
        }
        
        for (int i = 0; i < portraits.Count-1; i++)
        {
            Character c = _currentTurnOrder[i];
            
            PortraitsController.Instance.MovePortrait(c, previous[c], i);
        }
    }

    /// <summary>
    /// Reset stats for new turn of the given unit.
    /// </summary>
    private void ResetTurn(Character unit)
    {
        CharacterSelection.Instance.ResetSelector();
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

    private void SetFirstTurn()
    {
        List<Tuple<Character,float>> ordered = GetOrderedUnits();

        int count = 0;

        List<FramesUI> portraits = PortraitsController.Instance.GetPortraits();
        foreach (Tuple<Character, float> character in ordered)
        {
            if (count == portraits.Count)
                return;
            
            Character c = character.Item1;
            c.gameObject.name = c.GetCharacterName();

            FramesUI p = PortraitsController.Instance.SetPortrait(count, c.GetCharacterSprite(), c.GetCharacterName(),
                () => _cameraMovement.MoveTo(c.transform));
            count++;
            _currentTurnOrder.Add(c);
            PortraitsController.Instance.AddCharAndFrame(Tuple.Create(c,p));
        }
    }

    public void ReducePosition(Character unit)
    {
        int oldPos = GetMyTurn(unit);
        
        //If last, return
        if (oldPos == _currentTurnOrder.Count -1) return;
        
        //Move unit portrait one position behind
        int newPos = 0;

        if (unit.legs.GetCurrentHp() > 0)
        {
            newPos = oldPos + 1;
            PortraitsController.Instance.MovePortrait(unit, oldPos, newPos);

            Character other = _currentTurnOrder[newPos];
            PortraitsController.Instance.MovePortrait(other, newPos, oldPos);
            _currentTurnOrder.RemoveAt(oldPos);
            _currentTurnOrder.Insert(newPos, unit);
        }
        else
        {
            newPos = PortraitsController.Instance.GetPortraitsRectPosition().Count - 1;
            PortraitsController.Instance.MovePortrait(unit, oldPos, newPos);
            _currentTurnOrder.RemoveAt(oldPos);
            _currentTurnOrder.Insert(newPos, unit);

            for (int i = oldPos; i < newPos; i++)
            {
                Character u = _currentTurnOrder[i];
                PortraitsController.Instance.MovePortrait(u, i+1, i);
            }
        } 
        
        
    }

    List<Tuple<Character, float>> GetOrderedUnits()
    {
        List<Tuple<Character, float>> unitsList = new List<Tuple<Character, float>>();
        
        //Adds them to a collection with their initiative
        foreach (var character in _allUnits)
        {
            Tuple<Character, float> t = Tuple.Create(character, character.GetCharacterInitiative());
            unitsList.Add(t);
        }
        
        return unitsList.OrderByDescending(x => x.Item2).ToList();
    }

    public int GetMyTurn(Character unit)
    {
        for (int i = 0; i < _currentTurnOrder.Count; i++)
        {
            if (_currentTurnOrder[i] == unit)
                return i;
        }

        return -1;
    }

    public Character[] GetAllUnits()
    {
        return _allUnits;
    }

    private void GreenUnitDied()
    {
        _greenDeadCount++;
        if (_greenDeadCount < _greenTeam.Count) return;
        FindObjectOfType<ChangeScene>().Defeat();
    }

    private void RedUnitDied()
    {
        _redDeadCount++;
        if (_redDeadCount < _redTeam.Count) return;
        FindObjectOfType<ChangeScene>().Win();
    }

    public void SetMortarAttack(bool state)
    {
        _mortarAttack = state;
    }
    
    public void Notify(string action)
    {
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
        for (int i = _observers.Count - 1; i >= 0; i--)
        {
            _observers[i].Notify(action);
        }
    }
}
