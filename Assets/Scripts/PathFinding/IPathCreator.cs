using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathCreator
{
    void Calculate(Character character, Tile end);
    List<Tile> GetPath();

    void Reset();
}
