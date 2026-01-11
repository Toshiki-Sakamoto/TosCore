using System.Collections.Generic;
using UnityEngine;

namespace TosCore.MasterData
{
    /// <summary>
    /// Addressableを利用したマスタデータ管理レジストリクラス
    /// </summary>
    public abstract class AddressableMasterDataRegistry<TMaster> : MasterDataRegistry<TMaster>, IAddressableMasterDataRegistry
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
                }
                else
                {
                    Debug.LogWarning($"Addressable master type mismatch. Expected {typeof(TMaster).Name}, got {master.GetType().Name}.");
                }
            }
        }
    }
}
