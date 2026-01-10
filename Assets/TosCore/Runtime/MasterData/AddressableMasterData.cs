using System;
using UnityEngine;

namespace TosCore.MasterData
{
    /// <summary>
    /// Addressable に登録されたアセット GUID から安定した数値 ID を生成する基底クラス。
    /// </summary>
    public abstract class AddressableMasterData<TMaster> : ScriptableObject, IMasterData<TMaster>
        where TMaster : IMasterData<TMaster>
    {
        [SerializeField]
        private MasterId<TMaster> _id;

        public MasterId<TMaster> Id => _id;

        
        public bool Equals(IMasterData<TMaster> other) => 
            other != null && Id.Equals(other.Id);
    }
}
