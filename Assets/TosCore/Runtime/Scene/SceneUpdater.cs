using System;
using System.Collections.Generic;

namespace TosCore.Scene
{
    public interface ISceneUpdater
    {
        void Initialize(IEnumerable<ISceneTickable> tickables);
        void Tick(float deltaTime);
        bool HasTickables { get; }
    }

    public sealed class SceneUpdater : ISceneUpdater
    {
        private IReadOnlyList<ISceneTickable> _tickables = Array.Empty<ISceneTickable>();

        public bool HasTickables => _tickables.Count > 0;

        public void Initialize(IEnumerable<ISceneTickable> tickables)
        {
            _tickables = LifecycleOrdering.OrderByPriority(tickables);
        }

        public void Tick(float deltaTime)
        {
            if (!HasTickables)
            {
                return;
            }

            foreach (var tickable in _tickables)
            {
                tickable.Tick(deltaTime);
            }
        }
    }
}
