using System.Collections.Generic;
using UnityEngine;

namespace TosCore.MasterData
{
    /// <summary>
    /// Addressableロード結果の差し替え先
    /// </summary>
    public interface IAddressableMasterDataLoadTarget
    {
        string AddressableKey { get; }

        void ReplaceAll(IList<ScriptableObject> masters);
    }
}
