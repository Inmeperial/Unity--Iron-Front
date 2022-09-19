using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public Character CurrentTurnMecha { get => _currentTurnMecha; set => _currentTurnMecha = value; }
    public Character SelectedEnemy { get => _selectedEnemy; set => _selectedEnemy = value; }
    public EnumsClass.Team ActiveTeam => _activeTeam;

    public InputsReader InputsReader => _inputsReader;

    public GameCamerasController GameCamerasController => _gameCamerasController;

    [Header("References")]
    [SerializeField] private InputsReader _inputsReader;
    [SerializeField] private EquipmentManager _equipmentManager;
    [SerializeField] private GameCamerasController _gameCamerasController;
    [SerializeField] private CharacterSelector _characterSelector;
    [SerializeField] private GunsSelector _gunsSelector;
    [SerializeField] private PortraitsController _portraitsController;
    [SerializeField] private AttackHUD _attackHUD;
    //[SerializeField] private MechaEquipmentHUD _mechaEquipmentHUD;
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    

    [Header("Initializables")]
    [SerializeField] private Initializable[] _initializables;

    [Header("Configs")]
    [SerializeField] private float _attackDelay;
    [SerializeField] private float _endPanelDelay;

    private Character[] _mechas;

    private Character _currentTurnMecha;
    private Character _selectedEnemy;

    private EnumsClass.Team _activeTeam;
    private List<Character> _greenTeam = new List<Character>();
    private int _greenDeadCount;

    private List<Character> _redTeam = new List<Character>();
    private int _redDeadCount;

    private List<Character> _charactersInCurrentTurnOrder = new List<Character>();

    private bool _portraitStoppedMoving;

    #region Events
    public Action<Character> OnTurnMechaSelected;
    public Action OnMechaAttackPreparationsFinished;
    public Action<Character> OnMechaLegsAttacked;
    public Action OnEnemyMechaSelected;
    public Action OnEnemyMechaDeselected;

    public Action OnBeginAttackPreparations;
    public Action OnEndTurn;
    public Action OnBeginTurn;

    private Queue<Action> _endTurnActionsQueue = new Queue<Action>();
    private bool _currentActionFinished;
    #endregion
    private void Awake()
    {
        if (!_instance)
            _instance = this;
        else
            Destroy(gameObject);

        _mechas = FindObjectsOfType<Character>();

        SeparateMechasByTeam();

        _equipmentManager.Initialize();

        _attackHUD.OnAttackButtonClicked += BeginMechaAttackPreparations;

        _characterSelector.OnTurnMechaSelected += SetCurrentTurnMecha;
        _characterSelector.OnEnemyMechaSelected += EnemyMechaSelected;

        _portraitsController.OnPortraitStoppedMoving += OnPortraitStoppedMoving;

        _inputsReader.OnDeselectKeyPressed += EnableCharacterSelection;
    }

    private void Start()
    {
        foreach (Character mecha in _mechas)
        {
            _inputsReader.OnDeselectKeyPressed += mecha.ShowGunsMesh;
            _inputsReader.OnDeselectKeyPressed += mecha.ShowMechaMesh;

            OnBeginAttackPreparations += mecha.ShowMechaMesh;
            OnBeginAttackPreparations += mecha.ShowGunsMesh;

            mecha.OnMechaDeath += OnMechaDeath;
            mecha.OnBeginMove += _characterSelector.DisableCharacterSelection;
            mecha.OnEndMove += _characterSelector.EnableCharacterSelection;

            mecha.GetLegs().OnDamageTakenByAttack += OnMechaLegsDamaged;
            mecha.Initialize();
        }

        foreach (Initializable initializable in _initializables)
        {
            initializable.Initialize();
        }

        BeginFirstTurn();
    }

    private void SeparateMechasByTeam()
    {
        foreach (Character mecha in _mechas)
        {
            if (mecha.GetUnitTeam() == EnumsClass.Team.Green)
                _greenTeam.Add(mecha);
            else
                _redTeam.Add(mecha);
        }
    }

    public List<Character> GetEnemies(EnumsClass.Team unitTeam)
    {
        switch (unitTeam)
        {
            case EnumsClass.Team.Green:
                return _redTeam;

            case EnumsClass.Team.Red:
                return _greenTeam;

            default:
                return null;
        }
    }

    public Character[] GetMechas() => _mechas;

    private List<Tuple<Character, float>> GetOrderedUnits()
    {
        List<Tuple<Character, float>> unitsList = new List<Tuple<Character, float>>();

        //Adds them to a collection with their initiative
        foreach (Character character in _mechas)
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
                    orderedWithEnabled.Insert(orderedWithEnabled.Count - 1, tuple);
                else
                    orderedWithEnabled.Add(tuple);

                disabledCount++;
            }
        }
        return orderedWithEnabled;
    }
    private void BeginFirstTurn()
    {
        List<Tuple<Character, float>> orderedMechas = GetOrderedUnits();

        int count = 0;

        List<FramesUI> portraits = _portraitsController.GetPortraits();

        foreach (Tuple<Character, float> tuple in orderedMechas)
        {
            if (count == portraits.Count)
                return;

            Character character = tuple.Item1;

            character.transform.parent.name = character.GetCharacterName();

            FramesUI portrait = _portraitsController.SetPortrait(character, count, character.GetCharacterSprite(), character.GetCharacterName(), character.GetUnitTeam(),
                () => _gameCamerasController.CameraMovement.MoveTo(character.transform));

            count++;

            _charactersInCurrentTurnOrder.Add(character);

            _portraitsController.AddCharAndFrame(Tuple.Create(character, portrait));

            character.CheckWeight();

            if (!character.IsUnitEnabled())
                portrait.DisableFrame();
        }
        Character firstMecha = _charactersInCurrentTurnOrder[0];

        BeginTurn(firstMecha);
    }

    private void BeginMechaAttackPreparations()
    {
        _currentTurnMecha.SaveRotationBeforeLookingAtEnemy();
        _currentTurnMecha.RotateTowardsEnemy(_selectedEnemy.transform);

        OnBeginAttackPreparations?.Invoke();

        foreach (Character mecha in _mechas)
        {
            mecha.ShowGunsMesh();
            mecha.ShowMechaMesh();
        }

        Vector3 targetEnemyPosition = _selectedEnemy.transform.position;
        _gameCamerasController.CloseUpCamera.MoveCameraToParent(targetEnemyPosition, MechaAttackPreparationsFinished, _attackDelay);
    }

    private void MechaAttackPreparationsFinished()
    {       
        OnMechaAttackPreparationsFinished?.Invoke();

        EnableEndTurnButton();

        _gameCamerasController.CloseUpCamera.ResetCamera();
        _selectedEnemy.NotSelectedForAttack();

        _characterSelector.SelectionWithDelay(_currentTurnMecha);
        _characterSelector.EnableCharacterSelection();
    }

    private void SetCurrentTurnMecha(Character mecha)
    {
        if (_currentTurnMecha)
            _currentTurnMecha.DeselectThisUnit();

        if (_selectedEnemy)
            _selectedEnemy.DeselectThisUnit();

        _inputsReader.EnableKeysCheck();

        _gunsSelector.EnableGunSelection();

        _currentTurnMecha = mecha;

        _currentTurnMecha.SelectThisUnit();

        _activeTeam = mecha.GetUnitTeam();

        OnTurnMechaSelected?.Invoke(mecha);
    }

    private void OnCurrentTurnMechaDeselected()
    {
        if (!_currentTurnMecha)
            return;

        _currentTurnMecha.DeselectThisUnit();
        _currentTurnMecha = null;
    }
    private void EnemyMechaSelected(Character mecha)
    {
        if (_selectedEnemy)
        {
            _selectedEnemy.NotSelectedForAttack();
            _selectedEnemy.DeselectThisUnit();
        }

        _selectedEnemy = mecha;

        _selectedEnemy.SelectedAsEnemy();
        _selectedEnemy.SelectedForAttack();

        _characterSelector.DisableCharacterSelection();

        OnEnemyMechaSelected?.Invoke();

        DisableEndTurnButton();

        Action OnCloseUpFinished = () =>
        {
            HideNotInCombatMechasMesh();
            _currentTurnMecha.SelectingEnemy();
        };

        _gameCamerasController.CloseUpCamera.MoveCameraWithLerp(_selectedEnemy.transform.position, _currentTurnMecha.transform.position, OnCloseUpFinished);
    }

    private void EnemyMechaDeselected()
    {
        if (!_selectedEnemy)
            return;
        _attackHUD.HideAttackHUD();

        if (_currentTurnMecha)
            _currentTurnMecha.LoadRotationOnDeselect();

        Action OnCameraMovementFinished = () =>
        {
            OnEnemyMechaDeselected?.Invoke();
            _selectedEnemy.DeselectThisUnit();
            _selectedEnemy.NotSelectedForAttack();
            _selectedEnemy = null;

            _characterSelector.Selection(_currentTurnMecha);
        };

        _gameCamerasController.CloseUpCamera.MoveCameraToParent(_selectedEnemy.transform.position, OnCameraMovementFinished, _attackDelay);        
    }

    private void EnableCharacterSelection()
    {
        if (_activeTeam == EnumsClass.Team.Red)
            return;

        _characterSelector.EnableCharacterSelection();
    }

    private void HideNotInCombatMechasMesh()
    {
        foreach (Character mecha in _mechas)
        {
            if (mecha == _selectedEnemy)
                continue;

            if (mecha.IsDead())
                continue;

            mecha.HideMechaMesh();

            if (mecha != _currentTurnMecha)
                mecha.HideGunsMesh();
        }
    }

    private void OnMechaLegsDamaged(Character mecha)
    {
        ReducePositionInTurns(mecha);
    }

    private void ReducePositionInTurns(Character mecha)
    {
        int currentPos = GetMechaTurnPosition(mecha);

        if (currentPos < 0 || currentPos == _charactersInCurrentTurnOrder.Count - 1)
            return;
        
        StartCoroutine(ReducePosition(mecha, currentPos));
    }

    private IEnumerator ReducePosition(Character mecha, int currentPos)
    {
        //Revisar si hace falta descomentar los otros yield return
        yield return null;

        int newPos;

        _portraitStoppedMoving = false;

        WaitUntil waitUntilPortraitStoppedMoving = new WaitUntil(() => _portraitStoppedMoving);

        if (mecha.GetLegs().CurrentHP > 0)
        {
            if (mecha.IsOverweight())
            {
                newPos = currentPos + 2;

                if (newPos >= _charactersInCurrentTurnOrder.Count - 1)
                    newPos = _charactersInCurrentTurnOrder.Count - 1;
            }
            else
                newPos = currentPos + 1;

            _portraitsController.MovePortraitOfMechaFromTo(mecha, currentPos, newPos);

            //yield return waitUntilPortraitStoppedMoving;

            for (int i = newPos; i > currentPos; i--)
            {
                _portraitStoppedMoving = false;

                Character character = _charactersInCurrentTurnOrder[i];

                _portraitsController.MovePortraitOfMechaFromTo(character, i, i - 1);

                //yield return waitUntilPortraitStoppedMoving;
            }

            _charactersInCurrentTurnOrder.RemoveAt(newPos);

            _charactersInCurrentTurnOrder.Insert(newPos, mecha);
        }
        else
        {
            newPos = _portraitsController.PortraitsCount - 1;

            _portraitsController.MovePortraitOfMechaFromTo(mecha, currentPos, newPos);

            //yield return waitUntilPortraitStoppedMoving;

            _charactersInCurrentTurnOrder.RemoveAt(currentPos);

            _charactersInCurrentTurnOrder.Insert(newPos, mecha);

            for (int i = currentPos; i < newPos; i++)
            {
                _portraitStoppedMoving = false;

                Character character = _charactersInCurrentTurnOrder[i];

                _portraitsController.MovePortraitOfMechaFromTo(character, i + 1, i);

                //yield return waitUntilPortraitStoppedMoving;
            }
        }
    }

    private IEnumerator MoveCurrentMechaToLastPosition()
    {
        yield return null;

        _portraitsController.MovePortraitOfMechaFromTo(_currentTurnMecha, 0, _portraitsController.PortraitsCount - 1);

        _charactersInCurrentTurnOrder.RemoveAt(0);

        _charactersInCurrentTurnOrder.Add(_currentTurnMecha);

        //WaitUntil waitUntilPortraitStoppedMoving = new WaitUntil(() => _portraitStoppedMoving);

        for (int i = 0; i < _charactersInCurrentTurnOrder.Count-1; i++)
        {
            Character mecha = _charactersInCurrentTurnOrder[i];

            _portraitsController.MovePortraitOfMechaFromTo(mecha, i + 1, i);
        }
    }

    private void OnPortraitStoppedMoving() => _portraitStoppedMoving = true;

    private int GetMechaTurnPosition(Character mecha)
    {
        for (int i = 0; i < _charactersInCurrentTurnOrder.Count; i++)
        {
            if (_charactersInCurrentTurnOrder[i] != mecha)
                continue;
            
            return i;
        }
        return -1;
    }
    public bool IsActiveMecha(Character mecha) => _currentTurnMecha == mecha;

    public void EnableEndTurnButton() => _endTurnButton.gameObject.SetActive(true);
    public void DisableEndTurnButton() => _endTurnButton.gameObject.SetActive(false);

    public void BeginEndTurnProcess()
    {
        if (!_currentTurnMecha)
            return;

        DisableEndTurnButton();
        //_mechaEquipmentHUD.DisableButtonsInteraction();
        StartCoroutine(EndTurnActionsCheck());
    }

    private IEnumerator EndTurnActionsCheck()
    {
        if (_currentTurnMecha.GetUnitTeam() == EnumsClass.Team.Red)
        {
            EnemyCharacter enemy = _currentTurnMecha as EnemyCharacter;

            if (enemy)
                enemy.ForceEnd();
        }

        _currentTurnMecha.EndTurn();

        StartCoroutine(MoveCurrentMechaToLastPosition());

        TileHighlight.Instance.EndPreview();

        UnsubscribeFromInputs();

        OnEndTurn?.Invoke();

        StartCoroutine(EndTurnActionsExecutor());

        yield return new WaitUntil(() => _endTurnActionsQueue.Count <= 0);        

        Character newTurnMecha = _charactersInCurrentTurnOrder[0];
        
        while (newTurnMecha.IsDead() || !newTurnMecha.IsUnitEnabled())
        {
            yield return StartCoroutine(MoveCurrentMechaToLastPosition());

            newTurnMecha = _charactersInCurrentTurnOrder[0];
        }

        foreach (Character mecha in _charactersInCurrentTurnOrder)
        {
            mecha.SetTurn(false);
        }

        EnemyMechaDeselected();
        OnCurrentTurnMechaDeselected();

        BeginTurn(newTurnMecha);
    }

    private IEnumerator EndTurnActionsExecutor()
    {
        _currentActionFinished = false;

        if (_endTurnActionsQueue.Count > 0)
        {
            Action action = _endTurnActionsQueue.Dequeue();

            action?.Invoke();

            yield return new WaitUntil(() => _currentActionFinished);

            if (_endTurnActionsQueue.Count > 0)
                StartCoroutine(EndTurnActionsExecutor());
        }        
    }

    private void BeginTurn(Character mecha)
    {
        mecha.SetTurn(true);

        Action afterCameraMoves = () => _characterSelector.Selection(mecha);
        if (mecha.GetUnitTeam() == EnumsClass.Team.Green)
        {
            afterCameraMoves += () =>
            {
                _characterSelector.EnableCharacterSelection();
                SubscribeToInputs();
                _inputsReader.EnableKeysCheck();
                EnableEndTurnButton();
            };
        }
        else
        {
            _characterSelector.DisableCharacterSelection();
            _inputsReader.DisableKeysCheck();
            DisableEndTurnButton();            
        }

        afterCameraMoves += () => OnBeginTurn?.Invoke();

        _gameCamerasController.CameraMovement.MoveTo(mecha.transform, afterCameraMoves, mecha.transform);
    }

    private void OnMechaDeath(Character mecha)
    {
        _portraitsController.DisableDeadMechaPortrait(mecha);

        mecha.GetPositionTile().MakeTileFree();

        _inputsReader.OnDeselectKeyPressed -= mecha.ShowGunsMesh;
        _inputsReader.OnDeselectKeyPressed -= mecha.ShowMechaMesh;

        OnBeginAttackPreparations -= mecha.ShowMechaMesh;
        OnBeginAttackPreparations -= mecha.ShowGunsMesh;

        mecha.OnMechaDeath -= OnMechaDeath;
        mecha.OnBeginMove -= _characterSelector.DisableCharacterSelection;
        mecha.OnEndMove -= _characterSelector.EnableCharacterSelection;

        mecha.GetLegs().OnDamageTakenByAttack -= OnMechaLegsDamaged;

        if (mecha.GetUnitTeam() == EnumsClass.Team.Green)
            GreenUnitDied();
        else
            RedUnitDied();
    }
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

    private IEnumerator EndPanelDelayActivation(string panel)
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

    public void AddEndTurnAction(IEndActionNotifier notifier, Action action)
    {
        if (_endTurnActionsQueue.Contains(action))
            return;

        _endTurnActionsQueue.Enqueue(action);

        notifier.OnEndAction += ReceiveEndActionNotification;
    }

    private void ReceiveEndActionNotification() => _currentActionFinished = true;

    private void SubscribeToInputs()
    {
        _inputsReader.OnUndoKeyPressed += _currentTurnMecha.GetWaypointsPathfinding().UndoLastWaypoint;
        _inputsReader.OnDeselectKeyPressed += _currentTurnMecha.CancelEnemySelection;
        _inputsReader.OnDeselectKeyPressed += _currentTurnMecha.LoadRotationOnDeselect;
        _inputsReader.OnDeselectKeyPressed += _currentTurnMecha.ResetSelectedGun;
    }

    private void UnsubscribeFromInputs()
    {
        _inputsReader.OnUndoKeyPressed -= _currentTurnMecha.GetWaypointsPathfinding().UndoLastWaypoint;
        _inputsReader.OnDeselectKeyPressed -= _currentTurnMecha.CancelEnemySelection;
        _inputsReader.OnDeselectKeyPressed -= _currentTurnMecha.LoadRotationOnDeselect;
        _inputsReader.OnDeselectKeyPressed -= _currentTurnMecha.ResetSelectedGun;
    }

    private void OnDestroy()
    {
        _attackHUD.OnAttackButtonClicked -= BeginMechaAttackPreparations;

        _characterSelector.OnTurnMechaSelected -= SetCurrentTurnMecha;
        _characterSelector.OnEnemyMechaSelected -= EnemyMechaSelected;

        foreach (Character mecha in _mechas)
        {
            _inputsReader.OnDeselectKeyPressed -= mecha.ShowGunsMesh;
            _inputsReader.OnDeselectKeyPressed -= mecha.ShowMechaMesh;

            mecha.OnMechaDeath -= OnMechaDeath;

            mecha.OnBeginMove -= _characterSelector.DisableCharacterSelection;
            mecha.OnEndMove -= _characterSelector.EnableCharacterSelection;

            mecha.GetLegs().OnDamageTakenByAttack -= OnMechaLegsDamaged;
        }
    }
}
