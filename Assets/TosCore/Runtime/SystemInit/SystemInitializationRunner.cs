using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TosCore.Scene;

namespace TosCore.SystemInit
{
    /// <summary>
    /// 登録された <see cref="ISystemInitializer"/> を優先度順に一度だけ実行する
    /// 複数のシーン（や先行キック）から <see cref="InitializedOneShotAsync"/> が呼ばれても、実体の初期化は一度だけ走る
    /// </summary>
    public sealed class SystemInitializationRunner : ISystemInitializationRunner
    {
        private readonly IReadOnlyList<ISystemInitializer> _initializers;
        private UniTask? _running;

        public SystemInitializationRunner(IEnumerable<ISystemInitializer> initializers)
        {
            _initializers = LifecycleOrdering.OrderByPriority(initializers);
        }

        public bool IsInitialized { get; private set; }

        public UniTask InitializedOneShotAsync(CancellationToken ct = default)
        {
            _running ??= RunAllAsync(ct).Preserve();
            return _running.Value;
        }

        private async UniTask RunAllAsync(CancellationToken ct)
        {
            foreach (var initializer in _initializers)
            {
                ct.ThrowIfCancellationRequested();
                await initializer.InitializeAsync(ct);
            }

            IsInitialized = true;
        }
    }
}
