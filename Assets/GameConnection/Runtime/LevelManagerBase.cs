using System;
using Common.Scripts.PayloadTypes;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class LevelManagerBase<TInput, TOutput> : LevelManagerBase 
    where TInput : ScenePayloadBase
    where TOutput : ScenePayloadBase
{
    public override void Init(ScenePayloadBase payload)
    {
        var castedPayload = (TInput) payload;
        InitInternal(castedPayload);
    }
    protected abstract void InitInternal(TInput inPayload);

    protected void Exit(TOutput outPayload) => InvokeEndEvent(outPayload);
}

/// <summary>
/// Dont inherit from this - inherit from generic version
/// </summary>
public abstract class LevelManagerBase : MonoBehaviour
{
    public event EventHandler<ScenePayloadBase> OnEnd;
    public abstract void Init(ScenePayloadBase payload);

    protected void InvokeEndEvent(ScenePayloadBase payload) => OnEnd?.Invoke(this, payload);
}
