using System.Collections.Generic;

namespace TosCore.Entity
{
    /// <summary>
    /// 採番状態の保存管理
    /// </summary>
    public interface IIdSequenceStateStore
    {
        IReadOnlyDictionary<TypeToken, long> GetStates();
        
        void SetStates(IReadOnlyDictionary<TypeToken, long> state);
    }
}
