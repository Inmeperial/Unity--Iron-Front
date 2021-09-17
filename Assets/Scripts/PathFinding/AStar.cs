using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar//<Tile>
{
    public delegate bool Satisfies(Tile curr);
    public delegate Dictionary<Tile, float> GetNeighbours(Tile curr);
    public delegate float GetCost(Tile father, Tile child);
    public delegate float Heuristic(Tile current);
    public List<Tile> Run(Tile start, Satisfies satisfies, GetNeighbours getNeighbours, Heuristic heuristic, int watchDog = 1000)
    {
        Dictionary<Tile, float> cost = new Dictionary<Tile, float>();
        Dictionary<Tile, Tile> parents = new Dictionary<Tile, Tile>();
        PriorityQueue<Tile> pending = new PriorityQueue<Tile>();
        HashSet<Tile> visited = new HashSet<Tile>();
        pending.Enqueue(start, 0);
        cost.Add(start, 0);
        int count = 0;
        while (!pending.IsEmpty)
        {
            
            Tile current = pending.Dequeue();
            if (!current.IsFree())
                continue;
            watchDog--;
            if (watchDog <= 0)
            {
                return new List<Tile>();
            }
            if (satisfies(current))
            {
                return ConstructPath(current, parents);
            }
            visited.Add(current);
            Dictionary<Tile, float> neighbours = getNeighbours(current);
            foreach (var item in neighbours)
            {
                Tile node = item.Key;
                if (visited.Contains(node)) continue;
                float nodeCost = item.Value;
                float totalCost = cost[current] + nodeCost;
                if (cost.ContainsKey(node) && cost[node] < totalCost) continue;
                cost[node] = totalCost;
                parents[node] = current;
                pending.Enqueue(node, totalCost + heuristic(node));
            }
        }
        return new List<Tile>();
    }

    List<Tile> ConstructPath(Tile end, Dictionary<Tile, Tile> parents)
    {
        var path = new List<Tile>();
        path.Add(end);
        while (parents.ContainsKey(path[path.Count - 1]))
        {
            var lastNode = path[path.Count - 1];
            path.Add(parents[lastNode]);
        }
        path.Reverse();
        return path;
    }
}
