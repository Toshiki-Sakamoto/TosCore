using System.Threading;
using Cysharp.Threading.Tasks;
using TosCore.Scene;

namespace TosCore.SystemInit
{
    /// <summary>
    /// 各シーンの初期化フェーズ冒頭（最小 Priority）でシステム初期化の完了を待つゲート
    /// これにより、どのシーンも「システム初期化が終わってから」自身の <see cref="ISceneInitializable"/> が走る
    /// <see cref="SceneLifetimeScope"/> が全シーンへ自動登録するため、PJ 側での登録は不要
    /// </summary>
    public sealed class SystemReadyGate : ISceneInitializable, ILifecyclePrioritized
    {
        private readonly ISystemInitializationRunner _runner;

        public SystemReadyGate(ISystemInitializationRunner runner)
        {
            _runner = runner;
        }

        // どのシーンでも最初に走らせ、後続の初期化がシステム初期化済みを前提にできるようにする
        public int Priority => int.MinValue;

        public UniTask InitializeAsync(CancellationToken ct) => 
            _runner.InitializedOneShotAsync(ct);
    }
}
