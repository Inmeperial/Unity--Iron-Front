using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BBUnity.Actions;
using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("AI Actions/Move")]
[Help("Enemy AI will move towards the closest enemy depending on its move range.")]
public class MoveAction : GOAction
{
    [InParam("myUnit")]
    private EnemyCharacter _myUnit;

    public override void OnStart()
    {
        
        _myUnit = gameObject.GetComponent<EnemyCharacter>();
        
        if (_myUnit)
            Debug.Log("si encontre el enemy character en el start");
    }

    public override TaskStatus OnUpdate()
    {
        if (!_myUnit)
        {
            _myUnit = gameObject.GetComponent<EnemyCharacter>();
            if (!_myUnit)
            {
                Debug.Log("no encontre al enemy character");
    
                return TaskStatus.FAILED;
            }
        }
        
        Debug.Log("si encontre el enemy character");
        _myUnit.CalculateAutoMovement();
        _myUnit.EnemyMove();
        return TaskStatus.COMPLETED;
    
    }
    
    // void CalculateAutoMovement()
    // {
    //     List<Character> enemyTeam = GetEnemyTeam();
    //
    //     Character closestEnemy = GetClosestEnemy(enemyTeam);
    //
    //
    //     Tile enemyTile = closestEnemy.GetMyPositionTile();
    //     Tile closestTileToEnemy = null;
    //     
    //     int distance = 0;
    //     
    //     //Search for the closest tile to enemy position.
    //     for (int i = 0; i < enemyTile.neighboursForMove.Count; i++)
    //     {
    //         pathCreator.ResetPath();
    //         pathCreator.Calculate(_tilesInMoveRange[i], enemyTile.neighboursForMove[i], 1000);
    //         
    //         List<Tile> p = pathCreator.GetPath();
    //         if (i == 0)
    //         {
    //             distance = p.Count;
    //             closestTileToEnemy = p[p.Count-1];
    //             continue;
    //         }
    //
    //         if (p.Count >= distance) continue;
    //         
    //         distance = p.Count;
    //             
    //         closestTileToEnemy = p[p.Count-1];
    //     }
    //     
    //     //Finds the farthest tile I can reach in my movement range.
    //     for (int i = 0; i < _tilesInMoveRange.Count; i++)
    //     {
    //         pathCreator.ResetPath();
    //         pathCreator.Calculate(_tilesInMoveRange[i], closestTileToEnemy, 1000);
    //
    //         List<Tile> p = pathCreator.GetPath();
    //         if (i == 0)
    //         {
    //             distance = p.Count;
    //             _targetTile = p[0];
    //             continue;
    //         }
    //
    //         if (p.Count >= distance) continue;
    //         
    //         distance = p.Count;
    //
    //         _targetTile = p[0];
    //     }
    //     
    //     //Clears previous calculated paths
    //     pathCreator.ResetPath();
    //     
    //     _currentSteps = legs.GetMaxSteps();
    //     
    //     //Calculates shortest path.
    //     pathCreator.Calculate(_myPositionTile, _targetTile, _currentSteps);
    //     _path = pathCreator.GetPath();
    //     
    //     highlight.PathPreview(_path);
    //     highlight.CreatePathLines(_path);
    // }
    //
    // Character GetClosestEnemy(List<Character> enemies) 
    // {
    //     Character closestEnemy = null;
    //     
    //     List<Tile> path = new List<Tile>();
    //     
    //     for (int i = 0; i < enemies.Count; i++)
    //     {
    //         pathCreator.ResetPath();
    //         pathCreator.Calculate(_myPositionTile, enemies[i].GetMyPositionTile(), 1000);
    //
    //         List<Tile> p = pathCreator.GetPath();
    //         if (i == 0)
    //         {
    //             foreach (var tile in p)
    //             {
    //                 path.Add(tile);
    //             }
    //             closestEnemy = enemies[i];
    //             continue;
    //         }
    //
    //         if (p.Count >= path.Count) continue;
    //         path.Clear();
    //         foreach (var tile in p)
    //         {
    //             path.Add(tile);
    //         }
    //         closestEnemy = enemies[i];
    //     }
    //     _currentSteps = legs.GetMaxSteps();
    //     return closestEnemy;
    // }
    
    
}
