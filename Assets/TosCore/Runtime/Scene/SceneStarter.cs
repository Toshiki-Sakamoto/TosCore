using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.Scene
{
    public interface ISceneStarter
    {
        void Initialize(IEnumerable<ISceneStartable> startables);
        UniTask RunAsync(CancellationToken token);
        bool HasStartables { get; }
    }

    public sealed class SceneStarter : ISceneStarter
    {
        private IReadOnlyList<ISceneStartable> _startables = Array.Empty<ISceneStartable>();

        public bool HasStartables => _startables.Count > 0;

        public void Initialize(IEnumerable<ISceneStartable> startables)
        {
            _startables = LifecycleOrdering.OrderByPriority(startables);
        }

        public async UniTask RunAsync(CancellationToken token)
        {
            if (!HasStartables) return;

            foreach (var startable in _startables)
            {
                token.ThrowIfCancellationRequested();
                await startable.StartAsync(token);
            }
        }
    }
}
