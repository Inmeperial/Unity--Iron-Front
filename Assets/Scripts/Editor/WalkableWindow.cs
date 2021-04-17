using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WalkableWindow : EditorWindow
{
    Tile[] _tiles;

    private void OnEnable()
    {
        maxSize = new Vector2(200, 50);
        minSize = new Vector2(200, 50);
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
        if (GUILayout.Button("Make all tiles Walkable"))
        {
            foreach (var item in _tiles)
            {
                item.MakeWalkableColor();
            }
        }
    }
}
