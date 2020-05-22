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
        [ReadOnly, SerializeField] private string inputPayloadTypeName;
        [ReadOnly, SerializeField] private string outputPayloadTypeName;
        [ReadOnly, SerializeField] private string scenePath;
        // [ReadOnly, SerializeField] private int sceneBuildIdx;

        // Types Not in use currently - but good to have this info
        // as we might want to connect games via payload type (dynamically) in the future
        public string InputPayloadTypeName => inputPayloadTypeName;
        public string OutputPayloadTypeName => outputPayloadTypeName;
        public string ScenePath => scenePath;
        
        // public int SceneBuildIdx => sceneBuildIdx;

#if UNITY_EDITOR 
        public void CopyTo(SerializedProperty prop)
        {
            var inputTypeNameProp = prop.FindPropertyRelative(nameof(inputPayloadTypeName));
            var outputTypeNameProp = prop.FindPropertyRelative(nameof(outputPayloadTypeName));
            var scenePathProp = prop.FindPropertyRelative(nameof(scenePath));
            // var sceneIdxTypeNameProp = prop.FindPropertyRelative(nameof(sceneBuildIdx));

            inputTypeNameProp.stringValue = inputPayloadTypeName;
            outputTypeNameProp.stringValue = outputPayloadTypeName;
            scenePathProp.stringValue = scenePath;
            // sceneIdxTypeNameProp.intValue = sceneBuildIdx;
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