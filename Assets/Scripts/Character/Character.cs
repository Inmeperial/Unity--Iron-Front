using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : Teams
{
    //STATS
    #region Stats

    public Transform rayOrigin;
    [Header("Team")]
    [SerializeField] private Team _unitTeam;

    [Header("Body")] 
    [SerializeField] private Body _myBody;
    [SerializeField] private int _bodyMaxHP;
    [SerializeField] private int _bodyHP;
    [SerializeField] private Transform _bodyTransform;

    [Header("Left Arm")]
    [SerializeField] LeftArm _myLeftArm;
    public Gun leftGun;
    private bool _leftGunSelected;
    [SerializeField] private int _leftArmMaxHP;
    [SerializeField] private int _leftArmHP;
    [SerializeField] private Transform _lArmTransform;

    [Header("Right Arm")]
    [SerializeField] RightArm _myRightArm;
    public Gun rightGun;
    private bool _rightGunSelected;
    [SerializeField] private int _rightArmMaxHP;
    [SerializeField] private int _rightArmHP;
    [SerializeField] private Transform _rArmTransform;

    [Header("Legs")] 
    public Legs legs;
    [SerializeField] private Transform _legsTransform;
    private int _currentSteps;
    #endregion

    private Gun _selectedGun;

    
    
    //MOVEMENT RELATED
    public IPathCreator pathCreator;
    private GridMovement _move;
    public LayerMask block;
    private List<Tile> _tilesInMoveRange = new List<Tile>();
    private Tile _myPositionTile;
    private Tile _targetTile;
    private List<Tile> _path = new List<Tile>();

    //FLAGS
    private bool _canBeSelected;
    private bool _selected;
    private bool _moving = false;
    private bool _canMove = true;
    private bool _canAttack = true;
    private bool _leftArmAlive;
    private bool _rightArmAlive;
    private bool _canBeAttacked = false;

    //OTHERS
    private HashSet<Tile> _tilesInAttackRange = new HashSet<Tile>();
    private Dictionary<Tile, int> _tilesForAttackChecked = new Dictionary<Tile, int>();
    private Dictionary<Tile, int> _tilesForMoveChecked = new Dictionary<Tile, int>();
    private List<Character> _enemiesInRange = new List<Character>();
    private WorldUI _myUI;
    public EffectsController effectsController;

    [Header("DON'T SET")]
    public TurnManager turnManager;

    public TileHighlight highlight;

    public ButtonsUIManager buttonsManager;

    // Start is called before the first frame update
    void Start()
    {
        _canBeSelected = true;
        _bodyHP = _bodyMaxHP;
        _leftArmHP = _leftArmMaxHP;
        _leftArmAlive = _leftArmHP > 0 ? true : false;
        _rightArmHP = _rightArmMaxHP;
        _rightArmAlive = _rightArmHP > 0 ? true : false;
        _canMove = legs.GetLegsHP() > 0 ? true : false;
        _currentSteps = _canMove ? legs.GetMaxSteps() : 0;
        _selected = false;
        
        _move = GetComponent<GridMovement>();
        _move.SetMoveSpeed(legs.GetMoveSpeed());
        _move.SetRotationSpeed(legs.GetRotationSpeed());
        _myPositionTile = GetTileBelow();
        _myPositionTile.MakeTileOccupied();
        _myPositionTile.SetUnitAbove(this);
        turnManager = FindObjectOfType<TurnManager>();
        highlight = FindObjectOfType<TileHighlight>();
        buttonsManager = FindObjectOfType<ButtonsUIManager>();
        pathCreator = GetComponent<IPathCreator>();
        effectsController = FindObjectOfType<EffectsController>();
        _myUI = GetComponent<WorldUI>();

        if (_myUI)
            _myUI.SetLimits(_bodyMaxHP, _rightArmMaxHP, _leftArmMaxHP, legs.GetLegsMaxHP());

        if (_rightArmAlive)
        {
            _selectedGun = rightGun;
            _canAttack = true;
            _rightGunSelected = true;
            _leftGunSelected = false;
        }
        else if (_leftArmAlive)
        {
            _selectedGun = leftGun;
            _canAttack = true;
            _rightGunSelected = false;
            _leftGunSelected = true;
        }
        else
        {
            _selectedGun = null;
            _canAttack = false;
            _rightGunSelected = false;
            _leftGunSelected = false;
        }

        if (_bodyHP <= 0)
            NotSelectable();

        for (int i = 0; i < transform.childCount; i++)
        {
            var c = transform.GetChild(i);
            if (c.gameObject.name == "Body")
            {
                _bodyTransform = c;
                continue;
            }
                
            if (c.gameObject.name == "Legs")
            {
                _legsTransform = c;
                continue;
            }

            if (c.gameObject.name == "RArm")
            {
                _rArmTransform = c;
                continue;
            }

            if (c.gameObject.name == "LArm")
                _lArmTransform = c;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_selected && !_moving && _canMove && Input.GetMouseButtonDown(0))
        {
            GetTargetToMove();
        }
    }

    void PaintTilesInAttackRange(Tile currentTile, int count)
    {
        if (count >= _selectedGun.GetAttackRange() || (_tilesForAttackChecked.ContainsKey(currentTile) && _tilesForAttackChecked[currentTile] <= count))
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
                        highlight.PaintTilesInMoveAndAttackRange(tile);
                    }
                    else highlight.PaintTilesInAttackRange(tile);
                }

            }
            PaintTilesInAttackRange(tile, count + 1);
        }
    }

    public void PaintTilesInMoveRange(Tile currentTile, int count)
    {
        if (count >= _currentSteps || (_tilesForMoveChecked.ContainsKey(currentTile) && _tilesForMoveChecked[currentTile] <= count))
            return;

        _tilesForMoveChecked[currentTile] = count;

        foreach (var tile in currentTile.neighboursForMove)
        {
            if (!_tilesInMoveRange.Contains(tile))
            {
                if (tile.IsWalkable() && tile.IsFree())
                {
                    _tilesInMoveRange.Add(tile);
                    tile.inMoveRange = true;
                    if (tile.inAttackRange)
                    {
                        highlight.PaintTilesInMoveAndAttackRange(tile);
                    }
                    else highlight.PaintTilesInMoveRange(tile);
                }
                
            }
            PaintTilesInMoveRange(tile, count + 1);
        }
    }

    public void AddTilesInMoveRange()
    {
        highlight.AddTilesInMoveRange(_tilesInMoveRange);
    }

    #region Actions
    //This method is called from UI Button "Move".
    public void Move()
    {
        if (_moving == false && _path != null && _path.Count > 0)
        {
            _moving = true;
            buttonsManager.DeactivateBodyPartsContainer();
            buttonsManager.DeactivateMoveContainer();
            turnManager.UnitIsMoving();
            highlight.characterMoving = true;
            _myPositionTile.unitAboveSelected = false;
            _myPositionTile.EndMouseOverColor();
            ResetTilesInMoveRange();
            _move.StartMovement(_path);
        }
    }

    public void TakeDamageBody(int[] damages)
    {
        foreach (var item in damages)
        {
            if (item == 0)
            {
                Debug.Log("Bullet miss");
            }
            else
            {
                var hp = _bodyHP - item;
                _bodyHP = hp > 0 ? hp : 0;
                effectsController.PlayEffect(_bodyTransform.position, "Damage");
            }
        }
        if (_bodyHP <= 0)
        {
            NotSelectable();
        }
        buttonsManager.UpdateBodyHUD(_bodyHP, false);
        MakeNotAttackable();
    }

    public void TakeDamageLeftArm(int[] damages)
    {
        foreach (var item in damages)
        {
            if (item == 0)
            {
                Debug.Log("Bullet miss");
            }
            else
            {
                var hp = _leftArmHP - item;
                _leftArmHP = hp > 0 ? hp : 0;
                _leftArmAlive = _leftArmHP > 0 ? true : false;
                effectsController.PlayEffect(_lArmTransform.position, "Damage");
            }
        }
        CheckArms();
        buttonsManager.UpdateLeftArmHUD(_leftArmHP, false);
        MakeNotAttackable();
    }

    public void TakeDamageRightArm(int[] damages)
    {
        foreach (var item in damages)
        {
            if (item == 0)
            {
                Debug.Log("Bullet miss");
            }
            else
            {
                var hp = _rightArmHP - item;
                _rightArmHP = hp > 0 ? hp : 0;
                _rightArmAlive = _rightArmHP > 0 ? true : false;
                effectsController.PlayEffect(_rArmTransform.position, "Damage");
            }
        }
        CheckArms();
        buttonsManager.UpdateRightArmHUD(_rightArmHP, false);
        MakeNotAttackable();
    }

    public void TakeDamageLegs(int[] damages)
    {
        foreach (var item in damages)
        {
            if (item == 0)
            {
                Debug.Log("Bullet miss");
            }
            else
            {
                var hp = legs.GetLegsHP() - item;
                legs.UpdateHP(hp > 0 ? hp : 0);
                _canMove = legs.GetLegsHP() > 0 ? true : false;
                effectsController.PlayEffect(_legsTransform.position, "Damage");
            }
        }
        buttonsManager.UpdateLegsHUD(legs.GetLegsHP(), false);
        MakeNotAttackable();
    }

    public void SelectLeftGun()
    {
        _selectedGun = leftGun;
        _leftGunSelected = true;
        _rightGunSelected = false;
        ResetTilesInAttackRange();
        ResetTilesInMoveRange();
        if (_canAttack)
        {
            if (_path.Count == 0)
                PaintTilesInAttackRange(_myPositionTile, 0);
            else PaintTilesInAttackRange(_path[_path.Count - 1], 0);
            CheckEnemiesInAttackRange();
        }

        if (_canMove)
        {
            if (_path.Count == 0)
                PaintTilesInMoveRange(_myPositionTile, 0);
            else PaintTilesInMoveRange(_path[_path.Count - 1], 0);
        }
    }

    public void SelectRightGun()
    {
        _selectedGun = rightGun;
        _leftGunSelected = false;
        _rightGunSelected = true;
        ResetTilesInAttackRange();
        ResetTilesInMoveRange();
        if (_canAttack)
        {
            if (_path.Count == 0)
                PaintTilesInAttackRange(_myPositionTile, 0);
            else PaintTilesInAttackRange(_path[_path.Count - 1], 0);
            CheckEnemiesInAttackRange();
        }
        
        if (_canMove)
        {
            if (_path.Count == 0)
                PaintTilesInMoveRange(_myPositionTile, 0);
            else PaintTilesInMoveRange(_path[_path.Count - 1], 0);
        }
    }

    public void ResetTilesInAttackRange()
    {
        highlight.ClearTilesInAttackRange(_tilesInAttackRange);
        _tilesInAttackRange.Clear();
        _tilesForAttackChecked.Clear();
    }

    public void ResetTilesInMoveRange()
    {
        highlight.ClearTilesInMoveRange(_tilesInMoveRange);
        _tilesInMoveRange.Clear();
        _tilesForMoveChecked.Clear();
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
                pathCreator.Calculate(this, _targetTile, _currentSteps);
                if (pathCreator.GetDistance() <= legs.GetMaxSteps())
                {
                    _path = pathCreator.GetPath();
                    if (_path.Count > 0)
                    {
                        highlight.PathPreview(_path);
                        buttonsManager.ActivateMoveButton();
                        buttonsManager.ActivateUndo();
                        ResetTilesInMoveRange();
                        ResetTilesInAttackRange();
                        highlight.CreatePathLines(_path);
                        
                        if (CanAttack())
                            PaintTilesInAttackRange(_path[_path.Count - 1], 0);

                        PaintTilesInMoveRange(_path[_path.Count - 1], 0);
                        AddTilesInMoveRange();
                    }
                }
            }
        }
    }

    public int GetCurrentSteps()
    {
        return _currentSteps;
    }
    
    public Tile GetEndTile()
    {
        return _targetTile;
    }

    public Team GetUnitTeam()
    {
        return _unitTeam;
    }

    

    public int GetBodyMaxHP()
    {
        return _bodyMaxHP;
    }

    public int GetBodyHP()
    {
        return _bodyHP;
    }

    public int GetLeftArmMaxHP()
    {
        return _leftArmMaxHP;
    }

    public int GetLeftArmHP()
    {
        return _leftArmHP;
    }

    public int GetRightArmMaxHP()
    {
        return _rightArmMaxHP;
    }

    public int GetRightArmHP()
    {
        return _rightArmHP;
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
        _path = pathCreator.GetPath();
        return _path;
    }

    public Tile GetTileBelow()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, LayerMask.NameToLayer("GridBlock"));
        return hit.transform.gameObject.GetComponent<Tile>();
    }
    
    public bool HasEnemiesInRange()
    {
        return _enemiesInRange.Count > 0 ? true : false;
    }

    public Gun GetSelectedGun()
    {
        return _selectedGun;
    }

    public Gun GetLeftGun()
    {
        return leftGun;
    }

    public Gun GetRightGun()
    {
        return rightGun;
    }

    public Vector3 GetBodyPosition()
    {
        return _bodyTransform.GetComponent<Renderer>().bounds.center;
    }
    public Vector3 GetLArmPosition()
    {
        return _lArmTransform.GetComponent<Renderer>().bounds.center;
    }
    public Vector3 GetRArmPosition()
    {
        return _rArmTransform.GetComponent<Renderer>().bounds.center;
    }
    public Vector3 GetLegsPosition()
    {
        return _legsTransform.GetComponent<Renderer>().bounds.center;
    }

    #endregion

    #region Utilities
    
    public void Undo()
    {
        foreach (var item in _enemiesInRange)
        {
            turnManager.UnitCantBeAttacked(item);
        }
        ResetInRangeLists();
        GetPath();
        Debug.Log("path count: " + _path.Count);
        if (_path.Count > 0)
        {
            buttonsManager.ActivateUndo();
            PaintTilesInMoveRange(_path[_path.Count - 1], 0);
            if (CanAttack())
                PaintTilesInAttackRange(_path[_path.Count - 1], 0);
        }
        else
        {
            buttonsManager.DeactivateUndo();
            PaintTilesInMoveRange(_myPositionTile, 0);
            if (CanAttack())
                PaintTilesInAttackRange(_myPositionTile, 0);
        }
        
    }

    public void ResetInRangeLists()
    {
        foreach (var item in _enemiesInRange)
        {
            turnManager.UnitCantBeAttacked(item);
        }
        highlight.PathLinesClear();
        ResetTilesInMoveRange();
        ResetTilesInAttackRange();
        highlight.Undo();
        _enemiesInRange.Clear();
    }
    public void NewTurn()
    {
        _canMove = legs.GetLegsHP() > 0 ? true : false;
        _canAttack = true;
        _selectedGun.SetGun();
        _path.Clear();
        _currentSteps = _canMove ? legs.GetMaxSteps() : 0;
        _enemiesInRange.Clear();
        MakeNotAttackable();
        pathCreator.ResetPath();
    }

    public void ClearTargetTile()
    {
        _targetTile = null;
    }

    public void ReachedEnd()
    {
        _canMove = false;
        highlight.characterMoving = false;
        highlight.EndPreview();
        _moving = false;
        _myPositionTile.MakeTileFree();
        _myPositionTile = _targetTile;
        _myPositionTile.MakeTileOccupied();
        _myPositionTile.SetUnitAbove(this);
        _myPositionTile.unitAboveSelected = true;
        _myPositionTile.MouseOverColor();
        _targetTile = null;
        turnManager.UnitStoppedMoving();
        pathCreator.ResetPath();
        _tilesForAttackChecked.Clear();
        
        if (CanAttack())
        {
            PaintTilesInAttackRange(_myPositionTile, 0);
            CheckEnemiesInAttackRange();
        }
    }

    public void ReduceAvailableSteps(int amount)
    {
        var s = _currentSteps - amount;
        _currentSteps = s >= 0 ? s : 0;
    }

    public void IncreaseAvailableSteps(int amount)
    {
        var s = _currentSteps + amount;
        _currentSteps = s <= legs.GetMaxSteps() ? s : legs.GetMaxSteps();
    }

    public void SelectThisUnit()
    {
        _selected = true;
        ResetInRangeLists();
        _path.Clear();
        highlight.PathLinesClear();
        _myPositionTile = GetTileBelow();
        _myPositionTile.unitAboveSelected = true;
        _myPositionTile.GetComponent<TileMaterialhandler>().DiseableAndEnableSelectedNode(true);
        if (CanAttack())
        {
            if (_rightArmAlive)
                _selectedGun = rightGun;
            else if (_leftArmAlive)
                _selectedGun = leftGun;
            PaintTilesInAttackRange(_myPositionTile, 0);
            CheckEnemiesInAttackRange();
        }
        if (_canMove)
        {
            _currentSteps = legs.GetMaxSteps();
            AddTilesInMoveRange();
            PaintTilesInMoveRange(_myPositionTile, 0);
        }
    }

    public void DeselectThisUnit()
    {
        _selected = false;
        _myPositionTile.unitAboveSelected = false;
        _myPositionTile.EndMouseOverColor();
        foreach (var item in _enemiesInRange)
        {
            turnManager.UnitCantBeAttacked(item);
        }

        if (_canMove)
            _currentSteps = legs.GetMaxSteps();
        ResetInRangeLists();
        _path.Clear();
        highlight.PathLinesClear();
        pathCreator.ResetPath();
    }

    public void NotSelectable()
    {
        _canBeSelected = false;
    }
    
    public void SelectedAsEnemy()
    {
        _myPositionTile = GetTileBelow();
        _myPositionTile.unitAboveSelected = true;
        _myPositionTile.GetComponent<TileMaterialhandler>().DiseableAndEnableSelectedNode(true);
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

    void CheckEnemiesInAttackRange()
    {
        if (_tilesInAttackRange != null && _tilesInAttackRange.Count > 0)
        {
            foreach (var unit in _enemiesInRange)
            {
                turnManager.UnitCantBeAttacked(unit);
            }
            _enemiesInRange.Clear();
            foreach (var item in _tilesInAttackRange)
            {
                if (item.IsFree() == false)
                {
                    var unit = item.GetUnitAbove();
                    if (unit.GetUnitTeam() != _unitTeam)
                    {
                        turnManager.UnitCanBeAttacked(unit);
                        _enemiesInRange.Add(unit);
                    }
                }
            }
        }
    }
    
    public bool ThisUnitCanMove()
    {
        return _canMove;
    }

    public bool IsMoving()
    {
        return _moving;
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

    public void CheckArms()
    {
        if (!_leftArmAlive && !_rightArmAlive)
            _canAttack = false;
    }

    public bool LeftArmAlive()
    {
        return _leftArmAlive;
    }

    public bool RightArmAlive()
    {
        return _rightArmAlive;
    }

    public bool CanBeSelected()
    {
        return _canBeSelected;
    }
    public void DeactivateAttack()
    {
        ResetTilesInAttackRange();
        ResetTilesInMoveRange();
        highlight.PathLinesClear();
        _canAttack = false;
    }

    public void DeactivateMoveButton()
    {
        buttonsManager.DeactivateMoveButton();
    }
    #endregion

    private void OnMouseOver()
    {
        ShowWorldUI();
    }

    private void OnMouseExit()
    {
        HideWorldUI();
    }

    void ShowWorldUI()
    {
        _myUI.ActivateWorldUI(GetBodyHP(), GetRightArmHP(), GetLeftArmHP(), legs.GetLegsHP(), ThisUnitCanMove(), CanAttack());
    }

    void HideWorldUI()
    {
        _myUI.DeactivateWorldUI();
    }

    public void RotateTowardsEnemy(Vector3 pos)
    {
        _move.SetPosToRotate(pos);
        _move.StartRotation();
    }

    public bool RayToPartsForAttack(Vector3 pos, string tagToCheck)
    {
        RaycastHit hit;
        var position = rayOrigin.position;
        var dir = (pos - position).normalized; 
        Physics.Raycast(position, dir, out hit, 1000f);
        if (hit.collider.transform.gameObject.CompareTag(tagToCheck))
        {
            Debug.DrawRay(rayOrigin.position, dir, Color.green, 1000f);
            return true;
        }
            
        else
        {
            Debug.DrawRay(rayOrigin.position, dir, Color.red, 1000f);
            return false;
        }
    }

    public void SetCharacterMove(bool state)
    {
        _canMove = state;
    }

    public void Shoot()
    {
        if (_rightGunSelected)
        {
            effectsController.PlayEffect(_rArmTransform.position, "Attack");            
        }
        else if (_leftGunSelected)
        {
            effectsController.PlayEffect(_lArmTransform.position, "Attack");
        }
        
    }
}
