using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.Scene
{
    public interface ISceneInitializer
    {
        void Initialize(IEnumerable<ISceneInitializable> initializables);
        UniTask RunAsync(CancellationToken token);
        bool HasInitializers { get; }
    }

    public sealed class SceneInitializer : ISceneInitializer
    {
        private IReadOnlyList<ISceneInitializable> _initializables = Array.Empty<ISceneInitializable>();

        public bool HasInitializers => _initializables.Count > 0;

        public void Initialize(IEnumerable<ISceneInitializable> initializables)
        {
            _initializables = LifecycleOrdering.OrderByPriority(initializables);
        }

        public async UniTask RunAsync(CancellationToken token)
        {
            if (!HasInitializers) return;

            foreach (var initializable in _initializables)
            {
                token.ThrowIfCancellationRequested();
                await initializable.InitializeAsync(token);
            }
        }
    }
}
