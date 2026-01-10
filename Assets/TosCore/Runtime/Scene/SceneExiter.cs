using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.Scene
{
    public interface ISceneExiter
    {
        void Initialize(IEnumerable<ISceneExitable> exitables);
        UniTask RunAsync(CancellationToken token);
        bool HasExitables { get; }
    }

    public sealed class SceneExiter : ISceneExiter
    {
        private IReadOnlyList<ISceneExitable> _exitables = Array.Empty<ISceneExitable>();

        public bool HasExitables => _exitables.Count > 0;

        public void Initialize(IEnumerable<ISceneExitable> exitables)
        {
            _exitables = LifecycleOrdering.OrderByPriority(exitables);
        }

        public async UniTask RunAsync(CancellationToken token)
        {
            if (!HasExitables)
            {
                return;
            }

            foreach (var exitable in _exitables)
            {
                token.ThrowIfCancellationRequested();
                await exitable.ExitAsync(token);
            }
        }
    }
}
