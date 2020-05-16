using System;
using Common.Scripts.PayloadTypes;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    [Serializable]
    public class Runtime_ConnectedGame
    {
        [SerializeField] private Type inputPayloadType;
        [SerializeField] private Type outputPayloadType;
        [SerializeField] private int sceneBuildIdx;

        // Types Not in use currently - but good to have this info
        // as we might want to connect games via payload type (dynamically) in the future
        public Type InputPayloadType => inputPayloadType;
        public Type OutputPayloadType => outputPayloadType;
        public int SceneBuildIdx => sceneBuildIdx;

        public Runtime_ConnectedGame(Type inputPayloadType, Type outputPayloadType, int sceneBuildIdx)
        {
            if (!Helper.ValidatePayloadTypes(inputPayloadType, outputPayloadType)) return;
            
            this.inputPayloadType = inputPayloadType;
            this.outputPayloadType = outputPayloadType;
            this.sceneBuildIdx = sceneBuildIdx;
        }
    }
    
    /// <summary>
    /// A 
    /// </summary>
    public class GameManifest : ScriptableObject
    {
        [ReorderableList]
        [SerializeField] private Runtime_ConnectedGame[] connectedGames;
        
        // TODO: Order type validation

        public Runtime_ConnectedGame[] Games => connectedGames;
    }
}