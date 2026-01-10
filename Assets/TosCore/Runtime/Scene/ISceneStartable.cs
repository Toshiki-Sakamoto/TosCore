using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.Scene
{
    public interface ISceneStartable
    {
        UniTask StartAsync(CancellationToken ct);
    }
}
