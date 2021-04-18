using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AStarAgent : MonoBehaviour
{
    public Tile init;
    public Tile finit;
    AStar<Tile> _aStar = new AStar<Tile>();
    public List<Tile> PathFindingAstar()
    {
        return _aStar.Run(init, Satisfies, GetNeighboursCost, Heuristic);
    }
    float Heuristic(Tile curr)
    { 
        return Vector3.Distance(curr.transform.position, finit.transform.position);
    }
    Dictionary<Tile, float> GetNeighboursCost(Tile curr)
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
    bool Satisfies(Tile curr)
    {
        return curr == finit;
    }
}
