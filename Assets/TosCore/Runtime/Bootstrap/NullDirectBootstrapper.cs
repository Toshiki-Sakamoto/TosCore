using System.Threading;
using Cysharp.Threading.Tasks;

namespace TosCore.Bootstrap
{
    /// <summary>
    /// IDirectBootstrapper のデフォルト空実装
    /// PJ側で上書き登録されなかった場合に使用される
    /// </summary>
    public sealed class NullDirectBootstrapper : IDirectBootstrapper
    {
        public UniTask BootstrapAsync(CancellationToken ct) => UniTask.CompletedTask;
    }
}
