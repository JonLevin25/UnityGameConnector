using GameConnection.Payloads;
using UnityEngine;

public class StartGameLevelManager : LevelManagerBase<StartPayload, ElementPayload>
{
    protected override void InitInternal(StartPayload inPayload)
    {
        Debug.Log($"Started {nameof(StartGameLevelManager)}");
    }

    // Scripts within game should call this thru singleton
    public void EndGame(Element outputElement)
    {
        var payload = new ElementPayload(outputElement);
        Exit(payload);
    }
}