using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortPathfind : MonoBehaviour, IPathCreator
{
    [SerializeField] private AStarAgent _agent;
    private List<Tile> _path = new List<Tile>();

    void Start()
    {
        _agent = FindObjectOfType<AStarAgent>();
    }

    public void Calculate(Tile start, Tile end, int distance)
    {
        _agent.init = start;
        _agent.finit = end;
        _path = _agent.PathFindingAstar();
    }

    public List<Tile> GetPath()
    {
        return _path;
    }

    public void ResetPath()
    {
        _path.Clear();
    }

    public void UndoLastWaypoint()
    {
    }

    public int GetDistance()
    {
        return 0;
    }
}
