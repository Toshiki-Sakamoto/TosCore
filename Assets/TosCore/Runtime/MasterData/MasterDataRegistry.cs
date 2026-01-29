using System.Collections.Generic;

namespace TosCore.MasterData
{
    public abstract class MasterDataRegistry<TMaster> : IMasterDataRegistry<TMaster> 
        where TMaster : IMasterData<TMaster>
    {
        protected readonly Dictionary<MasterId<TMaster>, TMaster> _masterMap = new ();
        
        public void Register(TMaster master)
        {
            _masterMap[master.Id] = master;
            RegisterCore(master);
        }

        public void Clear()
        {
            _masterMap.Clear();
            ClearCore();
        }

        public bool TryGetValue(MasterId<TMaster> id, out TMaster master)
        {
            return _masterMap.TryGetValue(id, out master);
        }

        public IEnumerable<TMaster> GetAllMasters()
        {
            return _masterMap.Values;
        }

        protected virtual void RegisterCore(TMaster master) {}
        protected virtual void ClearCore() { }
    }
}