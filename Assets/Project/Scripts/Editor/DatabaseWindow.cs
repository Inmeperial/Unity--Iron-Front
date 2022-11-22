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

            LoadLegs();
            
            LoadGuns();
            
            LoadAbilities();
            
            LoadItems();
            
            Save();
        }
    }

    void LoadBodies()
    {
        _database.bodiesList.Clear();
            
        var bodiesPaths = AssetDatabase.FindAssets("t:BodySO", new[] {"Assets/ScriptableObjects/Parts/Bodies"});
            
        string[] bodyFiles = new string[bodiesPaths.Length];
            
        for (int i = 0; i < bodyFiles.Length; i++)
        {
            bodyFiles[i] = AssetDatabase.GUIDToAssetPath(bodiesPaths[i]);
                
            var body = (BodySO) AssetDatabase.LoadAssetAtPath(bodyFiles[i], typeof(BodySO));
                
            if (!_database.bodiesList.Contains(body))
                _database.bodiesList.Add(body);
        }
    }


    void LoadLegs()
    {
        _database.legsList.Clear();
            
        var legsPaths = AssetDatabase.FindAssets("t:LegsSO", new[] {"Assets/ScriptableObjects/Parts/Legs"});
            
        string[] legsFiles = new string[legsPaths.Length];
            
        for (int i = 0; i < legsFiles.Length; i++)
        {
            legsFiles[i] = AssetDatabase.GUIDToAssetPath(legsPaths[i]);
                
            var leg = (LegsSO) AssetDatabase.LoadAssetAtPath(legsFiles[i], typeof(LegsSO));
                
            if (!_database.legsList.Contains(leg))
                _database.legsList.Add(leg);
        }
    }

    void LoadGuns()
    {
        _database.gunsList.Clear();
            
        var gunsPaths = AssetDatabase.FindAssets("t:GunSO", new[] {"Assets/ScriptableObjects/Guns"});
            
        string[] gunFiles = new string[gunsPaths.Length];
            
        for (int i = 0; i < gunFiles.Length; i++)
        {
            gunFiles[i] = AssetDatabase.GUIDToAssetPath(gunsPaths[i]);
                
            var gun = (GunSO) AssetDatabase.LoadAssetAtPath(gunFiles[i], typeof(GunSO));
                
            if (!_database.gunsList.Contains(gun))
                _database.gunsList.Add(gun);
        }
    }

    void LoadAbilities()
    {
        _database.abilitiesList.Clear();
            
        var abilitiesPath = AssetDatabase.FindAssets("t:AbilitySO", new[] {"Assets/ScriptableObjects/Abilities"});
            
        string[] abilitiesFiles = new string[abilitiesPath.Length];
            
        for (int i = 0; i < abilitiesFiles.Length; i++)
        {
            abilitiesFiles[i] = AssetDatabase.GUIDToAssetPath(abilitiesPath[i]);
                
            var ability = (AbilitySO) AssetDatabase.LoadAssetAtPath(abilitiesFiles[i], typeof(AbilitySO));
                
            if (!_database.abilitiesList.Contains(ability))
                _database.abilitiesList.Add(ability);
        }
    }

    void LoadItems()
    {
        _database.itemsList.Clear();
            
        var itemsPath = AssetDatabase.FindAssets("t:ItemSO", new[] {"Assets/ScriptableObjects/Items"});
            
        string[] itemsFiles = new string[itemsPath.Length];
            
        for (int i = 0; i < itemsFiles.Length; i++)
        {
            itemsFiles[i] = AssetDatabase.GUIDToAssetPath(itemsPath[i]);
                
            var item = (ItemSO) AssetDatabase.LoadAssetAtPath(itemsFiles[i], typeof(ItemSO));
                
            if (!_database.itemsList.Contains(item))
                _database.itemsList.Add(item);
        }
    }

    void Save()
    {
        if (!_database) return;
        Debug.Log("saved");
        EditorUtility.SetDirty(_database);
        AssetDatabase.SaveAssets();
        EditorGUILayout.HelpBox("File saved.", MessageType.Error);
    }
}
