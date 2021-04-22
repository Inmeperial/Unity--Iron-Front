using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(GunSO))]

public class GunInspector : Editor
{
    private GunSO gun;
    GUIStyle _importantStyle = new GUIStyle();
    private void OnEnable()
    {
        gun = (GunSO)Selection.activeObject;
        _importantStyle.fontStyle = FontStyle.Bold;
        _importantStyle.fontSize = 15;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        GUILayout.BeginVertical();

        EditorGUILayout.LabelField("If Gun Type is Rifle", _importantStyle);
        EditorGUILayout.LabelField("Bullets Per Click has to", _importantStyle);
        EditorGUILayout.LabelField("be the same as Max Bullets.", _importantStyle);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Chance to hit other parts", _importantStyle);
        EditorGUILayout.LabelField("ONLY for Shotguns.", _importantStyle);

        GUILayout.EndVertical();

        EditorGUILayout.Space();

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
