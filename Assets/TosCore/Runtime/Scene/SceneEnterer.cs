using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.Scene
{
    public interface ISceneEnterer
    {
        void Initialize(IEnumerable<ISceneEnterable> enterables);
        UniTask RunAsync(CancellationToken token);
        bool HasEnterables { get; }
    }

    public sealed class SceneEnterer : ISceneEnterer
    {
        private IReadOnlyList<ISceneEnterable> _enterables = Array.Empty<ISceneEnterable>();

        public bool HasEnterables => _enterables.Count > 0;

        public void Initialize(IEnumerable<ISceneEnterable> enterables)
        {
            _enterables = LifecycleOrdering.OrderByPriority(enterables);
        }

        public async UniTask RunAsync(CancellationToken token)
        {
            if (!HasEnterables)
            {
                return;
            }

            foreach (var enterable in _enterables)
            {
                token.ThrowIfCancellationRequested();
                await enterable.EnterAsync(token);
            }
        }
    }
}
