using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyCharacter : Character
{
    private BehaviorExecutor _behaviorExecutor;
    public bool checkedParts;
    private Character _closestEnemy;
    protected override void Awake()
    {
        base.Awake();

        _behaviorExecutor = GetComponent<BehaviorExecutor>();

        if (_behaviorExecutor)
            _behaviorExecutor.paused = true;
    }

    public override void SelectThisUnit()
    {
        base.SelectThisUnit();
        _closestEnemy = null;
        _behaviorExecutor.paused = false;
    }

    public void SetAttackFORTEST()
    {
        _canAttack = false;
    }

    public void PauseFORTEST()
    {
        _behaviorExecutor.paused = true;
    }

    public bool CalculateAutoMovement()
    {
        List<Character> enemyTeam = GetEnemyTeam();

       _closestEnemy = CalculateClosestEnemy(enemyTeam);
        
        Tile closestTileToEnemy = ClosestTileToEnemy(_closestEnemy);
        
        int distance = 0;

        //Finds the farthest tile I can reach in my movement range.
        for (int i = 0; i < _tilesInMoveRange.Count; i++)
        {
            pathCreator.ResetPath();
            pathCreator.Calculate(_tilesInMoveRange[i], closestTileToEnemy, 1000);

            List<Tile> p = pathCreator.GetPath();
            
            if (i == 0)
            {
                distance = p.Count;
                _targetTile = p[0];
                continue;
            }

            if (p.Count >= distance) continue;
            
            distance = p.Count;

            _targetTile = p[0];
        }
        
        //Clears previous calculated paths
        pathCreator.ResetPath();
        
        if (_targetTile == _myPositionTile)
            return false;
        
        _currentSteps = legs.GetMaxSteps();
        
        //Calculates shortest path.
        pathCreator.Calculate(_myPositionTile, _targetTile, _currentSteps);
        _path = pathCreator.GetPath();
        
        highlight.PathPreview(_path);
        highlight.CreatePathLines(_path);
        return true;
    }
    
    private Character CalculateClosestEnemy(List<Character> enemies) 
    {
        Character closestEnemy = null;
        
        List<Tile> path = new List<Tile>();
        
        for (int i = 0; i < enemies.Count; i++)
        {
            Tile tile = ClosestTileToEnemy(enemies[i]);
            pathCreator.ResetPath();
            pathCreator.Calculate(_myPositionTile, tile, 1000);
            
            
            
            List<Tile> p = pathCreator.GetPath();
            if (i == 0)
            {
                foreach (var t in p)
                {
                    path.Add(t);
                }
                closestEnemy = enemies[i];
                continue;
            }

            if (p.Count >= path.Count) continue;
            path.Clear();
            foreach (var t in p)
            {
                path.Add(t);
            }
            closestEnemy = enemies[i];
        }
        _currentSteps = legs.GetMaxSteps();
        
        return closestEnemy;
    }

    //Search for the closest tile to enemy position.
    Tile ClosestTileToEnemy(Character character)
    {
        Tile closest = null;
        int distance = 0;
        Tile enemyTile = character.GetMyPositionTile();
        for (int i = 0; i < enemyTile.neighboursForMove.Count; i++)
        {
            pathCreator.ResetPath();
            
            if (_tilesInMoveRange.Count > 0)
                pathCreator.Calculate(_tilesInMoveRange[i], enemyTile.neighboursForMove[i], 1000);
            else pathCreator.Calculate(_myPositionTile.allNeighbours[i], enemyTile.neighboursForMove[i], 1000);
            List<Tile> p = pathCreator.GetPath();
            
            if (i == 0)
            {
                distance = p.Count;
                
                closest = p[p.Count-1];
                continue;
            }

            if (p.Count >= distance) continue;
            distance = p.Count;

            if (p.Count == 0)
                return closest;
            closest = p[p.Count-1];
        }

        return closest;
    }

    List<Character> GetEnemyTeam()
    {
        Character[] units = TurnManager.Instance.GetAllUnits();
        
        List<Character> enemyTeam = new List<Character>();

        foreach (Character c in units)
        {
            if (!c.IsDead() && c.GetUnitTeam() != _unitTeam)
            {
                enemyTeam.Add(c);
            }
        }

        return enemyTeam;
    }

    public Character GetClosestEnemy()
    {
        return _closestEnemy;
    }

    public void EnemyMove()
    {
        Move();
    }

    public override void ReachedEnd()
    {
        base.ReachedEnd();
        _behaviorExecutor.paused = false;
    }

    public void OnStartAction()
    {
        _behaviorExecutor.paused = true;
        checkedParts = false;
        //_behaviorExecutor.restartWhenFinished = true;
    }

    public void OnEndAction()
    {
        _behaviorExecutor.paused = false;
    }
}
