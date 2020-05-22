using System;
using NaughtyAttributes;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameConnection
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

#if UNITY_EDITOR 
        public void CopyTo(SerializedProperty prop)
        {
            var inputTypeNameProp = prop.FindPropertyRelative(nameof(inputPayloadTypeName));
            var outputTypeNameProp = prop.FindPropertyRelative(nameof(outputPayloadTypeName));
            var sceneIdxTypeNameProp = prop.FindPropertyRelative(nameof(sceneBuildIdx));

            inputTypeNameProp.stringValue = inputPayloadTypeName;
            outputTypeNameProp.stringValue = outputPayloadTypeName;
            sceneIdxTypeNameProp.intValue = sceneBuildIdx;
        }
#endif
        
        public Runtime_ConnectedGame(Type inputPayloadTypeName, Type outputPayloadTypeName, int sceneBuildIdx)
        {
            if (!Helper.ValidatePayloadTypes(inputPayloadTypeName, outputPayloadTypeName)) return;
            
            this.inputPayloadTypeName = inputPayloadTypeName.FullName;
            this.outputPayloadTypeName = outputPayloadTypeName.FullName;
            this.sceneBuildIdx = sceneBuildIdx;
        }
    }
}