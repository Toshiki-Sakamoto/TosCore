using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;

namespace TosCore.Network.Fusion
{
    /// <summary>
    /// Fusion セッションの起動骨組み（Runner 生成・コールバック登録・StartGame）
    /// </summary>
    public interface INetworkGameLauncher
    {
        /// <summary>Runner を生成・起動する。コールバック登録を済ませてから StartGame する</summary>
        UniTask<StartGameResult> StartGameAsync(GameMode mode, string sessionName, CancellationToken ct = default);

        /// <summary>起動を fire-and-forget で実行する</summary>
        void RunForeground();
    }
}
