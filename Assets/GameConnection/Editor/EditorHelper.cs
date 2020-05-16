using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common.Editor
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

        public static void SetArrayMangedRefs<T>(SerializedProperty arrayProp, IReadOnlyList<T> values)
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
                arrayProp.GetArrayElementAtIndex(i).managedReferenceValue = value;
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
    }
}