using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.Effects;
using Random = System.Random;

public class Character : Teams
{
    //STATS
    #region Stats

    public Transform rayOrigin;
    [Header("Team")]
    [SerializeField] private Team _unitTeam;

    [Header("Body")] 
    public Body body;
    [SerializeField] private Transform _bodyTransform;

    [Header("Left Arm")]
    public Arm leftArm;
    [SerializeField] private Gun _leftGun;
    private bool _leftGunSelected;
    [SerializeField] private Transform _lArmTransform;

    [Header("Right Arm")]
    public Arm rightArm;
    [SerializeField] private Gun _rightGun;
    private bool _rightGunSelected;
    [SerializeField] private Transform _rArmTransform;

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
    private bool _canMove = true;
    public bool _canAttack = true;
    private bool _leftArmAlive;
    private bool _rightArmAlive;
    private bool _canBeAttacked = false;
    public bool selectingEnemy = false;
    public bool selectedForAttack;
    public bool myTurn = false;

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
    private Animator _animator;

    [HideInInspector]
	public EffectsController effectsController;
    [HideInInspector]
    public TurnManager turnManager;
	[HideInInspector]
	public TileHighlight highlight;
	[HideInInspector]
	public ButtonsUIManager buttonsManager;

    // Start is called before the first frame update
    void Start()
    {
        _canBeSelected = true;
        if (_leftGun)
        {
            
            _leftArmAlive = leftArm.GetCurrentHP() > 0 ? true : false;
        }

        if (_rightGun)
        {
            _rightArmAlive = rightArm.GetCurrentHP() > 0 ? true : false;
        }
        leftArm.SetRightOrLeft("Left");
        rightArm.SetRightOrLeft("Right");

        _canMove = legs.GetCurrentHP() > 0 ? true : false;
        _currentSteps = _canMove ? legs.GetMaxSteps() : 0;
        _selected = false;

        _mechaMaterlaHandler = GetComponent<MechaMaterialhandler>();
        _smokeMechaHandler = GetComponent<SmokeMechaHandler>();
        _move = GetComponent<GridMovement>();
        _move.SetMoveSpeed(legs.GetMoveSpeed());
        _move.SetRotationSpeed(legs.GetRotationSpeed());
        _myPositionTile = GetTileBelow();
        _myPositionTile.MakeTileOccupied();
        _myPositionTile.SetUnitAbove(this);
        turnManager = FindObjectOfType<TurnManager>();
        highlight = FindObjectOfType<TileHighlight>();
        buttonsManager = FindObjectOfType<ButtonsUIManager>();
        pathCreator = GetComponent<IPathCreator>();
        effectsController = FindObjectOfType<EffectsController>();
        _myUI = GetComponent<WorldUI>();
        _animator = GetComponent<Animator>();

        if (_myUI)
            _myUI.SetLimits(body.GetMaxHP(), rightArm.GetMaxHP(), leftArm.GetMaxHP(), legs.GetMaxHP());

        if (_rightArmAlive)
        {
            _selectedGun = _rightGun;
            _canAttack = true;
            _rightGunSelected = true;
            _leftGunSelected = false;
        }
        else if (_leftArmAlive)
        {
            _selectedGun = _leftGun;
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

        if (body.GetCurrentHP() <= 0)
            NotSelectable();

        for (int i = 0; i < transform.childCount; i++)
        {
            var c = transform.GetChild(i);
            if (c.gameObject.name == "Body")
            {
                _bodyTransform = c;
                continue;
            }
                
            if (c.gameObject.name == "Legs")
            {
                _legsTransform = c;
                continue;
            }

            if (c.gameObject.name == "RArm")
            {
                _rArmTransform = c;
                continue;
            }

            if (c.gameObject.name == "LArm")
                _lArmTransform = c;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_selected && !_moving && _canMove && !selectingEnemy && Input.GetMouseButtonDown(0))
        {
            GetTargetToMove();
        }
    }

    //Se pintan los tiles dentro del rango de ataque
    void PaintTilesInAttackRange(Tile currentTile, int count)
    {
        if (count >= _selectedGun.GetAttackRange() || (_tilesForAttackChecked.ContainsKey(currentTile) && _tilesForAttackChecked[currentTile] <= count))
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
    public void PaintTilesInMoveRange(Tile currentTile, int count)
    {
        if (count >= _currentSteps || (_tilesForMoveChecked.ContainsKey(currentTile) && _tilesForMoveChecked[currentTile] <= count))
            return;

        _tilesForMoveChecked[currentTile] = count;

        foreach (var tile in currentTile.neighboursForMove)
        {
            if (!_tilesInMoveRange.Contains(tile))
            {
                if (tile.IsWalkable() && tile.IsFree())
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

    public void AddTilesInMoveRange()
    {
        highlight.AddTilesInMoveRange(_tilesInMoveRange);
    }

    #region Actions
    public void Move()
    {
        if (_moving == false && _path != null && _path.Count > 0)
        {
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
            _animator.SetBool("isWalkingAnimator", true);
            AudioManager.audioManagerInstance.PlaySound(soundMotorStart, this.gameObject);
            AudioManager.audioManagerInstance.PlaySound(soundWalk, this.gameObject);
            _smokeMechaHandler.SetMachineOn(true);
            _move.StartMovement(_path);
        }
    }
    
    public void SelectLeftGun()
    {
        _selectedGun = _leftGun;
        _leftGunSelected = true;
        _rightGunSelected = false;
        ResetTilesInAttackRange();
        ResetTilesInMoveRange();
        if (_canAttack)
        {
            if (_path.Count == 0)
                PaintTilesInAttackRange(_myPositionTile, 0);
            else PaintTilesInAttackRange(_path[_path.Count - 1], 0);
            CheckEnemiesInAttackRange();
        }

        if (_canMove)
        {
            if (_path.Count == 0)
                PaintTilesInMoveRange(_myPositionTile, 0);
            else PaintTilesInMoveRange(_path[_path.Count - 1], 0);
        }
    }

    public void SelectRightGun()
    {
        _selectedGun = _rightGun;
        _leftGunSelected = false;
        _rightGunSelected = true;
        ResetTilesInAttackRange();
        ResetTilesInMoveRange();
        if (_canAttack)
        {
            if (_path.Count == 0)
                PaintTilesInAttackRange(_myPositionTile, 0);
            else PaintTilesInAttackRange(_path[_path.Count - 1], 0);
            CheckEnemiesInAttackRange();
        }
        
        if (_canMove)
        {
            if (_path.Count == 0)
                PaintTilesInMoveRange(_myPositionTile, 0);
            else PaintTilesInMoveRange(_path[_path.Count - 1], 0);
        }
    }

    //Despinta las tiles en rango de ataque
    public void ResetTilesInAttackRange()
    {
        highlight.ClearTilesInAttackRange(_tilesInAttackRange);
        _tilesInAttackRange.Clear();
        _tilesForAttackChecked.Clear();
    }

    //Despinta las tiles en rango de movimiento
    public void ResetTilesInMoveRange()
    {
        highlight.ClearTilesInMoveRange(_tilesInMoveRange);
        _tilesInMoveRange.Clear();
        _tilesForMoveChecked.Clear();
    }

    #endregion

    #region Getters
    public void GetTargetToMove()
    {
        Transform target = MouseRay.GetTargetTransform(block);
        
        if (IsValidTarget(target))
        {
            var newTile = target.GetComponent<Tile>();
            //LLAMAR A MOVE
            if (_targetTile && _targetTile == newTile && _path.Count > 0)
            {
                Move();
            } 
            else
            {
                _targetTile = newTile;
                pathCreator.Calculate(this, _targetTile, _currentSteps);
                if (pathCreator.GetDistance() <= legs.GetMaxSteps())
                {
                    _path = pathCreator.GetPath();
                    if (_path.Count > 0)
                    {
                        highlight.PathPreview(_path);
                        ResetTilesInMoveRange();
                        ResetTilesInAttackRange();
                        highlight.CreatePathLines(_path);
                        
                        if (CanAttack())
                            PaintTilesInAttackRange(_path[_path.Count - 1], 0);

                        PaintTilesInMoveRange(_path[_path.Count - 1], 0);
                        AddTilesInMoveRange();
                    }
                }
            }
        }
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

    public Tile GetActualTilePosition()
    {
        return _myPositionTile;
    }

    public List<Tile> GetPath()
    {
        _path = pathCreator.GetPath();
        return _path;
    }

    public Tile GetTileBelow()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, LayerMask.NameToLayer("GridBlock"));
        return hit.transform.gameObject.GetComponent<Tile>();
    }
    
    public bool HasEnemiesInRange()
    {
        return _enemiesInRange.Count > 0 ? true : false;
    }

    public Gun GetSelectedGun()
    {
        return _selectedGun;
    }

    public Gun GetLeftGun()
    {
        return _leftGun;
    }

    public Gun GetRightGun()
    {
        return _rightGun;
    }

    public Vector3 GetBodyPosition()
    {
        return _bodyTransform.GetComponent<Renderer>().bounds.center;
    }
    public Vector3 GetLArmPosition()
    {
        return _lArmTransform.GetComponent<Renderer>().bounds.center;
    }
    public Vector3 GetRArmPosition()
    {
        return _rArmTransform.GetComponent<Renderer>().bounds.center;
    }
    public Vector3 GetLegsPosition()
    {
        return _legsTransform.GetComponent<Renderer>().bounds.center;
    }

    #endregion

    #region Utilities
    
    public void Undo()
    {
        ResetInRangeLists();
        GetPath();
        
        if (_path.Count > 1)
        {
            _targetTile = _path[_path.Count - 1];
            PaintTilesInMoveRange(_path[_path.Count - 1], 0);
            if (CanAttack())
                PaintTilesInAttackRange(_path[_path.Count - 1], 0);
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

    public void ResetInRangeLists()
    {
        foreach (var item in _enemiesInRange)
        {
            turnManager.UnitCantBeAttacked(item);
        }
        highlight.PathLinesClear();
        ResetTilesInMoveRange();
        ResetTilesInAttackRange();
        highlight.Undo();
        _enemiesInRange.Clear();
    }
    public void NewTurn()
    {
        myTurn = false;
        _canMove = legs.GetCurrentHP() > 0 ? true : false;
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
        _animator.SetBool("isWalkingAnimator", false);
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

    public void ReduceAvailableSteps(int amount)
    {
        var steps = _currentSteps - amount;
        _currentSteps = steps >= 0 ? steps : 0;
    }

    public void IncreaseAvailableSteps(int amount)
    {
        var steps = _currentSteps + amount;
        _currentSteps = steps <= legs.GetMaxSteps() ? steps : legs.GetMaxSteps();
    }

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

    public void DeselectThisUnit()
    {
        _selected = false;
        selectingEnemy = false;
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
    }

    public void NotSelectable()
    {
        _canBeSelected = false;
    }
    
    public void SelectedAsEnemy()
    {
        _myPositionTile = GetTileBelow();
        if (_myPositionTile)
        {
            _myPositionTile.unitAboveSelected = true;
            _myPositionTile.GetComponent<TileMaterialhandler>().DiseableAndEnableSelectedNode(true);
        }
    }

    //Check if selected object is a tile.
    private bool IsValidTarget(Transform target)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return false;
        
        if (!target) return false;
        
        if (target.gameObject.layer != LayerMask.NameToLayer("GridBlock")) return false;
        
        var tile = target.gameObject.GetComponent<Tile>();
        
        return (tile && tile.IsWalkable() && tile.IsFree() && tile.inMoveRange) || tile == _targetTile;
    }

    void CheckEnemiesInAttackRange()
    {
        if (_tilesInAttackRange != null && _tilesInAttackRange.Count > 0)
        {
            foreach (var unit in _enemiesInRange)
            {
                turnManager.UnitCantBeAttacked(unit);
            }
            _enemiesInRange.Clear();
            foreach (var item in _tilesInAttackRange)
            {
                if (item.IsFree() == false)
                {
                    var unit = item.GetUnitAbove();
                    if (unit.GetUnitTeam() != _unitTeam)
                    {
                        turnManager.UnitCanBeAttacked(unit);
                        _enemiesInRange.Add(unit);
                    }
                }
            }
        }
    }
    
    public bool ThisUnitCanMove()
    {
        return _canMove;
    }

    public bool IsMoving()
    {
        return _moving;
    }

    public void SetTargetTile(Tile target)
    {
        _targetTile = target;
    }

    public void MakeAttackable()
    {
        _canBeAttacked = true;
    }

    public void MakeNotAttackable()
    {
        _canBeAttacked = false;
    }

    public bool CanBeAttacked()
    {
        return _canBeAttacked;
    }

    public bool CanAttack()
    {
        return _canAttack;
    }

    public void CheckArms()
    {
        if (!LeftArmAlive() && !RightArmAlive())
            _canAttack = false;
    }
    
    public bool LeftArmAlive()
    {
        _leftArmAlive = leftArm.GetCurrentHP() > 0 ? true : false;
        return _leftArmAlive;
    }
    
    public bool RightArmAlive()
    {
        _rightArmAlive = rightArm.GetCurrentHP() > 0 ? true : false;
        return _rightArmAlive;
    }

    //DESCOMENTAR CUANDO SE IMPLEMENTE
    public bool CanBeSelected()
    {
        return _canBeSelected && myTurn;
    }
    public void DeactivateAttack()
    {
        ResetTilesInAttackRange();
        ResetTilesInMoveRange();
        highlight.PathLinesClear();
        _canAttack = false;
    }

    public float CalculateInitiative()
    {
        return legs.GetCurrentHP() / legs.GetMaxHP() * 100 + legs.GetLegsInitiative();
    }
    #endregion

    private void OnMouseOver()
    {
        if (!selectedForAttack)
            ShowWorldUI();
    }

    private void OnMouseExit()
    {
        HideWorldUI();
    }

    public void ShowWorldUI()
    {
        _myUI.SetWorldUIValues(body.GetCurrentHP(), rightArm.GetCurrentHP(), leftArm.GetCurrentHP(), legs.GetCurrentHP(), _canMove, _canAttack);
        _myUI.ContainerActivation(true);
    }

    public void HideWorldUI()
    {
        _myUI.DeactivateWorldUI();
    }

    public void RotateTowardsEnemy(Vector3 pos)
    {
        _move.SetPosToRotate(pos);
        _move.StartRotation();
    }
    
    public void RotateTowardsEnemy(Vector3 pos, Action callback)
    {
        _move.SetPosToRotate(pos);
        _move.StartRotation(callback);
    } 

    public bool RayToPartsForAttack(Vector3 pos, string tagToCheck)
    {
        Debug.Log("check " + tagToCheck);
        RaycastHit hit;
        var position = rayOrigin.position;
        var dir = (pos - position).normalized; 
        Physics.Raycast(position, dir, out hit, 1000f);
        if (hit.collider.transform.gameObject.CompareTag(tagToCheck))
        {
            Debug.DrawRay(position, dir * 20f, Color.green, 1000f);
            return true;
        }
        
        Debug.DrawRay(position, dir * 20f, Color.red, 1000f);
        return false;
    }

    public void SetCharacterMove(bool state)
    {
        _canMove = state;
    }

    //Lo llama el ButtonsUIManager, activa el efecto de ataque
    public void Shoot()
    {
        if (_rightGunSelected)
        {
            effectsController.PlayParticlesEffect(_rArmTransform.position, "Attack");
        }
        else if (_leftGunSelected)
        {
            effectsController.PlayParticlesEffect(_lArmTransform.position, "Attack");
        }
        
    }

    public void SetHurtAnimation()
    {
        _animator.SetTrigger("isReciveDamageAnimator");
    }

    public WorldUI GetMyUI()
    {
        return _myUI;
    }
}
