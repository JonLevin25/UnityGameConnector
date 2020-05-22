using System;
using System.Collections;
using System.Linq;
using GameConnection;
using GameConnection.Payloads;
using GameConnection.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Simplest implementation of Connecting games - loads the "manifest" from Resources and connects linearly
/// </summary>
public class TestGameConnectionManager : MonoBehaviour
{
    [SerializeField] private ConnectionDebugOverlay debugOverlay;
    [SerializeField] private GameCompletedMenu gameCompletedMenu;
    [SerializeField] private EndMenu endMenu;

    private ScenePayloadBase _currPayload;
    private ScenePayloadBase _nextPayload; // Has value only after level finished and before user clicked 'next'/'restart' game
    private GameManifest _manifest;
    private int _gameIdx = -1;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }
    
    public void Init()
    {
        gameCompletedMenu.Init(OnRestartGameClicked, OnNextGameClicked);
        endMenu.Init(OnRestartSequenceClicked);
        LoadManifest();
        LoadNextGame(new StartPayload());
    }

    private void OnRestartGameClicked() => RestartCurrGame();

    private void OnRestartSequenceClicked()
    {
        _gameIdx = 0;
        _nextPayload = new StartPayload();
        StartCoroutine(InitGame(_gameIdx, _nextPayload));
    }

    private void OnNextGameClicked()
    {
        if (_nextPayload is EndPayload)
        {
            StartCoroutine(EndSeries());
        }
        else
        {
            LoadNextGame(_nextPayload);
        }
    }

    private void RestartCurrGame()
    {
        Debug.Log("Restarting curr game");
        StartCoroutine(InitGame(_gameIdx, _currPayload));
    }

    private void LoadNextGame(ScenePayloadBase payload)
    {
        _currPayload = payload;
        _gameIdx++;
        if (_manifest.Games.Length <= _gameIdx)
        {
            Debug.LogError("No more games found! Ending game series");
            StartCoroutine(EndSeries());
            return;
        }
        
        StartCoroutine(InitGame(_gameIdx, payload));
    }
    
    private IEnumerator InitGame(int gameIdx, ScenePayloadBase payload)
    {
        Debug.Log($"Initializing Game");

        _nextPayload = null;
        HideMenus();
        debugOverlay.SetGame(gameIdx, payload.ToString());
        
        var buildIdx = _manifest.GetGameSceneBuildIdx(gameIdx);
        
        SceneManager.LoadScene(buildIdx);
        yield return null; // Wait frame for scene to load
        
        var sceneRootGOs = SceneManager
            .GetActiveScene()
            .GetRootGameObjects();
        
        // Assume single levelManager in scene, without a parent
        var levelManager = sceneRootGOs
            .Select(go => go.GetComponent<LevelManagerBase>())
            .Single(x => x != null);

        levelManager.OnEnd += OnLevelEnd;
        levelManager.Init(payload);
    }

    private void OnLevelEnd(object sender, ScenePayloadBase payload)
    {
        var levelManager = (LevelManagerBase) sender;
        Debug.Log($"Level ended! ({levelManager.name})");
        levelManager.OnEnd -= OnLevelEnd;
        _nextPayload = payload;
        gameCompletedMenu.gameObject.SetActive(true);
    }
    
    private IEnumerator EndSeries()
    {
        Debug.Log("Ending Game Series!");
        ShowMenu(endMenu.gameObject);
        debugOverlay.SetEndScene();
        
        var endIdx = _manifest.EndSceneIdx;
        
        SceneManager.LoadScene(endIdx);
        yield return null; // Wait frame for scene to load
        
        var endController = FindObjectOfType<EndSceneController>();
        endController.Init(_manifest);
    }

    private void LoadManifest()
    {
        _manifest = Resources.Load<GameManifest>(Consts.ManifestResourcePath);
        if (_manifest == null)
        {
            throw new Exception("no manifest found in resources! did you generate one from the menu?");
        }
    }

    private void ShowMenu(GameObject menu)
    {
        HideMenus();
        menu.SetActive(true);
    }

    private void HideMenus()
    {
        endMenu.gameObject.SetActive(false);
        gameCompletedMenu.gameObject.SetActive(false);
    }
}
