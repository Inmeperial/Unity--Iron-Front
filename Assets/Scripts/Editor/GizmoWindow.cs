using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GizmoWindow : EditorWindow
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

    [MenuItem("Tools/Show Gizmo")]
    public static void Open()
    {
        GetWindow<GizmoWindow>().Show();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Gizmo On/Off"))
        {
            foreach (var tile in _tiles)
            {
                tile.ShowGizmo();
            }
        }
    }
}
