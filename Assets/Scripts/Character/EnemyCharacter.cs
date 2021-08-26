using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    public override void SetTurn(bool state)
    {
        base.SetTurn(state);
        Debug.Log("start turn ia");
        
        
        
        //Move();
        StartCoroutine(StartMovement());
    }

    void CalculateAutoMovement()
    {
        Debug.Log("calculate auto movement ia");
        

        List<Character> enemyTeam = GetEnemyTeam();

        Character closestEnemy = GetClosestEnemy(enemyTeam);


        Tile enemyTile = closestEnemy.GetMyPositionTile();
        int distance = 0;
        
        for (int i = 0; i < _tilesInMoveRange.Count; i++)
        {
            pathCreator.ResetPath();
            pathCreator.Calculate(_tilesInMoveRange[i], enemyTile, 1000);

            var p = pathCreator.GetPath();
            if (i == 0)
            {
                distance = p.Count;
                _targetTile = p[0];
                continue;
            }

            if (p.Count < distance)
            {
                
                distance = p.Count;
                
                _targetTile = p[0];
            }
        }
        pathCreator.ResetPath();
        _currentSteps = legs.GetMaxSteps();
        pathCreator.Calculate(_myPositionTile, _targetTile, _currentSteps);
        _path = pathCreator.GetPath();
        highlight.PathPreview(_path);
        // ResetTilesInMoveRange();
        // ResetTilesInAttackRange();
        highlight.CreatePathLines(_path);
        // if (CanAttack())
        //     PaintTilesInAttackRange(_targetTile, 0);
        //
        // PaintTilesInMoveRange(_targetTile, 0);
        // AddTilesInMoveRange();
        // highlight.PaintLastTileInPath(_targetTile);
    }

    Character GetClosestEnemy(List<Character> enemies) 
    {
        Character closestEnemy = null;
        
        List<Tile> path = new List<Tile>();
        
        for (int i = 0; i < enemies.Count; i++)
        {
            pathCreator.ResetPath();
            pathCreator.Calculate(_myPositionTile, enemies[i].GetMyPositionTile(), 1000);

            var p = pathCreator.GetPath();
            if (i == 0)
            {
                foreach (var tile in p)
                {
                    path.Add(tile);
                }
                closestEnemy = enemies[i];
                continue;
            }

            if (p.Count < path.Count)
            {
                path.Clear();
                foreach (var tile in p)
                {
                    path.Add(tile);
                }
                closestEnemy = enemies[i];
            }
        }
        _currentSteps = legs.GetMaxSteps();
        return closestEnemy;
    }

    List<Character> GetEnemyTeam()
    {
        Character[] units = turnManager.GetAllUnits();
        
        List<Character> enemyTeam = new List<Character>();

        foreach (var c in units)
        {
            if (!c.IsDead() && c.GetUnitTeam() != _unitTeam)
            {
                enemyTeam.Add(c);
            }
        }

        return enemyTeam;
    }

    IEnumerator StartMovement()
    {
        yield return new WaitForSeconds(5f);
        
        PaintTilesInMoveRange(_myPositionTile, 0);
        PaintTilesInAttackRange(_myPositionTile, 0);
        CalculateAutoMovement();
        Debug.Log("empiezo a moverme");
        Move();
    }
}
