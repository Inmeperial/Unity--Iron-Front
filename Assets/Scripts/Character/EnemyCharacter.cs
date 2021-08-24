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
        Character[] units = turnManager.GetAllUnits();

        List<Character> greenTeam = new List<Character>();

        foreach (var c in units)
        {
            if (!c.IsDead() && c.GetUnitTeam() != _unitTeam)
            {
                greenTeam.Add(c);
            }
        }

        Character closestEnemy = null;
        
        List<Tile> path = new List<Tile>();
        
        for (int i = 0; i < greenTeam.Count; i++)
        {
            pathCreator.ResetPath();
            pathCreator.Calculate(_myPositionTile, greenTeam[i].GetMyPositionTile(), 1000);

            var p = pathCreator.GetPath();
            if (i == 0)
            {
                foreach (var tile in p)
                {
                    path.Add(tile);
                }
                closestEnemy = greenTeam[i];
                continue;
            }

            if (p.Count < path.Count)
            {
                path.Clear();
                foreach (var tile in p)
                {
                    path.Add(tile);
                }
                closestEnemy = greenTeam[i];
            }
        }


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
