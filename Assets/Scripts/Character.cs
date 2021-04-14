using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Teams
{
    GridMovement _move;
    public int maxHp;
    private int _hp;
    public int damage;
    public int steps;
    private int _steps;
    public float speed;
    public LayerMask mask;
    public IPathCreator pathCreator;
    [SerializeField] private Team _unitTeam;
    public Character _enemy;
    private bool _selected;
    private bool _moving = false;
    public bool _canMove = true;
    public bool _attacked = false;

    private Tile _myPositionTile;
    private Tile _targetTile;
    private MeshRenderer _render;
    private List<Tile> _path = new List<Tile>();

    [SerializeField] private TurnManager _turnManager;

    [SerializeField] private TileHighlight _highlight;
    

    // Start is called before the first frame update
    void Start()
    {
        _steps = steps;
        _hp = maxHp;
        _selected = false;
        _canMove = true;
        _move = GetComponent<GridMovement>();
        _render = GetComponent<MeshRenderer>();
        _turnManager = FindObjectOfType<TurnManager>();
        _myPositionTile = GetTileBelow();
        _myPositionTile.MakeTileOccupied();
        _highlight = FindObjectOfType<TileHighlight>();
        pathCreator = GetComponent<IPathCreator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_selected && !_moving && _canMove && Input.GetMouseButtonDown(0))
        {
            GetTargetToMove();
        }
    }

    #region Actions
    //This method is called from UI Button "Move".
    public void Move()
    {
        if (_path != null && _path.Count > 0)
        {
            _moving = true;
            _turnManager.UnitIsMoving();
            _highlight.characterMoving = true;
            _move.StartMovement(_path, speed);
        }
    }

    public void Attack()
    {
        _enemy.TakeDamage(damage);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(name + " took: " + damage + " damage.");
        _hp -= damage;
        Debug.Log(name + " current hp: " + _hp);
    }
    #endregion

    #region Getters
    public void GetTargetToMove()
    {
        Transform target = MouseRay.GetTargetTransform(mask);
        
        if (IsValidTarget(target))
        {
            var newTile = target.GetComponent<Tile>();
            if (_targetTile != null && _targetTile == newTile) return;
            else
            {
                _targetTile = newTile;
                
                if (pathCreator.GetDistance() <= steps)
                {
                    pathCreator.Calculate(this, _targetTile);
                    _path = pathCreator.GetPath();
                    _highlight.PathPreview(_path);
                    ActivateMoveButton();
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
        return _steps;
    }

    public int GetHP()
    {
        return _hp;
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
        pathCreator.GetPath();
        return _path;
    }

    public Tile GetTileBelow()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, LayerMask.NameToLayer("GridBlock"));
        return hit.transform.gameObject.GetComponent<Tile>();
    }
    #endregion

    #region Utilities
    public void NewTurn()
    {
        _canMove = true;
        _path.Clear();
        _steps = steps;
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
    }

    public void DeselectThisUnit()
    {
        Material mat = new Material(_render.sharedMaterial);
        mat.color = Color.white;

        _render.sharedMaterial = mat;
        _selected = false;
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
        if (target != null)
        {
            var tile = target.gameObject.GetComponent<Tile>();
            if (tile != null && tile.isWalkable && tile.IsFree())
            {
                return true;
            }
            else return false;
        }
        else return false;
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

    void ActivateMoveButton()
    {
        _turnManager.ActivateMoveButton();
    }

    public void DeactivateMoveButton()
    {
        _turnManager.DeactivateMoveButton();
    }
}
