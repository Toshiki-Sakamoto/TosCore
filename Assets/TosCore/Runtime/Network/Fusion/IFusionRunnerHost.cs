using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;

namespace TosCore.Network.Fusion
{
    /// <summary>
    /// Fusion の <see cref="NetworkRunner"/> のライフサイクル（生成・参照・破棄）を提供する
    /// </summary>
    public interface IFusionRunnerHost
    {
        /// <summary>生成済みの Runner。未生成なら null</summary>
        NetworkRunner Runner { get; }

        /// <summary>Runner を生成（既に在ればそれを返す）。入力提供を有効にする</summary>
        NetworkRunner CreateRunner();

        /// <summary>Runner を安全にシャットダウンして破棄する</summary>
        UniTask ShutdownRunnerAsync(CancellationToken ct);
    }
}
