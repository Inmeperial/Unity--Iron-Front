using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.Effects;
using Random = System.Random;

public class Character : EnumsClass
{
    //STATS
    #region Stats
    
    [Header("Team")]
    [SerializeField] private Team _unitTeam;
    [SerializeField] public Sprite _myIcon;
    [SerializeField] public string _myName;
    
    [SerializeField] private LineRenderer _rayForBody;
    [SerializeField] private LineRenderer _rayForLeftArm;
    [SerializeField] private LineRenderer _rayForRightArm;
    [SerializeField] private LineRenderer _rayForLegs;
    [SerializeField] private Material _rayHitMaterial;
    [SerializeField] private Material _rayMissMaterial;

    [Header("Body")] 
    public Body body;
    [SerializeField] private Transform _bodyTransform;

    [Header("Left Arm")]
    public Arm leftArm;
    private Gun _leftGun;
    private bool _leftGunSelected;
    [SerializeField] private GunsType _leftGunType;
    [SerializeField] private Transform _leftGunSpawn;

    [Header("Right Arm")]
    public Arm rightArm;
    private Gun _rightGun;
    private bool _rightGunSelected;
    [SerializeField] private GunsType _rightGunType;
    [SerializeField] private Transform _rightGunSpawn;
    
    [Header("Legs")] 
    public Legs legs;
    [SerializeField] private Transform _legsTransform;
    private int _currentSteps;
    #endregion

    private Gun _selectedGun;

    
    
    //MOVEMENT RELATED
    public IPathCreator pathCreator;
    private GridMovement _move;
    public LayerMask block;
    private List<Tile> _tilesInMoveRange = new List<Tile>();
    private Tile _myPositionTile;
    private Tile _targetTile;
    [SerializeField]private List<Tile> _path = new List<Tile>();

    //FLAGS
    private bool _canBeSelected;
    private bool _selected;
    private bool _moving = false;
    public bool _canMove = true;
    private bool _canAttack = true;
    private bool _leftArmAlive;
    private bool _rightArmAlive;
    private bool _canBeAttacked = false;
    private bool _selectingEnemy = false;
    private bool _selectedForAttack;
    private bool _myTurn = false;

    //OTHERS
    private HashSet<Tile> _tilesInAttackRange = new HashSet<Tile>();
    private Dictionary<Tile, int> _tilesForAttackChecked = new Dictionary<Tile, int>();
    private Dictionary<Tile, int> _tilesForMoveChecked = new Dictionary<Tile, int>();
    private List<Character> _enemiesInRange = new List<Character>();
    private WorldUI _myUI;
    private MechaMaterialhandler _mechaMaterlaHandler;
    private SmokeMechaHandler _smokeMechaHandler;
    public AudioClip soundMotorStart;
    public AudioClip soundWalk;
    private AnimationMechaHandler _animationMechaHandler;

    [HideInInspector]
	public EffectsController effectsController;
    [HideInInspector]
    public TurnManager turnManager;
	[HideInInspector]
	public TileHighlight highlight;
	[HideInInspector]
	public ButtonsUIManager buttonsManager;

    private void Awake()
    {
        transform.position = new Vector3(transform.position.x, 2.4f, transform.position.z);
        #region GetComponents
        _mechaMaterlaHandler = GetComponent<MechaMaterialhandler>();
        _smokeMechaHandler = GetComponent<SmokeMechaHandler>();
        _move = GetComponent<GridMovement>();
        pathCreator = GetComponent<IPathCreator>();
        _myUI = GetComponent<WorldUI>();
        _animationMechaHandler = GetComponent<AnimationMechaHandler>();
        #endregion
        #region FindObject
        turnManager = FindObjectOfType<TurnManager>();
        highlight = FindObjectOfType<TileHighlight>();
        buttonsManager = FindObjectOfType<ButtonsUIManager>();
        effectsController = FindObjectOfType<EffectsController>();
        
        #endregion
        #region GunsAndArms
        var gunSpawn = FindObjectOfType<GunsSpawner>();
        _leftArmAlive = leftArm.GetCurrentHp() > 0 ? true : false;
        _leftGun = gunSpawn.SpawnGun(_leftGunType, Vector3.zero, _leftGunSpawn);
        if (_leftGun)
        {
            _leftGun.gameObject.tag = "LArm";
            _leftGun.SetGun();
            _leftGun.StartRoulette();
            _leftGun.SetRightOrLeft("Left");
        }
        
        _rightArmAlive = rightArm.GetCurrentHp() > 0 ? true : false;
        _rightGun = gunSpawn.SpawnGun(_rightGunType, Vector3.zero, _rightGunSpawn);
        if (_rightGun)
        {
            _rightGun.gameObject.tag = "RArm";
            _rightGun.SetGun();
            _rightGun.StartRoulette();
            _rightGun.SetRightOrLeft("Right");
        }
        
        leftArm.SetRightOrLeft("Left");
        rightArm.SetRightOrLeft("Right");
        
        if (_rightArmAlive)
        {
            _selectedGun = _rightGun;
            if (_selectedGun.GetGunType() == GunsType.Shield)
                _selectedGun.Ability();
            _canAttack = true;
            _rightGunSelected = true;
            _leftGunSelected = false;
        }
        else if (_leftArmAlive)
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
            _canAttack = false;
            _rightGunSelected = false;
            _leftGunSelected = false;
        }
        #endregion
    }

    // Start is called before the first frame update
    void Start()
    {
        _canMove = legs.GetCurrentHp() > 0;
        _currentSteps = _canMove ? legs.GetMaxSteps() : 0;
        
        _myPositionTile = GetTileBelow();
        _myPositionTile.MakeTileOccupied();
        _myPositionTile.SetUnitAbove(this);
        _move.SetMoveSpeed(legs.GetMoveSpeed());
        _move.SetRotationSpeed(legs.GetRotationSpeed());
        
        if (_myUI)
            _myUI.SetLimits(body.GetMaxHp(), rightArm.GetMaxHp(), leftArm.GetMaxHp(), legs.GetMaxHp());

        _selected = false;
        _canBeSelected = body.GetCurrentHp() > 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_selected && !_moving && _canMove && !_selectingEnemy && Input.GetMouseButtonDown(0))
        {
            GetTargetToMove();
        }
    }
    

    #region Actions

    private void Move()
    {
        if (_moving != false || _path == null || _path.Count <= 0) return;
        
        _moving = true;
        buttonsManager.DeactivateBodyPartsContainer();
        turnManager.UnitIsMoving();
        highlight.characterMoving = true;
        if (_myPositionTile)
        {
            _myPositionTile.unitAboveSelected = false;
            _myPositionTile.EndMouseOverColor();
        }
            
        ResetTilesInMoveRange();
        _animationMechaHandler.SetIsWalkingAnimatorTrue();
        AudioManager.audioManagerInstance.PlaySound(soundMotorStart, this.gameObject);
        AudioManager.audioManagerInstance.PlaySound(soundWalk, this.gameObject);
        _smokeMechaHandler.SetMachineOn(true);
        _move.StartMovement(_path);
    }
    
    /// <summary>
    /// Creates shoot particle at selected gun position.
    /// </summary>
    public void Shoot()
    {
        if (_rightGunSelected)
        {
            effectsController.PlayParticlesEffect(_leftGun.gameObject.transform.position, "Attack");
            switch (_rightGunType)
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
            effectsController.PlayParticlesEffect(_rightGun.gameObject.transform.position, "Attack");
            switch (_leftGunType)
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
        if (_selectedGun.GetGunType() == GunsType.Shield)
            if (_canAttack) _selectedGun.Ability();
        
        _selectedGun = _leftGun;
        _leftGunSelected = true;
        _rightGunSelected = false;
        ResetTilesInAttackRange();
        ResetTilesInMoveRange();
        Debug.Log("can attack " + _canAttack);
        if (_selectedGun.GetGunType() != GunsType.Shield)
        {
            if (_canAttack)
            {
                PaintTilesInAttackRange(_path.Count == 0 ? _myPositionTile : _path[_path.Count - 1], 0);
                CheckEnemiesInAttackRange();
            }
        }
        else
        {
            if (_canAttack)
            {
                _selectedGun.Ability();
            }
        }
        

        if (_canMove)
        {
            PaintTilesInMoveRange(_path.Count == 0 ? _myPositionTile : _path[_path.Count - 1], 0);
        }
    }

    /// <summary>
    /// Select Right Gun and repaint tiles.
    /// </summary>
    public void SelectRightGun()
    {
        if (_selectedGun.GetGunType() == GunsType.Shield)
            if (_canAttack) _selectedGun.Ability();
        
        _selectedGun = _rightGun;
        _leftGunSelected = false;
        _rightGunSelected = true;
        ResetTilesInAttackRange();
        ResetTilesInMoveRange();
        
        if (_selectedGun.GetGunType() != GunsType.Shield)
        {
            if (_canAttack)
            {
                PaintTilesInAttackRange(_path.Count == 0 ? _myPositionTile : _path[_path.Count - 1], 0);
                CheckEnemiesInAttackRange();
            }
        }
        else
        {
            if (_canAttack)
            {
                _selectedGun.Ability();
            }
        }
        
        if (_canMove)
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
    public void SelectThisUnit()
    {
        _selected = true;
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
        
        //_mechaMaterlaHandler.SetSelectedMechaMaterial(true);
        if (CanAttack())
        {
            if (_rightArmAlive)
                _selectedGun = _rightGun;
            else if (_leftArmAlive)
                _selectedGun = _leftGun;
            PaintTilesInAttackRange(_myPositionTile, 0);
            CheckEnemiesInAttackRange();
        }
        if (_canMove)
        {
            _currentSteps = legs.GetMaxSteps();
            AddTilesInMoveRange();
            PaintTilesInMoveRange(_myPositionTile, 0);
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
        
        //_mechaMaterlaHandler.SetSelectedMechaMaterial(false);
        foreach (var item in _enemiesInRange)
        {
            turnManager.UnitCantBeAttacked(item);
        }

        if (_canMove)
            _currentSteps = legs.GetMaxSteps();
        ResetInRangeLists();
        _path.Clear();
        highlight.PathLinesClear();
        pathCreator.ResetPath();

        _rayForBody.positionCount = 0;
        _rayForLegs.positionCount = 0;
        _rayForLeftArm.positionCount = 0;
        _rayForRightArm.positionCount = 0;
    }

    /// <summary>
    /// Rotate Character towards enemy.
    /// </summary>
    public void RotateTowardsEnemy(Vector3 pos)
    {
        _move.SetPosToRotate(pos);
        _move.StartRotation();
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
    public bool RayToPartsForAttack(Vector3 partPosition, string tagToCheck)
    {
        var position = _rayForBody.gameObject.transform.position;
        var dir = (partPosition - position).normalized; 
        
        Physics.Raycast(position, dir, out var hit, 1000f);
        var goodHit = hit.collider.transform.gameObject.CompareTag(tagToCheck);

        LineRenderer renderer = null;
        switch (tagToCheck)
        {
            case "Body":
                renderer = _rayForBody;
                break;

            case "Legs":
                renderer = _rayForLegs;
                break;
            
            case "RArm":
                renderer = _rayForRightArm;
                break;
            
            case "LArm":
                renderer = _rayForLeftArm;
                break;
        }
        
        // renderer.positionCount = 2;
        // renderer.SetPosition(0, position);
        // renderer.SetPosition(1, partPosition);
        
        if (goodHit)
        {
            //renderer.material = _rayHitMaterial;
            Debug.DrawRay(position, dir * 20f, Color.green, 1000f);
            return true;
        }
        
        //renderer.material = _rayMissMaterial;
        Debug.DrawRay(position, dir * 20f, Color.red, 1000f);
        return false;
        
    }
    
    
    #endregion

    #region Getters

    private void GetTargetToMove()
    {
        Transform target = MouseRay.GetTargetTransform(block);

        if (!IsValidBlock(target)) return;
        
        var newTile = target.GetComponent<Tile>();
        //LLAMAR A MOVE
        if (_targetTile && _targetTile == newTile && _path.Count > 0)
        {
            Move();
        } 
        else
        {
                
            pathCreator.Calculate(this, newTile, _currentSteps);
                
            if (pathCreator.GetDistance() > legs.GetMaxSteps()) return;
                
            if (_targetTile) highlight.EndLastTileInPath(_targetTile);
            
            _targetTile = newTile;
            _path = pathCreator.GetPath();
                
            if (_path.Count <= 0) return;
                
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
        Physics.Raycast(transform.position, Vector3.down, out hit, LayerMask.NameToLayer("GridBlock"));
        return hit.transform.gameObject.GetComponent<Tile>();
    }
    
    /// <summary>
    /// Return true if Character has Enemy Units in attack range.
    /// </summary>
    public bool HasEnemiesInRange()
    {
        return _enemiesInRange.Count > 0 ? true : false;
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
        return _leftGun.gameObject.transform.position;
	}
    
    /// <summary>
    /// Return Right Arm world position.
    /// </summary>
    public Vector3 GetRArmPosition()
    {
        return _rightGun.gameObject.transform.position;
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
    public bool LeftArmAlive()
    {
        _leftArmAlive = leftArm.GetCurrentHp() > 0 ? true : false;
        return _leftArmAlive;
    }
    
    /// <summary>
    /// Return true if Character Right Arm has more than 0 HP.
    /// </summary>
    public bool RightArmAlive()
    {
        _rightArmAlive = rightArm.GetCurrentHp() > 0 ? true : false;
        return _rightArmAlive;
    }
    
    public bool ThisUnitCanMove()
    {
        return _canMove;
    }
    
    public bool CanBeSelected()
    {
        return _canBeSelected && _myTurn;
    }
    
    /// <summary>
    /// Return the Character initiative.
    /// </summary>
    public float GetCharacterInitiative()
    {
        return legs.GetCurrentHp() / legs.GetMaxHp() * 100 + legs.GetLegsInitiative();
    }
    #endregion

    #region Setters
    
    /// <summary>
    /// Set if it's Character turn.
    /// </summary>
    public void SetTurn(bool state)
    {
        _myTurn = state;
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
    #endregion
    
    #region Utilities
    
    //Se pintan los tiles dentro del rango de ataque
    private void PaintTilesInAttackRange(Tile currentTile, int count)
    {
        if (_selectedGun == null || count >= _selectedGun.GetAttackRange() || (_tilesForAttackChecked.ContainsKey(currentTile) && _tilesForAttackChecked[currentTile] <= count))
            return;
        
        _tilesForAttackChecked[currentTile] = count;

        foreach (var tile in currentTile.allNeighbours)
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
    private void PaintTilesInMoveRange(Tile currentTile, int count)
    {
        if (count >= _currentSteps || (_tilesForMoveChecked.ContainsKey(currentTile) && _tilesForMoveChecked[currentTile] <= count))
            return;

        _tilesForMoveChecked[currentTile] = count;

        foreach (var tile in currentTile.neighboursForMove)
        {
            if (!_tilesInMoveRange.Contains(tile))
            {
                if (tile.IsWalkable() && tile.IsUnitFree())
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
        foreach (var item in _enemiesInRange)
        {
            turnManager.UnitCantBeAttacked(item);
        }
        highlight.PathLinesClear();
        if (_path.Count > 0)
            highlight.EndLastTileInPath(_path[_path.Count-1]);
        ResetTilesInMoveRange();
        ResetTilesInAttackRange();
        highlight.Undo();
        _enemiesInRange.Clear();
    }

    private void AddTilesInMoveRange()
    {
        highlight.AddTilesInMoveRange(_tilesInMoveRange);
    }

    /// <summary>
    /// Reset Character for new turn.
    /// </summary>
    public void NewTurn()
    {
        _myTurn = false;
        _canMove = legs.GetCurrentHp() > 0 ? true : false;
        _canAttack = true;
        _selectedGun.SetGun();
        _path.Clear();
        _currentSteps = _canMove ? legs.GetMaxSteps() : 0;
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
    public void ReachedEnd()
    {
        _canMove = false;
        highlight.characterMoving = false;
        highlight.EndPreview();
        _moving = false;
        if (_myPositionTile)
        {
            _myPositionTile.MakeTileFree();
        }
        _myPositionTile = _targetTile;
        
        turnManager.UnitStoppedMoving();
        pathCreator.ResetPath();
        _tilesForAttackChecked.Clear();
        _tilesInAttackRange.Clear();
        _animationMechaHandler.SetIsWalkingAnimatorFalse();
        AudioManager.audioManagerInstance.StopSoundWithFadeOut(soundMotorStart,this.gameObject);
        AudioManager.audioManagerInstance.StopSound(soundWalk,this.gameObject);
        _smokeMechaHandler.SetMachineOn(false);

        if (CanAttack())
        {
            PaintTilesInAttackRange(_myPositionTile, 0);
            CheckEnemiesInAttackRange();
        }
        _myPositionTile.MakeTileOccupied();
        _myPositionTile.SetUnitAbove(this);
        _myPositionTile.unitAboveSelected = true;
        _myPositionTile.MouseOverColor();
        _targetTile = null;
    }

    /// <summary>
    /// Reduce Character steps in the given amount.
    /// </summary>
    public void ReduceAvailableSteps(int amount)
    {
        var steps = _currentSteps - amount;
        _currentSteps = steps >= 0 ? steps : 0;
    }

    /// <summary>
    /// Increase Character steps in the given amount.
    /// </summary>
    public void IncreaseAvailableSteps(int amount)
    {
        var steps = _currentSteps + amount;
        _currentSteps = steps <= legs.GetMaxSteps() ? steps : legs.GetMaxSteps();
    }
    
    /// <summary>
    /// Make Character not selectable.
    /// </summary>
    public void NotSelectable()
    {
        _canBeSelected = false;
    }
    
    /// <summary>
    /// Make Character selected as an enemy.
    /// </summary>
    public void SelectedAsEnemy()
    {
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
        
        var tile = target.gameObject.GetComponent<Tile>();
        
        return (tile && tile.IsWalkable() && tile.IsUnitFree() && tile.inMoveRange) || tile == _targetTile;
    }

    
    private void CheckEnemiesInAttackRange()
    {
        if (_tilesInAttackRange == null || _tilesInAttackRange.Count <= 0) return;
        
        foreach (var unit in _enemiesInRange)
        {
            turnManager.UnitCantBeAttacked(unit);
        }
        _enemiesInRange.Clear();
        foreach (var item in _tilesInAttackRange)
        {
            if (item.IsUnitFree()) continue;
            
            var unit = item.GetUnitAbove();
            
            if (unit.GetUnitTeam() == _unitTeam) continue;
            
            turnManager.UnitCanBeAttacked(unit);
            _enemiesInRange.Add(unit);
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
        if (!LeftArmAlive() && !RightArmAlive())
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
        if (!_selectedForAttack)
            ShowWorldUI();
    }

    private void OnMouseExit()
    {
        HideWorldUI();
    }

    /// <summary>
    /// Activates Character World UI.
    /// </summary>
    public void ShowWorldUI()
    {
        _myUI.SetName(_myName);
        _myUI.SetWorldUIValues(body.GetCurrentHp(), rightArm.GetCurrentHp(), leftArm.GetCurrentHp(), legs.GetCurrentHp(), _canMove, _canAttack);
        _myUI.ContainerActivation(true);
    }

    /// <summary>
    /// Deactivates Character World UI.
    /// </summary>
    public void HideWorldUI()
    {
        _myUI.DeactivateWorldUI();
    }


    #endregion

    public void SetHurtAnimation()
    {
        _animationMechaHandler.SetIsReciveDamageAnimatorTrue();
    }


}
