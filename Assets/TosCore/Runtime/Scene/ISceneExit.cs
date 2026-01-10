using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.Scene
{
    /// <summary>
    /// シーン終了時にクリーンアップを行うためのインターフェース。
    /// </summary>
    public interface ISceneExitable
    {
        UniTask ExitAsync(CancellationToken ct);
    }
}
