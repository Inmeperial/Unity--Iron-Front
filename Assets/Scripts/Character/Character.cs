using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[SelectionBase]
public class Character : Initializable
{
    [Header("References")]
    //[SerializeField] protected WorldUI _worldUI;
    [SerializeField] protected WaypointsPathfinding _waypointsPathfinding;
    [SerializeField] protected GridMovement _gridMovement;
    [SerializeField] protected ParticleMechaHandler _particleMechaHandler;
    [SerializeField] protected AnimationMechaHandler _animationMechaHandler;
    [SerializeField] protected AudioMechaHandler _audioMechaHandler;

    [Header("Rays")]
    [SerializeField] protected Transform _raycastToTile;
    [SerializeField] protected LineRenderer _rayForBody;
    [SerializeField] protected LineRenderer _rayForLeftArm;
    [SerializeField] protected LineRenderer _rayForRightArm;
    [SerializeField] protected LineRenderer _rayForLegs;
    [SerializeField] protected Material _rayHitMaterial;
    [SerializeField] protected Material _rayMissMaterial;
    [SerializeField] protected float _raysOffDelay;

    #region Parts
    [Header("Parts")]
    [SerializeField] protected Body _body;

    [SerializeField] protected GameObject _leftGunSpawn;
    protected Gun _leftGun;
    protected bool _leftGunSelected;

    [SerializeField] protected GameObject _rightGunSpawn;
    protected Gun _rightGun;
    protected bool _rightGunSelected;

    protected Gun _selectedGun;

    [SerializeField] protected Legs _legs;
    protected int _currentSteps;
    #endregion

    [Space(2)]

    [Header("Configs")]
    [SerializeField] protected EnumsClass.Team _unitTeam;
    [SerializeField] protected LayerMask _block;

    [Header("Data")]
    [SerializeField] protected MechaEquipmentSO _mechaEquipment;
    [SerializeField] protected AudioClip _soundHit;
    [SerializeField] protected Sprite _myIcon;

    protected string _myName;

    protected Equipable _equipable;
    protected Item _item;

    //TODO: REVISAR ESTA RANCIADA
    //protected Dictionary<PartsMechaEnum, GameObject> _partsDictionary = new Dictionary<PartsMechaEnum, GameObject>();

    [Header("Others")]
    
    protected List<Tile> _path = new List<Tile>();
    protected List<Tile> _tilesInMoveRange = new List<Tile>();
    protected Tile _myPositionTile;
    protected Tile _targetTile;

    protected HashSet<Tile> _tilesInAttackRange = new HashSet<Tile>();
    protected Dictionary<Tile, int> _tilesForAttackChecked = new Dictionary<Tile, int>();
    protected Dictionary<Tile, int> _tilesForMoveChecked = new Dictionary<Tile, int>();

    protected List<Character> _enemiesInRange = new List<Character>();

    public Quaternion SavedRotation { get; private set; } //Cambio Nico


    #region  Flags
    protected bool _canBeSelected;
    protected bool _selected;
    protected bool _moving = false;
    protected bool _canMove = true;
    protected bool _canAttack = true;
    protected bool _leftGunAlive;
    protected bool _rightGunAlive;
    protected bool _inAttackRange = false;
    protected bool _selectingEnemy = false;
    protected bool _selectedForAttack;
    protected bool _myTurn = false;
    protected bool _isDead = false;
    protected bool _equipableSelected;
    protected bool _rotated;
    protected bool _unitEnabled = true;
    
    protected bool _overweight;    
    
    protected bool _legsOvercharged;

    protected bool _isOnElevator;

    protected bool _movementReduced;
    #endregion

    public Action<Character, bool> OnOverweight;

    protected List<IObserver> _observers = new List<IObserver>();

    protected List<Equipable> _equipables = new List<Equipable>();

    protected float _startingHeight;

    public Action<Character> OnMechaSelected;
    public Action OnMechaDeselected;
    public Action OnSelectingEnemy;
    public Action OnBeginMove;
    public Action OnEndMove;
    public Action<Character, bool> OnMoveActionStateChange;
    public Action<Character, bool> OnAttackActionStateChange;
    public Action OnLeftGunSelected;
    public Action OnRightGunSelected;

    public Action<Character> OnMouseOverMecha;
    public Action<Character> OnMouseExitMecha;
    public Action<Character> OnMechaDeath;

    public Action OnMechaTurnStart;
    public virtual void Awake()
    {
        _startingHeight = transform.position.y;
    }

    // Start is called before the first frame update
    public override void Initialize()
    {
        ConfigureMecha();

        _myPositionTile = GetTileBelow();

        if (!_myPositionTile)
            throw new Exception("Tile not found on start!");

        _myPositionTile.MakeTileOccupied();
        _myPositionTile.SetUnitAbove(this);
        _gridMovement.SetMoveSpeed(_legs.GetMoveSpeed());
        _gridMovement.SetRotationSpeed(_legs.GetRotationSpeed());

        _selected = false;

        _canBeSelected = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_isDead)
            return;
        
        if (!_unitEnabled)
            return;

        if (_unitTeam == EnumsClass.Team.Red)
            return;

        if (!_isOnElevator)
        {
            if (_selected && !_moving && _canMove && !_selectingEnemy && Input.GetMouseButtonDown(0))
                GetTargetToMove();
        }
        

        if (!_selected && _equipables.Count > 0 && _equipableSelected)
            _equipable.Use(OnUseEquipable);
    }


    #region Actions

    protected void Move()
    {
        _moving = true;
        //ButtonsUIManager.Instance.DeactivateBodyPartsContainer();
        //ButtonsUIManager.Instance.DeactivateEquipablesButtons();
        //TurnManager.Instance.UnitIsMoving();
        TileHighlight.Instance.characterMoving = true;

        if (_myPositionTile)
        {
            _myPositionTile.unitAboveSelected = false;
            _myPositionTile.EndMouseOverColor();
        }

        ResetTilesInMoveRange();
        ResetTilesInAttackRange();

        OnBeginMove?.Invoke();

        _animationMechaHandler.SetIsWalkingAnimatorTrue();
        _audioMechaHandler.SetPlayMotorStart();
        _particleMechaHandler.SetMachineOn(true);
        _gridMovement.StartMovement(_path);
    }

    /// <summary>
    /// Creates shoot particle at selected gun position.
    /// </summary>
    //public void Shoot()
    //{
    //    if (!_selectedGun)
    //        return;

    //    //TODO: AGREGAR LA EJECUCION DE LA ANIMACION (EL SWITCH RANCIO) AL ARMA
    //    if (_selectedGun.GetLocation() == "Right")
    //    {
    //        switch (_rightGun.GetGunType())
    //        {
    //            //case EnumsClass.GunsType.None:
    //            //    break;
    //            //case EnumsClass.GunsType.AssaultRifle:
    //            //    _animationMechaHandler.SetIsMachineGunAttackRightAnimatorTrue();
    //            //    break;
    //            case EnumsClass.GunsType.Melee:
    //                _animationMechaHandler.SetIsHammerAttackRightAnimatorTrue();
    //                break;
    //            case EnumsClass.GunsType.Rifle:
    //                _animationMechaHandler.SetIsSniperAttackRightAnimatorTrue();
    //                break;
    //            case EnumsClass.GunsType.Shield:
    //                break;
    //            case EnumsClass.GunsType.Shotgun:
    //                _animationMechaHandler.SetIsShotgunAttackRightAnimatorTrue();
    //                break;
    //        }
    //    }
    //    else if (_selectedGun.GetLocation() == "Left")
    //    {
    //        switch (_leftGun.GetGunType())
    //        {
    //            case EnumsClass.GunsType.None:
    //                break;
    //            case EnumsClass.GunsType.AssaultRifle:
    //                _animationMechaHandler.SetIsMachineGunAttackLeftAnimatorTrue();
    //                break;
    //            case EnumsClass.GunsType.Melee:
    //                _animationMechaHandler.SetIsHammerAttackLeftAnimatorTrue();
    //                break;
    //            case EnumsClass.GunsType.Rifle:
    //                _animationMechaHandler.SetIsSniperAttackLeftAnimatorTrue();
    //                break;
    //            case EnumsClass.GunsType.Shield:
    //                break;
    //            case EnumsClass.GunsType.Shotgun:
    //                _animationMechaHandler.SetIsShotgunAttackLeftAnimatorTrue();
    //                break;
    //        }
    //    }
    //}

    /// <summary>
    /// Select Left Gun and repaint tiles.
    /// </summary>
    public void SelectLeftGun()
    {
        if (_selectingEnemy)
            return;

        if (!_leftGun)
            return;
        
        if (_rightGun)
            _rightGun.Deselect();
        
        foreach (Character enemy in _enemiesInRange)
        {
            enemy.MechaOutsideAttackRange();
        }

        _enemiesInRange.Clear();
        
        _selectedGun = _leftGun;
        _leftGunSelected = true;
        _rightGunSelected = false;
        ResetTilesInAttackRange();
        ResetTilesInMoveRange();

        //if (_selectedGun.GetGunType() != GunsType.Shield)
        //{
        //    if (_canAttack)
        //    {
        //        if (!_isOnElevator || _isOnElevator && _selectedGun.GetAttackRange() > 1)
        //        {
        //            if (_unitTeam == Team.Green)
        //                PaintTilesInAttackRange(_path.Count == 0 ? _myPositionTile : _path[_path.Count - 1], 0);
        //            else
        //                PaintTilesInAttackRange(_myPositionTile, 0);

        //            CheckEnemiesInAttackRange(); 
        //        }
        //    }
        //}
        //else if (!_isOnElevator && _canAttack)
        //        _selectedGun.Ability();

        if (_canAttack)
        {
            if (!_isOnElevator || _isOnElevator && _selectedGun.GetAttackRange() > 1)
            {
                if (_unitTeam == EnumsClass.Team.Green)
                    PaintTilesInAttackRange(_path.Count == 0 ? _myPositionTile : _path[_path.Count - 1], 0);
                else
                    PaintTilesInAttackRange(_myPositionTile, 0);

                CheckEnemiesInAttackRange();
            }
        }


        if (!_isOnElevator && _canMove)
        {
            if (_unitTeam == EnumsClass.Team.Green)
                PaintTilesInMoveRange(_path.Count == 0 ? _myPositionTile : _path[_path.Count - 1], 0);
            else
                PaintTilesInMoveRange(_myPositionTile, 0);
        }

        OnLeftGunSelected?.Invoke();
    }

    /// <summary>
    /// Select Right Gun and repaint tiles.
    /// </summary>
    public void SelectRightGun()
    {
        if (_selectingEnemy)
            return;

        if (!_rightGun)
            return;
        
        if (_leftGun)
            _leftGun.Deselect();

        foreach (Character enemy in _enemiesInRange)
        {
            enemy.MechaOutsideAttackRange();
        }
        _enemiesInRange.Clear();
        
        ResetRotationAndRays();
        
        _selectedGun = _rightGun;
        _leftGunSelected = false;
        _rightGunSelected = true;
        ResetTilesInAttackRange();
        ResetTilesInMoveRange();

        //if (_selectedGun.GetGunType() != GunsType.Shield)
        //{
        //    if (_canAttack)
        //    {
        //        if (!_isOnElevator || _isOnElevator && _selectedGun.GetAttackRange() > 1)
        //        {
        //            if (_unitTeam == Team.Green)
        //                PaintTilesInAttackRange(_path.Count == 0 ? _myPositionTile : _path[_path.Count - 1], 0);
        //            else
        //                PaintTilesInAttackRange(_myPositionTile, 0);


        //            CheckEnemiesInAttackRange(); 
        //        }
        //    }
        //}
        //else if (!_isOnElevator && _canAttack)
        //    _selectedGun.Ability();

        if (_canAttack)
        {
            if (!_isOnElevator || _isOnElevator && _selectedGun.GetAttackRange() > 1)
            {
                if (_unitTeam == EnumsClass.Team.Green)
                    PaintTilesInAttackRange(_path.Count == 0 ? _myPositionTile : _path[_path.Count - 1], 0);
                else
                    PaintTilesInAttackRange(_myPositionTile, 0);


                CheckEnemiesInAttackRange();
            }
        }

        if (!_isOnElevator && _canMove)
        {
            if (_unitTeam == EnumsClass.Team.Green)
                PaintTilesInMoveRange(_path.Count == 0 ? _myPositionTile : _path[_path.Count - 1], 0);
            else
                PaintTilesInMoveRange(_myPositionTile, 0);
        }

        OnRightGunSelected?.Invoke();
    }

    public void Undo()
    {
        ResetInRangeLists();
        GetPath();

        if (_path.Count > 1)
        {
            _targetTile = _path[_path.Count - 1];
            PaintTilesInMoveRange(_targetTile, 0);
            TileHighlight.Instance.PaintLastTileInPath(_targetTile);

            if (CanAttack())
                PaintTilesInAttackRange(_targetTile, 0);

            TileHighlight.Instance.PaintLastTileInPath(_targetTile);
        }
        else
        {
            _targetTile = null;
            _waypointsPathfinding.ResetPath();
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
        if (_isDead)
            return;

        _selected = true;
        
        OnMechaSelected?.Invoke(this);

        SavedRotation = transform.rotation; //Cambio Nico
        ResetInRangeLists();
        _path.Clear();
        TileHighlight.Instance.PathLinesClear();
        _targetTile = null;

        if (!_isOnElevator)
        {
            if (!_myPositionTile)
                _myPositionTile = GetTileBelow();
            
            if (_myPositionTile)
            {
                _myPositionTile.unitAboveSelected = true;
                _myPositionTile.MakeTileFree();
                _myPositionTile.GetComponent<TileMaterialhandler>().DiseableAndEnableSelectedNode(true);
            }
        }

        if (_canAttack)
        {
            if (_rightGun)
            {
                _selectedGun = _rightGun;
                SelectRightGun();
            }
            
            else if (_leftGun)
            {
                _selectedGun = _leftGun;
                SelectLeftGun();
            }

            else
                _selectedGun = null;
            
            if (_isOnElevator && _selectedGun.GetAttackRange() > 1)
            {
                PaintTilesInAttackRange(_myPositionTile, 0);
                CheckEnemiesInAttackRange();
            }
            else if (!_isOnElevator)
            {
                PaintTilesInAttackRange(_myPositionTile, 0);
                CheckEnemiesInAttackRange();
            }
        }
        
        if (!_isOnElevator && _canMove)
        {
            if (_legsOvercharged)
            {
                if (_movementReduced && _myTurn)
                {
                    _currentSteps *= 2;
                    _movementReduced = false;
                }
                else
                    _currentSteps = _legs.GetMaxSteps() * 2;
            }

            else if (_movementReduced && _myTurn)
                _movementReduced = false;
            
            else
                _currentSteps = _legs.CurrentHP > 0 ? _legs.GetMaxSteps() : _legs.GetMaxSteps()/2;
            
            
            PaintTilesInMoveRange(_myPositionTile, 0);
            AddTilesInMoveRange();
        }
    }

    /// <summary>
    /// Deselect Character and clear tiles.
    /// </summary>
    public void DeselectThisUnit()
    {
        if (_isDead)
            return;

        _selected = false;
        _selectingEnemy = false;
        
        if (!_isOnElevator && !_myPositionTile)
            _myPositionTile = GetTileBelow();
        
        if (_myPositionTile)
        {
            _myPositionTile.MakeTileOccupied();
            _myPositionTile.unitAboveSelected = false;
            _myPositionTile.EndMouseOverColor();
        }

        foreach (Character enemy in _enemiesInRange)
        {
            enemy.MechaOutsideAttackRange();
        }

        // if (_canMove)
        //     _currentSteps = legs.GetMaxSteps();
        ResetInRangeLists();
        _path.Clear();
        TileHighlight.Instance.PathLinesClear();
        _waypointsPathfinding.ResetPath();

        ShowMechaMesh();
        ShowGunsMesh();

        OnMechaDeselected?.Invoke();
    }

    /// <summary>
    /// Rotate Character towards enemy.
    /// </summary>
    /// <param name="target">Target to look at.</param>
    public void RotateTowardsEnemy(Transform target)
    {
        Vector3 pos = target.position;
        pos.y = transform.position.y;
        transform.LookAt(pos);
    }

    /// <summary>
    /// Cast a ray to given position. Returns true if collided tag is the same as given tag, false if not.
    /// </summary>
    public bool RayToPartsForAttack(Vector3 partPosition, string tagToCheck, bool drawRays)
    {
        if (partPosition == Vector3.zero)
            return false;
        
        Vector3 position = _rayForBody.gameObject.transform.position;
        Vector3 dir = (partPosition - position).normalized;

        Physics.Raycast(position, dir, out RaycastHit hit, 1000f);
        Transform hitObj = hit.collider.transform;
        //bool goodHit = hitObj.gameObject.CompareTag(tagToCheck) && hitObj.position == partPosition;
        bool targetHit = false;

        LineRenderer renderer = null;
        switch (tagToCheck)
        {
            case "Body":
                if (hitObj.gameObject.CompareTag(tagToCheck))
                {
                    if (hitObj.GetComponent<Body>().GetCharacter().GetUnitTeam() != _unitTeam)
                        targetHit = true;
                    else
                        Debug.Log("es del equipo: " + hitObj.GetComponent<Body>().GetCharacter().GetUnitTeam().ToString());
                }
                else
                    Debug.Log("tag hitteado: " + hitObj.tag + " - tag buscado: " + tagToCheck);
                
                renderer = _rayForBody;
                break;

            case "Legs":
                if (hitObj.gameObject.CompareTag(tagToCheck))
                {
                    if (hitObj.GetComponent<Legs>().GetCharacter().GetUnitTeam() != _unitTeam)
                        targetHit = true;
                    else
                        Debug.Log("es del equipo: " + hitObj.GetComponent<Legs>().GetCharacter().GetUnitTeam().ToString());
                }
                else
                    Debug.Log("tag hitteado: " + hitObj.tag + " - tag buscado: " + tagToCheck);

                renderer = _rayForLegs;
                break;

            case "RGun":
                if (hitObj.gameObject.CompareTag(tagToCheck))
                {
                    if (hitObj.GetComponent<Gun>().GetCharacter().GetUnitTeam() != _unitTeam)
                        targetHit = true;
                    else
                        Debug.Log("es del equipo: " + hitObj.GetComponent<Gun>().GetCharacter().GetUnitTeam().ToString());
                }
                else
                    Debug.Log("tag hitteado: " + hitObj.tag + " - tag buscado: " + tagToCheck);

                renderer = _rayForRightArm;
                break;

            case "LGun":
                if (hitObj.gameObject.CompareTag(tagToCheck))
                {
                    if (hitObj.GetComponent<Gun>().GetCharacter().GetUnitTeam() != _unitTeam)
                        targetHit = true;
                    else
                        Debug.Log("es del equipo: " + hitObj.GetComponent<Gun>().GetCharacter().GetUnitTeam().ToString());
                }
                else
                    Debug.Log("tag hitteado: " + hitObj.tag + " - tag buscado: " + tagToCheck);

                renderer = _rayForLeftArm;
                break;
        }

        
        if (drawRays)
        {
            renderer.positionCount = 2;
            renderer.SetPosition(0, position);
            renderer.SetPosition(1, partPosition);

            if (targetHit)
            {
                renderer.material = _rayHitMaterial;
                Debug.DrawRay(position, dir * 20f, Color.green, 10f);
            }
            else
            {
                renderer.material = _rayMissMaterial;
                Debug.DrawRay(position, dir * 20f, Color.red, 10f);
            }
            
        }

        return targetHit;
    }

    public bool RayToElevator(Vector3 colliderPosition)
    {
        Vector3 position = _rayForBody.gameObject.transform.position;
        Vector3 dir = (colliderPosition - position).normalized;

        Physics.Raycast(position, dir, out RaycastHit hit, 1000f);

        bool goodHit = hit.collider.transform.parent.GetComponent<Elevator>();

        if (goodHit)
            Debug.DrawRay(position, dir * 20f, Color.green, 10f);
        else
            Debug.DrawRay(position, dir * 20f, Color.red, 10f);  

        return goodHit;
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

    public void RaysOffDelay() => StartCoroutine(RaysOffWithDelay());

    IEnumerator RaysOffWithDelay()
    {
        yield return new WaitForSeconds(_raysOffDelay);
        RaysOff();
    }

    public void LegsOverchargeActivate() => _legsOvercharged = true;

    public void LegsOverchargeDeactivate() => _legsOvercharged = false;

    public void AddEquipable(Equipable equipable) => _equipables.Add(equipable);
    #endregion

    #region Getters

    public Body GetBody() => _body;

    // public Arm GetLeftArm()
    // {
    //     return _leftArm;
    // }
    //
    // public Arm GetRightArm()
    // {
    //     return _rightArm;
    // }

    public Legs GetLegs() => _legs;

    public bool AreLegsOvercharged() => _legsOvercharged;
    private void GetTargetToMove()
    {
        Transform target = MouseRay.GetTargetTransform(_block);

        if (!IsValidBlock(target))
            return;

        Tile newTile = target.GetComponent<Tile>();

        if (_targetTile && _targetTile == newTile && _path.Count > 0)
            Move();
        else
        {
            _waypointsPathfinding.Calculate(_myPositionTile, newTile, _currentSteps);
            
            if (!_legsOvercharged)
                if (_waypointsPathfinding.GetDistance() > _legs.GetMaxSteps())
                    return;
                else if (_waypointsPathfinding.GetDistance() > _legs.GetMaxSteps() * 2)
                    return;

            _path = _waypointsPathfinding.GetPath();
            
            if (_path.Count <= 0)
                return;
            
            if (_targetTile)
                TileHighlight.Instance.EndLastTileInPath(_targetTile);

            _targetTile = newTile;

            TileHighlight.Instance.PathPreview(_path);
            ResetTilesInMoveRange();
            ResetTilesInAttackRange();
            TileHighlight.Instance.CreatePathLines(_path);


            if (CanAttack())
                PaintTilesInAttackRange(_targetTile, 0);

            PaintTilesInMoveRange(_targetTile, 0);
            AddTilesInMoveRange();
            TileHighlight.Instance.PaintLastTileInPath(_targetTile);
        }
    }
    //public WorldUI GetMyUI() => _worldUI;

    public int GetCurrentSteps() => _currentSteps;

    public Tile GetEndTile() => _targetTile;

    public EnumsClass.Team GetUnitTeam() => _unitTeam;

    public bool IsSelected() => _selected;

    public Tile GetPositionTile()
    {
        if (!_myPositionTile)
            _myPositionTile = GetTileBelow();
        
        return _myPositionTile;
    }
    public List<Tile> GetPath()
    {
        _path = _waypointsPathfinding.GetPath();
        return _path;
    }

    private Tile GetTileBelow()
    {
        Vector3 pos = _raycastToTile.position;
        //Works at this height after prefab update
        //pos.y = 3;
        Physics.Raycast(pos, Vector3.down, out RaycastHit hit, LayerMask.NameToLayer("GridBlock"));

        Tile tile = hit.transform.gameObject.GetComponent<Tile>();

        return tile;
    }

    public bool HasEnemiesInRange() => _enemiesInRange.Count > 0;

    public Gun GetSelectedGun() => _selectedGun;
    public Gun GetLeftGun() => _leftGun;
    public Gun GetRightGun() => _rightGun;

    public void ResetSelectedGun()
    {
        if (_selectedGun)
            _selectedGun.ResetGun();
    }
    public Vector3 GetBodyPosition() => _body.transform.position;
    public Vector3 GetLArmPosition()
    {
        if (_leftGun)
            return _leftGun.GetObjectCenterCenter();            
        
        return Vector3.zero;
    }
    public Vector3 GetRArmPosition()
    {
        if (_rightGun)
            return _rightGun.GetObjectCenterCenter();

        return Vector3.zero;
    }
    public Vector3 GetLegsPosition() => _legs.transform.position;

    public bool IsMyTurn() => _myTurn;

    public bool IsDead() => _isDead;

    public bool IsUnitEnabled() => _unitEnabled;

    /// <summary>
    /// Return true if Character is selected for an attack.
    /// </summary>
    //public bool IsSelectedForAttack() => _selectedForAttack;

    public bool IsSelectingEnemy() => _selectingEnemy;

    /// <summary>
    /// Return true if Character can be attacked.
    /// </summary>
    public bool CanBeAttacked() => _inAttackRange;

    /// <summary>
    /// Return true if Character can attack.
    /// </summary>
    public bool CanAttack() => _canAttack;

    /// <summary>
    /// Return true if Character is moving.
    /// </summary>
    public bool IsMoving() => _moving;

    public bool CanMove() => _canMove;

    public bool CanBeSelected() => !_equipableSelected && _canBeSelected;

    /// <summary>
    /// Return the Character initiative.
    /// </summary>
    public float GetCharacterInitiative() => _legs.GetLegsInitiative();

    public Sprite GetCharacterSprite() => _myIcon;

    public string GetCharacterName() => _myName;

    public List<Equipable> GetEquipables() => _equipables;

    public void EquipableSelectionState(bool state, Equipable equipable)
    {
        _equipableSelected = state;
        _equipable = equipable;
    }

    public Equipable GetSelectedEquipable() => _equipable;

    public GameObject GetBurningSpawner() => _particleMechaHandler.GetBurningSpawnerFromParticleMechaHandler();

    public bool IsOverweight() => _overweight;

    public bool IsOnElevator() => _isOnElevator;

    //public MaterialMechaHandler GetMaterialHandler() => _materialMechaHandler;

    #endregion

    #region Setters

    /// <summary>
    /// Set if it's Character turn.
    /// </summary>
    public virtual void SetTurn(bool state)
    {
        _myTurn = state;

        if (!_myTurn)
            return;

        OnMechaTurnStart?.Invoke();

        if (_body.IsSmokeScreenActive())
            _body.DeactivateSmokeScreen();
    }

    /// <summary>
    /// Set if Character is selected for an Attack.
    /// </summary>
    public void SelectedForAttack() => _selectedForAttack = true;
    public void NotSelectedForAttack() => _selectedForAttack = false;

    /// <summary>
    /// Set if Character is selecting an enemy.
    /// </summary>
    public void SelectingEnemy()
    {
        _selectingEnemy = true;

        ResetTilesInAttackRange();
        ResetTilesInMoveRange();
        HideMechaMesh();
        OnSelectingEnemy?.Invoke();            
    }

    public void CancelEnemySelection() => _selectingEnemy = false;

    public void SetTargetTile(Tile target) => _targetTile = target;

    /// <summary>
    /// Set if Character can move.
    /// </summary>
    public void SetCharacterMoveState(bool state)
    {
        _canMove = state;
        OnMoveActionStateChange?.Invoke(this, _canMove);
    }

    public void SetSelection(bool state) => _selected = state;

    #endregion

    #region Utilities

    //Se pintan los tiles dentro del rango de ataque
    public void PaintTilesInAttackRange(Tile currentTile, int count)
    {
        if (!_leftGunAlive && !_rightGunAlive)
            return;

        if (_selectedGun == null || count >= _selectedGun.GetAttackRange() || (_tilesForAttackChecked.ContainsKey(currentTile) && _tilesForAttackChecked[currentTile] <= count))
            return;
        
        if (_isOnElevator && _selectedGun.GetGunType() == EnumsClass.GunsType.Melee)
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
                        TileHighlight.Instance.PaintTilesInMoveAndAttackRange(tile);
                    else
                        TileHighlight.Instance.PaintTilesInAttackRange(tile);
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
                            TileHighlight.Instance.PaintTilesInMoveAndAttackRange(tile);
                        else
                            TileHighlight.Instance.PaintTilesInMoveRange(tile);
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
        TileHighlight.Instance.ClearTilesInAttackRange(_tilesInAttackRange);
        _tilesInAttackRange.Clear();
        _tilesForAttackChecked.Clear();
    }

    /// <summary>
    /// Unpaint Tiles in Character move range.
    /// </summary>
    public void ResetTilesInMoveRange()
    {
        TileHighlight.Instance.ClearTilesInMoveRange(_tilesInMoveRange);
        _tilesInMoveRange.Clear();
        _tilesForMoveChecked.Clear();
    }

    /// <summary>
    /// Clear  lists of enemies in attack range, tiles in move and attack range. 
    /// </summary>
    public void ResetInRangeLists()
    {
        foreach (Character enemy in _enemiesInRange)
        {
            enemy.MechaOutsideAttackRange();
        }

        TileHighlight.Instance.PathLinesClear();

        if (_path.Count > 0)
            TileHighlight.Instance.EndLastTileInPath(_path[_path.Count - 1]);

        ResetTilesInMoveRange();
        ResetTilesInAttackRange();
        TileHighlight.Instance.Undo();
        _enemiesInRange.Clear();
    }

    protected void AddTilesInMoveRange() => TileHighlight.Instance.AddTilesInMoveRange(_tilesInMoveRange);

    /// <summary>
    /// Reset Character for new turn.
    /// </summary>
    public virtual void EndTurn()
    {
        if (_isDead)
            return;

        _legsOvercharged = false;

        if (_rightGun)
        {
            _rightGun.Deselect();
            _rightGun.ResetGun();
        }

        if (_leftGun)
        {
            _leftGun.Deselect();
            _leftGun.ResetGun();
        }

        _myTurn = false;

        SetCharacterMoveState(true);

        SetAttackActionState(true);

        _path.Clear();
        _currentSteps = _legs.CurrentHP > 0 ? _legs.GetMaxSteps() : _legs.GetMaxSteps() / 2;
        _enemiesInRange.Clear();
        _inAttackRange = false;
        _waypointsPathfinding.ResetPath();

        foreach (Equipable equipable in _equipables)
        {
            equipable.UpdateEquipableState();
        }
    }

    public void ClearTargetTile() => _targetTile = null;

    /// <summary>
    /// Executed when Character reached the end of the path.
    /// </summary>
    public virtual void ReachedEnd()
    {
        SetCharacterMoveState(false);

        _legsOvercharged = false;
        TileHighlight.Instance.characterMoving = false;
        TileHighlight.Instance.EndPreview();
        _moving = false;
        SavedRotation = transform.rotation; //Cambio Nico

        if (_myPositionTile)
        {
            _myPositionTile.MakeTileFree();
            _myPositionTile.SetUnitAbove(null);
        }

        if (_targetTile)
            _myPositionTile = _targetTile;
        else
            _targetTile = GetTileBelow();

        _myPositionTile.MakeTileOccupied();
        _myPositionTile.SetUnitAbove(this);
        _myPositionTile.unitAboveSelected = true;
        _myPositionTile.MouseOverColor();
        _targetTile = null;
        //TurnManager.Instance.UnitStoppedMoving();
        _waypointsPathfinding.ResetPath();
        _tilesForAttackChecked.Clear();
        _tilesInAttackRange.Clear();
        _animationMechaHandler.SetIsWalkingAnimatorFalse();
        _audioMechaHandler.SetMuteWalk();
        _particleMechaHandler.SetMachineOn(false);

        if (!CanAttack())
            return;

        PaintTilesInAttackRange(_myPositionTile, 0);
        CheckEnemiesInAttackRange();

        //ButtonsUIManager.Instance.ActivateEquipablesButtons();
        OnEndMove?.Invoke();
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
        else
            _currentSteps += amount;

    }

    /// <summary>
    /// Make Character not selectable.
    /// </summary>
    public void NotSelectable() => _canBeSelected = false;

    public void Dead()
    {
        if (_isDead)
            return;

        _canBeSelected = false;
        _isDead = true;
        
        OnMechaDeath?.Invoke(this);
    }

    /// <summary>
    /// Make Character selected as an enemy.
    /// </summary>
    public void SelectedAsEnemy()
    {
        if (_isDead)
            return;

        _myPositionTile = GetTileBelow();

        if (!_myPositionTile)
            return;

        _myPositionTile.unitAboveSelected = true;
        _myPositionTile.GetComponent<TileMaterialhandler>().DiseableAndEnableSelectedNode(true);
    }

    /// <summary>
    /// Check if given Target is a valid GridBlock to move.
    /// </summary>
    private bool IsValidBlock(Transform target)
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return false;

        if (!target)
            return false;

        if (target.gameObject.layer != LayerMask.NameToLayer("GridBlock"))
            return false;

        Tile tile = target.gameObject.GetComponent<Tile>();

        return (tile && tile.IsWalkable() && tile.IsFree() && tile.inMoveRange) || tile == _targetTile;
    }


    private void CheckEnemiesInAttackRange()
    {
        if (_tilesInAttackRange == null || _tilesInAttackRange.Count <= 0)
            return;

        foreach (Character enemy in _enemiesInRange)
        {
            enemy.MechaOutsideAttackRange();
        }

        _enemiesInRange.Clear();

        foreach (Tile tile in _tilesInAttackRange)
        {
            if (!tile.IsFree())
            {
                Character mecha = tile.GetUnitAbove();
                
                if (mecha.GetUnitTeam() == _unitTeam)
                    continue;
                
                if (mecha.IsDead())
                    continue;

                mecha.MechaInAttackRange();
                _enemiesInRange.Add(mecha);
            }
        }
    }

    public void SetAttackActionState(bool state)
    {
        _canAttack = state;
        OnAttackActionStateChange?.Invoke(this, _canAttack);
    }
    public void MechaInAttackRange() => _inAttackRange = true;

    public void MechaOutsideAttackRange() => _inAttackRange = false;

    public void DeactivateAttack()
    {
        ResetTilesInAttackRange();

        SetAttackActionState(false);
    }

    private void OnMouseOver()
    {
        if (_isDead)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!_selectedForAttack && _canBeSelected)
            OnMouseOverMecha?.Invoke(this);
            //_worldUI.Show();
            //SetWorldUIValues();

        if (!_selected)
            return;

        if (_inAttackRange && !_selectedForAttack)
        {
            if (GameManager.Instance.ActiveTeam == EnumsClass.Team.Red)
                return;
            
            RotateWithRays();
        }
    }

    private void OnMouseExit()
    {
        if (_isDead)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        OnMouseExitMecha?.Invoke(this);
        
        if (_inAttackRange)
            ResetRotationAndRays();
    }

    private void RotateWithRays()
    {
        Character selectedCharacter = GameManager.Instance.CurrentTurnMecha;

        if (!selectedCharacter.IsSelected()) 
            return;
        
        if (selectedCharacter.IsMoving()) 
            return;
        
        if (selectedCharacter._rotated) 
            return;

        selectedCharacter._rotated = true;

        selectedCharacter.SaveRotation();

        Vector3 posToLook = transform.position;

        posToLook.y = selectedCharacter.transform.position.y;

        selectedCharacter.transform.LookAt(posToLook);
        
        bool body = selectedCharacter.RayToPartsForAttack(GetBodyPosition(), "Body", true) && _body.CurrentHP > 0;
        bool lArm = selectedCharacter.RayToPartsForAttack(GetLArmPosition(), "LGun", true) && _leftGun;
        bool rArm = selectedCharacter.RayToPartsForAttack(GetRArmPosition(), "RGun", true) && _rightGun;
        bool legs = selectedCharacter.RayToPartsForAttack(GetLegsPosition(), "Legs", true) && _legs.CurrentHP > 0;
    }

    public void ResetRotationAndRays()
    {
        Character selectedCharacter = GameManager.Instance.CurrentTurnMecha;

        if (selectedCharacter == null) 
            return;

        if (selectedCharacter.IsSelectingEnemy()) 
            return;
        
        selectedCharacter._rotated = false;
        //c._move.StopRotation();
        selectedCharacter.LoadRotation(); //Volver la rotación del mecha a InitialRotation, esto podría ser más smooth
        selectedCharacter.RaysOff(); //Apago los raycasts cuando saco el mouse
    }

    public void SaveRotation() => SavedRotation = transform.rotation;
    public void LoadRotation() => transform.rotation = SavedRotation;

    //public WorldUI GetWorldUI() => _worldUI;

    //public void SetWorldUIValues()
    //{
    //    //float rightGunHP = 0;

    //    //if (_rightGun)
    //    //    rightGunHP = _rightGun.CurrentHP;
        
    //    //float leftGunHP = 0;

    //    //if (_leftGun)
    //    //    leftGunHP = _leftGun.CurrentHP;
        
    //    //_worldUI.SetWorldUIValues(_body.CurrentHP, rightGunHP, leftGunHP, _legs.CurrentHP, _canMove, _canAttack, _overweight);
    //    _worldUI.Show();
    //}

    #endregion

    #region Others

    public void SetHurtAnimation() => _animationMechaHandler.SetIsReciveDamageAnimatorTrue();

    //public void HitSoundMecha()
    //{
    //    AudioManager.audioManagerInstance.PlaySound(soundHit, this.gameObject);
    //}

    public void OnUseEquipable() => EquipableSelectionState(false, null);

    public Item GetItem() => _item;
    #endregion

    public void SetEquipment(MechaEquipmentSO equipment) => _mechaEquipment = equipment;

    protected virtual void ConfigureMecha()
    {
        if (!_mechaEquipment)
            return;

        _myName = _mechaEquipment.mechaName;

        _body.SetPartData(this, _mechaEquipment.body, _mechaEquipment.GetBodyColor());
        _body.SetAbilityData(_mechaEquipment.bodyAbility);

        _leftGun = Instantiate(_mechaEquipment.leftGun.prefab, _leftGunSpawn.transform);

        if (_leftGun)
        {
            _leftGun.transform.localPosition = Vector3.zero;
            _leftGun.SetAnimationHandler(_animationMechaHandler);
            _leftGun.SetGunData(_mechaEquipment.leftGun, this, "LGun", "Left");
            _leftGun.SetAbilityData(_mechaEquipment.leftGunAbility);
            _leftGunAlive = true;
        }

        _rightGun = Instantiate(_mechaEquipment.rightGun.prefab, _rightGunSpawn.transform);

        if (_rightGun)
        {
            _rightGun.transform.localPosition = Vector3.zero;
            _rightGun.SetAnimationHandler(_animationMechaHandler);
            _rightGun.SetGunData(_mechaEquipment.rightGun, this, "RGun", "Right");
            _rightGun.SetAbilityData(_mechaEquipment.rightGunAbility);
            _rightGunAlive = true;
        }

        _legs.SetPartData(this, _mechaEquipment.legs, _mechaEquipment.GetLegsColor());
        _legs.SetAbilityData(_mechaEquipment.legsAbility);

        if (_mechaEquipment.item)
        {
            _item = Instantiate(_mechaEquipment.item.itemPrefab, transform);
            _item.Initialize(this, _mechaEquipment.item);
        }
            
        SetCharacterMoveState(true);

        _currentSteps = _legs.GetMaxSteps();

        CheckWeight();

        if (_rightGun)
        {
            _selectedGun = _rightGun;

            //if (_selectedGun.GetGunType() == GunsType.Shield)
            //    _selectedGun.Ability();

            SetAttackActionState(true);
            _rightGunSelected = true;
            _leftGunSelected = false;
            return;
        }
        
        if (_leftGun)
        {
            _selectedGun = _leftGun;

            //if (_selectedGun.GetGunType() == GunsType.Shield)
            //    _selectedGun.Ability();

            SetAttackActionState(true);
            _rightGunSelected = false;
            _leftGunSelected = true;

            return;
        }

        _selectedGun = null;
        _leftGun = null;
        _rightGun = null;
        _rightGunSelected = false;
        _leftGunSelected = false;
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
        _body.SetShader(texture);

        _legs.SetShader(texture);

        if (_rightGun)
            _rightGun.SetShader(texture);

        if (_leftGun)
            _leftGun.SetShader(texture);
    }

    public void CheckWeight()
    {
        float totalWeight = 0;

        if (!_body)
            return;

        totalWeight += _body.GetWeight();

        if (_leftGun)
            totalWeight += _leftGun.GetWeight();

        if (_rightGun)
            totalWeight += _rightGun.GetWeight();
        
        totalWeight += _legs.GetWeight();

        if (totalWeight <= _body.GetMaxWeight())
            _overweight = false;
        else 
            _overweight = true;
        
        OnOverweight?.Invoke(this, _overweight);
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
            _equipables.Remove(ability);

        CheckWeight();
    }

    public void CharacterElevatedState(bool state, int extraRange, int extraCrit)
    {
        _isOnElevator = state;

        if (_leftGun)
        {
            _leftGun.ModifyRange(extraRange);
            _leftGun.ModifyCritChance(extraCrit);  
        }

        if (_rightGun)
        {
            _rightGun.ModifyRange(extraRange);
            _rightGun.ModifyCritChance(extraCrit); 
        }
    }

    public void TakeFallDamage(float dmgPercentage)
    {
        float legsDamage = _legs.MaxHp * dmgPercentage / 100;
        _legs.ReceiveDamage((int)legsDamage);

        if (_leftGun)
        {
            float lGunDamage = _leftGun.MaxHP * dmgPercentage / 100;
            _leftGun.ReceiveDamage((int)lGunDamage);
        }


        if (_rightGun)
        {
            float rGunDamage = _rightGun.MaxHP * dmgPercentage / 100;
            _rightGun.ReceiveDamage((int)rGunDamage);
        }

        float bodyDamage = _body.MaxHp * dmgPercentage / 100;
        _body.ReceiveDamage((int)bodyDamage);
        GetComponent<Rigidbody>().isKinematic = true;

        Vector3 pos = transform.position;
        pos.y = _startingHeight;
        transform.position = pos;
    }

    public void MovementReduction(int amount)
    {
        _movementReduced = true;
        
        _currentSteps = _legs.CurrentHP > 0 ? _legs.GetMaxSteps() : _legs.GetMaxSteps()/2;

        _currentSteps -= amount;
    }

    public void DisableUnit() => _unitEnabled = false;

    public void EnableUnit() => _unitEnabled = true;

    private void OnEnable()
    {
        if (_isDead)
            _animationMechaHandler.SetIsDeadAnimatorTrue();
    }
    
    public LayerMask GetBlockLayerMask() => _block;

    public WaypointsPathfinding GetWaypointsPathfinding() => _waypointsPathfinding;

    public void ShowMechaMesh()
    {
        _body.ChangePartMeshActiveStatus(true);
        _legs.ChangePartMeshActiveStatus(true);
    }
    public void HideMechaMesh()
    {
        _body.ChangePartMeshActiveStatus(false);
        _legs.ChangePartMeshActiveStatus(false);
    }
    public void ShowGunsMesh()
    {
        if (_leftGun)
            _leftGun.ChangeMeshRenderStatus(true);

        if (_rightGun)
            _rightGun.ChangeMeshRenderStatus(true);
    }

    public void HideGunsMesh()
    {
        if (_leftGun)
            _leftGun.ChangeMeshRenderStatus(false);

        if (_rightGun)
            _rightGun.ChangeMeshRenderStatus(false);
    }

    public bool IsEnemyInSight(Character enemy)
    {
        bool body = IsEnemyBodyInSight(enemy);

        bool lArm = IsEnemyLeftGunInSight(enemy);

        bool rArm = IsEnemyRightGunInSight(enemy);

        bool legs = IsEnemyLegsInSight(enemy);

        return body || lArm || rArm || legs;
    }

    public bool IsEnemyBodyInSight(Character enemy) => enemy.GetBody().CurrentHP > 0 && RayToPartsForAttack(enemy.GetBodyPosition(), "Body", false);
    public bool IsEnemyLeftGunInSight(Character enemy) => enemy.GetLeftGun() && RayToPartsForAttack(enemy.GetLArmPosition(), "LGun", false);
    public bool IsEnemyRightGunInSight(Character enemy) => enemy.GetRightGun() && RayToPartsForAttack(enemy.GetRArmPosition(), "RGun", false);
    public bool IsEnemyLegsInSight(Character enemy) => enemy.GetLegs().CurrentHP > 0 && RayToPartsForAttack(enemy.GetLegsPosition(), "Legs", false);
}
