using System;

namespace TosCore.Phase
{
    /// <summary>フェーズ状態の管理者</summary>
    public interface IPhaseManager<TPhase> where TPhase : Enum
    {
        /// <summary>現在のフェーズ</summary>
        TPhase CurrentPhase { get; }

        /// <summary>次のフェーズへ移行する</summary>
        void SetNextPhase(TPhase phase);
    }
}
