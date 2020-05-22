using System;
using System.Collections.Generic;
using System.Linq;
using GameConnection;
using GameConnection.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManifestTool
{
    private const string DialogTitle = "GameManifestTool";

    private static List<Scene> _scenesToClose;
    private static List<Scene> ScenesToClose
    {
        get => _scenesToClose ?? (_scenesToClose = new List<Scene>());
        set => _scenesToClose = value;
    }

    [MenuItem(EditorConsts.MenuItemsPrefix + "Generate Editor Manifest", priority = 1)]
    public static void GenerateManifest()
    {
        var confirm = EditorUtility.DisplayDialog(DialogTitle, "This will change all build scenes except the first. Continue?", "OK", "Cancel");
        if (!confirm) return;

        ScenesToClose = null;
        try
        {
            var games = FindGamesInProject().ToArray();

            var manifest = GetOrCreateManifest();
            SetManifestGames(manifest, games);
        }
        finally
        {
            CloseScenes(_scenesToClose);
        }
    }

    private static GameManifest GetOrCreateManifest()
    {
        var manifest = Resources.Load<GameManifest>(Consts.ManifestResourcePath);
        if (manifest) return manifest;

        manifest = ScriptableObject.CreateInstance<GameManifest>();
        Undo.RegisterCreatedObjectUndo(manifest, "Created game manifest");
        EditorHelper.CreateAndSaveAsset(EditorConsts.ManifestAssetPath, manifest);

        return manifest;
    }

    private static void SetManifestGames(GameManifest manifest, ConnectedGame[] games)
    {
        var serializedManifest = new SerializedObject(manifest);
        
        var connectedGamesProp = serializedManifest.FindProperty("connectedGames");
        Undo.RegisterCompleteObjectUndo(manifest, "Set Connected Games");
        
        EditorHelper.SetArrayObjectRefs(connectedGamesProp, games, 
            (prop, game) => game.CopyTo(prop));
        
        serializedManifest.ApplyModifiedProperties();
        
        EditorUtility.SetDirty(manifest);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    private static IEnumerable<ConnectedGame> FindGamesInProject()
    {
        var scenesWithLevelManagers = FindScenesWithLevelManagers();
        var games = scenesWithLevelManagers
            .Select(gameTup =>
            {
                var (scene, levelManager) = gameTup;
                var (inputType, outputType) = GetPayloadTypes(levelManager);
                return new ConnectedGame(inputType, outputType, scene);
            });

        return games;
    }

    private static IEnumerable<(string scenePath, LevelManagerBase)> FindScenesWithLevelManagers()
    {
        var scenePaths = AssetDatabase
            .FindAssets("t:Scene", new[] {"Assets"})
            .Select(AssetDatabase.GUIDToAssetPath);


        var filtered = 
            scenePaths.Select(scenePath =>
                {
                    var rootGOs = GetRootGameObjects(scenePath);
                    var results = rootGOs.Select(root =>
                    { 
                        var levelManager = root.GetComponent<LevelManagerBase>(); // For simplicity and time - allow only root-level managers
                        return (scenePath, levelManager);
                    })
                    .Where(x => x.levelManager != null)
                    .ToArray();

                if (results.Length == 0) return (scenePath, null);
                if (results.Length == 1) return results[0];

                throw new IndexOutOfRangeException(
                    $"Scene at path {scenePath} had more than one LevelManager! this is not allowed");
            })
            .Where(x => x.levelManager != null);

        return filtered;
    }

    private static (Type inputType, Type outputType) GetPayloadTypes(LevelManagerBase levelManager)
    {
        var lvlManagerType = levelManager.GetType();
        
        // will return specified generic type e.g. typeof(LevelManager<Element, End>)
        var lvlManagerGenericType = lvlManagerType.GetGenericBaseType(typeof(LevelManagerBase<,>)); 
        if (lvlManagerGenericType != null)
        {
            // Find types (e.g. LevelManager<ElementPayload, EndPayload>) 
            var genericArgs = lvlManagerGenericType.GenericTypeArguments;
            var inpType = genericArgs[0];
            var outType = genericArgs[1];

            return (inpType, outType);
        }
        else
        {
            throw new Exception($"LevelManager {levelManager.name} did not derive from generic LevelManagerBase! Actual type: {lvlManagerType.Name}");
        }
    }

    private static GameObject[] GetRootGameObjects(string scenePath)
    {
        var scene = EditorSceneManager.GetSceneByPath(scenePath);
        var wasLoaded = scene.isLoaded;
        if (!wasLoaded)
        {
            scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            ScenesToClose.Add(scene);
        }

        var rootGOs = scene.GetRootGameObjects();
        return rootGOs;
    }

    private static void CloseScenes(IEnumerable<Scene> scenesToClose)
    {
        if (scenesToClose == null) return;
        foreach (var scene in scenesToClose)
        {
            EditorSceneManager.CloseScene(scene, removeScene: true);
        }
    }
}
