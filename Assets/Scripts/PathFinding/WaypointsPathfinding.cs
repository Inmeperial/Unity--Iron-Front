using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaypointsPathfinding : MonoBehaviour, IPathCreator
{
    [SerializeField] private AStarAgent _agent;
    [SerializeField] private List<Tile> _fullMovePath = new List<Tile>();
    private Stack<List<Tile>> _partialPaths = new Stack<List<Tile>>();
    private Character _char;

    public void Start()
    {
        _agent = FindObjectOfType<AStarAgent>();
        _char = GetComponent<Character>();
    }

    public void Calculate(Tile start, Tile end, int distance)
    {
        if (_fullMovePath == null || _fullMovePath.Count == 0)
        {
            _fullMovePath = new List<Tile>();
            _agent.init = start;
        }
        //If list is not empty, pathfinding starts with last tile of the list.
        else if (_fullMovePath.Count > 1)
        {
            _agent.init = _fullMovePath[_fullMovePath.Count - 1];
        }
        
        _agent.finit = end;
        var temp = _agent.PathFindingAstar();

        if (_fullMovePath.Count > 0)
        {
            temp.RemoveAt(0);
            if (temp.Count <= distance)
            {
                foreach (var item in temp)
                {
                    _fullMovePath.Add(item);
                }
                _char.ReduceAvailableSteps(temp.Count);
            }
        }
        else
        {
            if (temp.Count-1 <= distance)
            {
                foreach (var item in temp)
                {
                    _fullMovePath.Add(item);
                }
                _char.ReduceAvailableSteps(temp.Count-1);
            }
        }
        _partialPaths.Push(temp);
    }

    public List<Tile> GetPath()
    {
        return _fullMovePath;
    }

    public int GetDistance()
    {
        return _fullMovePath.Count-1;
    }

    public void UndoLastWaypoint()
    {
        //Prevents path from breaking during movement.
        if (_char.IsMoving()) return;
        
        //Check if there is a path.
        if (_partialPaths.Count <= 0) return;
        
        //Get the partial path and return tiles to their normal color.
        var removed = _partialPaths.Pop();
        foreach (var tile in removed)
        {
            if (tile != _char.GetMyPositionTile())
                _char.IncreaseAvailableSteps(1);
        }
        _char.highlight.ClearTilesInMoveRange(removed);


        var tempStack = new Stack<List<Tile>>();

        //Invert the stack so tiles are added in the correct order.
        foreach (var partialList in _partialPaths)
        {
            tempStack.Push(partialList);
        }

        _fullMovePath.Clear();

        //Recreate the path.
        foreach (var item in tempStack)
        {
            _fullMovePath.AddRange(item);
        }
        if (_fullMovePath == null || _fullMovePath.Count == 0)
        {
            _char.ClearTargetTile();
        }
        
        _char.Undo();
    }

    public void ResetPath()
    {
        _fullMovePath.Clear();
        _partialPaths.Clear();
    }
}
