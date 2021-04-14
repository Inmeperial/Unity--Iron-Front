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
        }
        
        _agent.finit = end;
        var temp = _agent.PathFindingAstar();

        temp.RemoveAt(0);
        if (temp.Count <= _char.GetSteps())
        {
            foreach (var item in temp)
            {
                _fullPath.Add(item);
            }
            _char.ReduceAvailableSteps(temp.Count);
            _partialPaths.Push(temp);
        }        
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
        //Prevents path from breaking during movement.
        if (_char.IsMoving() == false)
        {
            //Check if there is a path.
            if (_partialPaths.Count > 0)
            {
                //Get the partial path and return tiles to their normal color.
                var removed = _partialPaths.Pop();
                foreach (var tile in removed)
                {
                    tile.EndPathfindingPreviewColor();
                    _char.IncreaseAvailableSteps(1);
                }


                var tempStack = new Stack<List<Tile>>();

                //Invert the stack so tiles are added in the correct order.
                foreach (var partialList in _partialPaths)
                {
                    tempStack.Push(partialList);
                }

                _fullPath.Clear();

                //Recreate the path.
                foreach (var item in tempStack)
                {
                    _fullPath.AddRange(item);
                }
                _char.ClearTargetTile();

                Debug.Log("path: " + _fullPath.Count);

                if (_fullPath == null || _fullPath.Count == 0)
                    _char.DeactivateMoveButton();
            }
        }
    }

    public void Reset()
    {
        _fullPath.Clear();
        _partialPaths.Clear();
    }
}
