using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TurnManager : Initializable
{
    [Header("Inputs")]
    [SerializeField] private InputsReader _inputsReader;

    [Header("References")]
    [SerializeField] private PortraitsController _portraitsController;

    //[SerializeField] private GameObject _winPanel;
    //[SerializeField] private GameObject _losePanel;
    //[SerializeField] private float _endPanelDelay;

    //private List<Character> _greenTeam = new List<Character>();
    //private int _greenDeadCount;

    //private List<Character> _redTeam = new List<Character>();
    //private int _redDeadCount;

    private Character[] _allUnits;

    private EnumsClass.Team _activeTeam;

    [Header("Turns")]
	[SerializeField] private List<Character> _charactersInCurrentTurnOrder = new List<Character>();
    
    //private List<IObserver> _observers = new List<IObserver>();

    private Character _actualCharacter;
    private CameraMovement _cameraMovement;

    //[Header("Mortar Turn")]
    //public float waitForMortarAttack;
    //private bool _mortarAttack;

    //private delegate void Execute();

    //Dictionary<string, Execute> _actionsDic = new Dictionary<string, Execute>();

    //public static TurnManager Instance;

    //private void Awake()
    //{
    //    _cameraMovement = FindObjectOfType<CameraMovement>();
        
    //    if (Instance != null)
    //    {
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        Instance = this;
    //        //DontDestroyOnLoad(gameObject);
    //    }
    //}

    public override void Initialize()
    {
    //    //GameManager.Instance.OnEndTurn += EndTurn;

    //    //_allUnits = GameManager.Instance.GetMechas();

    //    //SeparateByTeam(_allUnits);
    //    //_activeTeam = Team.Green;

    //    //Mortar[] mortars = FindObjectsOfType<Mortar>();

    //    //foreach (Mortar mortar in mortars)
    //    //{
    //    //    Subscribe(mortar);
    //    //}

    //    //Elevator[] elevators = FindObjectsOfType<Elevator>();

    //    //foreach (Elevator elevator in elevators)
    //    //{
    //    //    Subscribe(elevator);
    //    //}

    //    //SetFirstTurn();

    //    //_actualCharacter = _charactersInCurrentTurnOrder[0];

    //    //_activeTeam = _actualCharacter.GetUnitTeam();
        
    //    //_actionsDic.Add("GreenDead", GreenUnitDied);
    //    //_actionsDic.Add("RedDead", RedUnitDied);
        
        
    //    Action toDo = () =>
    //    {
    //        _actualCharacter.SetTurn(true);
    //        CharacterSelector.Instance.Selection(_actualCharacter);            
    //    };
    //    if (_activeTeam == EnumsClass.Team.Green)
    //    {
    //        CharacterSelector.Instance.EnableCharacterSelection();

    //        toDo += () => GameManager.Instance.EnableEndTurnButton();
    //        toDo += () =>
    //        {
    //            SubscribeToInputs();
    //            _inputsReader.EnableKeysCheck();
    //        };
    //    }
    //    else
    //    {
    //        CharacterSelector.Instance.DisableCharacterSelection();

    //        GameManager.Instance.DisableEndTurnButton();

    //        _inputsReader.DisableKeysCheck();
    //    }
        

    //    _cameraMovement.MoveTo(_actualCharacter.transform, toDo, _actualCharacter.transform);
    }


    //public void UnitIsMoving() => CharacterSelector.Instance.DeactivateCharacterSelection();

    //public void UnitStoppedMoving() => CharacterSelector.Instance.ActivateCharacterSelection();

    //private void SeparateByTeam(Character[] units)
    //{
    //    foreach (Character unit in units)
    //    {
    //        if (unit.GetUnitTeam() == EnumsClass.Team.Green)
    //            _greenTeam.Add(unit);
    //        else
    //            _redTeam.Add(unit);
    //    }
    //}

    //public List<Character> GetEnemies(EnumsClass.Team myTeam) => myTeam == EnumsClass.Team.Green ? _redTeam : _greenTeam;

    public void EndTurn()
    {
        //if (_actualCharacter == null)
        //    return;
        
        //if (_actualCharacter.IsMoving())
        //    return;

        //if (_actualCharacter.GetUnitTeam() == EnumsClass.Team.Red)
        //{
        //    EnemyCharacter enemy = _actualCharacter as EnemyCharacter;

        //    if (enemy)
        //        enemy.ForceEnd();
        //}

        //CharacterSelector.Instance.ResetSelector();

        //NotifyObserver("EndTurn");

        //NotifyObserver("Deselect");
        
        //StartCoroutine(OnEndTurn());
    }

    //private IEnumerator OnEndTurn()
    //{
    //    UnsubscribeFromInputs();

    //    //if (_mortarAttack)
    //    //{
    //    //    yield return new WaitUntil(() => !_mortarAttack);
    //    //    yield return new WaitForSeconds(waitForMortarAttack);
    //    //}
        
    //    //MoveToLast();

    //    //ResetTurn(_actualCharacter);

    //    //_actualCharacter = _charactersInCurrentTurnOrder[0];

    //    //while (_actualCharacter.GetBody().CurrentHP <= 0 || !_actualCharacter.IsUnitEnabled())
    //    //{
    //    //    MoveToLast();

    //    //    yield return new WaitForSeconds(1);

    //    //    _actualCharacter = _charactersInCurrentTurnOrder[0];
    //    //}


    //    Action toDo = () => CharacterSelector.Instance.Selection(_actualCharacter);

    //    _actualCharacter.SetTurn(true);

    //    _activeTeam = _actualCharacter.GetUnitTeam();

    //    if (_activeTeam == EnumsClass.Team.Green)
    //    {
    //        CharacterSelector.Instance.ActivateCharacterSelection();

    //        //TODO: CAMBIAR AL EQUIPMENT HUD
    //        //ButtonsUIManager.Instance.RightWeaponCircleState(true);
    //        //ButtonsUIManager.Instance.LeftWeaponCircleState(true);
    //        //ButtonsUIManager.Instance.RightWeaponBarButtonState(true);
    //        //ButtonsUIManager.Instance.LeftWeaponBarButtonState(true);

    //        toDo += () => GameManager.Instance.EnableEndTurnButton();
    //        toDo += () =>
    //        {
    //            SubscribeToInputs();
    //            _inputsReader.EnableKeysCheck();
    //        };
    //    }
    //    else
    //    {
    //        CharacterSelector.Instance.DeactivateCharacterSelection();

    //        GameManager.Instance.DisableEndTurnButton();

    //        _inputsReader.DisableKeysCheck();
    //        //TODO: CAMBIAR AL EQUIPMENT HUD
    //        //ButtonsUIManager.Instance.RightWeaponCircleState(false);
    //        //ButtonsUIManager.Instance.LeftWeaponCircleState(false);
    //        //ButtonsUIManager.Instance.RightWeaponBarButtonState(false);
    //        //ButtonsUIManager.Instance.LeftWeaponBarButtonState(false);
    //    }

    //    _cameraMovement.MoveTo(_actualCharacter.transform, toDo, _actualCharacter.transform);
    //}

    //private void MoveToLast()
    //{
    //    List<FramesUI> portraits = _portraitsController.GetPortraits();

    //    GameManager.Instance.DisableEndTurnButton();

    //    _portraitsController.MovePortraitOfMechaFromTo(_actualCharacter, 0, _portraitsController.PortraitsCount - 1);

    //    _charactersInCurrentTurnOrder.RemoveAt(0);

    //    _charactersInCurrentTurnOrder.Insert(_charactersInCurrentTurnOrder.Count, _actualCharacter);
        
    //    Dictionary<Character, int> previous = new Dictionary<Character, int>();

    //    for (int i = 0; i < _charactersInCurrentTurnOrder.Count; i++)
    //    {
    //        Character character = _charactersInCurrentTurnOrder[i];
    //        previous.Add(character, i);
    //    }
        
    //    for (int i = 0; i < portraits.Count-1; i++)
    //    {
    //        Character character = _charactersInCurrentTurnOrder[i];

    //        _portraitsController.MovePortraitOfMechaFromTo(character, previous[character], i);
    //    }
    //}

    /// <summary>
    /// Reset stats for new turn of the given unit.
    /// </summary>
    //private void ResetTurn(Character unit)
    //{
    //    TileHighlight.Instance.EndPreview();
    //    unit.NewTurn();
    //}

    //public EnumsClass.Team GetActiveTeam() => _activeTeam;

    //public void UnitCanBeAttacked(Character unit) => unit.MechaInAttackRange();

    //public void UnitCantBeAttacked(Character unit) => unit.MechaOutsideAttackRange();

    //public Tile GetUnitTile(Character unit) => unit.GetMyPositionTile();

    //private void SetFirstTurn()
    //{
    //    List<Tuple<Character,float>> orderedUnits = GetOrderedUnits();

    //    int count = 0;

    //    List<FramesUI> portraits = _portraitsController.GetPortraits();

    //    foreach (Tuple<Character, float> tuple in orderedUnits)
    //    {
    //        if (count == portraits.Count)
    //            return;
            
    //        Character character = tuple.Item1;

    //        character.transform.parent.name = character.GetCharacterName();
            
    //        FramesUI portrait = _portraitsController.SetPortrait(character, count, character.GetCharacterSprite(), character.GetCharacterName(), character.GetUnitTeam(),
    //            () => _cameraMovement.MoveTo(character.transform));

    //        count++;

    //        _charactersInCurrentTurnOrder.Add(character);

    //        _portraitsController.AddCharAndFrame(Tuple.Create(character,portrait));

    //        character.CheckWeight();

    //        if (!character.IsUnitEnabled())
    //            portrait.DisableFrame();              
    //    }
    //}

    //public void ReducePosition(Character unit)
    //{
    //    int oldPos = GetMyTurn(unit);
        
    //    //If last, return
    //    if (oldPos == _charactersInCurrentTurnOrder.Count -1)
    //        return;

    //    //Move unit portrait one position behind
    //    int newPos;

    //    if (unit.GetLegs().CurrentHP > 0)
    //    {
    //        if (unit.IsOverweight())
    //        {
    //            newPos = oldPos + 3;

    //            if (newPos >= _charactersInCurrentTurnOrder.Count - 1)
    //                newPos = _charactersInCurrentTurnOrder.Count - 1;
    //        }
    //        else
    //            newPos = oldPos + 1;

    //        _portraitsController.MovePortraitOfMechaFromTo(unit, oldPos, newPos);

    //        //var othersTurns = newPos - oldPos;

    //        for (int i = newPos; i > oldPos; i--)
    //        {
    //            _portraitsController.MovePortraitOfMechaFromTo(_charactersInCurrentTurnOrder[i], i, i-1);
    //        }

    //        Character other = _charactersInCurrentTurnOrder[newPos];

    //        //PortraitsController.Instance.MovePortrait(other, newPos, oldPos);
    //        _charactersInCurrentTurnOrder.RemoveAt(oldPos);

    //        _charactersInCurrentTurnOrder.Insert(newPos, unit);
    //    }
    //    else
    //    {
    //        newPos = _portraitsController.PortraitsCount - 1;

    //        _portraitsController.MovePortraitOfMechaFromTo(unit, oldPos, newPos);

    //        _charactersInCurrentTurnOrder.RemoveAt(oldPos);

    //        _charactersInCurrentTurnOrder.Insert(newPos, unit);

    //        for (int i = oldPos; i < newPos; i++)
    //        {
    //            Character u = _charactersInCurrentTurnOrder[i];

    //            _portraitsController.MovePortraitOfMechaFromTo(u, i+1, i);
    //        }
    //    } 
    //}

    //private List<Tuple<Character, float>> GetOrderedUnits()
    //{
    //    List<Tuple<Character, float>> unitsList = new List<Tuple<Character, float>>();
        
    //    //Adds them to a collection with their initiative
    //    foreach (Character character in _allUnits)
    //    {
    //        Tuple<Character, float> tuple = Tuple.Create(character, character.GetCharacterInitiative());

    //        unitsList.Add(tuple);
    //    }

    //    List<Tuple<Character, float>> orderedUnits = unitsList.OrderByDescending(x => x.Item2).ToList();

    //    List<Tuple<Character, float>> orderedWithEnabled = new List<Tuple<Character, float>>();

    //    int disabledCount = 0;

    //    foreach (Tuple<Character, float> tuple in orderedUnits)
    //    {
    //        if (tuple.Item1.IsUnitEnabled())
    //        {
    //            if (disabledCount == 0)
    //                orderedWithEnabled.Add(tuple);
    //            else 
    //                orderedWithEnabled.Insert(orderedWithEnabled.Count - disabledCount, tuple);
    //        }
                
    //        else
    //        {
    //            if (orderedWithEnabled.Count > 1)
    //                orderedWithEnabled.Insert(orderedWithEnabled.Count-1, tuple);
    //            else
    //                orderedWithEnabled.Add(tuple);

    //            disabledCount++;
    //        }
    //    }
    //    return orderedWithEnabled;
    //}

    //public int GetMyTurn(Character unit)
    //{
    //    for (int i = 0; i < _charactersInCurrentTurnOrder.Count; i++)
    //    {
    //        if (_charactersInCurrentTurnOrder[i] == unit)
    //            return i;
    //    }

    //    return -1;
    //}

    //public Character[] GetAllUnits() => _allUnits;

    //private void GreenUnitDied()
    //{
    //    _greenDeadCount++;

    //    if (_greenDeadCount < _greenTeam.Count)
    //        return;

    //    StartCoroutine(EndPanelDelayActivation("Lose"));
    //}

    //private void RedUnitDied()
    //{
    //    _redDeadCount++;

    //    if (_redDeadCount < _redTeam.Count) 
    //        return;

    //    StartCoroutine(EndPanelDelayActivation("Win"));
    //}

    //private IEnumerator EndPanelDelayActivation(string panel)
    //{
    //    yield return new WaitForSeconds(_endPanelDelay);

    //    foreach (EnemyCharacter unit in _redTeam)
    //    {
    //        unit.DisableUnit();
    //    }
        
    //    if (panel == "Win")
    //        _winPanel.SetActive(true);

    //    if (panel == "Lose")
    //        _losePanel.SetActive(true);
    //}

    //public void SetMortarAttack(bool state) => _mortarAttack = state;

    //private void SubscribeToInputs()
    //{
    //    _inputsReader.OnUndoKeyPressed += _actualCharacter.GetWaypointsPathfinding().UndoLastWaypoint;
    //    _inputsReader.OnDeselectKeyPressed += _actualCharacter.CancelEnemySelection;
    //    _inputsReader.OnDeselectKeyPressed += _actualCharacter.ResetRotationAndRays;
    //    _inputsReader.OnDeselectKeyPressed += _actualCharacter.ResetSelectedGun;
    //}

    //private void UnsubscribeFromInputs()
    //{
    //    _inputsReader.OnUndoKeyPressed -= _actualCharacter.GetWaypointsPathfinding().UndoLastWaypoint;
    //    _inputsReader.OnDeselectKeyPressed -= _actualCharacter.CancelEnemySelection;
    //    _inputsReader.OnDeselectKeyPressed -= _actualCharacter.ResetRotationAndRays;
    //    _inputsReader.OnDeselectKeyPressed -= _actualCharacter.ResetSelectedGun;
    //}
}
