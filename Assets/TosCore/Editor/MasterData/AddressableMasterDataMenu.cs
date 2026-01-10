using UnityEditor;
using UnityEngine;

namespace TosCore.Editor.MasterData
{
    public static class AddressableMasterDataMenu
    {
        private const string MenuPath = "TosCore/MasterData/Regenerate Addressable Id";

        [MenuItem(MenuPath, true)]
        private static bool RegenerateValidate()
        {
            var selection = Selection.objects;
            foreach (var target in selection)
            {
                if (target == null) continue;

                if (AddressableMasterDataEditorUtility.IsAddressableMasterDataType(target.GetType()))
                {
                    return true;
                }
            }

            return false;
        }

        [MenuItem(MenuPath)]
        private static void Regenerate()
        {
            var selection = Selection.objects;
            foreach (var t in selection)
            {
                AddressableMasterDataEditorUtility.TryAssignId(t, force: true, logIfMissing: true);
            }
        }
    }
}
