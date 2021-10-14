using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScriptableObject), true)]
public class SOBaseInspector : Editor
{
    protected ScriptableObject _object; 
    GUIStyle _importantStyle = new GUIStyle();
    private void OnEnable()
    {
        _object = (ScriptableObject)Selection.activeObject;
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
        if (!_object) return;
        Debug.Log("saved");
        EditorUtility.SetDirty(_object);
        AssetDatabase.SaveAssets();
        EditorGUILayout.HelpBox("File saved.", MessageType.Error);
    }
}
