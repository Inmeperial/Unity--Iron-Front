using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class DatabaseWindow : EditorWindow
{
    private ObjectsDatabase _database;
    private void OnEnable()
    {
        maxSize = new Vector2(300, 130);
        minSize = new Vector2(300, 130);
    }

    [MenuItem("Tools/Database")]
    public static void Open()
    {
        GetWindow<DatabaseWindow>().Show();
    }

    private void OnGUI()
    {
        _database = (ObjectsDatabase)EditorGUILayout.ObjectField("Database", _database, typeof(ObjectsDatabase), true);

        if (GUILayout.Button("Populate Database"))
        {
            LoadBodies();
            
            LoadArms();
            
            LoadLegs();
            
            LoadGuns();
        }
    }

    void LoadBodies()
    {
        _database.bodies.Clear();
            
        var bodiesPaths = AssetDatabase.FindAssets("t:BodySO", new[] {"Assets/ScriptableObjects/Parts/Bodies"});
            
        string[] bodyFiles = new string[bodiesPaths.Length];
            
        for (int i = 0; i < bodyFiles.Length; i++)
        {
            bodyFiles[i] = AssetDatabase.GUIDToAssetPath(bodiesPaths[i]);
                
            var body = (BodySO) AssetDatabase.LoadAssetAtPath(bodyFiles[i], typeof(BodySO));
                
            if (!_database.bodies.Contains(body))
                _database.bodies.Add(body);
        }
    }

    void LoadArms()
    {
        _database.arms.Clear();
            
        var armsPaths = AssetDatabase.FindAssets("t:ArmSO", new[] {"Assets/ScriptableObjects/Parts/Arms"});
            
        string[] armsFiles = new string[armsPaths.Length];
            
        for (int i = 0; i < armsFiles.Length; i++)
        {
            armsFiles[i] = AssetDatabase.GUIDToAssetPath(armsPaths[i]);
                
            var arm = (ArmSO) AssetDatabase.LoadAssetAtPath(armsFiles[i], typeof(ArmSO));
                
            if (!_database.arms.Contains(arm))
                _database.arms.Add(arm);
        }
    }

    void LoadLegs()
    {
        _database.legs.Clear();
            
        var legsPaths = AssetDatabase.FindAssets("t:LegsSO", new[] {"Assets/ScriptableObjects/Parts/Legs"});
            
        string[] legsFiles = new string[legsPaths.Length];
            
        for (int i = 0; i < legsFiles.Length; i++)
        {
            legsFiles[i] = AssetDatabase.GUIDToAssetPath(legsPaths[i]);
                
            var leg = (LegsSO) AssetDatabase.LoadAssetAtPath(legsFiles[i], typeof(LegsSO));
                
            if (!_database.legs.Contains(leg))
                _database.legs.Add(leg);
        }
    }

    void LoadGuns()
    {
        _database.guns.Clear();
            
        var gunsPaths = AssetDatabase.FindAssets("t:GunSO", new[] {"Assets/ScriptableObjects/Guns"});
            
        string[] gunFiles = new string[gunsPaths.Length];
            
        for (int i = 0; i < gunFiles.Length; i++)
        {
            gunFiles[i] = AssetDatabase.GUIDToAssetPath(gunsPaths[i]);
                
            var gun = (GunSO) AssetDatabase.LoadAssetAtPath(gunFiles[i], typeof(GunSO));
                
            if (!_database.guns.Contains(gun))
                _database.guns.Add(gun);
        }
    }
}
