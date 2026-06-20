using System;

namespace TosCore.Phase
{
    /// <summary>1つのフェーズ状態のコールバック受付</summary>
    public interface IPhaseHandler<TPhase> where TPhase : Enum
    {
        /// <summary>このハンドラが担当するフェーズ</summary>
        TPhase Phase { get; }

        /// <summary>フェーズ遷移時に1度だけ呼ばれる</summary>
        void OnEnter();

        /// <summary>フェーズ中、毎フレーム呼ばれる</summary>
        void OnUpdate(float deltaTime);

        /// <summary>フェーズ終了時に1度だけ呼ばれる</summary>
        void OnExit();
    }
}
