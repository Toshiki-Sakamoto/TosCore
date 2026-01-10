using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.Scene
{
    /// <summary>
    /// シーンロード直後の初期化処理を担当するコンポーネントが実装するインターフェース。
    /// </summary>
    public interface ISceneInitializable
    {
        UniTask InitializeAsync(CancellationToken ct);
    }
}
