﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    private BehaviorExecutor _behaviorExecutor;
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

    public void CalculateAutoMovement()
    {
        List<Character> enemyTeam = GetEnemyTeam();

        Character closestEnemy = GetClosestEnemy(enemyTeam);


        Tile enemyTile = closestEnemy.GetMyPositionTile();
        Tile closestTileToEnemy = null;
        
        int distance = 0;
        
        //Search for the closest tile to enemy position.
        for (int i = 0; i < enemyTile.neighboursForMove.Count; i++)
        {
            pathCreator.ResetPath();
            pathCreator.Calculate(_tilesInMoveRange[i], enemyTile.neighboursForMove[i], 1000);
            List<Tile> p = pathCreator.GetPath();
            
            if (i == 0)
            {
                distance = p.Count;
                
                closestTileToEnemy = p[p.Count-1];
                continue;
            }
            
            if (p.Count >= distance) continue;
            
            distance = p.Count;
                
            closestTileToEnemy = p[p.Count-1];
        }

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
        
        _currentSteps = legs.GetMaxSteps();
        
        //Calculates shortest path.
        pathCreator.Calculate(_myPositionTile, _targetTile, _currentSteps);
        _path = pathCreator.GetPath();

        highlight.PathPreview(_path);
        highlight.CreatePathLines(_path);
    }

    Character GetClosestEnemy(List<Character> enemies) 
    {
        Character closestEnemy = null;
        
        List<Tile> path = new List<Tile>();
        
        for (int i = 0; i < enemies.Count; i++)
        {
            pathCreator.ResetPath();
            pathCreator.Calculate(_myPositionTile, enemies[i].GetMyPositionTile(), 1000);

            List<Tile> p = pathCreator.GetPath();
            if (i == 0)
            {
                foreach (var tile in p)
                {
                    path.Add(tile);
                }
                closestEnemy = enemies[i];
                continue;
            }

            if (p.Count >= path.Count) continue;
            path.Clear();
                foreach (var tile in p)
                {
                    path.Add(tile);
                }
                closestEnemy = enemies[i];
        }
        _currentSteps = legs.GetMaxSteps();
        return closestEnemy;
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
    }

    public void OnEndAction()
    {
        _behaviorExecutor.paused = false;
    }
}
