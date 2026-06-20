using System;
using System.Collections.Generic;

namespace TosCore.Phase
{
    /// <summary>フェーズ値とハンドラの対応表</summary>
    public interface IPhaseHandlerRegistry<TPhase> where TPhase : Enum
    {
        /// <summary>ハンドラ一式を登録する</summary>
        void Set(IEnumerable<IPhaseHandler<TPhase>> handlers);

        /// <summary>フェーズに対応するハンドラを取得する</summary>
        bool TryGetValue(TPhase phase, out IPhaseHandler<TPhase> handler);
    }
}
