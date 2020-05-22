using System;
using System.Collections.Generic;
using System.Linq;
using GameConnection;
using GameConnection.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public static class GameManifestTool
{
    private const string DialogTitle = "GameManifestTool";

    private static List<Scene> _scenesToClose;
    private static List<Scene> ScenesToClose
    {
        get => _scenesToClose ?? (_scenesToClose = new List<Scene>());
        set => _scenesToClose = value;
    }

    // [MenuItem(EditorConsts.MenuItemsPrefix + "Generate Editor Manifest")]
    public static void GenerateManifest()
    {
        var userConfirm =
            ConfirmationPopup("This will regenerate the manifest, resetting the game order. Continue?");
        if (!userConfirm) return;

        ScenesToClose = null;
        try
        {
            var games = FindGamesInProject().ToArray();

            var manifest = GetOrCreateManifest();
            SetManifestGames(manifest, games);
            EditorGUIUtility.PingObject(manifest);
            Selection.activeObject = manifest;
        }
        finally
        {
            CloseScenes(_scenesToClose);
        }
    }

    public static void SetBuildIndices(GameManifest manifest, SceneAsset startScene, SceneAsset endScene)
    {
        if (manifest == null) throw new ArgumentNullException($"Cannot {nameof(SetBuildIndices)} with null manifest!");
        
        var userConfirm =
            ConfirmationPopup("This will set build scene indices, removing all current scenes. Continue?");
        if (!userConfirm) return;

        const int startIdx = 0;
        const int firstGameIdx = 1;
        
        var lastGameIdx = firstGameIdx + manifest.Games.Length - 1;
        var endIdx = lastGameIdx + 1;
        
        SetManifestScenes(manifest, startIdx, firstGameIdx, endIdx);
        SetBuildScenes(manifest, startScene, endScene);
    }

    private static void SetBuildScenes(GameManifest manifest, SceneAsset startScene, SceneAsset endScene)
    {
        var gameBuildScenes = manifest.Games.Select(
            (game, gameIdx) => new EditorBuildSettingsScene(game.ScenePath, true));

        var startScenePath = AssetDatabase.GetAssetPath(startScene);
        var endScenePath = AssetDatabase.GetAssetPath(endScene);
        
        var startBuildScene = new EditorBuildSettingsScene(startScenePath, true);
        var endBuildScene = new EditorBuildSettingsScene(endScenePath, true);

        // Allow startScene,firstGameScene not to be sequential. Redundant for now
        var startBufferScenes = Enumerable.Repeat(
            new EditorBuildSettingsScene(),
            manifest.FirstGameIdx - manifest.StartSceneIdx - 1
        );
        
        // Allow last game scene + endScene not to be sequential. Redundant for now
        var endBufferScenes = Enumerable.Repeat(
            new EditorBuildSettingsScene(),
            manifest.EndSceneIdx - manifest.LastGameIdx - 1
        );
        
        var allBuildIndices = startBufferScenes
            .Concat(gameBuildScenes)
            .Concat(endBufferScenes)
            .Prepend(startBuildScene)
            .Append(endBuildScene)
            .ToArray();

        EditorBuildSettings.scenes = allBuildIndices;
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

        EditorHelper.UpdateAndSaveAsset(
            manifest,
            serializedManifest =>
            {
                var connectedGamesProp = serializedManifest.FindProperty("connectedGames");
                EditorHelper.SetArrayObjectRefs(connectedGamesProp, games, 
                    (prop, game) => game.CopyTo(prop));
            }, 
            "Set GameManifest Connected Games"
        );
    }

    private static void SetManifestScenes(GameManifest manifest, int startSceneIdx, int firstGameSceneIdx, int endSceneIdx)
    {
        EditorHelper.UpdateAndSaveAsset(
            manifest,
            serializedManifest =>
            {
                serializedManifest.FindProperty("startSceneIdx").intValue = startSceneIdx;
                serializedManifest.FindProperty("firstGameIdx").intValue = firstGameSceneIdx;
                serializedManifest.FindProperty("endSceneIdx").intValue = endSceneIdx;
            },
            "Set GameManifest Scene Indices"
        );
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

    private static bool ConfirmationPopup(string message, string ok = "OK", string cancel = "Cancel")
    {
        var confirm = EditorUtility.DisplayDialog(DialogTitle, message, ok, cancel);
        return confirm;
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
