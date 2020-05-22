using System.Linq;
using GameConnection;
using GameConnection.Editor;
using UnityEditor;
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
    
    [MenuItem(EditorConsts.MenuItemsPrefix + "Config Tool", priority = 1)]
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
        var updateManifestButtonLabel = _manifestFound ? "Regenerate Game Manifest" : "Find Games";
        GUILayout.Label("Game Manifest", EditorStyles.boldLabel);
        if (GUILayout.Button(updateManifestButtonLabel))
        {
            GameManifestTool.GenerateManifest();
            UpdateIsManifestFound();
        }
        
        using (new EditorGUI.DisabledScope(!_manifestFound))
        {
            if (GUILayout.Button("Show Manifest"))
            {
                var manifest = LoadManifest();
                EditorGUIUtility.PingObject(manifest);
                Selection.activeObject = manifest; 
            }
            
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            
            GUILayout.Label("Scene Config", EditorStyles.boldLabel);
            if (GUILayout.Button("Set Build Indices"))
            {
                var manifest = LoadManifest();
                GameManifestTool.SetBuildIndices(manifest, _startScene, _endScene);
            }
            
            EditorGUI.BeginChangeCheck();
            _startScene = (SceneAsset) EditorGUILayout.ObjectField("Start Scene", _startScene, typeof(SceneAsset), false);
            _endScene = (SceneAsset) EditorGUILayout.ObjectField("End Scene", _endScene, typeof(SceneAsset), false);
            if (EditorGUI.EndChangeCheck())
            {
                Debug.Log("Changed!");
                SaveData();
            }
        }
    }
    
    private void LoadMissingData()
    {
        if (!_startScene) _startScene = LoadSceneAsset(StartScenePathKey, EditorConsts.DefaultStartSceneName);
        if (!_endScene) _endScene = LoadSceneAsset(EndScenePathKey, EditorConsts.DefaultEndSceneName);
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
    private static SceneAsset LoadSceneAsset(string scenePathKey, string fallbackSceneName = null)
    {
        var savedScenePath = EditorPrefs.GetString(scenePathKey);
        if (string.IsNullOrEmpty(savedScenePath))
        {
            var foundScenes = AssetDatabase
                .FindAssets($"{fallbackSceneName} t: Scene")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<SceneAsset>);

            var scene = foundScenes.FirstOrDefault();
            if (scene != null) SaveScenePath(scenePathKey, scene);
            return scene;
        }
        
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
