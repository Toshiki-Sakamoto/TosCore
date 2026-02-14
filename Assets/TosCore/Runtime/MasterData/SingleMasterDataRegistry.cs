using System;
using UnityEngine;

namespace TosCore.MasterData
{
    /// <summary>
    /// 単一マスタデータ用レジストリ基底クラス
    /// </summary>
    public abstract class SingleMasterDataRegistry<TMaster> : ISingleMasterDataRegistry<TMaster>
        where TMaster : IMasterData<TMaster>
    {
        private TMaster _master;
        private bool _hasValue;

        public void Register(TMaster master)
        {
            if (_hasValue)
            {
                Debug.LogWarning($"SingleMasterDataRegistry<{typeof(TMaster).Name}> already has a value. Overwriting.");
            }

            _master = master;
            _hasValue = true;
            RegisterCore(master);
        }

        public void Clear()
        {
            _master = default;
            _hasValue = false;
            ClearCore();
        }

        public TMaster GetValue()
        {
            if (!_hasValue)
            {
                throw new InvalidOperationException(
                    $"SingleMasterDataRegistry<{typeof(TMaster).Name}> has no registered master.");
            }

            return _master;
        }

        protected virtual void RegisterCore(TMaster master) { }
        protected virtual void ClearCore() { }
    }
}
