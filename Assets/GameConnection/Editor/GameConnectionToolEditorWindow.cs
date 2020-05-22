using GameConnection;
using GameConnection.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameConnectionToolEditorWindow : EditorWindow
{
    private bool _manifestFound;
    private SceneAsset _startScene;
    private SceneAsset _endScene;
    
    // Keys for loading from EditorPrefs
    private const string EditorPrefsPrefix = "GAMECONNECTION_EDITORTOOL_WINDOW_";
    private const string StartScenePathKey = EditorPrefsPrefix + "startScene";
    private const string EndScenePathKey = EditorPrefsPrefix + "endScene";
    
    [MenuItem(EditorConsts.MenuItemsPrefix + "Tool Window", priority = 1)]
    public static void ShowWindow()
    {
        GetWindow<GameConnectionToolEditorWindow>("Game Connection Tool");
    }

    private void OnEnable()
    {
        UpdateIsManifestFound();
        LoadMissingData();
    }

    private void OnGUI()
    {
        var updateManifestButtonLabel = _manifestFound ? "Update Game Manifest" : "Find Games";
        if (GUILayout.Button(updateManifestButtonLabel))
        {
            GameManifestTool.GenerateManifest();
            UpdateIsManifestFound();
        }
        
        using (new EditorGUI.DisabledScope(!_manifestFound))
        {
            EditorGUI.BeginChangeCheck();
            _startScene = (SceneAsset) EditorGUILayout.ObjectField("Start Scene", _startScene, typeof(SceneAsset), false);
            _endScene = (SceneAsset) EditorGUILayout.ObjectField("End Scene", _endScene, typeof(SceneAsset), false);
            if (EditorGUI.EndChangeCheck())
            {
                Debug.Log("Changed!");
                SaveData();
            }

            if (GUILayout.Button("Set Build Indices"))
            {
                var manifest = LoadManifest();
                GameManifestTool.SetBuildIndices(manifest, _startScene, _endScene);
            }
        }
    }

    private void LoadMissingData()
    {
        if (!_startScene) _startScene = LoadScenePath(StartScenePathKey);
        if (!_endScene) _endScene = LoadScenePath(EndScenePathKey);
    }

    private void SaveData()
    {
        SaveScenePath(StartScenePathKey, _startScene);
        SaveScenePath(EndScenePathKey, _endScene);
    }

    private static void SaveScenePath(string scenePathKey, SceneAsset scene)
    {
        var path = AssetDatabase.GetAssetPath(scene);
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Failed to find path for scene asset!");
            return;
        }
        
        EditorPrefs.SetString(scenePathKey, path);
    }
    private static SceneAsset LoadScenePath(string scenePathKey)
    {
        var savedScenePath = EditorPrefs.GetString(scenePathKey);
        if (string.IsNullOrEmpty(savedScenePath)) return null;
        
        return AssetDatabase.LoadAssetAtPath<SceneAsset>(savedScenePath);
    }

    private void UpdateIsManifestFound()
    {
        var manifest = LoadManifest();
        _manifestFound = manifest != null;
    }

    private static GameManifest LoadManifest()
    {
        var manifest = AssetDatabase.LoadAssetAtPath<GameManifest>(EditorConsts.ManifestAssetPath);
        return manifest;
    }
}
