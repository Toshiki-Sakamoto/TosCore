using System.Collections.Generic;
using UnityEngine;

namespace TosCore.MasterData
{
    public interface IAddressableMasterDataRegistry
    {
        string AddressableKey { get; }

        void Register(IList<ScriptableObject> masters);

        void Clear();
    }
}
