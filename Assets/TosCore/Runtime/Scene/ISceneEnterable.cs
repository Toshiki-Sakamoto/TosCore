using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.Scene
{
    /// <summary>
    /// 初期化後にシーンがアクティブ化されるタイミングで呼び出される初期動作を提供。
    /// </summary>
    public interface ISceneEnterable
    {
        UniTask EnterAsync(CancellationToken ct);
    }
}
