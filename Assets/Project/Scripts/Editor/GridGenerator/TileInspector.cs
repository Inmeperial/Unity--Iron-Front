using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tile))]
public class TileInspector : Editor
{
    Tile _tile;
    private void OnEnable()
    {
        _tile = Selection.activeGameObject.GetComponent<Tile>();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Make Walkable"))
            _tile.MakeWalkableColor();

        if (GUILayout.Button("Make Not Walkable"))
            _tile.MakeNotWalkableColor();
    }
}
