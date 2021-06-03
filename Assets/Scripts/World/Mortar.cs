﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mortar : MonoBehaviour
{
    [SerializeField] private LayerMask _mortarMask;
    [SerializeField] private LayerMask _gridBlock;
    [SerializeField] private KeyCode _deselectKey;
    [SerializeField] private int _shootRange;
    [SerializeField] private int _aoe;
    [SerializeField] private int _damage;

    [SerializeField] private Dictionary<Tile, int> _tilesForAttackChecked = new Dictionary<Tile, int>();
    [SerializeField] private Dictionary<Tile, int> _tilesForPreviewChecked = new Dictionary<Tile, int>();

    private HashSet<Tile> _tilesInAttackRange = new HashSet<Tile>();
    private HashSet<Tile> _tilesInPreviewRange = new HashSet<Tile>();

    private TileHighlight _highlight;

    [SerializeField] private bool _selected;

    [SerializeField] private bool _selectingCharacter;

    private Tile _myPositionTile;

    private Tile _last;
    // Start is called before the first frame update
    private void Start()
    {
        _highlight = FindObjectOfType<TileHighlight>();
        _myPositionTile = GetTileBelow();
        _selected = false;
        _selectingCharacter = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_selected && Input.GetMouseButtonDown(0))
        {
            
            if (MouseRay.GetTargetGameObject(_mortarMask))
            {
                _selected = true;
                PaintTilesInAttackRange(_myPositionTile, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) _selectingCharacter = !_selectingCharacter;

        if (Input.GetKeyDown(_deselectKey))
        {
            _selected = false;
            _highlight.ClearTilesInAttackRange(_tilesInAttackRange);
            _highlight.ClearTilesInPreview(_tilesInPreviewRange);
            _tilesInAttackRange.Clear();
            _tilesForAttackChecked.Clear();
            _tilesInPreviewRange.Clear();
            _tilesForPreviewChecked.Clear();
            _last = null;
        }

        if (_selected && _selectingCharacter && Input.GetMouseButtonDown(0))
        {
            var target = MouseRay.GetTargetTransform(_gridBlock);
            if (IsValidTarget(target))
            {
                PaintTilesInAttackRange(_myPositionTile, 0);
                var tile = target.GetComponent<Tile>();
                if (_last == null || tile != _last)
                {
                    _last = tile;
                    _highlight.ClearTilesInPreview(_tilesInPreviewRange);
                    _tilesInPreviewRange.Clear();
                    _tilesForPreviewChecked.Clear();
                    PaintTilesInPreviewRange(_last, 0);
                }
                else if (tile == _last)
                {
                    Attack();
                }
            }
        }
    }

    private void PaintTilesInAttackRange(Tile currentTile, int count)
    {
        if (count >= _shootRange || (_tilesForAttackChecked.ContainsKey(currentTile) && _tilesForAttackChecked[currentTile] <= count))
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
                        _highlight.PaintTilesInMoveAndAttackRange(tile);
                    }
                    else _highlight.PaintTilesInAttackRange(tile);
                }
            }
            PaintTilesInAttackRange(tile, count + 1);
        }
    }
    
    private void PaintTilesInPreviewRange(Tile currentTile, int count)
    {
        if (count >= _aoe || (_tilesForPreviewChecked.ContainsKey(currentTile) && _tilesForPreviewChecked[currentTile] <= count))
            return;
        
        _tilesForPreviewChecked[currentTile] = count;

        foreach (var tile in currentTile.allNeighbours)
        {
            if (!_tilesInPreviewRange.Contains(tile))
            {
                if (!tile.HasTileAbove() && tile.IsWalkable())
                {
                    _tilesInPreviewRange.Add(tile);
                    tile.inPreviewRange = true;
                    _highlight.PaintTilesInPreviewRange(tile);
                }
            }
            PaintTilesInPreviewRange(tile, count + 1);
        }
    }
    
    public Tile GetTileBelow()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, LayerMask.NameToLayer("GridBlock"));
        return hit.transform.gameObject.GetComponent<Tile>();
    }

    private bool IsValidTarget(Transform target)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return false;
        
        if (!target) return false;
        
        if (target.gameObject.layer != LayerMask.NameToLayer("GridBlock")) return false;
        
        var tile = target.gameObject.GetComponent<Tile>();
        return tile && tile.inAttackRange;
    }

    private void Attack()
    {
        Debug.Log("PUM PUM PUM");
    }
}
