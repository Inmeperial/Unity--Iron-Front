﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridGenerator : EditorWindow
{
    public GameObject tiles;
    GUIStyle _importantStyle = new GUIStyle();


    int _width;
    int _length;
    Transform _container;
    bool _error;
    private void OnEnable()
    {
        _error = false;
        this.maxSize = new Vector2 (300,130);
        this.minSize = new Vector2(300, 130);
        _importantStyle.fontStyle = FontStyle.Bold;

        var folder = AssetDatabase.FindAssets("GridBlock");

        if (folder.Length < 1)
        {
            Debug.LogError("No encontrado el prefab del bloque");
        }
        else
        {
            string[] files = new string[folder.Length];
            for (int i = 0; i < folder.Length; i++)
            {
                files[i] = AssetDatabase.GUIDToAssetPath(folder[i]);
            }

            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(files[0], typeof(GameObject));

            tiles = prefab;
        }
    }

    [MenuItem("Tools/Grid Generator")]
    public static void Open()
    {
        GetWindow<GridGenerator>().Show();
    }
    private void OnGUI()
    {
        _width = EditorGUILayout.IntField("Width", _width);
        _length = EditorGUILayout.IntField("Length", _length);
        _container = (Transform)EditorGUILayout.ObjectField("Container", _container, typeof(Transform), true);

        if (GUILayout.Button("Create Grid"))
        {
            if (_width > 0 && _length > 0)
            {
                _error = false;
                CreateGrid(_width, _length, _container);
            }
            else _error = true;
        }

        if (_error)
            ShowError();
    }

    void CreateGrid(int width, int length, Transform container)
    {
        Vector3 pos = Vector3.zero;

        for (int i = 0; i < width; i++)
        {
            pos.x = i;
            for (int j = 0; j < length; j++)
            {
                pos.z = j;
                GameObject obj;
                if (container)
                {
                    //obj = Instantiate(tiles, pos, container.transform.rotation, container);
                    obj = PrefabUtility.InstantiatePrefab(tiles, container) as GameObject;
                }
                else
                {
                    obj = PrefabUtility.InstantiatePrefab(tiles) as GameObject;
                }
                obj.transform.localPosition = pos;
                obj.GetComponent<Tile>().MakeWalkable();
            }
        }
    }

    void ShowError()
    {
        EditorGUILayout.HelpBox("WIDTH o LENGTH inválidos. \nDeben ser mayor a 0.", MessageType.Error);
    }
}
