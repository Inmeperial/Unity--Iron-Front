using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class Mortar : MonoBehaviour, IInteractable, IEndActionNotifier
{
    [Header("References")]
    [SerializeField] private Transform _textSpawnPos;
    [SerializeField] private GameObject _turnsCounterTextPrefab;
    [SerializeField] private GameObject _interactionShaderObject;

    [Header("Stats")]
    [SerializeField] private int _shootRange;
    [SerializeField] private int _aoe;
    [SerializeField] private int _damage;
    [SerializeField] private int _turnsToAttack;

    [Header("Configs")]
    [SerializeField] private LayerMask _mortarMask;
    [SerializeField] private LayerMask _gridBlock;
    [SerializeField] private KeyCode _deselectKey;
    [SerializeField] private float _activationTilesDetectionRange;
    [SerializeField] private float _waitAfterAttack;
    [SerializeField] private SoundData _sound;
    [SerializeField] private ParticleSystem _particleEffect;

    [Header("Debug")]
    [SerializeField] private bool _drawGizmo;

    private List<TextMeshProUGUI> _textsList = new List<TextMeshProUGUI>();
    private Dictionary<Tile, int> _tilesForAttackChecked = new Dictionary<Tile, int>();
    private Dictionary<Tile, int> _tilesForPreviewChecked = new Dictionary<Tile, int>();

    private HashSet<Tile> _tilesInAttackRange = new HashSet<Tile>();
    private HashSet<Tile> _tilesInPreviewRange = new HashSet<Tile>();

    private HashSet<Tile> _tilesToAttack = new HashSet<Tile>();
    private Tile _centerToAttack;
    private TileHighlight _highlight;

    Mortar[] _mortars;

    private bool _selected = false;

    private Tile _myPositionTile;

    private Tile _last;

    private HashSet<Tile> _tilesInActivationRange = new HashSet<Tile>();
    private Character _activationCharacter;
    private bool _attackPending = false;
    private int _turnCount = 0;

    public Action<IEndActionNotifier> OnEndAction { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Instance.InputsReader.OnDeselectKeyPressed += Deselect;

        _highlight = FindObjectOfType<TileHighlight>();
        _myPositionTile = GetTileBelow();
        _selected = false;

        GetTilesInActivationRange();
        
        GetTilesInAttackRange(_myPositionTile, 0);

        _mortars = FindObjectsOfType<Mortar>();

        GameManager.Instance.OnBeginTurn += CheckIfCanBeUsed;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GameManager.Instance.InputsReader.CanCheckKeys || GameManager.Instance.CurrentTurnMecha.IsUsingEquipable())
            return;

        if (Input.GetMouseButtonDown(0) && CanInteract() && EventSystem.current.IsPointerOverGameObject() == false)
            Interact();
    }

    public void Interact()
    {
        CharacterSelector.Instance.DisableCharacterSelection();

        GameManager.Instance.CurrentTurnMecha.DeselectThisUnit();

        if (!_selected)
        {
            foreach (Mortar mortar in _mortars)
            {
                if (mortar != this)
                    mortar.Deselect();
            }
            
            Character selectedCharacter = GameManager.Instance.CurrentTurnMecha;

            selectedCharacter.SetSelection(false);

            selectedCharacter.LoadRotationOnDeselect();

            _selected = true;

            if (IsSelectedCharacterAbove())
                _activationCharacter.ResetInRangeLists();

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
            
            if (!IsValidTarget(target)) 
                return;
            
            if (!IsSelectedCharacterAbove())
                return;
            
            _activationCharacter.ResetInRangeLists();

            foreach (Tile tiles in _tilesInAttackRange)
            {
                tiles.inAttackRange = true;
                _highlight.MortarPaintTilesInAttackRange(tiles);
            }

            Tile tile = target.GetComponent<Tile>();

            if (tile == _myPositionTile)
                return;

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
            else if (tile == _last && IsSelectedCharacterAbove())
            {
                _centerToAttack = _last;
                PrepareAttack();
            }
        }
    }

    //Determina si hay alguna unidad sobre las tiles de activacion
    private bool IsSelectedCharacterAbove()
    {
        foreach (Tile tile in _tilesInActivationRange)
        {
            Character character = tile.GetUnitAbove();
            
            if (!GameManager.Instance.IsActiveMecha(character))
                continue;
            
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
        if (EventSystem.current.IsPointerOverGameObject())
            return false;

        if (!target) 
            return false;

        if (target.gameObject.layer != LayerMask.NameToLayer("GridBlock"))
            return false;

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
        
        _activationCharacter.DoAttackAction();
        _activationCharacter = null;

        DisableInteractionVisuals();

        GameManager.Instance.OnEndTurn += CheckAttack;

        Deselect();
    }

    private void GetTilesInActivationRange()
    {
        Collider[] tiles = Physics.OverlapSphere(transform.position, _activationTilesDetectionRange, _gridBlock);

        foreach (Collider tile in tiles)
        {
            Tile t = tile.transform.GetComponent<Tile>();
            
            if (t.IsWalkable()) _tilesInActivationRange.Add(t);
        }
    }
    
    
    private void OnDrawGizmos()
    {
        if (!_drawGizmo) return;
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _activationTilesDetectionRange);

    }

    private void CheckAttack()
    {
        if (!_attackPending)
            return;

        _turnCount--;
        _textsList[0].text = _turnCount.ToString();
        _textsList[1].text = _turnCount.ToString();
        
        if (_turnCount > 0)
            return;
        
        Destroy(_textsList[0].gameObject);
        Destroy(_textsList[1].gameObject);
        
        _textsList.Clear();
        
        //TurnManager.Instance.SetMortarAttack(true);

        CameraMovement cameraMovement = GameManager.Instance.GameCamerasController.CameraMovement;

        GameManager.Instance.AddEndTurnAction(this, () => cameraMovement.MoveTo(_centerToAttack.gameObject.transform, Attack));
    }

    void Attack()
    {
        AudioManager.Instance.PlaySound(_sound, gameObject);
        foreach (Tile tile in _tilesToAttack)
        {
            Character unit = tile.GetUnitAbove();

            if (unit)
            {
                unit.GetBody().ReceiveDamage(_damage);
                
                if (unit.GetLeftGun())
                    unit.GetLeftGun().ReceiveDamage(_damage);
                
                if (unit.GetRightGun())
                    unit.GetRightGun().ReceiveDamage(_damage);
                
                unit.GetLegs().ReceiveDamage(_damage);
            }

            LandMine mine = tile.GetMineAbove();

            if (mine)
                mine.DestroyMine();

            Vector3 posForVFX = tile.transform.position + new Vector3(0, tile.transform.localScale.y / 2, 0);
            EffectsController.Instance.PlayParticlesEffect(_particleEffect, posForVFX, tile.transform.forward);
            EffectsController.Instance.CameraShake();
        }
        
        _attackPending = false;

        _turnCount = _turnsToAttack;

        GameManager.Instance.OnEndTurn -= CheckAttack;

        ClearTiles();

        Deselect();

        StartCoroutine(DelayAfterAttack());
    }

    private void CheckIfCanBeUsed()
    {
        if (_attackPending || GameManager.Instance.ActiveTeam == EnumsClass.Team.Red)
        {
            DisableInteractionVisuals();
            return;
        }

        EnableInteractionVisuals();
    }

    private IEnumerator DelayAfterAttack()
    {
        yield return new WaitForSeconds(_waitAfterAttack);

        OnEndAction?.Invoke(this);
    }

    private void Deselect()
    {
        if (!_selected)
            return;
        
        _selected = false;

        _highlight.MortarClearTilesInAttackRange(_tilesInAttackRange);

        foreach (Tile t in _tilesToAttack)
        {
            _highlight.PaintTilesInPreviewRange(t);
        }

        if (!_attackPending)
            ClearTiles();
        
        _tilesForPreviewChecked.Clear();
        
        _last = null;

        CharacterSelector.Instance.EnableCharacterSelection();

        Character currentMecha = GameManager.Instance.CurrentTurnMecha;

        if (currentMecha)
            CharacterSelector.Instance.Selection(currentMecha);
    }

    private void ClearTiles()
    {
        _highlight.MortarClearTilesInAttackRange(_tilesInAttackRange);
        _highlight.ClearTilesInPreview(_tilesInPreviewRange);
        _highlight.ClearTilesInPreview(_tilesToAttack);
        _tilesInPreviewRange.Clear();
        _tilesToAttack.Clear();
    }

    private bool CanInteract()
    {
        if (MouseRay.GetTargetGameObject(_mortarMask) != gameObject && !_selected)
            return false;

        if (GameManager.Instance.ActiveTeam == EnumsClass.Team.Red)
            return false;

        if (GameManager.Instance.CurrentTurnMecha == null)
            return false;

        if (!GameManager.Instance.CurrentTurnMecha.CanAttack())
            return false;

        if (GameManager.Instance.CurrentTurnMecha.IsSelectingEnemy())
            return false;

        if (_attackPending)
            return false;

        return true;
    }

    private void EnableInteractionVisuals()
    {
        _interactionShaderObject.SetActive(true);

        PaintTilesInActivationRange();
    }

    private void DisableInteractionVisuals()
    {
        _interactionShaderObject.SetActive(false);

        UnpaintTilesInActivationRange();
    }

    private void PaintTilesInActivationRange()
    {
        foreach (Tile tile in _tilesInActivationRange)
        {
            TileHighlight.Instance.MortarPaintTilesInActivationRange(tile);
        }
    }
    
    private void UnpaintTilesInActivationRange()
    {
        TileHighlight.Instance.ClearTilesInActivationRange(_tilesInActivationRange);
    }

    private void OnDestroy()
    {
        GameManager.Instance.InputsReader.OnDeselectKeyPressed -= Deselect;
        GameManager.Instance.OnBeginTurn -= CheckIfCanBeUsed;
    }
}
