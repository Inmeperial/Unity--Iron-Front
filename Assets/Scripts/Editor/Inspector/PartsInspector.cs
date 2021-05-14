using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PartSO), true)]
public class PartsInspector : Editor
{
    private PartSO part;
    GUIStyle _importantStyle = new GUIStyle();
    private void OnEnable()
    {
        part = (PartSO)Selection.activeObject;
        _importantStyle.fontStyle = FontStyle.Bold;
        _importantStyle.fontSize = 15;
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
        EditorUtility.SetDirty(part);
        AssetDatabase.SaveAssets();
        EditorGUILayout.HelpBox("File saved.", MessageType.Error);
    }
}
