using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    GridMovement _move;
    public float speed;
    public LayerMask mask;

    [SerializeField] private int _moveRadius;

    private bool _selected;
    private bool _moving = false;

    public Tile _myPositionTile;
    Tile _targetTile;
    MeshRenderer _render;

    [SerializeField] private TurnManager _turnManager;
    [SerializeField] private AStarAgent _agent;
    [SerializeField] private TileHighlight highlight;

    
    // Start is called before the first frame update
    void Start()
    {
        _selected = false;
        _move = GetComponent<GridMovement>();
        _render = GetComponent<MeshRenderer>();
        _turnManager = FindObjectOfType<TurnManager>();
        _myPositionTile = GetTileBelow();
        _myPositionTile.MakeTileOccupied();
        _agent = FindObjectOfType<AStarAgent>();
        highlight = FindObjectOfType<TileHighlight>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_selected && !_moving && Input.GetMouseButtonDown(0))
        {
            GetTargetToMove();
        }
    }

    public void GetTargetToMove()
    {
        Transform target = MouseRay.GetTargetTransform(mask);
        if (IsValidTarget(target))
        {
            _targetTile = target.GetComponent<Tile>();
            _agent.init = GetTileBelow();
            _agent.finit = _targetTile;
            var tilesList = _agent.PathFindingAstar();
            Debug.Log(tilesList.Count);
            if (tilesList.Count > 0 && tilesList.Count <= _moveRadius+1)
            {
                _moving = true;
                _turnManager.UnitIsMoving();
                highlight.characterMoving = true;
                _move.StartMovement(tilesList, speed);
            }
            else Debug.Log("Can't reach tile");
        }
    }

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

    public bool IsSelected()
    {
        return _selected;
    }

    public void ReachedEnd()
    {
        highlight.characterMoving = false;
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
}
