using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mortar : MonoBehaviour, IObserver
{
    public float activationTilesDetectionRange;
    [SerializeField] private LayerMask _mortarMask;
    [SerializeField] private LayerMask _gridBlock;
    [SerializeField] private KeyCode _deselectKey;
    [SerializeField] private int _shootRange;
    [SerializeField] private int _aoe;
    [SerializeField] private int _damage;

    private Dictionary<Tile, int> _tilesForAttackChecked = new Dictionary<Tile, int>();
    private Dictionary<Tile, int> _tilesForPreviewChecked = new Dictionary<Tile, int>();

    private HashSet<Tile> _tilesInAttackRange = new HashSet<Tile>();
    private HashSet<Tile> _tilesInPreviewRange = new HashSet<Tile>();

    private HashSet<Tile> _tilesToAttack = new HashSet<Tile>();
    private TileHighlight _highlight;

    private bool _selected = false;

    private Tile _myPositionTile;

    private Tile _last;

    private HashSet<Tile> _tilesInActivationRange = new HashSet<Tile>();
    private Character _activationCharacter;

    private delegate void Execute();
    Dictionary<string, Execute> _actionsDic = new Dictionary<string, Execute>();
    private bool _attackPending = false;
    private int _turnCount = 0;
    public bool drawGizmo;
    // Start is called before the first frame update
    private void Start()
    {
        _highlight = FindObjectOfType<TileHighlight>();
        _myPositionTile = GetTileBelow();
        _selected = false;
        _actionsDic.Add("EndTurn", ExecuteAttack);
        _actionsDic.Add("Deselect", Deselect);
        GetTilesInActivationRange();
        GetTilesInAttackRange(_myPositionTile, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(_deselectKey))
        {
            Deselect();
        }

        if (!_attackPending && Input.GetMouseButtonDown(0))
        {
            Selection();
        }
    }

    private void Selection()
    {
        if (MouseRay.GetTargetGameObject(_mortarMask))
        {
            _selected = true;
            if (SelectedPlayerAbove())
            {
                Debug.Log("hago la seleccion con mecha");
                Debug.Log("character list reset");
                _activationCharacter.ResetInRangeLists();
                foreach (var tiles in _tilesInAttackRange)
                {
                    tiles.inAttackRange = true;
                    //shader de aoe mortero
                    _highlight.PaintTilesInAttackRange(tiles);
                    //_highlight.PaintTilesInPreviewRange(tiles);
                }
            }
            else
            {
                foreach (var tiles in _tilesInAttackRange)
                {
                    tiles.inAttackRange = true;
                    //shader de aoe mortero
                    _highlight.PaintTilesInAttackRange(tiles);
                    //_highlight.PaintTilesInPreviewRange(tiles);
                }
                _highlight.PaintTilesInActivationRange(_tilesInActivationRange);                    
            }
            
        }
        else if (_selected)
        {
            var target = MouseRay.GetTargetTransform(_gridBlock);
            if (!IsValidTarget(target)) return;
            
            Debug.Log("selecciono tile");
            
            if (_activationCharacter)
            {
                Debug.Log("character list reset");
                _activationCharacter.ResetInRangeLists();
            }

            //_highlight.PaintTilesInActivationRange(_tilesInActivationRange);

            foreach (var tiles in _tilesInAttackRange)
            {
                Debug.Log("pinto tiles en rango de ataque");
                tiles.inAttackRange = true;
                // shader aoe bala mortero
                _highlight.PaintTilesInAttackRange(tiles);
            }

            var tile = target.GetComponent<Tile>();

            if (tile == _myPositionTile)
            {
                Debug.Log("es mi tile");
                return;
            }

            if (_last == null || tile != _last)
            {
                Debug.Log("pinto tiles en preview de ataque");
                _last = tile;
                _highlight.ClearTilesInPreview(_tilesInPreviewRange);
                _highlight.ClearTilesInPreview(_tilesToAttack);
                _tilesInPreviewRange.Clear();
                _tilesForPreviewChecked.Clear();
                _tilesToAttack.Clear();
                PaintTilesInPreviewRange(_last, 0);
            }
            else if (tile == _last && SelectedPlayerAbove())
            {
                Debug.Log("preparo el ataque");
                PrepareAttack();
            }
        }
    }

    //Determina si hay alguna unidad sobre las tiles de activacion
    private bool SelectedPlayerAbove()
    {
        var selector = FindObjectOfType<CharacterSelection>();
        foreach (var tile in _tilesInActivationRange)
        {
            var character = tile.GetUnitAbove();
            if (PlayerIsSelected(character, selector))
            {
                _activationCharacter = character;
                return true;
            }
        }
        return false;
    }

    //Obtiene los tiles dentro del rango de ataque (se usa solo en el start porque son siempre las mismas)
    private void GetTilesInAttackRange(Tile currentTile, int count)
    {
        if (count >= _shootRange || (_tilesForAttackChecked.ContainsKey(currentTile) &&
                                     _tilesForAttackChecked[currentTile] <= count))
            return;

        _tilesForAttackChecked[currentTile] = count;

        foreach (var tile in currentTile.allNeighbours)
        {
            if (!_tilesInAttackRange.Contains(tile))
            {
                if (!tile.HasTileAbove() && tile.IsWalkable())
                {
                    _tilesInAttackRange.Add(tile);
                }
            }
            GetTilesInAttackRange(tile, count + 1);
        }
    }

    //Pinta los tiles del preview del ataque
    private void PaintTilesInPreviewRange(Tile currentTile, int count)
    {
        if (count >= _aoe || (_tilesForPreviewChecked.ContainsKey(currentTile) &&
                              _tilesForPreviewChecked[currentTile] <= count))
            return;

        _tilesForPreviewChecked[currentTile] = count;

        foreach (var tile in currentTile.allNeighbours)
        {
            if (!_tilesInPreviewRange.Contains(tile))
            {
                if (!tile.HasTileAbove() && tile.IsWalkable())
                {
                    _tilesInPreviewRange.Add(tile);
                    _tilesToAttack.Add(tile);
                    tile.inPreviewRange = true;
                    _highlight.PaintTilesInPreviewRange(tile);
                }
            }

            PaintTilesInPreviewRange(tile, count + 1);
        }
    }

    private Tile GetTileBelow()
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

    private bool PlayerIsSelected(Character character, CharacterSelection selector)
    {
        return selector.PlayerIsSelected(character);
    }

    private void PrepareAttack()
    {
        //_tilesToAttack = _tilesInPreviewRange;
        _attackPending = true;
        _turnCount = 0;
        _activationCharacter.DeactivateAttack();
        _activationCharacter = null;
        Deselect();
    }

    private void GetTilesInActivationRange()
    {
        var tiles = Physics.OverlapSphere(transform.position, activationTilesDetectionRange, _gridBlock);

        foreach (var tile in tiles)
        {
            _tilesInActivationRange.Add(tile.transform.GetComponent<Tile>());
        }
    }
    
    
    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, activationTilesDetectionRange);

    }

    public void Notify(string action)
    {
        _actionsDic[action]();
    }

    private void ExecuteAttack()
    {
        if (!_attackPending) return;

        _turnCount++;
        if (_turnCount < 2) return;
        
        foreach (var tile in _tilesToAttack)
        {
            var unit = tile.GetCharacterAbove();
            if (unit)
            {
                unit.body.TakeDamageBody(_damage);
                unit.leftArm.TakeDamageArm(_damage);
                unit.rightArm.TakeDamageArm(_damage);
                unit.legs.TakeDamageLegs(_damage);
            }
        }
        _tilesToAttack.Clear();
        _attackPending = false;
        _turnCount = 0;
        Deselect();
    }

    private void Deselect()
    {
        _selected = false;
        _highlight.ClearTilesInActivationRange(_tilesInActivationRange);
        _highlight.ClearTilesInAttackRange(_tilesInAttackRange);
        _highlight.ClearTilesInPreview(_tilesInPreviewRange);
        _highlight.ClearTilesInPreview(_tilesToAttack);
        _tilesInPreviewRange.Clear();
        _tilesForPreviewChecked.Clear();
        _tilesToAttack.Clear();
        _last = null;
    }
}
