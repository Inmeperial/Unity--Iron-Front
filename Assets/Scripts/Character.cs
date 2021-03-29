using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    GridMovement _move;
    public float speed;
    public LayerMask mask;
    private bool _selected;
    Tile _myPositionTile;
    Tile _targetTile;
    MeshRenderer _render;
    private bool _moving = false;

    TurnManager _turnManager;
    // Start is called before the first frame update
    void Start()
    {
        _selected = false;
        _move = GetComponent<GridMovement>();
        _render = GetComponent<MeshRenderer>();
        _turnManager = FindObjectOfType<TurnManager>();
        _myPositionTile = GetTileBelow();
        _myPositionTile.MakeTileOccupied();
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
            _moving = true;
            _turnManager.UnitIsMoving();
            _move.StartMovement(GetTileBelow(), target, speed);
            _targetTile = target.GetComponent<Tile>();
        }
    }

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
}
