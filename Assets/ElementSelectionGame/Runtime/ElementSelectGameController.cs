using GameConnection.Payloads;
using UnityEngine;

public class ElementSelectGameController : MiniGameControllerBase<StartPayload, ElementPayload>
{
    protected override void InitInternal(StartPayload inPayload)
    {
        Debug.Log($"Started {nameof(ElementSelectGameController)}");
    }

    // Scripts within game should call this thru singleton
    public void EndGame(Element outputElement)
    {
        var payload = new ElementPayload(outputElement);
        EndGame(payload);
    }
}