using System.Collections.Generic;
using UnityEngine;

public class WaypointsPathfinding : MonoBehaviour
{
    private AStarAgent _agent;
    private List<Tile> _fullMovePath = new List<Tile>();
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
            _fullMovePath = new List<Tile>();
        //If list is not empty, pathfinding starts with last tile of the list.
        else if (_fullMovePath.Count > 1)
            start = _fullMovePath[_fullMovePath.Count - 1];

        _agent.SetStartAndFinish(start, end);

        List<Tile> temp = _agent.PathFindingAstar();

        if (temp.Count <= 0)
            return;

        if (_fullMovePath.Count > 0)
        {
            temp.RemoveAt(0);
            if (temp.Count <= distance)
            {
                foreach (Tile tile in temp)
                    _fullMovePath.Add(tile);

                _char.ReduceAvailableSteps(temp.Count);
            }
        }
        else
        {
            if (temp.Count-1 <= distance)
            {
                foreach (Tile tile in temp)
                    _fullMovePath.Add(tile);

                _char.ReduceAvailableSteps(temp.Count-1);
            }
        }
        _partialPaths.Push(temp);
    }

    public List<Tile> GetPath() => _fullMovePath;

    public int GetDistance() => _fullMovePath.Count - 1;

    public void UndoLastWaypoint()
    {
        //Prevents path from breaking during movement.
        if (_char.IsMoving())
            return;
        
        //Check if there is a path.
        if (_partialPaths.Count <= 0)
            return;

        //Get the partial path and return tiles to their normal color.
        List<Tile> removed = _partialPaths.Pop();

        foreach (Tile tile in removed)
        {
            if (tile != _char.GetPositionTile())
                _char.IncreaseAvailableSteps(1);
        }

        TileHighlight.Instance.ClearTilesInMoveRange(removed);


        Stack<List<Tile>> tempStack = new Stack<List<Tile>>();

        //Invert the stack so tiles are added in the correct order.
        foreach (List<Tile> partialList in _partialPaths)
            tempStack.Push(partialList);

        _fullMovePath.Clear();

        //Recreate the path.
        foreach (List<Tile> tilesList in tempStack)
            _fullMovePath.AddRange(tilesList);

        if (_fullMovePath == null || _fullMovePath.Count == 0)
            _char.ClearTargetTile();
        
        _char.Undo();
    }

    public void ResetPath()
    {
        _fullMovePath.Clear();
        _partialPaths.Clear();
    }
}
