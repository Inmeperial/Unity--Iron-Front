using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneSelectorWindow : EditorWindow
{
    private Vector2 _scrollPosition;
    private bool _useMultiScene;

    private string _sceneToSearch = "";

    [MenuItem("Custom Editor/Scene Selector List")]
    private static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SceneSelectorWindow), false, "Scene Selector");
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label("Multiple");
        _useMultiScene = EditorGUILayout.Toggle("", _useMultiScene);

        GUILayout.EndHorizontal();

        DrawSearchBar();
        EditorGUILayout.Space(3);

        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, false, GUILayout.Width(this.position.width), GUILayout.MinHeight(1), GUILayout.MaxHeight(1000), GUILayout.ExpandHeight(true));

        DrawAllScenes();

        GUILayout.EndScrollView();
    }

    private void DrawSearchBar()
    {
        GUILayout.BeginHorizontal();
        _sceneToSearch = GUILayout.TextField(_sceneToSearch);
        GUILayout.EndHorizontal();

        if (_sceneToSearch != "")
        {
            GUILayout.BeginVertical("Box");

            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                string sceneName = GetSceneName(EditorBuildSettings.scenes[i].path);
                sceneName = sceneName.ToLower();
                if (!sceneName.Contains(_sceneToSearch.ToLower())) continue;

                GUILayout.Space(5);

                if (GUILayout.Button(GetSceneName(EditorBuildSettings.scenes[i].path)))
                {
                    if (_useMultiScene)
                        EditorSceneManager.OpenScene(EditorBuildSettings.scenes[i].path, OpenSceneMode.Additive);
                    else
                    {
                        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                        EditorSceneManager.OpenScene(EditorBuildSettings.scenes[i].path);
                    }
                }
            }

            GUILayout.EndVertical();

            HorizontalRect(2);
        }
    }

    private void HorizontalRect(float height)
    {
        Rect rect = EditorGUILayout.GetControlRect(false, 10);

        rect.height = height;

        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }

    private void DrawAllScenes()
    {
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            GUILayout.Space(5);

            if (GUILayout.Button(GetSceneName(EditorBuildSettings.scenes[i].path)))
            {
                if (_useMultiScene)
                    EditorSceneManager.OpenScene(EditorBuildSettings.scenes[i].path, OpenSceneMode.Additive);
                else
                {
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    EditorSceneManager.OpenScene(EditorBuildSettings.scenes[i].path);
                }
            }
        }
    }

    private string GetSceneName(string _path)
    {
        char[] path = _path.ToCharArray();
        string sceneName = "";

        for (int i = path.Length - 1; i >= 0; i--)
        {
            if (path[i] == '/')
                break;
            sceneName += path[i];
        }

        path = sceneName.ToCharArray();
        sceneName = "";

        for (int i = path.Length - 1; i >= 0; i--)
        {
            if (path[i] == '.')
                break;
            sceneName += path[i];
        }

        return sceneName;
    }
}
