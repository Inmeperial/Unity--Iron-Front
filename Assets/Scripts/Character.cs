using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : Teams
{
    //STATS
    [SerializeField] private Team _unitTeam;
    public int bodyMaxHP;
    [SerializeField] private int _bodyHP;
    public int leftArmMaxHP;
    [SerializeField] private int _leftArmHP;
    public int rightArmMaxHP;
    [SerializeField] private int _rightArmHP;
    public int legsMaxHP;
    [SerializeField] private int _legsHP;
    public int maxBullets;
    [SerializeField] private int _availableBullets;
    public int damage;
    public int steps;
    [SerializeField] private int _steps;
    public float speed;
    public int attackRange;

    //MOVEMENT RELATED
    public IPathCreator pathCreator;
    private GridMovement _move;
    public LayerMask block;
    private bool _canBeAttacked = false;
    private List<Tile> _tilesInMoveRange = new List<Tile>();
    private Tile _myPositionTile;
    private Tile _targetTile;
    [SerializeField] private List<Tile> _path = new List<Tile>();

    //FLAGS
    private bool _selected;
    private bool _moving = false;
    private bool _canMove = true;
    private bool _canAttack = true;

    //OTHERS
    private List<Tile> _tilesInAttackRange = new List<Tile>();
    private List<Character> _enemyTargets = new List<Character>();
    private Character _enemy;
    private MeshRenderer _render;


    [SerializeField] private TurnManager _turnManager;

    [SerializeField] private TileHighlight _highlight;

    [SerializeField] private AStarAgent _agent;


    // Start is called before the first frame update
    void Start()
    {
        _steps = steps;
        _bodyHP = bodyMaxHP;
        _leftArmHP = leftArmMaxHP;
        _rightArmHP = rightArmMaxHP;
        _legsHP = legsMaxHP;
        _availableBullets = maxBullets;
        _selected = false;
        _canMove = true;
        _canAttack = true;
        _move = GetComponent<GridMovement>();
        _render = GetComponent<MeshRenderer>();
        _turnManager = FindObjectOfType<TurnManager>();
        _myPositionTile = GetTileBelow();
        _myPositionTile.MakeTileOccupied();
        _highlight = FindObjectOfType<TileHighlight>();
        _agent = FindObjectOfType<AStarAgent>();
        pathCreator = GetComponent<IPathCreator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_selected && !_moving && _canMove && Input.GetMouseButtonDown(0))
        {
            GetTargetToMove();
        }

        if (_selected && Input.GetKeyDown(KeyCode.Space))
        {
            if (_path.Count > 0)
            {
                PaintTilesInAttackRange(_path[_path.Count-1].allNeighbours, 0);
            }
            else PaintTilesInAttackRange(_myPositionTile.allNeighbours, 0);
        }
    }

    void PaintTilesInAttackRange(List<Tile> neighbours, int count)
    {
        if (count >= attackRange)
            return;

        foreach (var item in neighbours)
        {
            if (!_tilesInAttackRange.Contains(item))
            {
                if (!item.HasTileAbove() && item.IsWalkable())
                {
                    _tilesInAttackRange.Add(item);
                    _highlight.PaintTilesInAttackRange(item);
                }
               
            }
            
            PaintTilesInAttackRange(item.allNeighbours, count + 1);
        }
 
    }

    public void PaintTilesInMoveRange(List<Tile> neighbours, int count)
    {
        if (count >= _steps)
            return;

        foreach (var item in neighbours)
        {
            if (!_tilesInMoveRange.Contains(item))
            {
                _tilesInMoveRange.Add(item);
                _highlight.PaintTilesInMoveRange(item);
            }
            //GetTilesInAttackRange(item.neighbours, count + 1);
            PaintTilesInMoveRange(item.neighboursForMove, count + 1);
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
            _turnManager.UnitIsMoving();
            _highlight.characterMoving = true;
            _highlight.ClearTilesInRange(_tilesInMoveRange);
            _move.StartMovement(_path, speed);
        }
    }

    //public virtual void Attack()
    //{
    //    _canMove = false;
    //    _canAttack = false;
    //    _turnManager.DeactivateMoveButton();
    //    _turnManager.DeactivateAttackButton();
    //    _turnManager.DamageEnemy(_enemy, damage);
    //    _highlight.ClearTilesInRange(_tilesInMoveRange);
    //}

    //public void TakeDamage(int damage)
    //{
    //    _hp -= damage;
    //    _turnManager.UpdateHP(_hp, maxHp);
    //    DeselectThisUnit();
    //}
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
                pathCreator.Calculate(this, _targetTile, _steps);
                if (pathCreator.GetDistance() <= steps)
                {
                    _path = pathCreator.GetPath();
                    if (_path.Count > 0)
                    {
                        _highlight.PathPreview(_path);
                        ActivateMoveButton();
                        _highlight.ClearTilesInRange(_tilesInAttackRange);
                        _highlight.ClearTilesInRange(_tilesInMoveRange);
                        _highlight.CreatePathLines(_path);
                        _tilesInAttackRange.Clear();
                        _tilesInMoveRange.Clear();
                        PaintTilesInMoveRange(_path[_path.Count - 1].neighboursForMove, 0);
                        PaintTilesInAttackRange(_path[_path.Count - 1].allNeighbours, 0);
                        AddTilesInMoveRange();
                        CheckCloseEnemies();
                    }
                }
            }
        }
    }

    public void ActivateMoveButton()
    {
        _turnManager.ActivateMoveButton();
    }

    public void DeactivateMoveButton()
    {
        _turnManager.DeactivateMoveButton();
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
        return _steps;
    }

    //public int GetHP()
    //{
    //    return _hp;
    //}

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
        pathCreator.GetPath();
        return _path;
    }

    public Tile GetTileBelow()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, LayerMask.NameToLayer("GridBlock"));
        return hit.transform.gameObject.GetComponent<Tile>();
    }

    public int GetAvailableBullets()
    {
        return _availableBullets;
    }
    #endregion

    #region Utilities

    public void ReduceAvailableBullets(int quantity)
    {
        Debug.Log("reduzco balas");
        _availableBullets -= quantity;
    }

    public void IncreaseAvailableBullets(int quantity)
    {
        _availableBullets += quantity;
    }
    public void Undo()
    {
        //_highlight.ClearTilesInRange(_tilesInAttackRange);
        //_tilesInAttackRange.Clear();
        foreach (var item in _enemyTargets)
        {
            item.MakeNotAttackable();
        }
        _enemyTargets.Clear();
        GetPath();
        ResetInRangeLists();
        if (_path.Count > 0)
        {
            PaintTilesInMoveRange(_path[_path.Count - 1].neighboursForMove, 0);
        }
        else PaintTilesInMoveRange(_myPositionTile.neighboursForMove, 0);

    }

    void ResetInRangeLists()
    {
        foreach (var item in _tilesInMoveRange)
        {
            item.ResetColor();
        }
        _highlight.Undo();
        _tilesInMoveRange.Clear();

        foreach (var item in _tilesInAttackRange)
        {
            item.ResetColor();
        }
        _tilesInAttackRange.Clear();
    }
    public void NewTurn()
    {
        _canMove = true;
        _canAttack = true;
        _availableBullets = maxBullets;
        _path.Clear();
        _steps = steps;
        _enemyTargets.Clear();
        MakeNotAttackable();
        pathCreator.Reset();
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
        _targetTile = null;
        _turnManager.UnitStoppedMoving();
        pathCreator.Reset();
    }

    public void ReduceAvailableSteps(int amount)
    {
        _steps -= amount;
    }

    public void IncreaseAvailableSteps(int amount)
    {
        _steps += amount;
    }

    public void SelectThisUnit()
    {
        Material mat = new Material(_render.sharedMaterial);
        mat.color = Color.blue;

        _render.sharedMaterial = mat;
        _selected = true;
        PaintTilesInMoveRange(_myPositionTile.neighboursForMove, 0);
    }

    public void DeselectThisUnit()
    {
        Material mat = new Material(_render.sharedMaterial);
        mat.color = Color.white;

        _render.sharedMaterial = mat;
        _selected = false;

        ResetInRangeLists();
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

    void CheckCloseEnemies()
    {
        var enemies = _turnManager.GetEnemies(_unitTeam);

        _enemyTargets.Clear();
        if (_path.Count == 0)
        {
            
            foreach (var unit in enemies)
            {
                var dist = CalculateDistanceToEnemie(unit, _myPositionTile);

                if (dist <= attackRange)
                {
                    _enemyTargets.Add(unit);
                    _turnManager.UnitCanBeAttacked(unit);
                    var tile = _turnManager.GetUnitTile(unit);
                    _highlight.PaintTilesInAttackRange(tile);
                }
            }
        }
        else
        {
            foreach (var unit in enemies)
            {
                var dist = CalculateDistanceToEnemie(unit, _path[_path.Count-1]);

                if (dist <= attackRange)
                {
                    _enemyTargets.Add(unit);
                    _turnManager.UnitCanBeAttacked(unit);
                    var tile = _turnManager.GetUnitTile(unit);
                    _highlight.PaintTilesInAttackRange(tile);
                }
            }
        }
    }

    int CalculateDistanceToEnemie(Character enemy, Tile from)
    {
        _agent.init = from;
        _agent.finit = enemy.GetTileBelow();

        var path = _agent.PathFindingAstar();

        return path.Count-1;
    }
    #endregion

    public bool ThisUnitCanMove()
    {
        return _canMove;
    }

    public bool IsMoving()
    {
        return _moving;
    }

    public void SetEnemy(Character enemy)
    {
        _enemy = enemy;
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

    public void AttackBody(int bullets, int bulletDamage)
    {
        Debug.Log("Body attacked -- Bullets: " + bullets + " -- Damage: " + bulletDamage * bullets);
        _bodyHP -= bullets * bulletDamage;
        _canAttack = false;
    }

    public void AttackLeftArm(int bullets, int bulletDamage)
    {
        Debug.Log("larm attacked -- Bullets: " + bullets + " -- Damage: " + bulletDamage * bullets);
        _leftArmHP -= bullets * bulletDamage;
        _canAttack = false;
    }

    public void AttackRightArm(int bullets, int bulletDamage)
    {
        Debug.Log("rarm attacked -- Bullets: " + bullets + " -- Damage: " + bulletDamage * bullets);
        _rightArmHP -= bullets * bulletDamage;
        _canAttack = false;
    }

    public void AttackLegs(int bullets, int bulletDamage)
    {
        Debug.Log("legs attacked -- Bullets: " + bullets + " -- Damage: " + bulletDamage * bullets);
        _legsHP -= bullets * bulletDamage;
        _canAttack = false;
    }
}
