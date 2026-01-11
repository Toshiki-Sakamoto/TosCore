using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.MasterData
{
    public interface IMasterDataLoader
    {
        UniTask LoadAsync(CancellationToken cancellationToken);
    }
}
