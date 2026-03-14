using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.Entity
{
    /// <summary>
    /// リポジトリ状態のロード・保存契約
    /// </summary>
    public interface IEntityRepositoryStorage
    {
        UniTask LoadAsync(CancellationToken cancellationToken = default);

        UniTask SaveAsync(CancellationToken cancellationToken = default);
    }
}
