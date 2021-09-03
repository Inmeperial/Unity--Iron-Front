using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar<T>
{
    public delegate bool Satisfies(T curr);
    public delegate Dictionary<T, float> GetNeighbours(T curr);
    public delegate float GetCost(T father, T child);
    public delegate float Heuristic(T current);
    public List<T> Run(T start, Satisfies satisfies, GetNeighbours getNeighbours, Heuristic heuristic, int watchDog = 1000)
    {
        Dictionary<T, float> cost = new Dictionary<T, float>();
        Dictionary<T, T> parents = new Dictionary<T, T>();
        PriorityQueue<T> pending = new PriorityQueue<T>();
        HashSet<T> visited = new HashSet<T>();
        pending.Enqueue(start, 0);
        cost.Add(start, 0);
        int count = 0;
        while (!pending.IsEmpty)
        {
            T current = pending.Dequeue();
            watchDog--;
            if (watchDog <= 0)
            {
                return new List<T>();
            }
            if (satisfies(current))
            {
                return ConstructPath(current, parents);
            }
            visited.Add(current);
            Dictionary<T, float> neighbours = getNeighbours(current);
            foreach (var item in neighbours)
            {
                T node = item.Key;
                if (visited.Contains(node)) continue;
                float nodeCost = item.Value;
                float totalCost = cost[current] + nodeCost;
                if (cost.ContainsKey(node) && cost[node] < totalCost) continue;
                cost[node] = totalCost;
                parents[node] = current;
                pending.Enqueue(node, totalCost + heuristic(node));
            }
        }
        return new List<T>();
    }

    List<T> ConstructPath(T end, Dictionary<T, T> parents)
    {
        var path = new List<T>();
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
