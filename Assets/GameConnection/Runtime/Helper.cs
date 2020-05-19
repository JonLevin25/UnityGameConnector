using System;
using UnityEngine;
using Common.Scripts.PayloadTypes;

namespace Common
{
    public static class Helper
    {
        public static bool ValidatePayloadTypes(Type inpType, Type outType)
        {
            if (!IsScenePayloadType(inpType) || !IsScenePayloadType(outType))
            {
                Debug.LogError($"input or output payloads were not ScenePayloads! Input Type: {inpType.Name}. Output Type: {outType.Name}");
                return false;
            }

            return true;
        }

        public static bool ValidatePayloadType(Type t)
        {
            if (!IsScenePayloadType(t))
            {
                Debug.LogError($"type was not ScenePayload! Type Received: {t.Name}");
                return false;
            }

            return true;
        }
        
        public static bool IsScenePayloadType(Type t) => typeof(ScenePayloadBase).IsAssignableFrom(t);
    }
}