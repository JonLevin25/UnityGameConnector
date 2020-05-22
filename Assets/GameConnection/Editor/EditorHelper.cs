using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameConnection.Editor
{
    public static class EditorHelper
    {
        /// <summary>
        /// If text file not found, create it + return true. else return false
        /// </summary>
        public static bool CreateTextFileIfMissing(string assetPath, string content)
        {
            var file = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);
            if (file != null) return false;

            File.WriteAllText(assetPath, content);
            return true;
        }

        public static bool CreateDirIfMissing(string assetPath)
        {
            var dirAlreadyExists = AssetDatabase.IsValidFolder(assetPath);
            if (dirAlreadyExists) return false;

            Directory.CreateDirectory(assetPath);
            return true;
        }

        public static void CreateAndSaveAsset(string assetPath, Object asset)
        {
            var dir = Path.GetDirectoryName(assetPath);

            Directory.CreateDirectory(dir);
            AssetDatabase.CreateAsset(asset, assetPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void SetArrayObjectRefs<T>(SerializedProperty arrayProp, IReadOnlyList<T> values, Action<SerializedProperty, T> setter)
        {
            if (!arrayProp.isArray)
            {
                Debug.LogError("SerializedProp was not an array!");
                return;
            }

            SetArraySize(arrayProp, values.Count);
            var i = 0;
            foreach (var value in values)
            {
                var arrayElement = arrayProp.GetArrayElementAtIndex(i);
                setter(arrayElement, value);
                i++;
            }
        }

        public static void SetArraySize(SerializedProperty arrayProp, int length)
        {
            if (!arrayProp.isArray)
            {
                Debug.LogError("SerializedProp was not an array!");
                return;
            }

            var deltaSize = length - arrayProp.arraySize;
            if (deltaSize > 0)
            {
                for (var i = arrayProp.arraySize; i < length; i++)
                {
                    arrayProp.InsertArrayElementAtIndex(i);
                }
            }
            else if (deltaSize < 0)
            {
                for (var i = length; i < arrayProp.arraySize; i++)
                {
                    arrayProp.DeleteArrayElementAtIndex(i);
                }
            }
        }
        
        // https://stackoverflow.com/questions/457676/check-if-a-class-is-derived-from-a-generic-class
        public static bool IsSubclassOfRawGeneric(this Type myType, Type generic) {
            while (myType != null && myType != typeof(object)) {
                if (myType.IsGenericType && myType.GetGenericTypeDefinition() == generic)
                    return true;
                
                myType = myType.BaseType;
            }
            return false;
        }

        /// <summary>
        /// Find
        /// </summary>
        /// <param name="myType"></param>
        /// <param name="rawGeneric">The </param>
        public static Type GetGenericBaseType(this Type myType, Type rawGeneric)
        {
            while (myType != null && myType != typeof(object))
            {
                if (myType.IsGenericType && myType.GetGenericTypeDefinition() == rawGeneric) 
                    return myType;

                myType = myType.BaseType;
            }

            return null;
        }
    }
}