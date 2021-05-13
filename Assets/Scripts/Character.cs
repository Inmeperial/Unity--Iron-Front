using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : Teams
{
    //STATS
    #region Stats
    [Header("Team")]
    [SerializeField] private Team _unitTeam;

    [Header("Body")]
    [SerializeField] private int _bodyMaxHP;
    [SerializeField] private int _bodyHP;

    [Header("Left Arm")]
    public Gun leftGun;
    [SerializeField] private int _leftArmMaxHP;
    [SerializeField] private int _leftArmHP;

    [Header("Right Arm")]
    public Gun rightGun;
    [SerializeField] private int _rightArmMaxHP;
    [SerializeField] private int _rightArmHP;

    [Header("Legs")]
    [SerializeField] private int _legsMaxHP;
    [SerializeField] private int _legsHP;


    [Header("Movement")]
    [SerializeField] private int _maxSteps;
    [SerializeField] private int _currentSteps;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    #endregion

    private Gun _selectedGun;

    //MOVEMENT RELATED
    public IPathCreator pathCreator;
    private GridMovement _move;
    public LayerMask block;
    private List<Tile> _tilesInMoveRange = new List<Tile>();
    [SerializeField] private Tile _myPositionTile;
    private Tile _targetTile;
    [SerializeField] private List<Tile> _path = new List<Tile>();

    //FLAGS
    private bool _canBeSelected;
    private bool _selected;
    private bool _moving = false;
    private bool _canMove = true;
    [SerializeField] private bool _canAttack = true;
    [SerializeField] private bool _leftArmAlive;
    [SerializeField] private bool _rightArmAlive;
    private bool _canBeAttacked = false;

    //OTHERS
    private HashSet<Tile> _tilesInAttackRange = new HashSet<Tile>();
    private Dictionary<Tile, int> _tilesForAttackChecked = new Dictionary<Tile, int>();
    private Dictionary<Tile, int> _tilesForMoveChecked = new Dictionary<Tile, int>();
    private List<Character> _enemiesInRange = new List<Character>();
    private WorldUI _myUI;

    [Header("DON'T SET")]
    public TurnManager turnManager;

    public TileHighlight highlight;

    public ButtonsUIManager buttonsManager;

    // Start is called before the first frame update
    void Start()
    {
        _canBeSelected = true;
        _bodyHP = _bodyMaxHP;
        _leftArmHP = _leftArmMaxHP;
        _leftArmAlive = _leftArmHP > 0 ? true : false;
        _rightArmHP = _rightArmMaxHP;
        _rightArmAlive = _rightArmHP > 0 ? true : false;
        _legsHP = _legsMaxHP;
        _canMove = _legsHP > 0 ? true : false;
        _currentSteps = _canMove ? _maxSteps : 0;
        _selected = false;
        
        _move = GetComponent<GridMovement>();
        _move.SetMoveSpeed(_moveSpeed);
        _move.SetRotationSpeed(_rotationSpeed);
        _myPositionTile = GetTileBelow();
        _myPositionTile.MakeTileOccupied();
        _myPositionTile.SetUnitAbove(this);
        turnManager = FindObjectOfType<TurnManager>();
        highlight = FindObjectOfType<TileHighlight>();
        buttonsManager = FindObjectOfType<ButtonsUIManager>();
        pathCreator = GetComponent<IPathCreator>();
        _myUI = GetComponent<WorldUI>();

        if (_myUI)
            _myUI.SetLimits(_bodyMaxHP, _rightArmMaxHP, _leftArmMaxHP, _legsMaxHP);

        if (_rightArmAlive)
        {
            _selectedGun = rightGun;
            _canAttack = true;
        }
        else if (_leftArmAlive)
        {
            _selectedGun = leftGun;
            _canAttack = true;
        }
        else
        {
            _selectedGun = null;
            _canAttack = false;
        }

        if (_bodyHP <= 0)
            NotSelectable();
    }

    // Update is called once per frame
    void Update()
    {
        if (_selected && !_moving && _canMove && Input.GetMouseButtonDown(0))
        {
            GetTargetToMove();
        }
    }

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
    //This method is called from UI Button "Move".
    public void Move()
    {
        if (_moving == false && _path != null && _path.Count > 0)
        {
            _moving = true;
            buttonsManager.DeactivateBodyPartsContainer();
            buttonsManager.DeactivateMoveContainer();
            turnManager.UnitIsMoving();
            highlight.characterMoving = true;
            ResetTilesInMoveRange();
            _move.StartMovement(_path);
        }
    }

    public void TakeDamageBody(int[] damages)
    {
        foreach (var item in damages)
        {
            if (item == 0)
            {
                Debug.Log("Bullet miss");
            }
            else
            {
                var hp = _bodyHP - item;
                _bodyHP = hp > 0 ? hp : 0;
            }
        }
        if (_bodyHP <= 0)
        {
            NotSelectable();
        }
        buttonsManager.UpdateBodyHUD(_bodyHP, false);
        MakeNotAttackable();
    }

    public void TakeDamageLeftArm(int[] damages)
    {
        foreach (var item in damages)
        {
            if (item == 0)
            {
                Debug.Log("Bullet miss");
            }
            else
            {
                var hp = _leftArmHP - item;
                _leftArmHP = hp > 0 ? hp : 0;
                _leftArmAlive = _leftArmHP > 0 ? true : false;
            }
        }
        CheckArms();
        buttonsManager.UpdateLeftArmHUD(_leftArmHP, false);
        MakeNotAttackable();
    }

    public void TakeDamageRightArm(int[] damages)
    {
        foreach (var item in damages)
        {
            if (item == 0)
            {
                Debug.Log("Bullet miss");
            }
            else
            {
                var hp = _rightArmHP - item;
                _rightArmHP = hp > 0 ? hp : 0;
                _rightArmAlive = _rightArmHP > 0 ? true : false;
            }
        }
        CheckArms();
        buttonsManager.UpdateRightArmHUD(_rightArmHP, false);
        MakeNotAttackable();
    }

    public void TakeDamageLegs(int[] damages)
    {
        foreach (var item in damages)
        {
            if (item == 0)
            {
                Debug.Log("Bullet miss");
            }
            else
            {
                var hp = _legsHP - item;
                _legsHP = hp > 0 ? hp : 0;
                _canMove = _legsHP > 0 ? true : false;
            }
        }
        buttonsManager.UpdateLegsHUD(_legsHP, false);
        MakeNotAttackable();
    }

    public void TakeDamageLegs(int damage)
    {
        var hp = _legsHP - damage;
        _legsHP = hp > 0 ? hp : 0;
        _canMove = _legsHP > 0 ? true : false;
        buttonsManager.UpdateLegsHUD(_legsHP, true);
        MakeNotAttackable();
    }

    public void SelectLeftGun()
    {
        _selectedGun = leftGun;
        ResetTilesInAttackRange();

        if (CanAttack())
        {
            if (_path.Count == 0)
                PaintTilesInAttackRange(_myPositionTile, 0);
            else PaintTilesInAttackRange(_path[_path.Count - 1], 0);
            CheckEnemiesInAttackRange();
        }
    }

    public void SelectRightGun()
    {
        _selectedGun = rightGun;
        ResetTilesInAttackRange();

        if (CanAttack())
        {
            if (_path.Count == 0)
                PaintTilesInAttackRange(_myPositionTile, 0);
            else PaintTilesInAttackRange(_path[_path.Count - 1], 0);
            CheckEnemiesInAttackRange();
        }
    }

    public void ResetTilesInAttackRange()
    {
        highlight.ClearTilesInAttackRange(_tilesInAttackRange);
        _tilesInAttackRange.Clear();
        _tilesForAttackChecked.Clear();
    }

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
            if (_targetTile != null && _targetTile == newTile) return;
            else
            {
                _targetTile = newTile;
                pathCreator.Calculate(this, _targetTile, _currentSteps);
                if (pathCreator.GetDistance() <= _maxSteps)
                {
                    _path = pathCreator.GetPath();
                    if (_path.Count > 0)
                    {
                        highlight.PathPreview(_path);
                        buttonsManager.ActivateMoveButton();
                        buttonsManager.ActivateUndo();
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

    
    public Tile GetEndTile()
    {
        return _targetTile;
    }

    public Team GetUnitTeam()
    {
        return _unitTeam;
    }

    public int GetSteps()
    {
        return _currentSteps;
    }

    public int GetBodyMaxHP()
    {
        return _bodyMaxHP;
    }

    public int GetBodyHP()
    {
        return _bodyHP;
    }

    public int GetLeftArmMaxHP()
    {
        return _leftArmMaxHP;
    }

    public int GetLeftArmHP()
    {
        return _leftArmHP;
    }

    public int GetRightArmMaxHP()
    {
        return _rightArmMaxHP;
    }

    public int GetRightArmHP()
    {
        return _rightArmHP;
    }

    public int GetLegsMaxHP()
    {
        return _legsMaxHP;
    }

    public int GetLegsHP()
    {
        return _legsHP;
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
        return leftGun;
    }

    public Gun GetRightGun()
    {
        return rightGun;
    }

    #endregion

    #region Utilities
    
    public void Undo()
    {
        foreach (var item in _enemiesInRange)
        {
            turnManager.UnitCantBeAttacked(item);
        }
        ResetInRangeLists();
        GetPath();
        Debug.Log("path count: " + _path.Count);
        if (_path.Count > 0)
        {
            buttonsManager.ActivateUndo();
            PaintTilesInMoveRange(_path[_path.Count - 1], 0);
            if (CanAttack())
                PaintTilesInAttackRange(_path[_path.Count - 1], 0);
        }
        else
        {
            buttonsManager.DeactivateUndo();
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
        _canMove = _legsHP > 0 ? true : false;
        _canAttack = true;
        _selectedGun.SetGun();
        _path.Clear();
        _currentSteps = _canMove ? _maxSteps : 0;
        _enemiesInRange.Clear();
        MakeNotAttackable();
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
        _myPositionTile.MakeTileFree();
        _myPositionTile = _targetTile;
        _myPositionTile.MakeTileOccupied();
        _myPositionTile.SetUnitAbove(this);
        _targetTile = null;
        turnManager.UnitStoppedMoving();
        pathCreator.ResetPath();
        _tilesForAttackChecked.Clear();
        
        if (CanAttack())
        {
            PaintTilesInAttackRange(_myPositionTile, 0);
            CheckEnemiesInAttackRange();
        }
    }

    public void ReduceAvailableSteps(int amount)
    {
        var s = _currentSteps - amount;
        _currentSteps = s >= 0 ? s : 0;
    }

    public void IncreaseAvailableSteps(int amount)
    {
        var s = _currentSteps + amount;
        _currentSteps = s <= _maxSteps ? s : _maxSteps;
    }

    public void SelectThisUnit()
    {
        Debug.Log("unit selected");
        _selected = true;
        ResetInRangeLists();
        _path.Clear();
        highlight.PathLinesClear();
        _myPositionTile = GetTileBelow();
        _myPositionTile.unitAboveSelected = true;
        _myPositionTile.GetComponent<TileMaterialhandler>().DiseableAndEnableSelectedNode(true);
        if (CanAttack())
        {
            if (_rightArmAlive)
                _selectedGun = rightGun;
            else if (_leftArmAlive)
                _selectedGun = leftGun;
            PaintTilesInAttackRange(_myPositionTile, 0);
            CheckEnemiesInAttackRange();
        }
        if (_canMove)
        {
            _currentSteps = _maxSteps;
            AddTilesInMoveRange();
            PaintTilesInMoveRange(_myPositionTile, 0);
        }
    }

    public void DeselectThisUnit()
    {
        _selected = false;
        _myPositionTile.unitAboveSelected = false;
        _myPositionTile.EndMouseOverColor();
        foreach (var item in _enemiesInRange)
        {
            turnManager.UnitCantBeAttacked(item);
        }

        if (_canMove)
            _currentSteps = _maxSteps;
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
        _myPositionTile.unitAboveSelected = true;
        _myPositionTile.GetComponent<TileMaterialhandler>().DiseableAndEnableSelectedNode(true);
    }

    //Check if selected object is a tile.
    bool IsValidTarget(Transform target)
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return false;
        if (target != null)
        {
            if (target.gameObject.layer == LayerMask.NameToLayer("GridBlock"))
            {
                var tile = target.gameObject.GetComponent<Tile>();
                if (tile != null && tile.IsWalkable() && tile.IsFree())
                {
                    return true;
                }
                else return false;
            }
            return false;
        }
        else return false;
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
        if (!_leftArmAlive && !_rightArmAlive)
            _canAttack = false;
    }

    public bool LeftArmAlive()
    {
        return _leftArmAlive;
    }

    public bool RightArmAlive()
    {
        return _rightArmAlive;
    }

    public bool CanBeSelected()
    {
        return _canBeSelected;
    }
    public void DeactivateAttack()
    {
        ResetTilesInAttackRange();
        ResetTilesInMoveRange();
        highlight.PathLinesClear();
        _canAttack = false;
        _canMove = false;
    }

    public void DeactivateMoveButton()
    {
        buttonsManager.DeactivateMoveButton();
    }
    #endregion

    private void OnMouseOver()
    {
        ShowWorldUI();
    }

    private void OnMouseExit()
    {
        HideWorldUI();
    }

    void ShowWorldUI()
    {
        _myUI.ActivateWorldUI(GetBodyHP(), GetRightArmHP(), GetLeftArmHP(), GetLegsHP(), ThisUnitCanMove(), CanAttack());
    }

    void HideWorldUI()
    {
        _myUI.DeactivateWorldUI();
    }

    public void RotateTowardsEnemy(Vector3 pos)
    {
        _move.SetPosToRotate(pos);
        _move.StartRotation();
    }
}
