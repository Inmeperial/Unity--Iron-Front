using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;

[SelectionBase]
public class Character : EnumsClass, IObservable
{
    private Equipable _equipable;

    private Dictionary<PartsMechaEnum, GameObject> _partsDictionary = new Dictionary<PartsMechaEnum, GameObject>();

    [SerializeField] protected Transform _raycastToTile;
    //STATS
    
    [SerializeField] protected MechaEquipmentSO _mechaEquipment;
    public bool gunsOffOnCloseUp;
    
    [Header("Team")]
    [SerializeField] protected Team _unitTeam;
    [SerializeField] protected Sprite _myIcon;
    
    protected string _myName;

    [Header("Rays")] 
    
    [SerializeField] protected LineRenderer _rayForBody;
    [SerializeField] protected LineRenderer _rayForLeftArm;
    [SerializeField] protected LineRenderer _rayForRightArm;
    [SerializeField] protected LineRenderer _rayForLegs;
    [SerializeField] protected Material _rayHitMaterial;
    [SerializeField] protected Material _rayMissMaterial;
    [SerializeField] protected float _raysOffDelay;

    #region Parts
    [Header("Parts Spawns")]
    //Body
    [SerializeField] protected Transform _bodySpawnPosition;
    protected Body _body;
    protected Transform _bodyTransform;

    //Left Arm
    [SerializeField] protected Transform _leftArmSpawnPosition;
    //protected Arm _leftArm;
    protected Gun _leftGun;
    protected bool _leftGunSelected;
    [SerializeField] protected GameObject _leftGunSpawn;

    //Right Arm
    [SerializeField] protected Transform _rightArmSpawnPosition;
    //protected Arm _rightArm;
    protected Gun _rightGun;
    protected bool _rightGunSelected;
    [SerializeField] protected GameObject _rightGunSpawn;

    //Legs
    [SerializeField] protected Transform _leftLegSpawnPosition;
    [SerializeField] protected Transform _rightLegSpawnPosition;
    protected Legs _legs;
    protected Transform _legsTransform;
    protected int _currentSteps;

    #endregion

    [Header("Others")]
    protected Gun _selectedGun;

    //MOVEMENT RELATED
    public IPathCreator pathCreator;
    protected GridMovement _move;
    public LayerMask block;
    protected List<Tile> _tilesInMoveRange = new List<Tile>();
    protected Tile _myPositionTile;
    protected Tile _targetTile;
    protected List<Tile> _path = new List<Tile>();
    public Quaternion InitialRotation { get; private set; } //Cambio Nico
    public Quaternion RotationBeforeAttack { get; private set; }


    #region  Flags
    protected bool _canBeSelected;
    protected bool _selected;
    protected bool _moving = false;
    protected bool _canMove = true;
    protected bool _canAttack = true;
    protected bool _leftGunAlive;
    protected bool _rightGunAlive;
    protected bool _canBeAttacked = false;
    protected bool _selectingEnemy = false;
    protected bool _selectedForAttack;
    protected bool _myTurn = false;
    protected bool _isDead = false;
    protected bool _equipableSelected;
    protected bool _rotated;

    protected bool _overweight;
    
    public delegate void Overweight(bool state);
    public event Overweight OnOverweight;
    
    protected bool _legsOvercharged;

    protected bool _isOnElevator;
    #endregion

    //OTHERS
    public List<GameObject> bodyRenderContainer = new List<GameObject>();
    protected HashSet<Tile> _tilesInAttackRange = new HashSet<Tile>();
    protected Dictionary<Tile, int> _tilesForAttackChecked = new Dictionary<Tile, int>();
    protected Dictionary<Tile, int> _tilesForMoveChecked = new Dictionary<Tile, int>();
    protected List<Character> _enemiesInRange = new List<Character>();
    protected WorldUI _myUI;
    protected bool _worldUIToggled;
    public AudioClip soundHit;

    protected MaterialMechaHandler _materialMechaHandler;
    protected ParticleMechaHandler _particleMechaHandler;
    protected AnimationMechaHandler _animationMechaHandler;
    protected AudioMechaHandler _audioMechaHandler;

    protected List<IObserver> _observers = new List<IObserver>();

    [HideInInspector] public TileHighlight highlight;
    
    protected List<Equipable> _equipables = new List<Equipable>();
    
    public virtual void ManualAwake()
    {
        transform.position = new Vector3(transform.position.x, 4f, transform.position.z);

        #region GetComponents

        _materialMechaHandler = GetComponent<MaterialMechaHandler>();

        // switch (_unitTeam)
        // {
        //     case Team.Green:
        //         _materialMechaHandler.SetHandlerMaterial(_mechaEquipment.body.playerMaterial, _mechaEquipment.leftArm.playerMaterial, _mechaEquipment.legs.playerMaterial);
        //         break;
        //     case Team.Red:
        //         _materialMechaHandler.SetHandlerMaterial(_mechaEquipment.body.enemyMaterial, _mechaEquipment.leftArm.enemyMaterial, _mechaEquipment.legs.enemyMaterial);
        //         break;
        // }

        _partsDictionary = GetComponent<GetPartsOfMecha>().GetPartsObj();

        _particleMechaHandler = GetComponent<ParticleMechaHandler>();
        _animationMechaHandler = GetComponent<AnimationMechaHandler>();
        _audioMechaHandler = GetComponent<AudioMechaHandler>();
        _move = GetComponent<GridMovement>();
        pathCreator = GetComponent<IPathCreator>();
        _myUI = GetComponent<WorldUI>();

        #endregion

        highlight = FindObjectOfType<TileHighlight>();
    }

    // Start is called before the first frame update
    public virtual void ManualStart()
    {
        ConfigureMecha();
        Subscribe(TurnManager.Instance);
        _canMove = _legs.GetCurrentHp() > 0;
        _currentSteps = _canMove ? _legs.GetMaxSteps() : 0;

        _myPositionTile = GetTileBelow();
        _myPositionTile.MakeTileOccupied();
        _myPositionTile.SetUnitAbove(this);
        _move.SetMoveSpeed(_legs.GetMoveSpeed());
        _move.SetRotationSpeed(_legs.GetRotationSpeed());
        
        _myUI.SetLimits(_body.GetMaxHp(), _rightGun.GetMaxHp(), _leftGun.GetMaxHp(), _legs.GetMaxHp());

        _selected = false;
        _canBeSelected = _body.GetCurrentHp() > 0;

        _bodyTransform = _body.transform;
        _legsTransform = _raycastToTile;

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_isDead) return;
        
        if (_unitTeam == Team.Red) return;

        if (!_isOnElevator)
        {
            if (_selected && !_moving && _canMove && !_selectingEnemy && Input.GetMouseButtonDown(0))
            {
                GetTargetToMove();
            }
        }
        

        if (!_selected && _equipables.Count > 0 && _equipableSelected)
        {
            _equipable.Use(OnUseEquipable);
        }
    }


    #region Actions

    protected void Move()
    {
        _moving = true;
        ButtonsUIManager.Instance.DeactivateBodyPartsContainer();
        TurnManager.Instance.UnitIsMoving();
        highlight.characterMoving = true;
        if (_myPositionTile)
        {
            _myPositionTile.unitAboveSelected = false;
            _myPositionTile.EndMouseOverColor();
        }

        ResetTilesInMoveRange();
        ResetTilesInAttackRange();
        _animationMechaHandler.SetIsWalkingAnimatorTrue();
        _audioMechaHandler.SetPlayMotorStart();
        _particleMechaHandler.SetMachineOn(true);
        _move.StartMovement(_path);
    }

    /// <summary>
    /// Creates shoot particle at selected gun position.
    /// </summary>
    public void Shoot()
    {
        if (_rightGunSelected)
        {
            switch (_rightGun.GetGunType())
            {
                case GunsType.None:
                    break;
                case GunsType.AssaultRifle:
                    _animationMechaHandler.SetIsMachineGunAttackRightAnimatorTrue();
                    break;
                case GunsType.Melee:
                    _animationMechaHandler.SetIsHammerAttackRightAnimatorTrue();
                    break;
                case GunsType.Rifle:
                    _animationMechaHandler.SetIsSniperAttackRightAnimatorTrue();
                    break;
                case GunsType.Shield:
                    break;
                case GunsType.Shotgun:
                    _animationMechaHandler.SetIsShotgunAttackRightAnimatorTrue();
                    break;
            }
        }
        else if (_leftGunSelected)
        {
            switch (_leftGun.GetGunType())
            {
                case GunsType.None:
                    break;
                case GunsType.AssaultRifle:
                    _animationMechaHandler.SetIsMachineGunAttackLeftAnimatorTrue();
                    break;
                case GunsType.Melee:
                    _animationMechaHandler.SetIsHammerAttackLeftAnimatorTrue();
                    break;
                case GunsType.Rifle:
                    _animationMechaHandler.SetIsSniperAttackLeftAnimatorTrue();
                    break;
                case GunsType.Shield:
                    break;
                case GunsType.Shotgun:
                    _animationMechaHandler.SetIsShotgunAttackLeftAnimatorTrue();
                    break;
            }
        }
    }

    /// <summary>
    /// Select Left Gun and repaint tiles.
    /// </summary>
    public void SelectLeftGun()
    {
        if (!_leftGun) return;
        
        if (_rightGun) _rightGun.Deselect();
        
        foreach (var e in _enemiesInRange)
        {
            TurnManager.Instance.UnitCantBeAttacked(e);
        }
        _enemiesInRange.Clear();
        
        _selectedGun = _leftGun;
        _leftGunSelected = true;
        _rightGunSelected = false;
        ResetTilesInAttackRange();
        ResetTilesInMoveRange();

        if (_selectedGun.GetGunType() != GunsType.Shield)
        {
            if (_canAttack)
            {
                if (!_isOnElevator || _isOnElevator && _selectedGun.GetAttackRange() > 1)
                {
                    PaintTilesInAttackRange(_path.Count == 0 ? _myPositionTile : _path[_path.Count - 1], 0);
                    CheckEnemiesInAttackRange(); 
                }
            }
        }
        else
        {
            if (!_isOnElevator && _canAttack)
            {
                _selectedGun.Ability();
            }
        }

        if (!_isOnElevator && _canMove)
        {
            PaintTilesInMoveRange(_path.Count == 0 ? _myPositionTile : _path[_path.Count - 1], 0);
        }
    }

    /// <summary>
    /// Select Right Gun and repaint tiles.
    /// </summary>
    public void SelectRightGun()
    {
        if (!_rightGun) return;
        
        if (_leftGun) _leftGun.Deselect();

        foreach (var e in _enemiesInRange)
        {
            TurnManager.Instance.UnitCantBeAttacked(e);
        }
        _enemiesInRange.Clear();
        
        ResetRotationAndRays();
        
        _selectedGun = _rightGun;
        _leftGunSelected = false;
        _rightGunSelected = true;
        ResetTilesInAttackRange();
        ResetTilesInMoveRange();

        if (_selectedGun.GetGunType() != GunsType.Shield)
        {
            if (_canAttack)
            {
                if (!_isOnElevator || _isOnElevator && _selectedGun.GetAttackRange() > 1)
                {
                    PaintTilesInAttackRange(_path.Count == 0 ? _myPositionTile : _path[_path.Count - 1], 0);
                    CheckEnemiesInAttackRange(); 
                }
            }
        }
        else
        {
            if (!_isOnElevator && _canAttack)
            {
                _selectedGun.Ability();
            }
        }

        if (!_isOnElevator && _canMove)
        {
            PaintTilesInMoveRange(_path.Count == 0 ? _myPositionTile : _path[_path.Count - 1], 0);
        }
    }

    public void Undo()
    {
        ResetInRangeLists();
        GetPath();

        if (_path.Count > 1)
        {
            _targetTile = _path[_path.Count - 1];
            PaintTilesInMoveRange(_targetTile, 0);
            highlight.PaintLastTileInPath(_targetTile);
            if (CanAttack())
                PaintTilesInAttackRange(_targetTile, 0);
            highlight.PaintLastTileInPath(_targetTile);
        }
        else
        {
            _targetTile = null;
            pathCreator.ResetPath();
            PaintTilesInMoveRange(_myPositionTile, 0);
            if (CanAttack())
                PaintTilesInAttackRange(_myPositionTile, 0);
        }
    }

    /// <summary>
    /// Select Character and paint tiles.
    /// </summary>
    public virtual void SelectThisUnit()
    {
        if (_isDead) return;


        _selected = true;
        InitialRotation = transform.rotation; //Cambio Nico
        ResetInRangeLists();
        _path.Clear();
        highlight.PathLinesClear();
        _targetTile = null;
        _myPositionTile = GetTileBelow();
        if (_myPositionTile)
        {
            _myPositionTile.unitAboveSelected = true;
            _myPositionTile.MakeTileFree();
            _myPositionTile.GetComponent<TileMaterialhandler>().DiseableAndEnableSelectedNode(true);
        }

        if (_canAttack)
        {
            if (_rightGunAlive && _rightGun) _selectedGun = _rightGun;
            
            else if (_leftGunAlive && _leftGun) _selectedGun = _leftGun;
            
            else _selectedGun = null;

            if (!_isOnElevator || _isOnElevator && _selectedGun.GetAttackRange() > 1)
            {
                PaintTilesInAttackRange(_myPositionTile, 0);
                CheckEnemiesInAttackRange();
            }
        }
        
        if (!_isOnElevator && _canMove)
        {
            if (!_legsOvercharged)
                _currentSteps = _legs.GetCurrentHp() > 0 ? _legs.GetMaxSteps() : _legs.GetMaxSteps()/2;
            else _currentSteps = _legs.GetMaxSteps() * 2;
            
            PaintTilesInMoveRange(_myPositionTile, 0);
            AddTilesInMoveRange();
        }
    }

    /// <summary>
    /// Deselect Character and clear tiles.
    /// </summary>
    public void DeselectThisUnit()
    {
        _selected = false;
        _selectingEnemy = false;
        _myPositionTile = GetTileBelow();
        if (_myPositionTile)
        {
            _myPositionTile.MakeTileOccupied();
            _myPositionTile.unitAboveSelected = false;
            _myPositionTile.EndMouseOverColor();
        }

        foreach (Character item in _enemiesInRange)
        {
            TurnManager.Instance.UnitCantBeAttacked(item);
        }

        // if (_canMove)
        //     _currentSteps = legs.GetMaxSteps();
        ResetInRangeLists();
        _path.Clear();
        highlight.PathLinesClear();
        pathCreator.ResetPath();
    }

    /// <summary>
    /// Rotate Character towards enemy.
    /// </summary>
    public void RotateTowardsEnemy(Transform t, float height)
    {
        // _move.SetPosToRotate(pos);
        // _move.StartRotation();
        var pos = transform.position;
        pos.y = height;
        transform.LookAt(pos);
    }

    /// <summary>
    /// Rotate Character towards enemy and execute callback when finished.
    /// </summary>
    public void RotateTowardsEnemy(Vector3 pos, Action callback)
    {
        _move.SetPosToRotate(pos);
        _move.StartRotation(callback);
    }

    /// <summary>
    /// Cast a ray to given position. Returns true if collided tag is the same as given tag, false if not.
    /// </summary>
    public bool RayToPartsForAttack(Vector3 partPosition, string tagToCheck, bool drawRays)
    {
        if (partPosition == Vector3.zero) return false;
        
        Vector3 position = _rayForBody.gameObject.transform.position;
        Vector3 dir = (partPosition - position).normalized;

        Physics.Raycast(position, dir, out RaycastHit hit, 1000f);
        Transform hitObj = hit.collider.transform;
        //bool goodHit = hitObj.gameObject.CompareTag(tagToCheck) && hitObj.position == partPosition;
        bool goodHit = hitObj.gameObject.CompareTag(tagToCheck);

        LineRenderer renderer = null;
        switch (tagToCheck)
        {
            case "Body":
                renderer = _rayForBody;
                break;

            case "Legs":
                renderer = _rayForLegs;
                break;

            case "RGun":
                renderer = _rayForRightArm;
                break;

            case "LGun":
                renderer = _rayForLeftArm;
                break;
        }

        renderer.positionCount = 2;
        renderer.SetPosition(0, position);
        renderer.SetPosition(1, partPosition);

        if (goodHit)
        {
            if (drawRays)
            {
                renderer.material = _rayHitMaterial;
                Debug.DrawRay(position, dir * 20f, Color.green, 10f); 
            }
            
            return true;
        }

        if (drawRays)
        {
            renderer.material = _rayMissMaterial;
            Debug.DrawRay(position, dir * 20f, Color.red, 10f); 
        }
        
        return false;

    }

    private void RaysOff()
    {
        _rayForBody.positionCount = 0;
        _rayForBody.materials[0] = null;
        _rayForLegs.positionCount = 0;
        _rayForLegs.materials[0] = null;
        _rayForLeftArm.positionCount = 0;
        _rayForLeftArm.materials[0] = null;
        _rayForRightArm.positionCount = 0;
        _rayForRightArm.materials[0] = null;
    }

    public void RaysOffDelay()
    {
        StartCoroutine(RaysOffWithDelay());
    }

    IEnumerator RaysOffWithDelay()
    {
        yield return new WaitForSeconds(_raysOffDelay);
        RaysOff();
    }

    /// <summary>
    /// Set legs overcharge status to true
    /// </summary>
    public void LegsOverchargeActivate()
    {
        _legsOvercharged = true;
    }

    /// <summary>
    /// Set legs overcharge status to false
    /// </summary>
    public void LegsOverchargeDeactivate()
    {
        _legsOvercharged = false;
    }

    public void AddEquipable(Equipable equipable)
    {
        _equipables.Add(equipable);
    }
    #endregion

    #region Getters

    public Body GetBody()
    {
        return _body;
    }

    // public Arm GetLeftArm()
    // {
    //     return _leftArm;
    // }
    //
    // public Arm GetRightArm()
    // {
    //     return _rightArm;
    // }

    public Legs GetLegs()
    {
        return _legs;
    }

    public bool AreLegsOvercharged()
    {
        return _legsOvercharged;
    }
    private void GetTargetToMove()
    {
        Transform target = MouseRay.GetTargetTransform(block);

        if (!IsValidBlock(target)) return;

        Tile newTile = target.GetComponent<Tile>();

        if (_targetTile && _targetTile == newTile && _path.Count > 0)
        {
            Move();
        }
        else
        {

            pathCreator.Calculate(_myPositionTile, newTile, _currentSteps);
            
            if (!_legsOvercharged)
                if (pathCreator.GetDistance() > _legs.GetMaxSteps()) return;
                else if (pathCreator.GetDistance() > _legs.GetMaxSteps() * 2) return;

            _path = pathCreator.GetPath();
            
            if (_path.Count <= 0) return;
            
            if (_targetTile) highlight.EndLastTileInPath(_targetTile);

            _targetTile = newTile;

            highlight.PathPreview(_path);
            ResetTilesInMoveRange();
            ResetTilesInAttackRange();
            highlight.CreatePathLines(_path);


            if (CanAttack())
                PaintTilesInAttackRange(_targetTile, 0);

            PaintTilesInMoveRange(_targetTile, 0);
            AddTilesInMoveRange();
            highlight.PaintLastTileInPath(_targetTile);
        }
    }

    /// <summary>
    /// Return Character World UI.
    /// </summary>
    public WorldUI GetMyUI()
    {
        return _myUI;
    }

    public int GetCurrentSteps()
    {
        return _currentSteps;
    }

    public Tile GetEndTile()
    {
        return _targetTile;
    }

    public Team GetUnitTeam()
    {
        return _unitTeam;
    }

    public bool IsSelected()
    {
        return _selected;
    }


    /// <summary>
    /// Return the Tile below the Character.
    /// </summary>
    public Tile GetMyPositionTile()
    {
        return _myPositionTile;
    }

    /// <summary>
    /// Return the current path.
    /// </summary>
    public List<Tile> GetPath()
    {
        _path = pathCreator.GetPath();
        return _path;
    }

    /// <summary>
    /// Return the Tile below the Character.
    /// </summary>
    public Tile GetTileBelow()
    {
        RaycastHit hit;
        var pos = _raycastToTile.position;
        //Works at this height after prefab update
        //pos.y = 3;
        Physics.Raycast(pos, Vector3.down, out hit, LayerMask.NameToLayer("GridBlock"));
        return hit.transform.gameObject.GetComponent<Tile>();
    }

    /// <summary>
    /// Return true if Character has Enemy Units in attack range.
    /// </summary>
    public bool HasEnemiesInRange()
    {
        return _enemiesInRange.Count > 0;
    }

    /// <summary>
    /// Return the selected gun.
    /// </summary>
    public Gun GetSelectedGun()
    {
        return _selectedGun;
    }

    /// <summary>
    /// Return the Left Gun.
    /// </summary>
    public Gun GetLeftGun()
    {
        return _leftGun;
    }

    /// <summary>
    /// Return the Right Gun.
    /// </summary>
    public Gun GetRightGun()
    {
        return _rightGun;
    }

    /// <summary>
    /// Return Body world position.
    /// </summary>
    public Vector3 GetBodyPosition()
    {
        return _bodyTransform.position;
    }

    /// <summary>
    /// Return Left Arm world position.
    /// </summary>
    public Vector3 GetLArmPosition()
    {
        if (_leftGun) return _leftGun.gameObject.transform.position;
        
        return Vector3.zero;
    }

    /// <summary>
    /// Return Right Arm world position.
    /// </summary>
    public Vector3 GetRArmPosition()
    {
        if (_rightGun) return _rightGun.gameObject.transform.position;
        
        return Vector3.zero;
    }

    /// <summary>
    /// Return Legs world position.
    /// </summary>
    public Vector3 GetLegsPosition()
    {
        return _legsTransform.position;
    }

    /// <summary>
    /// Return true if it's Character turn.
    /// </summary>
    public bool IsMyTurn()
    {
        return _myTurn;
    }

    public bool IsDead()
    {
        return _isDead;
    }

    /// <summary>
    /// Return true if Character is selected for an attack.
    /// </summary>
    public bool IsSelectedForAttack()
    {
        return _selectedForAttack;
    }

    public bool IsSelectingEnemy()
    {
        return _selectingEnemy;
    }

    /// <summary>
    /// Return true if Character can be attacked.
    /// </summary>
    public bool CanBeAttacked()
    {
        return _canBeAttacked;
    }

    /// <summary>
    /// Return true if Character can attack.
    /// </summary>
    public bool CanAttack()
    {
        return _canAttack;
    }

    /// <summary>
    /// Return true if Character is moving.
    /// </summary>
    public bool IsMoving()
    {
        return _moving;
    }

    /// <summary>
    /// Return true if Character Left Arm has more than 0 HP.
    /// </summary>
    public bool LeftGunAlive()
    {
        //TODO: alive
        if (_leftGun)
            _leftGunAlive = _leftGun.GetCurrentHp() > 0;
        else _leftGunAlive = false;
        
        return _leftGunAlive;
    }

    /// <summary>
    /// Return true if Character Right Arm has more than 0 HP.
    /// </summary>
    public bool RightGunAlive()
    {
        //TODO: alive
        if (_rightGun)
            _rightGunAlive = _rightGun.GetCurrentHp() > 0;
        else _rightGunAlive = false;
        
        return _rightGunAlive;
    }

    public bool CanMove()
    {
        return _canMove;
    }

    public bool CanBeSelected()
    {
        return !_equipableSelected && _canBeSelected;
    }

    /// <summary>
    /// Return the Character initiative.
    /// </summary>
    public float GetCharacterInitiative()
    {
        return _legs.GetCurrentHp() / _legs.GetMaxHp() * 100 + _legs.GetLegsInitiative();
    }

    public Sprite GetCharacterSprite()
    {
        return _myIcon;
    }

    public string GetCharacterName()
    {
        return _myName;
    }

    // public Item GetItem()
    // {
    //     return _equipable;
    // }

    public List<Equipable> GetEquipables()
    {
        return _equipables;
    }
    
    public void EquipableSelectionState(bool state, Equipable equipable)
    {
        _equipableSelected = state;
        _equipable = equipable;
    }

    public GameObject GetBurningSpawner()
    {

        return _particleMechaHandler.GetBurningSpawnerFromParticleMechaHandler();
    }

    public bool IsOverweight()
    {
        return _overweight;
    }

    public MaterialMechaHandler GetMaterialHandler()
    {
        return _materialMechaHandler;
    }

    #endregion

    #region Setters

    /// <summary>
    /// Set if it's Character turn.
    /// </summary>
    public virtual void SetTurn(bool state)
    {
        _myTurn = state;
        NotifyObserver("AboveTurn");
    }

    /// <summary>
    /// Set if Character is selected for an Attack.
    /// </summary>
    public void SetSelectedForAttack(bool state)
    {
        _selectedForAttack = state;
    }

    /// <summary>
    /// Set if Character is selecting an enemy.
    /// </summary>
    public void SetSelectingEnemy(bool state)
    {
        _selectingEnemy = state;
    }

    public void SetTargetTile(Tile target)
    {
        _targetTile = target;
    }

    /// <summary>
    /// Set if Character can move.
    /// </summary>
    public void SetCharacterMove(bool state)
    {
        _canMove = state;
    }

    public void SetSelection(bool state)
    {
        _selected = state;
    }

    #endregion

    #region Utilities

    //Se pintan los tiles dentro del rango de ataque
    public void PaintTilesInAttackRange(Tile currentTile, int count)
    {
        //TODO: alive
        if (!_leftGunAlive && !_rightGunAlive) return;
        
        //if (!_leftGun && !_rightGun) return;

        if (_selectedGun == null || count >= _selectedGun.GetAttackRange() ||
            (_tilesForAttackChecked.ContainsKey(currentTile) && _tilesForAttackChecked[currentTile] <= count))
            return;

        _tilesForAttackChecked[currentTile] = count;

        foreach (Tile tile in currentTile.allNeighbours)
        {
            if (!_tilesInAttackRange.Contains(tile))
            {
                if (!tile.HasTileAbove() && tile.IsWalkable())
                {
                    _tilesInAttackRange.Add(tile);
                    tile.inAttackRange = true;
                    if (tile.inMoveRange)
                    {
                        highlight.PaintTilesInMoveAndAttackRange(tile);
                    }
                    else highlight.PaintTilesInAttackRange(tile);
                }

            }

            PaintTilesInAttackRange(tile, count + 1);
        }
    }

    //Se pintan los tiles dentro del rango de movimiento
    public void PaintTilesInMoveRange(Tile currentTile, int count)
    {
        if (count >= _currentSteps || _tilesForMoveChecked.ContainsKey(currentTile) && _tilesForMoveChecked[currentTile] <= count)
                return;

        _tilesForMoveChecked[currentTile] = count;

        foreach (Tile tile in currentTile.neighboursForMove)
        {
            if (!_tilesInMoveRange.Contains(tile))
            {
                if (tile.IsWalkable() && tile.IsFree())
                {
                    if (!_tilesInMoveRange.Contains(tile))
                    {
                        _tilesInMoveRange.Add(tile);
                        tile.inMoveRange = true;
                        if (tile.inAttackRange)
                        {
                            highlight.PaintTilesInMoveAndAttackRange(tile);
                        }
                        else highlight.PaintTilesInMoveRange(tile);
                    }
                }

            }

            PaintTilesInMoveRange(tile, count + 1);
        }
    }


    /// <summary>
    /// Unpaint Tiles in Character attack range.
    /// </summary>
    public void ResetTilesInAttackRange()
    {
        highlight.ClearTilesInAttackRange(_tilesInAttackRange);
        _tilesInAttackRange.Clear();
        _tilesForAttackChecked.Clear();
    }

    /// <summary>
    /// Unpaint Tiles in Character move range.
    /// </summary>
    public void ResetTilesInMoveRange()
    {
        highlight.ClearTilesInMoveRange(_tilesInMoveRange);
        _tilesInMoveRange.Clear();
        _tilesForMoveChecked.Clear();
    }

    /// <summary>
    /// Clear  lists of enemies in attack range, tiles in move and attack range. 
    /// </summary>
    public void ResetInRangeLists()
    {
        foreach (Character item in _enemiesInRange)
        {
            TurnManager.Instance.UnitCantBeAttacked(item);
        }

        highlight.PathLinesClear();
        if (_path.Count > 0)
            highlight.EndLastTileInPath(_path[_path.Count - 1]);
        ResetTilesInMoveRange();
        ResetTilesInAttackRange();
        highlight.Undo();
        _enemiesInRange.Clear();
    }

    protected void AddTilesInMoveRange()
    {
        highlight.AddTilesInMoveRange(_tilesInMoveRange);
    }

    /// <summary>
    /// Reset Character for new turn.
    /// </summary>
    public virtual void NewTurn()
    {
        if (_isDead) return;

        _legsOvercharged = false;
        if (_rightGun)
        {
            _rightGun.Deselect();
            _rightGun.ReloadGun();
        }

        if (_leftGun)
        {
            _leftGun.Deselect();
            _leftGun.ReloadGun();
        }
        _myTurn = false;
        _canMove = true;
        
        _canAttack = true;
        _path.Clear();
        _currentSteps = _legs.GetCurrentHp() > 0 ? _legs.GetMaxSteps() : _legs.GetMaxSteps() / 2;
        _enemiesInRange.Clear();
        _canBeAttacked = false;
        pathCreator.ResetPath();
    }

    public void ClearTargetTile()
    {
        _targetTile = null;
    }

    /// <summary>
    /// Executed when Character reached the end of the path.
    /// </summary>
    public virtual void ReachedEnd()
    {
        _canMove = false;
        _legsOvercharged = false;
        highlight.characterMoving = false;
        highlight.EndPreview();
        _moving = false;
        InitialRotation = transform.rotation; //Cambio Nico
        if (_myPositionTile)
        {
            _myPositionTile.MakeTileFree();
            _myPositionTile.SetUnitAbove(null);
        }

        _myPositionTile = _targetTile;
        _myPositionTile.MakeTileOccupied();
        _myPositionTile.SetUnitAbove(this);
        _myPositionTile.unitAboveSelected = true;
        _myPositionTile.MouseOverColor();
        _targetTile = null;
        TurnManager.Instance.UnitStoppedMoving();
        pathCreator.ResetPath();
        _tilesForAttackChecked.Clear();
        _tilesInAttackRange.Clear();
        _animationMechaHandler.SetIsWalkingAnimatorFalse();
        _audioMechaHandler.SetMuteWalk();
        _particleMechaHandler.SetMachineOn(false);

        if (!CanAttack()) return;

        PaintTilesInAttackRange(_myPositionTile, 0);
        CheckEnemiesInAttackRange();

    }

    /// <summary>
    /// Reduce Character steps in the given amount.
    /// </summary>
    public void ReduceAvailableSteps(int amount)
    {
        int steps = _currentSteps - amount;
        _currentSteps = steps >= 0 ? steps : 0;
    }

    /// <summary>
    /// Increase Character steps in the given amount.
    /// </summary>
    public void IncreaseAvailableSteps(int amount)
    {
        if (!_legsOvercharged)
        {
            int steps = _currentSteps + amount;
            _currentSteps = steps <= _legs.GetMaxSteps() ? steps : _legs.GetMaxSteps();
        }
        else _currentSteps += amount;

    }

    /// <summary>
    /// Make Character not selectable.
    /// </summary>
    public void NotSelectable()
    {
        _canBeSelected = false;
    }

    public void Dead()
    {
        if (_isDead) return;
        _canBeSelected = false;
        _isDead = true;
        PortraitsController.Instance.DeadPortrait(this);
        NotifyObserver(_unitTeam == Team.Green ? "GreenDead" : "RedDead");
    }

    /// <summary>
    /// Make Character selected as an enemy.
    /// </summary>
    public void SelectedAsEnemy()
    {
        if (_isDead) return;

        _myPositionTile = GetTileBelow();
        if (!_myPositionTile) return;

        _myPositionTile.unitAboveSelected = true;
        _myPositionTile.GetComponent<TileMaterialhandler>().DiseableAndEnableSelectedNode(true);
    }

    /// <summary>
    /// Check if given Target is a valid GridBlock to move.
    /// </summary>
    private bool IsValidBlock(Transform target)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return false;

        if (!target) return false;

        if (target.gameObject.layer != LayerMask.NameToLayer("GridBlock")) return false;

        Tile tile = target.gameObject.GetComponent<Tile>();

        return (tile && tile.IsWalkable() && tile.IsFree() && tile.inMoveRange) || tile == _targetTile;
    }


    private void CheckEnemiesInAttackRange()
    {
        if (_tilesInAttackRange == null || _tilesInAttackRange.Count <= 0) return;

        foreach (Character unit in _enemiesInRange)
        {
            TurnManager.Instance.UnitCantBeAttacked(unit);
        }

        _enemiesInRange.Clear();
        foreach (Tile item in _tilesInAttackRange)
        {
            if (!item.IsFree())
            {
                Character unit = item.GetUnitAbove();

                if (unit.GetUnitTeam() == _unitTeam) continue;

                if (unit.IsDead()) continue;
                TurnManager.Instance.UnitCanBeAttacked(unit);
                _enemiesInRange.Add(unit);
            }
        }
    }

    /// <summary>
    /// Make Character attackable.
    /// </summary>
    public void MakeAttackable()
    {
        _canBeAttacked = true;
    }

    /// <summary>
    /// Make Character not attackable.
    /// </summary>
    public void MakeNotAttackable()
    {
        _canBeAttacked = false;
    }

    /// <summary>
    /// Check if Left and Right arm have more than 0 HP and determines if Character can attack.
    /// </summary>
    public void CheckArms()
    {
        if ((!LeftGunAlive() && !RightGunAlive()) || _selectedGun == null)
            _canAttack = false;
    }

    /// <summary>
    /// Deactivates Character Attack Action.
    /// </summary>
    public void DeactivateAttack()
    {
        ResetTilesInAttackRange();
        _canAttack = false;
    }

    private void OnMouseOver()
    {
        if (Cursor.lockState == CursorLockMode.Locked) return;
        
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        if (!_selectedForAttack && _canBeSelected)
            ShowWorldUI();
        
        if (_canBeAttacked && !_selectedForAttack)
        {
            RotateWithRays();
        }
    }

    private void OnMouseExit()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        HideWorldUI();
        
        if (_canBeAttacked)
        {
            ResetRotationAndRays();
        }
    }

    private void RotateWithRays()
    {
        Character c = CharacterSelection.Instance.GetSelectedCharacter();
        if (!c._selected) return;
        
        if (c.IsMoving()) return;
        
        if (c._rotated) return;
        c._rotated = true;
        c.SetInitialRotation(c.transform.rotation);
        c.transform.LookAt(transform.position);
        bool body = c.RayToPartsForAttack(GetBodyPosition(), "Body", true) && _body.GetCurrentHp() > 0;
        bool lArm = c.RayToPartsForAttack(GetLArmPosition(), "LGun", true) && _leftGun.GetCurrentHp() > 0;
        bool rArm = c.RayToPartsForAttack(GetRArmPosition(), "RGun", true) && _rightGun.GetCurrentHp() > 0;
        bool legs = c.RayToPartsForAttack(GetLegsPosition(), "Legs", true) && _legs.GetCurrentHp() > 0;
    }

    public void ResetRotationAndRays()
    {
        Character c = CharacterSelection.Instance.GetSelectedCharacter();
        if (c == null) return;

        if (c.IsSelectingEnemy()) return;
        
        c._rotated = false;
        //c._move.StopRotation();
        c.transform.rotation = c.InitialRotation; //Volver la rotación del mecha a InitialRotation, esto podría ser más smooth
        c.RaysOff(); //Apago los raycasts cuando saco el mouse
    }

    public void SetInitialRotation(Quaternion rot)
    {
        InitialRotation = rot;
    }

    public void SetRotationBeforeAttack(Quaternion rot)
    {
        RotationBeforeAttack = rot;
    }

    public void ResetRotationOnDeselect()
    {
        transform.rotation = RotationBeforeAttack;
    }

    /// <summary>
    /// Activates Character World UI.
    /// </summary>
    public void ShowWorldUI()
    {
        _myUI.SetName(_myName);
        _myUI.SetWorldUIValues(_body.GetCurrentHp(), _rightGun.GetCurrentHp(), _leftGun.GetCurrentHp(), _legs.GetCurrentHp(), _canMove, _canAttack, _overweight);
        _myUI.ContainerActivation(true);
    }

    /// <summary>
    /// Deactivates Character World UI.
    /// </summary>
    public void HideWorldUI()
    {
        if (_worldUIToggled == false) _myUI.DeactivateWorldUI();
    }

    public void WorldUIToggled(bool state)
    {
        _worldUIToggled = state;
    }
    
    #endregion

    #region Others

    public void SetHurtAnimation()
    {
        _animationMechaHandler.SetIsReciveDamageAnimatorTrue();
    }

    //public void HitSoundMecha()
    //{
    //    AudioManager.audioManagerInstance.PlaySound(soundHit, this.gameObject);
    //}

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
    
    public void OnUseEquipable()
    {
        EquipableSelectionState(false, null);
        // ButtonsUIManager.Instance.equipmentButton.OnRightClick?.Invoke();
        // ButtonsUIManager.Instance.EquipmentButtonState(false);
        // ButtonsUIManager.Instance.UpdateItemButtonName();
    }
    
    // public void StartItemUpdate()
    // {
    //     StartCoroutine(ItemUpdate());
    // }
    //
    // IEnumerator ItemUpdate()
    // {
    //     while (true)
    //     {
    //         _item.Use();
    //         yield return new WaitForEndOfFrame();
    //     }
    // }
    //
    // public void StopItemUpdate()
    // {
    //     StopCoroutine(ItemUpdate());
    // }

    #endregion

    public void SetEquipment(MechaEquipmentSO equipment)
    {
        _mechaEquipment = equipment;
    }
    protected virtual void ConfigureMecha()
    {
        if (!_mechaEquipment) return;

        _myName = _mechaEquipment.name;

        _body = Instantiate(_mechaEquipment.body.prefab, _bodySpawnPosition);
        _body.ManualStart(this);
        _body.transform.localPosition = Vector3.zero;
        _body.SetPart(_mechaEquipment.body);
        var bodyMesh = Instantiate(_mechaEquipment.body.meshPrefab[0], _body.transform);
        bodyMesh.transform.localPosition = Vector3.zero;
        var bodyShader = bodyMesh.GetComponent<MasterShaderScript>();
        bodyShader.colorMecha = _mechaEquipment.GetBodyColor();


        _myUI.SetBodyButtonPart(_materialMechaHandler,MechaParts.Body);

        var leftArm = Instantiate(_mechaEquipment.body.armsMeshPrefab[0], _leftArmSpawnPosition);
        leftArm.transform.localPosition = Vector3.zero;
        var leftArmShader = leftArm.GetComponent<MasterShaderScript>();
        leftArmShader.colorMecha = _mechaEquipment.GetBodyColor();

        _leftGun = Instantiate(_mechaEquipment.leftGun.prefab, _leftGunSpawn.transform);

        if (_leftGun)
        {
            _leftGun.transform.localPosition = Vector3.zero;
            _leftGun.gameObject.tag = "LGun";
            _leftGun.SetGun(_mechaEquipment.leftGun, this);
            _leftGun.StartRoulette();

            _myUI.SetLeftArmButtonPart(_materialMechaHandler, MechaParts.LArm);
        }


        var rightArm = Instantiate(_mechaEquipment.body.armsMeshPrefab[1], _rightArmSpawnPosition);
        rightArm.transform.localPosition = Vector3.zero;
        var rightArmShader = rightArm.GetComponent<MasterShaderScript>();
        rightArmShader.colorMecha = _mechaEquipment.GetBodyColor();

        _rightGun = Instantiate(_mechaEquipment.rightGun.prefab, _rightGunSpawn.transform);

        if (_rightGun)
        {
            _rightGun.transform.localPosition = Vector3.zero;
            _rightGun.gameObject.tag = "RGun";
            _rightGun.SetGun(_mechaEquipment.rightGun, this);
            _rightGun.StartRoulette();
            _myUI.SetRightArmButtonPart(_materialMechaHandler,MechaParts.RArm);
        }

        _legs = Instantiate(_mechaEquipment.legs.prefab, _rightLegSpawnPosition);
        _legs.ManualStart(this);
        var lLeg = Instantiate(_mechaEquipment.legs.meshPrefab[0], _legs.transform);
        lLeg.transform.localPosition = Vector3.zero;
        var lLegShader = lLeg.GetComponent<MasterShaderScript>();
        lLegShader.colorMecha = _mechaEquipment.GetLegsColor();
        
        var rLeg = Instantiate(_mechaEquipment.legs.meshPrefab[1], _leftLegSpawnPosition);
        rLeg.transform.localPosition = Vector3.zero;
        var rLegShader = rLeg.GetComponent<MasterShaderScript>();
        rLegShader.colorMecha = _mechaEquipment.GetLegsColor();
        

        _legs.transform.localPosition = Vector3.zero;
        _legs.SetPart(_mechaEquipment.legs);
        _legs.SetOtherLeg(rLeg);
        
        CheckWeight();

        _materialMechaHandler.SetPartGameObject(_body, _leftGun, _rightGun, _legs);
        
        _myUI.SetLegsButtonPart(_materialMechaHandler, MechaParts.Legs);
        
        
        //TODO: alive
        if (_leftGun)
            _leftGunAlive = _leftGun.GetCurrentHp() > 0 ? true : false;
        else _leftGunAlive = false;

        if (_rightGun)
            _rightGunAlive = _rightGun.GetCurrentHp() > 0 ? true : false;
        else _rightGunAlive = false;

        if (_rightGun)
        {
            _selectedGun = _rightGun;
            if (_selectedGun.GetGunType() == GunsType.Shield)
                _selectedGun.Ability();
            _canAttack = true;
            _rightGunSelected = true;
            _leftGunSelected = false;
        }
        else if (_leftGun)
        {
            _selectedGun = _leftGun;
            if (_selectedGun.GetGunType() == GunsType.Shield)
                _selectedGun.Ability();
            _canAttack = true;
            _rightGunSelected = false;
            _leftGunSelected = true;
        }
        else
        {
            _selectedGun = null;
            _leftGun = null;
            _rightGun = null;
            _rightGunSelected = false;
            _leftGunSelected = false;
        }
    }
    
    //Funcion de Nico para el push/pull
    public void ChangeMyPosTile(Tile newTile)
	{
        _myPositionTile.unitAboveSelected = false;
        _myPositionTile.MakeTileFree();
        _myPositionTile.SetUnitAbove(null);
        _myPositionTile = newTile;
        _myPositionTile.MakeTileOccupied();
        _myPositionTile.SetUnitAbove(this);
	}

    public void SetShaderForAllParts(SwitchTextureEnum texture)
    {
        foreach (var item in _partsDictionary)
        {
            item.Value.GetComponent<MasterShaderScript>().ConvertEnumToStringEnumForShader(texture);
        }
    }

    public void SetShaderForPart(SwitchTextureEnum texture, PartsMechaEnum partEnum)
    {
        foreach (var item in _partsDictionary)
        {
            if (item.Key == partEnum)
            {
                item.Value.GetComponent<MasterShaderScript>().ConvertEnumToStringEnumForShader(texture);
            }
        }
    }

    public void CheckWeight()
    {
        float totalWeight = 0;

        if (!_body) return;

        if (_leftGun)
            totalWeight += _leftGun.GetWeight();

        if (_rightGun)
            totalWeight += _rightGun.GetWeight();
        
        totalWeight += _legs.GetWeight();

        if (totalWeight <= _body.GetMaxWeight())
        {
            _overweight = false;
        }
        else _overweight = true;
        
        OnOverweight?.Invoke(_overweight);
    }
    
    public void ArmDestroyed(string location, Ability ability)
    {
        if (location == "Right")
        {
            _rightGunAlive = false;
            _rightGun = null;
        }
        else
        {
            _leftGunAlive = false;
            _leftGun = null;
        }
        
        if (ability && _equipables.Contains(ability))
        {
            _equipables.Remove(ability);
        }
        CheckWeight();
    }

    public void OnEnterElevator(Elevator elevator)
    {
        Subscribe(elevator);
    }

    public void OnExitElevator(Elevator elevator)
    {
        Unsubscribe(elevator);
    }
    public void CharacterElevatedState(bool state, int extraRange, int extraCrit)
    {
        _isOnElevator = state;
        
        _leftGun.ModifyRange(extraRange);
        _leftGun.ModifyCritChance(extraCrit);
        
        _rightGun.ModifyRange(extraRange);
        _rightGun.ModifyCritChance(extraCrit);
    }
}
public enum PartsMechaEnum
{
    armL,
    armR,
    body,
    legL,
    legR
};
