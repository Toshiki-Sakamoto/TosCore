using System.Collections.Generic;
using UnityEngine;

namespace TosCore.MasterData
{
    /// <summary>
    /// Addressableを利用した単一マスタデータ管理リポジトリクラス
    /// </summary>
    public abstract class SingleAddressableMasterDataRepository<TMaster> : SingleMasterDataRepository<TMaster>, IAddressableMasterDataLoadTarget
        where TMaster : IAddressableMasterData<TMaster>
    {
        public abstract string AddressableKey { get; }

        void IAddressableMasterDataLoadTarget.ReplaceAll(IList<ScriptableObject> masters)
        {
            if (masters == null)
            {
                Clear();
                return;
            }

            foreach (var master in masters)
            {
                if (master is TMaster typed)
                {
                    Replace(typed);
                    return;
                }
            }

            Clear();
            Debug.LogWarning($"SingleAddressableMasterDataRepository<{typeof(TMaster).Name}>: No matching master found in loaded assets.");
        }
    }
}
