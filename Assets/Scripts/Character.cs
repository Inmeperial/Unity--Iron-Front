using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Teams
{
    GridMovement _move;
    public float speed;
    public LayerMask mask;
    IPathCreator _pathCreator;
    [SerializeField] private Team _unitTeam;
    [SerializeField] private int _moveRadius;

    private bool _selected;
    private bool _moving = false;
    public bool _canMove = true;
    public bool _attacked = false;

    public Tile _myPositionTile;
    Tile _targetTile;
    MeshRenderer _render;
    public List<Tile> _path = new List<Tile>();

    [SerializeField] private TurnManager _turnManager;

    [SerializeField] private TileHighlight _highlight;
    
    // Start is called before the first frame update
    void Start()
    {
        _selected = false;
        _canMove = true;
        _move = GetComponent<GridMovement>();
        _render = GetComponent<MeshRenderer>();
        _turnManager = FindObjectOfType<TurnManager>();
        _myPositionTile = GetTileBelow();
        _myPositionTile.MakeTileOccupied();
        _highlight = FindObjectOfType<TileHighlight>();
        _pathCreator = GetComponent<IPathCreator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_selected && !_moving && _canMove && Input.GetMouseButtonDown(0))
        {
            GetTargetToMove();
        }
    }

    //This method is called from UI Button "Move".
    public void Move()
    {
        _moving = true;
        _turnManager.UnitIsMoving();
        _highlight.characterMoving = true;
        _move.StartMovement(_path, speed);
    }
    public void GetTargetToMove()
    {
        Transform target = MouseRay.GetTargetTransform(mask);
        
        if (IsValidTarget(target))
        {
            _targetTile = target.GetComponent<Tile>();
            _pathCreator.Calculate(this, _targetTile);
            _path = _pathCreator.GetPath();
        }
    }
    //Check if selected object is a tile.
    bool IsValidTarget(Transform target)
    {
        if (target != null)
        {
            var tile = target.gameObject.GetComponent<Tile>();
            if (tile != null && tile.isWalkable && tile.IsFree())
            {
                if ((target.position - transform.position).magnitude <= _moveRadius+1)
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }
        else return false;
    }

    public Tile GetTileBelow()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, LayerMask.NameToLayer("GridBlock"));
        return hit.transform.gameObject.GetComponent<Tile>();
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

    public void ReachedEnd()
    {
        _canMove = false;
        _highlight.characterMoving = false;
        _moving = false;
        _myPositionTile.MakeTileFree();
        _myPositionTile = _targetTile;
        _myPositionTile.MakeTileOccupied();
        _targetTile = null;
        _turnManager.UnitStoppedMoving();
    }

    public Tile ActualPosition()
    {
        return _myPositionTile;
    }

    public int GetMoveRadius()
    {
        return _moveRadius;
    }

    public List<Tile> GetPath()
    {
        return _path;
    }
    public void NewTurn()
    {
        _canMove = true;
        _path.Clear();
        _pathCreator.Reset();
    }

    public bool ThisUnitCanMove()
    {
        return _canMove;
    }
}
