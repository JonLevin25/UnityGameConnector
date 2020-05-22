using System;
using NaughtyAttributes;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameConnection
{
    [Serializable]
    public class ConnectedGame
    {
        [ReadOnly, SerializeField] private string scenePath;
        [ReadOnly, SerializeField] private string inputPayloadTypeName;
        [ReadOnly, SerializeField] private string outputPayloadTypeName;

        // Types Not in use currently - but good to have this info
        // as we might want to connect games via payload type (dynamically) in the future
        public string ScenePath => scenePath;
        public string InputPayloadTypeName => inputPayloadTypeName;
        public string OutputPayloadTypeName => outputPayloadTypeName;

#if UNITY_EDITOR 
        public void CopyTo(SerializedProperty prop)
        {
            var inputTypeNameProp = prop.FindPropertyRelative(nameof(inputPayloadTypeName));
            var outputTypeNameProp = prop.FindPropertyRelative(nameof(outputPayloadTypeName));
            var scenePathProp = prop.FindPropertyRelative(nameof(scenePath));

            inputTypeNameProp.stringValue = inputPayloadTypeName;
            outputTypeNameProp.stringValue = outputPayloadTypeName;
            scenePathProp.stringValue = scenePath;
        }
#endif
        
        public ConnectedGame(Type inputPayloadTypeName, Type outputPayloadTypeName, string scenePath)
        {
            if (scenePath ==null) Debug.LogError("ScenePath cannot be null!");
            if (!Helper.ValidatePayloadTypes(inputPayloadTypeName, outputPayloadTypeName)) return;
            
            this.inputPayloadTypeName = inputPayloadTypeName.FullName;
            this.outputPayloadTypeName = outputPayloadTypeName.FullName;
            this.scenePath = scenePath;
        }
    }
}