using Game_02.Scripts;
using GameConnection.Payloads;
using UnityEngine;
using UnityEngine.Serialization;

public class G02_LevelManager : LevelManagerBase<ElementPayload, EndPayload>
{
    [FormerlySerializedAs("_config")] [SerializeField] private G02_Config config;
    [FormerlySerializedAs("_playerSpawn")] [SerializeField] private Transform playerSpawn;
    [SerializeField] private EnemyManager enemyManager;

    private Element _inpElement;
    private ElementAssetsHolder _assets;
    private G02_CharController _activeChar;
    
    public static G02_LevelManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    
    protected override void InitInternal(ElementPayload inPayload)
    {
        Debug.Log($"Game02: Init(element: {inPayload})");
        _inpElement = inPayload.Element;
        
        var elementToAssetsMap = config.MapElementAssets();
        _assets = elementToAssetsMap[_inpElement];

        InstantiatePlayer();
        enemyManager.Init(_assets.EnemyPrefab);
    }

    private void InstantiatePlayer()
    {
        Debug.Log($"Instantiating player prefab!");
        var prefab = _assets.CharPrefab;

        _activeChar = Instantiate(prefab, playerSpawn.position, playerSpawn.rotation);
        _activeChar.Init(_assets.ProjectilePrefab);
    }

    // Called by charController thru singleton instance
    public void EndGame()
    {
        Exit(new EndPayload());
    }
}