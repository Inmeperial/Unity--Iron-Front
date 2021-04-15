﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : Teams
{
    GridMovement _move;
    public int maxHp;
    private int _hp;
    public int damage;
    public int steps;
    private int _steps;
    public float speed;
    public LayerMask block;
    public int attackRange;

    public List<Tile> _tilesInAttackRange = new List<Tile>();
    public List<Tile> _tilesInMoveRange = new List<Tile>();

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
    [SerializeField] private List<Tile> _path = new List<Tile>();

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

        if (_selected && Input.GetKeyDown(KeyCode.Space))
        {
            if (_path.Count > 0)
            {
                PaintTilesInAttackRange(_path[_path.Count-1].neighbours, 0);
            }
            else PaintTilesInAttackRange(_myPositionTile.neighbours, 0);
        }
    }

    void PaintTilesInAttackRange(List<Tile> neighbours, int count)
    {
        if (count >= attackRange)
            return;

        foreach (var item in neighbours)
        {
            _tilesInAttackRange.Add(item);
            _highlight.PaintTilesInAttackRange(item);
            PaintTilesInAttackRange(item.neighbours, count + 1);
        }
    }

    public void PaintTilesInMoveRange(List<Tile> neighbours, int count)
    {
        //if (count >= steps)
        //    return;

        //foreach (var item in neighbours)
        //{
        //    _tilesInMoveRange.Add(item);
        //    item.InRangeColor();
        //    PaintTilesInAttackRange(item.neighbours, count + 1);
        //}
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
            _move.StartMovement(_path, speed);
        }
    }

    public void Attack()
    {
        _canMove = false;
        _turnManager.DeactivateMoveButton();
        _turnManager.DeactivateAttackButton();
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
        Transform target = MouseRay.GetTargetTransform(block);
        
        if (IsValidTarget(target))
        {
            var newTile = target.GetComponent<Tile>();
            if (_targetTile != null && _targetTile == newTile) return;
            else
            {
                _targetTile = newTile;
                pathCreator.Calculate(this, _targetTile, steps);
                var dist = pathCreator.GetDistance();
                if (dist != 0 && dist <= steps)
                {
                    _path = pathCreator.GetPath();
                    _highlight.PathPreview(_path);
                    ActivateMoveButton();
                    _highlight.ClearTilesInAttackRange(_tilesInAttackRange);
                    _tilesInAttackRange.Clear();
                    PaintTilesInAttackRange(_path[_path.Count - 1].neighbours, 0);
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

    public void Undo()
    {
        _highlight.ClearTilesInAttackRange(_tilesInAttackRange);
        _tilesInAttackRange.Clear();
    }
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
        PaintTilesInMoveRange(_myPositionTile.neighbours, 0);
    }

    public void DeselectThisUnit()
    {
        Material mat = new Material(_render.sharedMaterial);
        mat.color = Color.white;

        _render.sharedMaterial = mat;
        _selected = false;

        foreach (var item in _tilesInMoveRange)
        {
            item.EndPathfindingPreviewColor();
        }
        _tilesInMoveRange.Clear();

        foreach (var item in _tilesInAttackRange)
        {
            item.EndPathfindingPreviewColor();
        }
        _tilesInAttackRange.Clear();
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
                if (tile != null && tile.isWalkable && tile.IsFree())
                {
                    return true;
                }
                else return false;
            }
            return false;
        }
        else return false;
    }

    //void CheckCloseEnemies()
    //{
    //    if (_path.Count == 0)
    //    {
    //        var enemies = _turnManager.GetEnemies(_unitTeam);

    //        foreach (var unit in enemies)
    //        {
    //            var distance = pathCreator.Calculate()
    //        }
    //    }
    //}
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

    void ActivateMoveButton()
    {
        _turnManager.ActivateMoveButton();
    }

    public void DeactivateMoveButton()
    {
        _turnManager.DeactivateMoveButton();
    }
}
