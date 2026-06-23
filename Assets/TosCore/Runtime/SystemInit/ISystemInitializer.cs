using System.Threading;
using Cysharp.Threading.Tasks;
using TosCore.Scene;

namespace TosCore.SystemInit
{
    /// <summary>
    /// アプリ起動時に一度だけ走るシステム初期化の単位
    /// 各シーン毎に走る <see cref="ISceneInitializable"/> と異なり、<see cref="ISystemInitializationRunner"/> によりアプリ全体で一度だけ実行される
    /// 実行順は <see cref="ILifecyclePrioritized.Priority"/>（小さいほど先）で制御する
    /// </summary>
    public interface ISystemInitializer : ILifecyclePrioritized
    {
        UniTask InitializeAsync(CancellationToken ct);
    }
}
