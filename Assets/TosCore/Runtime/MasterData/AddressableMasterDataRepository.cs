using System.Collections.Generic;
using UnityEngine;

namespace TosCore.MasterData
{
    /// <summary>
    /// Addressableを利用したマスタデータ管理リポジトリクラス
    /// </summary>
    public abstract class AddressableMasterDataRepository<TMaster> : MasterDataRepository<TMaster>, IAddressableMasterDataLoadTarget
        where TMaster : IAddressableMasterData<TMaster>
    {
        public abstract string AddressableKey { get; }

        void IAddressableMasterDataLoadTarget.ReplaceAll(IList<ScriptableObject> masters)
        {
            var typedMasters = new List<TMaster>(masters?.Count ?? 0);
            if (masters == null)
            {
                ReplaceAll(typedMasters);
                return;
            }

            foreach (var master in masters)
            {
                if (master is TMaster typed)
                {
                    typedMasters.Add(typed);
                }
                else
                {
                    Debug.LogWarning($"Addressable master type mismatch. Expected {typeof(TMaster).Name}, got {master.GetType().Name}.");
                }
            }

            ReplaceAll(typedMasters);
        }
    }
}
