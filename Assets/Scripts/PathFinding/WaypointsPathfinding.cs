using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsPathfinding : MonoBehaviour, IPathCreator
{
    [SerializeField] private AStarAgent _agent;
    private List<Tile> _fullPath = new List<Tile>();
    private Stack<List<Tile>> _partialPaths = new Stack<List<Tile>>();
    private Character _char;
    void Start()
    {
        _agent = FindObjectOfType<AStarAgent>();
        _char = GetComponent<Character>();
    }

    public void Calculate(Character character, Tile end)
    {
        //If list is empty, pathfinding starts with the tile of player position.
        if (_fullPath == null || _fullPath.Count == 0)
        {
            _fullPath = new List<Tile>();
            _agent.init = character.GetTileBelow();
        }
        //If list is not empty, pathfinding starts with last tile of the list.
        else if (_fullPath.Count > 1)
        {
            _agent.init = _fullPath[_fullPath.Count - 1];
            Debug.Log("pos: " + _fullPath[_fullPath.Count - 1].transform.position);
        }
        
        _agent.finit = end;
        var temp = _agent.PathFindingAstar();

        temp.RemoveAt(0);
        if (temp.Count <= _char.GetSteps())
        {
            _fullPath.AddRange(temp);
            _char.ReduceAvailableSteps(temp.Count);
            _partialPaths.Push(temp);
        }


        //for (int i = 0; i < temp.Count; i++)
        //{
        //    _fullPath.Add(temp[i]);
        //}
        
    }

    public List<Tile> GetPath()
    {
        return _fullPath;
    }

    public int GetDistance()
    {
        return _fullPath.Count;
    }
    public void UndoLastWaypoint()
    {
        if (_partialPaths.Count > 0)
        {
            var removed = _partialPaths.Pop();
            foreach (var tile in removed)
            {
                tile.EndPathfindingPreviewColor();
                _char.IncreaseAvailableSteps(1);
            }
            var tempStack = new Stack<List<Tile>>();

            foreach (var partialList in _partialPaths)
            {
                tempStack.Push(partialList);
            }
            _fullPath.Clear();
            foreach (var item in tempStack)
            {
                _fullPath.AddRange(item);
            }
            _char.ClearTargetTile();
        }
    }

    public void Reset()
    {
        _fullPath.Clear();
        _partialPaths.Clear();
    }
}
