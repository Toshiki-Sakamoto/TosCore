using System;
using VContainer;

namespace TosCore.Phase
{
    /// <summary>フェーズハンドラの基底。manager を inject し、自分で次フェーズへ進められる</summary>
    public abstract class PhaseHandlerBase<TPhase> : IPhaseHandler<TPhase> where TPhase : Enum
    {
        [Inject] protected IPhaseManager<TPhase> _phaseManager;

        public abstract TPhase Phase { get; }

        public virtual void OnEnter() { }
        public virtual void OnUpdate(float deltaTime) { }
        public virtual void OnExit() { }
    }
}
