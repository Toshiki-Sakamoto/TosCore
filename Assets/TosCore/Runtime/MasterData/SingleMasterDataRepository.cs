using System;

namespace TosCore.MasterData
{
    /// <summary>
    /// 単一マスタデータ用リポジトリ基底クラス
    /// </summary>
    public abstract class SingleMasterDataRepository<TMaster> : ISingleMasterDataRepository<TMaster>
        where TMaster : IMasterData<TMaster>
    {
        private TMaster _master;
        private bool _hasValue;

        public TMaster GetValue()
        {
            if (!_hasValue)
            {
                throw new InvalidOperationException(
                    $"SingleMasterDataRepository<{typeof(TMaster).Name}> has no registered master.");
            }

            return _master;
        }

        internal void Replace(TMaster master)
        {
            ClearCore();
            _master = master;
            _hasValue = true;
            RegisterCore(master);
        }

        internal void Clear()
        {
            ClearCore();
            _master = default;
            _hasValue = false;
        }

        protected virtual void RegisterCore(TMaster master) { }
        protected virtual void ClearCore() { }
    }
}
