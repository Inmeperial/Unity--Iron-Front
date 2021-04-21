using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(GunSO))]

public class GunInspector : Editor
{
    private GunSO gun;
    private void OnEnable()
    {
        gun = (GunSO)Selection.activeObject;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        bool save = GUILayout.Button("Save Preset");
        EditorGUILayout.LabelField("Save preset changes and write them on disk.");
        if (save)
            SavePreset();
    }
    void SavePreset()
    {
        EditorUtility.SetDirty(gun);
        AssetDatabase.SaveAssets();
        EditorGUILayout.HelpBox("File saved.", MessageType.Error);
    }
}
