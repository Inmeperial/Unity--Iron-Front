using UnityEngine;
using UnityEditor;

public class GetNeighboursWindow : EditorWindow
{
    Tile[] _tiles;
    private void OnEnable()
    {
        this.maxSize = new Vector2(300, 130);
        this.minSize = new Vector2(300, 130);
        var t = FindObjectsOfType<Tile>();
        _tiles = new Tile[t.Length];
        _tiles = t;
    }
    [MenuItem("Tools/Get Tiles Neighbours")]
    public static void Open()
    {
        GetWindow<GetNeighboursWindow>().Show();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Calculate walls and floor Tiles"))
        {
            foreach (var tile in _tiles)
            {
                tile.CheckIfTileAbove();
                if (tile.HasTileAbove())
                    tile.MakeNotWalkableColor();
                EditorUtility.SetDirty(tile);
            }
        }

        if (GUILayout.Button("Get Walkable and Attackable Neighbours"))
        {
            foreach (var tile in _tiles)
            {
                
                if (!tile.HasTileAbove())
                {
                    tile.neighboursForMove.Clear();
                    tile.allNeighbours.Clear();
                    tile.GetNeighbours();
                }
                EditorUtility.SetDirty(tile);
            }
        }
    }
}
