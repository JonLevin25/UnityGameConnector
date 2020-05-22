using System;
using UnityEngine;

namespace GameConnection
{
    [Serializable] // TODO: do I actually need to serialize this?
    public class Editor_ConnectedGame
    {
        [SerializeField] private Type inputPayloadType;
        [SerializeField] private Type outputPayloadType;
        [SerializeField] private string scenePath;

        public Type InputPayloadType => inputPayloadType;
        public Type OutputPayloadType => outputPayloadType;
        public string ScenePath => scenePath;

        public Editor_ConnectedGame(Type inputPayloadType, Type outputPayloadType, string scenePath)
        {
            if (!Helper.ValidatePayloadTypes(inputPayloadType, outputPayloadType)) 
                throw new Exception($"Failed to create {GetType().Name}");
            
            this.inputPayloadType = inputPayloadType;
            this.outputPayloadType = outputPayloadType;
            this.scenePath = scenePath;
        }
    }
}