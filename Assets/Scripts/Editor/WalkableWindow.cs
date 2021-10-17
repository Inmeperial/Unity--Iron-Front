using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WalkableWindow : EditorWindow
{
    Tile[] _tiles;

    private void OnEnable()
    {
        maxSize = new Vector2(300, 130);
        minSize = new Vector2(300, 130);
        var tiles = FindObjectsOfType<Tile>();
        _tiles = new Tile[tiles.Length];
        _tiles = tiles;
    }

    [MenuItem("Tools/Walkable Tiles")]
    public static void Open()
    {
        GetWindow<WalkableWindow>().Show();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Make all tiles Walkable and clear neighbours"))
        {
            foreach (var item in _tiles)
            {
                item.allNeighbours.Clear();
                item.neighboursForMove.Clear();
                item.MakeWalkableColor();
            }
        }
    }
}
