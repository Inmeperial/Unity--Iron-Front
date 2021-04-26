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
    [SerializeField] private float _speed;
    #endregion

    private Gun _selectedGun;

    //MOVEMENT RELATED
    public IPathCreator pathCreator;
    private GridMovement _move;
    public LayerMask block;
    private List<Tile> _tilesInMoveRange = new List<Tile>();
    private Tile _myPositionTile;
    private Tile _targetTile;
    [SerializeField] private List<Tile> _path = new List<Tile>();

    //FLAGS
    private bool _selected;
    private bool _moving = false;
    private bool _canMove = true;
    private bool _canAttack = true;
    [SerializeField] private bool _canBeAttacked = false;

    //OTHERS
    private List<Tile> _tilesInAttackRange = new List<Tile>();
    private List<Character> _enemiesInRange = new List<Character>();
    private MeshRenderer _render;

    private TurnManager _turnManager;

    private TileHighlight _highlight;

    private ButtonsUIManager _buttonsManager;

    // Start is called before the first frame update
    void Start()
    {
        _bodyHP = _bodyMaxHP;
        _leftArmHP = _leftArmMaxHP;
        _rightArmHP = _rightArmMaxHP;
        _legsHP = _legsMaxHP;
        _canMove = _legsHP > 0 ? true : false;
        _currentSteps = _canMove ? _maxSteps : 0;
        _selected = false;
        _canAttack = true;
        _move = GetComponent<GridMovement>();
        _render = GetComponent<MeshRenderer>();
        _turnManager = FindObjectOfType<TurnManager>();
        _myPositionTile = GetTileBelow();
        _myPositionTile.MakeTileOccupied();
        _myPositionTile.SetUnitAbove(this);
        _highlight = FindObjectOfType<TileHighlight>();
        _buttonsManager = FindObjectOfType<ButtonsUIManager>();
        pathCreator = GetComponent<IPathCreator>();

        _selectedGun = leftGun;


    }

    // Update is called once per frame
    void Update()
    {
        if (_selected && !_moving && _canMove && Input.GetMouseButtonDown(0))
        {
            GetTargetToMove();
        }
    }

    void PaintTilesInAttackRange(Vector3 pos, List<Tile> neighbours, int count)
    {

        var a = Physics.OverlapSphere(pos, _selectedGun.GetAttackRange() * _myPositionTile.transform.localScale.z);

        foreach (var item in a)
        {
            var t = item.GetComponent<Tile>();
            if (t)
            {
                if (!t.HasTileAbove() && t.IsWalkable())
                {
                    _tilesInAttackRange.Add(t);
                    _highlight.PaintTilesInAttackRange(t);
                }
            }
        }
        //if (count >= _selectedGun.GetAttackRange())
        //{
        //    Debug.Log("termine");
        //    return;
        //}
        //var list = CheckIfContains(neighbours);
        //foreach (var tile in list)
        //{
        //    if (!_tilesInAttackRange.Contains(tile))
        //    {
        //        if (!tile.HasTileAbove() && tile.IsWalkable())
        //        {
        //            _tilesInAttackRange.Add(tile);
        //            _highlight.PaintTilesInAttackRange(tile);
        //        }
               
        //    }
            
        //    PaintTilesInAttackRange(tile.allNeighbours, count + 1);
        //}
 
    }

    List<Tile> CheckIfContains(List<Tile> list)
    {
        var temp = new List<Tile>();
        foreach (var item in list)
        {
            if (_tilesInAttackRange.Contains(item))
                continue;
            temp.Add(item);
        }
        return temp;
    }

    public void PaintTilesInMoveRange(List<Tile> neighbours, int count)
    {
        //var a = Physics.OverlapSphere(_myPositionTile.transform.position, _currentSteps * _myPositionTile.transform.localScale.z);

        //foreach (var item in a)
        //{
        //    var t = item.GetComponent<Tile>();
        //    if (t)
        //    {
        //        if (!t.HasTileAbove() && t.IsWalkable())
        //        {
        //            _tilesInMoveRange.Add(t);
        //            _highlight.PaintTilesInMoveRange(t);
        //        }
        //    }
        //}
        if (count >= _currentSteps)
            return;

        foreach (var tile in neighbours)
        {
            if (!_tilesInMoveRange.Contains(tile))
            {
                _tilesInMoveRange.Add(tile);
                _highlight.PaintTilesInMoveRange(tile);
            }
            PaintTilesInMoveRange(tile.neighboursForMove, count + 1);
        }
    }

    public void AddTilesInMoveRange()
    {
        _highlight.AddTilesInMoveRange(_tilesInMoveRange);
    }

    #region Actions
    //This method is called from UI Button "Move".
    public void Move()
    {
        if (_moving == false && _path != null && _path.Count > 0)
        {
            _moving = true;
            _buttonsManager.DeactivateBodyPartsContainer();
            _buttonsManager.DeactivateMoveContainer();
            _turnManager.UnitIsMoving();
            _highlight.characterMoving = true;
            _highlight.ClearTilesInRange(_tilesInMoveRange);
            _move.StartMovement(_path, _speed);
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
        _buttonsManager.UpdateBodyHUD(_bodyHP, false);
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
            }
        }
        _buttonsManager.UpdateLeftArmHUD(_leftArmHP, false);
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
            }
        }
        _buttonsManager.UpdateRightArmHUD(_rightArmHP, false);
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

                if (_legsHP == 0)
                    _canMove = false;
            }
        }
        _buttonsManager.UpdateLegsHUD(_legsHP, false);
        MakeNotAttackable();
    }

    public void TakeDamageLegs(int damage)
    {
        var hp = _legsHP - damage;
        _legsHP = hp > 0 ? hp : 0;

        _buttonsManager.UpdateLegsHUD(_legsHP, true);
        MakeNotAttackable();
        if (_legsHP == 0)
            _canMove = false;
    }

    public void SelectLeftGun()
    {
        _selectedGun = leftGun;
        ResetTilesInAttackRange();
        if (_path.Count == 0)
            PaintTilesInAttackRange(_myPositionTile.transform.position, _myPositionTile.allNeighbours, 0);
        else PaintTilesInAttackRange(_path[_path.Count - 1].transform.position, _path[_path.Count - 1].allNeighbours, 0);
        CheckEnemiesInAttackRange();
    }

    public void SelectRightGun()
    {
        _selectedGun = rightGun;
        ResetTilesInAttackRange();
        if (_path.Count == 0)
            PaintTilesInAttackRange(_myPositionTile.transform.position, _myPositionTile.allNeighbours, 0);
        else PaintTilesInAttackRange(_path[_path.Count - 1].transform.position, _path[_path.Count - 1].allNeighbours, 0);
        CheckEnemiesInAttackRange();
    }

    public void ResetTilesInAttackRange()
    {
        foreach (var item in _tilesInAttackRange)
        {
            item.ResetColor();
        }
        _tilesInAttackRange.Clear();
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
                        _highlight.PathPreview(_path);
                        _buttonsManager.ActivateMoveButton();
                        _buttonsManager.ActivateUndo();
                        _highlight.ClearTilesInRange(_tilesInAttackRange);
                        _highlight.ClearTilesInRange(_tilesInMoveRange);
                        _highlight.CreatePathLines(_path);
                        ResetTilesInAttackRange();
                        _tilesInMoveRange.Clear();
                        PaintTilesInAttackRange(_path[_path.Count - 1].transform.position, _path[_path.Count - 1].allNeighbours, 0);
                        PaintTilesInMoveRange(_path[_path.Count - 1].neighboursForMove, 0);
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
    #endregion

    #region Utilities
    
    public void Undo()
    {
        foreach (var item in _enemiesInRange)
        {
            _turnManager.UnitCantBeAttacked(item);
        }
        ResetInRangeLists();
        GetPath();
        Debug.Log("path count: " + _path.Count);
        if (_path.Count > 0)
        {
            _buttonsManager.ActivateUndo();
            PaintTilesInMoveRange(_path[_path.Count - 1].neighboursForMove, 0);
        }
        else
        {
            _buttonsManager.DeactivateUndo();
            PaintTilesInMoveRange(_myPositionTile.neighboursForMove, 0);
        }

    }

    public void ResetInRangeLists()
    {
        foreach (var item in _enemiesInRange)
        {
            item.MakeNotAttackable();
        }

        foreach (var item in _tilesInMoveRange)
        {
            item.ResetColor();
        }
        _highlight.Undo();

        foreach (var item in _tilesInAttackRange)
        {
            item.ResetColor();
        }
        _tilesInMoveRange.Clear();
        _tilesInAttackRange.Clear();
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
        _highlight.characterMoving = false;
        _highlight.EndPreview();
        _moving = false;
        _myPositionTile.MakeTileFree();
        _myPositionTile = _targetTile;
        _myPositionTile.MakeTileOccupied();
        _myPositionTile.SetUnitAbove(this);
        _targetTile = null;
        _turnManager.UnitStoppedMoving();
        pathCreator.ResetPath();

        CheckEnemiesInAttackRange();
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
        Material mat = new Material(_render.sharedMaterial);
        mat.color = Color.blue;
        _render.sharedMaterial = mat;

        _selected = true;
        ResetInRangeLists();
        _path.Clear();
        _highlight.PathLinesClear();
        if (_canAttack)
        {
            PaintTilesInAttackRange(_myPositionTile.transform.position, _myPositionTile.allNeighbours, 0);
            CheckEnemiesInAttackRange();
        }
        if (_canMove)
        {
            _currentSteps = _maxSteps;
            AddTilesInMoveRange();
            PaintTilesInMoveRange(_myPositionTile.neighboursForMove, 0);
        }
    }

    public void DeselectThisUnit()
    {
        Material mat = new Material(_render.sharedMaterial);
        mat.color = Color.white;

        _render.sharedMaterial = mat;
        _selected = false;
        foreach (var item in _enemiesInRange)
        {
            _turnManager.UnitCantBeAttacked(item);
        }

        if (_canMove)
            _currentSteps = _maxSteps;
        ResetInRangeLists();
        _path.Clear();
        _highlight.PathLinesClear();
        pathCreator.ResetPath();
    }

    public void SelectedAsEnemy()
    {
        Material mat = new Material(_render.sharedMaterial);
        mat.color = Color.red;

        _render.sharedMaterial = mat;
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
                _turnManager.UnitCantBeAttacked(unit);
            }
            _enemiesInRange.Clear();
            foreach (var item in _tilesInAttackRange)
            {
                if (item.IsFree() == false)
                {
                    var unit = item.GetUnitAbove();
                    if (unit.GetUnitTeam() != _unitTeam)
                    {
                        _turnManager.UnitCanBeAttacked(unit);
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

    public void DeactivateAttack()
    {
        _canAttack = false;
        _canMove = false;
    }

    public void DeactivateMoveButton()
    {
        _buttonsManager.DeactivateMoveButton();
    }
    #endregion


}
