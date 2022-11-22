using System.Collections.Generic;

public interface IPathCreator
{
    void Calculate(Tile start, Tile end, int distance);
    List<Tile> GetPath();

    int GetDistance();

    void UndoLastWaypoint();
    void ResetPath();

    //List<Tile> Calculate(Tile start, Tile end);
}
