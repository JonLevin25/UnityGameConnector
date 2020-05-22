using System;
using System.Linq;
using GameConnection;
using GameConnection.Payloads;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Simplest implementation of Connecting games - loads the "manifest" from Resources and connects linearly
/// </summary>
public class TestGameConnectionManager : MonoBehaviour
{
    [SerializeField] private bool initOnStart;
    private GameManifest _manifest;
    private int _gameIdx = -1;

    private void Start()
    {
        if (initOnStart) Init();
    }
    
    public void Init()
    {
        LoadManifest();
        LoadNextGame(new StartPayload());
    }

    private void LoadNextGame(ScenePayloadBase payload)
    {
        _gameIdx++;
        if (_manifest.Games.Length <= _gameIdx)
        {
            Debug.LogError("No more games found! Ending game series");
            EndSeries();
        }
        
        InitCurrGame(payload);
    }
    
    private void InitCurrGame(ScenePayloadBase payload)
    {
        Debug.Log($"Initializing Game");
        
        var buildIdx = _manifest.GetGameSceneBuildIdx(_gameIdx);
        SceneManager.LoadScene(buildIdx);
        
        var sceneRootGOs = SceneManager
            .GetActiveScene()
            .GetRootGameObjects();
        
        // Assume single levelManager in scene, without a parent
        var levelManager = sceneRootGOs
            .Select(go => go.GetComponent<LevelManagerBase>())
            .Single();

        levelManager.OnEnd += OnLevelEnd;
        levelManager.Init(payload);
    }

    private void OnLevelEnd(object sender, ScenePayloadBase payload)
    {
        var levelManager = (LevelManagerBase) sender;
        Debug.Log($"Level ended! ({levelManager.name})");
        levelManager.OnEnd -= OnLevelEnd;

        if (payload is EndPayload)
        {
            EndSeries();
        }
        else
        {
            LoadNextGame(payload);
        }
    }
    
    private void EndSeries()
    {
        Debug.Log("Ending Game Series!");
        var endIdx = _manifest.EndSceneBuildIdx;
        SceneManager.LoadScene(endIdx);
    }

    private void LoadManifest()
    {
        _manifest = Resources.Load<GameManifest>(Consts.ManifestResourcePath);
        if (_manifest == null)
        {
            throw new Exception("no manifest found in resources! did you generate one from the menu?");
        }
    }
}
