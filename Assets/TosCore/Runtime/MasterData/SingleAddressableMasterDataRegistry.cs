using System.Collections.Generic;
using UnityEngine;

namespace TosCore.MasterData
{
    /// <summary>
    /// Addressableを利用した単一マスタデータ管理レジストリクラス
    /// </summary>
    public abstract class SingleAddressableMasterDataRegistry<TMaster> : SingleMasterDataRegistry<TMaster>, IAddressableMasterDataRegistry
        where TMaster : AddressableMasterData<TMaster>
    {
        public abstract string AddressableKey { get; }

        public void Register(IList<ScriptableObject> masters)
        {
            foreach (var master in masters)
            {
                if (master is TMaster typed)
                {
                    Register(typed);
                    return;
                }
            }

            Debug.LogWarning($"SingleAddressableMasterDataRegistry<{typeof(TMaster).Name}>: No matching master found in loaded assets.");
        }
    }
}
