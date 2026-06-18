using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.Bootstrap
{
    /// <summary>
    /// 正規ルート外(Editor直起動等)で起動された際の補完初期化処理
    /// PJ側で実装する
    /// </summary>
    public interface IDirectBootstrapper
    {
        UniTask BootstrapAsync(CancellationToken ct);
    }
}
