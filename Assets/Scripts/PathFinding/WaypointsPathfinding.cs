using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsPathfinding : MonoBehaviour, IPathCreator
{
    [SerializeField] private AStarAgent _agent;
    private List<Tile> _path = new List<Tile>();

    void Start()
    {
        _agent = FindObjectOfType<AStarAgent>();
    }

    public void Calculate(Character character, Tile end)
    {
        if (_path == null || _path.Count == 0)
        {
            _path = new List<Tile>();
            _agent.init = character.GetTileBelow();
        }
        else if (_path.Count > 1)
        {
            _agent.init = _path[_path.Count - 1];
            Debug.Log("pos: " + _path[_path.Count - 1].transform.position);
        }
        
        _agent.finit = end;
        var temp = _agent.PathFindingAstar();
        for (int i = 0; i < temp.Count; i++)
        {
            _path.Add(temp[i]);
        }
    }

    public List<Tile> GetPath()
    {
        return _path;
    }

    public void Reset()
    {
        _path.Clear();
    }
}
