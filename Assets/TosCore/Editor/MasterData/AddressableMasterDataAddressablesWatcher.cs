using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;

namespace TosCore.Editor.MasterData
{
    /// <summary>
    /// Addressableに変更を見てMasterDataのIDを更新するクラス
    /// </summary>
    [InitializeOnLoad]
    public static class AddressableMasterDataAddressablesWatcher
    {
        static AddressableMasterDataAddressablesWatcher()
        {
            AddressableAssetSettings.OnModificationGlobal += OnAddressablesModified;
        }

        private static void OnAddressablesModified(AddressableAssetSettings settings, AddressableAssetSettings.ModificationEvent evt, object data)
        {
            switch (evt)
            {
                case AddressableAssetSettings.ModificationEvent.EntryAdded:
                case AddressableAssetSettings.ModificationEvent.EntryCreated:
                case AddressableAssetSettings.ModificationEvent.EntryModified:
                case AddressableAssetSettings.ModificationEvent.EntryMoved:
                case AddressableAssetSettings.ModificationEvent.BatchModification:
                    ProcessEntryData(data);
                    break;
            }
        }

        private static void ProcessEntryData(object data)
        {
            if (data == null) return;

            if (data is AddressableAssetEntry entry)
            {
                AddressableMasterDataEditorUtility.ProcessEntry(entry, force: false, logIfMissing: true);
                return;
            }

            if (data is AddressableAssetEntry[] entryArray)
            {
                for (var i = 0; i < entryArray.Length; i++)
                {
                    AddressableMasterDataEditorUtility.ProcessEntry(entryArray[i], force: false, logIfMissing: true);
                }

                return;
            }

            if (data is IList<AddressableAssetEntry> entryList)
            {
                for (var i = 0; i < entryList.Count; i++)
                {
                    AddressableMasterDataEditorUtility.ProcessEntry(entryList[i], force: false, logIfMissing: true);
                }

                return;
            }

            if (data is IEnumerable<AddressableAssetEntry> entryEnumerable)
            {
                foreach (var entryItem in entryEnumerable)
                {
                    AddressableMasterDataEditorUtility.ProcessEntry(entryItem, force: false, logIfMissing: true);
                }

                return;
            }

            if (data is string guid)
            {
                ProcessGuid(guid);
                return;
            }

            if (data is string[] guidArray)
            {
                for (var i = 0; i < guidArray.Length; i++)
                {
                    ProcessGuid(guidArray[i]);
                }

                return;
            }

            if (data is IList<string> guidList)
            {
                for (var i = 0; i < guidList.Count; i++)
                {
                    ProcessGuid(guidList[i]);
                }

                return;
            }

            if (data is IEnumerable<string> guidEnumerable)
            {
                foreach (var guidItem in guidEnumerable)
                {
                    ProcessGuid(guidItem);
                }
            }
        }

        private static void ProcessGuid(string guid)
        {
            if (string.IsNullOrEmpty(guid)) return;

            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path)) return;

            AddressableMasterDataEditorUtility.ProcessAssetsAtPath(path, force: false, logIfMissing: true);
        }
    }
}
