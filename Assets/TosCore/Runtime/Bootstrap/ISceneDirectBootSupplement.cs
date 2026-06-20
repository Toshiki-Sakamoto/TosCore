using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.Bootstrap
{
    /// <summary>
    /// シーン直起動で前提が欠けていたとき、それを補完する処理。
    /// PJ が実装し、SceneLifetimeScope のスコープに登録する（複数可）。
    /// </summary>
    public interface ISceneDirectBootSupplement
    {
        /// <summary>不足している前提の補完を実行する。</summary>
        UniTask RunAsync(CancellationToken ct);
    }
}
