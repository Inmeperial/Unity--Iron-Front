using System.Collections.Generic;
using UnityEngine;
public class AStarAgent : MonoBehaviour
{
    private Tile _initialTile;
    private Tile _finishTile;
    private AStar _aStar = new AStar();

    public List<Tile> PathFindingAstar() => _aStar.Run(_initialTile, Satisfies, GetNeighboursCost, Heuristic);

    private float Heuristic(Tile curr) => Vector3.Distance(curr.transform.position, _finishTile.transform.position);

    private Dictionary<Tile, float> GetNeighboursCost(Tile curr)
    {
        Dictionary<Tile, float> dic = new Dictionary<Tile, float>();
        for (int i = 0; i < curr.neighboursForMove.Count; i++)
        {
            
            float cost = 0;
            cost += Vector3.Distance(curr.transform.position, curr.neighboursForMove[i].transform.position);
            dic[curr.neighboursForMove[i]] = cost;
        }
        return dic;
    }

    private bool Satisfies(Tile curr) => curr == _finishTile;

    public void SetStartAndFinish(Tile start, Tile finish)
    {
        _initialTile = start;
        _finishTile = finish;
    }
}
