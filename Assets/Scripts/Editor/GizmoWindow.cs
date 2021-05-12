using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GizmoWindow : EditorWindow
{
    private void OnEnable()
    {
        maxSize = new Vector2(200, 100);
        minSize = new Vector2(200, 100);
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
            var tiles = FindObjectsOfType<Tile>();
            foreach (var tile in tiles)
            {
                tile.ShowGizmo();
                EditorUtility.SetDirty(tile);
            }
            AssetDatabase.SaveAssets();
        }

        if (GUILayout.Button("Tiles Shader On"))
        {
            var tiles = FindObjectsOfType<TileMaterialhandler>();
            foreach (var tile in tiles)
            {
                tile.GetChilds();
                tile.DiseableAndEnableStatus(true);
                tile.DiseableAndEnableSelectedNode(true);
                EditorUtility.SetDirty(tile);
            }
            AssetDatabase.SaveAssets();
        }
        
        if (GUILayout.Button("Tiles Shader Off"))
        {
            var tiles = FindObjectsOfType<TileMaterialhandler>();
            foreach (var tile in tiles)
            {
                tile.GetChilds();
                tile.DiseableAndEnableStatus(false);
                tile.DiseableAndEnableSelectedNode(false);
                EditorUtility.SetDirty(tile);
            }
            AssetDatabase.SaveAssets();
        }
    }
}
