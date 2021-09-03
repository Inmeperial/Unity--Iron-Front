using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class Mortar : MonoBehaviour, IObserver, IInteractable
{
    public float activationTilesDetectionRange;
    [SerializeField] private LayerMask _mortarMask;
    [SerializeField] private LayerMask _gridBlock;
    [SerializeField] private KeyCode _deselectKey;
    [SerializeField] private int _shootRange;
    [SerializeField] private int _aoe;
    [SerializeField] private int _damage;
    [SerializeField] private int _turnsToAttack;
    [SerializeField] private Transform _textSpawnPos;
    [SerializeField] private GameObject _turnsCounterTextPrefab;
    private List<TextMeshProUGUI> _textsList = new List<TextMeshProUGUI>();
    private Dictionary<Tile, int> _tilesForAttackChecked = new Dictionary<Tile, int>();
    private Dictionary<Tile, int> _tilesForPreviewChecked = new Dictionary<Tile, int>();

    private HashSet<Tile> _tilesInAttackRange = new HashSet<Tile>();
    private HashSet<Tile> _tilesInPreviewRange = new HashSet<Tile>();

    private HashSet<Tile> _tilesToAttack = new HashSet<Tile>();
    private Tile _centerToAttack;
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
        _actionsDic.Add("EndTurn", CheckAttack);
        _actionsDic.Add("Deselect", Deselect);
        GetTilesInActivationRange();

        foreach (Tile t in _tilesInActivationRange)
        {
            _highlight.MortarPaintTilesInActivationRange(t);
        }
        
        GetTilesInAttackRange(_myPositionTile, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_selected && Input.GetKeyDown(_deselectKey))
        {
            Deselect();
        }

        if (!_attackPending && Input.GetMouseButtonDown(0))
        {
            Interact();
        }
    }

    public void Interact()
    {
        //Chequeo si hago click en el mortero o no
        if (MouseRay.GetTargetGameObject(_mortarMask) == gameObject)
        {
            Mortar[] mortars = FindObjectsOfType<Mortar>();

            foreach (Mortar m in mortars)
            {
                if (m != this)
                    m.Deselect();
            }
            
            Character s = CharacterSelection.Instance.GetSelectedCharacter();
            s.SetSelection(false);
            s.ResetRotationAndRays();
            _selected = true;
            if (SelectedPlayerAbove())
            {
                _activationCharacter.ResetInRangeLists();
            }
            foreach (Tile tiles in _tilesInAttackRange)
            {
                tiles.inAttackRange = true;
                _highlight.MortarPaintTilesInAttackRange(tiles);
            }
        }
        //Si no hago click en el mortero (hago click en un tile), pero ya estaba seleccionado
        else if (_selected)
        {
            Transform target = MouseRay.GetTargetTransform(_gridBlock);
            
            if (!IsValidTarget(target)) return;
            
            if (!SelectedPlayerAbove()) return;
            
            _activationCharacter.ResetInRangeLists();

            foreach (Tile tiles in _tilesInAttackRange)
            {
                tiles.inAttackRange = true;
                _highlight.MortarPaintTilesInAttackRange(tiles);
            }

            Tile tile = target.GetComponent<Tile>();

            if (tile == _myPositionTile)
            {
                return;
            }

            if (_last == null || tile != _last)
            {
                _last = tile;
                _highlight.ClearTilesInPreview(_tilesInPreviewRange);
                _tilesInPreviewRange.Clear();
                _tilesForPreviewChecked.Clear();
                
                foreach (Tile tiles in _tilesInAttackRange)
                {
                    tiles.inAttackRange = true;
                    _highlight.MortarPaintTilesInAttackRange(tiles);
                }
                
                _tilesToAttack.Clear();
                PaintTilesInPreviewRange(_last, 0);
            }
            else if (tile == _last && SelectedPlayerAbove())
            {
                _centerToAttack = _last;
                PrepareAttack();
            }
        }
    }

    //Determina si hay alguna unidad sobre las tiles de activacion
    private bool SelectedPlayerAbove()
    {
        foreach (Tile tile in _tilesInActivationRange)
        {
            Character character = tile.GetUnitAbove();
            
            if (!CharacterSelection.Instance.PlayerIsSelected(character)) continue;
            
            _activationCharacter = character;
            return true;
        }

        _activationCharacter = null;
        return false;
    }

    //Obtiene los tiles dentro del rango de ataque (se usa solo en el start porque son siempre las mismas)
    private void GetTilesInAttackRange(Tile currentTile, int count)
    {
        if (count >= _shootRange || (_tilesForAttackChecked.ContainsKey(currentTile) &&
                                     _tilesForAttackChecked[currentTile] <= count))
            return;

        _tilesForAttackChecked[currentTile] = count;

        foreach (Tile tile in currentTile.allNeighbours)
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

        foreach (Tile tile in currentTile.allNeighbours)
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
        Physics.Raycast(transform.position, Vector3.down, out var hit, LayerMask.NameToLayer("GridBlock"));
        return hit.transform.gameObject.GetComponent<Tile>();
    }

    private bool IsValidTarget(Transform target)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return false;

        if (!target) return false;

        if (target.gameObject.layer != LayerMask.NameToLayer("GridBlock")) return false;

        Tile tile = target.gameObject.GetComponent<Tile>();
        return tile && tile.inAttackRange;
    }

    private void PrepareAttack()
    {
        _textsList.Clear();
        
        _attackPending = true;
        _turnCount = _turnsToAttack;
        
        TextMeshProUGUI mortarText = Instantiate(_turnsCounterTextPrefab, _textSpawnPos.position, Quaternion.identity).GetComponentInChildren<TextMeshProUGUI>();
        _textsList.Add(mortarText);
        
        Vector3 pos = _centerToAttack.transform.position;
        pos.y = _textSpawnPos.position.y;
        TextMeshProUGUI missileText = Instantiate(_turnsCounterTextPrefab, pos, Quaternion.identity).GetComponentInChildren<TextMeshProUGUI>();
        _textsList.Add(missileText);
        
        mortarText.text = _turnCount.ToString();
        missileText.text = _turnCount.ToString();
        
        _activationCharacter.DeactivateAttack();
        _activationCharacter = null;
        Deselect();
    }

    private void GetTilesInActivationRange()
    {
        Collider[] tiles = Physics.OverlapSphere(transform.position, activationTilesDetectionRange, _gridBlock);

        foreach (Collider tile in tiles)
        {
            Tile t = tile.transform.GetComponent<Tile>();
            
            if (t.IsWalkable()) _tilesInActivationRange.Add(t);
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

    private void CheckAttack()
    {
        if (!_attackPending) return;

        _turnCount--;
        _textsList[0].text = _turnCount.ToString();
        _textsList[1].text = _turnCount.ToString();
        
        if (_turnCount > 0) return;
        
        Destroy(_textsList[0].gameObject);
        Destroy(_textsList[1].gameObject);
        
        _textsList.Clear();
        
        TurnManager.Instance.SetMortarAttack(true);
        
        CameraMovement c = FindObjectOfType<CameraMovement>();
        
        c.MoveTo(_centerToAttack.gameObject.transform, Attack);
    }

    void Attack()
    {
        foreach (Tile tile in _tilesToAttack)
        {
            Character unit = tile.GetCharacterAbove();
            if (unit)
            {
                unit.body.TakeDamage(_damage);
                unit.leftArm.TakeDamage(_damage);
                unit.rightArm.TakeDamage(_damage);
                unit.legs.TakeDamage(_damage);
                
                if (unit.body.GetCurrentHp() <= 0) unit.Dead();
            }

            LandMine mine = tile.GetMineAbove();
            if (mine) mine.DestroyMine();

            EffectsController.Instance.PlayParticlesEffect(tile.gameObject, "Mine");
        }
        
        _attackPending = false;
        TurnManager.Instance.SetMortarAttack(false);
        _turnCount = _turnsToAttack;
        Deselect();
    }

    private void Deselect()
    {
        Character selection = CharacterSelection.Instance.GetSelectedCharacter(); 
        
        if (selection) CharacterSelection.Instance.Selection(selection);
        
        _selected = false;
        _highlight.MortarClearTilesInAttackRange(_tilesInAttackRange);

        foreach (Tile t in _tilesToAttack)
        {
            _highlight.PaintTilesInPreviewRange(t);
        }
        
        if (!_attackPending)
        {
            _highlight.ClearTilesInPreview(_tilesInPreviewRange);
            _highlight.ClearTilesInPreview(_tilesToAttack);
            _tilesInPreviewRange.Clear();
            _tilesToAttack.Clear();
        }
        
        _tilesForPreviewChecked.Clear();
        
        _last = null;
    }
    
    
}
