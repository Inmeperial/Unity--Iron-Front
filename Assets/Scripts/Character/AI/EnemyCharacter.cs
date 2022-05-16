using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    [Header("AI")]
    [SerializeField] private float _delayAfterAction;

    private BehaviorExecutor _behaviorExecutor;
    private Character _closestEnemy;
    
    [HideInInspector]
    public bool checkedParts;
    [HideInInspector]
    public bool checkedEnemy;

    private CameraMovement _camera;

    private bool _failSafeRunning;
    public override void Awake()
    {
        base.Awake();

        _camera = FindObjectOfType<CameraMovement>();
        _behaviorExecutor = GetComponent<BehaviorExecutor>();

        if (_behaviorExecutor)
            _behaviorExecutor.paused = true;
    }

    public override void NewTurn()
    {
        base.NewTurn();
        _closestEnemy = null;
        checkedEnemy = false;
    }

    public override void SelectThisUnit()
    {
        if (_myTurn)
            StartCoroutine(DelayStart());
        else
            _behaviorExecutor.paused = true;
        
        if (!_canAttack && !_canMove)
            return;

        base.SelectThisUnit();
        _closestEnemy = null;
        
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(2);
        
        _behaviorExecutor.paused = false;
    }

    public void SetAttackFORTEST()
    {
        _canAttack = false;
    }

    public void ForceBehaviorPause()
    {
        _behaviorExecutor.paused = true;
    }

    public bool FoundPath()
    {
        _closestEnemy = CalculateClosestEnemy();
        
        Tile closestTileToEnemy = ClosestTileToEnemy(_closestEnemy);

        if (!closestTileToEnemy)
            return false;
        
        int distance = 0;

        //Finds the farthest tile I can reach in my movement range.
        for (int i = 0; i < _tilesInMoveRange.Count; i++)
        {
            _waypointsPathfinding.ResetPath();
            _waypointsPathfinding.Calculate(_tilesInMoveRange[i], closestTileToEnemy, 1000);

            List<Tile> path = _waypointsPathfinding.GetPath();
            
            if (i == 0)
            {
                distance = path.Count;
                _targetTile = path[0];
                continue;
            }

            if (path.Count >= distance)
                continue;
            
            distance = path.Count;

            if (distance <= 0) 
                continue;
            
            _targetTile = path[0];
        }

        //Clears previous calculated paths
        _waypointsPathfinding.ResetPath();

        if (_targetTile == null) 
            return false;
        
        if (_targetTile == _myPositionTile)
            return false;
        
        _currentSteps = _legs.GetMaxSteps();
        
        //Calculates shortest path.
        _waypointsPathfinding.Calculate(_myPositionTile, _targetTile, _currentSteps);
        _path = _waypointsPathfinding.GetPath();
        
        TileHighlight.Instance.PathPreview(_path);
        TileHighlight.Instance.CreatePathLines(_path);
        return true;
    }
    
    public Character CalculateClosestEnemy()
    {
        if (!_canAttack && !_canMove)
            return _closestEnemy;
        if (_closestEnemy != null)
            return _closestEnemy;
        
        Character closestEnemy = null;

        List<Tile> path = new List<Tile>();
        
        List<Character> enemies = GetEnemyTeam();
        
        for (int i = 0; i < enemies.Count; i++)
        {

            Tile tile = ClosestTileToEnemy(enemies[i]);

            if (!tile)
                continue;

            _waypointsPathfinding.ResetPath();
            _waypointsPathfinding.Calculate(_myPositionTile, tile, 1000);

            List<Tile> pathToEnemyClosestTile = _waypointsPathfinding.GetPath();

            if (pathToEnemyClosestTile.Count == 0) 
                continue;
            
            if (path.Count == 0)
            {
                foreach (Tile t in pathToEnemyClosestTile)
                    path.Add(t);

                closestEnemy = enemies[i];
                continue;
            }

            if (pathToEnemyClosestTile.Count >= path.Count)
                continue;

            path.Clear();

            foreach (Tile t in pathToEnemyClosestTile)
                path.Add(t);

            closestEnemy = enemies[i];
        }
        _currentSteps = _legs.GetMaxSteps();
        
        return closestEnemy;
    }

    //Search for the closest tile to enemy position.
    Tile ClosestTileToEnemy(Character character)
    {
        Tile closest = null;
        int distance = 0;

        if (!character) 
            return null;
        
        Tile enemyTile = character.GetMyPositionTile();

        if (!enemyTile) 
            return null;
        
        for (int i = 0; i < enemyTile.neighboursForMove.Count; i++)
        {
            for (int j = 0; j < _tilesInMoveRange.Count; j++)
            {
                _waypointsPathfinding.ResetPath();

                _waypointsPathfinding.Calculate(_tilesInMoveRange[j], enemyTile.neighboursForMove[i], 1000);
                List<Tile> path = _waypointsPathfinding.GetPath();
            
                if (path.Count == 0)
                    continue;
                
                //if (i == 0 && j == 0)
                if (distance == 0)
                {
                    distance = path.Count;
                
                    closest = path[path.Count-1];
                    continue;
                }

                if (path.Count >= distance)
                    continue;

                distance = path.Count;
                
                closest = path[path.Count-1];
            }
        }

        return closest;
    }

    List<Character> GetEnemyTeam()
    {
        Character[] units = TurnManager.Instance.GetAllUnits();
        
        List<Character> enemyTeam = new List<Character>();

        foreach (Character unit in units)
        {
            if (!unit.IsDead() && unit.GetUnitTeam() != _unitTeam)
                enemyTeam.Add(unit);
        }
        return enemyTeam;
    }

    public void SetClosestEnemy(Character character)
    {
        checkedEnemy = true;
        _closestEnemy = character;
    }

    public Character GetClosestEnemy() => _closestEnemy;

    public void EnemyMove()
    {
        if (_moving)
            return;

        if (_path.Count > 0)
        {
            _camera.MoveTo(_path[_path.Count-1].transform);
            Move();
        }
        else
        {
            _canMove = false;
            OnEndAction();
        }
    }

    public override void ReachedEnd()
    {
        base.ReachedEnd();
        _failSafeRunning = false;
        _behaviorExecutor.paused = false;
    }

    public void OnStartAction(Action action)
    {
        _behaviorExecutor.paused = true;
        checkedParts = false;
        StopAllCoroutines();
        StartCoroutine(FailSafe(action));
    }

    public void OnEndAction()
    {
        _failSafeRunning = false;
        _behaviorExecutor.paused = false;
    }

    public void ForceEnd()
    {
        _behaviorExecutor.paused = true;
        checkedParts = false;
        _closestEnemy = null;
    }

    /// <summary>
    /// Resumes behaviour tree execution after the given time.
    /// </summary>
    /// <param name="time">Time to resume. If time is less than or equal 0 uses character default time.</param>
    public void OnEndActionWithDelay(float time)
    {
        if (time <= 0)
            time = _delayAfterAction;

        StartCoroutine(EndDelay(time));
    }

    IEnumerator EndDelay(float time)
    {
        yield return new WaitForSeconds(time);
        
        OnEndAction();
    }

    //protected override void ConfigureMecha()
    //{
    //    if (!_mechaEquipment)
    //        _mechaEquipment = _equipment;
    //    base.ConfigureMecha();
    //}

    IEnumerator FailSafe(Action action)
    {
        if (_failSafeRunning) 
            yield return null;
        
        _failSafeRunning = true;

        float time = 0f;

        while (_failSafeRunning && time <= 5)
        {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (_failSafeRunning && time >= 5)
            action?.Invoke();

        _failSafeRunning = false;
    }
}
