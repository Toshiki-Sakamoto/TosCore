using System.Collections.Generic;

namespace TosCore.MasterData
{
    public abstract class MasterDataRepository<TMaster> : IMasterDataRepository<TMaster>
        where TMaster : IMasterData<TMaster>
    {
        protected readonly Dictionary<MasterId<TMaster>, TMaster> _masterMap = new();

        public bool TryGetValue(MasterId<TMaster> id, out TMaster master)
        {
            return _masterMap.TryGetValue(id, out master);
        }

        public IEnumerable<TMaster> GetAllMasters()
        {
            return _masterMap.Values;
        }

        internal void ReplaceAll(IEnumerable<TMaster> masters)
        {
            _masterMap.Clear();
            ClearCore();

            if (masters == null) return;

            foreach (var master in masters)
            {
                if (ReferenceEquals(master, null)) continue;

                _masterMap[master.Id] = master;
                RegisterCore(master);
            }
        }

        protected virtual void RegisterCore(TMaster master) {}
        protected virtual void ClearCore() { }
    }
}
