using System;
using System.Security.Cryptography;
using System.Text;
using TosCore.MasterData;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TosCore.Editor.MasterData
{
    public static class AddressableMasterDataEditorUtility
    {
        public static bool TryAssignId(Object target, bool force, bool logIfMissing = true)
        {
            if (target == null) return false;
            if (!IsAddressableMasterDataType(target.GetType())) return false;

            var uniqueKey = ResolveAddressableGuid(target);
            if (string.IsNullOrEmpty(uniqueKey))
            {
                if (logIfMissing)
                {
                    Debug.LogWarning($"[{target.name}] Addressables entry not found. Ensure the asset is added to Addressables before generating an ID.", target);
                }

                return false;
            }

            var generated = GenerateStableId(uniqueKey);
            var serialized = new SerializedObject(target);
            var idProperty = serialized.FindProperty("_id");
            var valueProperty = idProperty?.FindPropertyRelative("_id");
            if (valueProperty == null)
            {
                if (logIfMissing)
                {
                    Debug.LogWarning($"[{target.name}] MasterId field '_id' not found.", target);
                }

                return false;
            }

            if (!force && valueProperty.intValue == generated)
            {
                return true;
            }

            valueProperty.intValue = generated;
            serialized.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
            return true;
        }

        public static void ProcessAssetsAtPath(string assetPath, bool force, bool logIfMissing = true)
        {
            if (string.IsNullOrEmpty(assetPath)) return;

            var assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            foreach (var asset in assets)
            {
                TryAssignId(asset, force, logIfMissing);
            }
        }

        public static void ProcessEntry(AddressableAssetEntry entry, bool force, bool logIfMissing = true)
        {
            if (entry == null) return;

            var path = AssetDatabase.GUIDToAssetPath(entry.guid);
            if (string.IsNullOrEmpty(path)) return;

            ProcessAssetsAtPath(path, force, logIfMissing);
        }

        public static bool IsAddressableMasterDataType(Type type)
        {
            while (type != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(AddressableMasterData<>))
                {
                    return true;
                }

                type = type.BaseType;
            }

            return false;
        }

        private static int GenerateStableId(string key)
        {
            using var md5 = MD5.Create();
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            var value = BitConverter.ToUInt32(bytes, 0);
            if (value == 0)
            {
                value = BitConverter.ToUInt32(bytes, 4);
                if (value == 0)
                {
                    value = BitConverter.ToUInt32(bytes, 8);
                    if (value == 0)
                    {
                        value = BitConverter.ToUInt32(bytes, 12);
                    }
                }
            }

            var candidate = (int)(value % int.MaxValue);
            return candidate == 0 ? int.MaxValue : candidate;
        }

        private static string ResolveAddressableGuid(UnityEngine.Object target)
        {
            var path = AssetDatabase.GetAssetPath(target);
            if (string.IsNullOrEmpty(path)) return string.Empty;

            var assetGuid = AssetDatabase.AssetPathToGUID(path);
            if (string.IsNullOrEmpty(assetGuid)) return string.Empty;

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var entry = settings?.FindAssetEntry(assetGuid);
            if (entry == null)
            {
                return string.Empty;
            }

            return entry.guid;
        }
    }
}
