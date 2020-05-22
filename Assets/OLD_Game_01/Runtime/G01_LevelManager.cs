using System;
using GameConnection.Payloads;
using UnityEngine;

public class G01_LevelManager : LevelManagerBase<ElementPayload, ElementPayload>
{
    [SerializeField] private GameObject _fireGamePrefab;
    [SerializeField] private GameObject _windGamePrefab;
    
    public static G01_LevelManager Instance { get; private set; }
    private Element _gameElement;

    private void Awake()
    {
        Instance = this;
    }

    protected override void InitInternal(ElementPayload inPayload)
    {
        // Map to either wind or fire
        _gameElement = GetGameElement(inPayload);
        
        Debug.Log($"G01 - Init. Element received: {inPayload}. But will treat as (mapped to): {_gameElement}");
        switch (_gameElement)
        {
            case Element.Fire:
                StartFireGame();
                break;
            case Element.Wind:
                StartWindGame();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // Scripts within game should call this thru singleton
    public void EndGame(Element outputElement)
    {
        var payload = new ElementPayload(outputElement);
        Exit(payload);
    }

    private void StartWindGame()
    {
        Debug.Log("Starting Town Game [Wind]");
        // Start Icy Wind version of Game
        Instantiate(_windGamePrefab);
    }

    private void StartFireGame()
    {
        Debug.Log("Starting Town Game [Fire]");
        // Start burning village version of game
        Instantiate(_fireGamePrefab);
    }

    // Only want to support two games here for now (fire, wind).
    // That means The "Chosen" element not necessarily the "input" element - 
    private Element GetGameElement(ElementPayload payload)
    {
        var inpElement = payload.Element;
        switch (inpElement)
        {
            case Element.Water:
            case Element.Wind:
                return Element.Wind;
            
            case Element.Earth:
            case Element.Fire:
                return Element.Fire;
            default:
                throw new ArgumentOutOfRangeException(nameof(inpElement), inpElement, null);
        }
    }
}