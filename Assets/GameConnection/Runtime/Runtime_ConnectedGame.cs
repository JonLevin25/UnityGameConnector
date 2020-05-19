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
        [ReadOnly, SerializeField] private string inputPayloadTypeName;
        [ReadOnly, SerializeField] private string outputPayloadTypeName;
        [ReadOnly, SerializeField] private int sceneBuildIdx;

        // Types Not in use currently - but good to have this info
        // as we might want to connect games via payload type (dynamically) in the future
        public string InputPayloadTypeName => inputPayloadTypeName;
        public string OutputPayloadTypeName => outputPayloadTypeName;
        public int SceneBuildIdx => sceneBuildIdx;

        public Runtime_ConnectedGame(Type inputPayloadTypeName, Type outputPayloadTypeName, int sceneBuildIdx)
        {
            if (!Helper.ValidatePayloadTypes(inputPayloadTypeName, outputPayloadTypeName)) return;
            
            this.inputPayloadTypeName = inputPayloadTypeName.FullName;
            this.outputPayloadTypeName = outputPayloadTypeName.FullName;
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