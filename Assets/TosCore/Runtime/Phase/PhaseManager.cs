using System;
using MessagePipe;
using TosCore.Scene;

namespace TosCore.Phase
{
    /// <summary>
    /// フェーズ遷移を駆動する汎用マネージャ
    /// SetNextPhase で 前ハンドラ OnExit → 次ハンドラ OnEnter、毎 Tick で現在ハンドラ OnUpdate を呼ぶ
    /// 遷移時に <see cref="PhaseChangedMessage{TPhase}"/> を publish する
    /// </summary>
    public sealed class PhaseManager<TPhase> : IPhaseManager<TPhase>, ISceneTickable where TPhase : Enum
    {
        private readonly IPhaseHandlerRegistry<TPhase> _registry;
        private readonly IPublisher<PhaseChangedMessage<TPhase>> _publisher;
        private IPhaseHandler<TPhase> _current;

        public TPhase CurrentPhase { get; private set; }

        public PhaseManager(
            IPhaseHandlerRegistry<TPhase> registry,
            IPublisher<PhaseChangedMessage<TPhase>> publisher)
        {
            _registry = registry;
            _publisher = publisher;
        }

        public void SetNextPhase(TPhase phase)
        {
            _current?.OnExit();
            CurrentPhase = phase;
            _current = _registry.TryGetValue(phase, out var handler) ? handler : null;
            _current?.OnEnter();
            _publisher.Publish(new PhaseChangedMessage<TPhase>(phase));
        }

        public void Tick(float deltaTime) => _current?.OnUpdate(deltaTime);
    }
}
