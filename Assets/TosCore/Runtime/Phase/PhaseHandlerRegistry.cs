using System;
using System.Collections.Generic;
using System.Linq;

namespace TosCore.Phase
{
    /// <summary>
    /// フェーズ値と <see cref="IPhaseHandler{TPhase}"/> の対応表を保持する
    /// シーン初期化時に DI で集めたハンドラを <see cref="Set"/> で登録し、PhaseManager が遷移時に引く
    /// </summary>
    public sealed class PhaseHandlerRegistry<TPhase> : IPhaseHandlerRegistry<TPhase> where TPhase : Enum
    {
        private Dictionary<TPhase, IPhaseHandler<TPhase>> _handlers = new();

        public void Set(IEnumerable<IPhaseHandler<TPhase>> handlers)
        {
            _handlers = handlers.ToDictionary(x => x.Phase, x => x);
        }

        public bool TryGetValue(TPhase phase, out IPhaseHandler<TPhase> handler)
        {
            return _handlers.TryGetValue(phase, out handler);
        }
    }
}
